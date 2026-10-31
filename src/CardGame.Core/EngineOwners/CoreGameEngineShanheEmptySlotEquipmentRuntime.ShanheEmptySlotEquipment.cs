#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheEmptySlotEquipmentRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheEmptySlotEquipmentProgramHost.PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly) => PlaceShanheEmptySlotEquipment(frame, armorOnly);
    internal Card[] ShanheEmptySlotEquipmentCandidates(int ownerSeat, bool armorOnly = false)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        if (!owner.IsAlive || armorOnly && _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor))
            return[];
        // Shanhe excludes equipment before dealing and before recycling. Its
        // random equipment skills use the registered deck's printed templates,
        // creating a native entity directly in the field, never in DrawPile.
        var pool = _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is { StartingResourcesExpireOnUse: true } ? global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.DeckId)) : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
        return pool.Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && (!armorOnly || _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Armor) && _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count(equipped => _coreFieldEquipmentOperations.FieldEquipmentSlot(equipped) == _coreFieldEquipmentOperations.FieldEquipmentSlot(card)) < owner.EquipmentSlotCapacity(_coreFieldEquipmentOperations.FieldEquipmentSlot(card))).OrderBy(card => card.Id).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28080)]
    internal bool CanOfferShanheEmptySlotEquipment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(effect => effect.Op is SkillProgramEffectOp.PlaceRandomEmptySlotDeckEquipment or SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment) || context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && ShanheEmptySlotEquipmentCandidates(candidate.OwnerSeat, trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment)).Length > 0;
    internal SkillProgramStepOutcome PlaceShanheEmptySlotEquipment(ProgramSkillFrame supplied, bool armorOnly)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatAccess._currentSeat)
            throw new InvalidOperationException("Empty-slot equipment placement requires its owner's actual preparation frame.");
        var candidates = ShanheEmptySlotEquipmentCandidates(frame.OwnerSeat, armorOnly);
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        var from = CardLocation.DrawPile;
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is { StartingResourcesExpireOnUse: true })
        {
            card = card with
            {
                Id = _coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1
            };
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
            from = CardLocation.OutsideGame;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheRandomEquipmentCreatedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, card.Id, card.Kind, card.Suit, card.Rank));
        }

        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
        // Native placement has no Use declaration, processing or replacement.
        // Every entity enters through one real movement into remaining capacity.
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(frame.OwnerSeat), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, frame.OwnerSeat, slot, card.Id, card.Kind, null));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 将【{(card.DisplayName)}】置入空装备栏。", frame.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }
}
