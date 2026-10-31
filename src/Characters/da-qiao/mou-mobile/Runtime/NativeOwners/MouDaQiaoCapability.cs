#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDaQiaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDaQiaoMouMobileRuntime>> getCharacterDaQiaoMouMobileRuntime) : IMouDaQiaoHost, IMouDaQiaoQuery
{
    public int Limit() => getCharacterDaQiaoMouMobileRuntime().Value.MouDaQiaoGuoseLimit;
    public bool Allows(int owner, int? card, int target, string branch) => getCharacterDaQiaoMouMobileRuntime().Value.MouDaQiaoGuoseTarget(owner, card, target, branch);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouDaQiaoGuose or SkillProgramEffectOp.ResolveMouDaQiaoGuoseRemove => getCharacterDaQiaoMouMobileRuntime().Value.BeginMouDaQiaoGuose(f),
        SkillProgramEffectOp.ResolveMouDaQiaoLiuliHeart => getCharacterDaQiaoMouMobileRuntime().Value.ResolveMouDaQiaoHeart(f),
        _ => getCharacterDaQiaoMouMobileRuntime().Value.BeginMouDaQiaoLiuliPhase(f)};
}
