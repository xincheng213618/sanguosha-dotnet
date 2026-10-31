#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> _resolve;
    private CharacterSimaYiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiHappyGodRuntime> resolve) => new(resolve);
}
