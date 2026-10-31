#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PlanHappyWeiYanGainDestination(card, from, requestedTo, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreGainGiftProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreGainGiftProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _targetFactory = targetFactory;
    public void RecordDelegatedJudgmentPayment(global::CardGame.Core.JudgmentFrame j, long batchId) => this._targetFactory().RecordDelegatedJudgmentPayment(j, batchId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreDelegatedJudgmentChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreDelegatedJudgmentChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelegatedJudgmentChoicesRuntime> _targetFactory = targetFactory;
    public bool IsDelegatedJudgment(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsDelegatedJudgment(trigger);
    public void BeginDelegatedJudgmentChoice(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().BeginDelegatedJudgmentChoice(j, candidate, program, trigger, cards);
    public bool TryResolveDelegatedJudgmentChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryResolveDelegatedJudgmentChoice(selected);
    public bool ResolveAiDelegatedJudgmentChoice() => this._targetFactory().ResolveAiDelegatedJudgmentChoice();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreRiceJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreRiceJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRiceJudgmentRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> RiceJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().RiceJudgmentReplacementCards(owner, trigger);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default) => this._targetFactory().RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    public void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ClearJudgmentEffectiveKindAfterMove(card, from, to);
    public void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().ResolveSilverLionRemoval(card, from, reason);
    public void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveWoodenOxMove(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreRicePileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreRicePileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> _targetFactory = targetFactory;
    public bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p) => this._targetFactory().RiceProducer(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreEquipmentSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreEquipmentSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _targetFactory = targetFactory;
    public void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveEquipmentSkillGrant(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementBatchBuilder BeginCardMovementBatch(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> destinations) => this._targetFactory().BeginCardMovementBatch(sources, destinations);
    public void CompleteCardMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements, bool committed) => this._targetFactory().CompleteCardMovementBatch(batch, movements, committed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCharacterWangYiMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCharacterWangYiMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> _targetFactory = targetFactory;
    public bool HasDeckTopJudgmentReplacementMaterial() => this._targetFactory().HasDeckTopJudgmentReplacementMaterial();
    public void BeginDeckTopJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().BeginDeckTopJudgmentReplacementChoice(pending, candidate, program, trigger);
    public bool TryResolveDeckTopJudgmentReplacement(global::CardGame.Core.PromptChoice selected, global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().TryResolveDeckTopJudgmentReplacement(selected, pending, candidate, program, trigger);
    public bool IsDeckTopJudgmentReplacementInput(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsDeckTopJudgmentReplacementInput(judgment, owner, card);
    public bool TryResolveAiDeckTopJudgmentReplacement() => this._targetFactory().TryResolveAiDeckTopJudgmentReplacement();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void AddThought(global::CardGame.Core.AiThoughtRecord thought) => this._targetFactory().AddThought(thought);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public bool IsProtectedJudgmentSourceEquipment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, int judgmentSubjectSeat, string judgmentReason) => this._targetFactory().IsProtectedJudgmentSourceEquipment(owner, card, judgmentSubjectSeat, judgmentReason);
    public global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().CurrentJudgmentCandidate(pending);
    public void AdvanceJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().AdvanceJudgmentCandidate(pending);
    public void SetJudgmentFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetJudgmentFrameStep(frameId, step);
    public void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().ReplaceJudgmentFrame(frame);
    public global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId) => this._targetFactory().GetJudgmentFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreJudgmentReplacementProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentReplacementProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}
