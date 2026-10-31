#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRecipientScopedDamageCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId);
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }

    void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update);
}

internal interface ICoreRecipientScopedDamageCoreSignedDamagePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSignedDamagePayments { get; }

    bool IsValidSignedDamagePayment(global::CardGame.Core.ProgramSkillFrame f, bool requirePaid);
}

internal interface ICoreRecipientScopedDamageResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRecipientScopedDamageCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreRecipientScopedDamageCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICoreRecipientScopedDamageContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreRecipientScopedDamageKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRecipientScopedDamageModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreRecipientScopedDamageCharacterCheZhouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCheZhouRules { get; }
}

internal interface ICoreRecipientScopedDamageCorePassiveRulePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPublicEnemyParticipantDamageModifiers { get; }

    bool HasEligiblePublicEnemyDamageObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> propagatedTargets);
}

internal interface ICoreRecipientScopedDamageCoreProgramVirtualDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    int? FrozenProgramDuelInstructionIndex(long frameId);
}

internal interface ICoreRecipientScopedDamageCoreLateFixedTargetDamageCapScopesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidLateFixedTargetDamageCapScope(global::CardGame.Core.RecipientScopedDamageState scope, long attackFrameId, global::System.Collections.Generic.IReadOnlyList<int> targets);
    (global::System.Collections.Generic.IReadOnlyList<int> Targets, int Index, global::CardGame.Core.RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreRecipientScopedDamageCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreRecipientScopedDamageCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreRecipientScopedDamageCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
