#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterNiuFuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuFuOlRuntime> _resolve;
    private CharacterNiuFuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterNiuFuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuFuOlRuntime> resolve) => new(resolve);
}
