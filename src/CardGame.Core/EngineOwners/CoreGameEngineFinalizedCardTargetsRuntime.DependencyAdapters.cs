#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetWindowProtocolsRuntime))]
internal sealed class CoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsAdapter(Func<CoreProgramTargetWindowProtocolsRuntime> targetFactory) : ICoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsOperations
{
    private readonly Func<CoreProgramTargetWindowProtocolsRuntime> _targetFactory = targetFactory;
    public ProgramTargetWindowProtocols TargetWindowProtocols { get => this._targetFactory().TargetWindowProtocols; }
}
