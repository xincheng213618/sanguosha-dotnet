#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileCoreObserverSubtreesOperations
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileCharacterWangFuZhaoLeiMobileAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileCharacterWangFuZhaoLeiMobileOperations
    {
        public bool XunyiFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.WangFuZhaoLeiMobileEntries.XunyiFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterWangFuZhaoLeiMobileContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangFuZhaoLeiMobileContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
