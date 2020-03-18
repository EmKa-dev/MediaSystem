using MediaSystem.DesktopClientWPF.GUITest;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageViewerViewModel : BaseViewModel
    {
        public byte[] ImageData { get; set; }

        public ImageViewerViewModel()
        {
            ImageData = TestImageResources.Blue;
        }

        public ImageViewerViewModel(byte[] data)
        {
            ImageData = data;
        }
    }
}
