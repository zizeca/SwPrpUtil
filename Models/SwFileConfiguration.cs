using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	internal class SwFileConfiguration
	{
		public SwFileConfiguration BaseFileConfiguration = null;

		public string ConfigurationName;

		public List<SwProperty> Properties;
	}
}