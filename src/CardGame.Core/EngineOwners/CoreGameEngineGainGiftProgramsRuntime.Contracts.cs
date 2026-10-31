#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGainGiftProgramsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreGainGiftProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreGainGiftProgramsCoreFireTargetBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDrainFireTargetMovement(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGainGiftProgramsCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceProgramJudgment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreGainGiftProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreGainGiftProgramsCoreGainGiftObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertGainGiftReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreGainGiftProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGainGiftProgramsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId);
}

internal interface ICoreGainGiftProgramsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreGainGiftProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreGainGiftProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreGainGiftProgramsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreGainGiftProgramsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveNativeGivenCards(long producerFrameId, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreGainGiftProgramsCharacterZhugeJinMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileMingzheRegistration { get; }
}

internal interface ICoreGainGiftProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreGainGiftProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGainGiftProgramsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreGainGiftProgramsCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
}

internal interface ICoreGainGiftProgramsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreGainGiftProgramsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreGainGiftProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreGainGiftProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreGainGiftProgramsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementTiming? CaptureMovementTiming();
    int[] MatchingOwnerBatchMovementIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger);
}

internal interface ICoreGainGiftProgramsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreGainGiftProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreGainGiftProgramsCoreDelegatedJudgmentChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDelegatedJudgmentDraft(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.DelegatedJudgmentDraft d);
}

internal interface ICoreGainGiftProgramsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}
