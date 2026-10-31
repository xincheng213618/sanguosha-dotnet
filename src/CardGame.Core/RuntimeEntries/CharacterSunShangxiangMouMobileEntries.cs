#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime))]
internal sealed partial class CharacterSunShangxiangMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime> _resolve;
    private CharacterSunShangxiangMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShangxiangMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime> resolve) => new(resolve);
}
