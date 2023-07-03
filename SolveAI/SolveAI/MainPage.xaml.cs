

namespace SolveAI;

public partial class MainPage : ContentPage
{

    public string APIKey;
	public MainPage()
	{
		InitializeComponent();
    }

    private void Submit_Clicked(object sender, EventArgs e)
    {




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

