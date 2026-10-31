#nullable enable

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
        var stop = !receipt.Red || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || _coreMatchOutcomeWinnerAccess._winner != Winner.None;
        var count = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Count;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheSourceYanyu = null, PendingMovementContinuation = null, InstructionIndex = stop ? count : frame.InstructionIndex });
        return false;
    }

    internal void SettleShanheSourceYanyuCard(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var receipt = frame.ShanheSourceYanyu!;
        if (frame.AttackAttempt is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheSourceYanyuCardSettledEvent>().Any(fact => fact.ProgramFrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex))
            throw new InvalidOperationException("Source Yanyu cannot claim before damage returns or settle its material twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceYanyu = receipt with { Stage = ShanheSourceYanyuStage.Movement }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (receipt.RetainedCardId is { } cardId && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing && (!receipt.Red || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive))
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(item => item.Id == cardId);
            var to = receipt.Red ? CardLocation.Hand(frame.OwnerSeat) : CardLocation.DiscardPile;
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, to, receipt.Red ? CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuClaimReason : CoreGameEngineShanheSourceYanyuDefinitions.ShanheSourceYanyuBlackReason, move =>
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
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    SkillProgramStepOutcome IShanheSourceYanyuHost.ResolveSourceYanyu(ProgramSkillFrame frame, SkillProgramEffect effect) => ResolveShanheSourceYanyu(frame, effect);
}
