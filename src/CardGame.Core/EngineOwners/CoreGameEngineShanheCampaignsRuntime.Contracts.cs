#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheCampaignsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreShanheCampaignsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICoreShanheCampaignsCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PlaceAdvancedEquipment(global::CardGame.Core.Card card, int targetSeat);
}

internal interface ICoreShanheCampaignsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}
