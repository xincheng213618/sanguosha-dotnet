#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamagePreventionDrawRuntime
{
    internal SkillProgramStepOutcome PreventDamageAndDrawMultiple(ProgramSkillFrame frame, int multiplier)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target } context || target != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != context.ParentFrameId || parent.TargetSeat != target || parent.Prevented || parent.Amount <= 0 || context.Amount != parent.Amount)
            throw new InvalidOperationException("Prevention draw requires the exact original own before-damage parent amount.");
        var drawCount = checked(parent.Amount * multiplier);
        var beforeSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0L : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        // The one atomic instruction commits prevention before drawing. A draw
        // observer returns at the next instruction, never executing this twice.
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, drawCount, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason($"skill-program.{(frame.SkillId)}.prevented-damage-draw"));
        var afterSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? beforeSequence : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        var actualCount = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > beforeSequence && m.Sequence <= afterSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == $"skill-program.{(frame.SkillId)}.prevented-damage-draw");
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { PreventionDrawReceipt = new(current.InstructionIndex - 1, parent.Id, parent.SourceSeat, parent.TargetSeat, parent.Amount, multiplier, beforeSequence, afterSequence, actualCount) });
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 570)]
    internal bool IsPreventionDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? paidEffect, ProgramMovementContinuation movement) => paidEffect?.Op is SkillProgramEffectOp.PreventCurrentDamageAndDrawMultiple or SkillProgramEffectOp.PreventParticipantDamageDrawActualSource && movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null && frame.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0 } context && (paidEffect.Op == SkillProgramEffectOp.PreventParticipantDamageDrawActualSource || context.TargetSeat == frame.OwnerSeat) && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(parent => parent.Id == context.ParentFrameId && _corePreventionDrawObserversOperations.IsValidPreventionDrawReceipt(frame, parent));
    SkillProgramStepOutcome global::CardGame.Core.IDamagePreventionDrawProgramHost.PreventDamageAndDrawMultiple(ProgramSkillFrame frame, int multiplier) => PreventDamageAndDrawMultiple(frame, multiplier);
}
