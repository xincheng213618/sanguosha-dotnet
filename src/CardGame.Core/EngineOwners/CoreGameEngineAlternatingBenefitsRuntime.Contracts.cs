#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreAlternatingBenefitsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramEventHistoryIndex ProgramHistory { get; }

    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}
