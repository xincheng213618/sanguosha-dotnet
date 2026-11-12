#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZouShiLeRuntime
{
    internal bool HasYunzhengEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "zheng");
    internal bool HasYunzhengHandEntityMark(int ownerSeat, int cardId) =>
        _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasYunzhengEntityMark(cardId);

    SkillProgramStepOutcome ILeZouShiHost.ResolveHuoxin(ProgramSkillFrame frame) => BeginHuoxin(frame);
}
