#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeFactionDefenseResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionDefenseResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeFactionDefenseKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionDefenseKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, global::CardGame.Core.FactionResponsePolicySource source) => engine.ComposeNativeFactionDefenseHandle(attack, ownerSeat, candidateSeats, source);
    }

    [ProgramCapability]
    private sealed class CoreNativeFactionDefenseKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionDefenseKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
