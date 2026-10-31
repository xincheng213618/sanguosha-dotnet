#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLeiBoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLeiBoAnniversaryEntries> getCharacterLeiBoAnniversaryRuntime) : IAnniversaryLeiBoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeAnniversaryLeiBoSilei => getCharacterLeiBoAnniversaryRuntime().InitializeLeiBo(f),
        SkillProgramEffectOp.RecordAnniversaryLeiBoSileiLoot => getCharacterLeiBoAnniversaryRuntime().RecordLeiBoLoot(f),
        SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => getCharacterLeiBoAnniversaryRuntime().RequestLeiBoSlash(f),
        SkillProgramEffectOp.SettleAnniversaryLeiBoSileiPenalty => getCharacterLeiBoAnniversaryRuntime().SettleLeiBoPenalty(f),
        SkillProgramEffectOp.BeginAnniversaryLeiBoShuaijie => getCharacterLeiBoAnniversaryRuntime().BeginLeiBoShuaijie(f),
        SkillProgramEffectOp.FinishAnniversaryLeiBoShuaijie => getCharacterLeiBoAnniversaryRuntime().FinishLeiBoShuaijie(f),
        _ => throw new InvalidOperationException("Unknown Lei Bo operation.")};
}
