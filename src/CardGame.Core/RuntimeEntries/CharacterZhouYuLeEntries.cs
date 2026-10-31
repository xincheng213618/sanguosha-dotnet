#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> _resolve;
    private CharacterZhouYuLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> resolve) => new(resolve);
}
