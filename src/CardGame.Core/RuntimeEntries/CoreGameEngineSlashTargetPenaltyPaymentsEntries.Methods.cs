#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyPaymentsEntries
{
    internal void ContinueTargetPenaltyChildren(ProgramSkillFrame supplied) => _resolve().ContinueTargetPenaltyChildren(supplied);
    internal void FinishTargetPenalty(ProgramSkillFrame supplied, bool completed) => _resolve().FinishTargetPenalty(supplied, completed);
    internal void PublishTargetPenaltyRecast(ProgramSkillFrame f, int actual) => _resolve().PublishTargetPenaltyRecast(f, actual);
    internal SkillProgramStepOutcome RequireTargetDiscardOrEquipmentRecast(ProgramSkillFrame supplied) => _resolve().RequireTargetDiscardOrEquipmentRecast(supplied);
    internal void ResolveTargetPenaltyChoice(PromptChoice selected) => _resolve().ResolveTargetPenaltyChoice(selected);
    internal bool ResumeTargetPenalty(long frameId) => _resolve().ResumeTargetPenalty(frameId);
    internal bool ReturnTargetPenaltyMovement(ProgramSkillFrame f) => _resolve().ReturnTargetPenaltyMovement(f);
    internal PromptChoice SelectAiTargetPenalty(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiTargetPenalty(decision, f);
    internal string SlashTargetPenaltyCostReason(ProgramSkillFrame f, bool recast) => _resolve().SlashTargetPenaltyCostReason(f, recast);
    internal IReadOnlyList<PromptChoice> TargetPenaltyChoices(ProgramSkillFrame f) => _resolve().TargetPenaltyChoices(f);
    internal bool TargetPenaltyStillMayIssue(ProgramSkillFrame f) => _resolve().TargetPenaltyStillMayIssue(f);
    internal bool ValidTargetPenaltyPayment(ProgramSkillFrame f) => _resolve().ValidTargetPenaltyPayment(f);
    internal long SlashTargetPenaltySequence { get => _resolve().SlashTargetPenaltySequence; }
}
