using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestiLog.Models
{
    public class UploaderViewModel
    {
        public List<string> initialPreview { get; set; }
        public List<FileUploaded> initialPreviewConfig { get; set; }

        public UploaderViewModel()
        {
            initialPreview = new List<string>();
            initialPreviewConfig = new List<FileUploaded>();
        }
    }

    public class FileUploaded
    {
        public string caption { get; set; }
        public int size { get; set; }
        public string width { get; set; }
        public string url { get; set; }
        public string key { get; set; }
    }

    public class ISelectViewModel
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}