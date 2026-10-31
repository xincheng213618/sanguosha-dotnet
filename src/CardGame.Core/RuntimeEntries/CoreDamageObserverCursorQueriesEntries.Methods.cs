#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDamageObserverCursorQueriesEntries
{
    internal global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => _resolve().DamageCursorEffectiveTop(includeNestedObservers);
    internal bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().DamageFrameRidesOn(ride, beneath);
    internal bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().DamageProgramMatchesWindow(root, window);
    internal bool HasMalformedDamagePostEventEdge(long windowId) => _resolve().HasMalformedDamagePostEventEdge(windowId);
    internal bool IsPendingDamageProgramDying() => _resolve().IsPendingDamageProgramDying();
    internal global::CardGame.Core.DamageCursorProjection ResolveDamageCursor(global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().ResolveDamageCursor(window);
    internal bool TypedDamageObserverCandidateMatches(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TypedDamageObserverCandidateMatches(frame, candidate);
    internal bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().TypedDamagePostEventEdge(parent, child);
    internal global::CardGame.Core.ProgramSkillFrame? TypedDamageProgramObserver(long windowId) => _resolve().TypedDamageProgramObserver(windowId);
}
