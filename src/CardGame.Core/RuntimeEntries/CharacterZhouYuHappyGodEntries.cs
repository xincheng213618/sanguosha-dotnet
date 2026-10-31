#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuHappyGodRuntime> _resolve;
    private CharacterZhouYuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuHappyGodRuntime> resolve) => new(resolve);
}
