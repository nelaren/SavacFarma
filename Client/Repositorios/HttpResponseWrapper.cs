using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SavacFarma.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {

        public HttpResponseWrapper(T response, bool error, HttpResponseMessage httpResponseMessage)
        {
            this.Response = response;
            this.Error = error;
            this.HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public async Task<string> GetBody()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
