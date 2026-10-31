#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreFeibaiEffectiveUseKindsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind FeibaiRecordedKind(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact);
}

internal interface ICoreFeibaiEffectiveUseKindsCoreSlashUseOrdinalsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind SlashUseKindAtPrefix(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current, global::CardGame.Core.CardKind original);
}
