#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreJudgmentReplacementProgramsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreJudgmentReplacementProgramsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreJudgmentReplacementProgramsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreJudgmentReplacementProgramsCoreGainGiftProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordDelegatedJudgmentPayment(global::CardGame.Core.JudgmentFrame j, long batchId);
}

internal interface ICoreJudgmentReplacementProgramsCoreDelegatedJudgmentChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDelegatedJudgment(global::CardGame.Core.SkillProgramTrigger trigger);
    void BeginDelegatedJudgmentChoice(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
    bool TryResolveDelegatedJudgmentChoice(global::CardGame.Core.PromptChoice selected);
    bool ResolveAiDelegatedJudgmentChoice();
}

internal interface ICoreJudgmentReplacementProgramsCoreRiceJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> RiceJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger);
}

internal interface ICoreJudgmentReplacementProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default);
    void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
    void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreJudgmentReplacementProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreJudgmentReplacementProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreJudgmentReplacementProgramsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreJudgmentReplacementProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreJudgmentReplacementProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
    void ClearPendingDecision();
}

internal interface ICoreJudgmentReplacementProgramsCoreRicePileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p);
}

internal interface ICoreJudgmentReplacementProgramsCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreJudgmentReplacementProgramsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementBatchBuilder BeginCardMovementBatch(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> destinations);
    void CompleteCardMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements, bool committed);
}

internal interface ICoreJudgmentReplacementProgramsCharacterWangYiMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDeckTopJudgmentReplacementMaterial();
    void BeginDeckTopJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger);
    bool TryResolveDeckTopJudgmentReplacement(global::CardGame.Core.PromptChoice selected, global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger);
    bool IsDeckTopJudgmentReplacementInput(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool TryResolveAiDeckTopJudgmentReplacement();
}

internal interface ICoreJudgmentReplacementProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreJudgmentReplacementProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreJudgmentReplacementProgramsCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreJudgmentReplacementProgramsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreJudgmentReplacementProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreJudgmentReplacementProgramsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreJudgmentReplacementProgramsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreJudgmentReplacementProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreJudgmentReplacementProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreJudgmentReplacementProgramsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreJudgmentReplacementProgramsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProtectedJudgmentSourceEquipment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, int judgmentSubjectSeat, string judgmentReason);
    global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
    void AdvanceJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
    void SetJudgmentFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
    void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId);
}

internal interface ICoreJudgmentReplacementProgramsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreJudgmentReplacementProgramsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreJudgmentReplacementProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreJudgmentReplacementProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}
