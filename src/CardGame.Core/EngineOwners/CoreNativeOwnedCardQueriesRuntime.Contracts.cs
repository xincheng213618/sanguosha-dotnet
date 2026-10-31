#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeOwnedCardQueriesCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindShiLsPlayableCard(global::CardGame.Core.CharacterState actor, int id);
    bool TryShiLsHandLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation from);
}

internal interface ICoreNativeOwnedCardQueriesCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindLastZoneJudgmentConversionCard(global::CardGame.Core.CharacterState owner, int cardId);
    bool TryGetLastZoneJudgmentConversionLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation location);
}

internal interface ICoreNativeOwnedCardQueriesCharacterDuKuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindDuKuiFanyinRevealedCard(global::CardGame.Core.CharacterState actor, int cardId);
}

internal interface ICoreNativeOwnedCardQueriesCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindNativePublicPileOriginalCard(global::CardGame.Core.CharacterState provider, int id);
    bool TryNativePublicPilePaymentLocation(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation from);
}

internal interface ICoreNativeOwnedCardQueriesCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindShouliFieldCard(global::CardGame.Core.CharacterState actor, int? id);
    bool TryShouliFieldLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation location);
}

internal interface ICoreNativeOwnedCardQueriesCharacterDuanJiongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindDuanJiongSaoguDiscardCard(global::CardGame.Core.CharacterState actor, int id);
    bool IsDuanJiongSaoguSelectedDiscard(int actor, int id);
}

internal interface ICoreNativeOwnedCardQueriesCharacterZhouFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindBoundaryZhouFeiUseCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsBoundaryZhouFeiUseQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreNativeOwnedCardQueriesCharacterFuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindFuRongDiscardCard(global::CardGame.Core.CharacterState actor, int id);
    bool IsFuRongSelectedDiscard(int seat, int id);
}

internal interface ICoreNativeOwnedCardQueriesCharacterZhangHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? AnniversaryZhangHuanQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsAnniversaryZhangHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeOwnedCardQueriesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetWoodenOxGrain(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeOwnedCardQueriesCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindQingyaDiscardCard(global::CardGame.Core.CharacterState actor, int id);
    bool IsQingyaSelectedDiscard(int actor, int id);
}

internal interface ICoreNativeOwnedCardQueriesCharacterBaiLingYunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindAnniversaryBaiLingYunViewedCard(global::CardGame.Core.CharacterState actor, int id);
}

internal interface ICoreNativeOwnedCardQueriesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeOwnedCardQueriesCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedRequestedDeckBasicMaterial(int actor, int id);
}

internal interface ICoreNativeOwnedCardQueriesCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnPublicPileConversionCard(global::CardGame.Core.CharacterState owner, int cardId);
    bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeOwnedCardQueriesCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? AvailableDeclarationCard(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId);
}

internal interface ICoreNativeOwnedCardQueriesCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? HappyWeiYanBoneReturnCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsHappyWeiYanBoneReturnQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICoreNativeOwnedCardQueriesCharacterCaoAngMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? MouCaQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id);
    bool IsMouCaExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICoreNativeOwnedCardQueriesCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id);
}
