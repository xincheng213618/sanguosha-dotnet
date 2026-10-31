#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangChangPuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangChangPuOlRuntime> _resolve;
    private CharacterZhangChangPuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangChangPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangChangPuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangChangPuOlRuntime> resolve) => new(resolve);
}
