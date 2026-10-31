#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(PlayerViewProjector))]
internal sealed class CorePlayerViewProjectionQueriesPlayerViewProjectorAdapter(Func<PlayerViewProjector> targetFactory) : ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations
{
    private readonly Func<PlayerViewProjector> _targetFactory = targetFactory;
    public PlayerViewProjector Get() => this._targetFactory();
}
