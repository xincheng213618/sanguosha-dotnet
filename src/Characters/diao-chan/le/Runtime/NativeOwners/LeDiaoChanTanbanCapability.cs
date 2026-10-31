#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LeDiaoChanTanbanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime) : ILeDiaoChanTanbanHost
{
    public SkillProgramStepOutcome InitializeTanban(ProgramSkillFrame frame) => getCharacterDiaoChanLeRuntime().InitializeTanban(frame);
    public SkillProgramStepOutcome SwapTanbanHandMarks(ProgramSkillFrame frame) => getCharacterDiaoChanLeRuntime().SwapTanbanHandMarks(frame);
}
