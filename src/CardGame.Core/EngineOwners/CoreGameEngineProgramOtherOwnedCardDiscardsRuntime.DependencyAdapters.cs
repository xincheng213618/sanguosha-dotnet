#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramPaidCardRuntime))]
internal sealed class CoreProgramOtherOwnedCardDiscardsProgramPaidCardAdapter(Func<ProgramPaidCardRuntime> targetFactory) : ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations
{
    private readonly Func<ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public ProgramPaidCardRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
