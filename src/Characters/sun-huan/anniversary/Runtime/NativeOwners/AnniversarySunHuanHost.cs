#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversarySunHuanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunHuanAnniversaryRuntime>> getCharacterSunHuanAnniversaryRuntime) : IAnniversarySunHuanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunHuanAnniversaryRuntime().Value.BeginSunHuan(f, op);
}
