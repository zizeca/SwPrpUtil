using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SolidWorks.Interop.sldworks;

namespace SwPrpUtil.Model
{
	public class SwHolder
	{
		private SwHolder() { }

		private static SldWorks swApp;

		private static bool _firsRun = true;
		private static bool _closeSwappAfterCloseWindow = true;
		private static bool _sldVisible = false;



		public async static Task<SldWorks> GetSwAppAsync()
		{
			//throw new InvalidOperationException("Test exception");

			Process[] pname = Process.GetProcessesByName("SldWorks");
			if (pname.Length != 0)
			{
				if(_firsRun)
				{
					_firsRun = false;
					_sldVisible = swApp?.Visible ?? true;
					_closeSwappAfterCloseWindow = false;
				}

				if(swApp != (pname[0] as SldWorks))
				{
					swApp = null;
				}

				Debug.WriteLine("SldWorks is running");
			}
			else
			{
				swApp = null; //check if procees terminated but swApp not null
			}


			Debug.WriteLine("Num process = {0}", pname.Length);

			if(swApp != null && pname.Length != 0)
			{
				swApp.Visible = true;
			}

			if (swApp == null || pname.Length == 0)
			{
				return await Task<SldWorks>.Run(() => {
					swApp = Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application")) as SldWorks;
					swApp.Visible = false;
					return swApp;
				});

			}
			else
			{
				Debug.WriteLine("Sld works is not null");
			}

			return swApp;
		}

		public static void Dispose()
		{
			
			if (swApp != null && _closeSwappAfterCloseWindow )
			{
				swApp.CloseAllDocuments(true);
				swApp.ExitApp();
				swApp = null;

				//if process is not close
				try
				{
					foreach (Process proc in Process.GetProcessesByName("SldWorks"))
					{
						proc.Kill();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			} 
			else if( swApp != null)
			{
				swApp.Visible = true;
			}



		}
	}
}
