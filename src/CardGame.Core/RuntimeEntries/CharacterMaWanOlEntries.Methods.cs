#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaWanOlEntries
{
    internal bool AdvanceMaWanSubtree() => _resolve().AdvanceMaWanSubtree();
    internal PromptChoice AiMaWan(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMaWan(p, f);
    internal void AnswerMaWan(PromptChoice choice) => _resolve().AnswerMaWan(choice);
    internal SkillProgramStepOutcome BeginMaWanTargets(ProgramSkillFrame supplied) => _resolve().BeginMaWanTargets(supplied);
    internal SkillProgramStepOutcome BeginMaWanVotes(ProgramSkillFrame supplied) => _resolve().BeginMaWanVotes(supplied);
    internal bool CanOfferMaWan(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMaWan(candidate, trigger, context);
    internal void CollectIssuedMaWanHunjiangCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMaWanHunjiangCandidates(action, window, result);
    internal void CompleteMaWanTargets(ProgramSkillFrame f) => _resolve().CompleteMaWanTargets(f);
    internal void ContinueMaWan(long id) => _resolve().ContinueMaWan(id);
    internal bool ContinuePaidMaWan(ProgramSkillFrame f) => _resolve().ContinuePaidMaWan(f);
    internal bool DrainMaWan(long id) => _resolve().DrainMaWan(id);
    internal void FinishMaWan(ProgramSkillFrame f) => _resolve().FinishMaWan(f);
    internal bool HasIssuedMaWanHunjiangCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedMaWanHunjiangCandidate(candidate, context);
    internal bool HasMaWanHunjiangTargetTail(CardUseFrame use) => _resolve().HasMaWanHunjiangTargetTail(use);
    internal void IssueMaWanHunjiangUse(long id, CardActionContext? action) => _resolve().IssueMaWanHunjiangUse(id, action);
    internal IReadOnlyList<PromptChoice> MaWanChoices(ProgramSkillFrame f) => _resolve().MaWanChoices(f);
    internal int MaWanChooser(ProgramSkillFrame f) => _resolve().MaWanChooser(f);
    internal int[] MaWanExtraTargets(ProgramSkillFrame f, CardUseFrame use) => _resolve().MaWanExtraTargets(f, use);
    internal bool MaWanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MaWanFirstChild(f, child);
    internal bool MaWanHasMovement(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved) => _resolve().MaWanHasMovement(f, moved);
    internal ProgramSkillFrame? MaWanObserverRoot() => _resolve().MaWanObserverRoot();
    internal bool MaWanObserverSuffix(int root) => _resolve().MaWanObserverSuffix(root);
    internal CardConversionSource MaWanSource(ProgramSkillFrame f) => _resolve().MaWanSource(f);
    internal void PublishMaWan(ProgramSkillFrame f) => _resolve().PublishMaWan(f);
    internal void RecordMaWanHunjiangRedirect(ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordMaWanHunjiangRedirect(redirector, useId, originalSeat, newSeat);
    internal CardUseFrame RequireMaWanUse(ProgramSkillFrame f) => _resolve().RequireMaWanUse(f);
    internal void ResolveMaWanVotes(ProgramSkillFrame f) => _resolve().ResolveMaWanVotes(f);
    internal bool ResumeMaWan(long id) => _resolve().ResumeMaWan(id);
    internal bool TryGetMaWanVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primary) => _resolve().TryGetMaWanVirtualPrimaryReturn(use, parent, out primary);
    internal bool HasMaWanCapability { get => _resolve().HasMaWanCapability; }
}
