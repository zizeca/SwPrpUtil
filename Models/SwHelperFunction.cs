using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.swconst;

namespace SwPrpUtil.Models
{
	internal static class SwHelperFunction
	{
		/// <summary>
		/// Method for convert file path whith extension to solidworks const enumiration value.
		/// </summary>
		/// <remarks>
		/// Method suport file with extension:<br/>
		/// part(*.sldprt),<br/>
		/// drawing(*.slddrw),<br/>
		/// assembly(*.sldasm).
		/// </remarks>
		/// <param name="path">Path to solidwork file</param>
		/// <returns>Type of file as enum <see cref="swDocumentTypes_e"/></returns>
		/// <exception cref="NotSupportedException">
		/// Throw if file not supported in this application.
		/// </exception>
		public static swDocumentTypes_e GetTypeIdFromExtension(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path) || !Path.HasExtension(path))
			{
				throw new ArgumentException(string.Format("Wrong path: \"{0}\"", path));
			}

			string ext = Path.GetExtension(path).ToLower();

			switch (ext)
			{
				case ".sldprt":
					return swDocumentTypes_e.swDocPART;

				case ".sldasm":
					return swDocumentTypes_e.swDocASSEMBLY;

				case ".slddrw":
					return swDocumentTypes_e.swDocDRAWING;

				default:
					throw new NotSupportedException(ext);
			}
		}
	}
}