#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedCardMoveLegacyFacts(Func<RuntimeEntries.CharacterZhangFenAnniversaryEntries> getCharacterZhangFenAnniversaryRuntime, Func<RuntimeEntries.CharacterPangTongAnniversaryGodEntries> getCharacterPangTongAnniversaryGodRuntime, Func<RuntimeEntries.CharacterDongBaiOlEntries> getCharacterDongBaiOlRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime) : IOwnedCardMoveLegacyFacts
{
    public bool UnupgradedCartDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => getCharacterZhangFenAnniversaryRuntime().IsUnupgradedDagongCheDiscardForbidden(actorSeat, card, from, intent);
    public bool ChainedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => getCharacterPangTongAnniversaryGodRuntime().IsShenPangTongHandDiscardForbidden(actorSeat, card, from, intent);
    public bool GiftedHandDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => getCharacterDongBaiOlRuntime().IsXiaHuiDiscardForbidden(actorSeat, card, from, intent);
    public bool TurnHandCategoryRestricted(int ownerSeat, Card card) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsTurnHandCategoryRestricted(matchParticipantsState.ReferenceToPlayers[ownerSeat], card);
    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool HasEquipmentDiscardProtection(int ownerSeat) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(matchParticipantsState.ReferenceToPlayers[ownerSeat], SkillProgramCardPolicyKind.PreventForeignEquipmentDiscard);
}
