using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolveAI
{
    internal class OpenAIService
    {

        public static async Task<AIResponse> AICALL(string prompt, string apiKey)
        {

            // API call
            var client = new HttpClient();
            prompt = prompt.Replace("\r", "").Replace("\n", " ").Replace("\"", " ");
            apiKey = apiKey.Replace("\r", "").Replace("\n", " ").Replace("\"", " ");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/completions"),
                Content = new StringContent("{\n   \"model\": \"text-davinci-003\", \"prompt\": \"" + $"{prompt}\",\n    \"max_tokens\": 2000,\n    \"temperature\": 0.5\n}}")
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Response Handling
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Returns the Response Deserialized
            dynamic json = JsonConvert.DeserializeObject(responseContent);
            //return json.choices[0].text;
            string code = " ";
            string name = "Default Text";
            string result = json.choices[0].text;

            return new AIResponse(code, name, result);
        }



    }
}
