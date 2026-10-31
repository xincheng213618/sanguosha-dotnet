#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramPaidCardRuntimeCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreProgramPaidCardRuntimeCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramPaidCardRuntimeCharacterLeiBoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryLeiBoSelfLoot(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect e);
}

internal interface ICoreProgramPaidCardRuntimeResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CountLivingInAttackRange(int subjectSeat);
}

internal interface ICoreProgramPaidCardRuntimeCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreProgramPaidCardRuntimeCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreProgramPaidCardRuntimeCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramPaidCardRuntimeCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreProgramPaidCardRuntimeCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreProgramPaidCardRuntimeCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramRandomTransfersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DoesProgramFrozenSuitMatchChoice(global::CardGame.Core.ProgramSkillFrame frame, string cardBind, string choiceBind);
}

internal interface ICoreProgramPaidCardRuntimeCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreProgramPaidCardRuntimeCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}

internal interface ICoreProgramPaidCardRuntimeCoreEquipmentSlotGroupProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSelectProgramPindianOpponent(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat);
}

internal interface ICoreProgramPaidCardRuntimeCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext GuanParentAction(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramPaidCardRuntimeCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect GetOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramPaidCardRuntimeCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPublicHandRevealWindow(long parentId, string key);
}

internal interface ICoreProgramPaidCardRuntimeContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreProgramPaidCardRuntimeCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiQinJudgmentCard(int id);
    bool CanTransferZixiQinJudgmentCard(global::CardGame.Core.Card card, int target);
}

internal interface ICoreProgramPaidCardRuntimeCharacterDaQiaoMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AllowsPostCostSlashRedirectPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardLocation source, global::CardGame.Core.Card card);
}

internal interface ICoreProgramPaidCardRuntimeCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreProgramPaidCardRuntimeCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramCategoryChallengesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DoProgramBoundCardsMatchCategories(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory> categories);
}

internal interface ICoreProgramPaidCardRuntimeCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramPaidCardRuntimeCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TrySettleHappyWeiYanRetainedGain(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason, long? producerFrameId = default);
}

internal interface ICoreProgramPaidCardRuntimeCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card);
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramPaidCardRuntimeCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default);
    global::CardGame.Core.Card[] GetClaimableProgramDamageCards(global::CardGame.Core.ProgramSkillFrame frame);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default);
}

internal interface ICoreProgramPaidCardRuntimeCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default);
}

internal interface ICoreProgramPaidCardRuntimeCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
