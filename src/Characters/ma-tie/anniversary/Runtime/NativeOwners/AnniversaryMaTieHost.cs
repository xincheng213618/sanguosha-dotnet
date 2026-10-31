#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMaTieHost(Func<RuntimeEntries.CharacterMaTieAnniversaryEntries> getCharacterMaTieAnniversaryRuntime) : IAnniversaryMaTieHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian => getCharacterMaTieAnniversaryRuntime().BeginMaTie(f),
        SkillProgramEffectOp.ResolveAnniversaryMaTieQuxianReward => getCharacterMaTieAnniversaryRuntime().BeginMaTieReward(f),
        SkillProgramEffectOp.RecordAnniversaryMaTieZhuiji => getCharacterMaTieAnniversaryRuntime().RecordMaTieZhuiji(f),
        _ => throw new InvalidOperationException("Unknown Ma Tie operation.")};
}
