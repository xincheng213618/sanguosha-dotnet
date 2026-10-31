#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramPublicCardProtocolsRuntime : IProgramPublicCardProtocolQueries
{
    private readonly RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries _coreRecipientCategoryMarkObserversOperations;
    internal CoreGameEngineProgramPublicCardProtocolsRuntime(RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries coreRecipientCategoryMarkObserversOperations)
    {
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
    }
}
