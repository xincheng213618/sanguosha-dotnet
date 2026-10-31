#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDualColorDuelProgramsEntries
{
    internal SkillProgramStepOutcome BeginDualColorDuel(ProgramSkillFrame input) => _resolve().BeginDualColorDuel(input);
    internal void CancelDualColorDuel(ProgramSkillFrame frame) => _resolve().CancelDualColorDuel(frame);
    internal void CompleteDualColorDuelAttackReturn(ProgramSkillFrame frame, GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteDualColorDuelAttackReturn(frame, completion);
    internal bool CompleteIssuedDualColorDuelAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteIssuedDualColorDuelAttack(completion);
    internal int DualColorChooser(ProgramSkillFrame frame) => _resolve().DualColorChooser(frame);
    internal bool DualColorDraftMatches(ProgramSkillFrame frame) => _resolve().DualColorDraftMatches(frame);
    internal IReadOnlyList<int> DualColorHandIds(int seat, bool red) => _resolve().DualColorHandIds(seat, red);
    internal bool DualColorPaymentMatches(ProgramSkillFrame frame) => _resolve().DualColorPaymentMatches(frame);
    internal bool DualColorSourceCurrent(ProgramSkillFrame frame) => _resolve().DualColorSourceCurrent(frame);
    internal void FinishDualColorDuel(ProgramSkillFrame frame) => _resolve().FinishDualColorDuel(frame);
    internal bool IsDualColorDuelUse(long id) => _resolve().IsDualColorDuelUse(id);
    internal bool MatchesDualColorDuelParent(ProgramSkillFrame frame, ProgramDualColorDuelOrigin origin) => _resolve().MatchesDualColorDuelParent(frame, origin);
    internal void PayDualColorHands(ProgramSkillFrame frame) => _resolve().PayDualColorHands(frame);
    internal void PublishDualColorChoice(ProgramSkillFrame frame) => _resolve().PublishDualColorChoice(frame);
    internal void ResolveDualColorChoice(PromptChoice selected) => _resolve().ResolveDualColorChoice(selected);
    internal bool ResumeDualColorDuel(long frameId) => _resolve().ResumeDualColorDuel(frameId);
    internal void ReturnDualColorDuel(CardUseFrame use) => _resolve().ReturnDualColorDuel(use);
    internal PromptChoice SelectAiDualColorChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiDualColorChoice(decision, frame);
}
