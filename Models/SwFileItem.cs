using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        #region InfoSwFile

        public SwFileSummaryInfo SummaryInfo { get; set; }

		// Main custom property
		//public List<SwProperty> MainProperty { get; set; }
		public SwFileConfiguration MainProperty { get; set; }

		//Configurations with self properties
		public List<SwFileConfiguration> SwFileConfigurations { get; set; }

        #endregion

        #region Ctors

        public SwFileItem()
		{
			this.SummaryInfo = null;
			this.MainProperty = null;
			this.SwFileConfigurations = null;
		}

		public SwFileItem(string path)
        {
			this.FilePath = path;
        }


		public SwFileItem(ModelDoc2 doc)
		{
			ReadFromDoc(doc);
		}

		#endregion Ctors

		/// <summary>
		/// Read all information from opened solidwroks document
		/// </summary>
		/// <param name="doc">Opened document in solidworks application</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public void ReadFromDoc(ModelDoc2 doc)
		{
			if (doc == null)
			{
				throw new ArgumentNullException(nameof(doc));
			}

			string[] configNames = (string[])doc.GetConfigurationNames();

			if (configNames == null || configNames.Count() == 0)
				throw new Exception("Configuration get exception");



			FilePath = doc.GetPathName();
			MainProperty = new SwFileConfiguration("Main Properties", GetSwProperties(ref doc));
			SummaryInfo = new SwFileSummaryInfo(doc);

			SwFileConfigurations = new List<SwFileConfiguration>();
			foreach (string configName in configNames)
			{
				SwFileConfiguration fc = new SwFileConfiguration(configName, GetSwProperties(ref doc, configName));
				SwFileConfigurations.Add(fc);
			}

		}

		/// <summary>
		/// Get properties from solidworks document
		/// </summary>
		/// <param name="doc">Opened document in solidworks application</param>
		/// <param name="configName">configuration name. If name if "" return Main properties</param>
		/// <returns>List of properties</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static List<SwProperty> GetSwProperties(ref ModelDoc2 doc, string configName = "")
		{
			if (doc == null)
				throw new ArgumentNullException(nameof(doc));

			List<SwProperty> retList = new List<SwProperty>();

			CustomPropertyManager manager = doc.Extension.CustomPropertyManager[configName];

			object PropNames = null;
			object PropTypes = null;
			object PropValues = null;
			object Resolved = null;
			object PropLink = null;
			int PropCount = 0;

			PropCount = manager.GetAll3(ref PropNames, ref PropTypes, ref PropValues, ref Resolved, ref PropLink);

			try
			{
				Debug.WriteLine(string.Format("Found {0} properties", PropCount));
				for (int i = 0; i < PropCount; i++)
				{
					SwProperty prp = new SwProperty
					{
						PropertyName = ((string[])PropNames)[i],
						TypePrp = (swCustomInfoType_e)((int[])PropTypes)[i],
						Expression = ((string[])PropValues)[i]
					};
					Debug.WriteLine(string.Format("Add prp: {0}|{1}|{2}", prp.PropertyName, prp.TypePrp.ToString(), prp.Expression));
					retList.Add(prp);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(string.Format("Catch exception {0}", e.Message));
			}
			return retList;
		}
	}
}