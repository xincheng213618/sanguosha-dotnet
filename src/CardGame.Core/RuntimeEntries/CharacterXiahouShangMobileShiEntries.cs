#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouShangMobileShiRuntime))]
internal sealed partial class CharacterXiahouShangMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterXiahouShangMobileShiRuntime> _resolve;
    private CharacterXiahouShangMobileShiEntries(Func<EngineOwners.CharacterXiahouShangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouShangMobileShiEntries Create(Func<EngineOwners.CharacterXiahouShangMobileShiRuntime> resolve) => new(resolve);
}
