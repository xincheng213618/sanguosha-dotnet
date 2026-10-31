#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceXingxiaEntries
{
    internal void AnswerSourceXingxiaRed(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceXingxiaRed(selected);
    internal void ContinueSourceXingxia(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSourceXingxia(f);
    internal global::CardGame.Core.PromptChoice SourceXingxiaAiChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceXingxiaAiChoice(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)> SourceXingxiaRedCards(int payer) => _resolve().SourceXingxiaRedCards(payer);
    internal global::CardGame.Core.SkillProgramStepOutcome StartSourceXingxia(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartSourceXingxia(supplied);
}
