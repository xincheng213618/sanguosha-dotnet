#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed partial class CharacterSunJianMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _resolve;
    private CharacterSunJianMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJianMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> resolve) => new(resolve);
}
