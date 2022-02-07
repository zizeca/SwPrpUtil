using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
    internal class SwFileProperty
    {
		public SwFileProperty() { }

		public SwFileProperty(ModelDoc2 doc)
        {
			this.ReadFilePropertyFromDoc(doc);
        }

		public List<SwProperty> GetProperties(string configName = "")
        {
			if (configName == "") return this.MainProperty.Properties;

            foreach (var config in this.SwFileConfigurations)
            {
				if(config.ConfigurationName == configName)
					return config.Properties;
            }
			throw new Exception(configName + " Not found");
        }


        public SwFileSummaryInfo SummaryInfo { get; set; }

		// Main custom property
		//public List<SwProperty> MainProperty { get; set; }
		public SwCustomProperty MainProperty { get; set; }

		//Configurations with self properties
		public List<SwCustomProperty> SwFileConfigurations { get; set; }


		/// <summary>
		/// Get properties from solidworks document
		/// </summary>
		/// <param name="doc">Opened document in solidworks application</param>
		/// <param name="configName">configuration name. If name is "" return Main properties</param>
		/// <returns>List of properties</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static List<SwProperty> GetPropertiesFromSwDoc(ref ModelDoc2 doc, string configName = "")
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

		/// <summary>
		/// Read all information from opened solidwroks document
		/// </summary>
		/// <param name="doc">Opened document in solidworks application</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public void ReadFilePropertyFromDoc(ModelDoc2 doc)
		{
			if (doc == null)
				throw new ArgumentNullException(nameof(doc));

			string[] configNames = (string[])doc.GetConfigurationNames();

			if (configNames == null || configNames.Count() == 0)
				throw new Exception("Configuration get exception");

			MainProperty = new SwCustomProperty("Main Properties", GetPropertiesFromSwDoc(ref doc));
			SummaryInfo = new SwFileSummaryInfo(doc);

			SwFileConfigurations = new List<SwCustomProperty>();
			foreach (string configName in configNames)
			{
				SwCustomProperty fc = new SwCustomProperty(configName, GetPropertiesFromSwDoc(ref doc, configName));
				SwFileConfigurations.Add(fc);
			}

		}
	}
}
