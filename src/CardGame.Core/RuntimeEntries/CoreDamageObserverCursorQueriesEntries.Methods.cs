#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDamageObserverCursorQueriesEntries
{
    internal ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => _resolve().DamageCursorEffectiveTop(includeNestedObservers);
    internal bool DamageFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().DamageFrameRidesOn(ride, beneath);
    internal bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _resolve().DamageProgramMatchesWindow(root, window);
    internal bool HasMalformedDamagePostEventEdge(long windowId) => _resolve().HasMalformedDamagePostEventEdge(windowId);
    internal bool TypedDamageObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => _resolve().TypedDamageObserverCandidateMatches(frame, candidate);
    internal bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().TypedDamagePostEventEdge(parent, child);
    internal ProgramSkillFrame? TypedDamageProgramObserver(long windowId) => _resolve().TypedDamageProgramObserver(windowId);
}
