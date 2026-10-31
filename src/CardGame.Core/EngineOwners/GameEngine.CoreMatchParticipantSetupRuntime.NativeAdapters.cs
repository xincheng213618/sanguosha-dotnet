#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreMatchParticipantSetupRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }
}
