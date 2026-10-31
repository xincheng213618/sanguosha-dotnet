#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime))]
internal sealed class CoreWenyiDamageHealthCoreDamageArmorPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreDamageArmorPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageArmorPaymentsRuntime> _targetFactory = targetFactory;
    public bool IsIssuedDamageArmorPayment(global::CardGame.Core.DamageFrame damage) => this._targetFactory().IsIssuedDamageArmorPayment(damage);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed class CoreWenyiDamageHealthCoreNativeDamageWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreNativeDamageWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().TryBeginDamageTriggerWindow(attack, damageFrameId, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime))]
internal sealed class CoreWenyiDamageHealthCoreNativeDamageCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreNativeDamageCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CompleteDamageAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreWenyiDamageHealthCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreWenyiDamageHealthCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState killer) => this._targetFactory().BeginDying(attack, damageFrameId, victim, killer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreWenyiDamageHealthCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get => this._targetFactory().CurrentDamageAttempt; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreWenyiDamageHealthCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreWenyiDamageHealthCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreWenyiDamageHealthCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWenyiDamageHealthCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool HasWenyiHealthSubject(int targetSeat) => this._targetFactory().HasWenyiHealthSubject(targetSeat);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectWenyiHpCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectWenyiHpCandidates(change);
    public bool HasHuanShuJunCapability { get => this._targetFactory().HasHuanShuJunCapability; }

    public bool HuanShuJunPaidOrPreparing(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().HuanShuJunPaidOrPreparing(f);
    public bool WenyiHpRoot(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().WenyiHpRoot(f);
    public bool IsIssuedWenyiUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsIssuedWenyiUse(use);
    public global::CardGame.Core.ProgramSkillFrame? HuanShuJunObserverRoot() => this._targetFactory().HuanShuJunObserverRoot();
}
