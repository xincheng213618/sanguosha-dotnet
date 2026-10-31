#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSkillProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsNativeCardGameCoreProgramLegalityRouterAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsNativeCardGameCoreProgramLegalityRouterOperations
    {
        public global::CardGame.Core.ProgramLegalityRouter CreateProgramLegalityRouter(global::CardGame.Core.ProgramLegalityCatalog catalog, global::System.Func<global::System.Reflection.MethodInfo, object?>? resolveTarget = default) => new global::CardGame.Core.ProgramLegalityRouter(catalog, engine, resolveTarget);
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsProgramLegalityAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsProgramLegalityOperations
    {
        public ref global::CardGame.Core.ProgramLegalityRouter? _programLegality => ref engine._programLegality;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSkillProgramsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsKernelCoreProgramRuntimeModulesOperations
    {
        public object? ResolveRuntimeHandlerTarget(global::System.Reflection.MethodInfo method) => engine.ResolveRuntimeHandlerTarget(method);
    }
}
