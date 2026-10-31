#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramChoicesEntries
{
    internal SkillProgramStepOutcome ChooseProgramOption(ProgramSkillFrame frame, int chooserSeat, string resultBind, IReadOnlyList<SkillProgramChoiceOption> options) => _resolve().ChooseProgramOption(frame, chooserSeat, resultBind, options);
    internal int CountChooserProgramBoundCards(ProgramSkillFrame frame, string bind, int chooserSeat) => _resolve().CountChooserProgramBoundCards(frame, bind, chooserSeat);
    internal bool IsClaimableProgramOptionStillAvailable(PromptChoice selected) => _resolve().IsClaimableProgramOptionStillAvailable(selected);
    internal bool IsProgramOptionAvailable(ProgramSkillFrame frame, int chooserSeat, SkillProgramChoiceOption option) => _resolve().IsProgramOptionAvailable(frame, chooserSeat, option);
    internal void ResolveProgramOptionChoice(PromptChoice selected) => _resolve().ResolveProgramOptionChoice(selected);
    internal PromptChoice SelectAiProgramOption(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramOption(decision, frame);
    internal int? TryCountChooserProgramBoundCards(ProgramSkillFrame frame, string bind, int chooserSeat) => _resolve().TryCountChooserProgramBoundCards(frame, bind, chooserSeat);
}
