#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChengOlRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PublicCardsAfterResolution, 10)]
    internal IReadOnlyList<CardSnapshot> ChengliePublicCards() => !HasMaChengCapability ? [] : Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.Chenglie is { Outcome: false }).SelectMany(f => f.Chenglie!.Revealed).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray());
    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 20)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeChenglieSnapshots(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll)
    {
        if (!HasMaChengCapability)
            return ordinary;
        var piles = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.ChenglieLease is { Settled: false }).Select(u => u.ChenglieLease!).SelectMany(l => l.Cards.Where(c => c.HolderSeat == holder && _cardZonesOperations._cardZones.GetLocation(c.CardId) == c.Location).Select(c => (Lease: l, Card: c))).GroupBy(x => x.Lease.Source.OwnerSeat).Select(g => new NamedAmbushPileSnapshot("ol:chenglie", "骋烈", g.Count(), revealAll || g.Key == viewer ? Array.AsReadOnly(g.Select(x => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(x.Card.CardId)), x.Card.Kind)).ToArray()) : null)).ToArray();
        if (piles.Length == 0)
            return ordinary;
        return Array.AsReadOnly((ordinary ?? []).Concat(piles).ToArray());
    }

    internal bool IsChengliePrivateProcessingCard(int cardId) => HasMaChengCapability && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Chenglie is { Outcome: false } r && r.Stage is ChenglieStage.Exchange or ChenglieStage.ExchangeChildren or ChenglieStage.Assignment or ChenglieStage.AssignmentChildren or ChenglieStage.CleanupChildren && r.Pool.Contains(cardId) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing);
    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 20)]
    [ProgramViewContributor(ProgramViewContribution.PublicProcessing, 10)]
    internal IReadOnlyList<Card> WithoutChengliePrivateProcessing(IReadOnlyList<Card> cards) => !HasMaChengCapability ? cards : Array.AsReadOnly(cards.Where(c => !IsChengliePrivateProcessingCard(c.Id)).ToArray());
    internal IReadOnlyList<CardSnapshot> ChengliePrivateCards(int viewer) => !HasMaChengCapability ? [] : Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.Chenglie is { Outcome: false }).SelectMany(f => f.Chenglie!.Pool).Where(IsChengliePrivateProcessingCard).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray());
}
