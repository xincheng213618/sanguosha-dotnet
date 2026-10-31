#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramAttackAttemptCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramAttackAttemptKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramAttackAttemptCorePassiveRulePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPerOccurrenceSourceDamageModifiers(int sourceSeat);
}

internal interface ICoreProgramAttackAttemptCorePassiveDamageCapsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextRecipientPassiveDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalTengjia { get; }

    bool UsesFormalSilverLion { get; }
}

internal interface ICoreProgramAttackAttemptCoreDynamicDiscardDamagePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureDynamicDiscardDamageCompleted(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramAttackAttemptCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramAttackAttemptCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreProgramAttackAttemptCoreRecoveryDamageEdgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertRecoveryDamageOrigin(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramAttackAttemptCharacterJiangWeiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileJwSuspendedDying(long attackId);
}

internal interface ICoreProgramAttackAttemptCoreProgramOwnerDeathDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOwnerDeathSuspendedDying(long programFrameId);
}

internal interface ICoreProgramAttackAttemptCoreNativeDrawThresholdDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertNativeDrawThresholdDyingOrigin(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramAttackAttemptCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramAttackAttemptModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreProgramAttackAttemptCoreShanheS5YimieOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginShanheS5YimieRecovery(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureWuXiongmuProgramDamageCompleted(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreProgramAttackAttemptCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreProgramAttackAttemptCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame CaptureYintianProgramDamageCompleted(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCoreRecipientScopedDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenwuRecipientDamageModifiers { get; }

    bool HasPublicEnemyRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack);
    bool HasFixedTargetRecipientDamageScope(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCoreLateFixedTargetDamageCapScopesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::System.Collections.Generic.IReadOnlyList<int> Targets, int Index, global::CardGame.Core.RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramAttackAttemptCoreSuitDamagePreventionBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureSuitPreventionDamageCompleted(global::CardGame.Core.ProgramSkillFrame f, bool applied);
}

internal interface ICoreProgramAttackAttemptCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramAttackAttemptCharacterCheZhouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryCheZhouRules { get; }
}

internal interface ICoreProgramAttackAttemptCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreProgramAttackAttemptCoreProgramDamageHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreProgramAttackAttemptCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramAttackAttemptKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}
