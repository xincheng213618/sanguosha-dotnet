#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDamageObserverCursorQueriesRuntime
{

    internal ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.DamageCursorEffectiveTop(includeNestedObservers);
    internal static bool DamageObserverRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => RuntimeSkills.RuntimeRules_Core_CommonRules.DamageObserverRidesOn(ride, beneath);
    internal ProgramSkillFrame? TypedDamageProgramObserver(long windowId) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.TypedDamageProgramObserver(windowId);
    internal bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.DamageProgramMatchesWindow(root, window);
    internal bool TypedDamageObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.TypedDamageObserverCandidateMatches(frame, candidate);
    internal bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.TypedDamagePostEventEdge(parent, child);
    internal static bool ProgramMovementReturnMatches(ProgramSkillFrame producer, CardsMovedTriggerWindowFrame movement, bool allowFactionRequestCost = false) => RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement, allowFactionRequestCost);
    internal static bool ProgramHpReturnMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame changed, bool allowFactionRequestCost = false) => RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramHpReturnMatches(producer, changed, allowFactionRequestCost);
    internal static bool HasAlternateProgramMovementReturn(CardsMovedTriggerWindowFrame movement, bool allowFactionRequestCost = false) => RuntimeSkills.RuntimeRules_Core_CommonRules.HasAlternateProgramMovementReturn(movement, allowFactionRequestCost);
    internal bool HasMalformedDamagePostEventEdge(long windowId) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.HasMalformedDamagePostEventEdge(windowId);
    internal bool DamageFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _coreDamageAdjacencyServicesOperations.Value.DamageAdjacency.DamageFrameRidesOn(ride, beneath);
}
