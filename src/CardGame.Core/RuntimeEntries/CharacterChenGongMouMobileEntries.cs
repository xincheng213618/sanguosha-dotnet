#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenGongMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime> _resolve;
    private CharacterChenGongMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenGongMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenGongMouMobileRuntime> resolve) => new(resolve);
}
