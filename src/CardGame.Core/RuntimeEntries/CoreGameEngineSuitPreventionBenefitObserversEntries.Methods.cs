#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSuitPreventionBenefitObserversEntries
{
    internal bool IsSuitPlacementProgramDying() => _resolve().IsSuitPlacementProgramDying();
    internal bool MatchedJudgmentOriginalCandidate(ProgramSkillFrame root) => _resolve().MatchedJudgmentOriginalCandidate(root);
    internal bool SuitBenefitOriginalCandidate(ProgramSkillFrame root) => _resolve().SuitBenefitOriginalCandidate(root);
    internal bool SuitPlacementDamageEdge(DamageFrame damage, ProgramSkillFrame root) => _resolve().SuitPlacementDamageEdge(damage, root);
    internal bool SuitPlacementFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SuitPlacementFirstChild(root, child);
    internal bool SuitPlacementMovementRoot(ProgramSkillFrame root, CardsMovedTriggerWindowFrame child) => _resolve().SuitPlacementMovementRoot(root, child);
    internal ProgramSkillFrame? SuitPlacementObserverRoot(long? beforeDamageId = default) => _resolve().SuitPlacementObserverRoot(beforeDamageId);
    internal bool SuitPlacementPipelineEdge(int index) => _resolve().SuitPlacementPipelineEdge(index);
}
