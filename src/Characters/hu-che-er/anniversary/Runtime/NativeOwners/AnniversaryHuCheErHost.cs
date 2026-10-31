#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHuCheErHost(Func<RuntimeEntries.CharacterHuCheErAnniversaryEntries> getCharacterHuCheErAnniversaryRuntime) : IAnniversaryHuCheErHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveAnniversaryHuCheErDaoji => getCharacterHuCheErAnniversaryRuntime().BeginHuCheEr(f),
        SkillProgramEffectOp.RecordAnniversaryHuCheErFuzhongGain => getCharacterHuCheErAnniversaryRuntime().RecordHuCheErGain(f),
        SkillProgramEffectOp.SpendAnniversaryHuCheErFuzhong => getCharacterHuCheErAnniversaryRuntime().SpendHuCheErFour(f),
        _ => throw new InvalidOperationException("Unknown Hu Che Er operation.")};
}
