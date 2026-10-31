#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHumanNativeCommandsEntries
{
    internal void HumanBorrowedSwordFactionSlashCore(bool advanceToHumanBoundary) => _resolve().HumanBorrowedSwordFactionSlashCore(advanceToHumanBoundary);
    internal void HumanBorrowedSwordResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedEffectiveKind, bool advanceToHumanBoundary) => _resolve().HumanBorrowedSwordResponseCore(useSlash, requestedSlashCardId, requestedEffectiveKind, advanceToHumanBoundary);
    internal void HumanCixiongDoubleSwordsCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanCixiongDoubleSwordsCore(selected, advanceToHumanBoundary);
    internal void HumanDuelResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => _resolve().HumanDuelResponseCore(useSlash, requestedSlashCardId, requestedResponseCardKind, advanceToHumanBoundary);
    internal void HumanDyingProgramTriggerCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanDyingProgramTriggerCore(selected, advanceToHumanBoundary);
    internal void HumanDyingResponseCore(bool usePeach, int? requestedPeachCardId, bool useAlcohol, int? requestedAlcoholCardId, bool advanceToHumanBoundary, CardConversionSource? peachConversionSource = default, CardConversionSource? alcoholConversionSource = default) => _resolve().HumanDyingResponseCore(usePeach, requestedPeachCardId, useAlcohol, requestedAlcoholCardId, advanceToHumanBoundary, peachConversionSource, alcoholConversionSource);
    internal void HumanEndPlayCore(bool advanceToHumanBoundary) => _resolve().HumanEndPlayCore(advanceToHumanBoundary);
    internal void HumanFactionDefenseResponseCore(bool useDodge, bool useBagua, int? requestedCardId, bool advanceToHumanBoundary) => _resolve().HumanFactionDefenseResponseCore(useDodge, useBagua, requestedCardId, advanceToHumanBoundary);
    internal void HumanFactionSlashResponseCore(bool useSlash, int? requestedCardId, CardKind? requestedEffectiveKind, bool advanceToHumanBoundary) => _resolve().HumanFactionSlashResponseCore(useSlash, requestedCardId, requestedEffectiveKind, advanceToHumanBoundary);
    internal void HumanFireAttackCardCore(int cardId, bool advanceToHumanBoundary) => _resolve().HumanFireAttackCardCore(cardId, advanceToHumanBoundary);
    internal void HumanFireAttackSkipCore(bool advanceToHumanBoundary) => _resolve().HumanFireAttackSkipCore(advanceToHumanBoundary);
    internal void HumanGroupResponseCore(bool useResponse, bool useBagua, int? requestedResponseCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => _resolve().HumanGroupResponseCore(useResponse, useBagua, requestedResponseCardId, requestedResponseCardKind, advanceToHumanBoundary);
    internal void HumanHarvestCardCore(int cardId, bool advanceToHumanBoundary) => _resolve().HumanHarvestCardCore(cardId, advanceToHumanBoundary);
    internal void HumanIceSwordCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanIceSwordCore(selected, advanceToHumanBoundary);
    internal void HumanNullificationCore(bool useNullification, int? requestedNullificationCardId, bool advanceToHumanBoundary, CardConversionSource? selectedConversionSource = default) => _resolve().HumanNullificationCore(useNullification, requestedNullificationCardId, advanceToHumanBoundary, selectedConversionSource);
    internal void HumanPlayCore(CharacterState actor, LegalAction action, bool advanceToHumanBoundary) => _resolve().HumanPlayCore(actor, action, advanceToHumanBoundary);
    internal void HumanProgramViewAsSlashResponseCore(IReadOnlyList<int> requestedCardIds, CardConversionSource source, bool advanceToHumanBoundary) => _resolve().HumanProgramViewAsSlashResponseCore(requestedCardIds, source, advanceToHumanBoundary);
    internal void HumanQilinBowCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanQilinBowCore(selected, advanceToHumanBoundary);
    internal void HumanQinglongCrescentBladeCore(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().HumanQinglongCrescentBladeCore(selected, advanceToHumanBoundary);
    internal void HumanRequestFactionDefenseCore(bool advanceToHumanBoundary) => _resolve().HumanRequestFactionDefenseCore(advanceToHumanBoundary);
    internal void HumanRequestFactionSlashCore(bool advanceToHumanBoundary) => _resolve().HumanRequestFactionSlashCore(advanceToHumanBoundary);
    internal void HumanRespondCore(bool useDodge, bool useBagua, int? requestedDodgeCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => _resolve().HumanRespondCore(useDodge, useBagua, requestedDodgeCardId, requestedResponseCardKind, advanceToHumanBoundary);
    internal void HumanRevealGeneralCore(GeneralSelectionSlot slot, bool advanceToHumanBoundary) => _resolve().HumanRevealGeneralCore(slot, advanceToHumanBoundary);
    internal void HumanSelectGeneralCore(string generalId, bool advanceToHumanBoundary) => _resolve().HumanSelectGeneralCore(generalId, advanceToHumanBoundary);
    internal void HumanSelectGodFactionCore(string factionId, bool advanceToHumanBoundary) => _resolve().HumanSelectGodFactionCore(factionId, advanceToHumanBoundary);
    internal void HumanSkipDiscardPolicyCore(bool useSkill, bool advanceToHumanBoundary) => _resolve().HumanSkipDiscardPolicyCore(useSkill, advanceToHumanBoundary);
    internal void HumanSlashResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary) => _resolve().HumanSlashResponseCore(useSlash, requestedSlashCardId, requestedResponseCardKind, advanceToHumanBoundary);
    internal void HumanStoneAxeCore(IReadOnlyList<int> discardedCardIds, bool advanceToHumanBoundary) => _resolve().HumanStoneAxeCore(discardedCardIds, advanceToHumanBoundary);
    internal void HumanTargetCardSelectionCore(int slot, bool advanceToHumanBoundary) => _resolve().HumanTargetCardSelectionCore(slot, advanceToHumanBoundary);
    internal void HumanUseEquipmentEffectCore(CharacterState actor, CardKind equipmentKind, IReadOnlyList<int> cardIds, IReadOnlyList<int> targetSeats, bool advanceToHumanBoundary) => _resolve().HumanUseEquipmentEffectCore(actor, equipmentKind, cardIds, targetSeats, advanceToHumanBoundary);
    internal void HumanZhangbaSlashResponseCore(IReadOnlyList<int> requestedCardIds, bool advanceToHumanBoundary) => _resolve().HumanZhangbaSlashResponseCore(requestedCardIds, advanceToHumanBoundary);
    internal void HumanZhuqueFanCore(bool convertToFireSlash, bool advanceToHumanBoundary) => _resolve().HumanZhuqueFanCore(convertToFireSlash, advanceToHumanBoundary);
}
