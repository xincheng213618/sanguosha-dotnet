#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CharacterLuBuMobileGodCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICharacterLuBuMobileGodCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}

[Core.ProgramCapability(typeof(CoreProgramCardUsePaymentsRuntime))]
internal sealed class CharacterLuBuMobileGodCoreProgramCardUsePaymentsAdapter(Func<CoreProgramCardUsePaymentsRuntime> targetFactory) : ICharacterLuBuMobileGodCoreProgramCardUsePaymentsOperations
{
    private readonly Func<CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}
