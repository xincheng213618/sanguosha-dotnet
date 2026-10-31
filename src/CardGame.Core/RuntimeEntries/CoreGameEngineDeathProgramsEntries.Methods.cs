#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathProgramsEntries
{
    internal void AdvanceKillDiedProgramCandidate(global::CardGame.Core.ProgramKillTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceKillDiedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceOwnerDiedProgramCandidate(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceOwnerDiedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceOwnerDiedProgramCursor(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame) => _resolve().AdvanceOwnerDiedProgramCursor(frame);
    internal void AssertKillDiedProgramInvariant() => _resolve().AssertKillDiedProgramInvariant();
    internal void AssertOwnerDiedProgramInvariant() => _resolve().AssertOwnerDiedProgramInvariant();
    internal global::CardGame.Core.SkillProgramStepOutcome CauseProgramDeathUnlessBoundCardKind(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> excludedCardKinds) => _resolve().CauseProgramDeathUnlessBoundCardKind(frame, sourceBind, excludedCardKinds);
    internal void ChangeProgramAttributedMarker(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference targetReference, global::CardGame.Core.PlayerMarkerKind marker, int amount) => _resolve().ChangeProgramAttributedMarker(frame, targetReference, marker, amount);
    internal void ClaimProgramDeathCleanupCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ClaimProgramDeathCleanupCards(frame);
    internal void ClearAttributedMarkerSources(global::CardGame.Core.CharacterState owner, long resolutionId) => _resolve().ClearAttributedMarkerSources(owner, resolutionId);
    internal void CompleteDeathResolution(long deathFrameId) => _resolve().CompleteDeathResolution(deathFrameId);
    internal void ContinueDeathOutcome(long deathFrameId) => _resolve().ContinueDeathOutcome(deathFrameId);
    internal void ContinueDeathResolution(long deathFrameId) => _resolve().ContinueDeathResolution(deathFrameId);
    internal void ContinueKillDiedProgramWindowCore() => _resolve().ContinueKillDiedProgramWindowCore();
    internal void ContinueOwnerDiedProgramWindowCore() => _resolve().ContinueOwnerDiedProgramWindowCore();
    internal void ContinuePlayerDeathCleanup(long deathFrameId) => _resolve().ContinuePlayerDeathCleanup(deathFrameId);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateOwnerDiedProgramContext(global::CardGame.Core.ProgramDeathTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateOwnerDiedProgramContext(frame, candidate);
    internal global::CardGame.Core.DeathFrame? FilteredDeathClaimParent(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FilteredDeathClaimParent(ownerSeat, context);
    internal global::CardGame.Core.DeathFrame GetCurrentDeathFrame(long expectedId) => _resolve().GetCurrentDeathFrame(expectedId);
    internal int GetMarkerSourceCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.PlayerMarkerKind marker, int skillOwnerSeat) => _resolve().GetMarkerSourceCount(player, marker, skillOwnerSeat);
    internal bool HasInitialFilteredDeathClaimResources(global::CardGame.Core.ProgramTriggerLegalityContext eligibility) => _resolve().HasInitialFilteredDeathClaimResources(eligibility);
    internal bool IsCommittedFilteredDeathClaimMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsCommittedFilteredDeathClaimMovement(frame, effect, pending);
    internal global::CardGame.Core.Card[] QueryProgramDeathCleanupCards(global::CardGame.Core.DeathFrame death, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> sourceZones) => _resolve().QueryProgramDeathCleanupCards(death, sourceZones);
    internal bool TryBeginKillDiedProgramWindow(global::CardGame.Core.DeathFrame death) => _resolve().TryBeginKillDiedProgramWindow(death);
    internal bool TryBeginOwnerDiedProgramWindow(global::CardGame.Core.DeathFrame death, global::CardGame.Core.CharacterState owner) => _resolve().TryBeginOwnerDiedProgramWindow(death, owner);
    internal void UpdateDeathFrame(long frameId, global::System.Func<global::CardGame.Core.DeathFrame, global::CardGame.Core.DeathFrame> update) => _resolve().UpdateDeathFrame(frameId, update);
}
