#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _resolve;
    private CharacterCaoXiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> resolve) => new(resolve);
}
