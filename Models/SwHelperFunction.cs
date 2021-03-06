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
		/// <exception cref="ArgumentException">
		/// <exception cref="NotSupportedException">
		/// Throw if file not supported in this application.
		/// </exception>
		public static swDocumentTypes_e GetSwDocTypeIdFromExtension(string path)
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

		/// <summary>
		/// Check file extension. Also ignore temp file (file begin with '~').
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>true if extension is ".sldprt", ".sldasm", ".slddrw", otherwise false</returns>
		public static bool IsSolidworksFile(string filePath)
        {
			if(filePath == null) return false;
			if (Path.GetFileName(filePath).First() == '~') return false;
			string ext = Path.GetExtension(filePath).ToLower();
			return (ext == ".sldprt" || ext == ".sldasm" || ext == ".slddrw");
		}

		#region Parse sw load file

		/// <summary>
		/// Parser solidworks open doc errors
		/// </summary>
		/// <param name="ErrorCode"></param>
		/// <returns>list with errors</returns>
		public static IEnumerable<string> SwOpenDocErrorToString(int ErrorCode)
		{
			IEnumerable<Enum> flags = GetFlags((swFileLoadError_e)ErrorCode);
			foreach (var flag in flags)
			{
				yield return SwFailLoadCodeToString((swFileLoadError_e)flag);
			}
		}

		/// <summary>
		/// Description solidworks Load file error
		/// </summary>
		/// <param name="Error"></param>
		/// <returns></returns>
		public static string SwFailLoadCodeToString(swFileLoadError_e Error)
		{
			switch (Error)
			{
				case swFileLoadError_e.swGenericError:
					return "Another error was encountered";

				case swFileLoadError_e.swFileNotFoundError:
					return "Unable to locate the file; the file is not loaded or the referenced file (that is, component) is suppressed";

				case swFileLoadError_e.swIdMatchError:
					return "(Obsolete) Warning appears if the internal ID of the document does not match the internal ID saved with the referencing document.";

				case swFileLoadError_e.swReadOnlyWarn:
					return "(Obsolete) Warning appears because the document is read only.";

				case swFileLoadError_e.swSharingViolationWarn:
					return "(Obsolete) Warning appears if the document is being used by another user.";

				case swFileLoadError_e.swDrawingANSIUpdateWarn:
					return "(Obsolete) Warning appears because radial dimension arrows now displayed outside when the dimension text is outside of the arc or circle.";

				case swFileLoadError_e.swSheetScaleUpdateWarn:
					return "(Obsolete) Warning appears because SOLIDWORKS now applies the scale of the sheet to the sketch entities on the sheet; which means that the sheet looks the same but dimension values are scaled";

				case swFileLoadError_e.swNeedsRegenWarn:
					return "(Obsolete) Warning appears because the document needs to be rebuilt.";

				case swFileLoadError_e.swBasePartNotLoadedWarn:
					return "(Obsolete) Warning appears because the document was defined in the context of another existing document that is not loaded.";

				case swFileLoadError_e.swFileAlreadyOpenWarn:
					return "(Obsolete) Warning appears because the document is already open.";

				case swFileLoadError_e.swInvalidFileTypeError:
					return "File type argument is not valid";

				case swFileLoadError_e.swDrawingsOnlyRapidDraftWarn:
					return "(Obsolete) Warning appears because the only RapidDraft format conversion that can take place is a drawing document that is not Detached.";

				case swFileLoadError_e.swViewOnlyRestrictions:
					return "(Obsolete) Warning appears because the document is view only and a configuration other than the default configuration is set.";

				case swFileLoadError_e.swFutureVersion:
					return "The document was saved in a future version of SOLIDWORKS";

				case swFileLoadError_e.swViewMissingReferencedConfig:
					return "(Obsolete) Warning appears because a configuration that a drawing view is referencing no longer exists in the model (part or assembly); the active configuration is used.";

				case swFileLoadError_e.swDrawingSFSymbolConvertWarn:
					return "(Obsolete) Warning appears asking the user if he or she wants to convert this drawing's surface finish symbols to the sizes specified in ANSI Y14.36M-1996 and ISO 1302-1978.";

				case swFileLoadError_e.swFileWithSameTitleAlreadyOpen:
					return "A document with the same name is already open";

				case swFileLoadError_e.swLiquidMachineDoc:
					return "File encrypted by Liquid Machines";

				case swFileLoadError_e.swLowResourcesError:
					return "File is open and blocked because the system memory is low, or the number of GDI handles has exceeded the allowed maximum";

				case swFileLoadError_e.swNoDisplayData:
					return "File contains no display data";

				case swFileLoadError_e.swAddinInteruptError:
					return "The user attempted to open a file, and then interrupted the open-file routine to open a different file";

				case swFileLoadError_e.swFileRequiresRepairError:
					return "A document has non-critical custom property data corruption";

				case swFileLoadError_e.swFileCriticalDataRepairError:
					return "A document has critical data corruption";

				case swFileLoadError_e.swApplicationBusy:
					return "Application Busy";
				default:
					return string.Format("Unknown error (swFileLoadError_e = {0:X}).", (int)Error);
			}
		}


		#endregion Parse sw load file


		/// <summary>
		/// Method parser flags
		/// </summary>
		/// <remarks>
		/// Input:
		///	  Enum flags = flag1 | flag2 | ... | flagN
		///	Output:
		///	  IEnumerable<Enum> f = {flag1, flag2, ... , flagN }
		/// </remarks>
		/// <param name="input">Enumiration flags</param>
		/// <returns>Array flags</returns>
		public static IEnumerable<Enum> GetFlags(Enum input)
		{
			foreach (Enum value in Enum.GetValues(input.GetType()))
				if (input.HasFlag(value))
					yield return value;
		}

		/// <summary>
		/// Get solidworks files from dir (without recursion)
		/// </summary>
		/// <param name="pathToDir"> path to directory</param>
		/// <returns>list of solidwroks files or empty</returns>
		public static IEnumerable<string> GetFilesFromDir(string pathToDir)
		{
			return from file in Directory.EnumerateFiles(pathToDir)
				   where Path.GetExtension(file)?.ToLower() == ".sldprt" ||
						   Path.GetExtension(file)?.ToLower() == ".sldasm" ||
						   Path.GetExtension(file)?.ToLower() == ".slddrw"
				   select file;
		}
	}
}