#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuLeRuntime
{
    internal bool ValidPingluStarted(ProgramSkillFrame frame)
    {
        if (frame.Pinglu is not { } r || r.Source != PingluSource(frame) || r.GameplayHash != frame.GameplayHash || frame.SkillId != "le:pinglu" || frame.ActivationId != "all-in-range-random-hand" || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || r.InstructionIndex != frame.InstructionIndex || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != r.GameplayHash || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || phase.TurnNumber != r.ActualTurn || phase.ActualTurnOwnerSeat != r.TurnOwner || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<PingluStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [var started] && started == new PingluStartedEvent(frame.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner, r.Targets.Count, r.SequenceBefore) && // Active programs publish ProgramSkillStarted, while lifecycle
 // triggers publish ProgramBindingStarted. The signed Pinglu fact
        // above retains this activation's exact source instance and hash.
        _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.ActivationId == frame.ActivationId && e.OwnerSeat == frame.OwnerSeat) == 1;
    }
}
