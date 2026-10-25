using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileSunHaoDrawIssuedEvent(long FrameId, CardConversionSource Source, string Hash, int ActualTurn, int TurnOwner, long WindowId, int X) : IGameEvent;
public sealed record MobileSunHaoDrawReturnedEvent(long FrameId, long? AttemptId, long Before, long After, int Drawn, int Gained) : IGameEvent;
public sealed record MobileSunHaoDrawReceipt(CardConversionSource Source, string Hash, int ActualTurn, int TurnOwner, long WindowId, int X,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] NativeDrawOutcome? Outcome = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileSunHaoDrawReceipt? MobileSunHaoDraw { get; init; }
}

public sealed partial class GameEngine
{
    private const string MobileSunHaoDrawReason = "skill-program.mobile:sun-hao-canshi.draw";

    // This is deliberately a population query local to Canshi. It never changes
    // IsRuleWounded, real HP, recovery space or any unrelated wounded predicate.
    private bool HasQualifiedMobileGuiming(CharacterState owner)
    {
        if (!owner.IsAlive || owner.IdentityConcealed || !SunHaoActualTurnOpen || owner.Seat != _currentSeat) return false;
        var grants = owner.SkillGrants.Grants.Where(g => _contentRegistry.GetSkill(g.SkillId).Program?.CardPolicies.Any(p =>
            p.Kind == SkillProgramCardPolicyKind.OtherWuTreatedWoundedForCanshi) == true).ToArray();
        if (grants.Length == 0) return false;
        var suppressors = PrivateGeneralLibrarySuppressionInputs(owner);
        return grants.Any(g => LibraryBaseGrantLive(owner,g) && IsCurrentTurnSkillGrantQualified(owner,g) && IsProjectedGrantQualified(owner,g) &&
            IsGeneralLibraryGrantQualified(owner,g) && IsHpConditionalGrantQualified(owner,g) && IsEquipmentSuitGrantQualified(owner,g) &&
            (suppressors.Count == 0 || suppressors.Contains(g.SkillId) || g.SourceId.StartsWith("equipment:",StringComparison.Ordinal)) &&
            HasSkillRoleQualification(owner,g.SkillId,g.SkillInstanceId,Role.Lord));
    }

