#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QianZhaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime) : IQianZhaoHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveWeifuJudgment => getCharacterQianZhaoOlRuntime().BeginWeifu(f),
        SkillProgramEffectOp.OfferWeifuCategoryTargets => getCharacterQianZhaoOlRuntime().BeginWeifuTargets(f),
        SkillProgramEffectOp.ResolveKuansaiTargetChoice => getCharacterQianZhaoOlRuntime().BeginKuansai(f),
        _ => throw new InvalidOperationException("Unknown Qian Zhao native operation.")};
}
