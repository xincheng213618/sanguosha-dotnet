#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDrawPhaseObligationsEntries
{
    internal bool HasDrawPhaseObligationBoundaryFrame() => _resolve().HasDrawPhaseObligationBoundaryFrame();
    internal bool IsActualSkippedDrawContext(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsActualSkippedDrawContext(context);
    internal void ReturnDrawPhaseObligationWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame completed) => _resolve().ReturnDrawPhaseObligationWindow(completed);
    internal bool TryBeginActualDrawCompletion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects) => _resolve().TryBeginActualDrawCompletion(owner, delayedEffects);
}
