#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenBoundaryMobileEntries
{
    internal void AnswerBoundarySidi(PromptChoice choice) => _resolve().AnswerBoundarySidi(choice);
    internal SkillProgramStepOutcome BeginBoundarySidi(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginBoundarySidi(supplied, effect);
    internal IReadOnlyList<int> BoundarySidiActualTargets(ProgramCardTriggerWindowFrame window) => _resolve().BoundarySidiActualTargets(window);
    internal IReadOnlyList<PromptChoice> BoundarySidiChoices(ProgramSkillFrame frame) => _resolve().BoundarySidiChoices(frame);
    internal int[] BoundarySidiEligibleHolders(int owner, string instance) => _resolve().BoundarySidiEligibleHolders(owner, instance);
    internal IReadOnlyList<BoundarySidiPrediction> BoundarySidiRecords(int owner, string instance) => _resolve().BoundarySidiRecords(owner, instance);
    internal void CollectBoundarySidiUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectBoundarySidiUses(action, window, result);
    internal void CommitBoundarySidiResult(ProgramSkillFrame supplied, string bind, long action, string outcome) => _resolve().CommitBoundarySidiResult(supplied, bind, action, outcome);
    internal bool ContinueBoundarySidiIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueBoundarySidiIneffectiveSimple(id, continuation);
    internal IReadOnlyList<BoundarySidiPredictionView>? GetBoundarySidiPredictionViews(int viewer) => _resolve().GetBoundarySidiPredictionViews(viewer);
    internal bool IsBoundarySidiUseIneffective(CardUseFrame use) => _resolve().IsBoundarySidiUseIneffective(use);
    internal bool ObservesBoundarySidiResponse(CardActionContext action) => _resolve().ObservesBoundarySidiResponse(action);
    internal void PublishBoundarySidi(ProgramSkillFrame frame) => _resolve().PublishBoundarySidi(frame);
    internal bool QualifyBoundarySidi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyBoundarySidi(candidate, trigger, context);
    internal bool TryBoundarySidiEquipmentCommitted(long id, Card equipment) => _resolve().TryBoundarySidiEquipmentCommitted(id, equipment);
    internal bool TryBoundarySidiWindow(ProgramSkillWindowContext? context, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryBoundarySidiWindow(context, out window);
}
