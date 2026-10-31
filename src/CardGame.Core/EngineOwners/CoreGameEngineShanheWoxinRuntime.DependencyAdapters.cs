#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime))]
internal sealed class CoreShanheWoxinCoreDamageArmorPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheWoxinCoreDamageArmorPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? DamageArmorPaymentOwner(global::CardGame.Core.DamageFrame damage) => this._targetFactory().DamageArmorPaymentOwner(damage);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreShanheWoxinCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheWoxinCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreShanheWoxinCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheWoxinCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId) => this._targetFactory().GetProgramAttackState(ownerFrameId);
    public void UpdateProgramAttackState(long ownerFrameId, global::System.Func<global::CardGame.Core.AttackAttemptState, global::CardGame.Core.AttackAttemptState> update) => this._targetFactory().UpdateProgramAttackState(ownerFrameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreShanheWoxinCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheWoxinCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreShanheWoxinCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheWoxinCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
