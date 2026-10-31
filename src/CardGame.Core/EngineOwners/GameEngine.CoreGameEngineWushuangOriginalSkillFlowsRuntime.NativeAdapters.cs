#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreWushuangOriginalSkillFlowsKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillFlowsKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CoreWushuangOriginalSkillFlowsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillFlowsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreWushuangOriginalSkillFlowsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreWushuangOriginalSkillFlowsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
