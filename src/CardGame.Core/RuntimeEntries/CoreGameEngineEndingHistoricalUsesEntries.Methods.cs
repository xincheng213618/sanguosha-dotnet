#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingHistoricalUsesEntries
{
    internal SkillProgramStepOutcome BeginEndingHistoricalUses(ProgramSkillFrame supplied) => _resolve().BeginEndingHistoricalUses(supplied);
    internal void CompleteHistoricalEndingAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteHistoricalEndingAttack(completion);
    internal void ContinueHistoricalEndingPayment(long id) => _resolve().ContinueHistoricalEndingPayment(id);
    internal bool ExactHistoricalEnding(ProgramSkillFrame f) => _resolve().ExactHistoricalEnding(f);
    internal void FinishHistoricalEndingReturn(EndingHistoricalUseReturn ret) => _resolve().FinishHistoricalEndingReturn(ret);
    internal IReadOnlyList<LegalAction> HistoricalEndingActions(ProgramSkillFrame f) => _resolve().HistoricalEndingActions(f);
    internal IReadOnlyList<PromptChoice> HistoricalEndingChoices(ProgramSkillFrame f) => _resolve().HistoricalEndingChoices(f);
    internal IReadOnlyList<EndingHistoricalUseSlot> OwnPlayHistory(int owner) => _resolve().OwnPlayHistory(owner);
    internal void PublishHistoricalEndingChoice(ProgramSkillFrame f) => _resolve().PublishHistoricalEndingChoice(f);
    internal void RecordOwnPlayEligibleUse(CardActionContext action) => _resolve().RecordOwnPlayEligibleUse(action);
    internal void ResolveHistoricalEndingChoice(PromptChoice choice) => _resolve().ResolveHistoricalEndingChoice(choice);
    internal bool ResumeEndingHistoricalUses(long id) => _resolve().ResumeEndingHistoricalUses(id);
    internal void ReturnHistoricalEndingUse(CardUseFrame use) => _resolve().ReturnHistoricalEndingUse(use);
    internal bool ReturnRegisteredCompleteHistoricalEndingAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteHistoricalEndingAttack(completion);
    internal PromptChoice SelectAiHistoricalEndingUse(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiHistoricalEndingUse(decision, f);
    internal bool TracksOwnPlayHistory { get => _resolve().TracksOwnPlayHistory; }
}
