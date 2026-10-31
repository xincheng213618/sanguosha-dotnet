#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreLateFixedTargetDamageCapScopesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreLateFixedTargetDamageCapScopesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreLateFixedTargetDamageCapScopesCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(global::CardGame.Core.CharacterState target, int? sourceSeat, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default);
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId);
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }

    void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update);
}

internal interface ICoreLateFixedTargetDamageCapScopesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreProgramVirtualDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreLateFixedTargetDamageCapScopesCoreDamageArmorPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? DamageArmorPaymentOwner(global::CardGame.Core.DamageFrame damage);
    bool IsIssuedDamageArmorPayment(global::CardGame.Core.DamageFrame damage);
}
