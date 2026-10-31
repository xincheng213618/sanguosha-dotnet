#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeJudgmentCharacterWangYiMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDeckTopJudgmentReplacementMaterial();
}

internal interface ICoreNativeJudgmentCharacterMaYuanYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnBinghuoJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnWeifuJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeJudgmentCoreFinalizedJudgmentDamageChildrenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeginFinalizedDamageChildJudgment(global::CardGame.Core.JudgmentFrame original, long parentFrameId, int targetSeat, string reason, global::CardGame.Core.JudgmentContinuationKind continuation, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility? visibility);
}

internal interface ICoreNativeJudgmentCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShiHzResponseColorAllowed(int responder, bool? effectiveIsRed);
}

internal interface ICoreNativeJudgmentCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool EnsureDrawPile();
}

internal interface ICoreNativeJudgmentCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeJudgmentCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNativeJudgmentCoreExactRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramRepeatedJudgmentClaimOutcome(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit);
}

internal interface ICoreNativeJudgmentCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeJudgmentCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnShenZfJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreNativeJudgmentCoreWushuangCombatProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnWushuangLightningJudgment(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeJudgmentCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeJudgmentCorePassiveRulePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyDelayedJudgmentOutcomePolicies(global::CardGame.Core.JudgmentFrame frame, bool succeeded);
}

internal interface ICoreNativeJudgmentCoreSlashSuitDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Suit suit);
    bool TryReturnProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBagua(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeJudgmentContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeJudgmentCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnNingwuJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnLishiJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame pending, bool hit);
}

internal interface ICoreNativeJudgmentCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed);
}

internal interface ICoreNativeJudgmentCoreSourceHuojieOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSourceHuojieLightningJudgment(global::CardGame.Core.JudgmentFrame j);
    bool ReturnSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesProgramJudgmentReplacement(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int targetSeat, string reason);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false);
    void BeginProgramJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate);
}

internal interface ICoreNativeJudgmentCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeJudgmentCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeJudgmentCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeJudgmentCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreNativeJudgmentCoreProgramJudgmentOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnProgramJudgmentOutcome(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentCoreDamageJudgmentSuitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDamageJudgmentSuitResult(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit);
    bool TryReturnDamageJudgmentSuitPayment(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteFactionDefenseBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded);
}

internal interface ICoreNativeJudgmentCharacterZhangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnZhangJiJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
}

internal interface ICoreNativeJudgmentCoreDiscardedProvenanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProvenanceJudgmentOrigin(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit effectiveSuit);
}

internal interface ICoreNativeJudgmentCoreProgramRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeProgramRepeatedJudgment(global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreNativeJudgmentCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreNativeJudgmentCharacterZhangQiYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback);
    int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback);
}

internal interface ICoreNativeJudgmentCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeJudgmentCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default);
}

internal interface ICoreNativeJudgmentCharacterGuoJiaMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnShenGuoJiaJudgment(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreNativeJudgmentCoreBaguaResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PublishFailedBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeJudgmentCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeJudgmentCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder();
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNativeJudgmentCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeJudgmentCoreOwnedDamagePointJudgmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOwnedDamagePointJudgmentResultClaimed(global::CardGame.Core.JudgmentFrame judgment);
}

internal interface ICoreNativeJudgmentCharacterGuoJiaMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGjLightningJudgment(global::CardGame.Core.JudgmentFrame j);
    bool ReturnMouGjJudgment(global::CardGame.Core.JudgmentFrame j);
}

internal interface ICoreNativeJudgmentCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
    void BeginDelayedJudgmentOrTurnStart(global::CardGame.Core.CharacterState current, int? excludedCardId = default);
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
    void EndTurn();
}

internal interface ICoreNativeJudgmentCoreSourceCursesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? TakeSourceCurseForJudgment(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreNativeJudgmentCoreJudgmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramJudgmentWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, global::CardGame.Core.Suit effectiveSuit, bool succeeded);
}

internal interface ICoreNativeJudgmentCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.Card? GetDelayedJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreNativeJudgmentCoreZhenyiJudgmentWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginZhenyiBeforeResultWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit);
}

internal interface ICoreNativeJudgmentCoreNativeEquipmentInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBaguaEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, bool factionDefense = false);
}
