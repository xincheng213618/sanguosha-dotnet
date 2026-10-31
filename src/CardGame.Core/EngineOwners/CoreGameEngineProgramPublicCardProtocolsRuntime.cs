#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramPublicCardProtocolsRuntime : global::CardGame.Core.IProgramPublicCardProtocolQueries
{
    private readonly ICoreProgramPublicCardProtocolsCharacterLuSuMouOperations _characterLuSuMouOperations;
    private readonly ICoreProgramPublicCardProtocolsCoreRecipientCategoryMarkObserversOperations _coreRecipientCategoryMarkObserversOperations;
    internal CoreGameEngineProgramPublicCardProtocolsRuntime(ICoreProgramPublicCardProtocolsCharacterLuSuMouOperations characterLuSuMouOperations, ICoreProgramPublicCardProtocolsCoreRecipientCategoryMarkObserversOperations coreRecipientCategoryMarkObserversOperations)
    {
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
    }
}
