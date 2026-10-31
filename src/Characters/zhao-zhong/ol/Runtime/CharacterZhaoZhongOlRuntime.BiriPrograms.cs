#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBiriFirstNonDrawGain)]
    internal void ReturnBiriDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ZhaoZhong!;
        if (!r.Paid || r.Stage != ZhaoZhongStage.Drawing || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Biri returned to another issued native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiriDrawReturnedEvent(f.Id, 1, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueZhaoZhong(f.Id);
    }
}
