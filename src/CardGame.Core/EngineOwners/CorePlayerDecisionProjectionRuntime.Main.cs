#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePlayerDecisionProjectionRuntime
{
    internal PendingDecision? PendingDecision => _coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(_coreMatchDefinitionOptionsAccess._options.HumanSeat)is { } decision ? CoreSnapshotValueProjectionRuntime.CloneDecision(decision) : null;
}
