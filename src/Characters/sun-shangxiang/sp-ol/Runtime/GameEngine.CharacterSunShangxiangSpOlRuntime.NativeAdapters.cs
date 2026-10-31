#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSunShangxiangSpOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangSpOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSunShangxiangSpOlCharacterSunShangxiangSpOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangSpOlCharacterSunShangxiangSpOlOperations
    {
        public bool SpSunShangxiangLiangzhuObserverEdge(int n) => engine.SunShangxiangSpOlEntries.SpSunShangxiangLiangzhuObserverEdge(n);
        public bool SpSunShangxiangLiangzhuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SunShangxiangSpOlEntries.SpSunShangxiangLiangzhuFirstChild(f, child);
        public bool SpSunShangxiangLiangzhuSuffix(int n) => engine.SunShangxiangSpOlEntries.SpSunShangxiangLiangzhuSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterSunShangxiangSpOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangSpOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSunShangxiangSpOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangSpOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSunShangxiangSpOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangSpOlCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
