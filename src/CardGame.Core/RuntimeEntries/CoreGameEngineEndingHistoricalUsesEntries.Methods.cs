#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingHistoricalUsesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginEndingHistoricalUses(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginEndingHistoricalUses(supplied);
    internal void CompleteHistoricalEndingAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteHistoricalEndingAttack(completion);
    internal void ContinueHistoricalEndingPayment(long id) => _resolve().ContinueHistoricalEndingPayment(id);
    internal bool ExactHistoricalEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactHistoricalEnding(f);
    internal void FinishHistoricalEndingReturn(global::CardGame.Core.EndingHistoricalUseReturn ret) => _resolve().FinishHistoricalEndingReturn(ret);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HistoricalEndingActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HistoricalEndingActions(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HistoricalEndingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HistoricalEndingChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EndingHistoricalUseSlot> OwnPlayHistory(int owner) => _resolve().OwnPlayHistory(owner);
    internal void PublishHistoricalEndingChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHistoricalEndingChoice(f);
    internal void RecordOwnPlayEligibleUse(global::CardGame.Core.CardActionContext action) => _resolve().RecordOwnPlayEligibleUse(action);
    internal void ResolveHistoricalEndingChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHistoricalEndingChoice(choice);
    internal bool ResumeEndingHistoricalUses(long id) => _resolve().ResumeEndingHistoricalUses(id);
    internal void ReturnHistoricalEndingUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnHistoricalEndingUse(use);
    internal bool ReturnRegisteredCompleteHistoricalEndingAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteHistoricalEndingAttack(completion);
    internal global::CardGame.Core.PromptChoice SelectAiHistoricalEndingUse(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiHistoricalEndingUse(decision, f);
    internal bool TracksOwnPlayHistory { get => _resolve().TracksOwnPlayHistory; }
}
