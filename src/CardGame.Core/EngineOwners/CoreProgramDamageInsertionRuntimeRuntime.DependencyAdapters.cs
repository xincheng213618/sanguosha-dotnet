#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramDamageInsertionRuntime))]
internal sealed class CoreProgramDamageInsertionRuntimeProgramDamageInsertionAdapter(Func<ProgramDamageInsertionRuntime> targetFactory) : ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations
{
    private readonly Func<ProgramDamageInsertionRuntime> _targetFactory = targetFactory;
    public ProgramDamageInsertionRuntime Get() => this._targetFactory();
}
