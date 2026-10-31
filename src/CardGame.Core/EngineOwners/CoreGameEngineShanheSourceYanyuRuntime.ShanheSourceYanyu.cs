#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceYanyuRuntime
{
    internal const string ShanheSourceYanyuSkill = "shanhe:source-yanyu-3292";
    internal const string ShanheSourceYanyuReason = "shanhe.source-yanyu-3292";
    internal bool IsShanheSourceYanyuPreparation(ProgramSkillFrame frame) => frame.SkillId == ShanheSourceYanyuSkill && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } actual } && actual == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && actual == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && actual != frame.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Draw && frame.SelectedTargetSeats.SequenceEqual([actual]);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62046, SkillProgramEffectOp.ResolveShanheSourceYanyu)]
    internal bool CanOfferShanheSourceYanyu(ProgramTriggerLegalityContext context) => context.SkillId == ShanheSourceYanyuSkill && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && (context.WindowContext is null || context.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } actual } && actual == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && actual != context.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[actual].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Draw);
    internal SkillProgramStepOutcome ResolveShanheSourceYanyu(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourceYanyuPreparation(frame) || frame.ShanheSourceYanyu is not null || frame.JudgmentOutcomeBindings?.SingleOrDefault(item => item.Name == effect.OutcomeBind)is not { } outcome || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(frame, outcome) || outcome.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || outcome.InstructionIndex != frame.InstructionIndex - 1 || outcome.ResultBind != effect.SourceBind || outcome.Reason != ShanheSourceYanyuReason)
            throw new InvalidOperationException("Source Yanyu requires its exact bounded native preparation judgment.");
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        if (binding.CardIds.Count > 1 || binding.CardIds is [var retained] && (retained != outcome.CardId || binding.SourceLocations is not [var from] || from != CardLocation.Processing || _cardZonesOperations._cardZones.GetLocation(retained) != from))
            throw new InvalidOperationException("Source Yanyu cannot replace its original unclaimed judgment material.");
        var red = outcome.Suit is Suit.Heart or Suit.Diamond;
        var promise = red && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
        var receipt = new ShanheSourceYanyuReceipt(frame.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, frame.WindowContext!.ParentFrameId, effect.SourceBind!, outcome, red, promise, binding.CardIds.Count == 1 ? binding.CardIds[0] : null, promise ? ShanheSourceYanyuStage.Damage : ShanheSourceYanyuStage.Movement);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceYanyu = receipt });
        if (promise)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceYanyuDamageRequestedEvent(frame.Id, frame.InstructionIndex, outcome.JudgmentFrameId, frame.OwnerSeat, receipt.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber));
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, receipt.TargetSeat, 1, nature: DamageNature.Fire);
        }

        SettleShanheSourceYanyuCard(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // This handler runs before the generic executor's game-over cancellation.
    // Only a receipt which already promised damage may claim on its exact return.
    [ProgramContinuation(ProgramContinuationKind.Resume, 62046)]
    internal bool ResumeShanheSourceYanyu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceYanyu: { } receipt } frame || frame.Id != id)
            return false;
        AssertShanheSourceYanyu(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!));
        if (frame.AttackAttempt is not null)
            return true;
        if (receipt.Stage == ShanheSourceYanyuStage.Damage)
        {
            SettleShanheSourceYanyuCard(frame);
            return true;
        }

        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id)))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var stop = !receipt.Red || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || _coreMatchOutcomeWinnerAccess._winner != Winner.None;
        var count = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Count;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheSourceYanyu = null, PendingMovementContinuation = null, InstructionIndex = stop ? count : frame.InstructionIndex });
        return false;
    }

    internal void SettleShanheSourceYanyuCard(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var receipt = frame.ShanheSourceYanyu!;
        if (frame.AttackAttempt is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceYanyuCardSettledEvent>().Any(fact => fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex))
            throw new InvalidOperationException("Source Yanyu cannot claim before damage returns or settle its material twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceYanyu = receipt with { Stage = ShanheSourceYanyuStage.Movement }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (receipt.RetainedCardId is { } cardId && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing && (!receipt.Red || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive))
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(item => item.Id == cardId);
            var to = receipt.Red ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile;
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, to, receipt.Red ? global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuClaimReason : global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuBlackReason, move =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShanheSourceYanyu = active.ShanheSourceYanyu!with { MovementSequence = move.Sequence, Destination = to } });
            });
        }

        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        receipt = frame.ShanheSourceYanyu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceYanyuCardSettledEvent(frame.Id, receipt.InstructionIndex, receipt.Outcome.JudgmentFrameId, frame.OwnerSeat, receipt.TargetSeat, receipt.RetainedCardId, receipt.Red, receipt.MovementSequence, receipt.Destination));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62046)]
    internal bool ReturnShanheSourceYanyuMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceYanyu is not { Stage: ShanheSourceYanyuStage.Movement })
            return false;
        AssertShanheSourceYanyu(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 62046)]
    internal bool IsShanheSourceYanyuMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.ResolveShanheSourceYanyu && frame.ShanheSourceYanyu is { Stage: ShanheSourceYanyuStage.Movement } receipt && IsShanheSourceYanyuPreparation(frame) && receipt.InstructionIndex == frame.InstructionIndex && receipt.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && receipt.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && receipt.PreparationFrameId == frame.WindowContext!.ParentFrameId && effect.SourceBind == receipt.CardBind && effect.OutcomeBind == receipt.Outcome.Name && _coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(frame, receipt.Outcome) && receipt.Outcome.TargetSeat == receipt.TargetSeat && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62046)]
    internal void AssertShanheSourceYanyu(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.ShanheSourceYanyu is not { } receipt)
            return;
        if (!IsShanheSourceYanyuPreparation(frame) || receipt.InstructionIndex != frame.InstructionIndex || frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count || plan.Instructions[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.ResolveShanheSourceYanyu } effect || effect.SourceBind != receipt.CardBind || effect.OutcomeBind != receipt.Outcome.Name || receipt.PreparationFrameId != frame.WindowContext!.ParentFrameId || receipt.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || receipt.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || receipt.Outcome.TargetSeat != receipt.TargetSeat || receipt.Outcome.InstructionIndex != frame.InstructionIndex - 1 || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(frame, receipt.Outcome) || receipt.Red != (receipt.Outcome.Suit is Suit.Heart or Suit.Diamond) || receipt.RetainedCardId is { } retained && retained != receipt.Outcome.CardId || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceYanyuDamageRequestedEvent>().Count(fact => fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex && fact.JudgmentFrameId == receipt.Outcome.JudgmentFrameId && fact.OwnerSeat == frame.OwnerSeat && fact.TargetSeat == receipt.TargetSeat && fact.TurnNumber == receipt.TurnNumber) != (receipt.DamagePromised ? 1 : 0) || receipt.DamagePromised && !receipt.Red || !Enum.IsDefined(receipt.Stage) || receipt.Stage == ShanheSourceYanyuStage.Damage && (receipt.MovementSequence is not null || receipt.Destination is not null) || receipt.Stage == ShanheSourceYanyuStage.Movement && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceYanyuCardSettledEvent>().Count(fact => fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex && fact.JudgmentFrameId == receipt.Outcome.JudgmentFrameId && fact.OwnerSeat == frame.OwnerSeat && fact.TargetSeat == receipt.TargetSeat && fact.CardId == receipt.RetainedCardId && fact.Red == receipt.Red && fact.MovementSequence == receipt.MovementSequence && fact.Destination == receipt.Destination) != 1 || receipt.MovementSequence is { } sequence && (receipt.RetainedCardId is not { } cardId || receipt.Destination != (receipt.Red ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.Sequence == sequence && move.CardId == cardId && move.From == CardLocation.Processing && move.To == receipt.Destination && move.TurnNumber == receipt.TurnNumber && move.Reason == (receipt.Red ? global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuClaimReason : global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuBlackReason))))
            throw new InvalidOperationException("Source Yanyu lost its native preparation, exact judgment, promised damage or unique material receipt.");
    }

    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceYanyuHost.ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) => ResolveShanheSourceYanyu(frame, effect);
}
