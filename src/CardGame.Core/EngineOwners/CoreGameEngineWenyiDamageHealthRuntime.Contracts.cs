#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWenyiDamageHealthKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreWenyiDamageHealthCoreDamageArmorPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedDamageArmorPayment(global::CardGame.Core.DamageFrame damage);
}

internal interface ICoreWenyiDamageHealthResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWenyiDamageHealthCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreWenyiDamageHealthCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreWenyiDamageHealthCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreWenyiDamageHealthCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState killer);
}

internal interface ICoreWenyiDamageHealthCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreWenyiDamageHealthCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreWenyiDamageHealthCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreWenyiDamageHealthCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWenyiDamageHealthCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWenyiHealthSubject(int targetSeat);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectWenyiHpCandidates(global::CardGame.Core.HpChangeContext change);
    bool HasHuanShuJunCapability { get; }

    bool HuanShuJunPaidOrPreparing(global::CardGame.Core.ProgramSkillFrame f);
    bool WenyiHpRoot(global::CardGame.Core.ProgramSkillFrame f);
    bool IsIssuedWenyiUse(global::CardGame.Core.CardUseFrame use);
    global::CardGame.Core.ProgramSkillFrame? HuanShuJunObserverRoot();
}
