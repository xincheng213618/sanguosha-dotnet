#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsEntries
{
    internal IReadOnlyList<int> ActorHandLimitPrimaryTargets(CardActionContext action) => _resolve().ActorHandLimitPrimaryTargets(action);
    internal bool ActorHandLimitTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ActorHandLimitTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal ProgramCardTriggerWindowFrame AppendActorHandLimitTargetCandidates(ProgramCardTriggerWindowFrame frame) => _resolve().AppendActorHandLimitTargetCandidates(frame);
    internal IReadOnlyList<ProgramCardTriggerCandidate> CollectActorHandLimitTargetCandidates(CardUseFrame use, long windowId, IReadOnlyList<ProgramCardTriggerCandidate> existing) => _resolve().CollectActorHandLimitTargetCandidates(use, windowId, existing);
    internal bool HasIssuedActorHandLimitTargetCandidate(CardUseFrame use, ProgramCardTriggerCandidate candidate) => _resolve().HasIssuedActorHandLimitTargetCandidate(use, candidate);
    internal bool IsActorHandLimitTargetCandidate(ProgramCardTriggerCandidate candidate) => _resolve().IsActorHandLimitTargetCandidate(candidate);
    internal CardUseFrame? LifecycleActorHandLimitUse(CardActionContext action) => _resolve().LifecycleActorHandLimitUse(action);
    internal void RecordActorHandLimitTargetAnnouncements(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().RecordActorHandLimitTargetAnnouncements(action, window);
    internal bool ShouldCollectActorHandLimitPenalty(CardActionContext action, int ownerSeat, SkillProgramTriggerWindow window) => _resolve().ShouldCollectActorHandLimitPenalty(action, ownerSeat, window);
    internal bool TryBeginAcceptedSlashActorHandLimitTargetPrograms(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginAcceptedSlashActorHandLimitTargetPrograms(attack);
    internal bool HasActorHandLimitTargetPrograms { get => _resolve().HasActorHandLimitTargetPrograms; }
}
