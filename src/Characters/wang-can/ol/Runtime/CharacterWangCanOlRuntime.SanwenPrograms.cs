#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal void PaySanwenDraw(ProgramSkillFrame f)
    {
        var r = f.WangCan!;
        var count = r.ActualCount * 2;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Stage = WangCanStage.SanwenDraw, Before = before, After = before, ActualCount = 0, Cursor = count }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], count, true, new(WangCanReason(WangCanStage.SanwenDraw))) : [];
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = f.WangCan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = cards.Count } });
        IssueWangCanSegment(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
