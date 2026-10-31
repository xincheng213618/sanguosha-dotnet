#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTaishiCiMouOlRuntime))]
internal sealed partial class CharacterTaishiCiMouOlEntries
{
    private readonly Func<EngineOwners.CharacterTaishiCiMouOlRuntime> _resolve;
    private CharacterTaishiCiMouOlEntries(Func<EngineOwners.CharacterTaishiCiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMouOlEntries Create(Func<EngineOwners.CharacterTaishiCiMouOlRuntime> resolve) => new(resolve);
}
