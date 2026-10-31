#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMobileRuntime> _resolve;
    private CharacterGuoHuaiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMobileRuntime> resolve) => new(resolve);
}
