#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHengAnniversaryRuntime
{
    internal static bool ZhangHengField(CardLocation at) => at is { Zone: CardZoneKind.Equipment or CardZoneKind.Judgment, OwnerSeat: not null };
    internal AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to)
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey(AnniversaryZhangHengComposition.Liangjue) || from == to || !ZhangHengField(from) && !ZhangHengField(to))
            return null;
        // Recording happens after the atomic zone change. Query each original
        // boundary location so a new Hand/Processing identity cannot rewrite
        // the effective color the entity had in its source field zone.
        bool BlackAt(CardLocation at) => ZhangHengField(at) && _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[at.OwnerSeat!.Value], card, at)is Suit.Spade or Suit.Club;
        return new(BlackAt(from), BlackAt(to));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49600)]
    internal bool QualifyAnniversaryZhangHeng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (c.SkillId == AnniversaryZhangHengComposition.Dangzai)
            return w.Window == SkillProgramTriggerWindow.PlayPhaseStarting && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == c.OwnerSeat && ZhangHengDonors(c.OwnerSeat).Any();
        if (c.SkillId != AnniversaryZhangHengComposition.Liangjue)
            return true;
        if (w.MovementBatch is not { } batch || w.MovementIndex is not { } index || index != c.OccurrenceIndex || index < 0 || index >= batch.Movements.Count || batch.Movements[index] is not { AnniversaryZhangHeng: { } frozen } movement || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement))
            return false;
        if (w.Window == SkillProgramTriggerWindow.CardsMoved)
            return frozen.SourceBlack && ZhangHengField(movement.From) && movement.From.OwnerSeat == c.OwnerSeat;
        // One actual entity crossing two of the same owner's field zones is one
        // movement opportunity, already represented by its departure binding.
        return w.Window == SkillProgramTriggerWindow.CardsGained && frozen.DestinationBlack && ZhangHengField(movement.To) && movement.To.OwnerSeat == c.OwnerSeat && !(ZhangHengField(movement.From) && movement.From.OwnerSeat == c.OwnerSeat);
    }
}
