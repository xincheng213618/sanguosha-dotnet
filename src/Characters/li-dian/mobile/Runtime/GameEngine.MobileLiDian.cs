namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileLiDianWangxiSkill = "mobile:wangxi";
    private static readonly CardMoveReason MobileLiDianWangxiDraw = new("skill.mobile-li-dian.wangxi.draw");
    [ProgramCapability]
    private sealed class MobileLiDianHost(GameEngine engine) : IMobileLiDianWangxiHost
    { public SkillProgramStepOutcome Draw(ProgramSkillFrame f) => engine.BeginMobileLiDianWangxi(f); }

    private bool ExactMobileLiDianDamage(ProgramSkillFrame f, out ProgramSkillWindowContext c)
    {
        c = f.WindowContext!;
        var parentId = c?.ParentFrameId;
        return f.SkillId == MobileLiDianWangxiSkill && f.InstructionIndex == 1 && c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0,
            SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } && source != target &&
            IsValidPlayerSeat(source) && IsValidPlayerSeat(target) && c.OwnerSeat == f.OwnerSeat &&
            (f.TriggerId == "taking-each-point" ? target == f.OwnerSeat : f.TriggerId == "dealing-each-point" && source == f.OwnerSeat) &&
            _resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == parentId) is { } window &&
            window.TriggerWindow == c.Window && window.ParentFrameId == damageId && window.SourceSeat == source && window.TargetSeat == target &&
            window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count &&
            MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) &&
            _resolutionStack.OfType<DamageFrame>().Any(d => d.Id == damageId && d.SourceSeat == source && d.TargetSeat == target);
    }
    private SkillProgramStepOutcome BeginMobileLiDianWangxi(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.MobileLiDianWangxi is not null || !ExactMobileLiDianDamage(f, out var c))
            throw new InvalidOperationException("Mobile Wangxi requires its original per-point participant damage candidate.");
        var source = c.SourceSeat!.Value; var target = c.TargetSeat!.Value;
        if (!_players[source].IsAlive || !_players[target].IsAlive) return SkillProgramStepOutcome.Continue;
        var recipients = new[] { source, target }.OrderBy(seat => RelativeTableOrder(_currentSeat, seat)).ToArray();
        ReplaceRuntimeTop(f with { MobileLiDianWangxi = new(new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash,
            c.ParentFrameId, c.DamageFrameId!.Value, source, target, c.Amount, c.OccurrenceIndex, _turnNumber, _currentSeat, recipients) });
        ContinueMobileLiDianWangxi(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }
    private bool ValidMobileLiDianWangxi(ProgramSkillFrame f) => f.MobileLiDianWangxi is { } r && ExactMobileLiDianDamage(f, out var c) &&
        r.Source == new CardConversionSource(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && r.Hash == f.GameplayHash &&
        r.Hash == _contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash && r.WindowId == c.ParentFrameId && r.DamageId == c.DamageFrameId &&
        r.SourceSeat == c.SourceSeat && r.TargetSeat == c.TargetSeat && r.Amount == c.Amount && r.OccurrenceIndex == c.OccurrenceIndex &&
        IsValidPlayerSeat(r.TurnSeat) && r.TurnNumber >= 0 && r.RecipientIndex is >= 0 and <= 2 &&
        r.Recipients.SequenceEqual(new[] { r.SourceSeat, r.TargetSeat }.OrderBy(seat => RelativeTableOrder(r.TurnSeat, seat))) &&
        (r.AwaitingMovement ? r.Draw is { RequestedCount: 1 } draw && r.RecipientIndex < 2 && draw.RecipientSeat == r.Recipients[r.RecipientIndex] &&
            draw.SequenceBefore >= 0 && draw.SequenceAfter >= draw.SequenceBefore && draw.ActualDrawIds.Count <= 1 &&
            (draw.Replacement is null || draw.ActualDrawIds.Count == 0) : r.Draw is null);

    private void ContinueMobileLiDianWangxi(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileLiDianWangxi: { } r } f || f.Id != id) return;
        if (!ValidMobileLiDianWangxi(f)) throw new InvalidOperationException("Mobile Wangxi lost its frozen seating order and original per-point damage receipt.");
        if (r.AwaitingMovement)
        {
            if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
                TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(id) ||
                TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id)) return;
            ReplaceRuntimeTop(f = GetActiveProgramFrame(id) with { MobileLiDianWangxi = r = r with { RecipientIndex = r.RecipientIndex + 1, AwaitingMovement = false, Draw = null }, PendingMovementContinuation = null });
        }
        while (r.RecipientIndex < 2 && !_players[r.Recipients[r.RecipientIndex]].IsAlive)
            ReplaceRuntimeTop(f = f with { MobileLiDianWangxi = r = r with { RecipientIndex = r.RecipientIndex + 1 } });
        if (r.RecipientIndex == 2)
        {
            ReplaceRuntimeTop(f with { MobileLiDianWangxi = null }); AdvanceRuntimeProgram(id); return;
        }
        BeginOperationProgramDraw(f, r.Recipients[r.RecipientIndex], 1, MobileLiDianWangxiDraw);
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawMobileLiDianWangxi)]
    private void ReturnMobileLiDianWangxiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidMobileLiDianWangxi(f) || f.MobileLiDianWangxi is not { AwaitingMovement: false, Draw: null } r || r.RecipientIndex >= 2 ||
            outcome.RecipientSeat != r.Recipients[r.RecipientIndex] || outcome.RequestedCount != 1 || outcome.ActualDrawIds.Count > 1 ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !_cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter &&
                m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == MobileLiDianWangxiDraw)
                .Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Mobile Wangxi cannot accept another recipient's or a repeated native draw return.");
        ReplaceRuntimeTop(f = f with { MobileLiDianWangxi = r with { AwaitingMovement = true, Draw = outcome }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        ContinueMobileLiDianWangxi(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 71010)]
    private bool ResumeMobileLiDianWangxi(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileLiDianWangxi.AwaitingMovement: true } f || f.Id != id) return false;
        ContinueMobileLiDianWangxi(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 71010)]
    private bool ReturnMobileLiDianWangxiMovement(ProgramSkillFrame f)
    {
        if (f.MobileLiDianWangxi is not { AwaitingMovement: true }) return false;
        ContinueMobileLiDianWangxi(f.Id); return true;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 71010)]
    private bool ProveMobileLiDianWangxiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) =>
        e?.Op == SkillProgramEffectOp.DrawMobileLiDianWangxi && ValidMobileLiDianWangxi(f) &&
        f.MobileLiDianWangxi is { AwaitingMovement: true, Draw: { } draw } r && r.RecipientIndex < 2 &&
        pending == f.PendingMovementContinuation && pending.SubjectSeat == r.Recipients[r.RecipientIndex] && pending.SubjectSeat == draw.RecipientSeat &&
        pending.BeforeCount == 0 && pending.CoverageResultBind is null && draw.RequestedCount == 1 && draw.SequenceAfter >= draw.SequenceBefore &&
        draw.ActualDrawIds.All(id => _cardMovements.Any(m => m.CardId == id && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(draw.RecipientSeat) && m.Reason == MobileLiDianWangxiDraw));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 71010)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 71010)]
    private bool ProveMobileLiDianWangxiRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) =>
        f.MobileLiDianWangxi is { AwaitingMovement: true, Draw.Replacement: not null } && ValidMobileLiDianWangxi(f) &&
        RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 71010)]
    private void AssertMobileLiDianWangxi(ProgramSkillFrame f)
    { if (f.MobileLiDianWangxi is not null && !ValidMobileLiDianWangxi(f)) throw new InvalidOperationException("Mobile Wangxi changed its original participant pair, seating order or damage occurrence."); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 71010)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 71010)]
    private bool KeepPaidMobileLiDianWangxi(ProgramSkillFrame f) => f.MobileLiDianWangxi is not null && ValidMobileLiDianWangxi(f);
}
