using SolidWorks.Interop.sldworks;
using System;
using System.IO;

namespace SwPrpUtil.Models
{
    internal class SwFileItem
    {
        #region Path_properies

        private string _filePath;

        public string Extension { get; private set; }

        public string FileName { get; private set; }

        public string FileNameWithoutExtension { get; private set; }

        /// <summary>
        /// File path fill other info of path to file/
        /// No check extension and type of file
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                Extension = Path.GetExtension(_filePath);
                FileName = Path.GetFileName(_filePath);
                FileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
            }
        }

        #endregion Path_properies

        public SwFileProperty FileProperties { get; set; }

        #region Ctors

        public SwFileItem()
        {
            FileProperties = null;
        }

        public SwFileItem(string path)
        {
            this.FilePath = path;
        }

        public SwFileItem(ModelDoc2 doc)
        {
            ReadFromDoc(doc);
        }

        private void ReadFromDoc(ModelDoc2 doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));

            FilePath = doc.GetPathName();
            FileProperties = new SwFileProperty(doc);

            throw new NotImplementedException();
        }

        #endregion Ctors
    }
}