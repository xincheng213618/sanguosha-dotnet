#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhiBoundaryMobileEntries
{
    internal int BoundaryCaoZhiGameDamage(int owner) => _resolve().BoundaryCaoZhiGameDamage(owner);
    internal void CaptureBoundaryCaoZhiDamageFace(DamageAppliedEvent applied) => _resolve().CaptureBoundaryCaoZhiDamageFace(applied);
    internal bool QualifyBoundaryCaoZhiChengzhang(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoZhiChengzhang(c, context);
    internal bool QualifyBoundaryCaoZhiDamage(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryCaoZhiDamage(c, trigger, context);
    internal void SelectBoundaryCaoZhiTrick(ProgramSkillFrame supplied, string bind) => _resolve().SelectBoundaryCaoZhiTrick(supplied, bind);
    internal void UpgradeBoundaryCaoZhiJiushi(ProgramSkillFrame supplied) => _resolve().UpgradeBoundaryCaoZhiJiushi(supplied);
    internal bool HasBoundaryCaoZhi { get => _resolve().HasBoundaryCaoZhi; }
}
