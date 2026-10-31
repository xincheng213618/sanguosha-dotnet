#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiHappyGodRuntime> _resolve;
    private CharacterTaishiCiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiHappyGodRuntime> resolve) => new(resolve);
}
