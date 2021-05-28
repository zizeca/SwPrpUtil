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

		private static bool SwWasStarted = false;
		private static bool ProcessChecked = false;


		public async static Task<SldWorks> GetSwAppAsync()
		{
			if (!ProcessChecked)
			{
				Process[] pname = Process.GetProcessesByName("SldWorks");
				if (pname.Length != 0)
				{
					SwWasStarted = true;
					Console.WriteLine("SldWorks is running");
				}

				ProcessChecked = true;
			}


			if (swApp == null)
			{

				//*
				return await Task<SldWorks>.Run(() => {
					swApp = Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application")) as SldWorks;

					if (SwWasStarted)
						swApp.Visible = true;
					else
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
			if (swApp != null && !SwWasStarted)
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
		}
	}
}
