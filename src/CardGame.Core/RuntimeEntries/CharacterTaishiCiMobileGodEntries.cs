#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> _resolve;
    private CharacterTaishiCiMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> resolve) => new(resolve);
}
