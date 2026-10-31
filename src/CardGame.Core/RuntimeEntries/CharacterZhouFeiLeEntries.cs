#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFeiLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _resolve;
    private CharacterZhouFeiLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFeiLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> resolve) => new(resolve);
}
