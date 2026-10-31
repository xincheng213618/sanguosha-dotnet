#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWenyiPeachSuppliesCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false);
}

internal interface ICoreWenyiPeachSuppliesCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDrawFundedDistinctBasicPayment(global::CardGame.Core.DrawFundedDistinctBasicPayment p);
}

internal interface ICoreWenyiPeachSuppliesCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreWenyiPeachSuppliesCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativePublicPileCardRequest NativePublicPileUseRequest(int actor, int provider, global::CardGame.Core.CardKind? kind = default, long? parent = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> GetNativePublicPileOriginalCards(global::CardGame.Core.NativePublicPileCardRequest request);
    void PrepareNativePublicPilePayment(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.NativePublicPileCardSelection selection);
}

internal interface ICoreWenyiPeachSuppliesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreWenyiPeachSuppliesCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuyouIdentityKind(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> WuyouIdentitySources(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreWenyiPeachSuppliesCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICoreWenyiPeachSuppliesCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreWenyiPeachSuppliesCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEquipmentSlotNamedSkill(string skill);
    global::CardGame.Core.EquipmentSlotNamedUsePayment? UnconsumedEquipmentSlotPayment(global::CardGame.Core.CardConversionSource source);
    bool EquipmentSlotNamedChoiceIsPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.EquipmentSlot slot);
    void BeginEquipmentSlotNamedUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::CardGame.Core.PendingDecision original, global::CardGame.Core.LegalAction? action, global::CardGame.Core.PromptChoice? choice);
}

internal interface ICoreWenyiPeachSuppliesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> FindProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramCardIdentitySources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreWenyiPeachSuppliesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreWenyiPeachSuppliesCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? ActiveWeijieDraft();
    void DispatchWeijieRecovery(global::CardGame.Core.WeijiePaidOrigin p, bool dying);
}

internal interface ICoreWenyiPeachSuppliesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreWenyiPeachSuppliesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreWenyiPeachSuppliesCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreWenyiPeachSuppliesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWenyiPeachSuppliesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreWenyiPeachSuppliesCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreWenyiPeachSuppliesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreWenyiPeachSuppliesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreWenyiPeachSuppliesCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreWenyiPeachSuppliesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreWenyiPeachSuppliesCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
    bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit);
}

internal interface ICoreWenyiPeachSuppliesCoreRequestedDeckBasicOwnershipOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasValidRequestedDeckBasicMaterial(global::CardGame.Core.ResolutionFrame owner);
}

internal interface ICoreWenyiPeachSuppliesCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreWenyiPeachSuppliesCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenyiPeachRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetWenyiPeachRequest(global::CardGame.Core.ProgramSkillFrame f, out int actor, out int target);
    void PrepareWenyiSuppliedPeach(global::CardGame.Core.ProgramSkillFrame supplied, int provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalSources = default, global::CardGame.Core.WeijiePaidOrigin? paidWeijie = default);
    void FinishUnissuedWenyiPeach(global::CardGame.Core.ProgramSkillFrame supplied);
    bool WenyiHpRoot(global::CardGame.Core.ProgramSkillFrame f);
    bool IsWenyiPeachRequestAfterPaidSupply(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WeijiePaidOrigin paid);
    void PublishHuanShuJun(global::CardGame.Core.ProgramSkillFrame f);
    void FinishHuanShuJun(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreWenyiPeachSuppliesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreWenyiPeachSuppliesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreWenyiPeachSuppliesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreWenyiPeachSuppliesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWenyiPeachSuppliesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}
