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
        var discarded = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunYuanEntityMovedEvent>()
            .Where(e => e.FrameId == f.Id && e.Stage == SunYuanStage.DiscardChildren && e.To == CardLocation.DiscardPile)
            .ToDictionary(e => e.CardId);
        var eligible = r.Materials
            .Where(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.EffectiveKind) &&
                discarded.TryGetValue(c.CardId, out var paid) &&
                _zoneEntries.CurrentEntry(CardLocation.DiscardPile, c.CardId) == paid.Sequence)
            .Select(c => c.CardId)
            .ToArray();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || eligible.Length == 0)
        {
            FinishSunYuan(f, true);
            return;
        }

        IssueSunYuanMove(f, eligible, CardLocation.Hand(f.OwnerSeat), SunYuanStage.ReclaimChildren, DongxinReclaimReason, false, movement =>
        {
            var original = r.Materials.Single(c => c.CardId == movement.CardId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DongxinEntityAcquiredEvent(
                f.Id, r.Source, f.OwnerSeat, movement.CardId, original.EffectiveKind, discarded[movement.CardId].Sequence, movement.Sequence));
        });
        ContinueSunYuan(f.Id);
    }
}
