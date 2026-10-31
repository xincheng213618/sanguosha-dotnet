#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPaidCardFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CoreGameEngineFieldEquipmentEntries> getCoreGameEngineFieldEquipmentRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CharacterGuanYuRuntimeEntries> getCharacterGuanYuRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries> getCoreGameEngineProgramOwnedCardsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLeiBoAnniversaryRuntime>> getCharacterLeiBoAnniversaryRuntime, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CharacterDaQiaoMobileEntries> getCharacterDaQiaoMobileRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime, Func<RuntimeEntries.CharacterDaQiaoLeEntries> getCharacterDaQiaoLeRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries> getCoreGameEngineEquipmentSlotGroupProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramRandomTransfersRuntime>> getCoreGameEngineProgramRandomTransfersRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries> getCoreGameEngineProgramCategoryChallengesRuntime) : IProgramPaidCardFacts
{
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public IReadOnlyList<int> Seats => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => player.Seat).ToArray());

    public int Seat(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Seat;
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool IsHuman(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public bool EquipmentAreaAbolished(int seat) => matchParticipantsState.ReferenceToPlayers[seat].EquipmentAreaAbolished;
    public bool JudgmentAreaAbolished(int seat) => matchParticipantsState.ReferenceToPlayers[seat].JudgmentAreaAbolished;
    public int EquipmentSlotCapacity(int seat, EquipmentSlot slot) => matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(slot);
    public EquipmentSlot FieldEquipmentSlot(Card card) => getCoreGameEngineFieldEquipmentRuntime().FieldEquipmentSlot(card);
    public bool HasSkillInstance(int seat, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instanceId);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry()!.GetSkill(skillId);
    public PlayerSkillContext SkillContext(int seat) => getCoreNativeSkillContextQueriesRuntime().CreateSkillContext(matchParticipantsState.ReferenceToPlayers[seat]);
    public IReadOnlyList<Card> Hand(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public IReadOnlyList<Card> Equipment(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public IReadOnlyList<Card> Judgment(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetJudgment(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public IReadOnlyList<Card> CardsAt(CardLocation location) => Array.AsReadOnly(getCardZoneStore().CardsAt(location).ToArray());
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public int ProviderSeat(ProgramSkillFrame frame) => getCharacterGuanYuRuntimeRuntime().GuanParentAction(frame).ProviderSeat;
    public SkillProgramEffect OwnedSelectionEffect(ProgramSkillFrame frame) => getCoreGameEngineProgramOwnedCardsRuntime().GetOwnedSelectionEffect(frame);
    public bool IsSatisfiedOwnHandReward(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterLeiBoAnniversaryRuntime().Value.IsAnniversaryLeiBoSelfLoot(frame, effect);
    public bool ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
    public bool SelfDiscardForbidden(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(chooser, card, source, intent);
    public bool AllowsOwnedCardPayment(ProgramSkillFrame frame, CardLocation source, Card card) => getCharacterDaQiaoMobileRuntime().AllowsPostCostSlashRedirectPayment(frame, source, card);
    public bool IsActiveSourceEquipment(int owner, string skillId, string instanceId, Card card) => getCoreGameEngineEquipmentSkillsRuntime().IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    public bool IsZixiJudgment(int cardId) => getCharacterDaQiaoLeRuntime().IsZixiQinJudgmentCard(cardId);
    public bool CanTransferZixiJudgment(Card card, int destinationSeat) => getCharacterDaQiaoLeRuntime().CanTransferZixiQinJudgmentCard(card, destinationSeat);
    public bool HasCardPolicy(int seat, SkillProgramCardPolicyKind policy) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(matchParticipantsState.ReferenceToPlayers[seat], policy);
    public CardKind JudgmentKind(Card card) => getCoreActualTurnsRuntime().GetJudgmentEffectiveCardKind(card);
    public bool IsDelayedCard(CardKind kind) => EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(kind);
    public bool HasJudgmentEffectiveCard(int seat, CardKind kind) => getCoreActualTurnsRuntime().HasJudgmentEffectiveCard(matchParticipantsState.ReferenceToPlayers[seat], kind);
    public Suit EffectiveSuit(int seat, Card card) => getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], card);
    public int CountLivingInAttackRange(int seat) => getCoreGameEngineProgramCardPaymentsRuntime().CountLivingInAttackRange(seat);
    public IReadOnlyList<int> TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTargetSeats(owner, kind, context);
    public bool CanSelectPindianOpponent(ProgramSkillFrame frame, int seat) => getCoreGameEngineEquipmentSlotGroupProgramsRuntime().CanSelectProgramPindianOpponent(frame, seat);
    public Card[] ClaimableDamageCards(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().GetClaimableProgramDamageCards(frame);
    public bool FrozenSuitMatchesChoice(ProgramSkillFrame frame, string cardBind, string choiceBind) => getCoreGameEngineProgramRandomTransfersRuntime().Value.DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind);
    public bool BoundCardsMatchCategories(ProgramSkillFrame frame, string bind, IReadOnlyList<SkillProgramCardCategory> categories) => getCoreGameEngineProgramCategoryChallengesRuntime().DoProgramBoundCardsMatchCategories(frame, bind, categories);
}
