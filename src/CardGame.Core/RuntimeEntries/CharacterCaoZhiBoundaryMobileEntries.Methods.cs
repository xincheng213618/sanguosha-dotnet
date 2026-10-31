#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhiBoundaryMobileEntries
{
    internal int BoundaryCaoZhiGameDamage(int owner) => _resolve().BoundaryCaoZhiGameDamage(owner);
    internal void CaptureBoundaryCaoZhiDamageFace(global::CardGame.Core.DamageAppliedEvent applied) => _resolve().CaptureBoundaryCaoZhiDamageFace(applied);
    internal bool QualifyBoundaryCaoZhiChengzhang(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoZhiChengzhang(c, context);
    internal bool QualifyBoundaryCaoZhiDamage(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoZhiDamage(c, trigger, context);
    internal void SelectBoundaryCaoZhiTrick(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().SelectBoundaryCaoZhiTrick(supplied, bind);
    internal void UpgradeBoundaryCaoZhiJiushi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().UpgradeBoundaryCaoZhiJiushi(supplied);
    internal bool HasBoundaryCaoZhi { get => _resolve().HasBoundaryCaoZhi; }
}
