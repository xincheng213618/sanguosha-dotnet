#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> _resolve;
    private CharacterLuBuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> resolve) => new(resolve);
}
