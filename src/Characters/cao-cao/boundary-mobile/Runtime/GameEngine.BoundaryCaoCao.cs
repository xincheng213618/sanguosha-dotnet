namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundaryCaoCaoCapability(GameEngine engine) : IBoundaryCaoCaoHost
    {
        public SkillProgramStepOutcome ClaimDamageCards(ProgramSkillFrame f)
        {
            engine.ClaimProgramDamageCards(f.Id, f.OwnerSeat, new(BoundaryCaoCaoMobile.ClaimReason));
            return engine.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
        }
    }

    // The ordinary claim helper owns material/group accounting. Its committed
    // claim event and original damage window also prove this movement return;
    // no second pending object or card-use-keyed receipt is necessary.
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 58700)]
    private bool BoundaryCaoCaoClaimMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (effect?.Op != SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards || f.SkillId != BoundaryCaoCaoMobile.Jianxiong || f.InstructionIndex != 1 ||
            p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null ||
            BoundaryCaoCaoCommittedClaim(f) is not { } claim) return false;
        return _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == f.Id && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.OriginOwnerSeat == f.OwnerSeat &&
            batch.Movements.Count == claim.CardIds.Count && batch.Movements.All(m => claim.CardIds.Contains(m.CardId) && m.From == CardLocation.Processing &&
                m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryCaoCaoMobile.ClaimReason && _cardMovements.Contains(m)));
    }

    private ProgramDamageCardsClaimedEvent? BoundaryCaoCaoCommittedClaim(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryCaoCaoMobile.Jianxiong || f.InstructionIndex is < 1 or > 2 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.TargetSeat != f.OwnerSeat) return null;
        var index = _resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index < 2 || _resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != context.DamageFrameId ||
            window.TriggerWindow != context.Window || window.TargetSeat != f.OwnerSeat || window.SourceSeat != context.SourceSeat ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) ||
            _resolutionStack[index - 2] is not DamageFrame damage || damage.Id != context.DamageFrameId || damage.TargetSeat != f.OwnerSeat || damage.Amount != context.Amount) return null;
        var attack = _resolutionStack.SingleOrDefault(frame => frame.Id == damage.ParentFrameId) switch
        {
            ProgramSkillFrame { AttackAttempt: not null } => (IDamageAttempt)new ProgramAttackHandle(this, damage.ParentFrameId),
            ProgramSkillFrame { CardAttack.Active: true } or CardUseFrame { CardAttack.Active: true } or JudgmentFrame { CardAttack.Active: true } => new CardAttackHandle(this, damage.ParentFrameId),
            _ => null
        };
        if (attack is null || !attack.DamageWasApplied) return null;
        var claims = ProgramEventHistory<ProgramDamageCardsClaimedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var physical = attack.PhysicalCards.Select(c => c.Id).ToHashSet();
        return claims is [var claim] && claim.SkillId == f.SkillId && claim.BindingId == f.TriggerId && claim.OwnerSeat == f.OwnerSeat &&
            claim.CardIds.Count > 0 && claim.CardIds.Distinct().Count() == claim.CardIds.Count && claim.CardIds.All(id => physical.Contains(id) &&
                _cardMovements.Any(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryCaoCaoMobile.ClaimReason)) ? claim : null;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 58700)]
    private bool ContinueBoundaryCaoCaoPaidClaim(ProgramSkillFrame f) => BoundaryCaoCaoCommittedClaim(f) is not null;

    // The generic movement return requires a live skill before it reaches the
    // executor. This already-paid claim instead returns on its original native
    // damage proof, then the normal executor continues with Draw exactly once.
    [ProgramContinuation(ProgramContinuationKind.Movement, 58700)]
    private bool ReturnBoundaryCaoCaoPaidClaim(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryCaoCaoMobile.Jianxiong || f.InstructionIndex != 1 ||
            f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat)
            return false;
        if (BoundaryCaoCaoCommittedClaim(f) is null)
            throw new InvalidOperationException("Mobile Jianxiong movement return lost its original paid damage-card claim.");
        ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
