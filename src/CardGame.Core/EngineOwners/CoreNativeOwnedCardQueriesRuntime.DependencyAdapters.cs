#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindShiLsPlayableCard(global::CardGame.Core.CharacterState actor, int id) => this._targetFactory().FindShiLsPlayableCard(actor, id);
    public bool TryShiLsHandLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation from) => this._targetFactory().TryShiLsHandLocation(actor, card, out from);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCoreLastZoneConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCoreLastZoneConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindLastZoneJudgmentConversionCard(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().FindLastZoneJudgmentConversionCard(owner, cardId);
    public bool TryGetLastZoneJudgmentConversionLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation location) => this._targetFactory().TryGetLastZoneJudgmentConversionLocation(owner, card, out location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterDuKuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterDuKuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindDuKuiFanyinRevealedCard(global::CardGame.Core.CharacterState actor, int cardId) => this._targetFactory().FindDuKuiFanyinRevealedCard(actor, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindNativePublicPileOriginalCard(global::CardGame.Core.CharacterState provider, int id) => this._targetFactory().FindNativePublicPileOriginalCard(provider, id);
    public bool TryNativePublicPilePaymentLocation(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation from) => this._targetFactory().TryNativePublicPilePaymentLocation(provider, card, out from);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindShouliFieldCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().FindShouliFieldCard(actor, id);
    public bool TryShouliFieldLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, out global::CardGame.Core.CardLocation location) => this._targetFactory().TryShouliFieldLocation(actor, card, out location);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterDuanJiongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterDuanJiongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindDuanJiongSaoguDiscardCard(global::CardGame.Core.CharacterState actor, int id) => this._targetFactory().FindDuanJiongSaoguDiscardCard(actor, id);
    public bool IsDuanJiongSaoguSelectedDiscard(int actor, int id) => this._targetFactory().IsDuanJiongSaoguSelectedDiscard(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterZhouFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterZhouFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindBoundaryZhouFeiUseCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().FindBoundaryZhouFeiUseCard(actor, id);
    public bool IsBoundaryZhouFeiUseQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => this._targetFactory().IsBoundaryZhouFeiUseQuery(actor, selectedId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterFuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterFuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindFuRongDiscardCard(global::CardGame.Core.CharacterState actor, int id) => this._targetFactory().FindFuRongDiscardCard(actor, id);
    public bool IsFuRongSelectedDiscard(int seat, int id) => this._targetFactory().IsFuRongSelectedDiscard(seat, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterZhangHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterZhangHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? AnniversaryZhangHuanQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().AnniversaryZhangHuanQueryOriginalCard(actor, id);
    public bool IsAnniversaryZhangHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsAnniversaryZhangHuanExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetWoodenOxGrain(global::CardGame.Core.CharacterState player) => this._targetFactory().GetWoodenOxGrain(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetAuthority(global::CardGame.Core.CharacterState player) => this._targetFactory().GetAuthority(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterYaDanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterYaDanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYaDanOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindQingyaDiscardCard(global::CardGame.Core.CharacterState actor, int id) => this._targetFactory().FindQingyaDiscardCard(actor, id);
    public bool IsQingyaSelectedDiscard(int actor, int id) => this._targetFactory().IsQingyaSelectedDiscard(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterBaiLingYunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterBaiLingYunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindAnniversaryBaiLingYunViewedCard(global::CardGame.Core.CharacterState actor, int id) => this._targetFactory().FindAnniversaryBaiLingYunViewedCard(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public bool IsSelectedRequestedDeckBasicMaterial(int actor, int id) => this._targetFactory().IsSelectedRequestedDeckBasicMaterial(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindOwnPublicPileConversionCard(global::CardGame.Core.CharacterState owner, int cardId) => this._targetFactory().FindOwnPublicPileConversionCard(owner, cardId);
    public bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsOwnPublicPileCard(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? AvailableDeclarationCard(global::CardGame.Core.CharacterState owner) => this._targetFactory().AvailableDeclarationCard(owner);
    public global::CardGame.Core.DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId) => this._targetFactory().UnclaimedDeclarationPayment(provider, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? HappyWeiYanBoneReturnCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().HappyWeiYanBoneReturnCard(actor, id);
    public bool IsHappyWeiYanBoneReturnQuery(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().IsHappyWeiYanBoneReturnQuery(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterCaoAngMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterCaoAngMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? MouCaQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => this._targetFactory().MouCaQueryOriginalCard(actor, id);
    public bool IsMouCaExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => this._targetFactory().IsMouCaExactCardQuery(actor, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed class CoreNativeOwnedCardQueriesCharacterGongsunZanMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeOwnedCardQueriesCharacterGongsunZanMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsMouGszHuCard(owner, id);
}
