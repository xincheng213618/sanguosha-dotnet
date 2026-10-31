#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhangBoundaryMobileEntries
{
    internal BoundaryJiangchiIssuedEvent? ActiveBoundaryJiangchi(int owner) => _resolve().ActiveBoundaryJiangchi(owner);
    internal IEnumerable<RuleQueryContribution> BoundaryCaoZhangPhaseContributions(CharacterState player, SkillRuleQuery query) => _resolve().BoundaryCaoZhangPhaseContributions(player, query);
    internal bool BoundaryJiangchiCardDistance(ProgramCardDistanceQuery q) => _resolve().BoundaryJiangchiCardDistance(q);
    internal bool BoundaryJiangchiConvertedDistance(ProgramSlashConversionDistanceQuery q) => _resolve().BoundaryJiangchiConvertedDistance(q);
    internal bool BoundaryJiangchiSlashDistance(ProgramSlashDistanceQuery q) => _resolve().BoundaryJiangchiSlashDistance(q);
    internal bool BoundaryJiangchiUseBan(ProgramCardUseQuery q) => _resolve().BoundaryJiangchiUseBan(q);
    internal bool CanOfferBoundaryCaoZhang(ProgramTriggerCandidate c) => _resolve().CanOfferBoundaryCaoZhang(c);
    internal SkillProgramStepOutcome ExecuteBoundaryCaoZhang(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().ExecuteBoundaryCaoZhang(supplied, e);
    internal bool ValidBoundaryJiangchiPreparation(ProgramSkillFrame f, BoundaryJiangchiPreparation r) => _resolve().ValidBoundaryJiangchiPreparation(f, r);
}