    private SkillProgramStepOutcome BeginMobileSunHaoDraw(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is not null || frame.NativeDraw is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Mobile Canshi requires one clean original draw instruction.");
        var guiming = HasQualifiedMobileGuiming(_players[frame.OwnerSeat]);
        var count = _players.Count(p => p.IsAlive && (IsRuleWounded(p) || guiming && p.Seat != frame.OwnerSeat && p.HasHpValue &&
            !p.IdentityConcealed && GetEffectiveFactionId(p) == "wu"));
        var receipt = new MobileSunHaoDrawReceipt(SunHaoSource(frame), frame.GameplayHash, _turnNumber, _currentSeat, context.ParentFrameId, count);
        ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = receipt });
        AdvanceEventRulesAndQueueFact(new MobileSunHaoDrawIssuedEvent(frame.Id,receipt.Source,receipt.Hash,receipt.ActualTurn,receipt.TurnOwner,receipt.WindowId,receipt.X));
        return BeginOperationProgramDraw(frame, frame.OwnerSeat, count, new(MobileSunHaoDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation)]
    private void ReturnMobileSunHaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.MobileSunHaoDraw is not { Outcome: null } receipt || frame.NativeDraw is not null ||
            outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != receipt.X || outcome.SequenceAfter != _movementSequence)
            throw new InvalidOperationException("Mobile Canshi lost its once-issued exact native draw return.");
        ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = receipt with { Outcome = outcome }, PendingMovementContinuation = new(frame.OwnerSeat,0,null) });
        AdvanceEventRulesAndQueueFact(new MobileSunHaoDrawReturnedEvent(frame.Id,outcome.AttemptFrameId,outcome.SequenceBefore,outcome.SequenceAfter,outcome.ActualDrawIds.Count,outcome.ReplacementGainedIds.Count));
        ContinueMobileSunHaoDraw(frame.Id);
    }

    private void ContinueMobileSunHaoDraw(long id)
    {
        var frame = GetActiveProgramFrame(id);
        AssertMobileSunHaoDraw(frame);
        if (frame.MobileSunHaoDraw is not { Outcome: not null }) return;
        if (DrainSunHao(frame)) return;
        frame = GetActiveProgramFrame(id);
        var receipt = frame.MobileSunHaoDraw!;
        if (ProgramEventHistory<SunHaoCanshiGrantedEvent>().Any(e => e.FrameId == id))
            throw new InvalidOperationException("A mobile Canshi replacement cannot issue the same obligation twice.");
        AdvanceEventRulesAndQueueFact(new SunHaoCanshiGrantedEvent(frame.Id,receipt.Source,receipt.Hash,receipt.ActualTurn,receipt.TurnOwner,receipt.WindowId,receipt.X));
        ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = null, PendingMovementContinuation = null });
        FinishProgramSkill(frame,true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume,80400)]
    private bool ResumeMobileSunHaoDraw(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileSunHaoDraw: not null } frame || frame.Id != id) return false;
        ContinueMobileSunHaoDraw(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement,80400)]
    private bool ReturnMobileSunHaoDrawMovement(ProgramSkillFrame frame)
    { if (frame.MobileSunHaoDraw is null) return false; ContinueMobileSunHaoDraw(frame.Id); return true; }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,80400)]
    private void AssertMobileSunHaoDraw(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is not { } receipt) return;
        if (frame.SkillId != SunHaoComposition.MobileCanshi || frame.TriggerId != "normal-draw-replace-wounded" || frame.ActivationId != frame.TriggerId ||
            frame.InstructionIndex != 1 || frame.SunHaoCanshi is not null || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 ||
            receipt.Source != SunHaoSource(frame) || receipt.Hash != frame.GameplayHash || receipt.ActualTurn != _turnNumber || receipt.TurnOwner != _currentSeat ||
            receipt.X < 0 || receipt.X > _players.Count || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context ||
            context.ParentFrameId != receipt.WindowId || context.OwnerSeat != frame.OwnerSeat ||
            _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == receipt.WindowId) is not { NormalDrawReplaced: false, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } ||
            ProgramEventHistory<MobileSunHaoDrawIssuedEvent>().Where(e => e.FrameId == frame.Id).ToArray() is not [var issued] ||
            issued != new MobileSunHaoDrawIssuedEvent(frame.Id,receipt.Source,receipt.Hash,receipt.ActualTurn,receipt.TurnOwner,receipt.WindowId,receipt.X))
            throw new InvalidOperationException("Mobile Canshi lost its original qualified source, frozen population, draw phase or issuing frame.");
        if (receipt.Outcome is null)
        {
            if (frame.PendingMovementContinuation is not null || frame.NativeDraw is not { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, Operation: SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation } draw ||
                draw.InstructionIndex != frame.InstructionIndex || draw.RequestedCount != receipt.X || draw.Reason.Value != MobileSunHaoDrawReason || !draw.Recipients.SequenceEqual([frame.OwnerSeat]) ||
                ProgramEventHistory<MobileSunHaoDrawReturnedEvent>().Any(e => e.FrameId == frame.Id))
                throw new InvalidOperationException("Mobile Canshi lost its unpaid native draw child.");
            return;
        }
        var result = receipt.Outcome;
        if (frame.NativeDraw is not null || frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != frame.OwnerSeat ||
            result.RecipientSeat != frame.OwnerSeat || result.RequestedCount != receipt.X || result.SequenceBefore < 0 || result.SequenceAfter < result.SequenceBefore || result.SequenceAfter > _movementSequence ||
            result.ActualDrawIds.Count > receipt.X || result.ActualDrawIds.Distinct().Count() != result.ActualDrawIds.Count || result.Replacement is not null && result.ActualDrawIds.Count != 0 ||
            !_cardMovements.Where(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && m.From == CardLocation.DrawPile &&
                m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == MobileSunHaoDrawReason).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(result.ActualDrawIds) ||
            ProgramEventHistory<MobileSunHaoDrawReturnedEvent>().Where(e => e.FrameId == frame.Id).ToArray() is not [var returned] ||
            returned != new MobileSunHaoDrawReturnedEvent(frame.Id,result.AttemptFrameId,result.SequenceBefore,result.SequenceAfter,result.ActualDrawIds.Count,result.ReplacementGainedIds.Count) ||
            result.AttemptFrameId is { } attempt && ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frame.Id &&
                e.RecipientSeat == frame.OwnerSeat && e.RequestedCount == receipt.X && e.Replaced == (result.Replacement is not null)) != 1)
            throw new InvalidOperationException("Mobile Canshi lost its once-returned physical draw outcome and child cursor.");
        if (result.AttemptFrameId is { } request)
        {
            if (ProgramEventHistory<NativeDrawAttemptStartedEvent>().Where(e => e.Context.AttemptFrameId == request).ToArray() is not [var start] ||
                start.Context.ParentFrameId != frame.Id || start.Context.RecipientSeat != frame.OwnerSeat || start.Context.RequestedCount != receipt.X ||
                start.Context.Reason.Value != MobileSunHaoDrawReason || start.Context.ActualTurnNumber != receipt.ActualTurn || start.Context.ActualTurnOwnerSeat != receipt.TurnOwner ||
                start.Context.SequenceBefore > result.SequenceBefore || start.Return != new NativeDrawAttemptReturn(NativeDrawAttemptReturnKind.ProgramOperation,frame.InstructionIndex,0))
                throw new InvalidOperationException("Mobile Canshi retained an outcome from a foreign native draw request.");
        }
        else if (result.Replacement is not null || ProgramEventHistory<NativeDrawAttemptStartedEvent>().Any(e => e.Context.ParentFrameId == frame.Id))
            throw new InvalidOperationException("Mobile Canshi cannot invent or omit an accepted native draw request.");
    }

    private bool IsMobileSunHaoDrawMovement(ProgramSkillFrame frame, CardMovementRecord movement)
    {
        if (frame.MobileSunHaoDraw?.Outcome is not { } result || !_cardMovements.Contains(movement) || movement.To != CardLocation.Hand(frame.OwnerSeat)) return false;
        return result.ActualDrawIds.Contains(movement.CardId) && movement.Sequence > result.SequenceBefore && movement.Sequence <= result.SequenceAfter &&
            movement.From == CardLocation.DrawPile && movement.Reason.Value == MobileSunHaoDrawReason ||
            result.Replacement is { } replacement && replacement.GainedIds.Contains(movement.CardId) && movement.Sequence > replacement.SequenceBefore &&
            movement.Sequence <= replacement.SequenceAfter && movement.Reason == replacement.Reason;
    }
    private bool MobileSunHaoDrawFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.MobileSunHaoDraw is not { Outcome: not null }) return false;
        AssertMobileSunHaoDraw(frame);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is CardsMovedTriggerWindowFrame moved && ProgramMovementReturnMatches(frame,moved) && moved.Batch.Movements.Count > 0 &&
            moved.Batch.Movements.All(m => IsMobileSunHaoDrawMovement(frame,m));
    }
}
