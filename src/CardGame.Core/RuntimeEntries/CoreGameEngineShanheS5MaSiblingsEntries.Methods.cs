#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5MaSiblingsEntries
{
    internal bool CanContinuePaidShanheS5Tieqi(ProgramSkillFrame frame) => _resolve().CanContinuePaidShanheS5Tieqi(frame);
    internal bool CanPayShanheS5Tieqi(ProgramSkillFrame frame, Card[] cards) => _resolve().CanPayShanheS5Tieqi(frame, cards);
    internal void FinishShanheS5Tieqi(ProgramSkillFrame frame, bool paid) => _resolve().FinishShanheS5Tieqi(frame, paid);
    internal bool IsShanheS5Tieqi(ProgramSkillFrame frame) => _resolve().IsShanheS5Tieqi(frame);
    internal void PublishShanheS5Tieqi(ProgramSkillFrame frame) => _resolve().PublishShanheS5Tieqi(frame);
    internal void ResolveShanheS5TieqiChoice(PromptChoice choice) => _resolve().ResolveShanheS5TieqiChoice(choice);
    internal bool ResumeShanheS5Tieqi(ProgramSkillFrame frame) => _resolve().ResumeShanheS5Tieqi(frame);
    internal PromptChoice SelectAiShanheS5Tieqi(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiShanheS5Tieqi(decision, frame);
    internal IReadOnlyList<PromptChoice> ShanheS5TieqiChoices(ProgramSkillFrame frame) => _resolve().ShanheS5TieqiChoices(frame);
    internal Card[] ShanheS5TieqiColorCards(ProgramSkillFrame frame) => _resolve().ShanheS5TieqiColorCards(frame);
    internal bool ValidShanheS5TieqiPayment(ProgramSkillFrame frame) => _resolve().ValidShanheS5TieqiPayment(frame);
}
