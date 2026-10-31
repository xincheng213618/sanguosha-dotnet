#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreMovementEquipmentEffectsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreMovementEquipmentEffectsKernelCoreGameEngineMovementEquipmentEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMovementEquipmentEffectsKernelCoreGameEngineMovementEquipmentEffectsOperations
    {
        public global::CardGame.Core.NativeOwners.MovementArmorOwner CreateMovementArmorOwner(global::CardGame.Core.CharacterState owner) => engine.ComposeNativeMovementArmorOwner(owner);
    }
}
