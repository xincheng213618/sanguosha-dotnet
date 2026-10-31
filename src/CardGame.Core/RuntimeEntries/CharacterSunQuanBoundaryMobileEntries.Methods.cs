#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanBoundaryMobileEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId, int sourceSeat, int targetSeat, global::CardGame.Core.RecoveryAttemptCompletion completion, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> ordinary) => _resolve().BoundarySunQuanMobileRecoveryCandidates(parentFrameId, sourceSeat, targetSeat, completion, ordinary);
    internal bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.RecoveryReplacementCandidate candidate) => _resolve().IsBoundarySunQuanMobileRecoveryCandidateCurrent(frame, candidate);
    internal bool IsBoundarySunQuanMobileSelfPeach(long parentFrameId, int sourceSeat, int targetSeat, global::CardGame.Core.RecoveryAttemptCompletion completion) => _resolve().IsBoundarySunQuanMobileSelfPeach(parentFrameId, sourceSeat, targetSeat, completion);
}
