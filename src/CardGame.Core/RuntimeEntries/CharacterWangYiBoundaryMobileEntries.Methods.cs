#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYiBoundaryMobileEntries
{
    internal void AnswerBoundaryWangYiMijiCount(PromptChoice choice) => _resolve().AnswerBoundaryWangYiMijiCount(choice);
    internal int BoundaryWangYiMijiDrawCount(ProgramSkillFrame f) => _resolve().BoundaryWangYiMijiDrawCount(f);
    internal SkillProgramStepOutcome ChooseBoundaryWangYiMijiCount(ProgramSkillFrame frame) => _resolve().ChooseBoundaryWangYiMijiCount(frame);
    internal bool ValidBoundaryWangYiMijiCount(ProgramSkillFrame f) => _resolve().ValidBoundaryWangYiMijiCount(f);
}
