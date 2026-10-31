#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal void PayDongxin(ProgramSkillFrame f)
    {
        var r = f.SunYuan!;
        if (r.Paid || r.Selected.Count != r.Required || r.Selected.Any(id => !SunYuanEligibleCards(f.OwnerSeat, r.Holder, false).Any(c => c.Id == id)) || !DongxinDirectionAvailable(f.OwnerSeat, f.TriggerId!))
            throw new InvalidOperationException("Dongxin cannot underpay, repeat a direction, or replace its exact HE primary set.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunYuan = r with { Paid = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunYuanPaidEvent(f.Id, r.ActualTurn, r.TurnOwner, f.OwnerSeat, f.TriggerId!, r.Selected.Count));
        IssueSunYuanMove(f, r.Selected, CardLocation.DiscardPile, SunYuanStage.DiscardChildren, DongxinDiscardReason, true);
        ContinueSunYuan(f.Id);
    }

    internal void ReclaimDongxin(ProgramSkillFrame f)
    {
        var r = f.SunYuan!;
        var eligible = r.Materials.Where(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.EffectiveKind) && _cardZonesOperations._cardZones.Contains(CardLocation.DiscardPile, c.CardId) && _coreProgramEventHistoryOperations.ProgramEventHistory<SunYuanEntityMovedEvent>().SingleOrDefault(e => e.FrameId == f.Id && e.Stage == SunYuanStage.DiscardChildren && e.CardId == c.CardId)is { } paid && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == c.CardId).Sequence == paid.Sequence).Select(c => c.CardId).ToArray();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || eligible.Length == 0)
        {
            FinishSunYuan(f, true);
            return;
        }

        IssueSunYuanMove(f, eligible, CardLocation.Hand(f.OwnerSeat), SunYuanStage.ReclaimChildren, DongxinReclaimReason, false);
        ContinueSunYuan(f.Id);
    }
}
