#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDrawPhaseObligationsRuntime
{
    internal bool HasDrawPhaseObligationBoundaryFrame() => _ownedDrawPhaseOperations.Get().HasDrawPhaseObligationBoundaryFrame();
    internal bool IsActualSkippedDrawContext(ProgramSkillWindowContext context) => _ownedDrawPhaseOperations.Get().IsActualSkippedDrawContext(context);
    internal bool TryBeginActualDrawCompletion(CharacterState owner, GameEngine.DelayedTurnEffects delayedEffects) => _ownedDrawPhaseOperations.Get().TryBeginActualDrawCompletion(owner, (int)delayedEffects);
    internal void ReturnDrawPhaseObligationWindow(ProgramLifecycleTriggerWindowFrame completed) => _ownedDrawPhaseOperations.Get().ReturnDrawPhaseObligationWindow(completed);
}
