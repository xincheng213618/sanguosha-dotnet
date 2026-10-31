#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramPaidCardRuntime))]
internal sealed class CoreProgramChoicesProgramPaidCardAdapter(Func<ProgramPaidCardRuntime> targetFactory) : ICoreProgramChoicesProgramPaidCardOperations
{
    private readonly Func<ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public ProgramPaidCardRuntime Get() => this._targetFactory();
}
