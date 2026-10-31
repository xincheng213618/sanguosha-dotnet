#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class CharacterZhangLiaoMobileGodCoreNativeEquipmentInvocationsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangLiaoMobileGodCoreNativeEquipmentInvocationsOperations
    {
        public bool NativeEquipmentObserverEdge(int index) => engine.NativeEquipmentInvocationsEntries.NativeEquipmentObserverEdge(index);
    }
}
