namespace LDtk.Codegen.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LDtk.Full;

public class GameObjectFactoryGenerator(LDtkFileFull ldtkFile, Options options) : BaseGenerator(ldtkFile, options)
{
	void GenHeaders()
	{
		Line($"namespace {Options.Namespace};");
		Blank();
		Line("// This file was automatically generated, any modifications will be lost!");
		Line("#pragma warning disable");
		Blank();
		Line("using LDtk;");
		Blank();
	}

	public void Generate()
	{
		GenHeaders();

		Line("public static class GameObjectFactory");
		StartBlock();
		{
			Line("public static MGameObject FromLDtkEntity(LDtkLevel levelData, LDtk.EntityInstance entityInstance)");
			StartBlock();
			{
				Line("CustomLevelData customData = levelData.GetCustomFields<CustomLevelData>();");
				Line("");
				Line("Vector2 overridePos = Vector2.Zero;");
				Line("if (customData is not null)");
				StartBlock();
				{
					Line("overridePos = new Vector2(customData.OverrideX, customData.OverrideY);");
				}
				EndBlock();

				Line("");
				Line("");

				Line("switch (entityInstance._Identifier)");
				StartBlock();
				{
					// Entity Classes
					foreach (EntityDefinition e in LDtkFile.Defs.Entities)
					{
						GenCaseForEntity(e);
					}

					Line("default: break;");
				}
				EndBlock();

				Line("throw new Exception(string.Format(\"{0} not a valid identifier\", entityInstance._Identifier));");
			}
			EndBlock();
		}
		EndBlock();

		Line("");
		Line("#pragma warning restore");

		Output("MugEngine", "GameObjectFactory");
	}

	void GenCaseForEntity(EntityDefinition e)
	{
		Line(string.Format("case \"{0}\":", e.Identifier));
		StartBlock();
		{
			string dataIdentifier = string.Format("{0}LDtkData", e.Identifier);
			Line(string.Format("{0} data = levelData.GetEntityFromInstance<{0}>(entityInstance);", dataIdentifier));
			Line("data.Position += overridePos;");
			Line("");
			Line(string.Format("return new {0}(data);", e.Identifier));
		}
		EndBlock();
		Line("break;");
	}
}
