using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
    internal class SwPrpEditor : ObservableObject
    {
        private List<SwFileItem> _targetFiles;
        public List<SwFileItem> TargetFiles { get => _targetFiles; }

        private List<SwFileItem> _sourceFiles;
        public List<SwFileItem> SourceFiles { get => _sourceFiles; }

        /// <summary>
        /// Properties for modify and add to target files
        /// </summary>
        private List<SwProperty> _targetProperties;

        public List<SwProperty> TargetProperties { get => _targetProperties; }

        private string _statusMessage = "Ready";

        /// <summary>
        /// Status of state SwPrpEditor
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            private set => Set(ref _statusMessage, value);
        }

        //ctor
        public SwPrpEditor()
        {
            _targetProperties = new List<SwProperty>();
            _targetFiles = new List<SwFileItem>();
            _sourceFiles = new List<SwFileItem>();
        }

        /// <summary>
        /// Get all files from direcory "pathToFolder" and run AddFiles method
        /// </summary>
        /// <param name="pathToFolder"> path to directory with SolidWorks files </param>
        /// <returns> true if success to add files </returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public bool AddFolder(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
                throw new DirectoryNotFoundException(pathToFolder);

            string[] files = Directory.GetFiles(pathToFolder);
            if (files.Length == 0)
            {
                Debug.WriteLine(string.Format("Directory {0} has not files", pathToFolder));
                return false;
            }

            AddFiles(files);

            return true;
        }

        /// <summary>
        /// Create SwFileItem and add to _targetFiles
        /// </summary>
        /// <param name="files"> pathes to solidworks files</param>
        public void AddTargetFiles(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file) && !File.Exists(file)) continue;

                //ignore no solidworks files
                string ext = Path.GetExtension(file).ToLower();
                if (!(ext == ".sldptr" || ext == ".sldasm" || ext == ".slddrw")) continue;

                if (_targetFiles == null)
                    _targetFiles = new List<SwFileItem>();

                if (!_targetFiles.Exists(x => x.FilePath == file))
                {
                    SwFileItem swFileItem = new SwFileItem();
                    swFileItem.FilePath = file;
                    _targetFiles.Add(swFileItem);
                }
            }
        }

        /// <summary>
        /// Add files from list of pathes to _importedFiles
        /// </summary>
        /// <param name="pathes"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddFiles(string[] pathes)
        {
            if (pathes == null || pathes.Count() == 0)
                throw new ArgumentException(nameof(pathes));

            foreach (string path in pathes)
            {
                if (string.IsNullOrEmpty(path) || path.Contains(@"~$")) continue;

                string extension = Path.GetExtension(path)?.ToLower();
                if (extension == ".sldprt" || extension == ".sldasm" || extension == ".slddrw")
                {
                    SwFileItem fileItem = new SwFileItem();
                    fileItem.FilePath = path;
                    _targetFiles.Add(fileItem);
                }
                else
                {
                    Debug.WriteLine(string.Format("Ignoring file {0}", path));
                }
            }

            OnPropertyChanged(nameof(TargetFiles));
        }

        /// <summary>
        /// Read all properties from file and add file to _sourceFiles
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileLoadException"></exception>
        public async Task<bool> ImportFileProperties(string pathToFile)
        {
            if (string.IsNullOrEmpty(pathToFile) || !File.Exists(pathToFile))
                throw new ArgumentException(nameof(pathToFile));

            ///Start or get solidworks process

            StatusMessage = "Run SolidWorks";
            SldWorks swApp;
            try
            {
                swApp = await SwHolder.Instance.GetSwAppAsync();
            }
            catch (Exception e)
            {
                StatusMessage = string.Format("Cautch error esception {0}", e.Message);
                return false;
            }
            StatusMessage = "Solidworks Started";

            _ = swApp.SetCurrentWorkingDirectory(Path.GetDirectoryName(pathToFile));

            #region Open_Document

            int Error = 0; // file load error code
            int Warning = 0; // file load warning code

            ModelDoc2 doc;

            doc = swApp.GetOpenDocumentByName(pathToFile); // check if file is open, return ModelDoc2 or null

            if (doc == null)
            {
                StatusMessage = string.Format("Openning... {0}", Path.GetFileName(pathToFile));

                //Open document
                doc = swApp.OpenDoc6(pathToFile,
                                        (int)SwHelperFunction.GetSwDocTypeIdFromExtension(pathToFile),
                                        (int)(swOpenDocOptions_e.swOpenDocOptions_Silent | swOpenDocOptions_e.swOpenDocOptions_ReadOnly),
                                        "",
                                        ref Error,
                                        ref Warning);
            }

            if (Error != 0)
                throw new FileLoadException(string.Format("Open file error code {0:X}", Error));

            if (Warning != 0)
                Debug.WriteLine(string.Format("Open file has warning, code: {0:X}", Warning));

            StatusMessage = "Rebuild document";
            doc.Rebuild((int)swRebuildOptions_e.swRebuildAll);

            #endregion Open_Document

            try
            {
                _sourceFiles.Add(new SwFileItem(doc));
            }
            catch (Exception e)
            {
                StatusMessage = string.Format("Cautch error when create fileitem. Cause: {0}", e.Message);
                return false;
            }
            finally
            {
                doc.Quit();
            }

            OnPropertyChanged(nameof(SourceFiles));

            return true;
        }

        public void RunProcess()
        {
            // write properties from _swSourceProperties to each SwFileItem in _swFileItems

            throw new NotImplementedException();
        }

        public void ClearTargetFileList()
        {
            _targetFiles.Clear();
            OnPropertyChanged(nameof(TargetFiles));
        }

        public void ClearTargetPropertyList()
        {
            _targetProperties.Clear();
            OnPropertyChanged(nameof(TargetProperties));
        }
    }
}