#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDrawPhaseObligationsRuntime
{
    internal bool HasDrawPhaseObligationBoundaryFrame() => _ownedDrawPhaseOperations.Get().HasDrawPhaseObligationBoundaryFrame();
    internal bool TryBeginScheduledDraw(int actorSeat) => _ownedDrawPhaseOperations.Get().TryBeginScheduledDraw(actorSeat);
    internal void ResolveDrawWithoutAttempt(int actorSeat, bool normalDrawReplaced = false, int adjustment = 0, int? frozenBase = null) =>
        _ownedDrawPhaseOperations.Get().ResolveDrawWithoutAttempt(actorSeat, normalDrawReplaced, adjustment, frozenBase);
    internal bool IsActualSkippedDrawContext(ProgramSkillWindowContext context) => _ownedDrawPhaseOperations.Get().IsActualSkippedDrawContext(context);
    internal bool TryBeginActualDrawCompletion(CharacterState owner, GameEngine.DelayedTurnEffects delayedEffects) => _ownedDrawPhaseOperations.Get().TryBeginActualDrawCompletion(owner, (int)delayedEffects);
    internal void ReturnDrawPhaseObligationWindow(ProgramLifecycleTriggerWindowFrame completed) => _ownedDrawPhaseOperations.Get().ReturnDrawPhaseObligationWindow(completed);
}
