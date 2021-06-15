using System.Collections;
using System.IO.Ports;
using SwPrpUtil.Infrastructure;

namespace SwPrpUtil.Infrastructure
{
	/// <summary>
	/// Provides a virtual folder data structure for arbitrary
	/// child items.
	/// </summary>
	internal class FolderItem : ObservableObject
	{
		#region Name

		/// <summary>
		/// The name that can be displayed or used as an
		/// ID to perform more complex styling.
		/// </summary>
		private string name;

		/// <summary>
		/// The name that can be displayed or used as an
		/// ID to perform more complex styling.
		/// </summary>
		public string Name
		{
			get { return name; }
			set
			{
				//ignore if values are equal
				Set(ref name, value);
			}
		}

		#endregion Name

		#region Items

		/// <summary>
		/// The child items of the folder.
		/// </summary>
		private IEnumerable items;

		/// <summary>
		/// The child items of the folder.
		/// </summary>
		public IEnumerable Items
		{
			get { return items; }
			set
			{
				Set(ref items, value);
			}
		}

		#endregion Items

		public FolderItem()
		{
		}

		/// <summary>
		/// This method is invoked by WPF to render the object if
		/// no data template is available.
		/// </summary>
		/// <returns>Returns the value of the <see cref="Name"/>
		/// property.</returns>
		public override string ToString()
		{
			return string.Format("{0}: {1}", GetType().Name, Name);
		}
	}
}