#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnerBatchEquipmentLossRuntime
{
    internal static SkillProgramTriggerFacts CaptureOwnerBatchEquipmentLoss(SkillProgramTriggerFacts facts, CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, IReadOnlyList<int> matchingIndexes) => RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.CaptureOwnerBatchEquipmentLoss(facts, batch, candidate, trigger, matchingIndexes);
    internal static bool UsesOwnerBatchEquipmentLoss(SkillProgramTriggerCondition condition) => RuntimeSkills.RuntimeRules_Core_OwnerBatchEquipmentLoss.UsesOwnerBatchEquipmentLoss(condition);
}
