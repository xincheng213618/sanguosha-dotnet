#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedJudgmentDamageChildrenEntries
{
    internal bool CanBeginFinalizedDamageChildJudgment(JudgmentFrame original, long parentFrameId, int targetSeat, string reason, JudgmentContinuationKind continuation, string? resultBind, SkillProgramCardSetVisibility? visibility) => _resolve().CanBeginFinalizedDamageChildJudgment(original, parentFrameId, targetSeat, reason, continuation, resultBind, visibility);
    internal JudgmentFrame? FinalizedJudgmentDamageParent(long requestingProgramId) => _resolve().FinalizedJudgmentDamageParent(requestingProgramId);
}
