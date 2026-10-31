#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoRenProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoRenBoundaryEntries> getCharacterCaoRenBoundaryRuntime) : ICaoRenProgramHost
{
    public SkillProgramStepOutcome DiscardHandOrUseEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().BeginDiscardHandOrUseEquipment(frame);
    public SkillProgramStepOutcome MoveFieldEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().BeginMoveFieldEquipment(frame);
}
