#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePublicJournalProjectionRuntime
{
    private readonly RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    internal CorePublicJournalProjectionRuntime(RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations)
    {
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
    }
}
