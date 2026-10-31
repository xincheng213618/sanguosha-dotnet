#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetWindowProtocolsRuntime))]
internal sealed class CoreSoleTargetRedirectCoreProgramTargetWindowProtocolsAdapter(Func<CoreProgramTargetWindowProtocolsRuntime> targetFactory) : ICoreSoleTargetRedirectCoreProgramTargetWindowProtocolsOperations
{
    private readonly Func<CoreProgramTargetWindowProtocolsRuntime> _targetFactory = targetFactory;
    public ProgramTargetWindowProtocols TargetWindowProtocols { get => this._targetFactory().TargetWindowProtocols; }
}
