namespace ComputerCompany.Application.Client.BusinessModels;

public class Song(string title, string uri)
{
    private readonly string _title = title;
    private readonly string _uri = uri;
    private string playOrPause = "▷";

    public string Title => _title;
    public string Uri => _uri;
    public string PlayOrPause { get => playOrPause; }

    public void ChangePlayStatus()
    {
        playOrPause = PlayOrPause == "▷" ? "⏸" : "▷";
    }

    public void ChangePlayStatus(bool status)
    {
        playOrPause = status ? "⏸" : "▷";
    }
}