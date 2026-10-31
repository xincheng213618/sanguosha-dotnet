#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeWeaponResponsesKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeWeaponResponsesKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeCixiongDoubleSwordsHandle(attack);
    }

    [ProgramCapability]
    private sealed class CoreNativeWeaponResponsesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeWeaponResponsesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
