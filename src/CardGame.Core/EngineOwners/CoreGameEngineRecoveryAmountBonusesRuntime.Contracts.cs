#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecoveryAmountBonusesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRecoveryAmountBonusesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreRecoveryAmountBonusesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecoveryAmountBonusesCharacterCaoSongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int SupportedCaoSongRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount);
}

internal interface ICoreRecoveryAmountBonusesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecoveryAmountBonusesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRecoveryAmountBonusesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
