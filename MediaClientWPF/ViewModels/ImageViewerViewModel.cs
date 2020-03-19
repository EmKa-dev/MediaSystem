
namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageViewerViewModel : BaseViewModel
    {
        public byte[] ImageData { get; set; }

        public ImageViewerViewModel(byte[] data)
        {
            ImageData = data;
        }
    }
}
