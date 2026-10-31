#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal void AdvanceGongjieDonor(ProgramSkillFrame f)
    {
        var r = f.CaoYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Index = r.Index + 1, Stage = CaoYuStage.GongjieTake, Materials = [], GrainIds = [], Before = 0, After = 0, Count = 0 }, PendingMovementContinuation = null });
        if (f.CaoYu!.Index >= r.Targets.Count)
            AfterGongjieGifts(f);
        else
            PublishCaoYu(f);
    }

    internal void AfterGongjieGifts(ProgramSkillFrame f)
    {
        var count = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuCardPaidEvent>().Where(e => e.FrameId == f.Id && e.Stage == CaoYuStage.GongjieGiftChildren && e.Suit != Suit.None).Select(e => e.Suit).Distinct().Count();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishCaoYu(f);
            return;
        }

        DrawCaoYu(f, count, CaoYuStage.GongjieDrawChildren, GongjieDrawReason);
    }
}
