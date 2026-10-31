#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanYanBoundaryMobileEntries
{
    internal CardUseFrame? BoundaryYanYanClaimUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool settled = false) => _resolve().BoundaryYanYanClaimUse(c, w, settled);
    internal long BoundaryYanYanEpoch(int owner) => _resolve().BoundaryYanYanEpoch(owner);
    internal bool BoundaryYanYanIssuedClaimMatches(CardUseFrame use, BoundaryYanYanClaim p) => _resolve().BoundaryYanYanIssuedClaimMatches(use, p);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryYanYanPublicStates(CharacterState owner, string skillId) => _resolve().BoundaryYanYanPublicStates(owner, skillId);
    internal int BoundaryYanYanSlashBonus(int owner) => _resolve().BoundaryYanYanSlashBonus(owner);
    internal bool CanOfferBoundaryYanYan(ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryYanYan(gate);
    internal SkillProgramStepOutcome ClaimBoundaryYanYan(ProgramSkillFrame f) => _resolve().ClaimBoundaryYanYan(f);
    internal SkillProgramStepOutcome FinishBoundaryYanYan(ProgramSkillFrame f) => _resolve().FinishBoundaryYanYan(f);
    internal bool HasBoundaryYanYanHe(int seat) => _resolve().HasBoundaryYanYanHe(seat);
    internal bool IsBoundaryYanYanClaimedEntity(long useId, int card) => _resolve().IsBoundaryYanYanClaimedEntity(useId, card);
    internal bool IssuedBoundaryYanYanClaim(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IssuedBoundaryYanYanClaim(c, w);
    internal bool KeepPaidBoundaryYanYan(ProgramSkillFrame f) => _resolve().KeepPaidBoundaryYanYan(f);
    internal void ObserveBoundaryYanYan(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates) => _resolve().ObserveBoundaryYanYan(action, window, candidates);
    internal SkillProgramStepOutcome PrepareBoundaryYanYan(ProgramSkillFrame f) => _resolve().PrepareBoundaryYanYan(f);
    internal bool ReturnBoundaryYanYanClaim(ProgramSkillFrame f) => _resolve().ReturnBoundaryYanYanClaim(f);
    internal bool ValidBoundaryYanYanOccurrence(int owner, string instance, ProgramSkillWindowContext context, scoped out BoundaryYanYanOccurrence occurrence) => _resolve().ValidBoundaryYanYanOccurrence(owner, instance, context, out occurrence);
    internal bool ValidBoundaryYanYanPrepared(ProgramSkillFrame f) => _resolve().ValidBoundaryYanYanPrepared(f);
}
