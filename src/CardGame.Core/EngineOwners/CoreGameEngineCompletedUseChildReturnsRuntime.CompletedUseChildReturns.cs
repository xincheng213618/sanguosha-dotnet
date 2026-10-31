#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedUseChildReturnsRuntime
{
    internal void RestoreCompletedUseAttackAfterIssuedChild(ProgramSkillFrame program, ProgramCardTriggerWindowFrame window)
    {
        if (window.Continuation != ProgramCardContinuation.CompletedSlash || program.WindowContext?.CardUse is not { } identity || identity.ParentCardUseFrameId != window.ParentFrameId || identity.CardActionId != window.Action.ActionId)
            return;
        var heji = _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiNativeIssuedEvent>().Where(e => e.ProgramFrameId == program.Id && e.IncomingUseId == window.ParentFrameId && e.IncomingActionId == window.Action.ActionId && e.Source.SkillId == program.SkillId && e.Source.SkillInstanceId == program.SkillInstanceId && e.GameplayHash == program.GameplayHash).ToArray();
        var counter = _coreProgramEventHistoryOperations.ProgramEventHistory<FulueCounterUseIssuedEvent>().Where(e => e.Invoice.ProgramFrameId == program.Id && e.Invoice.OriginalUseId == window.ParentFrameId && e.Invoice.OriginalActionId == window.Action.ActionId && e.Invoice.Source.SkillId == program.SkillId && e.Invoice.Source.SkillInstanceId == program.SkillInstanceId && e.Invoice.GameplayHash == program.GameplayHash).ToArray();
        long? childId = heji is [var issued] && _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiReturnedEvent>().Count(e => e.ProgramFrameId == program.Id && e.UseId == issued.UseId) == 1 ? issued.UseId : counter is [var reverse] ? reverse.UseId : null;
        if (childId is not { } id)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == id) != 1 || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } action, CardAttack: not null } original || original.Id != window.AttackOwnerFrameId || action.ActionId != window.Action.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.Id) != 1 || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } active && active.ResolutionId != original.Id)
            throw new InvalidOperationException("An issued completed-card child lost its finished native use or original completed attack.");
        // A native child deactivates the old attack. Only its exact completed
        // parent regains the handle needed to close the original trigger window.
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = _kernelCoreMainOperations.CreateCardAttackHandle(original.Id);
    }
}
