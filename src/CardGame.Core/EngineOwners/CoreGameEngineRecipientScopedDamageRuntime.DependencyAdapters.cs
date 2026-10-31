#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreRecipientScopedDamageCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId) => this._targetFactory().GetProgramAttackState(ownerFrameId);
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }

    public void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update) => this._targetFactory().UpdateProgramAttackState(ownerFrameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime))]
internal sealed class CoreRecipientScopedDamageCoreSignedDamagePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreSignedDamagePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> _targetFactory = targetFactory;
    public bool HasSignedDamagePayments { get => this._targetFactory().HasSignedDamagePayments; }

    public bool IsValidSignedDamagePayment(global::CardGame.Core.ProgramSkillFrame f, bool requirePaid) => this._targetFactory().IsValidSignedDamagePayment(f, requirePaid);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreRecipientScopedDamageCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime))]
internal sealed class CoreRecipientScopedDamageCorePassiveDamageCapsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCorePassiveDamageCapsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _targetFactory = targetFactory;
    public int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => this._targetFactory().GetIncomingFixedDamageCap(target, visibleSkillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed class CoreRecipientScopedDamageCharacterCheZhouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCharacterCheZhouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryCheZhouRules { get => this._targetFactory().HasAnniversaryCheZhouRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime))]
internal sealed class CoreRecipientScopedDamageCorePassiveRulePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCorePassiveRulePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasPublicEnemyParticipantDamageModifiers { get => this._targetFactory().HasPublicEnemyParticipantDamageModifiers; }

    public bool HasEligiblePublicEnemyDamageObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<int> propagatedTargets) => this._targetFactory().HasEligiblePublicEnemyDamageObserver(attack, propagatedTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime))]
internal sealed class CoreRecipientScopedDamageCoreProgramVirtualDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreProgramVirtualDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> _targetFactory = targetFactory;
    public int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().DamageProgramUseInstructionIndex(attack);
    public int? FrozenProgramDuelInstructionIndex(long frameId) => this._targetFactory().FrozenProgramDuelInstructionIndex(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime))]
internal sealed class CoreRecipientScopedDamageCoreLateFixedTargetDamageCapScopesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreLateFixedTargetDamageCapScopesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> _targetFactory = targetFactory;
    public bool IsValidLateFixedTargetDamageCapScope(global::CardGame.Core.RecipientScopedDamageState scope, long attackFrameId, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsValidLateFixedTargetDamageCapScope(scope, attackFrameId, targets);
    public (global::System.Collections.Generic.IReadOnlyList<int> Targets, int Index, global::CardGame.Core.RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().FixedDamageCapChain(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreRecipientScopedDamageCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreRecipientScopedDamageCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreRecipientScopedDamageCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecipientScopedDamageCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}
