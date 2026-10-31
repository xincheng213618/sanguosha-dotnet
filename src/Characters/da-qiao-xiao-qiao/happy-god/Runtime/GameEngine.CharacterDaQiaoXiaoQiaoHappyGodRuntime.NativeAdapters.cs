#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool SameNameHandObserverSuffix(int index) => engine.SameNameHandDiscardOrDamageObserversEntries.SameNameHandObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineActionQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineActionQueriesOperations
    {
        public global::CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(global::CardGame.Core.NativeOwners.ActionQuery? previous = default) => engine.ComposeNativeActionQueryScope(previous);
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodCharacterDaQiaoXiaoQiaoHappyGodAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodCharacterDaQiaoXiaoQiaoHappyGodOperations
    {
        public bool HappyQiaoFirstSkillChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.DaQiaoXiaoQiaoHappyGodEntries.HappyQiaoFirstSkillChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterDaQiaoXiaoQiaoHappyGodProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDaQiaoXiaoQiaoHappyGodProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }
}
