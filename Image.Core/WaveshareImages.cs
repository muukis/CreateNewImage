using System.Drawing;

namespace Image.Core
{
    public class WaveshareImages
    {
        public Bitmap RedImage { get; } = new Bitmap(ImageCreator.WIDTH, ImageCreator.HEIGHT);
        public Bitmap BlackImage { get; } = new Bitmap(ImageCreator.WIDTH, ImageCreator.HEIGHT);
    }
}
