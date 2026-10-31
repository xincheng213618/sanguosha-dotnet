#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLingTongBoundaryMobileEntries> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongHost
{
    public SkillProgramStepOutcome SelectEquipmentPair(ProgramSkillFrame frame) => getCharacterLingTongBoundaryMobileRuntime().SelectBoundaryLingTongEquipmentPair(frame);
}
