#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCardActionsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreCardActionsKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreCardActionsKernelCoreGameEngineProgramTriggerWindowsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsKernelCoreGameEngineProgramTriggerWindowsOperations
    {
        public global::CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class CoreCardActionsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCardActionsModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreCardActionsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreCardActionsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCardActionsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreCardActionsKernelCoreMainAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsKernelCoreMainOperations2
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CoreCardActionsModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class CoreCardActionsProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCardActionsProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }
}
