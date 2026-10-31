#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFengOlRuntime> _resolve;
    private CharacterYangFengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangFengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFengOlRuntime> resolve) => new(resolve);
}
