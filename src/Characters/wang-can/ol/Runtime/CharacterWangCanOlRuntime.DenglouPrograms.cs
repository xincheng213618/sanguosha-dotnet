#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal void BeginDenglouLease(ProgramSkillFrame f)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var lease = WangCanLease(f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = f.WangCan!with { Lease = lease, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = new List<int>();
        for (var i = 0; i < 4 && _corePhysicalDrawFacadesOperations.EnsureDrawPile(); i++)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
            ids.Add(card.Id);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, lease, new(WangCanReason(WangCanStage.DenglouViewChildren)));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = f.WangCan!with { Entities = ids, Remaining = ids, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = ids.Count, LeaseBefore = before, LeaseAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        IssueWangCanSegment(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueDenglou(ProgramSkillFrame f)
    {
        var r = f.WangCan!;
        var remaining = r.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == r.Lease).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r = r with { Remaining = remaining, Stage = WangCanStage.DenglouChoose, Native = null }, PendingMovementContinuation = null });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            PayWangCanMove(f, remaining, CardLocation.DiscardPile, WangCanStage.DenglouCancel);
            return;
        }

        if (remaining.Length == 0)
        {
            FinishWangCan(f);
            return;
        }

        var choices = WangCanChoices(f);
        if (choices.Count == 0)
        {
            PayWangCanMove(f, remaining, CardLocation.DiscardPile, WangCanStage.DenglouDiscard);
            return;
        }

        PublishWangCan(f, f.OwnerSeat, choices, "登楼：选择仍暂存的一张基本牌及真实使用目标。");
    }

    internal CardSnapshot[] DenglouPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.WangCan?.Lease is not null).SelectMany(f => f.WangCan!.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == f.WangCan.Lease).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
}
