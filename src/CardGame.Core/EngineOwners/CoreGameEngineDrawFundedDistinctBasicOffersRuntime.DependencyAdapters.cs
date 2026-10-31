#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreCommandSessionQueriesRuntime))]
internal sealed class CoreDrawFundedDistinctBasicOffersCoreCommandSessionQueriesAdapter(Func<CoreCommandSessionQueriesRuntime> targetFactory) : ICoreDrawFundedDistinctBasicOffersCoreCommandSessionQueriesOperations
{
    private readonly Func<CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
