using System;

namespace JMath
{
	public static class JMathTest
	{
		
		public static void Test()
		{
			Float
				a = 10000000000f,
				b = 0.001f;
			
			Print(a);
			Print(b);
			Print(a.Sqrt());
			Print(b.Sqrt());
		}

		static void Print(object obj)
		{
			Console.WriteLine(obj.ToString());
		}
	}
}

