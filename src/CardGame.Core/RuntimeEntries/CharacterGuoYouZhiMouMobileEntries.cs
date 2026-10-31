#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime))]
internal sealed partial class CharacterGuoYouZhiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> _resolve;
    private CharacterGuoYouZhiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoYouZhiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> resolve) => new(resolve);
}
