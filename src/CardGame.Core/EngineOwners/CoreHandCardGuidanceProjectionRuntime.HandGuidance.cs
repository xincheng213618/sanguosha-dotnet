#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHandCardGuidanceProjectionRuntime
{
    internal IReadOnlyList<HandCardGuidance> GetHumanHandGuidance() => _coreHandGuidanceOperations.BuildHumanHandGuidance(_coreNativeLegalActionsOperations.GetHumanLegalActions());
}
