#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePublicJournalProjectionRuntime
{
    private readonly ICorePublicJournalProjectionCharacterSimaYiMouOperations _characterSimaYiMouOperations;
    internal CorePublicJournalProjectionRuntime(ICorePublicJournalProjectionCharacterSimaYiMouOperations characterSimaYiMouOperations)
    {
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
    }
}
