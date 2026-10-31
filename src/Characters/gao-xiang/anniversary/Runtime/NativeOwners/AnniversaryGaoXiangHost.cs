#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGaoXiangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGaoXiangAnniversaryRuntime>> getCharacterGaoXiangAnniversaryRuntime) : IAnniversaryGaoXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterGaoXiangAnniversaryRuntime().Value.BeginGaoXiang(f);
}
