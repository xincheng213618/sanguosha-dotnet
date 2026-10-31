#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangOlRuntime> _resolve;
    private CharacterZhouFangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangOlRuntime> resolve) => new(resolve);
}
