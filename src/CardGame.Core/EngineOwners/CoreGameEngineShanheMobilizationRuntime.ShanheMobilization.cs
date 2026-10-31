#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheMobilizationRuntime
{
    internal const string ShanheMobilizationSkill = "shanhe:current-mobilization-2203";
    SkillProgramStepOutcome global::CardGame.Core.IShanheMobilizationHost.ReviveRoundAllies(ProgramSkillFrame frame) => BeginShanheMobilization(frame);
    internal bool IsActualShanheMobilizationRound(ProgramSkillFrame frame) => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && frame.SkillId == ShanheMobilizationSkill && frame.TriggerId == "next-round-allies" && frame.WindowContext is { Window: SkillProgramTriggerWindow.RoundStarting } context && context.SourceSeat == _coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber > 0 && _coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra && _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Count == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundStartedEvent>().LastOrDefault()is { } started && started.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber && started.ActorSeat == _coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat;
    internal int[] ShanheMobilizationDeadAllies(int ownerSeat)
    {
        // The existing public journal is the death clock. Acquiring the tactic
        // after an earlier death still qualifies at the next real round.
        var round = 0;
        var deaths = new Dictionary<int, int>();
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
            if (fact is RoundStartedEvent started)
                round = started.RoundNumber;
            else if (fact is PlayerDiedEvent died)
                deaths[died.VictimSeat] = round;
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.Seat != ownerSeat && !player.IsAlive && player.GeneralSelected && player.HasHpValue && player.MaxHp > 0 && _coreTeamActivitiesOperations.SamePublicTeam(_coreMatchParticipantsPlayersAccess._players[ownerSeat], player) && deaths.TryGetValue(player.Seat, out var diedRound) && diedRound < _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber).Select(player => player.Seat).Order().ToArray();
    }

    internal SkillProgramStepOutcome BeginShanheMobilization(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheMobilization is not null)
            throw new InvalidOperationException("Mobilization cannot issue its original round twice.");
        if (!IsActualShanheMobilizationRound(frame))
            return SkillProgramStepOutcome.Continue;
        var targets = ShanheMobilizationDeadAllies(frame.OwnerSeat);
        if (targets.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMobilization = new(frame.InstructionIndex, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, 0, false) { TargetSeats = targets } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMobilizationIssuedEvent(frame.Id, frame.InstructionIndex, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, string.Join(",", targets)));
        ContinueShanheMobilization(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueShanheMobilization(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheMobilization is { } receipt)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMobilization = null });
                _coreSkillProgramsOperations.FinishProgramSkill(frame, false);
                return;
            }

            if (receipt.TargetIndex >= receipt.TargetSeats.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMobilization = null });
                _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
                return;
            }

            if (receipt.StatePublished)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMobilization = receipt with { TargetIndex = receipt.TargetIndex + 1, StatePublished = false } });
                continue;
            }

            var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeats[receipt.TargetIndex]];
            // Claim this exact recipient before real state observers can run.
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMobilization = receipt with { StatePublished = true } });
            if (!target.IsAlive && target.HasHpValue && target.MaxHp > 0 && _coreTeamActivitiesOperations.SamePublicTeam(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], target))
            {
                var wasFaceDown = _coreProgramLifeRestorationOperations.RestoreProgramCharacterLife(frame, target, target.MaxHp);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMobilizationRevivedEvent(frame.Id, frame.OwnerSeat, target.Seat, receipt.RoundNumber, target.Hp));
                _coreProgramLifeRestorationOperations.PublishProgramRevivalFaceState(target, wasFaceDown);
            }
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62045)]
    internal bool ResumeShanheMobilization(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheMobilization: not null } frame || frame.Id != id)
            return false;
        ContinueShanheMobilization(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62045)]
    internal bool ReturnShanheMobilizationMovement(ProgramSkillFrame frame) => frame.ShanheMobilization is not null && ResumeShanheMobilization(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62045)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62045)]
    internal bool CanCompleteShanheMobilization(ProgramSkillFrame frame) => frame.ShanheMobilization is not null;
}
