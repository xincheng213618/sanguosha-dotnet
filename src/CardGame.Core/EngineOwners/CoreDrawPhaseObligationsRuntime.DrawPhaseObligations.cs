#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDrawPhaseObligationsRuntime
{
    internal bool HasDrawPhaseObligationBoundaryFrame() => _ownedDrawPhaseOperations.Get().HasDrawPhaseObligationBoundaryFrame();
    internal bool IsActualSkippedDrawContext(ProgramSkillWindowContext context) => _ownedDrawPhaseOperations.Get().IsActualSkippedDrawContext(context);
    internal bool TryBeginActualDrawCompletion(CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects) => _ownedDrawPhaseOperations.Get().TryBeginActualDrawCompletion(owner, (int)delayedEffects);
    internal void ReturnDrawPhaseObligationWindow(ProgramLifecycleTriggerWindowFrame completed) => _ownedDrawPhaseOperations.Get().ReturnDrawPhaseObligationWindow(completed);
}
