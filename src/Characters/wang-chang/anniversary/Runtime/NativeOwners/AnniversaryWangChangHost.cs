#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryWangChangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangChangAnniversaryRuntime>> getCharacterWangChangAnniversaryRuntime) : IAnniversaryWangChangHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWangChangAnniversaryRuntime().Value.BeginWangChang(f, op);
}
