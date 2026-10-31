#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouHuiAnniversaryRuntime))]
internal sealed partial class CharacterXiahouHuiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXiahouHuiAnniversaryRuntime> _resolve;
    private CharacterXiahouHuiAnniversaryEntries(Func<EngineOwners.CharacterXiahouHuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouHuiAnniversaryEntries Create(Func<EngineOwners.CharacterXiahouHuiAnniversaryRuntime> resolve) => new(resolve);
}
