#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoTengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoTengOlRuntime> _resolve;
    private CharacterCaoTengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoTengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoTengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoTengOlRuntime> resolve) => new(resolve);
}
