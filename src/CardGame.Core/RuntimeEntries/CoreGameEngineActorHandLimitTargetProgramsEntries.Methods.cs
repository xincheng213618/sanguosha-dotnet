#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> ActorHandLimitPrimaryTargets(global::CardGame.Core.CardActionContext action) => _resolve().ActorHandLimitPrimaryTargets(action);
    internal bool ActorHandLimitTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().ActorHandLimitTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame AppendActorHandLimitTargetCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().AppendActorHandLimitTargetCandidates(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectActorHandLimitTargetCandidates(global::CardGame.Core.CardUseFrame use, long windowId, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> existing) => _resolve().CollectActorHandLimitTargetCandidates(use, windowId, existing);
    internal bool HasIssuedActorHandLimitTargetCandidate(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramCardTriggerCandidate candidate) => _resolve().HasIssuedActorHandLimitTargetCandidate(use, candidate);
    internal bool IsActorHandLimitTargetCandidate(global::CardGame.Core.ProgramCardTriggerCandidate candidate) => _resolve().IsActorHandLimitTargetCandidate(candidate);
    internal global::CardGame.Core.CardUseFrame? LifecycleActorHandLimitUse(global::CardGame.Core.CardActionContext action) => _resolve().LifecycleActorHandLimitUse(action);
    internal void RecordActorHandLimitTargetAnnouncements(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().RecordActorHandLimitTargetAnnouncements(action, window);
    internal bool ShouldCollectActorHandLimitPenalty(global::CardGame.Core.CardActionContext action, int ownerSeat, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().ShouldCollectActorHandLimitPenalty(action, ownerSeat, window);
    internal bool TryBeginAcceptedSlashActorHandLimitTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginAcceptedSlashActorHandLimitTargetPrograms(attack);
    internal bool HasActorHandLimitTargetPrograms { get => _resolve().HasActorHandLimitTargetPrograms; }
}
