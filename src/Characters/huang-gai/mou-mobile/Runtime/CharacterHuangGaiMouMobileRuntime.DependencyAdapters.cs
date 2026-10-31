#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangGaiMouMobileCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterHuangGaiMouMobileCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(CoreProgramCardUsePaymentsRuntime))]
internal sealed class CharacterHuangGaiMouMobileCoreProgramCardUsePaymentsAdapter(Func<CoreProgramCardUsePaymentsRuntime> targetFactory) : ICharacterHuangGaiMouMobileCoreProgramCardUsePaymentsOperations
{
    private readonly Func<CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}
