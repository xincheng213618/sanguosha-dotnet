#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeLegalActionsRuntime))]
internal sealed class CoreHandCardGuidanceProjectionCoreNativeLegalActionsAdapter(Func<CoreNativeLegalActionsRuntime> targetFactory) : ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations
{
    private readonly Func<CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<LegalAction> GetHumanLegalActions() => this._targetFactory().GetHumanLegalActions();
}
