using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheMobilizationSkill = "shanhe:current-mobilization-2203";
    [ProgramCapability]
    private sealed class ShanheMobilizationCapability(GameEngine engine) : IShanheMobilizationHost
    {
        public SkillProgramStepOutcome ReviveRoundAllies(ProgramSkillFrame frame) => engine.BeginShanheMobilization(frame);
    }

    private bool IsActualShanheMobilizationRound(ProgramSkillFrame frame) =>
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null &&
        frame.SkillId == ShanheMobilizationSkill && frame.TriggerId == "next-round-allies" &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.RoundStarting } context &&
        context.SourceSeat == _currentSeat && _players[frame.OwnerSeat].IsAlive &&
        _roundNumber > 0 && _turnProgression.Kind != ActualTurnKind.Extra && _roundTurnSeats.Count == 1 &&
        CompleteProgramEventHistory().OfType<RoundStartedEvent>().LastOrDefault() is { } started &&
        started.RoundNumber == _roundNumber && started.ActorSeat == _currentSeat;

    private int[] ShanheMobilizationDeadAllies(int ownerSeat)
    {
        // The existing public journal is the death clock. Acquiring the tactic
        // after an earlier death still qualifies at the next real round.
        var round = 0;
        var deaths = new Dictionary<int, int>();
        foreach (var fact in CompleteProgramEventHistory())
            if (fact is RoundStartedEvent started) round = started.RoundNumber;
            else if (fact is PlayerDiedEvent died) deaths[died.VictimSeat] = round;
        return _players.Where(player => player.Seat != ownerSeat && !player.IsAlive && player.GeneralSelected &&
                player.HasHpValue && player.MaxHp > 0 && SamePublicTeam(_players[ownerSeat], player) &&
                deaths.TryGetValue(player.Seat, out var diedRound) && diedRound < _roundNumber)
            .Select(player => player.Seat).Order().ToArray();
    }

    private SkillProgramStepOutcome BeginShanheMobilization(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheMobilization is not null)
            throw new InvalidOperationException("Mobilization cannot issue its original round twice.");
        if (!IsActualShanheMobilizationRound(frame)) return SkillProgramStepOutcome.Continue;
        var targets = ShanheMobilizationDeadAllies(frame.OwnerSeat);
        if (targets.Length == 0) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame = frame with
        {
            ShanheMobilization = new(frame.InstructionIndex, _roundNumber, 0, false) { TargetSeats = targets }
        });
        AdvanceEventRulesAndQueueFact(new ShanheMobilizationIssuedEvent(frame.Id, frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _roundNumber, string.Join(",", targets)));
        ContinueShanheMobilization(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void ContinueShanheMobilization(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheMobilization is { } receipt)
        {
            if (TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) ||
                TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
                TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) ||
                TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id)) return;
            frame = GetActiveProgramFrame(frame.Id);
            if (_winner != Winner.None || _status == EngineStatus.Completed)
            { ReplaceRuntimeTop(frame = frame with { ShanheMobilization = null }); FinishProgramSkill(frame, false); return; }
            if (receipt.TargetIndex >= receipt.TargetSeats.Count)
            { ReplaceRuntimeTop(frame = frame with { ShanheMobilization = null }); FinishProgramSkill(frame, true); return; }
            if (receipt.StatePublished)
            {
                ReplaceRuntimeTop(frame = frame with { ShanheMobilization = receipt with
                    { TargetIndex = receipt.TargetIndex + 1, StatePublished = false } });
                continue;
            }
            var target = _players[receipt.TargetSeats[receipt.TargetIndex]];
            // Claim this exact recipient before real state observers can run.
            ReplaceRuntimeTop(frame = frame with { ShanheMobilization = receipt with { StatePublished = true } });
            if (!target.IsAlive && target.HasHpValue && target.MaxHp > 0 && SamePublicTeam(_players[frame.OwnerSeat], target))
            {
                var wasFaceDown = RestoreProgramCharacterLife(frame, target, target.MaxHp);
                AdvanceEventRulesAndQueueFact(new ShanheMobilizationRevivedEvent(frame.Id, frame.OwnerSeat, target.Seat,
                    receipt.RoundNumber, target.Hp));
                PublishProgramRevivalFaceState(target, wasFaceDown);
            }
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62045)]
    private bool ResumeShanheMobilization(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheMobilization: not null } frame || frame.Id != id) return false;
        ContinueShanheMobilization(frame); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 62045)]
    private bool ReturnShanheMobilizationMovement(ProgramSkillFrame frame) => frame.ShanheMobilization is not null && ResumeShanheMobilization(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62045)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62045)]
    private bool CanCompleteShanheMobilization(ProgramSkillFrame frame) => frame.ShanheMobilization is not null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62045)]
    private void AssertShanheMobilization(ProgramSkillFrame frame)
    {
        if (frame.ShanheMobilization is not { } receipt) return;
        if (_modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is null ||
            frame.SkillId != ShanheMobilizationSkill || frame.TriggerId != "next-round-allies" ||
            frame.WindowContext?.Window != SkillProgramTriggerWindow.RoundStarting || receipt.RoundNumber <= 0 ||
            receipt.InstructionIndex != frame.InstructionIndex || receipt.TargetSeats.Count is < 1 or > 7 ||
            receipt.TargetSeats.Distinct().Count() != receipt.TargetSeats.Count || receipt.TargetIndex < 0 ||
            receipt.TargetIndex > receipt.TargetSeats.Count || receipt.StatePublished && receipt.TargetIndex == receipt.TargetSeats.Count ||
            receipt.TargetSeats.Any(seat => !IsValidPlayerSeat(seat) || seat == frame.OwnerSeat) ||
            ProgramEventHistory<ShanheMobilizationIssuedEvent>().Where(item => item.FrameId == frame.Id).ToArray() is not [var issued] ||
            issued.InstructionIndex != receipt.InstructionIndex || issued.RoundNumber != receipt.RoundNumber ||
            issued.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            issued.TargetSeatsKey != string.Join(",", receipt.TargetSeats))
            throw new InvalidOperationException("Mobilization lost its original round, public-death recipients or owning frame.");
    }
}
