#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangWanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> _resolve;
    private CharacterYangWanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangWanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangWanOlRuntime> resolve) => new(resolve);
}
