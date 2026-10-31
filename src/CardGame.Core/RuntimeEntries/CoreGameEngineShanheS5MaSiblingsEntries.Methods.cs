#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5MaSiblingsEntries
{
    internal bool CanContinuePaidShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidShanheS5Tieqi(frame);
    internal bool CanPayShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Card[] cards) => _resolve().CanPayShanheS5Tieqi(frame, cards);
    internal void FinishShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame, bool paid) => _resolve().FinishShanheS5Tieqi(frame, paid);
    internal bool IsShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsShanheS5Tieqi(frame);
    internal bool IsShanheS5TieqiMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsShanheS5TieqiMovement(frame, effect, pending);
    internal void PublishShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShanheS5Tieqi(frame);
    internal void ResolveShanheS5TieqiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveShanheS5TieqiChoice(choice);
    internal bool ResumeShanheS5Tieqi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeShanheS5Tieqi(frame);
    internal global::CardGame.Core.PromptChoice SelectAiShanheS5Tieqi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiShanheS5Tieqi(decision, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheS5TieqiChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheS5TieqiChoices(frame);
    internal global::CardGame.Core.Card[] ShanheS5TieqiColorCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheS5TieqiColorCards(frame);
    internal bool ValidShanheS5TieqiPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidShanheS5TieqiPayment(frame);
}
