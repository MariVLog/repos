using System.Collections.Generic;
using System.Net;
using System.ServiceModel;

namespace UapiPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            AirLowfareShop();
        }

        private static void AirLowfareShop()
        {
            #region MAKE WEB SERVICE CALL TO UAPI

            //THIS ALWAYS REMAINS AS IS - STANDARD
            var binding = new BasicHttpBinding
            {
                Name = "Mari",
                MaxReceivedMessageSize = 2097152
            };
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            #endregion

            //THIS ALWAYS REMAINS AS IS - STANDARD
            //This is mari's comment
            //THIS IS CHRISTO's COMMENT
            var credHeaders = new Dictionary<string, string> { { "Username", "Universal API/uAPI3454945120-33f61ac4" }, { "Password", "9i!N=Ft7Pd" } };

            var client = new SVCuapiAIR.AirLowFareSearchPortTypeClient(binding, new EndpointAddress("https://emea.universal-api.travelport.com/B2BGateway/connect/uAPI/AirService"));    //SERVER WE ARE CONNECTING TO


            if (client.ClientCredentials != null)
            {
                client.ClientCredentials.UserName.UserName = "Universal API/uAPI3454945120-33f61ac4";
                client.ClientCredentials.UserName.Password = "9i!N=Ft7Pd";
            }

            client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(credHeaders));
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(client.Endpoint.Address.Uri, null);
            servicePoint.Expect100Continue = false;

            client.Open();
            client.service(null, new SVCuapiAIR.LowFareSearchReq
            {
                BillingPointOfSaleInfo = new SVCuapiAIR.BillingPointOfSaleInfo
                {
                    OriginApplication = "UAPI"
                },
                TargetBranch = "P2771968",
                Items = new SVCuapiAIR.SearchAirLeg[1]
                {
                    new SVCuapiAIR.SearchAirLeg
                    {
                        SearchOrigin = new SVCuapiAIR.typeSearchLocation[1]
                        {
                            new SVCuapiAIR.typeSearchLocation
                            {
                                Item = new SVCuapiAIR.Airport
                                {
                                    Code = "JNB"
                                }
                            }
                        },
                        SearchDestination = new SVCuapiAIR.typeSearchLocation[1]
                        {
                            new SVCuapiAIR.typeSearchLocation
                            {
                               Item = new SVCuapiAIR.Airport
                               {
                                   Code = "CPT"
                               }
                            }
                        },
                        Items = new SVCuapiAIR.typeFlexibleTimeSpec[1]
                        {
                            new SVCuapiAIR.typeFlexibleTimeSpec
                            {
                                PreferredTime = "2019-10-08",
                            }
                        }
                    }
                },
                AirSearchModifiers = new SVCuapiAIR.AirSearchModifiers
                {
                    PreferredProviders = new SVCuapiAIR.Provider[1]
                    {
                        new SVCuapiAIR.Provider
                        {
                            Code = "1G"        //CODE FOR GALLILEO, currently the only one we are using
                        }
                    }
                },
                SearchPassenger = new SVCuapiAIR.SearchPassenger[2]
                {
                    new SVCuapiAIR.SearchPassenger
                    {
                        Code = "ADT",
                        BookingTravelerRef = "asgewertjherth"
                    },
                    new SVCuapiAIR.SearchPassenger
                    {
                        Code = "ADT",
                        BookingTravelerRef = "tyjrtyrtertyrety"
                    }
                }
            });
            client.Close();
        }
    }
}