#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanYanBoundaryMobileEntries
{
    internal void AssertBoundaryYanYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryYanYan(f);
    internal bool BoundaryYanYanClaimMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().BoundaryYanYanClaimMovement(f, effect, p);
    internal global::CardGame.Core.CardUseFrame? BoundaryYanYanClaimUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w, bool settled = false) => _resolve().BoundaryYanYanClaimUse(c, w, settled);
    internal long BoundaryYanYanEpoch(int owner) => _resolve().BoundaryYanYanEpoch(owner);
    internal bool BoundaryYanYanIssuedClaimMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.BoundaryYanYanClaim p) => _resolve().BoundaryYanYanIssuedClaimMatches(use, p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryYanYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().BoundaryYanYanPublicStates(owner, skillId);
    internal int BoundaryYanYanSlashBonus(int owner) => _resolve().BoundaryYanYanSlashBonus(owner);
    internal bool CanOfferBoundaryYanYan(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryYanYan(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimBoundaryYanYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ClaimBoundaryYanYan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome FinishBoundaryYanYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBoundaryYanYan(f);
    internal bool HasBoundaryYanYanHe(int seat) => _resolve().HasBoundaryYanYanHe(seat);
    internal bool IsBoundaryYanYanClaimedEntity(long useId, int card) => _resolve().IsBoundaryYanYanClaimedEntity(useId, card);
    internal bool IssuedBoundaryYanYanClaim(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IssuedBoundaryYanYanClaim(c, w);
    internal bool KeepPaidBoundaryYanYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidBoundaryYanYan(f);
    internal void ObserveBoundaryYanYan(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => _resolve().ObserveBoundaryYanYan(action, window, candidates);
    internal global::CardGame.Core.SkillProgramStepOutcome PrepareBoundaryYanYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepareBoundaryYanYan(f);
    internal bool ReturnBoundaryYanYanClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryYanYanClaim(f);
    internal bool ValidBoundaryYanYanOccurrence(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.BoundaryYanYanOccurrence occurrence) => _resolve().ValidBoundaryYanYanOccurrence(owner, instance, context, out occurrence);
    internal bool ValidBoundaryYanYanPrepared(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryYanYanPrepared(f);
}
