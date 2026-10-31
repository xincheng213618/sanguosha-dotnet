#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDamageWindowsCoreOrderedPrintedSkillLossOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeDamageWindowsCharacterZhaoXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanGainMeiYingFromDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int source, int target, bool sourceLess);
}

internal interface ICoreNativeDamageWindowsCoreDamageCounterpartAcquisitionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferDamageCounterpartAcquisition(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int DamageInstancesTakenThisTurn(int ownerSeat);
}

internal interface ICoreNativeDamageWindowsCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferDianhuDamage(int owner, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendDagongCheIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result);
}

internal interface ICoreNativeDamageWindowsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendGuanHaiYouqueDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result);
}

internal interface ICoreNativeDamageWindowsCharacterKanZeAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendAnniversaryKanZeDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result);
}

internal interface ICoreNativeDamageWindowsCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanDrawAfterKuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int target);
}

internal interface ICoreNativeDamageWindowsCharacterHuLieMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendMouHuLieDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates);
}

internal interface ICoreNativeDamageWindowsCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreNativeDamageWindowsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateAfterDamageProgramContext(global::CardGame.Core.DamageTriggerWindowFrame damage, global::CardGame.Core.ProgramTriggerCandidate candidate);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
}

internal interface ICoreNativeDamageWindowsCharacterBaoXinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame CaptureBaoXinYimouWindow(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCharacterSimaHuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferSimaHuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeDamageWindowsCoreDamageAdjacencyServicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageAdjacencyRuntime DamageAdjacency { get; }
}

internal interface ICoreNativeDamageWindowsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeDamageWindowsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendZhaohuoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates);
}

internal interface ICoreNativeDamageWindowsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeDamageWindowsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeDamageWindowsCoreLastDamageSourceReciprocityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder();
}

internal interface ICoreNativeDamageWindowsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreNativeDamageWindowsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendZhangYanSujiDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates);
}

internal interface ICoreNativeDamageWindowsCharacterMangYaChangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendAnniversaryJiedaoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow timing, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates);
}

internal interface ICoreNativeDamageWindowsCharacterXiZhiCaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferXianfuDamage(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeDamageWindowsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendMouLuSuIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result);
}
