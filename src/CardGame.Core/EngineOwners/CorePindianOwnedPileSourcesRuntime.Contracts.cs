#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePindianOwnedPileSourcesCoreOutsidePhaseDrawDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long OutsidePhaseMovementSequence { get; }
}

internal interface ICorePindianOwnedPileSourcesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanHaiPindianSource(global::CardGame.Core.PindianFrame p, bool revealed);
}
