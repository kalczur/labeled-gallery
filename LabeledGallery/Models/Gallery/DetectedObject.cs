namespace LabeledGallery.Models.Gallery;

public class DetectedObject
{
    public string Id { get; set; }
    public string Label { get; set; }
    public float Accuracy { get; set; }
    public DetectionProvider? DetectionProvider { get; set; }
}

public enum DetectionProvider
{
    User,
    Gcp
}