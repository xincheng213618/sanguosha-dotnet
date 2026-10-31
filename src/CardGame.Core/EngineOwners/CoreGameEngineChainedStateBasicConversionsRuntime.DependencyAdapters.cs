#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreChainedStateBasicConversionsCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreChainedStateBasicConversionsCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(CoreCommandSessionQueriesRuntime))]
internal sealed class CoreChainedStateBasicConversionsCoreCommandSessionQueriesAdapter(Func<CoreCommandSessionQueriesRuntime> targetFactory) : ICoreChainedStateBasicConversionsCoreCommandSessionQueriesOperations
{
    private readonly Func<CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
