#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhangBoundaryMobileEntries
{
    internal global::CardGame.Core.BoundaryJiangchiIssuedEvent? ActiveBoundaryJiangchi(int owner) => _resolve().ActiveBoundaryJiangchi(owner);
    internal void AssertBoundaryJiangchiFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryJiangchiFrame(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> BoundaryCaoZhangPhaseContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().BoundaryCaoZhangPhaseContributions(player, query);
    internal bool BoundaryJiangchiCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().BoundaryJiangchiCardDistance(q);
    internal bool BoundaryJiangchiConvertedDistance(global::CardGame.Core.ProgramSlashConversionDistanceQuery q) => _resolve().BoundaryJiangchiConvertedDistance(q);
    internal bool BoundaryJiangchiSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().BoundaryJiangchiSlashDistance(q);
    internal bool BoundaryJiangchiUseBan(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().BoundaryJiangchiUseBan(q);
    internal bool CanOfferBoundaryCaoZhang(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CanOfferBoundaryCaoZhang(c);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteBoundaryCaoZhang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().ExecuteBoundaryCaoZhang(supplied, e);
    internal bool ValidBoundaryJiangchiPreparation(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.BoundaryJiangchiPreparation r) => _resolve().ValidBoundaryJiangchiPreparation(f, r);
}
