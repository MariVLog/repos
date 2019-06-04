namespace UapiPlayground
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Text;

    public class HttpHeaderMessageInspector : IClientMessageInspector
    {
        private readonly Dictionary<string, string> _httpHeaders;

        public HttpHeaderMessageInspector(Dictionary<string, string> httpHeaders)
        {
            _httpHeaders = httpHeaders;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            var userName = "";
            var passWord = "";

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                foreach (var httpHeader in _httpHeaders)
                {
                    if (string.Compare(httpHeader.Key, "Username", StringComparison.Ordinal) == 0)
                    {
                        userName = httpHeader.Value;
                    }
                    else if (string.Compare(httpHeader.Key, "Password", StringComparison.Ordinal) == 0)
                    {
                        passWord = httpHeader.Value;
                    }
                }
                if (httpRequestMessage != null)
                    httpRequestMessage.Headers[HttpRequestHeader.Authorization] = "Basic " +
                                                                                  Convert.ToBase64String(
                                                                                      Encoding.ASCII.GetBytes(userName +
                                                                                                              ":" +
                                                                                                              passWord));
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();

                foreach (var httpHeader in _httpHeaders)
                {
                    httpRequestMessage.Headers.Add(httpHeader.Key, httpHeader.Value);
                }
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }
            return null;
        }
    }

    internal class HttpHeadersEndpointBehavior : IEndpointBehavior
    {
        private readonly Dictionary<string, string> _httpHeaders;

        public HttpHeadersEndpointBehavior(Dictionary<string, string> httpHeaders)
        {
            _httpHeaders = httpHeaders;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new HttpHeaderMessageInspector(_httpHeaders);

            clientRuntime.ClientMessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}