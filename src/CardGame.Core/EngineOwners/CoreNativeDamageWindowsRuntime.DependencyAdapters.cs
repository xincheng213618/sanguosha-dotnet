#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime))]
internal sealed class CoreNativeDamageWindowsCoreOrderedPrintedSkillLossAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreOrderedPrintedSkillLossOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> _targetFactory = targetFactory;
    public bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferOrderedPrintedSkillLoss(ownerSeat, trigger, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeDamageWindowsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterZhaoXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterZhaoXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoXiangOlRuntime> _targetFactory = targetFactory;
    public bool CanGainMeiYingFromDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int source, int target, bool sourceLess) => this._targetFactory().CanGainMeiYingFromDamage(owner, trigger, source, target, sourceLess);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime))]
internal sealed class CoreNativeDamageWindowsCoreDamageCounterpartAcquisitionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreDamageCounterpartAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> _targetFactory = targetFactory;
    public bool CanOfferDamageCounterpartAcquisition(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferDamageCounterpartAcquisition(owner, trigger, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreNativeDamageWindowsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public int DamageInstancesTakenThisTurn(int ownerSeat) => this._targetFactory().DamageInstancesTakenThisTurn(ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public bool CanOfferDianhuDamage(int owner, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferDianhuDamage(owner, t, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterZhangFenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterZhangFenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _targetFactory = targetFactory;
    public void AppendDagongCheIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => this._targetFactory().AppendDagongCheIssuedDamageCandidates(attack, window, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public void AppendGuanHaiYouqueDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => this._targetFactory().AppendGuanHaiYouqueDamageCandidates(attack, window, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterKanZeAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterKanZeAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> _targetFactory = targetFactory;
    public void AppendAnniversaryKanZeDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => this._targetFactory().AppendAnniversaryKanZeDamageCandidates(attack, window, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public bool CanDrawAfterKuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int target) => this._targetFactory().CanDrawAfterKuiDamage(owner, trigger, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterHuLieMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterHuLieMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuLieMouRuntime> _targetFactory = targetFactory;
    public void AppendMouHuLieDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => this._targetFactory().AppendMouHuLieDamageCandidates(attack, window, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreNativeDamageWindowsCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreNativeDamageWindowsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateAfterDamageProgramContext(global::CardGame.Core.DamageTriggerWindowFrame damage, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateAfterDamageProgramContext(damage, candidate);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ExposeProgramTriggerDecision(candidate, context);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterBaoXinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterBaoXinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoXinOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame CaptureBaoXinYimouWindow(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CaptureBaoXinYimouWindow(frame, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterSimaHuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterSimaHuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaHuiOlRuntime> _targetFactory = targetFactory;
    public bool CanOfferSimaHuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferSimaHuiDamage(owner, trigger, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime))]
internal sealed class CoreNativeDamageWindowsCoreDamageAdjacencyServicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreDamageAdjacencyServicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageAdjacencyServicesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageAdjacencyRuntime DamageAdjacency { get => this._targetFactory().DamageAdjacency; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public void AppendZhaohuoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => this._targetFactory().AppendZhaohuoDamageCandidates(attack, window, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeDamageWindowsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreNativeDamageWindowsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime))]
internal sealed class CoreNativeDamageWindowsCoreLastDamageSourceReciprocityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreLastDamageSourceReciprocityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> _targetFactory = targetFactory;
    public bool CanOfferLastDamageSourceReciprocity(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferLastDamageSourceReciprocity(ownerSeat, skillId, trigger, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreNativeDamageWindowsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder() => this._targetFactory().CurrentTableOrder();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public void AppendZhangYanSujiDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => this._targetFactory().AppendZhangYanSujiDamageCandidates(attack, window, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterMangYaChangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterMangYaChangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> _targetFactory = targetFactory;
    public void AppendAnniversaryJiedaoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow timing, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => this._targetFactory().AppendAnniversaryJiedaoDamageCandidates(attack, timing, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterXiZhiCaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterXiZhiCaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> _targetFactory = targetFactory;
    public bool CanOfferXianfuDamage(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().CanOfferXianfuDamage(ownerSeat, trigger, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreNativeDamageWindowsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDamageWindowsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public void AppendMouLuSuIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> result) => this._targetFactory().AppendMouLuSuIssuedDamageCandidates(attack, window, result);
}
