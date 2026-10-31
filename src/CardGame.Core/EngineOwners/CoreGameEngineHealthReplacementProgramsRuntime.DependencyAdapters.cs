#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat) => this._targetFactory().BuildDyingResponderSeats(victimSeat);
    public void ExposeHumanDyingPrompt() => this._targetFactory().ExposeHumanDyingPrompt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => this._targetFactory().TryBeginIdentityAppearanceForDamage(attack, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreQimeiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreQimeiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> _targetFactory = targetFactory;
    public bool HasQimeiHealthSubject(int seat) => this._targetFactory().HasQimeiHealthSubject(seat);
    public void CaptureQimeiHpQualification(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureQimeiHpQualification(change);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreHealthReplacementProgramsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyQiaoHealth(int seat) => this._targetFactory().HasHappyQiaoHealth(seat);
    public global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false) => this._targetFactory().CurrentHappyQiaoPool(seat, parentFrameId, healthWrite);
    public void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after) => this._targetFactory().CompleteHappyQiaoHealthPayment(parentFrameId, targetSeat, kind, before, after);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreHealthReplacementProgramsCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool HasWenyiHealthSubject(int targetSeat) => this._targetFactory().HasWenyiHealthSubject(targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CalculateCardDamageLayer(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime))]
internal sealed class CoreHealthReplacementProgramsCharacterDongBaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCharacterDongBaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> _targetFactory = targetFactory;
    public void ObserveDongBaiHpDecreased(int seat, int before, int after) => this._targetFactory().ObserveDongBaiHpDecreased(seat, before, after);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
    public bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying) => this._targetFactory().TryBeginMandatorySelfDyingProgram(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind) => this._targetFactory().RecordHpChange(parentFrameId, sourceSeat, targetSeat, before, after, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreDyingEntryProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreDyingEntryProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying) => this._targetFactory().TryBeginDyingEntryProgramWindow(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreHealthQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreHealthQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().TryBeginHappyQiaoAttackHpLossChoice(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed class CoreHealthReplacementProgramsCoreGrantedEntityPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCoreGrantedEntityPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _targetFactory = targetFactory;
    public long? GrantedEntityTrailerHealthOwner(global::CardGame.Core.IGameEvent payload) => this._targetFactory().GrantedEntityTrailerHealthOwner(payload);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed class CoreHealthReplacementProgramsCharacterSunJianMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHealthReplacementProgramsCharacterSunJianMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _targetFactory = targetFactory;
    public void CaptureMouSunJianOlHp(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureMouSunJianOlHp(change);
}
