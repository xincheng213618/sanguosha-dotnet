#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePindianCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICorePindianCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDouyuPairPreReveal(global::CardGame.Core.PindianFrame frame, int opponentCardId, bool opponentTop);
    global::CardGame.Core.PindianResult ApplyDouyuPairResult(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PindianResult native);
    bool IsDouyuPairSubtree(global::CardGame.Core.PindianFrame pair);
    void AssertDouyuWindows();
    void AssertDouyuPairSubtreeProcessing(global::CardGame.Core.PindianFrame pair);
    bool IsDouyuPairAssignment(global::CardGame.Core.PindianFrame pair, global::CardGame.Core.PindianResult result);
}

internal interface ICorePindianCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CaptureAlcoholPindianRank(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, bool source, bool top);
}

internal interface ICorePindianCorePindianRevealWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPairPindianReveal(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PindianResult result);
    bool IsPairPindianRevealSubtree(global::CardGame.Core.PindianFrame pair);
    void AssertPindianRevealWindows();
}

internal interface ICorePindianCorePindianClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePindianClaimChoice(global::CardGame.Core.PromptChoice selected);
    bool BeginPindianClaims(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICorePindianCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICorePindianCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICorePindianCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferPindianTopChoice(global::CardGame.Core.CharacterState owner);
    bool HasCurrentPindianSourceCard(global::CardGame.Core.PindianFrame frame);
    int ReserveOrReadPindianTop(global::CardGame.Core.CharacterState owner, bool reserve);
    int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    int[] CurrentPindianProcessingIds(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianCharacterZhuRongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMobileZhuRongPindianReturn(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianFrame native);
}

internal interface ICorePindianCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICorePindianCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMobileZhitiPindianPrograms(global::CardGame.Core.PindianFrame parent);
    bool IsMobileZhitiContestSubtree(global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICorePindianCharacterJianYongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryJianYongZongshiSubtree(global::CardGame.Core.PindianFrame p);
}

internal interface ICorePindianCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICorePindianCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICorePindianCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICorePindianCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICorePindianCorePindianOwnedPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertOwnedPilePindianSource(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICorePindianCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuLuKangPindianInput(global::CardGame.Core.CharacterState owner);
    bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner);
    bool TryBeginWuLuKangPairBeforeReveal(global::CardGame.Core.PindianFrame f, int opponentCardId, bool top);
    global::CardGame.Core.WuLuKangPindianInput WuLuKangActualInput(global::CardGame.Core.ResolutionFrame parent, int seat, int id, global::CardGame.Core.CardLocation from, int rank, bool top);
    void RevealWuLuKangInputs(global::CardGame.Core.ResolutionFrame parent);
    bool IsWuLuKangPairSubtree(global::CardGame.Core.PindianFrame pair);
    void AssertWuLuKangWindows();
    void AssertWuLuKangPairProcessing(global::CardGame.Core.PindianFrame pair);
    bool IsWuLuKangAssignedPair(global::CardGame.Core.PindianFrame f, global::CardGame.Core.PindianResult result);
}

internal interface ICorePindianCorePindianHandReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordPindianHandContestReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PindianFrame native);
}

internal interface ICorePindianCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBePindianTarget(int sourceSeat, int targetSeat);
}

internal interface ICorePindianCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICorePindianCorePindianMaximumSlashClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPindianPolicyClaims(global::CardGame.Core.PindianFrame frame);
}

internal interface ICorePindianCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePindianResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePindianCorePindianRandomSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinuePindianRandomSelection(global::CardGame.Core.PindianFrame frame);
    bool TryResolvePindianRandomChoice(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected);
    bool IsPindianRandomPrompt(global::CardGame.Core.PindianFrame frame);
    bool AssertPindianRandomSelection(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PendingDecision decision);
}

internal interface ICorePindianCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ForeignTurnPindianOrigin? FreezeForeignTurnPindianOrigin(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.PindianFrame pindian);
}
