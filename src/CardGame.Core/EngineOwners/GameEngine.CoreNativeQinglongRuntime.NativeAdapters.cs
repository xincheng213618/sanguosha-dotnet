#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeQinglongKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeQinglongKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeQinglongKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeQinglongKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQinglongCrescentBladeHandle(attack);
    }
}
