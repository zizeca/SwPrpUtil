﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Model
{
	class SwPrpEditor
	{
		private List<SwFileItem> _swFileItems;
		private List<SwProperty> _swSourceProperties;


		public bool AddFolder(string pathToFolder) 
		{
			//Open folder
			//Read files from folder
			//Invoke AddFile for each files



			throw new NotImplementedException();
		}

		public bool AddFile(string pathToFile)
		{
			//Create SwFileItem and add to _swFileItems/

			throw new NotImplementedException();
		}

		public bool ImportPropertiesFromFile(string pathToFile, string configName = null)
		{
			throw new NotImplementedException();
		}




	}
}
