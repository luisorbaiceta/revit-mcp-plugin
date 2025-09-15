using System;

namespace SampleCommandSet.Models
{
    public class BoundingBoxInfo
    {
        public Point Min { get; set; }
        public Point Max { get; set; }
    }

    public class ViewCropInfo
    {
        public BoundingBoxInfo CropBox { get; set; }
        public bool CropBoxActive { get; set; }
        public bool CropBoxVisible { get; set; }
    }
}
