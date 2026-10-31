#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanBoundaryMobileEntries
{
    internal IReadOnlyList<RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId, int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion, IReadOnlyList<RecoveryReplacementCandidate> ordinary) => _resolve().BoundarySunQuanMobileRecoveryCandidates(parentFrameId, sourceSeat, targetSeat, completion, ordinary);
    internal bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(RecoveryReplacementFrame frame, RecoveryReplacementCandidate candidate) => _resolve().IsBoundarySunQuanMobileRecoveryCandidateCurrent(frame, candidate);
    internal bool IsBoundarySunQuanMobileSelfPeach(long parentFrameId, int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion) => _resolve().IsBoundarySunQuanMobileSelfPeach(parentFrameId, sourceSeat, targetSeat, completion);
}
