#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDaQiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoMouMobileEntries> getCharacterDaQiaoMouMobileRuntime) : IMouDaQiaoHost, IMouDaQiaoQuery
{
    public int Limit() => getCharacterDaQiaoMouMobileRuntime().MouDaQiaoGuoseLimit;
    public bool Allows(int owner, int? card, int target, string branch) => getCharacterDaQiaoMouMobileRuntime().MouDaQiaoGuoseTarget(owner, card, target, branch);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouDaQiaoGuose or SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove => getCharacterDaQiaoMouMobileRuntime().BeginMouDaQiaoGuose(f),
        SkillProgramEffectOp.ResolveMouDaQiaoLiuliHeart => getCharacterDaQiaoMouMobileRuntime().ResolveMouDaQiaoHeart(f),
        _ => getCharacterDaQiaoMouMobileRuntime().BeginMouDaQiaoLiuliPhase(f)};
}
