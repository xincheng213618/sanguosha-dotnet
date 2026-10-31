#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedJudgmentDamageChildrenEntries
{
    internal void AssertFinalizedJudgmentDamageChildren() => _resolve().AssertFinalizedJudgmentDamageChildren();
    internal bool CanBeginFinalizedDamageChildJudgment(global::CardGame.Core.JudgmentFrame original, long parentFrameId, int targetSeat, string reason, global::CardGame.Core.JudgmentContinuationKind continuation, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility? visibility) => _resolve().CanBeginFinalizedDamageChildJudgment(original, parentFrameId, targetSeat, reason, continuation, resultBind, visibility);
    internal global::CardGame.Core.JudgmentFrame? FinalizedJudgmentDamageParent(long requestingProgramId) => _resolve().FinalizedJudgmentDamageParent(requestingProgramId);
}
