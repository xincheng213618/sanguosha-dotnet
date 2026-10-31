#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramDamageInsertionRuntimeRuntime : IProgramDamageInsertionFacts
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations _programDamageInsertionOperations;
    internal CoreProgramDamageInsertionRuntimeRuntime(IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations, ICoreProgramDamageInsertionRuntimeProgramDamageInsertionOperations programDamageInsertionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._programDamageInsertionOperations = programDamageInsertionOperations;
    }
}
