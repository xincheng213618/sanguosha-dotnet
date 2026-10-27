using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourceYanyuSkill = "shanhe:source-yanyu-3292";
    private const string ShanheSourceYanyuReason = "shanhe.source-yanyu-3292";
    private static readonly CardMoveReason ShanheSourceYanyuClaimReason = new("skill-program.shanhe:source-yanyu-3292.claim");
    private static readonly CardMoveReason ShanheSourceYanyuBlackReason = new("skill-program.shanhe:source-yanyu-3292.black");

    private bool IsShanheSourceYanyuPreparation(ProgramSkillFrame frame) =>
        frame.SkillId == ShanheSourceYanyuSkill && _options.ShanheBattle is not null &&
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } actual } &&
        actual == _currentSeat && actual == _turnProgression.OwnerSeat && actual != frame.OwnerSeat &&
        _turnProgression.TurnNumber == _turnNumber && _phase == TurnPhase.Draw &&
        frame.SelectedTargetSeats.SequenceEqual([actual]);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62046, SkillProgramEffectOp.ResolveShanheSourceYanyu)]
    private bool CanOfferShanheSourceYanyu(ProgramTriggerLegalityContext context) =>
        context.SkillId == ShanheSourceYanyuSkill && _options.ShanheBattle is not null &&
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId &&
        (context.WindowContext is null ||
         context.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } actual } &&
         actual == _currentSeat && actual != context.OwnerSeat && _players[actual].IsAlive && _phase == TurnPhase.Draw);

    private SkillProgramStepOutcome ResolveShanheSourceYanyu(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceYanyuPreparation(frame) || frame.ShanheSourceYanyu is not null ||
            frame.JudgmentOutcomeBindings?.SingleOrDefault(item => item.Name == effect.OutcomeBind) is not { } outcome ||
            !IsValidProgramJudgmentOutcome(frame, outcome) || outcome.TargetSeat != _currentSeat ||
            outcome.InstructionIndex != frame.InstructionIndex - 1 || outcome.ResultBind != effect.SourceBind || outcome.Reason != ShanheSourceYanyuReason)
            throw new InvalidOperationException("Source Yanyu requires its exact bounded native preparation judgment.");
        var binding = GetProgramCardSet(frame, effect.SourceBind!);
        if (binding.CardIds.Count > 1 || binding.CardIds is [var retained] &&
            (retained != outcome.CardId || binding.SourceLocations is not [var from] || from != CardLocation.Processing ||
             _cardZones.GetLocation(retained) != from))
            throw new InvalidOperationException("Source Yanyu cannot replace its original unclaimed judgment material.");
        var red = outcome.Suit is Suit.Heart or Suit.Diamond;
        var promise = red && _players[_currentSeat].IsAlive && _players[frame.OwnerSeat].IsAlive && _winner == Winner.None;
        var receipt = new ShanheSourceYanyuReceipt(frame.InstructionIndex, _turnNumber, _currentSeat,
            frame.WindowContext!.ParentFrameId, effect.SourceBind!, outcome, red, promise,
            binding.CardIds.Count == 1 ? binding.CardIds[0] : null, promise ? ShanheSourceYanyuStage.Damage : ShanheSourceYanyuStage.Movement);
        ReplaceRuntimeTop(frame = frame with { ShanheSourceYanyu = receipt });
        if (promise)
        {
            AdvanceEventRulesAndQueueFact(new ShanheSourceYanyuDamageRequestedEvent(frame.Id, frame.InstructionIndex,
                outcome.JudgmentFrameId, frame.OwnerSeat, receipt.TargetSeat, _turnNumber));
            return BeginProgramSkillDamage(frame, receipt.TargetSeat, 1, nature: DamageNature.Fire);
        }
        SettleShanheSourceYanyuCard(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // This handler runs before the generic executor's game-over cancellation.
    // Only a receipt which already promised damage may claim on its exact return.
    [ProgramContinuation(ProgramContinuationKind.Resume, 62046)]
    private bool ResumeShanheSourceYanyu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceYanyu: { } receipt } frame || frame.Id != id) return false;
        AssertShanheSourceYanyu(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!));
        if (frame.AttackAttempt is not null) return true;
        if (receipt.Stage == ShanheSourceYanyuStage.Damage)
        {
            SettleShanheSourceYanyuCard(frame);
            return true;
        }
        if (_winner == Winner.None && (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id))) return true;
        frame = GetActiveProgramFrame(id);
        var stop = !receipt.Red || !_players[receipt.TargetSeat].IsAlive || !_players[frame.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || _winner != Winner.None;
        var count = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Count;
        ReplaceRuntimeTop(frame with { ShanheSourceYanyu = null, PendingMovementContinuation = null,
            InstructionIndex = stop ? count : frame.InstructionIndex });
        return false;
    }

    private void SettleShanheSourceYanyuCard(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var receipt = frame.ShanheSourceYanyu!;
        if (frame.AttackAttempt is not null || ProgramEventHistory<ShanheSourceYanyuCardSettledEvent>().Any(fact =>
            fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex))
            throw new InvalidOperationException("Source Yanyu cannot claim before damage returns or settle its material twice.");
        ReplaceRuntimeTop(frame = frame with { ShanheSourceYanyu = receipt with { Stage = ShanheSourceYanyuStage.Movement },
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (receipt.RetainedCardId is { } cardId && _cardZones.GetLocation(cardId) == CardLocation.Processing &&
            (!receipt.Red || _players[frame.OwnerSeat].IsAlive))
        {
            var card = _cardZones.CardsAt(CardLocation.Processing).Single(item => item.Id == cardId);
            var to = receipt.Red ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile;
            MoveCard(card, CardLocation.Processing, to, receipt.Red ? ShanheSourceYanyuClaimReason : ShanheSourceYanyuBlackReason, move =>
            {
                var active = GetActiveProgramFrame(frame.Id);
                ReplaceRuntimeTop(active with { ShanheSourceYanyu = active.ShanheSourceYanyu! with
                { MovementSequence = move.Sequence, Destination = to } });
            });
        }
        frame = GetActiveProgramFrame(frame.Id); receipt = frame.ShanheSourceYanyu!;
        AdvanceEventRulesAndQueueFact(new ShanheSourceYanyuCardSettledEvent(frame.Id, receipt.InstructionIndex,
            receipt.Outcome.JudgmentFrameId, frame.OwnerSeat, receipt.TargetSeat, receipt.RetainedCardId,
            receipt.Red, receipt.MovementSequence, receipt.Destination));
        if (_winner == Winner.None && TryBeginCardsMovedProgramWindow()) return;
        ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id) with { PendingMovementContinuation = null });
        AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62046)]
    private bool ReturnShanheSourceYanyuMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceYanyu is not { Stage: ShanheSourceYanyuStage.Movement }) return false;
        AssertShanheSourceYanyu(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!));
        ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 62046)]
    private bool IsShanheSourceYanyuMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ResolveShanheSourceYanyu &&
        frame.ShanheSourceYanyu is { Stage: ShanheSourceYanyuStage.Movement } receipt &&
        IsShanheSourceYanyuPreparation(frame) && receipt.InstructionIndex == frame.InstructionIndex &&
        receipt.TurnNumber == _turnNumber && receipt.TargetSeat == _currentSeat &&
        receipt.PreparationFrameId == frame.WindowContext!.ParentFrameId &&
        effect.SourceBind == receipt.CardBind && effect.OutcomeBind == receipt.Outcome.Name &&
        IsValidProgramJudgmentOutcome(frame, receipt.Outcome) && receipt.Outcome.TargetSeat == receipt.TargetSeat &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62046)]
    private void AssertShanheSourceYanyu(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.ShanheSourceYanyu is not { } receipt) return;
        if (!IsShanheSourceYanyuPreparation(frame) || receipt.InstructionIndex != frame.InstructionIndex ||
            frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count ||
            plan.Instructions[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.ResolveShanheSourceYanyu } effect ||
            effect.SourceBind != receipt.CardBind || effect.OutcomeBind != receipt.Outcome.Name ||
            receipt.PreparationFrameId != frame.WindowContext!.ParentFrameId || receipt.TurnNumber != _turnNumber ||
            receipt.TargetSeat != _currentSeat || receipt.Outcome.TargetSeat != receipt.TargetSeat ||
            receipt.Outcome.InstructionIndex != frame.InstructionIndex - 1 || !IsValidProgramJudgmentOutcome(frame, receipt.Outcome) ||
            receipt.Red != (receipt.Outcome.Suit is Suit.Heart or Suit.Diamond) ||
            receipt.RetainedCardId is { } retained && retained != receipt.Outcome.CardId ||
            ProgramEventHistory<ShanheSourceYanyuDamageRequestedEvent>().Count(fact =>
                fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex &&
                fact.JudgmentFrameId == receipt.Outcome.JudgmentFrameId && fact.OwnerSeat == frame.OwnerSeat &&
                fact.TargetSeat == receipt.TargetSeat && fact.TurnNumber == receipt.TurnNumber) != (receipt.DamagePromised ? 1 : 0) ||
            receipt.DamagePromised && !receipt.Red || !Enum.IsDefined(receipt.Stage) ||
            receipt.Stage == ShanheSourceYanyuStage.Damage && (receipt.MovementSequence is not null || receipt.Destination is not null) ||
            receipt.Stage == ShanheSourceYanyuStage.Movement && ProgramEventHistory<ShanheSourceYanyuCardSettledEvent>().Count(fact =>
                fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex &&
                fact.JudgmentFrameId == receipt.Outcome.JudgmentFrameId && fact.OwnerSeat == frame.OwnerSeat &&
                fact.TargetSeat == receipt.TargetSeat && fact.CardId == receipt.RetainedCardId && fact.Red == receipt.Red &&
                fact.MovementSequence == receipt.MovementSequence && fact.Destination == receipt.Destination) != 1 ||
            receipt.MovementSequence is { } sequence && (receipt.RetainedCardId is not { } cardId ||
                receipt.Destination != (receipt.Red ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile) ||
                !_cardMovements.Any(move => move.Sequence == sequence && move.CardId == cardId && move.From == CardLocation.Processing &&
                    move.To == receipt.Destination && move.TurnNumber == receipt.TurnNumber &&
                    move.Reason == (receipt.Red ? ShanheSourceYanyuClaimReason : ShanheSourceYanyuBlackReason))))
            throw new InvalidOperationException("Source Yanyu lost its native preparation, exact judgment, promised damage or unique material receipt.");
    }

    [ProgramCapability]
    private sealed class ShanheSourceYanyuCapability(GameEngine engine) : IShanheSourceYanyuHost
    {
        public SkillProgramStepOutcome ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) =>
            engine.ResolveShanheSourceYanyu(frame, effect);
    }
}
