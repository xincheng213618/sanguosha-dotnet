#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime))]
internal sealed partial class CharacterMengHuoMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> _resolve;
    private CharacterMengHuoMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> resolve) => new(resolve);
}
