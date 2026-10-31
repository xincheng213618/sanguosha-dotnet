#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyEntries
{
    internal SkillProgramStepOutcome BeginIncomingDistanceCourtesy(ProgramSkillFrame supplied, IncomingDistanceCourtesyKind kind) => _resolve().BeginIncomingDistanceCourtesy(supplied, kind);
    internal bool CanContinueIncomingDistanceCourtesy(ProgramSkillFrame f) => _resolve().CanContinueIncomingDistanceCourtesy(f);
    internal bool CanOfferIncomingDistanceCourtesy(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferIncomingDistanceCourtesy(candidate, t, c);
    internal void CommitIncomingDistanceCourtesyMovement(long id, IncomingDistanceCourtesyMovement movement) => _resolve().CommitIncomingDistanceCourtesyMovement(id, movement);
    internal void CompleteIncomingDistanceCourtesy(ProgramSkillFrame f) => _resolve().CompleteIncomingDistanceCourtesy(f);
    internal bool DrainIncomingDistanceCourtesyChildren(ProgramSkillFrame f) => _resolve().DrainIncomingDistanceCourtesyChildren(f);
    internal bool ExactIncomingDistanceEndingParent(ProgramSkillFrame f) => _resolve().ExactIncomingDistanceEndingParent(f);
    internal Card[] IncomingDistanceCosts(int owner) => _resolve().IncomingDistanceCosts(owner);
    internal IReadOnlyList<PromptChoice> IncomingDistanceCourtesyChoices(ProgramSkillFrame f) => _resolve().IncomingDistanceCourtesyChoices(f);
    internal IEnumerable<RuleQueryContribution> IncomingDistanceCourtesyContributions(int target) => _resolve().IncomingDistanceCourtesyContributions(target);
    internal void IssueNextEndingCourtesyBenefit(ProgramSkillFrame supplied) => _resolve().IssueNextEndingCourtesyBenefit(supplied);
    internal void PublishIncomingDistanceCourtesyChoice(ProgramSkillFrame f) => _resolve().PublishIncomingDistanceCourtesyChoice(f);
    internal void RequireIncomingDistanceCourtesyReceipt(ProgramSkillFrame f) => _resolve().RequireIncomingDistanceCourtesyReceipt(f);
    internal void ResolveIncomingDistanceCourtesyChoice(PromptChoice choice) => _resolve().ResolveIncomingDistanceCourtesyChoice(choice);
    internal bool ResumeIncomingDistanceCourtesy(long id) => _resolve().ResumeIncomingDistanceCourtesy(id);
    internal bool ReturnIncomingDistanceCourtesyMovement(ProgramSkillFrame f) => _resolve().ReturnIncomingDistanceCourtesyMovement(f);
    internal PromptChoice SelectAiIncomingDistanceCourtesy(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiIncomingDistanceCourtesy(p, f);
}
