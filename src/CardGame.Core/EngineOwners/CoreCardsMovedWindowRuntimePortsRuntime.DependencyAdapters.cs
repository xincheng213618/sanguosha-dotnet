#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterQiaoZhouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterQiaoZhouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoZhouOlRuntime> _targetFactory = targetFactory;
    public bool ReturnQiaoZhouPreparationMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnQiaoZhouPreparationMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreShanheNullifiedTrickClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreShanheNullifiedTrickClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheNullifiedTrickClaimsRuntime> _targetFactory = targetFactory;
    public bool IsShanheNullifiedClaimAwaitingMovement(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().IsShanheNullifiedClaimAwaitingMovement(window);
    public bool IsShanheNullifiedClaimMovement(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsShanheNullifiedClaimMovement(window, batch);
    public void ReturnShanheNullifiedClaimMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnShanheNullifiedClaimMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public bool IsWenheCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsWenheCardEffectMovement(use, batch);
    public bool IsWenheCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => this._targetFactory().IsWenheCardEffectMovementReturn(use, movement);
    public void ContinueWenheCardEffect(long id) => this._targetFactory().ContinueWenheCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public void ContinueGuoSiNativePayment(long id) => this._targetFactory().ContinueGuoSiNativePayment(id);
    public void AssertGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertGuoSiNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWumiaoFloodMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsWumiaoFloodMovement(use, batch);
    public bool IsWumiaoFloodMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsWumiaoFloodMovementReturn(use, frame);
    public void ContinueWumiaoFlood(long id) => this._targetFactory().ContinueWumiaoFlood(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public void ReturnSoleTargetQuarantineMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnSoleTargetQuarantineMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> _targetFactory = targetFactory;
    public void ContinueManualDiscard(long frameId) => this._targetFactory().ContinueManualDiscard(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool IsCardUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsCardUsePaymentBatch(use, batch);
    public bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => this._targetFactory().IsPaidCardUseMovementReturn(use, movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public void ContinueRecoveryPaidCardUse(long useId) => this._targetFactory().ContinueRecoveryPaidCardUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public bool IsNativeCardEffectDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsNativeCardEffectDrawMovement(use, batch);
    public bool IsNativeCardEffectDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsNativeCardEffectDrawMovementReturn(use, frame);
    public void ContinueNativeDrawTwo(long id) => this._targetFactory().ContinueNativeDrawTwo(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterZhangQiYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterZhangQiYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation? FaluDiscardOwnedSource(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().FaluDiscardOwnedSource(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreSourceOwnerGainsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreSourceOwnerGainsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectSourceOwnerGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardMovementSourceCount count) => this._targetFactory().CollectSourceOwnerGainCandidates(batch, candidate, trigger, count);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsSkillInvocationMovementBatch(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsSkillInvocationMovementBatch(f, batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterXiahouDunMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterXiahouDunMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> _targetFactory = targetFactory;
    public int[] MatchingMouXiahouDunDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().MatchingMouXiahouDunDiscardIndexes(batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreProgramDiscardOriginsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreProgramDiscardOriginsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().GetProgramDiscardSource(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreEndingHistoricalUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreEndingHistoricalUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _targetFactory = targetFactory;
    public void ContinueHistoricalEndingPayment(long id) => this._targetFactory().ContinueHistoricalEndingPayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreHuxinMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreHuxinMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _targetFactory = targetFactory;
    public void ReturnHuxinMirrorPlacementMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window) => this._targetFactory().ReturnHuxinMirrorPlacementMovement(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool ReturnCardSupplyCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnCardSupplyCompletionMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterWangCanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterWangCanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _targetFactory = targetFactory;
    public void ContinueDenglouNativePayment(long id) => this._targetFactory().ContinueDenglouNativePayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public void ContinueMouZhitianFirePayment(long id) => this._targetFactory().ContinueMouZhitianFirePayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public bool IsFriendDuelResponseMovementReturn(global::CardGame.Core.FriendDuelResponseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => this._targetFactory().IsFriendDuelResponseMovementReturn(parent, movement);
    public void ContinueFriendDuelResponse(long id) => this._targetFactory().ContinueFriendDuelResponse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public bool IsSoleIncomingRangeDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsSoleIncomingRangeDrawMovement(use, batch);
    public bool IsSoleIncomingRangeDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame window) => this._targetFactory().IsSoleIncomingRangeDrawMovementReturn(use, window);
    public void ContinueSoleIncomingRangeDraw(long id) => this._targetFactory().ContinueSoleIncomingRangeDraw(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterPangTongFriendAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterPangTongFriendOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> _targetFactory = targetFactory;
    public int FriendManjuanMovedCount(global::CardGame.Core.CardMovementBatchContext batch, int seat, global::CardGame.Core.SkillProgramTrigger trigger, int fallback) => this._targetFactory().FriendManjuanMovedCount(batch, seat, trigger, fallback);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsKemingEchoMovement(global::CardGame.Core.KemingSkillEchoFrame f, global::CardGame.Core.CardMovementBatchContext b) => this._targetFactory().IsKemingEchoMovement(f, b);
    public void ReturnKemingEchoMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window) => this._targetFactory().ReturnKemingEchoMovement(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreFactionRequestCostRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreFactionRequestCostRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _targetFactory = targetFactory;
    public void ContinuePaidFactionRequestCost(long producerId) => this._targetFactory().ContinuePaidFactionRequestCost(producerId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreDrawFundedDistinctBasicPaymentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreDrawFundedDistinctBasicPaymentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _targetFactory = targetFactory;
    public void ResumeDrawFundedDistinctBasicMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame completed) => this._targetFactory().ResumeDrawFundedDistinctBasicMovement(completed);
    public void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => this._targetFactory().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().DiscardedProvenanceOrigin(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreRicePileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreRicePileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> _targetFactory = targetFactory;
    public bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p) => this._targetFactory().RiceProducer(p);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterLiJueOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterLiJueOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> _targetFactory = targetFactory;
    public int[] MatchingLiJueUseDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().MatchingLiJueUseDiscardIndexes(batch, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreProgramMovementSourceCountsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreProgramMovementSourceCountsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().ProgramMovementSourceCounts(batch, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsPendingWenhePoisonWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsPendingWenhePoisonWindow(frame);
    public bool ReturnWenhePoisonFireAttackMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnWenhePoisonFireAttackMovement(child);
    public bool HasWenhePoisonDepartures(global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().HasWenhePoisonDepartures(batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreNativeDrawOrdinalsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreNativeDrawOrdinalsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> _targetFactory = targetFactory;
    public int[] MatchingNativeDrawOrdinalIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().MatchingNativeDrawOrdinalIndexes(batch, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreNativeDrawThresholdAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreNativeDrawThresholdOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public int[] MatchingNativeDrawThresholdIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().MatchingNativeDrawThresholdIndexes(batch, candidate);
    public global::CardGame.Core.NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().MatchingNativeDrawThresholdProgress(batch, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool IsGuanduCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsGuanduCardEffectMovement(use, batch);
    public bool IsGuanduCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsGuanduCardEffectMovementReturn(use, frame);
    public void ContinueGuanduCardEffect(long id) => this._targetFactory().ContinueGuanduCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public void ContinueColorFireAttackPayment(long useId) => this._targetFactory().ContinueColorFireAttackPayment(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool ReturnCardResponseCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnCardResponseCompletionMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreNianRevivalAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreNianRevivalOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> _targetFactory = targetFactory;
    public bool IsNianRevivalMovementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsNianRevivalMovementBatch(f, batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool HasPaidCardUseMovementTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().HasPaidCardUseMovementTail(use, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreGameDomainsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreGameDomainsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectFirstDomainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().CollectFirstDomainCandidates(batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public void ContinuePolicyCounterspellPayment(long windowId) => this._targetFactory().ContinuePolicyCounterspellPayment(windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreDeferredTurnEndPreludesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreDeferredTurnEndPreludesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> _targetFactory = targetFactory;
    public void ReturnDeferredTurnEndPrelude(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnDeferredTurnEndPrelude(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public void PrepareLingkongGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().PrepareLingkongGainBatch(batch);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public bool IsRoundEndingMovement(global::CardGame.Core.RoundEndingBoundaryFrame parent, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsRoundEndingMovement(parent, batch);
    public void ReturnRoundEndingMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().ReturnRoundEndingMovement(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreRoundPricedPileAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreRoundPricedPileAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _targetFactory = targetFactory;
    public void ContinueRoundPileAlcoholUse(long id) => this._targetFactory().ContinueRoundPileAlcoholUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreCardsMovedWindowRuntimePortsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardsMovedWindowRuntimePortsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool IsDragonBoatCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsDragonBoatCardEffectMovement(use, batch);
    public bool IsDragonBoatCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsDragonBoatCardEffectMovementReturn(use, frame);
    public void ContinueDragonBoatCardEffect(long id) => this._targetFactory().ContinueDragonBoatCardEffect(id);
}
