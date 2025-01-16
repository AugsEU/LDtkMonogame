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
		Line("using System.Collections.Generic;");
		Blank();
	}

	public void Generate()
	{
		GenHeaders();

		Line("public static class GameObjectFactory");
		StartBlock();
		{
			AddCustomDataCache();

			Line("public static MGameObject FromLDtkEntity(LDtkLevel levelData, LDtk.EntityInstance entityInstance, Vector2 basePosition)");
			StartBlock();
			{
				Line("CustomLevelData customData = GetCustomLevelData(levelData);");
				Line("");
				Line("Vector2 overridePos = basePosition;");
				Line("overridePos += entityInstance.Px.ToVector2();");

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
			Line("data.Position = overridePos;");
			Line("");
			Line(string.Format("return new {0}(data);", e.Identifier));
		}
		EndBlock();
		Line("break;");
	}

	void AddCustomDataCache()
	{
		string code = """"

	public static Dictionary<LDtkLevel, CustomLevelData> sCustomDataCache = new();
	public static void WarmCache(LDtkLevel levelData)
	{
		if (sCustomDataCache.ContainsKey(levelData))
		{
			return;
		}

		CustomLevelData customData = levelData.GetCustomFields<CustomLevelData>();
		sCustomDataCache.Add(levelData, customData);
	}

	public static CustomLevelData GetCustomLevelData(LDtkLevel levelData)
	{
		if (sCustomDataCache.TryGetValue(levelData, out CustomLevelData data))
		{
			return data;
		}

		CustomLevelData customData = levelData.GetCustomFields<CustomLevelData>();
		sCustomDataCache.Add(levelData, customData);

		return customData;
	}


"""";
		Line(code);
	}
}
