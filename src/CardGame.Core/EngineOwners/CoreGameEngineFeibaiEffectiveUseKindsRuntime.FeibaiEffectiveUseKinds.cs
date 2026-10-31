#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsRuntime
{
    internal CardKind FeibaiEffectiveUseKind(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent current)
    {
        // The dependency captured this name at the original native declaration.
        // Keep that exact fact in history; an optional ordinal kind is corroboration.
        var original = _characterCaiYongLeOperations.FeibaiRecordedKind(fact);
        if (fact.EffectiveCardKind is { } recorded && recorded != original)
            throw new InvalidOperationException("Feibai's captured name differs from its native declaration kind.");
        // Response-use facts share their incoming card's origin frame. A Dodge
        // must never consume that incoming Slash's committed kind conversion.
        if (fact.RoundActionType != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(original))
            return original;
        return _coreSlashUseOrdinalsOperations.SlashUseKindAtPrefix2(fact, history, end, current, original);
    }
}
