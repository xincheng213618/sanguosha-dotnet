#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime))]
internal sealed partial class CharacterGuoHuaiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> _resolve;
    private CharacterGuoHuaiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve) => new(resolve);
}
