#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeLegalActionsRuntime))]
internal sealed class CoreHandGuidanceCoreNativeLegalActionsAdapter(Func<CoreNativeLegalActionsRuntime> targetFactory) : ICoreHandGuidanceCoreNativeLegalActionsOperations
{
    private readonly Func<CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<LegalAction> GetHumanLegalActions() => this._targetFactory().GetHumanLegalActions();
}

[Core.ProgramCapability(typeof(CoreCommandSessionQueriesRuntime))]
internal sealed class CoreHandGuidanceCoreCommandSessionQueriesAdapter(Func<CoreCommandSessionQueriesRuntime> targetFactory) : ICoreHandGuidanceCoreCommandSessionQueriesOperations
{
    private readonly Func<CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
