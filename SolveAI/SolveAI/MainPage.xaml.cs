

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
            ReadAPIKey();
            Status.Text = "Solving";
            AIResponse response = await OpenAIService.AICALL("Solve this: " + Entry.Text, APIKey);
            if (response != null)
            {
                ResponseText.Text = response.Text;
                Status.Text = "Finished!";
            }
            Running.IsRunning = false;
            Status.Text = "Ready";

        }
        catch { }
        



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

}

