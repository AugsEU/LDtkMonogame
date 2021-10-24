#pragma warning disable CS1591
namespace LDtk.Generator
{
    public class LdtkCodeCustomizer
    {

        public virtual void CustomizeEntity(CompilationUnitClass entity, EntityDefinition ed, LdtkGeneratorContext ctx)
        {
            entity.Fields.Add(new CompilationUnitField("Uid", "long"));
            entity.Fields.Add(new CompilationUnitField("Identifier", "string"));
            entity.Fields.Add(new CompilationUnitField("Width", "float"));
            entity.Fields.Add(new CompilationUnitField("Height", "float"));
            entity.Fields.Add(new CompilationUnitField("Position", "Vector2"));
            entity.Fields.Add(new CompilationUnitField("Pivot", "Vector2"));
        }

        public virtual void CustomizeLevel(CompilationUnitClass level, LDtkWorld ldtkJson, LdtkGeneratorContext ctx)
        {
            // level.Fields.Add(new CompilationUnitField("Uid", "long"));
            // level.Fields.Add(new CompilationUnitField("Identifier", "string"));
            // level.Fields.Add(new CompilationUnitField("WorldCoords", "Vector2"));
            // level.Fields.Add(new CompilationUnitField("Width", "float"));
            // level.Fields.Add(new CompilationUnitField("Height", "float"));
            // level.Fields.Add(new CompilationUnitField("Entities", "object[]"));
        }

    }
}
#pragma warning restore CS1591