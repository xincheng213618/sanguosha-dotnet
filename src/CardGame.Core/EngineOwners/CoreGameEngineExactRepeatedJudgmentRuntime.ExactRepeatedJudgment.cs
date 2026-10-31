#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExactRepeatedJudgmentRuntime
{
    SkillProgramStepOutcome IRepeatedJudgmentClaimPolicyProgramHost.RepeatJudgmentWithClaimPolicy(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy) => BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
    internal SkillProgramStepOutcome BeginProgramExactRepeatedJudgment(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (policy is not (SkillProgramClaimHandLimitExemption.ActualTurn or SkillProgramClaimHandLimitExemption.None) || active.RepeatedJudgment is not null || suits.Count == 0 || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Exact repeated judgment requires its actual owner preparation turn.");
        active = active with
        {
            RepeatedJudgment = new ProgramRepeatedJudgment(reason, bind, Array.AsReadOnly(suits.ToArray()), 0)
            {
                ClaimHandLimitExemption = policy,
                ClaimTurn = new(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        return _coreProgramLifecycleOperations.StartProgramJudgment(active, frame.OwnerSeat, reason, bind, SkillProgramCardSetVisibility.Public, frame.OwnerSeat);
    }

    internal void CaptureProgramRepeatedJudgmentClaimOutcome(JudgmentFrame judgment, Card card, Suit suit)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == judgment.ParentFrameId)is not { RepeatedJudgment: { ClaimHandLimitExemption: not null } repeated } frame)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { RepeatedJudgment = repeated with { FinalOutcome = new(judgment.Id, card.Id, card.Kind, suit, repeated.SuccessSuits.Contains(suit)) } });
    }

    internal bool ExactRepeatedJudgmentSourceLive(ProgramSkillFrame frame, ProgramRepeatedJudgment state) => state.ClaimTurn is { } turn && turn.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && turn.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame);
    internal void ResumeProgramExactRepeatedJudgment(JudgmentFrame judgment, ProgramSkillFrame frame, ProgramRepeatedJudgment state)
    {
        if (_coreNativeDamageHandlesOperations.Value.GetJudgmentCard(judgment)is not { } card)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var outcome = state.FinalOutcome ?? throw new InvalidOperationException("Exact repeated judgment lost its finalized outcome.");
        if (outcome.JudgmentFrameId != judgment.Id || outcome.CardId != card.Id || outcome.CardKind != card.Kind || state.LastClaimReceipt is not null)
            throw new InvalidOperationException("Exact repeated judgment returned a different final entity.");
        var live = ExactRepeatedJudgmentSourceLive(frame, state);
        var location = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var next = state with
        {
            CompletedCount = checked(state.CompletedCount + 1),
            LastMatched = live && outcome.Matched
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RepeatedJudgment = next });
        if (location != CardLocation.Judgment(judgment.TargetSeat))
        {
            // A different actual claimant keeps the entity; its final color can still permit another judgment.
            if (!live)
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "判定返回前技能来源或当前回合已失效。");
            else
                _coreProgramRepeatedJudgmentOperations.ContinueProgramRepeatedJudgmentAfterMovement(frame.Id);
            return;
        }

        var to = live && outcome.Matched ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], to, new($"skill-program.{(frame.SkillId)}.repeatJudgment"), (_, records) =>
        {
            if (to != CardLocation.Hand(frame.OwnerSeat))
                return;
            var actual = records.SingleOrDefault(m => m.CardId == outcome.CardId && m.From == location && m.To == to && m.From != m.To);
            if (actual is null)
                return;
            var turn = next.ClaimTurn!;
            if (actual.TurnNumber != turn.TurnNumber || turn.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || turn.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
                throw new InvalidOperationException("An exact judgment receipt changed its actual turn.");
            var grant = next.ClaimHandLimitExemption == SkillProgramClaimHandLimitExemption.ActualTurn ? _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandLimitExemptCard(turn.TurnNumber, turn.TurnOwnerSeat, frame.Id, frame.InstructionIndex - 1, judgment.Id, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.OwnerSeat, card.Id, actual.Sequence) : null;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RepeatedJudgment = active.RepeatedJudgment!with { LastClaimReceipt = new(frame.InstructionIndex - 1, judgment.Id, card.Id, actual.Sequence, actual.From, actual.To, turn.TurnNumber, turn.TurnOwnerSeat, grant?.GrantSequence) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentCardClaimedEvent(judgment.Id, frame.SkillId, frame.OwnerSeat, card.Id, card.Kind));
            if (grant is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnHandLimitExemptCardGrantedEvent(grant));
        });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal void CleanupExactRepeatedJudgment(ProgramSkillFrame frame)
    {
        if (frame.RepeatedJudgment is not { ClaimHandLimitExemption: not null, FinalOutcome: { } outcome, LastClaimReceipt: null })
            return;
        var from = CardLocation.Judgment(frame.OwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(outcome.CardId) != from)
            return;
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == outcome.CardId);
        if (card.Kind != outcome.CardKind)
            throw new InvalidOperationException("Cancelled exact judgment changed its physical entity.");
        // Cleanup follows the established non-awaiting bound-card cleanup boundary.
        // Only the still-unclaimed owning outcome is moved; a real claimant retains its card.
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.DiscardPile, new CardMoveReason($"skill-program.{(frame.SkillId)}.repeatJudgment.cancel-cleanup"));
    }
}
