#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterTaishiCiMouOlRuntime))]
internal sealed partial class CharacterTaishiCiMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMouOlRuntime> _resolve;
    private CharacterTaishiCiMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMouOlRuntime> resolve) => new(resolve);
}
