#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDualColorDuelProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDualColorDuel(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginDualColorDuel(input);
    internal void CancelDualColorDuel(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CancelDualColorDuel(frame);
    internal void CompleteDualColorDuelAttackReturn(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteDualColorDuelAttackReturn(frame, completion);
    internal bool CompleteIssuedDualColorDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteIssuedDualColorDuelAttack(completion);
    internal int DualColorChooser(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DualColorChooser(frame);
    internal bool DualColorDraftMatches(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DualColorDraftMatches(frame);
    internal global::System.Collections.Generic.IReadOnlyList<int> DualColorHandIds(int seat, bool red) => _resolve().DualColorHandIds(seat, red);
    internal bool DualColorPaymentMatches(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DualColorPaymentMatches(frame);
    internal bool DualColorSourceCurrent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DualColorSourceCurrent(frame);
    internal void FinishDualColorDuel(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishDualColorDuel(frame);
    internal bool IsDualColorDuelUse(long id) => _resolve().IsDualColorDuelUse(id);
    internal bool MatchesDualColorDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramDualColorDuelOrigin origin) => _resolve().MatchesDualColorDuelParent(frame, origin);
    internal void PayDualColorHands(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PayDualColorHands(frame);
    internal void PublishDualColorChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDualColorChoice(frame);
    internal void ResolveDualColorChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDualColorChoice(selected);
    internal bool ResumeDualColorDuel(long frameId) => _resolve().ResumeDualColorDuel(frameId);
    internal void ReturnDualColorDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnDualColorDuel(use);
    internal global::CardGame.Core.PromptChoice SelectAiDualColorChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiDualColorChoice(decision, frame);
}
