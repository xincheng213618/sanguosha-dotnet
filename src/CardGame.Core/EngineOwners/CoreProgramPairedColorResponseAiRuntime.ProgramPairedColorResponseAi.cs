#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPairedColorResponseAiRuntime
{
    internal double GetPairedColorDispositionAiScore(SkillProgramTrigger trigger, int owner, ProgramCardTriggerWindowFrame window)
    {
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.OfferPairedColorCardDisposition } effect] || _corePairedColorResponsesOperations.ExactPairedColorResponseLink(window)is not { } pair || pair.ResponseIsRed is not { } red || pair.PairedIsRed != red || pair.ResponseSeat == pair.PairedSeat || owner != pair.ResponseSeat && owner != pair.PairedSeat)
            return 0d;
        var counterpart = owner == pair.ResponseSeat ? pair.PairedSeat : pair.ResponseSeat;
        // Unknown hand faces have no place in an optional-trigger estimate.
        if (!_coreMatchParticipantsPlayersAccess._players[counterpart].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[counterpart]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[counterpart]).Count == 0)
            return 0d;
        var candidate = window.CandidateIndex < window.Candidates.Count ? window.Candidates[window.CandidateIndex] : null;
        return candidate is { } source && source.OwnerSeat == owner && _coreDamageCounterpartAcquisitionOperations.HasDamageCounterpartAcquisition(owner, source.SkillId, effect.StateId!, counterpart) ? 12d : 8d;
    }
}
