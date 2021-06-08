using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Infrastructure
{
	internal abstract class ObservableObject : INotifyPropertyChanged , IDisposable
	{


		#region Dispose

		private bool _disposed;
		public void Dispose()
		{
			Dispose(true);
		}

		// ~VievModel()
		// {
		//		Dispode(false);
		// }

		protected virtual void Dispose(bool Disposing)
		{
			if (!Disposing || _disposed) return;
			_disposed = true;
		}

		#endregion Dispose

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}

		protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
		{
			if (Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(PropertyName);
			return true;
		}

	}
}
