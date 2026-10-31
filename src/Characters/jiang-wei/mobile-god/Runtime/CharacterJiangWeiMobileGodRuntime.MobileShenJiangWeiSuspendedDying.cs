#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMobileGodRuntime
{
    internal bool IsOriginalDyingSuspendedByMobileJwShenpei(DyingFrame dying)
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
        {
            if (f.MobileJiangWei is not { Operation: SkillProgramEffectOp.ResolveMobileJiangWeiShenpei, Stage: MobileJiangWeiStage.ThunderIssued } r || r.DyingId != dying.Id || dying.VictimSeat != f.OwnerSeat || f.SkillId != "mobile:shenpei" || f.InstructionIndex != 1 || r.Source != MobileJwSource(f) || r.GameplayHash != f.GameplayHash || f.AttackAttempt is not { Nature: DamageNature.Thunder, SourceLess: false } attack || attack.SourceSeat != f.OwnerSeat || f.AttackReturn is null || !MobileJwParent(f) || f.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering, Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry || entry.ResumeDyingFrameId != dying.Id || r.ParentId != entry.Id)
                continue;
            var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
            if (index < 2 || _resolutionStackOperations._resolutionStack[index - 2].Id != dying.Id || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiangWeiStartedEvent>().Count(e => e == new MobileJiangWeiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ParentId, r.DyingId, r.Amount)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRecoveryAppliedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.InstructionIndex == f.InstructionIndex && e.Receipt.GameplayHash == f.GameplayHash && e.Receipt.SourceSeat == f.OwnerSeat && e.Receipt.TargetSeat == f.OwnerSeat && e.Receipt.RequestedAmount == r.Amount) != 1)
                continue;
            return true;
        }

        return false;
    }
}
