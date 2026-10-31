#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiAnniversaryGodRuntime
{
    internal bool IsShenDaCarrier(int id) => id > 0 && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenDengAiCarrierCreatedEvent>().Where(e => e.CardId == id).ToArray()is [var f] && global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime.IsShenDaCuixin(f.Source.SkillId) && _contentRegistryOperations._contentRegistry.GetSkill(f.Source.SkillId).Program?.GameplayHash == f.GameplayHash && _coreAdvancedLifecycleOperations.GetAdvancedCard(id)is { Suit: Suit.None, Rank: 0 } c && c.Kind == f.Kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(c.Kind);
    internal CardLocation NormalizeShenDaCarrierDestination(Card card, CardLocation from, CardLocation to)
    {
        if (!IsShenDaCarrier(card.Id) || from == to || to == CardLocation.OutsideGame)
            return to;
        if (from == CardLocation.OutsideGame)
            return to == CardLocation.Processing && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id) && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { ShenDengAiCuixin: { } o } u && o.CarrierId == card.Id && IsShenDaCuixinUse(u.Id) ? to : CardLocation.OutsideGame;
        if (from.Zone == CardZoneKind.Judgment)
            return to.Zone == CardZoneKind.Judgment ? to : CardLocation.OutsideGame;
        return to.Zone is CardZoneKind.Judgment or CardZoneKind.Processing ? to : CardLocation.OutsideGame;
    }

    internal void RecordShenDaCarrierDestruction(CardMovementRecord m)
    {
        if (m.To != CardLocation.OutsideGame || !IsShenDaCarrier(m.CardId))
            return;
        if (m.From == CardLocation.OutsideGame || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenDengAiCarrierDestroyedEvent>().Any(e => e.CardId == m.CardId))
            throw new InvalidOperationException("A Cuixin delayed-card carrier cannot be destroyed twice or re-enter a reusable zone.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiCarrierDestroyedEvent(m.CardId, m.Sequence, m.From, m.Reason));
    }

    internal bool IsShenDaCarrierDestroyedMaterial(CardMovementRecord m) => m.To == CardLocation.OutsideGame && IsShenDaCarrier(m.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenDengAiCarrierDestroyedEvent>().Where(e => e.CardId == m.CardId).ToArray()is [var f] && f.MovementSequence == m.Sequence && f.From == m.From && f.Reason == m.Reason;
    internal bool IsShenDaCarrierDestroyedMaterial(int id, CardLocation from, CardLocation to) => to == CardLocation.OutsideGame && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.From == from && m.To == to && IsShenDaCarrierDestroyedMaterial(m));
    internal bool IsShenDaCarrierDestroyedMaterial(long use, int id) => _coreCardUseLifecycleOperations.LifecycleCardUse(use)?.Action?.PhysicalCards.SingleOrDefault(c => c.CardId == id)is { } c && IsShenDaCarrierDestroyedMaterial(id, c.From, CardLocation.OutsideGame);
    internal void RetireUnplacedShenDaCarrier(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ShenDengAiCuixin: { CarrierId: > 0 } o } && IsShenDaCuixinUse(id) && _cardZonesOperations._cardZones.GetLocation(o.CarrierId) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(o.CarrierId), CardLocation.Processing, CardLocation.OutsideGame, new("skill-program.shen-deng-ai.cuixin.carrier-retired"));
    }
}
