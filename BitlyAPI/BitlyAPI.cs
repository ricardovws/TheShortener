﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitlyAPI
{
    public class BitlyAPI
    {
        private string _bitlyApiUrl;
        private string _bitlyApiToken;


        public BitlyAPI()
        {
            _bitlyApiUrl = ConfigurationManager.AppSettings["BitlyApiUrl"];
            string fileJsonName = ConfigurationManager.AppSettings["BitlyApiTokenFileName"];
            var jsonObj = InsertApiTokenJsonFile(fileJsonName);
            _bitlyApiToken = jsonObj["ApiToken"];

        }

        public async Task<string> ShortenAsync(string longUrl)
        {
            return await Task.Run(() => Shorten(longUrl));
        }

        private string Shorten(string longUrl)
        {
            var errorMessage = "Sorry, but is not possible to short this URL!";

            if (CheckAccessToken())
            {
                using (HttpClient client = new HttpClient())
                {
                    string temp = string.Format(_bitlyApiUrl, _bitlyApiToken, WebUtility.UrlEncode(longUrl));
                    var response = client.GetAsync(temp).Result;

                    try
                    {
                        var message = response.Content.ReadAsStringAsync().Result;
                        dynamic obj = JsonConvert.DeserializeObject(message);
                        return obj.results[longUrl].shortUrl;
                    }
                    catch (Exception)
                    {
                        return errorMessage;
                    }
                }
            }
            return errorMessage;
        }

        private bool CheckAccessToken()
        {
            if (string.IsNullOrEmpty(_bitlyApiToken))
                return false;
            string temp = string.Format(_bitlyApiUrl, _bitlyApiToken, "google.com");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(temp).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public dynamic InsertApiTokenJsonFile(string fileJsonName)
        {
            string json = File.ReadAllText(fileJsonName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            return jsonObj;
        }
    }
}

