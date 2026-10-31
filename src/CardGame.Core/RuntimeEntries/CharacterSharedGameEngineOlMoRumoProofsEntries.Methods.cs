#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsEntries
{
    internal void AssertOlMoRumo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertOlMoRumo(frame);
    internal void AssertOlMoRumoStates() => _resolve().AssertOlMoRumoStates();
    internal bool OlMoRumoCandidateSource(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().OlMoRumoCandidateSource(candidate);
    internal bool OlMoRumoDamageBefore(global::CardGame.Core.OlMoRumoRoundStartedEvent started) => _resolve().OlMoRumoDamageBefore(started);
    internal global::CardGame.Core.OlMoRumoEnteredEvent? OlMoRumoEntry(int ownerSeat) => _resolve().OlMoRumoEntry(ownerSeat);
    internal bool OlMoRumoNativeLossProof(global::CardGame.Core.OlMoRumoHpLossAppliedEvent paid) => _resolve().OlMoRumoNativeLossProof(paid);
    internal bool OlMoRumoParentMatches(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OlMoRumoParentMatches(frame);
}
