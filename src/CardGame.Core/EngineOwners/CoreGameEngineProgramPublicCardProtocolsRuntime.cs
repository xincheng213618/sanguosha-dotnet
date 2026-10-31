#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramPublicCardProtocolsRuntime : global::CardGame.Core.IProgramPublicCardProtocolQueries
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries _coreRecipientCategoryMarkObserversOperations;
    internal CoreGameEngineProgramPublicCardProtocolsRuntime(global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries coreRecipientCategoryMarkObserversOperations)
    {
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
    }
}
