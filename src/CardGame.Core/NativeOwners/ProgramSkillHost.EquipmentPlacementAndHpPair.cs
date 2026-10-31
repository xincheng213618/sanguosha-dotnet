#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public SkillProgramStepOutcome PlaceSelectedEquipment(ProgramSkillFrame frame, int targetSeat, string bind) => getCoreGameEngineEquipmentPlacementAndHpPairRuntime().PlaceProgramSelectedEquipment(frame, targetSeat, bind);
    public void FreezeSelectedHpPair(ProgramSkillFrame frame) => getCoreGameEngineEquipmentPlacementAndHpPairRuntime().FreezeProgramSelectedHpPair(frame);
}
