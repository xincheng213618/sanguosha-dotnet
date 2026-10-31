#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyEntries
{
    internal void AssertRegisteredIncomingDistanceCourtesyReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredIncomingDistanceCourtesyReceipt(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginIncomingDistanceCourtesy(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.IncomingDistanceCourtesyKind kind) => _resolve().BeginIncomingDistanceCourtesy(supplied, kind);
    internal bool CanContinueIncomingDistanceCourtesy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIncomingDistanceCourtesy(f);
    internal bool CanOfferIncomingDistanceCourtesy(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferIncomingDistanceCourtesy(candidate, t, c);
    internal void CommitIncomingDistanceCourtesyMovement(long id, global::CardGame.Core.IncomingDistanceCourtesyMovement movement) => _resolve().CommitIncomingDistanceCourtesyMovement(id, movement);
    internal void CompleteIncomingDistanceCourtesy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteIncomingDistanceCourtesy(f);
    internal bool DrainIncomingDistanceCourtesyChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainIncomingDistanceCourtesyChildren(f);
    internal bool ExactIncomingDistanceEndingParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactIncomingDistanceEndingParent(f);
    internal global::CardGame.Core.Card[] IncomingDistanceCosts(int owner) => _resolve().IncomingDistanceCosts(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> IncomingDistanceCourtesyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IncomingDistanceCourtesyChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> IncomingDistanceCourtesyContributions(int target) => _resolve().IncomingDistanceCourtesyContributions(target);
    internal bool IsIncomingDistanceCourtesyMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsIncomingDistanceCourtesyMovement(f, e, p);
    internal void IssueNextEndingCourtesyBenefit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueNextEndingCourtesyBenefit(supplied);
    internal void PublishIncomingDistanceCourtesyChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishIncomingDistanceCourtesyChoice(f);
    internal void RequireIncomingDistanceCourtesyReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIncomingDistanceCourtesyReceipt(f);
    internal void ResolveIncomingDistanceCourtesyChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveIncomingDistanceCourtesyChoice(choice);
    internal bool ResumeIncomingDistanceCourtesy(long id) => _resolve().ResumeIncomingDistanceCourtesy(id);
    internal bool ReturnIncomingDistanceCourtesyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnIncomingDistanceCourtesyMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiIncomingDistanceCourtesy(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiIncomingDistanceCourtesy(p, f);
}
