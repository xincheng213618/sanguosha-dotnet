#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreHpLossSlashMaterialsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpLossSlashMaterialsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreHpLossSlashMaterialsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpLossSlashMaterialsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreHpLossSlashMaterialsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreHpLossSlashMaterialsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
