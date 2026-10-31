#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiuBoundaryMobileEntries
{
    internal SkillProgramStepOutcome BeginBoundaryCaoXiuQingxi(ProgramSkillFrame supplied) => _resolve().BeginBoundaryCaoXiuQingxi(supplied);
    internal void PublishBoundaryCaoXiuQingxi(ProgramSkillFrame frame) => _resolve().PublishBoundaryCaoXiuQingxi(frame);
    internal void ResolveBoundaryCaoXiuQingxi(PromptChoice choice) => _resolve().ResolveBoundaryCaoXiuQingxi(choice);
}
