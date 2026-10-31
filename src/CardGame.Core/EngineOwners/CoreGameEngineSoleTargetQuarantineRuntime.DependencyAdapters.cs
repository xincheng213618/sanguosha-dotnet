#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetWindowProtocolsRuntime))]
internal sealed class CoreSoleTargetQuarantineCoreProgramTargetWindowProtocolsAdapter(Func<CoreProgramTargetWindowProtocolsRuntime> targetFactory) : ICoreSoleTargetQuarantineCoreProgramTargetWindowProtocolsOperations
{
    private readonly Func<CoreProgramTargetWindowProtocolsRuntime> _targetFactory = targetFactory;
    public ProgramTargetWindowProtocols TargetWindowProtocols { get => this._targetFactory().TargetWindowProtocols; }
}
