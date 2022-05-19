using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	/// <summary>
	/// Structure for wrap custom properties and configuration name
	/// </summary>
	internal class SwCustomProperty
	{
		// parent configuration
		//public SwFileConfiguration BaseFileConfiguration { get; set; }

		// list of children configurations
		//public List<SwFileConfiguration> Children { get; set; }

		/// <summary>
		/// Nmae of configuration file
		/// </summary>
		public string ConfigurationName { get; set; }

		/// <summary>
		/// Custom properties
		/// </summary>
		public List<SwProperty> Properties { get; set; }

		/// <summary>
		/// Create structure
		/// </summary>
		/// <param name="configurationName">name of configuration</param>
		/// <param name="properties">List of SwProperty(solidworks custom properties)</param>
		public SwCustomProperty(string configurationName, List<SwProperty> properties)
		{
			ConfigurationName = configurationName;
			Properties = properties;
		}

		/// <summary>
		/// Default empty object
		/// </summary>
		public SwCustomProperty()
		{
		}
	}
}