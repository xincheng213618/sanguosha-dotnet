#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryKernelCoreMainOperations
    {
        public global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public global::CardGame.Core.CommandResult Accept(global::System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryCharacterZhaoTongZhaoGuangAnniversaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryCharacterZhaoTongZhaoGuangAnniversaryOperations
    {
        public bool AnniversaryQingrenFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhaoTongZhaoGuangAnniversaryEntries.AnniversaryQingrenFirstChild(f, child);
        public bool AnniversaryQingrenObserverSuffix(int root) => engine.ZhaoTongZhaoGuangAnniversaryEntries.AnniversaryQingrenObserverSuffix(root);
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhaoTongZhaoGuangAnniversaryKernelCoreGameEngineRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhaoTongZhaoGuangAnniversaryKernelCoreGameEngineRuntimeOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }
}
