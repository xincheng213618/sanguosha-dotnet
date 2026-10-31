#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoRenProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoRenBoundaryRuntime>> getCharacterCaoRenBoundaryRuntime) : ICaoRenProgramHost
{
    public SkillProgramStepOutcome DiscardHandOrUseEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().Value.BeginDiscardHandOrUseEquipment(frame);
    public SkillProgramStepOutcome MoveFieldEquipment(ProgramSkillFrame frame) => getCharacterCaoRenBoundaryRuntime().Value.BeginMoveFieldEquipment(frame);
}
