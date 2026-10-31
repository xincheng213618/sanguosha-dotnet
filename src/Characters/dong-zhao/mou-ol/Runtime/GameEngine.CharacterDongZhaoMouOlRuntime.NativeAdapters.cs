#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlKernelCoreGameEngineProgramAttackAttemptAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlKernelCoreGameEngineProgramAttackAttemptOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class CharacterDongZhaoMouOlCharacterDongZhaoMouOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterDongZhaoMouOlCharacterDongZhaoMouOlOperations
    {
        public bool MouDongZhaoObserverEdge(int index) => engine.DongZhaoMouOlEntries.MouDongZhaoObserverEdge(index);
        public bool MouDongZhaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.DongZhaoMouOlEntries.MouDongZhaoFirstChild(f, child);
        public bool MouDongZhaoObserverSuffix(int index) => engine.DongZhaoMouOlEntries.MouDongZhaoObserverSuffix(index);
    }
}
