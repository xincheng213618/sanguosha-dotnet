#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouJieAnniversaryRuntime))]
internal sealed partial class CharacterXiahouJieAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXiahouJieAnniversaryRuntime> _resolve;
    private CharacterXiahouJieAnniversaryEntries(Func<EngineOwners.CharacterXiahouJieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouJieAnniversaryEntries Create(Func<EngineOwners.CharacterXiahouJieAnniversaryRuntime> resolve) => new(resolve);
}
