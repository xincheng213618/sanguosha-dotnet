#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsEntries
{
    internal bool BeginGameStartingPrograms() => _resolve().BeginGameStartingPrograms();
    internal SkillProgramStepOutcome ExecuteParticipantReserve(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ExecuteParticipantReserve(effect, frame);
    internal void ExpireAttributedNatureMarkers(int ownerSeat) => _resolve().ExpireAttributedNatureMarkers(ownerSeat);
    internal void ObserveAutomaticDyingTransitions(IGameEvent payload) => _resolve().ObserveAutomaticDyingTransitions(payload);
    internal void PublishPrivateReserveChoice(ProgramSkillFrame frame) => _resolve().PublishPrivateReserveChoice(frame);
    internal void ResolveAutomaticDyingTransitionPrograms(IGameEvent payload) => _resolve().ResolveAutomaticDyingTransitionPrograms(payload);
    internal void ResolveMarkerOrHpPayment(PromptChoice choice) => _resolve().ResolveMarkerOrHpPayment(choice);
    internal void ResolvePrivateReserveChoice(PromptChoice choice) => _resolve().ResolvePrivateReserveChoice(choice);
    internal void SetAttributedNatureMarker(int source, int target, PlayerMarkerKind marker, int amount, long id) => _resolve().SetAttributedNatureMarker(source, target, marker, amount, id);
}
