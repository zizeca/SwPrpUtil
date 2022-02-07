using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SwPrpUtil.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
    internal class SwPrpEditor : ObservableObject
    {
        #region prop

        private List<SwFileItem> _targetFiles;

        public List<SwFileItem> TargetFiles
        {
            get => _targetFiles;
            set => Set(ref _targetFiles, value);
        }

        private List<SwFileItem> _sourceFiles;
        public List<SwFileItem> SourceFiles { get => _sourceFiles; }

        /// <summary>
        /// Properties for modify and add to target files
        /// </summary>
        private List<SwProperty> _targetProperties;

        public List<SwProperty> TargetProperties
        {
            get => _targetProperties;
            set => Set(ref _targetProperties, value);
        }

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

        #endregion prop

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
                    _targetFiles.Add(new SwFileItem(file));
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

            //StatusMessage = "Rebuild document";
            //doc.Rebuild((int)swRebuildOptions_e.swRebuildAll);

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

        public async void ModifyTargetFilesAsync(CancellationToken token,
                                                    bool modifyMainProperty,
                                                    bool modifyConfigProperty,
                                                    bool rewriteIfExist)
        {
            if (_targetProperties == null || _targetProperties.Count == 0) 
                throw new InvalidOperationException("Target properties is null or empty");


            StatusMessage = "Run SolidWorks";
            SldWorks swApp;
            try
            {
                swApp = await SwHolder.Instance.GetSwAppAsync();
            }
            catch (Exception e)
            {
                StatusMessage = string.Format("Cautch error esception {0}", e.Message);
                return;
            }
            StatusMessage = "Solidworks Started";

            foreach (var file in _targetFiles)
            {
                if (token.IsCancellationRequested)
                {
                    StatusMessage = "Operation cancelled";
                    return;
                }

                int Error = 0; // file load or save error code
                int Warning = 0; // file load or save warning code

                ModelDoc2 doc = await Task.Run(() =>
                {
                    return swApp.OpenDoc6(  file.FilePath, 
                                            (int)SwHelperFunction.GetSwDocTypeIdFromExtension(file.FilePath), 
                                            (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                                            "",
                                            ref Error,
                                            ref Warning);
                });

                if (doc != null || Error != 0) continue;

                if (modifyMainProperty)
                {
                    ModifyCustomProperty(doc, "", rewriteIfExist);
                }

                if (modifyConfigProperty)
                {
                    string[] configurations = doc.GetConfigurationNames();
                    foreach (string configuration in configurations)
                    {
                        ModifyCustomProperty(doc, configuration, rewriteIfExist);
                    }
                }

                _ = doc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref Error, ref Warning);

                swApp.CloseDoc(doc.GetPathName());
                doc = null;
            }
        }

        private void ModifyCustomProperty(ModelDoc2 doc, string configName, bool rewriteIfExist)
        {
            CustomPropertyManager manager = doc.Extension.CustomPropertyManager[configName];
            swCustomPropertyAddOption_e option = rewriteIfExist ? swCustomPropertyAddOption_e.swCustomPropertyReplaceValue : swCustomPropertyAddOption_e.swCustomPropertyOnlyIfNew;
            foreach (SwProperty item in _targetProperties)
            {
                manager.Add3(item.PropertyName, (int)item.TypePrp, item.Expression, (int)option);
            }
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


        #region ExtractProperties

        public static List<SwProperty> ExtractProperties(SwFileItem fileItem, string configName = "")
        {
            if (fileItem == null) throw new ArgumentNullException(nameof(fileItem));
            return ExtractProperties(fileItem.FileProperties, configName);
        }

        public static List<SwProperty> ExtractProperties(SwFileProperty fileProperty, string configName = "")
        {
            if (fileProperty == null) throw new ArgumentNullException(nameof(fileProperty));
            return fileProperty.GetProperties(configName);
        }

        public static List<SwProperty> ExtractProperties(SwCustomProperty swCustomProperty)
        {
            if (swCustomProperty == null) throw new ArgumentNullException(nameof(swCustomProperty));
            return swCustomProperty.Properties;
        }

        #endregion ExtractProperties
    }
}