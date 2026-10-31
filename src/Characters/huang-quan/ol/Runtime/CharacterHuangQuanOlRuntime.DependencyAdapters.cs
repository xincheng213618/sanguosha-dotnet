#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardUsePaymentsRuntime))]
internal sealed class CharacterHuangQuanOlCoreProgramCardUsePaymentsAdapter(Func<CoreProgramCardUsePaymentsRuntime> targetFactory) : ICharacterHuangQuanOlCoreProgramCardUsePaymentsOperations
{
    private readonly Func<CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}
