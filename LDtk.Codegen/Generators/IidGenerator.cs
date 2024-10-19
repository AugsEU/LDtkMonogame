namespace LDtk.Codegen.Generators;

using LDtk.Full;

public class IidGenerator(LDtkFileFull ldtkFile, Options options) : BaseGenerator(ldtkFile, options)
{
	public void Generate()
	{
		Line($"namespace {Options.Namespace};");
		Blank();
		Line("// This file was automatically generated, any modifications will be lost!");
		Blank();
		Line("#pragma warning disable");
		Line("public static class Worlds");
		StartBlock();
		foreach (LDtkWorld w in LDtkFile.Worlds)
		{
			Line($"public static class {w.Identifier}");
			StartBlock();
			Line($"public static readonly System.Guid Iid = System.Guid.Parse(\"{w.Iid}\");");
			Blank();
			foreach (LDtkLevel l in w.Levels)
			{
				Line($"public static readonly System.Guid {l.Identifier} = System.Guid.Parse(\"{l.Iid}\");");
			}
			EndBlock();
		}
		EndBlock();
		Line("#pragma warning restore");

		Output("Iids", "Worlds");
	}
}
