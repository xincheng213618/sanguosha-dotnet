#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramChoicesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseProgramOption(global::CardGame.Core.ProgramSkillFrame frame, int chooserSeat, string resultBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramChoiceOption> options) => _resolve().ChooseProgramOption(frame, chooserSeat, resultBind, options);
    internal int CountChooserProgramBoundCards(global::CardGame.Core.ProgramSkillFrame frame, string bind, int chooserSeat) => _resolve().CountChooserProgramBoundCards(frame, bind, chooserSeat);
    internal bool IsClaimableProgramOptionStillAvailable(global::CardGame.Core.PromptChoice selected) => _resolve().IsClaimableProgramOptionStillAvailable(selected);
    internal bool IsProgramOptionAvailable(global::CardGame.Core.ProgramSkillFrame frame, int chooserSeat, global::CardGame.Core.SkillProgramChoiceOption option) => _resolve().IsProgramOptionAvailable(frame, chooserSeat, option);
    internal void ResolveProgramOptionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramOptionChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramOption(decision, frame);
    internal int? TryCountChooserProgramBoundCards(global::CardGame.Core.ProgramSkillFrame frame, string bind, int chooserSeat) => _resolve().TryCountChooserProgramBoundCards(frame, bind, chooserSeat);
}
