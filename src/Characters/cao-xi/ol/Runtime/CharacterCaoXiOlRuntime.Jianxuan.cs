#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal void IssueCaoXiDraw(ProgramSkillFrame f)
    {
        var r = f.CaoXiJianxuan!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var cursor = checked(r.Cursor + 1);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiJianxuan = r with { Cursor = cursor, Before = before, After = before, ActualDrawCount = 0 } });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat!.Value], 1, true, new(CaoXiDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiJianxuan = f.CaoXiJianxuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualDrawCount = drawn.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiJianxuanDrawPaidEvent(f.Id, cursor, r.RecipientSeat.Value, drawn.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
