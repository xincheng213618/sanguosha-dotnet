#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesProgramPhaseScheduleAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesProgramPhaseScheduleOperations
    {
        public ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule => ref engine._programPhaseSchedule;
    }

    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreGrantedEntityPhasesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGrantedEntityPhasesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
