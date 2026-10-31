#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoOlGodEntries
{
    internal PromptChoice AiOlGuixin(PendingDecision p, ProgramSkillFrame f) => _resolve().AiOlGuixin(p, f);
    internal void AnswerOlGuixinPriority(PromptChoice chosen) => _resolve().AnswerOlGuixinPriority(chosen);
    internal SkillProgramStepOutcome BeginOlGuixin(ProgramSkillFrame supplied) => _resolve().BeginOlGuixin(supplied);
    internal void FinishOlGuixin(ProgramSkillFrame f, bool cancelled) => _resolve().FinishOlGuixin(f, cancelled);
    internal IReadOnlyList<OlGuixinCandidate> OlGuixinCandidates(ProgramSkillFrame f, int target) => _resolve().OlGuixinCandidates(f, target);
    internal bool ResumeOlGuixin(long id) => _resolve().ResumeOlGuixin(id);
    internal bool RetainOlGuixinPaid(ProgramSkillFrame f) => _resolve().RetainOlGuixinPaid(f);
    internal bool ReturnOlGuixinMovement(ProgramSkillFrame f) => _resolve().ReturnOlGuixinMovement(f);
    internal bool ValidOlGuixin(ProgramSkillFrame f) => _resolve().ValidOlGuixin(f);
}
