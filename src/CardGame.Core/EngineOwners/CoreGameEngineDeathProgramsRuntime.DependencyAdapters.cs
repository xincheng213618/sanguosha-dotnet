#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreDeathProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDeathProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed class CoreDeathProgramsCharacterSunQuanAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterSunQuanAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DiscardAnniversarySunQuanMingOnDeath(global::CardGame.Core.DeathFrame death) => this._targetFactory().DiscardAnniversarySunQuanMingOnDeath(death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreDeathProgramsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime))]
internal sealed class CoreDeathProgramsCorePublicPileSourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePublicPileSourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat) => this._targetFactory().PublicPileSources(seat);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source) => this._targetFactory().PublicPileCards(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime))]
internal sealed class CoreDeathProgramsCoreConvertingGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreConvertingGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertingGiftsRuntime> _targetFactory = targetFactory;
    public void MarkConvertingGiftExactDeath(long deathFrameId, int victimSeat) => this._targetFactory().MarkConvertingGiftExactDeath(deathFrameId, victimSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreDeathProgramsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBuquWounds(global::CardGame.Core.CharacterState player) => this._targetFactory().GetBuquWounds(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player) => this._targetFactory().GetAuthority(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreDeathProgramsCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().MoveLogicalDiscardCards(initiatorSeat, executorSeat, cards, from, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreDeathProgramsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreDeathProgramsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyQiaoCardMaximumHealth(long attackId) => this._targetFactory().TryBeginHappyQiaoCardMaximumHealth(attackId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime))]
internal sealed class CoreDeathProgramsCharacterDianWeiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterDianWeiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginHappyDianWeiDeathRemovals(global::CardGame.Core.DeathFrame supplied) => this._targetFactory().TryBeginHappyDianWeiDeathRemovals(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreDeathProgramsCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public void ResetHengyeAfterKill(global::CardGame.Core.CharacterState? killer) => this._targetFactory().ResetHengyeAfterKill(killer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime))]
internal sealed class CoreDeathProgramsCharacterPangTongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterPangTongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void CompleteShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied) => this._targetFactory().CompleteShenPangTongKunyuDeath(supplied);
    public bool TryBeginShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied) => this._targetFactory().TryBeginShenPangTongKunyuDeath(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreDeathProgramsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    public void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDeathProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind) => this._targetFactory().GetProgramCardSet(frame, bind);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts) => this._targetFactory().CollectEligibleProgramTriggerCandidates(owner, window, facts);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().ExposeProgramTriggerDecision(candidate, context);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime))]
internal sealed class CoreDeathProgramsCharacterCaoZhenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterCaoZhenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DiscardShiCaoZHolderDeathPiles(global::CardGame.Core.DeathFrame death) => this._targetFactory().DiscardShiCaoZHolderDeathPiles(death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime))]
internal sealed class CoreDeathProgramsCharacterZhouFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterZhouFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DiscardBoundaryZhouFeiHolderDeathPiles(global::CardGame.Core.DeathFrame death) => this._targetFactory().DiscardBoundaryZhouFeiHolderDeathPiles(death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime))]
internal sealed class CoreDeathProgramsCharacterHuangQuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterHuangQuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(global::CardGame.Core.DeathFrame death) => this._targetFactory().DiscardShiQuanHolderDeathPiles(death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime))]
internal sealed class CoreDeathProgramsCoreLeastHandMarkerDrawProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreLeastHandMarkerDrawProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> _targetFactory = targetFactory;
    public int IndependentIssuedDrawMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source) => this._targetFactory().IndependentIssuedDrawMarkerCount(target, marker, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreDeathProgramsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreDeathProgramsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsTeamMode { get => this._targetFactory().IsTeamMode; }
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreDeathProgramsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreDeathProgramsCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime))]
internal sealed class CoreDeathProgramsCharacterSimaShiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterSimaShiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaShiMouRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMouSimaShiDeathCandidates(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().IncludeMouSimaShiDeathCandidates(owner, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed class CoreDeathProgramsCoreModeRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreModeRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> _targetFactory = targetFactory;
    public void ApplyModeDeathOutcome(long deathFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer) => this._targetFactory().ApplyModeDeathOutcome(deathFrameId, victim, killer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreDeathProgramsCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void CompleteDyingAfterDeath(global::CardGame.Core.DyingFrame dying, bool survived) => this._targetFactory().CompleteDyingAfterDeath(dying, survived);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed class CoreDeathProgramsCoreCardDamageMaximumHpReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreCardDamageMaximumHpReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _targetFactory = targetFactory;
    public bool IsExactMaximumHpReplacementDeathParent(global::CardGame.Core.DeathFrame death, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().IsExactMaximumHpReplacementDeathParent(death, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreDeathProgramsCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void NotifyAiOfDeath(global::CardGame.Core.CharacterState? killer, global::CardGame.Core.CharacterState victim) => this._targetFactory().NotifyAiOfDeath(killer, victim);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreDeathProgramsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime))]
internal sealed class CoreDeathProgramsCoreMatchPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreMatchPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _targetFactory = targetFactory;
    public string GetRoleName(global::CardGame.Core.Role role) => this._targetFactory().GetRoleName(role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDeathProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreDeathProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime))]
internal sealed class CoreDeathProgramsCoreRecipientCategoryMarksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreRecipientCategoryMarksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> _targetFactory = targetFactory;
    public int IndependentRecipientCategoryMarkerCount(global::CardGame.Core.CharacterState holder, global::CardGame.Core.PlayerMarkerKind marker, int source) => this._targetFactory().IndependentRecipientCategoryMarkerCount(holder, marker, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime))]
internal sealed class CoreDeathProgramsCharacterYuanYinAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterYuanYinAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> _targetFactory = targetFactory;
    public void CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(global::CardGame.Core.ProgramKillTriggerWindowFrame window, global::CardGame.Core.DeathFrame death) => this._targetFactory().CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(window, death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreDeathProgramsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDeathProgramsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime))]
internal sealed class CoreDeathProgramsCoreNativePlayerDeathAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreNativePlayerDeathOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> _targetFactory = targetFactory;
    public void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default) => this._targetFactory().BeginPlayerDeath(parentFrameId, victim, killer, attack, dying, causingProgramSkillFrameId, causingCardDamageReplacementId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreDeathProgramsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime))]
internal sealed class CoreDeathProgramsCorePlayPhaseSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCorePlayPhaseSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _targetFactory = targetFactory;
    public void CloseCurrentPlayPhaseSkillGrants(string reason) => this._targetFactory().CloseCurrentPlayPhaseSkillGrants(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreDeathProgramsCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DiscardQushiHolderDeathSeals(global::CardGame.Core.DeathFrame death) => this._targetFactory().DiscardQushiHolderDeathSeals(death);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime))]
internal sealed class CoreDeathProgramsCoreHpDamageShieldsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeathProgramsCoreHpDamageShieldsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpDamageShieldsRuntime> _targetFactory = targetFactory;
    public int IndependentMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source) => this._targetFactory().IndependentMarkerCount(target, marker, source);
}
