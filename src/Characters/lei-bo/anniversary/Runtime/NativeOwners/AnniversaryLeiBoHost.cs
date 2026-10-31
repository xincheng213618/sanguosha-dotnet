#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLeiBoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLeiBoAnniversaryRuntime>> getCharacterLeiBoAnniversaryRuntime) : IAnniversaryLeiBoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeAnniversaryLeiBoSilei => getCharacterLeiBoAnniversaryRuntime().Value.InitializeLeiBo(f),
        SkillProgramEffectOp.RecordAnniversaryLeiBoSileiLoot => getCharacterLeiBoAnniversaryRuntime().Value.RecordLeiBoLoot(f),
        SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => getCharacterLeiBoAnniversaryRuntime().Value.RequestLeiBoSlash(f),
        SkillProgramEffectOp.SettleAnniversaryLeiBoSileiPenalty => getCharacterLeiBoAnniversaryRuntime().Value.SettleLeiBoPenalty(f),
        SkillProgramEffectOp.BeginAnniversaryLeiBoShuaijie => getCharacterLeiBoAnniversaryRuntime().Value.BeginLeiBoShuaijie(f),
        SkillProgramEffectOp.FinishAnniversaryLeiBoShuaijie => getCharacterLeiBoAnniversaryRuntime().Value.FinishLeiBoShuaijie(f),
        _ => throw new InvalidOperationException("Unknown Lei Bo operation.")};
}
