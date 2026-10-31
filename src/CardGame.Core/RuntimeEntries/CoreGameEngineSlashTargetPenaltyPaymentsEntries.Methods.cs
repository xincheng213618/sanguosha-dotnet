#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyPaymentsEntries
{
    internal void ContinueTargetPenaltyChildren(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinueTargetPenaltyChildren(supplied);
    internal void FinishTargetPenalty(global::CardGame.Core.ProgramSkillFrame supplied, bool completed) => _resolve().FinishTargetPenalty(supplied, completed);
    internal void PublishTargetPenaltyRecast(global::CardGame.Core.ProgramSkillFrame f, int actual) => _resolve().PublishTargetPenaltyRecast(f, actual);
    internal global::CardGame.Core.SkillProgramStepOutcome RequireTargetDiscardOrEquipmentRecast(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RequireTargetDiscardOrEquipmentRecast(supplied);
    internal void ResolveTargetPenaltyChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTargetPenaltyChoice(selected);
    internal bool ResumeTargetPenalty(long frameId) => _resolve().ResumeTargetPenalty(frameId);
    internal bool ReturnTargetPenaltyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnTargetPenaltyMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiTargetPenalty(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiTargetPenalty(decision, f);
    internal string SlashTargetPenaltyCostReason(global::CardGame.Core.ProgramSkillFrame f, bool recast) => _resolve().SlashTargetPenaltyCostReason(f, recast);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TargetPenaltyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TargetPenaltyChoices(f);
    internal bool TargetPenaltyStillMayIssue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TargetPenaltyStillMayIssue(f);
    internal bool ValidTargetPenaltyPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidTargetPenaltyPayment(f);
    internal long SlashTargetPenaltySequence { get => _resolve().SlashTargetPenaltySequence; }
}
