using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KeyboardsLibrary.Core.Entity;
using MongoDB.Driver;

namespace KeyboardsLibrary.Core
{
    public class KeyboardServices : IKeyboardServices
    {
        private readonly IMongoCollection<EbayKeyboard> _keyboards;
        public KeyboardServices(IDbClient dbClient)
        {
            _keyboards = dbClient.GetKeyboardCollection();
        }
        public List<EbayKeyboard> GetKeyboardsEbay()
        {
            return _keyboards.Find(keyboard => true).ToList();
        }

        public EbayKeyboard AddKeyboard(EbayKeyboard kb)
        {
            _keyboards.InsertOne(kb);
            return kb;
        }

        public EbayKeyboard GetKeyboard(string id)
        {
            return _keyboards.Find(kb => kb.Id == id).First();
        }

        public EbayKeyboard GetKeyboardEbayId(string id)
        {
            return _keyboards.Find(kb => kb.EbayId == id).First();
        }

        public void DeleteKeyboard(string id)
        {
            _keyboards.DeleteOne(kb => kb.Id == id);
        }

        public void DeleteKeyboardEbayId(string id)
        {
            _keyboards.DeleteOne(kb => kb.EbayId == id);
        }

        public EbayKeyboard UpdateKeyboard(EbayKeyboard kb)
        {
            GetKeyboardEbayId(kb.EbayId);
            _keyboards.ReplaceOne(kbs => kbs.EbayId == kb.EbayId, kb);
            return kb;
        }

        public List<EbayKeyboard> StoreAndUpdate(List<EbayKeyboard> KbList)
        {
            GetHtmlAsync();
            foreach (var kb in KbList)
            {
                EbayKeyboard compare = _keyboards.Find(keyb => keyb.EbayId == kb.EbayId).FirstOrDefault();

                if (compare == null)
                {
                    Console.WriteLine("DOENST EXIST SO POST");
                    Console.WriteLine(kb.Name);
                    _keyboards.InsertOne(kb);
                }
                else // update since compare was found
                {
                    Console.WriteLine("UPDATE");
                    Console.WriteLine(compare.Name);
                    Console.WriteLine(kb.Name);
                    Console.WriteLine(compare.EbayId);
                    Console.WriteLine(kb.EbayId);
                    kb.Id = compare.Id;
                    _keyboards.ReplaceOne(kbs => kbs.EbayId == kb.EbayId, kb);
                }
            }

            return KbList;
        }

        public async Task<List<EbayKeyboard>> GetScrapedData()
        {
            //var url = "https://www.ebay.ca/sch/i.html?_fosrp=1&_from=R40&_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=L4J0A1&_sargn=-1%26saslc%3D1&_salic=2&_sop=13&_dmd=1&_ipg=200";

            var url =
                "https://www.ebay.ca/sch/i.html?_fosrp=1&_from=R40&_nkw=gaming+keyboard&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=L4J0A1&_sargn=-1%26saslc%3D1&_salic=2&_sop=13&_dmd=1&_ipg=200";
            
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            
            // now loaded for parsing
            htmlDocument.LoadHtml(html);
            
            Thread.Sleep(3000);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("id", "")
                    .Equals("ListViewInner")).ToList(); 

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                    .Contains("item")).ToList();


            List <EbayKeyboard> NewList = new List<EbayKeyboard>();
            
            foreach (var ProductListItem in ProductListItems)
            {
                EbayKeyboard NewKb = new EbayKeyboard();
                
                // id
                Console.WriteLine(ProductListItem.GetAttributeValue("listingid", ""));
                
                var id = ProductListItem.GetAttributeValue("listingid", "");
                NewKb.EbayId = id;
                
                // product name
                Console.WriteLine(ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    .Replace("New listing", "").TrimStart());

                var name = ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    .Replace("New listing", "").TrimStart();
                NewKb.Name = name;
                
                // price
                Console.WriteLine(
                    Regex.Match(
                        ProductListItem.Descendants("li")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                        .Replace("New listing", "").Trim('C').TrimStart()
                    ,@"\d+.\d+"
                    )
                );

                var price = Regex.Match(
                    ProductListItem.Descendants("li")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                        .Replace("New listing", "").Trim('C').TrimStart()
                    , @"\d+.\d+"
                );
                NewKb.Price = Double.Parse(price.Value);
                
                // url
                Console.WriteLine(
                    ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "")
                );

                var link = ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "");
                NewKb.Link = link;
                
                // listing type
                Console.WriteLine(
                    ProductListItem.Descendants("li")
                            .Where(node => node.GetAttributeValue("class", "")
                                .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t').TrimEnd().Replace("\t", "")
                );

                var standing = ProductListItem.Descendants("li")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t').Replace("\t", "").Replace("\n", " ");
                NewKb.Standing = standing;
                
                // image
                Console.WriteLine(ProductListItem.Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", ""));

                var image = ProductListItem.Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", "");
                NewKb.Image = image;
                
                NewList.Add(NewKb);
                
                Console.WriteLine();
            }

            return NewList;
        }


        private static async void GetHtmlAsync()
        {
            //var url = "https://www.ebay.ca/sch/i.html?_fosrp=1&_from=R40&_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=L4J0A1&_sargn=-1%26saslc%3D1&_salic=2&_sop=13&_dmd=1&_ipg=200";

            var url =
                "https://www.ebay.ca/sch/i.html?_fosrp=1&_from=R40&_nkw=gaming+keyboard&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=L4J0A1&_sargn=-1%26saslc%3D1&_salic=2&_sop=13&_dmd=1&_ipg=200";
            
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            
            // now loaded for parsing
            htmlDocument.LoadHtml(html);
            
            Thread.Sleep(3000);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("id", "")
                    .Equals("ListViewInner")).ToList(); 

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                    .Contains("item")).ToList();
            
            

            foreach (var ProductListItem in ProductListItems)
            {
                // id
                Console.WriteLine(ProductListItem.GetAttributeValue("listingid", ""));
                
                // product name
                Console.WriteLine(ProductListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                        .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    .Replace("New listing", "").TrimStart());
                
                // price
                Console.WriteLine(
                    Regex.Match(
                        ProductListItem.Descendants("li")
                        .Where(node => node.GetAttributeValue("class", "")
                            .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                        .Replace("New listing", "").Trim('C').TrimStart()
                    ,@"\d+.\d+"
                    )
                );
                
                // url
                Console.WriteLine(
                    ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "")
                );
                
                // listing type
                Console.WriteLine(
                    ProductListItem.Descendants("li")
                            .Where(node => node.GetAttributeValue("class", "")
                                .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t').TrimEnd()
                );
                
                // image
                Console.WriteLine(ProductListItem.Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", ""));
                
                Console.WriteLine();
            }
            
        }
    }
}