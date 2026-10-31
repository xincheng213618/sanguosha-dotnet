#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAtomicCardMovesRuntime
{
    internal void MoveProgramCardsFromMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null, Func<Card, CardLocation, CardLocation>? destinationForCard = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified, Action<HappyWeiYanGainRequestSettlement>? afterRequestSettled = null, long? producerFrameId = null)
    {
        if (afterPhysicalRecords is not null && afterRequestSettled is not null)
            throw new InvalidOperationException("A gain request must choose one physical or request-settlement continuation.");
        var sequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        var entries = ids.Select(id =>
        {
            var source = _cardZonesOperations._cardZones.GetLocation(id);
            var card = _cardZonesOperations._cardZones.CardsAt(source).Single(item => item.Id == id);
            var target = destinationForCard?.Invoke(card, source) ?? destination;
            var phantomDestination = _characterZuoCiHappyGodOperations.PlanHappyZuoCiPhantomDestination(card, source, target, reason);
            if (source.Zone == CardZoneKind.Equipment && card.IsGeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, source, target))
                target = CardLocation.OutsideGame;
            target = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, source, target, reason);
            target = _characterZhongHuiAnniversaryGodOperations.NormalizeLishiDiscardDestination(card, source, target, reason);
            var gainDestination = _coreHappyWeiYanGainDestinationsOperations.PlanHappyWeiYanGainDestination(card, source, target, reason);
            target = gainDestination?.ActualTo ?? target;
            if (_characterZuoCiHappyGodOperations.NormalizeHappyZuoCiPhantomDestination(card, source, target) != target)
                throw new InvalidOperationException("A second atomic destination replacement requires the phantom's original request invoice.");
            return (Card: card, Source: source, Target: target, GainDestination: gainDestination, PhantomDestination: phantomDestination);
        }).ToArray();
        if (entries.Length == 0)
            return;
        if (ids.Distinct().Count() != ids.Count || entries.Any(item => item.Source == item.Target && (item.GainDestination is null || item.Source.Zone != CardZoneKind.Hand || item.GainDestination.From != item.Source || item.GainDestination.ActualTo != item.Target)))
            throw new InvalidOperationException("An atomic card move requires distinct cards leaving their source.");
        var retainedEntries = entries.Where(item => item.Source == item.Target).ToArray();
        if (retainedEntries.Length > 0 && afterPhysicalRecords is not null)
            throw new InvalidOperationException("A replaced gain retaining its source hand requires a typed request-settlement continuation, not physical movement evidence.");
        var producerId = producerFrameId ?? _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id;
        if ((retainedEntries.Length > 0 || afterRequestSettled is not null) && producerId is null)
            throw new InvalidOperationException("A gain request settlement requires its original active producer.");
        var retained = retainedEntries.Select(item => _coreHappyWeiYanGainDestinationsOperations.SettleHappyWeiYanRetainedGain(item.GainDestination!, producerId)).ToArray();
        entries = entries.Where(item => item.Source != item.Target).ToArray();
        if (entries.Length == 0)
        {
            afterRequestSettled?.Invoke(new(producerId!.Value, null, sequenceBefore, reason) { RetainedGains = retained });
            return;
        }

        var shiyou = entries.Any(item => _coreHellActivitiesOperations.CanPlanHellDiscard(item.Target)) ? _coreHellMengpoProgramsOperations.PrepareHellShiyouDiscardTransfers(entries.Select(item => new CardTransfer(item.Card.Id, item.Source, item.Target)).ToArray(), reason) : null;
        if (shiyou is not null)
            entries = entries.Select((item, index) => (item.Card, item.Source, Target: shiyou.Transfers[index].To, item.GainDestination, item.PhantomDestination)).ToArray();
        if (entries.Any(item => item.Source == item.Target))
            throw new InvalidOperationException("An atomic discard replacement must still leave its original source.");
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch(entries.Select(item => item.Source), entries.Select(item => item.Target));
        var movements = new List<CardMovementRecord>(entries.Length);
        var committed = false;
        try
        {
            // Commit all physical cards before removal hooks observe the discard cost.
            foreach (var group in entries.GroupBy(item => (item.Source, item.Target)))
                _cardZonesOperations._cardZones.MoveMany(group.Select(item => item.Card.Id), group.Key.Source, group.Key.Target);
            if (afterPhysicalRecords is not null || afterRequestSettled is not null)
            {
                foreach (var item in entries)
                    movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(item.Card, item.Source, item.Target, reason, disclosure: disclosure, happyWeiYanGainDestination: item.GainDestination, happyZuoCiPhantomDestination: item.PhantomDestination));
                if (shiyou is not null)
                    _coreHellMengpoProgramsOperations.CompleteHellShiyouDiscardTransfers(shiyou, movements);
                var records = Array.AsReadOnly(movements.ToArray());
                if (afterRequestSettled is not null)
                    afterRequestSettled(new(producerId!.Value, batch.Id, sequenceBefore, reason) { Movements = records, RetainedGains = retained });
                else
                    afterPhysicalRecords!(batch.Id, records);
                foreach (var item in entries)
                {
                    _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(item.Card, item.Source, item.Target);
                    _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(item.Card, item.Source, item.Target);
                    _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(item.Card, item.Source, reason);
                    _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(item.Card, item.Source, item.Target);
                    _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(item.Card, item.Source, item.Target);
                }
            }
            else
                foreach (var item in entries)
                {
                    movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(item.Card, item.Source, item.Target, reason, disclosure: disclosure, happyWeiYanGainDestination: item.GainDestination, happyZuoCiPhantomDestination: item.PhantomDestination));
                    _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(item.Card, item.Source, item.Target);
                    _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(item.Card, item.Source, item.Target);
                    _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(item.Card, item.Source, reason);
                    _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(item.Card, item.Source, item.Target);
                    _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(item.Card, item.Source, item.Target);
                }

            if (afterPhysicalRecords is null && afterRequestSettled is null && shiyou is not null)
                _coreHellMengpoProgramsOperations.CompleteHellShiyouDiscardTransfers(shiyou, movements);
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, movements, committed);
        }

        foreach (var item in entries.Where(item => item.Card.Kind == CardKind.ReturnSoul))
            _coreHellSoulCardsOperations.DestroyHellReturnSoulIfDiscarded(item.Card, _cardZonesOperations._cardZones.GetLocation(item.Card.Id));
    }

    internal bool IsRedirectedPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(move, expectedSource, expectedReason) || _characterZhongHuiAnniversaryGodOperations.IsLishiPaidDiscardMovement(move, expectedSource, expectedReason) || move.From == expectedSource && move.Reason == expectedReason && move.HappyZuoCiPhantomDestination is { RequestedTo: var requested } && requested == CardLocation.DiscardPile && _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedMaterial(move);
}
