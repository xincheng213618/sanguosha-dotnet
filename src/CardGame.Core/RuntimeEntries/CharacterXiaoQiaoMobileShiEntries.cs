#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiaoQiaoMobileShiRuntime))]
internal sealed partial class CharacterXiaoQiaoMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterXiaoQiaoMobileShiRuntime> _resolve;
    private CharacterXiaoQiaoMobileShiEntries(Func<EngineOwners.CharacterXiaoQiaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoMobileShiEntries Create(Func<EngineOwners.CharacterXiaoQiaoMobileShiRuntime> resolve) => new(resolve);
}
