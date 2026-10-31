#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime))]
internal sealed partial class CharacterZhongHuiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime> _resolve;
    private CharacterZhongHuiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongHuiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiMobileShiRuntime> resolve) => new(resolve);
}
