#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal void PayYinjuDraw(ProgramSkillFrame f, int seat)
    {
        var r = f.XinPi!with
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Requested = 1,
            Actual = 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { XinPi = r, PendingMovementContinuation = new(seat, 0, null) });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, new(YinjuDrawReason)).Count;
        r = r with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Actual = actual
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XinPi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinjuDrawPaidEvent(f.Id, r.Relation!, r.UseId!.Value, r.ActionId!.Value, seat, r.Before, r.After, actual));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
