#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CorePindianCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CorePindianCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginDouyuPairPreReveal(global::CardGame.Core.PindianFrame frame, int opponentCardId, bool opponentTop) => this._targetFactory().TryBeginDouyuPairPreReveal(frame, opponentCardId, opponentTop);
    public global::CardGame.Core.PindianResult ApplyDouyuPairResult(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PindianResult native) => this._targetFactory().ApplyDouyuPairResult(frame, native);
    public bool IsDouyuPairSubtree(global::CardGame.Core.PindianFrame pair) => this._targetFactory().IsDouyuPairSubtree(pair);
    public void AssertDouyuWindows() => this._targetFactory().AssertDouyuWindows();
    public void AssertDouyuPairSubtreeProcessing(global::CardGame.Core.PindianFrame pair) => this._targetFactory().AssertDouyuPairSubtreeProcessing(pair);
    public bool IsDouyuPairAssignment(global::CardGame.Core.PindianFrame pair, global::CardGame.Core.PindianResult result) => this._targetFactory().IsDouyuPairAssignment(pair, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CorePindianCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public int? CaptureAlcoholPindianRank(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, bool source, bool top) => this._targetFactory().CaptureAlcoholPindianRank(frame, owner, card, source, top);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed class CorePindianCorePindianRevealWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianRevealWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginPairPindianReveal(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PindianResult result) => this._targetFactory().TryBeginPairPindianReveal(frame, result);
    public bool IsPairPindianRevealSubtree(global::CardGame.Core.PindianFrame pair) => this._targetFactory().IsPairPindianRevealSubtree(pair);
    public void AssertPindianRevealWindows() => this._targetFactory().AssertPindianRevealWindows();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime))]
internal sealed class CorePindianCorePindianClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianClaimsRuntime> _targetFactory = targetFactory;
    public void ResolvePindianClaimChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolvePindianClaimChoice(selected);
    public bool BeginPindianClaims(global::CardGame.Core.PindianFrame frame) => this._targetFactory().BeginPindianClaims(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CorePindianCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CorePindianCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CorePindianCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed class CorePindianCorePindianPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> _targetFactory = targetFactory;
    public bool CanOfferPindianTopChoice(global::CardGame.Core.CharacterState owner) => this._targetFactory().CanOfferPindianTopChoice(owner);
    public bool HasCurrentPindianSourceCard(global::CardGame.Core.PindianFrame frame) => this._targetFactory().HasCurrentPindianSourceCard(frame);
    public int ReserveOrReadPindianTop(global::CardGame.Core.CharacterState owner, bool reserve) => this._targetFactory().ReserveOrReadPindianTop(owner, reserve);
    public int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectivePindianRank(owner, card);
    public int[] CurrentPindianProcessingIds(global::CardGame.Core.PindianFrame frame) => this._targetFactory().CurrentPindianProcessingIds(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime))]
internal sealed class CorePindianCharacterZhuRongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterZhuRongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public void CaptureMobileZhuRongPindianReturn(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianFrame native) => this._targetFactory().CaptureMobileZhuRongPindianReturn(parent, native);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CorePindianCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CorePindianCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginMobileZhitiPindianPrograms(global::CardGame.Core.PindianFrame parent) => this._targetFactory().TryBeginMobileZhitiPindianPrograms(parent);
    public bool IsMobileZhitiContestSubtree(global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().IsMobileZhitiContestSubtree(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime))]
internal sealed class CorePindianCharacterJianYongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterJianYongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryJianYongZongshiSubtree(global::CardGame.Core.PindianFrame p) => this._targetFactory().IsBoundaryJianYongZongshiSubtree(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CorePindianCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().JuewuForeignHandRank(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CorePindianCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CorePindianCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CorePindianCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime))]
internal sealed class CorePindianCorePindianOwnedPileSourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianOwnedPileSourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePindianOwnedPileSourcesRuntime> _targetFactory = targetFactory;
    public void AssertOwnedPilePindianSource(global::CardGame.Core.PindianFrame frame) => this._targetFactory().AssertOwnedPilePindianSource(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CorePindianCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public bool HasWuLuKangPindianInput(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasWuLuKangPindianInput(owner);
    public bool CanWuLuKangDiscardForPindian(global::CardGame.Core.CharacterState owner) => this._targetFactory().CanWuLuKangDiscardForPindian(owner);
    public bool TryBeginWuLuKangPairBeforeReveal(global::CardGame.Core.PindianFrame f, int opponentCardId, bool top) => this._targetFactory().TryBeginWuLuKangPairBeforeReveal(f, opponentCardId, top);
    public global::CardGame.Core.WuLuKangPindianInput WuLuKangActualInput(global::CardGame.Core.ResolutionFrame parent, int seat, int id, global::CardGame.Core.CardLocation from, int rank, bool top) => this._targetFactory().WuLuKangActualInput(parent, seat, id, from, rank, top);
    public void RevealWuLuKangInputs(global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().RevealWuLuKangInputs(parent);
    public bool IsWuLuKangPairSubtree(global::CardGame.Core.PindianFrame pair) => this._targetFactory().IsWuLuKangPairSubtree(pair);
    public void AssertWuLuKangWindows() => this._targetFactory().AssertWuLuKangWindows();
    public void AssertWuLuKangPairProcessing(global::CardGame.Core.PindianFrame pair) => this._targetFactory().AssertWuLuKangPairProcessing(pair);
    public bool IsWuLuKangAssignedPair(global::CardGame.Core.PindianFrame f, global::CardGame.Core.PindianResult result) => this._targetFactory().IsWuLuKangAssignedPair(f, result);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime))]
internal sealed class CorePindianCorePindianHandReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianHandReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> _targetFactory = targetFactory;
    public void RecordPindianHandContestReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PindianFrame native) => this._targetFactory().RecordPindianHandContestReturn(f, native);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed class CorePindianCoreEquipmentSlotGroupProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreEquipmentSlotGroupProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _targetFactory = targetFactory;
    public bool CanBePindianTarget(int sourceSeat, int targetSeat) => this._targetFactory().CanBePindianTarget(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CorePindianCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime))]
internal sealed class CorePindianCorePindianMaximumSlashClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianMaximumSlashClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianMaximumSlashClaimsRuntime> _targetFactory = targetFactory;
    public void AssertPindianPolicyClaims(global::CardGame.Core.PindianFrame frame) => this._targetFactory().AssertPindianPolicyClaims(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime))]
internal sealed class CorePindianCorePindianRandomSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCorePindianRandomSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> _targetFactory = targetFactory;
    public bool TryContinuePindianRandomSelection(global::CardGame.Core.PindianFrame frame) => this._targetFactory().TryContinuePindianRandomSelection(frame);
    public bool TryResolvePindianRandomChoice(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryResolvePindianRandomChoice(frame, selected);
    public bool IsPindianRandomPrompt(global::CardGame.Core.PindianFrame frame) => this._targetFactory().IsPindianRandomPrompt(frame);
    public bool AssertPindianRandomSelection(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PendingDecision decision) => this._targetFactory().AssertPindianRandomSelection(frame, decision);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed class CorePindianCoreForeignTurnContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePindianCoreForeignTurnContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ForeignTurnPindianOrigin? FreezeForeignTurnPindianOrigin(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.PindianFrame pindian) => this._targetFactory().FreezeForeignTurnPindianOrigin(program, pindian);
}
