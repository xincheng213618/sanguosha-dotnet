#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTieredRoundZeroReturnsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuCheErUse(long id);
    global::CardGame.Core.Card MouHuCheErRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterHuLieMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuLieUse(long id);
    global::CardGame.Core.Card MouHuLieRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreTieredRoundZeroReturnsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLiuYanLegacyAlcoholUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedTieredRoundUse(global::CardGame.Core.CardUseFrame use, bool zeroOnly = false);
    bool IsTieredRoundZeroUse(long id);
    global::CardGame.Core.Card TieredRoundZeroRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreTieredRoundZeroReturnsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreTieredRoundZeroReturnsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZongluanUse(long id);
    global::CardGame.Core.Card ZongluanRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYouqueUse(long id);
    global::CardGame.Core.Card YouqueRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreTieredRoundZeroReturnsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuanpeiUse(long id);
    global::CardGame.Core.Card HuanpeiRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreTieredRoundZeroReturnsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreTieredRoundZeroReturnsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuLuKangKegouUse(long id);
    global::CardGame.Core.Card WuLuKangKegouRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ReadCardAppearance(global::CardGame.Core.CardAppearanceReference appearance);
}

internal interface ICoreTieredRoundZeroReturnsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundOnceCardlessBasicUse(long id);
    global::CardGame.Core.Card RoundOnceCardlessNativeRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouBingcaiLegacyAlcoholUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWeijieUse(long id);
    global::CardGame.Core.Card WeijieRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanLegacyAlcoholUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreTieredRoundZeroReturnsCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue);
}

internal interface ICoreTieredRoundZeroReturnsCharacterBaoSanNiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChosenAssaultUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZonghuUse(long id);
    global::CardGame.Core.Card ZonghuRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterZhangXiuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhangXiuAnniversaryUse(long id);
    global::CardGame.Core.Card MouZhangXiuAnniversaryRepresentation(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterAHuiNanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAHuiNanJuemanUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreTieredRoundZeroReturnsCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTianChouShandaoUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyZeroFireUse(long id);
}

internal interface ICoreTieredRoundZeroReturnsCoreCappedConversionBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidCappedConversionBenefit(global::CardGame.Core.ProgramSkillFrame f);
}
