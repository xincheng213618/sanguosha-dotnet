#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryYanXiangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYanXiangAnniversaryRuntime>> getCharacterYanXiangAnniversaryRuntime) : IAnniversaryYanXiangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanXiangAnniversaryRuntime().Value.BeginYanXiang(f, op);
}
