#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool ValidWuXiongmuStarted(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { } r || f.SkillId != WuXiongmuSkill || f.TriggerId != "round-start" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.InstructionIndex != r.InstructionIndex || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.RoundStarting } context || context.ParentFrameId != r.WindowId || context.OwnerSeat != f.OwnerSeat || r.Source != WuXiongmuSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || !WuXiongmuNativeCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiongmuStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var start] && start == new XiongmuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.RoundNumber, r.ActualTurn, r.TurnOwner, r.RequestedDraw, r.OriginalHandCount, r.OriginalMaxHp, r.SequenceBefore) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiongmuStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.RoundNumber == r.RoundNumber) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == context.Window) == 1;
    }

}
