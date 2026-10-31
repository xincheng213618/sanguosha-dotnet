#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDeathProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreDeathProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDeathProgramsCharacterSunQuanAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DiscardAnniversarySunQuanMingOnDeath(global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDeathProgramsCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICoreDeathProgramsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkConvertingGiftExactDeath(long deathFrameId, int victimSeat);
}

internal interface ICoreDeathProgramsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBuquWounds(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDeathProgramsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDeathProgramsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreDeathProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyQiaoCardMaximumHealth(long attackId);
}

internal interface ICoreDeathProgramsCharacterDianWeiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyDianWeiDeathRemovals(global::CardGame.Core.DeathFrame supplied);
}

internal interface ICoreDeathProgramsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreDeathProgramsCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetHengyeAfterKill(global::CardGame.Core.CharacterState? killer);
}

internal interface ICoreDeathProgramsCharacterPangTongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied);
    bool TryBeginShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied);
}

internal interface ICoreDeathProgramsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreDeathProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDeathProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void ExposeProgramTriggerDecision(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreDeathProgramsCharacterCaoZhenMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DiscardShiCaoZHolderDeathPiles(global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCharacterZhouFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DiscardBoundaryZhouFeiHolderDeathPiles(global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCharacterHuangQuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DiscardShiQuanHolderDeathPiles(global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDeathProgramsCoreLeastHandMarkerDrawProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int IndependentIssuedDrawMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source);
}

internal interface ICoreDeathProgramsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreDeathProgramsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTeamMode { get; }

    bool IsNationalWarMode { get; }
}

internal interface ICoreDeathProgramsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreDeathProgramsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreDeathProgramsCharacterSimaShiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMouSimaShiDeathCandidates(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreDeathProgramsCoreModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyModeDeathOutcome(long deathFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer);
}

internal interface ICoreDeathProgramsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDyingAfterDeath(global::CardGame.Core.DyingFrame dying, bool survived);
}

internal interface ICoreDeathProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDeathProgramsCoreCardDamageMaximumHpReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactMaximumHpReplacementDeathParent(global::CardGame.Core.DeathFrame death, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDeathProgramsCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfDeath(global::CardGame.Core.CharacterState? killer, global::CardGame.Core.CharacterState victim);
}

internal interface ICoreDeathProgramsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreDeathProgramsCoreMatchPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRoleName(global::CardGame.Core.Role role);
}

internal interface ICoreDeathProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreDeathProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreDeathProgramsCoreRecipientCategoryMarksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int IndependentRecipientCategoryMarkerCount(global::CardGame.Core.CharacterState holder, global::CardGame.Core.PlayerMarkerKind marker, int source);
}

internal interface ICoreDeathProgramsCharacterYuanYinAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(global::CardGame.Core.ProgramKillTriggerWindowFrame window, global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreDeathProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDeathProgramsCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default);
}

internal interface ICoreDeathProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDeathProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDeathProgramsCorePlayPhaseSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CloseCurrentPlayPhaseSkillGrants(string reason);
}

internal interface ICoreDeathProgramsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DiscardQushiHolderDeathSeals(global::CardGame.Core.DeathFrame death);
}

internal interface ICoreDeathProgramsCoreHpDamageShieldsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int IndependentMarkerCount(global::CardGame.Core.CharacterState target, global::CardGame.Core.PlayerMarkerKind marker, int source);
}
