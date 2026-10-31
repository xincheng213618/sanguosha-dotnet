#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPlacementAndHpPairEntries
{
    internal void AssertEquipmentPlacementAndHpPair(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertEquipmentPlacementAndHpPair(frame, plan);
    internal bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId) => _resolve().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    internal bool CanPlaceOwnedEquipment(int ownerSeat, int targetSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation source) => _resolve().CanPlaceOwnedEquipment(ownerSeat, targetSeat, card, source);
    internal void FreezeProgramSelectedHpPair(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FreezeProgramSelectedHpPair(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome PlaceProgramSelectedEquipment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string bind) => _resolve().PlaceProgramSelectedEquipment(frame, targetSeat, bind);
}
