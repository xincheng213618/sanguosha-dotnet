#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAlternativeViewAsCostsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipMouXqJueyanFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreAlternativeViewAsCostsCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreAlternativeViewAsCostsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreAlternativeViewAsCostsCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipMouLiuChengFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreAlternativeViewAsCostsCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangJinzhiPaidMaterial(global::CardGame.Core.CardActionContext? action, int card);
}

internal interface ICoreAlternativeViewAsCostsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreAlternativeViewAsCostsCoreShanheSourceMouDongZhuoWineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheSourceMouWine(long id);
}

internal interface ICoreAlternativeViewAsCostsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareSiruiResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreAlternativeViewAsCostsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipDiouFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreAlternativeViewAsCostsCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedExtraCardEffect(long id);
}

internal interface ICoreAlternativeViewAsCostsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipHuanpeiFinishedMovement(long id, global::CardGame.Core.Card c);
}

internal interface ICoreAlternativeViewAsCostsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }
}

internal interface ICoreAlternativeViewAsCostsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreAlternativeViewAsCostsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources);
}

internal interface ICoreAlternativeViewAsCostsCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipDrawFundedDistinctBasicFinishedMovement(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCoreRoundDistinctBasicPlayOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishRoundDistinctBasicAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangShopDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreAlternativeViewAsCostsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreAlternativeViewAsCostsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreAlternativeViewAsCostsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishHappyQiaoAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? acceptedConversion);
}

internal interface ICoreAlternativeViewAsCostsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreAlternativeViewAsCostsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouSmPlayedActor(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreAlternativeViewAsCostsCharacterZhaoTongZhaoGuangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryYizanAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedWeijieUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreAlternativeViewAsCostsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreAlternativeViewAsCostsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareWuZhangFeiResponseRanks(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? source, int? actorSeat = default);
}

internal interface ICoreAlternativeViewAsCostsCharacterDongZhuoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDongZhuoOlWine(long id);
}

internal interface ICoreAlternativeViewAsCostsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreAlternativeViewAsCostsCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishLeiluanAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreAlternativeViewAsCostsCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishAnniversaryXunXuAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreAlternativeViewAsCostsCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishMouJsAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card);
}

internal interface ICoreAlternativeViewAsCostsCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkipChainedStateBasicFinishedMovement(long id, global::CardGame.Core.Card card);
}
