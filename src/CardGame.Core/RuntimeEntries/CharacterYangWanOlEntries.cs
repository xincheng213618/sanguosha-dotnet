#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime))]
internal sealed partial class CharacterYangWanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> _resolve;
    private CharacterYangWanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangWanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> resolve) => new(resolve);
}
