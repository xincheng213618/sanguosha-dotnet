#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanMouMobileRuntime> _resolve;
    private CharacterDiaoChanMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanMouMobileRuntime> resolve) => new(resolve);
}
