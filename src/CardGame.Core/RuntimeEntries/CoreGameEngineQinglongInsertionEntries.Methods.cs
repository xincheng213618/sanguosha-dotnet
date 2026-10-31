#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQinglongInsertionEntries
{
    internal void BindQinglongInsertedUse(CardUseFrame child) => _resolve().BindQinglongInsertedUse(child);
    internal bool RestoreContinuationAfterQinglongFollowup(long completedOwnerId) => _resolve().RestoreContinuationAfterQinglongFollowup(completedOwnerId);
    internal void RestoreFactionRequestAfterQinglongChoice(long ownerId) => _resolve().RestoreFactionRequestAfterQinglongChoice(ownerId);
    internal void SuspendContinuationForQinglongFollowup(long outerResolutionId, NativeOwners.FactionCardRequestHandle? followupRequest = default) => _resolve().SuspendContinuationForQinglongFollowup(outerResolutionId, followupRequest);
    internal void SuspendFactionRequestForQinglongChoice(NativeOwners.CardAttackHandle attack) => _resolve().SuspendFactionRequestForQinglongChoice(attack);
    internal bool TryResumeQinglongInsertion() => _resolve().TryResumeQinglongInsertion();
}
