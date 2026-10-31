#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaWanOlEntries
{
    internal bool AdvanceMaWanSubtree() => _resolve().AdvanceMaWanSubtree();
    internal global::CardGame.Core.PromptChoice AiMaWan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMaWan(p, f);
    internal void AnswerMaWan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMaWan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMaWanTargets(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMaWanTargets(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMaWanVotes(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMaWanVotes(supplied);
    internal bool CanOfferMaWan(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMaWan(candidate, trigger, context);
    internal void CollectIssuedMaWanHunjiangCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMaWanHunjiangCandidates(action, window, result);
    internal void CompleteMaWanTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteMaWanTargets(f);
    internal void ContinueMaWan(long id) => _resolve().ContinueMaWan(id);
    internal bool ContinuePaidMaWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMaWan(f);
    internal bool DrainMaWan(long id) => _resolve().DrainMaWan(id);
    internal void FinishMaWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMaWan(f);
    internal bool HasIssuedMaWanHunjiangCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedMaWanHunjiangCandidate(candidate, context);
    internal bool HasMaWanDamageObserver(long id) => _resolve().HasMaWanDamageObserver(id);
    internal bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasMaWanHunjiangTargetTail(use);
    internal void IssueMaWanHunjiangUse(long id, global::CardGame.Core.CardActionContext? action) => _resolve().IssueMaWanHunjiangUse(id, action);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MaWanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaWanChoices(f);
    internal int MaWanChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaWanChooser(f);
    internal int[] MaWanExtraTargets(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => _resolve().MaWanExtraTargets(f, use);
    internal bool MaWanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MaWanFirstChild(f, child);
    internal bool MaWanHasMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardsMovedTriggerWindowFrame moved) => _resolve().MaWanHasMovement(f, moved);
    internal global::CardGame.Core.ProgramSkillFrame? MaWanObserverRoot() => _resolve().MaWanObserverRoot();
    internal bool MaWanObserverSuffix(int root) => _resolve().MaWanObserverSuffix(root);
    internal global::CardGame.Core.CardConversionSource MaWanSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaWanSource(f);
    internal void PublishMaWan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMaWan(f);
    internal void RecordMaWanHunjiangRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordMaWanHunjiangRedirect(redirector, useId, originalSeat, newSeat);
    internal global::CardGame.Core.CardUseFrame RequireMaWanUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireMaWanUse(f);
    internal void ResolveMaWanVotes(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveMaWanVotes(f);
    internal bool ResumeMaWan(long id) => _resolve().ResumeMaWan(id);
    internal bool TryGetMaWanVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primary) => _resolve().TryGetMaWanVirtualPrimaryReturn(use, parent, out primary);
    internal bool HasMaWanCapability { get => _resolve().HasMaWanCapability; }
}
