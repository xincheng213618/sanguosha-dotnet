#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreCommandSessionCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreCommandSessionCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot State { get => this._targetFactory().State; }
}
