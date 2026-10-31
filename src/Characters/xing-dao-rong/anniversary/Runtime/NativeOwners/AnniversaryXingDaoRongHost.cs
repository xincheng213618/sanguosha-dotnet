#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryXingDaoRongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXingDaoRongAnniversaryRuntime>> getCharacterXingDaoRongAnniversaryRuntime) : IAnniversaryXingDaoRongHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterXingDaoRongAnniversaryRuntime().Value.BeginXingDaoRong(f);
}
