#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiBoundaryMobileEntries
{
    internal int BoundaryGuoHuaiCount(int turn, int owner) => _resolve().BoundaryGuoHuaiCount(turn, owner);
    internal TurnEndingBoundaryFrame? BoundaryGuoHuaiEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().BoundaryGuoHuaiEnding(candidate, context);
    internal void CaptureBoundaryGuoHuaiDiscardBatch(CardMovementBatchContext batch) => _resolve().CaptureBoundaryGuoHuaiDiscardBatch(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureBoundaryGuoHuaiDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBoundaryGuoHuaiDiscardBatch(batch, completed);
    internal void PrepareBoundaryGuoHuaiJingce(ProgramSkillFrame supplied) => _resolve().PrepareBoundaryGuoHuaiJingce(supplied);
    internal bool QualifyBoundaryGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryGuoHuaiJingce(candidate, context);
    internal void RecordBoundaryGuoHuaiDiscard(CardMovementRecord move, long nativeParent, long action, int turn, int actualOwner) => _resolve().RecordBoundaryGuoHuaiDiscard(move, nativeParent, action, turn, actualOwner);
    internal bool HasBoundaryGuoHuaiJingce { get => _resolve().HasBoundaryGuoHuaiJingce; }
}
