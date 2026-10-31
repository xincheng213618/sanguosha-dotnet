#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueHpTargetAnnouncementsEntries
{
    internal void AppendUnannouncedUniqueHpTargets(global::CardGame.Core.CardUseFrame supplied, long windowId, global::System.Collections.Generic.List<(global::CardGame.Core.ProgramTriggerCandidate Candidate, global::CardGame.Core.ProgramSkillWindowContext Context)> entries) => _resolve().AppendUnannouncedUniqueHpTargets(supplied, windowId, entries);
    internal global::CardGame.Core.ActualUseTargetWindowFrame AppendUniqueHpTargetsToCurrentWindow(global::CardGame.Core.ActualUseTargetWindowFrame parent) => _resolve().AppendUniqueHpTargetsToCurrentWindow(parent);
    internal global::CardGame.Core.ActualUseTargetIdentity? FreezeUniqueHpPeerTarget(global::CardGame.Core.CardUseFrame use, int target) => _resolve().FreezeUniqueHpPeerTarget(use, target);
    internal bool IsUniqueHpPeerCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsUniqueHpPeerCandidate(c);
    internal bool MatchesAnnouncedUniqueHpLegacyTarget(global::CardGame.Core.ActualUseTargetIdentity identity) => _resolve().MatchesAnnouncedUniqueHpLegacyTarget(identity);
    internal bool TryBeginUnannouncedUniqueHpTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy) => _resolve().TryBeginUnannouncedUniqueHpTargets(attack, legacy);
    internal long? UniqueHpLegacyTargetProducer(global::CardGame.Core.CardUseFrame use, int target) => _resolve().UniqueHpLegacyTargetProducer(use, target);
    internal bool HasUniqueHpPeerPrograms { get => _resolve().HasUniqueHpPeerPrograms; }
}
