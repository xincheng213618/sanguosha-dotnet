#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreActualDiscardPhaseCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public void ExpireCaoChunPlayDebt(int owner, int phaseInstance) => this._targetFactory().ExpireCaoChunPlayDebt(owner, phaseInstance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreActualDiscardPhaseCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => this._targetFactory().ConsumeDuFuRenPhase(kind, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreActualDiscardPhaseCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().MoveLogicalDiscardCards(initiatorSeat, executorSeat, cards, from, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginSkillInvocation(long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string? skillInstanceId = default, global::CardGame.Core.SkillInvocationReturnKind returnKind = (global::CardGame.Core.SkillInvocationReturnKind)0) => this._targetFactory().TryBeginSkillInvocation(parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId, returnKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterLuMengMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterLuMengMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMobileRuntime> _targetFactory = targetFactory;
    public bool TryBeginPreEntryDiscardPolicy(global::CardGame.Core.CharacterState current) => this._targetFactory().TryBeginPreEntryDiscardPolicy(current);
    public bool TryResolvePreEntryDiscardPolicy(bool useSkill) => this._targetFactory().TryResolvePreEntryDiscardPolicy(useSkill);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreActualDiscardPhaseCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void EndTurn() => this._targetFactory().EndTurn();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreActualDiscardPhaseCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool TryBeginPlayEndingProgramWindow(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginPlayEndingProgramWindow(owner);
    public bool TryBeginDiscardPhaseProgramWindow(global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginDiscardPhaseProgramWindow(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreActualDiscardPhaseCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime))]
internal sealed class CoreActualDiscardPhaseCoreActualDiscardRecoveryPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreActualDiscardRecoveryPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveryPhasesRuntime> _targetFactory = targetFactory;
    public void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor) => this._targetFactory().StartActualDiscardRecoveryPhase(kind, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreActualDiscardPhaseCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CoreActualDiscardPhaseCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public bool HasCurrentIssuedPhaseHandDebt() => this._targetFactory().HasCurrentIssuedPhaseHandDebt();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterQiaoZhouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterQiaoZhouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> _targetFactory = targetFactory;
    public bool TrySkipQiaoZhouDiscardPhase(global::CardGame.Core.CharacterState current) => this._targetFactory().TrySkipQiaoZhouDiscardPhase(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreActualDiscardPhaseCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableHand(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetSelfDiscardableHand(owner);
    public int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner) => this._targetFactory().RequiredSelfHandLimitDiscard(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreActualDiscardPhaseCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetHandLimit(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHandLimit(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public bool HasCurrentIssuedWeiluDebt(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasCurrentIssuedWeiluDebt(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime))]
internal sealed class CoreActualDiscardPhaseCharacterDuXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualDiscardPhaseCharacterDuXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuXiOlRuntime> _targetFactory = targetFactory;
    public bool TrySkipDuXiDiscardPhase(global::CardGame.Core.CharacterState player) => this._targetFactory().TrySkipDuXiDiscardPhase(player);
}
