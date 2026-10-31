#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongHost(Func<RuntimeEntries.CharacterLingTongBoundaryMobileEntries> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongHost
{
    public SkillProgramStepOutcome SelectEquipmentPair(ProgramSkillFrame frame) => getCharacterLingTongBoundaryMobileRuntime().SelectBoundaryLingTongEquipmentPair(frame);
}
