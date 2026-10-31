#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeFactionSlashKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionSlashKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeFactionSlashResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionSlashResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeFactionSlashContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionSlashContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreNativeFactionSlashKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFactionSlashKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, global::CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, global::System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, global::CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, global::CardGame.Core.CardKind requiredKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, global::CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }
}
