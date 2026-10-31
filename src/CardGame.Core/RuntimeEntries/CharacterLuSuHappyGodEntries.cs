#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuHappyGodRuntime> _resolve;
    private CharacterLuSuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuHappyGodRuntime> resolve) => new(resolve);
}
