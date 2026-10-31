#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime))]
internal sealed partial class CharacterMaWanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> _resolve;
    private CharacterMaWanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaWanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> resolve) => new(resolve);
}
