

using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SolveAI;

public partial class MainPage : ContentPage
{

    public string APIKey;
	public MainPage()
	{
		InitializeComponent();
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        
        try
        {
            Running.IsRunning = true;
            Status.Text = "Reading Key";
            ResponseText.Text = "";
            ReadAPIKey();
            Status.Text = "Solving";
            await StreamChatCompletion("Solve this: " + Entry.Text, APIKey);

            Status.Text = "Finished!";
            
            Running.IsRunning = false;
            Status.Text = "Ready";

        }
        catch {
            Running.IsRunning = false;
            Status.Text = "API Fail";
            }
        



    }
    private void ReadAPIKey()
    {
        var path = FileSystem.Current.AppDataDirectory;
        var fullPath = Path.Combine(path, "APIKey.txt");
        if (File.Exists(fullPath))
        {
            APIKey = File.ReadAllText(fullPath);
        }
        else
        {
            DisplayAlert("API Key", "You need to save a valid API Key in your settings tab!", "Yessir");
        }
        
    }
    public async Task StreamChatCompletion(string message, string apiKey)
    {
        // API call
        var client = new HttpClient();
        apiKey = apiKey.Replace("\r", "").Replace("\n", " ").Replace("\"", " ");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-4-0613",
            messages = new[]
            {
            new { role = "user", content = message }
        },
            temperature = 0,
            max_tokens = 100,
            stream = true
        };

        var jsonRequest = JsonConvert.SerializeObject(requestBody);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.openai.com/v1/chat/completions"),
            Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
        };

        // Response Handling
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        using (var responseStream = await response.Content.ReadAsStreamAsync())
        using (var streamReader = new StreamReader(responseStream))
        {
            while (!streamReader.EndOfStream)
            {
                string chunk = await streamReader.ReadLineAsync();
                try {
                dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(chunk.Substring(6));

                string content = jsonResponse.choices[0].delta.content;
                ResponseText.Text += content;
                    }
                catch { }
            }
        }
    }


}

