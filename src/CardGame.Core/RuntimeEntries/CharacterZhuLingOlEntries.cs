#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuLingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuLingOlRuntime> _resolve;
    private CharacterZhuLingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuLingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuLingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuLingOlRuntime> resolve) => new(resolve);
}
