using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace SolveAI;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}

    // User Clicks Save Button
    private async void Save_Clicked(object sender, EventArgs e)
    {
        var path = FileSystem.Current.AppDataDirectory;
        var fullPath = Path.Combine(path, "APIKey.txt");
        string apiKey = ApiKeyEntry.Text;
        if (apiKey == "" || apiKey == "API KEY HERE")
        {
            await DisplayAlert("Error", "Please enter a valid API Key", "OK");
            return;
        }
        else
        {
            using (var writer = new StreamWriter(fullPath))
            {
                writer.WriteLine(apiKey);
                writer.Close();
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            string text = "API Key Saved";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
    // User Clicks OpenAI Info Button
    private void OpenAiInfoButton_Clicked(object sender, EventArgs e)
    {
        Launcher.OpenAsync("https://platform.openai.com/account/api-keys");
    }
}