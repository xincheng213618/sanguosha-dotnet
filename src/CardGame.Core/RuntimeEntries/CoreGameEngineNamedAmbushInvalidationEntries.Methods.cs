#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationEntries
{
    internal void InvalidateNamedAmbushUse(ProgramSkillFrame f) => _resolve().InvalidateNamedAmbushUse(f);
}
