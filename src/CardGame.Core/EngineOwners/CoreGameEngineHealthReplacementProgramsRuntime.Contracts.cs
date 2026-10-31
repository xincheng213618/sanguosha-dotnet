#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHealthReplacementProgramsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat);
    void ExposeHumanDyingPrompt();
}

internal interface ICoreHealthReplacementProgramsCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount);
}

internal interface ICoreHealthReplacementProgramsCoreQimeiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQimeiHealthSubject(int seat);
    void CaptureQimeiHpQualification(global::CardGame.Core.HpChangeContext change);
}

internal interface ICoreHealthReplacementProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreHealthReplacementProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyQiaoHealth(int seat);
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
    void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after);
}

internal interface ICoreHealthReplacementProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHealthReplacementProgramsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWenyiHealthSubject(int targetSeat);
}

internal interface ICoreHealthReplacementProgramsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreHealthReplacementProgramsCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CalculateCardDamageLayer(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreHealthReplacementProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreHealthReplacementProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreHealthReplacementProgramsCharacterDongBaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveDongBaiHpDecreased(int seat, int before, int after);
}

internal interface ICoreHealthReplacementProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHealthReplacementProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHealthReplacementProgramsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind);
}

internal interface ICoreHealthReplacementProgramsCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHealthReplacementProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreHealthReplacementProgramsCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreHealthReplacementProgramsCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? GrantedEntityTrailerHealthOwner(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreHealthReplacementProgramsCharacterSunJianMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouSunJianOlHp(global::CardGame.Core.HpChangeContext change);
}
