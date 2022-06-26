using LabeledGallery.DatabaseSetting;

namespace LabeledGallery;

public class Settings
{
    public string BaseUrl { get; set; }
    public DatabaseSettings Database { get; set; }
}