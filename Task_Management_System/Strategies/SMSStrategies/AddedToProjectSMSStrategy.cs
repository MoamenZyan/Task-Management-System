using Project.API.Interfaces;
using Project.API.Utils;
using RestSharp;

namespace Project.API.Strategies.SMSStrategies
{
    public class AddedToProjectSMSStrategy(SMSSettings smsSettings) : INotificationStrategy
    {
        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
            try
            {
                var options = new RestClientOptions("https://dkwe28.api.infobip.com")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/sms/2/text/advanced", Method.Post);
                request.AddHeader("Authorization", smsSettings.APIKey);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                var data = $@"{{
                    ""messages"": [
                         {{
                              ""destinations"": [
                                    {{
                                        ""to"": ""+2{to}""
                                    }}
                              ],
                              ""from"": ""Task Management System"",
                              ""text"": ""{userName}, you have been added to project {body["ProjectName"]} as {body["UserRole"]}""
                        }}
                      ]
                    }}";
                request.AddStringBody(data, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.IsSuccessful);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
