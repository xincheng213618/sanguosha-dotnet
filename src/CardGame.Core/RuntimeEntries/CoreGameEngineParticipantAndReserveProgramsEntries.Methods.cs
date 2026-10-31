#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsEntries
{
    internal bool BeginGameStartingPrograms() => _resolve().BeginGameStartingPrograms();
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteParticipantReserve(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecuteParticipantReserve(effect, frame);
    internal void ExpireAttributedNatureMarkers(int ownerSeat) => _resolve().ExpireAttributedNatureMarkers(ownerSeat);
    internal void ObserveAutomaticDyingTransitions(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAutomaticDyingTransitions(payload);
    internal void PublishPrivateReserveChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPrivateReserveChoice(frame);
    internal void ResolveAutomaticDyingTransitionPrograms(global::CardGame.Core.IGameEvent payload) => _resolve().ResolveAutomaticDyingTransitionPrograms(payload);
    internal void ResolveMarkerOrHpPayment(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveMarkerOrHpPayment(choice);
    internal void ResolvePrivateReserveChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePrivateReserveChoice(choice);
    internal void SetAttributedNatureMarker(int source, int target, global::CardGame.Core.PlayerMarkerKind marker, int amount, long id) => _resolve().SetAttributedNatureMarker(source, target, marker, amount, id);
}
