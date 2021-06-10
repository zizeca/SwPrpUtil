using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtilConsole
{
	internal class Program
	{
		public enum Testflag
		{
			none = 0,
			one = 1,
			two = 2,
			four = 4,
			eight = 8
		}

		private static Testflag f = Testflag.four | Testflag.one;
		private static Testflag z = Testflag.four | Testflag.none;

		private static void Main(string[] args)
		{
			IEnumerable<Enum> e1 = GetFlags(f);
			IEnumerable<Enum> e2 = GetFlags(z);
			IEnumerable<Enum> e3 = GetUniqueFlags(z);

			Console.WriteLine("test flags f");
			foreach (var item in e1)
			{
				Console.Write("{0} ,", item.ToString());
			}

			Console.WriteLine("test flags z");
			foreach (var item in e2)
			{
				Console.Write("{0} ,", item.ToString());
			}

			Console.WriteLine("test unique flags z");
			foreach (var item in e3)
			{
				Console.Write("{0} ,", item.ToString());
			}


			Console.Read();
		}

		private static IEnumerable<Enum> GetFlags(Enum input)
		{
			foreach (Enum value in Enum.GetValues(input.GetType()))
				if (input.HasFlag(value))
					yield return value;
		}

		public static IEnumerable<Enum> GetUniqueFlags(Enum flags)
		{
			ulong flag = 1;
			foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
			{
				ulong bits = Convert.ToUInt64(value);
				while (flag < bits)
				{
					flag <<= 1;
				}

				if (flag == bits && flags.HasFlag(value))
				{
					yield return value;
				}
			}
		}
	}
}