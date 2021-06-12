using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace SwPrpUtil.Models
{
	public class SwHolder : IDisposable
	{
		private static SwHolder _instance = null;

		public static SwHolder Instance
		{
			get
			{
				if (_instance == null)
					_instance = new SwHolder();
				return _instance;
			}
		}

		private SwHolder()
		{
		}

		private SldWorks swApp;

		private static bool _firsRun = true;
		private static bool _shallExitApp = true;
		private static bool _sldVisible = false;

		public static bool SwAppIsRun()
		{
			if (_instance == null || _instance.swApp == null) return false;

			Process[] pname = Process.GetProcessesByName("SldWorks");
			if (pname.Length != 0 && _instance.swApp != (pname[0] as SldWorks)) return false;

			return true;
		}

		public async Task<SldWorks> GetSwAppAsync()
		{
			//throw new InvalidOperationException("Test exception");

			Process[] pname = Process.GetProcessesByName("SldWorks");
			if (pname.Length != 0)
			{
				if (_firsRun)
				{
					_firsRun = false;
					_sldVisible = swApp?.Visible ?? true;
					_shallExitApp = false;
				}

				if (swApp != (pname[0] as SldWorks))
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

			if (swApp != null && pname.Length != 0)
			{
				swApp.Visible = true;
			}

			if (swApp == null || pname.Length == 0)
			{
				return await Task<SldWorks>.Run(() =>
				{
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

		public static void DisposeInstance()
		{
			if (_instance != null)
			{
				_instance.Dispose();
				_instance = null;
			}
		}

		public void Dispose()
		{
			Debug.WriteLine("Try dispose swholder");
			if (swApp != null && _shallExitApp)
			{
				swApp.CloseAllDocuments(true);
				swApp.ExitApp();
				swApp = null;

				Debug.WriteLine("sww holder disposing");

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
			else if (swApp != null)
			{
				swApp.Visible = true;
			}
		}
	}
}