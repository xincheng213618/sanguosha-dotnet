#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoRenBoundaryRuntime
{
    SkillProgramStepOutcome ICaoRenProgramHost.DiscardHandOrUseEquipment(ProgramSkillFrame frame) => BeginDiscardHandOrUseEquipment(frame);
    SkillProgramStepOutcome ICaoRenProgramHost.MoveFieldEquipment(ProgramSkillFrame frame) => BeginMoveFieldEquipment(frame);
}
