#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenBoundaryMobileEntries
{
    internal void AnswerBoundarySidi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundarySidi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundarySidi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginBoundarySidi(supplied, effect);
    internal global::System.Collections.Generic.IReadOnlyList<int> BoundarySidiActualTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().BoundarySidiActualTargets(window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundarySidiChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundarySidiChoices(frame);
    internal int[] BoundarySidiEligibleHolders(int owner, string instance) => _resolve().BoundarySidiEligibleHolders(owner, instance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BoundarySidiPrediction> BoundarySidiRecords(int owner, string instance) => _resolve().BoundarySidiRecords(owner, instance);
    internal void CollectBoundarySidiUses(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectBoundarySidiUses(action, window, result);
    internal void CommitBoundarySidiResult(global::CardGame.Core.ProgramSkillFrame supplied, string bind, long action, string outcome) => _resolve().CommitBoundarySidiResult(supplied, bind, action, outcome);
    internal bool ContinueBoundarySidiIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueBoundarySidiIneffectiveSimple(id, continuation);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BoundarySidiPredictionView>? GetBoundarySidiPredictionViews(int viewer) => _resolve().GetBoundarySidiPredictionViews(viewer);
    internal bool IsBoundarySidiUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsBoundarySidiUseIneffective(use);
    internal bool ObservesBoundarySidiResponse(global::CardGame.Core.CardActionContext action) => _resolve().ObservesBoundarySidiResponse(action);
    internal void PublishBoundarySidi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundarySidi(frame);
    internal bool QualifyBoundarySidi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundarySidi(candidate, trigger, context);
    internal bool TryBoundarySidiEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => _resolve().TryBoundarySidiEquipmentCommitted(id, equipment);
    internal bool TryBoundarySidiWindow(global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryBoundarySidiWindow(context, out window);
}
