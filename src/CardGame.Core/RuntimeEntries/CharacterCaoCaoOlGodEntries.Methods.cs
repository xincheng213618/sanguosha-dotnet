#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoOlGodEntries
{
    internal global::CardGame.Core.PromptChoice AiOlGuixin(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiOlGuixin(p, f);
    internal void AnswerOlGuixinPriority(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerOlGuixinPriority(chosen);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOlGuixin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginOlGuixin(supplied);
    internal void FinishOlGuixin(global::CardGame.Core.ProgramSkillFrame f, bool cancelled) => _resolve().FinishOlGuixin(f, cancelled);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.OlGuixinCandidate> OlGuixinCandidates(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().OlGuixinCandidates(f, target);
    internal bool ResumeOlGuixin(long id) => _resolve().ResumeOlGuixin(id);
    internal bool RetainOlGuixinPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainOlGuixinPaid(f);
    internal bool ReturnOlGuixinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOlGuixinMovement(f);
    internal bool ValidOlGuixin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOlGuixin(f);
}
