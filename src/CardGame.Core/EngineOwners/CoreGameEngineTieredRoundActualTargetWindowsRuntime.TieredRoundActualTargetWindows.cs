#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTieredRoundActualTargetWindowsRuntime
{
    internal void SyncIssuedTieredRoundZeroTrickTargetWindows(long useId, CardActionContext previousAction)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { TieredRoundConversionUse: { FrozenTier: 2, MaterialCount: 0 }, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0 } currentAction } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || !_coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(use, true))
            return;
        foreach (var window in _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Where(w => w.ParentFrameId == use.Id && w.Action.Type == CardActionType.Use && w.Action.ActionId == currentAction.ActionId && w.TrickContinuation is { EffectCardId: 0 } && w.Continuation is ProgramCardContinuation.CommittedTrick or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.BeforeTrickTargetEffects).ToArray())
        {
            // Compare against the actual pre-update producer input, not a
            // reconstructed target subset or an arbitrary same-id action.
            if (!global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime.TieredRoundActionsStructurallyMatch(window.Action, previousAction))
                throw new InvalidOperationException("A zero-material trick window lost its exact pre-update action.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Action = currentAction });
        }
    }
}
