#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _resolve;
    private CharacterYangFuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangFuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> resolve) => new(resolve);
}
