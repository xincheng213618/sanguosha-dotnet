#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28122, SkillProgramEffectOp.ResolveWenrenSequentialDraw)]
    internal bool CanActivateWenren(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == c.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<WenrenCommittedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Phase == p);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWenrenSequentialDraw)]
    internal void ReturnWenrenDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.TaoQian!;
        if (!r.Paid || r.Stage != TaoQianStage.Drawing || r.Cursor >= r.Targets.Count || outcome.RecipientSeat != r.Targets[r.Cursor] || outcome.RequestedCount != r.DrawAmount)
            throw new InvalidOperationException("Wenren's native draw returned to a different frozen recipient or amount.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Stage = TaoQianStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenrenDrawReturnedEvent(f.Id, r.Cursor, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueTaoQian(f.Id);
    }
}
