#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramChildWindowRuntimeRuntime : IProgramChildWindowEntries
{
    private readonly RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    internal CoreProgramChildWindowRuntimeRuntime(RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations)
    {
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
    }
}
