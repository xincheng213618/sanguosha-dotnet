#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiRuBoundaryMobileEntries
{
    internal global::CardGame.Core.PromptChoice AiBoundaryLiRuMieji(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiBoundaryLiRuMieji(p, f);
    internal void AnswerBoundaryLiRuMieji(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryLiRuMieji(choice);
    internal global::System.Collections.Generic.IReadOnlyList<int> BoundaryLiRuLostCardTargets(int owner) => _resolve().BoundaryLiRuLostCardTargets(owner);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)> BoundaryLiRuMiejiCards(int target, bool gift) => _resolve().BoundaryLiRuMiejiCards(target, gift);
    internal global::CardGame.Core.CardConversionSource BoundaryLiRuSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryLiRuSource(f);
    internal bool KeepPaidBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidBoundaryLiRuMieji(f);
    internal void PayBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayBoundaryLiRuMieji(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PublishBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryLiRuMieji(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveBoundaryLiRuMieji(supplied);
    internal bool ReturnBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryLiRuMieji(f);
    internal bool ValidBoundaryLiRuMieji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryLiRuMieji(f);
}
