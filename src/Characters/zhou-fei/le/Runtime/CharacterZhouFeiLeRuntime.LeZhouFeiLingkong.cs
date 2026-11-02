#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal bool HasLingkongEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "lingkong");
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasLingkongEntityMark(cardId);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:lingkong") && HasLingkongHandEntityMark(owner.Seat, cardId);

}
