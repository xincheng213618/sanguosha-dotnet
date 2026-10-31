#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 27390, SkillProgramEffectOp.RecastZhubiAndMarkActualDraw)]
    internal bool AllowsLiuBaRecast(ProgramActivationLegalityContext c) => _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((c.OwnerSeat, c.SkillId, c.Activation.UsageGroup)) < c.Owner.MaxHp && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && LiuBaRecastEntities(p.Seat).Length > 0);
    internal LiuBaEntity[] LiuBaRecastEntities(int seat) => new[]
    {
        CardLocation.Hand(seat),
        CardLocation.Equipment(seat)
    }.SelectMany(l => _cardZonesOperations._cardZones.CardsAt(l).Select(c => new LiuBaEntity(c.Id, c.Kind, l))).OrderBy(c => c.CardId).ToArray();
    internal LiuBaEntity[] LiuBaCoins(int seat) => LiuBaOwnedEntities(seat).Where(e => HasLiuBaCoinEntity(e.CardId)).ToArray();
    internal void PayLiuBaRecast(ProgramSkillFrame f, LiuBaEntity material)
    {
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.LiuBa!.Recipient, [material.CardId]);
        PrepareLiuBaMovement(ref f, LiuBaStage.RecastChildren, [material]);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([material.CardId], CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (batch, records) => RecordLiuBaMovement(f, batch, records));
        CompleteLiuBaMovement(f, 1);
    }

    internal void PayLiuBaExchange(ProgramSkillFrame f)
    {
        var r = f.LiuBa!;
        var live = LiuBaCoins(f.OwnerSeat);
        if (r.Coins.Count == 0 || r.Coins.Count != r.SelectedBottom.Count || r.Coins.Distinct().Count() != r.Coins.Count || r.SelectedBottom.Distinct().Count() != r.SelectedBottom.Count || r.Entities.Any(e => !live.Contains(e)) || !r.ViewedBottom.SequenceEqual(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(r.ViewedBottom.Count).Select(c => c.Id)))
            throw new InvalidOperationException("Liu Ba exchange requires both exact frozen original sets.");
        var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
        var mapping = r.SelectedBottom.Select((id, i) => (id, coin: r.Coins[i])).ToDictionary(x => x.id, x => x.coin);
        var desired = deck.Select(id => mapping.GetValueOrDefault(id, id)).ToArray();
        var entities = r.Entities.Concat(r.SelectedBottom.Select(id => new LiuBaEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, CardLocation.DrawPile))).ToArray();
        PrepareLiuBaMovement(ref f, LiuBaStage.ExchangeChildren, entities);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), CardLocation.DrawPile, new(LiuBaExchangeReason), (batch, records) =>
        {
            RecordLiuBaMovement(f, batch, records);
            var now = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Select(c => c.Id).ToArray();
            _cardZonesOperations._cardZones.ReorderDrawPileTop(now, desired.Reverse().ToArray(), []);
        }, (card, from) => from == CardLocation.DrawPile ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DrawPile);
        CompleteLiuBaMovement(f, entities.Length);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 50)]
    internal IReadOnlyList<CardSnapshot> LiuBaPrivatelyViewedBottomCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { LiuBa: { Stage: LiuBaStage.Coins or LiuBaStage.Bottom } r } f && f.OwnerSeat == viewerSeat && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { IsPrivate: true, PlayerSeat: var actor } && actor == viewerSeat ? r.ViewedBottom.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray() : [];
}
