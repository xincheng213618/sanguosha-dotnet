#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineParticipantPreventionRuntime
{
    internal SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } c || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != c.ParentFrameId || parent.Prevented || parent.Amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.SourceSeat) || parent.SourceSeat == parent.TargetSeat || f.OwnerSeat != parent.SourceSeat && f.OwnerSeat != parent.TargetSeat || !_coreMatchParticipantsPlayersAccess._players[parent.SourceSeat].IsAlive || c.SourceSeat != parent.SourceSeat || c.TargetSeat != parent.TargetSeat || c.Amount != parent.Amount)
            throw new InvalidOperationException("Participant prevention lost its exact sourced damage attempt.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, parent.SourceSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(f.SkillId)}.prevented-damage-draw"));
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(parent.SourceSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.prevented-damage-draw");
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PreventionDrawReceipt = new(f.InstructionIndex - 1, parent.Id, parent.SourceSeat, parent.TargetSeat, parent.Amount, 2, before, after, actual) });
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
    }

    internal bool IsParticipantPreventionDraw(ProgramSkillFrame f) => f.InstructionIndex > 0 && ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op == SkillProgramEffectOp.PreventParticipantDamageDrawActualSource;
    SkillProgramStepOutcome global::CardGame.Core.IParticipantPreventionHost.PreventParticipantDamageDrawActualSource(ProgramSkillFrame f) => PreventParticipantDamageDrawActualSource(f);
}
