#nullable enable
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineProgramStateProjectionDefinitions
{
internal static readonly IReadOnlyList<ProgramBooleanStateSnapshot> EmptyProgramBooleanStateSnapshots = Array.AsReadOnly(Array.Empty<ProgramBooleanStateSnapshot>());
internal static readonly IReadOnlyList<DirectedTurnCardPolicy> EmptyDirectedPolicySnapshots = Array.AsReadOnly(Array.Empty<DirectedTurnCardPolicy>());
internal static readonly IReadOnlyList<TurnCardActionProhibition> EmptyActionProhibitionSnapshots = Array.AsReadOnly(Array.Empty<TurnCardActionProhibition>());
}
