#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueHpTargetAnnouncementsEntries
{
    internal void AppendUnannouncedUniqueHpTargets(CardUseFrame supplied, long windowId, List<(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context)> entries) => _resolve().AppendUnannouncedUniqueHpTargets(supplied, windowId, entries);
    internal ActualUseTargetWindowFrame AppendUniqueHpTargetsToCurrentWindow(ActualUseTargetWindowFrame parent) => _resolve().AppendUniqueHpTargetsToCurrentWindow(parent);
    internal ActualUseTargetIdentity? FreezeUniqueHpPeerTarget(CardUseFrame use, int target) => _resolve().FreezeUniqueHpPeerTarget(use, target);
    internal bool IsUniqueHpPeerCandidate(ProgramTriggerCandidate c) => _resolve().IsUniqueHpPeerCandidate(c);
    internal bool MatchesAnnouncedUniqueHpLegacyTarget(ActualUseTargetIdentity identity) => _resolve().MatchesAnnouncedUniqueHpLegacyTarget(identity);
    internal bool TryBeginUnannouncedUniqueHpTargets(NativeOwners.CardAttackHandle attack, bool legacy) => _resolve().TryBeginUnannouncedUniqueHpTargets(attack, legacy);
    internal long? UniqueHpLegacyTargetProducer(CardUseFrame use, int target) => _resolve().UniqueHpLegacyTargetProducer(use, target);
    internal bool HasUniqueHpPeerPrograms { get => _resolve().HasUniqueHpPeerPrograms; }
}
