using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace WiM.Utilities.ServiceAgent
{
    public abstract class ServiceAgentBase
    {
        #region "Events"

        #endregion

        #region Properties & Fields
        private HttpClient client;
        #endregion

        #region Constructors
        public ServiceAgentBase(string BaseUrl)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
        }
        #endregion

        #region Methods
        public async Task<T> ExecuteAsync<T>(RequestInfo request) where T : new()
        {
            try
            {
                HttpResponseMessage response=null;
                T result = default(T);
                switch (request.Method)
                {
                    case methodType.e_GET:
                        response = await client.GetAsync(request.RequestURI);
                        break;
                    case methodType.e_POST:
                        response = await client.PostAsync(request.RequestURI, request.Content);
                        break;
                    case methodType.e_PUT:
                        response = await client.PutAsync(request.RequestURI, request.Content);
                        break;
                    case methodType.e_DELETE:
                        response = await client.DeleteAsync(request.RequestURI);
                        break;
                }//end switch

                if (response == null) throw new Exception("http request invalid");

                response.EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(stringResult))
                    result = JsonConvert.DeserializeObject<T>(stringResult);

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }

        }//end ExecuteAsync<T>

        #endregion

    }//end class ServiceAgentBase

    public class RequestInfo
    {
        public methodType Method { get; set; }
        public string RequestURI { get; set; }
        public string DataType { get; set; }
        public HttpContent Content { get; set; }

        public RequestInfo(string uri, methodType mtd = methodType.e_GET) {
            this.RequestURI = uri;
            this.Method = mtd;            
        }
        public RequestInfo(string uri, Object data,contentType type = contentType.JSON,  methodType mtd = methodType.e_GET)
        {
            this.RequestURI = uri;
            this.Method = mtd;
            switch (type)
            {
                case contentType.JSON:
                    this.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, GetMediaType(type));
                    break;
                case contentType.XML:
                    break;
            }
            
        }
        private string GetMediaType(contentType type) {
            switch (type)
            {
                case contentType.JSON:
                    return "application/json";
                case contentType.XML:
                    return "application/xml";
            }

            return "application/json";
        }
    }
    public enum methodType
    {
        e_GET,
        e_POST,
        e_PUT,
        e_DELETE
    }
    public enum contentType
    {
        JSON,
        XML
    }
}
