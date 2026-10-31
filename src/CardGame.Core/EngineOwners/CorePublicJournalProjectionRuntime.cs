#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePublicJournalProjectionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    internal CorePublicJournalProjectionRuntime(global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations)
    {
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
    }
}
