#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathProgramsEntries
{
    internal void AdvanceKillDiedProgramCandidate(ProgramKillTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceKillDiedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceOwnerDiedProgramCandidate(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceOwnerDiedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceOwnerDiedProgramCursor(ProgramDeathTriggerWindowFrame frame) => _resolve().AdvanceOwnerDiedProgramCursor(frame);
    internal SkillProgramStepOutcome CauseProgramDeathUnlessBoundCardKind(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<CardKind> excludedCardKinds) => _resolve().CauseProgramDeathUnlessBoundCardKind(frame, sourceBind, excludedCardKinds);
    internal void ChangeProgramAttributedMarker(ProgramSkillFrame frame, ProgramParticipantReference targetReference, PlayerMarkerKind marker, int amount) => _resolve().ChangeProgramAttributedMarker(frame, targetReference, marker, amount);
    internal void ClaimProgramDeathCleanupCards(ProgramSkillFrame frame) => _resolve().ClaimProgramDeathCleanupCards(frame);
    internal void ClearAttributedMarkerSources(CharacterState owner, long resolutionId) => _resolve().ClearAttributedMarkerSources(owner, resolutionId);
    internal void CompleteDeathResolution(long deathFrameId) => _resolve().CompleteDeathResolution(deathFrameId);
    internal void ContinueDeathOutcome(long deathFrameId) => _resolve().ContinueDeathOutcome(deathFrameId);
    internal void ContinueDeathResolution(long deathFrameId) => _resolve().ContinueDeathResolution(deathFrameId);
    internal void ContinueKillDiedProgramWindowCore() => _resolve().ContinueKillDiedProgramWindowCore();
    internal void ContinueOwnerDiedProgramWindowCore() => _resolve().ContinueOwnerDiedProgramWindowCore();
    internal void ContinuePlayerDeathCleanup(long deathFrameId) => _resolve().ContinuePlayerDeathCleanup(deathFrameId);
    internal ProgramSkillWindowContext CreateOwnerDiedProgramContext(ProgramDeathTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateOwnerDiedProgramContext(frame, candidate);
    internal DeathFrame? FilteredDeathClaimParent(int ownerSeat, ProgramSkillWindowContext context) => _resolve().FilteredDeathClaimParent(ownerSeat, context);
    internal DeathFrame GetCurrentDeathFrame(long expectedId) => _resolve().GetCurrentDeathFrame(expectedId);
    internal int GetMarkerSourceCount(CharacterState player, PlayerMarkerKind marker, int skillOwnerSeat) => _resolve().GetMarkerSourceCount(player, marker, skillOwnerSeat);
    internal bool HasInitialFilteredDeathClaimResources(ProgramTriggerLegalityContext eligibility) => _resolve().HasInitialFilteredDeathClaimResources(eligibility);
    internal Card[] QueryProgramDeathCleanupCards(DeathFrame death, IReadOnlyList<CardZoneKind> sourceZones) => _resolve().QueryProgramDeathCleanupCards(death, sourceZones);
    internal bool TryBeginKillDiedProgramWindow(DeathFrame death) => _resolve().TryBeginKillDiedProgramWindow(death);
    internal bool TryBeginOwnerDiedProgramWindow(DeathFrame death, CharacterState owner) => _resolve().TryBeginOwnerDiedProgramWindow(death, owner);
    internal void UpdateDeathFrame(long frameId, Func<DeathFrame, DeathFrame> update) => _resolve().UpdateDeathFrame(frameId, update);
}
