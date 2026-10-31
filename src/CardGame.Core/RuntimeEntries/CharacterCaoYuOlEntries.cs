#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> _resolve;
    private CharacterCaoYuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoYuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoYuOlRuntime> resolve) => new(resolve);
}
