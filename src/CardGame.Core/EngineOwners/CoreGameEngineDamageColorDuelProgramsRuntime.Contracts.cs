#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageColorDuelProgramsCoreAvailableBoundSubsetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAvailableBoundPeachRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDamageColorDuelProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageColorDuelProgramsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTieredRoundZeroUse(long id);
    global::CardGame.Core.Card TieredRoundZeroRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreDamageColorDuelProgramsCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageColorDuelProgramsCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool JueshengActorMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.JueshengDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDamageColorDuelProgramsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDamageColorDuelProgramsCharacterDongZhuoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDongZhuoOlWine(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterZhangXiuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhangXiuAnniversaryUse(long id);
    global::CardGame.Core.Card MouZhangXiuAnniversaryRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterAHuiNanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAHuiNanJuemanUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreDamageColorDuelProgramsCoreActualTurnLegacyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactLegacyActualUseCompletion(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDamageColorDuelProgramsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDamageColorDuelProgramsCharacterBaoSanNiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChosenAssaultUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreDamageColorDuelProgramsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuanpeiUse(long id);
    global::CardGame.Core.Card HuanpeiRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYouqueUse(long id);
    global::CardGame.Core.Card YouqueRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card EntityAtCurrentLocation(int id);
}

internal interface ICoreDamageColorDuelProgramsCharacterHuLieMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuLieUse(long id);
    global::CardGame.Core.Card MouHuLieRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreDamageColorDuelProgramsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWeijieUse(long id);
    global::CardGame.Core.Card WeijieRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuLuKangKegouUse(long id);
    global::CardGame.Core.Card WuLuKangKegouRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOriginalUseLightning(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreDamageColorDuelProgramsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLiuYanLegacyAlcoholUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouBingcaiLegacyAlcoholUse(long id);
    bool IsMouLixianRemovedMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTianChouShandaoUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouUse(long id);
    global::CardGame.Core.Card DiouRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDamageColorDuelProgramsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcZeroUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreDamageColorDuelProgramsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZonghuUse(long id);
    global::CardGame.Core.Card ZonghuRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreShanheSourceMouDongZhuoWineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheSourceMouWine(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDrawFundedDistinctBasicUse(long frameId);
    global::CardGame.Core.Card DrawFundedDistinctBasicRepresentation(long frameId);
}

internal interface ICoreDamageColorDuelProgramsCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedZeroEntityDuel(long id);
}

internal interface ICoreDamageColorDuelProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageColorDuelProgramsCorePublicPileEquipmentSequencesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageColorDuelProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreDamageColorDuelProgramsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterLvYiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLySongyanDrawTwo(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreDamageColorDuelProgramsCharacterXiahouEnAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertXiahouEnShiwuDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCharacterCaiYangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCaiYangXunjiDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXuHuangZeroUse(long id);
    global::CardGame.Core.Card MouXuHuangRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreDamageColorDuelProgramsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimedUseCardEntity(long frameId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXqJueyanUse(long id);
    global::CardGame.Core.Card MouXqJueyanRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterDiaoChanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertMouDiaoChanRingUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreDamageColorDuelProgramsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreDamageColorDuelProgramsCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHuangFuSongCapability { get; }
}

internal interface ICoreDamageColorDuelProgramsCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDamageColorDuelProgramsCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanLegacyAlcoholUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLiuChengUse(long id);
    global::CardGame.Core.Card MouLiuChengRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDyingSuitsPeachUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DyingSuitsPeachReturn ret);
}

internal interface ICoreDamageColorDuelProgramsCoreShanheSourceLiuYanTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSourceXiechanDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyZeroFireUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreOrderedSelectedActorDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsOrderedSelectedActorDuelProgram(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.SkillProgramStepOutcome BeginOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame);
    void CompleteOrderedSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin, long useId);
    void AssertOrderedSelectedActorDuelUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin);
}

internal interface ICoreDamageColorDuelProgramsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZongluanUse(long id);
    global::CardGame.Core.Card ZongluanRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreDamageColorDuelProgramsCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChainedStateBasicUse(long id);
    global::CardGame.Core.Card ChainedStateBasicRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellDestroyedSoulMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreDamageColorDuelProgramsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuCheErUse(long id);
    global::CardGame.Core.Card MouHuCheErRepresentation(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamageColorDuelProgramsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreWushuangSkillsBOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangBZeroUse(long id);
}

internal interface ICoreDamageColorDuelProgramsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}
