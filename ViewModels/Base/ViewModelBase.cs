using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.ViewModels.Base
{
	internal abstract class ViewModelBase : INotifyPropertyChanged , IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private bool _Disposed;

		protected virtual void Dispose(bool Disposing)
		{
			if (!Disposing || _Disposed) return;
			_Disposed = true;
		}


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
