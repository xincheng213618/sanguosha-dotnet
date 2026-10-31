#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPlacementAndHpPairEntries
{
    internal bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId) => _resolve().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    internal bool CanPlaceOwnedEquipment(int ownerSeat, int targetSeat, Card card, CardLocation source) => _resolve().CanPlaceOwnedEquipment(ownerSeat, targetSeat, card, source);
    internal void FreezeProgramSelectedHpPair(ProgramSkillFrame frame) => _resolve().FreezeProgramSelectedHpPair(frame);
    internal SkillProgramStepOutcome PlaceProgramSelectedEquipment(ProgramSkillFrame frame, int targetSeat, string bind) => _resolve().PlaceProgramSelectedEquipment(frame, targetSeat, bind);
}
