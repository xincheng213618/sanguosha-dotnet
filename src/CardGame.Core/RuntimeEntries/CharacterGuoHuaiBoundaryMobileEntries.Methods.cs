#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiBoundaryMobileEntries
{
    internal int BoundaryGuoHuaiCount(int turn, int owner) => _resolve().BoundaryGuoHuaiCount(turn, owner);
    internal global::CardGame.Core.TurnEndingBoundaryFrame? BoundaryGuoHuaiEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BoundaryGuoHuaiEnding(candidate, context);
    internal void CaptureBoundaryGuoHuaiDiscardBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureBoundaryGuoHuaiDiscardBatch(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureBoundaryGuoHuaiDiscardBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBoundaryGuoHuaiDiscardBatch(batch, completed);
    internal void PrepareBoundaryGuoHuaiJingce(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareBoundaryGuoHuaiJingce(supplied);
    internal bool QualifyBoundaryGuoHuaiJingce(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryGuoHuaiJingce(candidate, context);
    internal void RecordBoundaryGuoHuaiDiscard(global::CardGame.Core.CardMovementRecord move, long nativeParent, long action, int turn, int actualOwner) => _resolve().RecordBoundaryGuoHuaiDiscard(move, nativeParent, action, turn, actualOwner);
    internal bool HasBoundaryGuoHuaiJingce { get => _resolve().HasBoundaryGuoHuaiJingce; }
}
