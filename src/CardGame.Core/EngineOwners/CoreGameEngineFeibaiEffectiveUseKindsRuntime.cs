#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsRuntime
{
    private readonly RuntimeEntries.CharacterCaiYongLeEntries _characterCaiYongLeOperations;
    private readonly RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries _coreSlashUseOrdinalsOperations;
    internal CoreGameEngineFeibaiEffectiveUseKindsRuntime(RuntimeEntries.CharacterCaiYongLeEntries characterCaiYongLeOperations, RuntimeEntries.CoreGameEngineSlashUseOrdinalsEntries coreSlashUseOrdinalsOperations)
    {
        this._characterCaiYongLeOperations = characterCaiYongLeOperations;
        this._coreSlashUseOrdinalsOperations = coreSlashUseOrdinalsOperations;
    }
}
