#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouEnAnniversaryRuntime))]
internal sealed partial class CharacterXiahouEnAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXiahouEnAnniversaryRuntime> _resolve;
    private CharacterXiahouEnAnniversaryEntries(Func<EngineOwners.CharacterXiahouEnAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouEnAnniversaryEntries Create(Func<EngineOwners.CharacterXiahouEnAnniversaryRuntime> resolve) => new(resolve);
}
