#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCharacterStateProgramsCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishVirtualBasicUse(long id);
}

internal interface ICoreCharacterStateProgramsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped);
}

internal interface ICoreCharacterStateProgramsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreCharacterStateProgramsCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSoleIncomingRangeDraw(long id);
}

internal interface ICoreCharacterStateProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreCharacterStateProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreCharacterStateProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void AdvanceRuntimeProgram(long frameId);
    void AdvanceRuntimeFrame(long frameId);
}

internal interface ICoreCharacterStateProgramsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDragonBoatCardEffect(long id);
}

internal interface ICoreCharacterStateProgramsCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNativeDrawTwo(long id);
}

internal interface ICoreCharacterStateProgramsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWumiaoFlood(long id);
}

internal interface ICoreCharacterStateProgramsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreCharacterStateProgramsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreCharacterStateProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCharacterStateProgramsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceAfterDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped);
}

internal interface ICoreCharacterStateProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCharacterStateProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreCharacterStateProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCharacterStateProgramsCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginChainedStateBasicChild(long parentId, long childId);
    void ResumeChainedStateBasicState(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame child);
}

internal interface ICoreCharacterStateProgramsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreCharacterStateProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreCharacterStateProgramsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuanduCardEffect(long id);
}

internal interface ICoreCharacterStateProgramsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueWenheCardEffect(long id);
}

internal interface ICoreCharacterStateProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreCharacterStateProgramsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedDiciChainTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectIssuedDiciChainCandidates(global::CardGame.Core.CharacterStateChangeContext change);
}

internal interface ICoreCharacterStateProgramsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireGiftRetentionObligations();
}
