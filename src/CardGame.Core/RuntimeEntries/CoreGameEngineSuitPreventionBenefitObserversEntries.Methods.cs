#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSuitPreventionBenefitObserversEntries
{
    internal bool IsSuitPlacementProgramDying() => _resolve().IsSuitPlacementProgramDying();
    internal bool MatchedJudgmentOriginalCandidate(global::CardGame.Core.ProgramSkillFrame root) => _resolve().MatchedJudgmentOriginalCandidate(root);
    internal bool SuitBenefitOriginalCandidate(global::CardGame.Core.ProgramSkillFrame root) => _resolve().SuitBenefitOriginalCandidate(root);
    internal bool SuitPlacementDamageEdge(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.ProgramSkillFrame root) => _resolve().SuitPlacementDamageEdge(damage, root);
    internal bool SuitPlacementFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SuitPlacementFirstChild(root, child);
    internal bool SuitPlacementMovementRoot(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().SuitPlacementMovementRoot(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? SuitPlacementObserverRoot(long? beforeDamageId = default) => _resolve().SuitPlacementObserverRoot(beforeDamageId);
    internal bool SuitPlacementPipelineEdge(int index) => _resolve().SuitPlacementPipelineEdge(index);
}
