using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class Sample {
	public static void Main (string []args) {
		Calculate ();

		ApplyMagicMethodBodyReplacement ();

		Calculate ();

		ApplyMagicMethodBodyReplacement ();

		Calculate ();
	}

	private static void Calculate () {
		for (int i = 1; i < 5; i++)
			for (int j = 1; j < 4; j++)
				Console.WriteLine ("Do (" + i + ", " + j + "): " + Do (i, j));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int Do (int x, int y) {
		return x + y;
	}

	private static Dictionary<Assembly, int> assembly_count = new Dictionary<Assembly, int> ();

	private static void ApplyMagicMethodBodyReplacement () {
#if false
		var name = "System.Runtime.CompilerServices.RuntimeFeature";
#else
		var name = "Mono.Runtime";
#endif
		var monoType = Type.GetType (name, false);
		try {
			Console.WriteLine ("Apply Delta Update");
			var assm = typeof(Sample).Assembly;

			int count;
			if (!assembly_count.TryGetValue (assm, out count))
				count = 1;
			else
				count++;
			assembly_count [assm] = count;

			string basename = assm.Location;
			string dmeta_name = $"{basename}.{count}.dmeta";
			string dil_name = $"{basename}.{count}.dil";
			byte[] dmeta_data = System.IO.File.ReadAllBytes (dmeta_name);
			byte[] dil_data = System.IO.File.ReadAllBytes (dil_name);

			var update = monoType.GetMethod("LoadMetadataUpdate");
			update.Invoke (null, new object[] { assm, dmeta_data, dil_data });
		} catch (Exception e) {
			Console.WriteLine ("the impossible happen: " + e);
		}
	}
}
