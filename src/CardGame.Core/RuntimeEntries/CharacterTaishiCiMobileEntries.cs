#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileRuntime> _resolve;
    private CharacterTaishiCiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileRuntime> resolve) => new(resolve);
}
