#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime))]
internal sealed partial class CharacterXinXianYingMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> _resolve;
    private CharacterXinXianYingMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXinXianYingMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve) => new(resolve);
}
