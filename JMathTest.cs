using System;

namespace JMath
{
	public static class JMathTest
	{
		public static void Test()
		{
			Float
				a = 1000000000000f,
				b = 0.001f;
			
			Print(a);
			Print(b);
			Print(a/b);
		}

		static void Print(object obj)
		{
			Console.WriteLine(obj.ToString());
		}
	}
}

