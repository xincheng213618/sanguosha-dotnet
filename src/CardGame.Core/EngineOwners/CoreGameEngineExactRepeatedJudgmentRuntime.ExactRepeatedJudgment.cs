#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExactRepeatedJudgmentRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IRepeatedJudgmentClaimPolicyProgramHost.RepeatJudgmentWithClaimPolicy(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy) => BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
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
        if (judgment.Continuation != JudgmentContinuationKind.ProgramSkill || judgment.TargetSeat != frame.OwnerSeat || judgment.SourceSeat != frame.OwnerSeat || judgment.Reason != repeated.Reason || judgment.ProgramResultBind != repeated.ResultBind || repeated.LastMatched is not null || repeated.FinalOutcome is not null || repeated.LastClaimReceipt is not null)
            throw new InvalidOperationException("Exact repeated judgment final outcome lost its owning parent.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { RepeatedJudgment = repeated with { FinalOutcome = new(judgment.Id, card.Id, card.Kind, suit, repeated.SuccessSuits.Contains(suit)) } });
    }

    internal bool ExactRepeatedJudgmentSourceLive(ProgramSkillFrame frame, ProgramRepeatedJudgment state) => state.ClaimTurn is { } turn && turn.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && turn.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId);
    internal void ResumeProgramExactRepeatedJudgment(JudgmentFrame judgment, ProgramSkillFrame frame, ProgramRepeatedJudgment state)
    {
        if (_coreNativeDamageHandlesOperations.GetJudgmentCard(judgment)is not { } card)
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
            var grant = next.ClaimHandLimitExemption == SkillProgramClaimHandLimitExemption.ActualTurn ? _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandLimitExemptCard(turn.TurnNumber, turn.TurnOwnerSeat, frame.Id, frame.InstructionIndex - 1, judgment.Id, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.OwnerSeat, card.Id, actual.Sequence) : null;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { RepeatedJudgment = active.RepeatedJudgment!with { LastClaimReceipt = new(frame.InstructionIndex - 1, judgment.Id, card.Id, actual.Sequence, actual.From, actual.To, turn.TurnNumber, turn.TurnOwnerSeat, grant?.GrantSequence) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentCardClaimedEvent(judgment.Id, frame.SkillId, frame.OwnerSeat, card.Id, card.Kind));
            if (grant is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnHandLimitExemptCardGrantedEvent(grant));
        });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal bool IsExactRepeatedJudgmentFinalizedChildSubtree(ProgramJudgmentTriggerWindowFrame window, JudgmentFrame? judgment, int windowIndex)
    {
        if (judgment is null || !window.Activated || window.ParentFrameId != judgment.Id || windowIndex < 1 || _resolutionStackOperations._resolutionStack[windowIndex - 1] != judgment || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || windowIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[windowIndex + 1] is not ProgramSkillFrame child || child.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentFinalized } context || context.ParentFrameId != window.Id || context.Judgment != window.Judgment)
            return false;
        var candidate = window.Candidates[window.CandidateIndex];
        if (candidate.OwnerSeat != child.OwnerSeat || candidate.SkillId != child.SkillId || candidate.TriggerId != child.TriggerId || candidate.SkillInstanceId != child.SkillInstanceId || candidate.GameplayHash != child.GameplayHash)
            return false;
        var owner = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == judgment.ParentFrameId);
        if (owner is not { InstructionIndex: > 0, RepeatedJudgment: { ClaimHandLimitExemption: SkillProgramClaimHandLimitExemption.ActualTurn or SkillProgramClaimHandLimitExemption.None, FinalOutcome: { } outcome } state } || owner.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || judgment.TargetSeat != owner.OwnerSeat || window.Judgment.SubjectSeat != owner.OwnerSeat || outcome.JudgmentFrameId != judgment.Id || outcome.JudgmentFrameId != window.Judgment.JudgmentFrameId || outcome.CardId != judgment.CardId || outcome.CardId != window.Judgment.CardId || outcome.CardKind != judgment.CardKind || outcome.CardKind != window.Judgment.CardKind || outcome.FinalSuit != judgment.Suit || outcome.FinalSuit != window.Judgment.Suit)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(owner, _contentRegistryOperations._contentRegistry.GetSkill(owner.SkillId).Program!);
        if (owner.InstructionIndex > plan.Instructions.Count)
            return false;
        var effect = plan.Instructions[owner.InstructionIndex - 1];
        return effect.Op == SkillProgramEffectOp.RepeatJudgment && effect.ClaimHandLimitExemption == state.ClaimHandLimitExemption && effect.JudgmentReason == state.Reason && effect.ResultBind == state.ResultBind && effect.Suits.SequenceEqual(state.SuccessSuits);
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

    internal void AssertExactRepeatedJudgmentReceipt(ProgramSkillFrame frame, IReadOnlyList<SkillProgramEffect> instructions)
    {
        if (frame.RepeatedJudgment is not { ClaimHandLimitExemption: not null } state)
            return;
        var index = frame.InstructionIndex - 1;
        if (index < 0 || index >= instructions.Count || instructions[index] is not { Op: SkillProgramEffectOp.RepeatJudgment, ClaimHandLimitExemption: SkillProgramClaimHandLimitExemption.ActualTurn or SkillProgramClaimHandLimitExemption.None } effect || state.ClaimHandLimitExemption != effect.ClaimHandLimitExemption || state.Reason != effect.JudgmentReason || state.ResultBind != effect.ResultBind || !state.SuccessSuits.SequenceEqual(effect.Suits) || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || state.ClaimTurn is not { } turn || turn.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || turn.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            throw new InvalidOperationException("Exact repeated judgment lost its instruction or actual turn.");
        if (state.FinalOutcome is { } outcome && (outcome.JudgmentFrameId <= 0 || outcome.CardId <= 0 || !Enum.IsDefined(outcome.FinalSuit) || outcome.Matched != state.SuccessSuits.Contains(outcome.FinalSuit)))
            throw new InvalidOperationException("Exact repeated judgment has an invalid final outcome.");
        if (state.LastClaimReceipt is not { } receipt)
            return;
        if (state.FinalOutcome is not { Matched: true } final || state.LastMatched != true || receipt.InstructionIndex != index || final.JudgmentFrameId != receipt.JudgmentFrameId || final.CardId != receipt.CardId || receipt.From != CardLocation.Judgment(frame.OwnerSeat) || receipt.To != CardLocation.Hand(frame.OwnerSeat) || receipt.TurnNumber != turn.TurnNumber || receipt.TurnOwnerSeat != turn.TurnOwnerSeat || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == receipt.MovementSequence && m.CardId == receipt.CardId && m.CardKind == final.CardKind && m.From == receipt.From && m.To == receipt.To && m.TurnNumber == receipt.TurnNumber))
            throw new InvalidOperationException("Exact repeated judgment receipt differs from its actual movement.");
        if (state.ClaimHandLimitExemption == SkillProgramClaimHandLimitExemption.None)
        {
            if (receipt.GrantSequence is not null || _turnCardUseEffectsOperations._turnCardUseEffects.HandLimitExemptCards.Any(g => g.ParentFrameId == frame.Id && g.EffectIndex == index && g.JudgmentFrameId == final.JudgmentFrameId))
                throw new InvalidOperationException("An exact judgment without exemption acquired a hand-limit grant.");
            return;
        }

        var grant = receipt.GrantSequence is { } sequence ? _turnCardUseEffectsOperations._turnCardUseEffects.HandLimitExemptCards.SingleOrDefault(g => g.GrantSequence == sequence) : null;
        if (grant is null || grant.ParentFrameId != frame.Id || grant.EffectIndex != index || grant.JudgmentFrameId != final.JudgmentFrameId || grant.CardId != final.CardId || grant.MovementSequence != receipt.MovementSequence || grant.BeneficiarySeat != frame.OwnerSeat || grant.TurnNumber != turn.TurnNumber || grant.TurnSeat != turn.TurnOwnerSeat || grant.Source != global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame))
            throw new InvalidOperationException("Exact repeated judgment receipt differs from its actual movement or grant.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 310)]
    internal void AssertRegisteredExactRepeatedJudgmentReceipt(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        AssertExactRepeatedJudgmentReceipt(frame, plan.Instructions);
    }
}
