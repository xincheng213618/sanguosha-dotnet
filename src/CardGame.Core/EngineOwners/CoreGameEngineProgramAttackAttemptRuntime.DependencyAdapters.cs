#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramAttackAttemptCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime))]
internal sealed class CoreProgramAttackAttemptCorePassiveRulePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCorePassiveRulePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasPerOccurrenceSourceDamageModifiers(int sourceSeat) => this._targetFactory().HasPerOccurrenceSourceDamageModifiers(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime))]
internal sealed class CoreProgramAttackAttemptCorePassiveDamageCapsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCorePassiveDamageCapsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _targetFactory = targetFactory;
    public bool HasNextRecipientPassiveDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasNextRecipientPassiveDamageCap(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreProgramAttackAttemptCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalTengjia { get => this._targetFactory().UsesFormalTengjia; }
    public bool UsesFormalSilverLion { get => this._targetFactory().UsesFormalSilverLion; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime))]
internal sealed class CoreProgramAttackAttemptCoreDynamicDiscardDamagePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreDynamicDiscardDamagePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureDynamicDiscardDamageCompleted(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().CaptureDynamicDiscardDamageCompleted(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramAttackAttemptCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreProgramAttackAttemptCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime))]
internal sealed class CoreProgramAttackAttemptCoreRecoveryDamageEdgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreRecoveryDamageEdgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> _targetFactory = targetFactory;
    public void AssertRecoveryDamageOrigin(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertRecoveryDamageOrigin(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime))]
internal sealed class CoreProgramAttackAttemptCharacterJiangWeiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCharacterJiangWeiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsMobileJwSuspendedDying(long attackId) => this._targetFactory().IsMobileJwSuspendedDying(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime))]
internal sealed class CoreProgramAttackAttemptCoreProgramOwnerDeathDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreProgramOwnerDeathDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> _targetFactory = targetFactory;
    public bool IsOwnerDeathSuspendedDying(long programFrameId) => this._targetFactory().IsOwnerDeathSuspendedDying(programFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime))]
internal sealed class CoreProgramAttackAttemptCoreNativeDrawThresholdDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreNativeDrawThresholdDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> _targetFactory = targetFactory;
    public void AssertNativeDrawThresholdDyingOrigin(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().AssertNativeDrawThresholdDyingOrigin(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramAttackAttemptCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime))]
internal sealed class CoreProgramAttackAttemptCoreShanheS5YimieAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreShanheS5YimieOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> _targetFactory = targetFactory;
    public bool TryBeginShanheS5YimieRecovery(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().TryBeginShanheS5YimieRecovery(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreProgramAttackAttemptCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureWuXiongmuProgramDamageCompleted(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CaptureWuXiongmuProgramDamageCompleted(frame, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreProgramAttackAttemptCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramAttackAttemptCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreProgramAttackAttemptCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame CaptureYintianProgramDamageCompleted(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CaptureYintianProgramDamageCompleted(frame, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime))]
internal sealed class CoreProgramAttackAttemptCoreRecipientScopedDamageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreRecipientScopedDamageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> _targetFactory = targetFactory;
    public bool HasShenwuRecipientDamageModifiers { get => this._targetFactory().HasShenwuRecipientDamageModifiers; }

    public bool HasPublicEnemyRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasPublicEnemyRecipientDamageScope(attack);
    public bool HasFixedTargetRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().HasFixedTargetRecipientDamageScope(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime))]
internal sealed class CoreProgramAttackAttemptCoreLateFixedTargetDamageCapScopesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreLateFixedTargetDamageCapScopesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> _targetFactory = targetFactory;
    public (global::System.Collections.Generic.IReadOnlyList<int> Targets, int Index, global::CardGame.Core.RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().FixedDamageCapChain(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime))]
internal sealed class CoreProgramAttackAttemptCoreSuitDamagePreventionBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreSuitDamagePreventionBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> _targetFactory = targetFactory;
    public void CaptureSuitPreventionDamageCompleted(global::CardGame.Core.ProgramSkillFrame f, bool applied) => this._targetFactory().CaptureSuitPreventionDamageCompleted(f, applied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreProgramAttackAttemptCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreProgramAttackAttemptCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed class CoreProgramAttackAttemptCharacterCheZhouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCharacterCheZhouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCheZhouAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryCheZhouRules { get => this._targetFactory().HasAnniversaryCheZhouRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreProgramAttackAttemptCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime))]
internal sealed class CoreProgramAttackAttemptCoreProgramDamageHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreProgramDamageHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramAttackAttemptCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}
