#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreSnapshotValueProjectionRuntime
{
    internal CardSnapshot ToSnapshot(Card card) => _cardSnapshotProjectorOperations.Get().Snapshot(card);
    internal CardSnapshot ToJudgmentSnapshot(Card card) => _cardSnapshotProjectorOperations.Get().JudgmentSnapshot(card);
    internal static PendingDecision CloneDecision(PendingDecision decision) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.CloneDecision(decision);
    internal static GameCommand CloneCommand(GameCommand command) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.CloneCommand(command);
    internal static GeneralDefinition CreateHiddenGeneral() => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.CreateHiddenGeneral();
}
