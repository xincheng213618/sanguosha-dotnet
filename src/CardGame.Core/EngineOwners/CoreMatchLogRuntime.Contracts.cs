#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMatchLogCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouSmSecretLog(global::CardGame.Core.GameLogEntry log);
}

internal interface ICoreMatchLogKernelCoreGameEngineObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ObserverDispatcher Observers { get; }
}
