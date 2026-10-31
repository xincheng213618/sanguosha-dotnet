#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoRenProgramCapability(Func<RuntimeEntries.CharacterCaoRenBoundaryEntries> getCharacterCaoRenBoundaryRuntime) : ICaoRenProgramHost
{
    public SkillProgramStepOutcome DiscardHandOrUseEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().BeginDiscardHandOrUseEquipment(frame);
    public SkillProgramStepOutcome MoveFieldEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().BeginMoveFieldEquipment(frame);
}
