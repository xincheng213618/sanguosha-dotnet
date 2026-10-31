#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnerBatchEquipmentLossRuntime
{
    internal static SkillProgramTriggerFacts CaptureOwnerBatchEquipmentLoss(SkillProgramTriggerFacts facts, CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, IReadOnlyList<int> matchingIndexes) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.CaptureOwnerBatchEquipmentLoss(facts, batch, candidate, trigger, matchingIndexes);
    internal static bool UsesOwnerBatchEquipmentLoss(SkillProgramTriggerCondition condition) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.UsesOwnerBatchEquipmentLoss(condition);
}
