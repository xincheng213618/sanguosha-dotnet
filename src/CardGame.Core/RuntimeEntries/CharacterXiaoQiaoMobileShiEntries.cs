#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime))]
internal sealed partial class CharacterXiaoQiaoMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime> _resolve;
    private CharacterXiaoQiaoMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMobileShiRuntime> resolve) => new(resolve);
}
