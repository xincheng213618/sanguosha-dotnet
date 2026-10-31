#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreModeSetupEquipmentCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreModeSetupEquipmentCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreModeSetupEquipmentCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PlaceAdvancedEquipment(global::CardGame.Core.Card card, int targetSeat);
}

internal interface ICoreModeSetupEquipmentCoreModeSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeSetupFeature ModeSetup { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ModeSeatSetupFacts> CaptureModeSetupSeats();
}

internal interface ICoreModeSetupEquipmentKernelCoreGameEngineModeSetupEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost();
}

internal interface ICoreModeSetupEquipmentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreModeSetupEquipmentCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
