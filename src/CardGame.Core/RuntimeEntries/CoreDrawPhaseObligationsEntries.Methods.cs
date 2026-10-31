#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDrawPhaseObligationsEntries
{
    internal bool HasDrawPhaseObligationBoundaryFrame() => _resolve().HasDrawPhaseObligationBoundaryFrame();
    internal bool IsActualSkippedDrawContext(ProgramSkillWindowContext context) => _resolve().IsActualSkippedDrawContext(context);
    internal void ReturnDrawPhaseObligationWindow(ProgramLifecycleTriggerWindowFrame completed) => _resolve().ReturnDrawPhaseObligationWindow(completed);
    internal bool TryBeginActualDrawCompletion(CharacterState owner, GameEngine.DelayedTurnEffects delayedEffects) => _resolve().TryBeginActualDrawCompletion(owner, delayedEffects);
}
