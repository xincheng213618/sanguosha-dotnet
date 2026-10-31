#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXingDaoRongAnniversaryRuntime))]
internal sealed partial class CharacterXingDaoRongAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXingDaoRongAnniversaryRuntime> _resolve;
    private CharacterXingDaoRongAnniversaryEntries(Func<EngineOwners.CharacterXingDaoRongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXingDaoRongAnniversaryEntries Create(Func<EngineOwners.CharacterXingDaoRongAnniversaryRuntime> resolve) => new(resolve);
}
