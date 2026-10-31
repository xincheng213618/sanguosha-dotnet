#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceXingxiaEntries
{
    internal void AnswerSourceXingxiaRed(PromptChoice selected) => _resolve().AnswerSourceXingxiaRed(selected);
    internal void ContinueSourceXingxia(ProgramSkillFrame f) => _resolve().ContinueSourceXingxia(f);
    internal PromptChoice SourceXingxiaAiChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SourceXingxiaAiChoice(p, f);
    internal IReadOnlyList<(Card Card, CardLocation From)> SourceXingxiaRedCards(int payer) => _resolve().SourceXingxiaRedCards(payer);
    internal SkillProgramStepOutcome StartSourceXingxia(ProgramSkillFrame supplied) => _resolve().StartSourceXingxia(supplied);
}
