using CardGame.Core.RuntimeSkills;

namespace CardGame.Core;

// Synchronous random acquisition reads each original physical area only when
// its participant is visited. The match's one generator owns every random step.
[ProgramRuntimeModule]
internal sealed class ProgramRandomCardAcquisitionRuntime(IProgramRuntimeFrames frames,
    ICardZoneQueries cards, IMatchParticipantStateAccess participants, IProgramContentQueries content,
    IProgramBoundInstructionFacts instructions, IProgramBoundCardMovementOperations movement,
    IProgramRuntimeEvents events, IProgramRandomCardAcquisitionFacts facts,
    IProgramRandomCardAcquisitionNativeEntries native, IMatchRandomSource random)
{
    internal void TakeProgramRandomHandCards(
        long frameId,
        int ownerSeat,
        int amountPerTarget,
        CardMoveReason reason)
    {
        var frame = frames.ActiveProgram(frameId);
        if (amountPerTarget != 1 || frame.SelectedTargetSeats.Count == 0 ||
            frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count)
            throw new InvalidOperationException("The configured random hand-card transfer has invalid targets or amount.");

        var selection = content.Skill(frame.SkillId).Program!.Triggers
            .Single(trigger => trigger.Id == frame.TriggerId).Effects
            .Single(effect => effect.Op == SkillProgramEffectOp.SelectTargets);
        var usesFrozenEligibility = selection.TargetKind == SkillProgramTargetKind.OtherLivingHandAtLeastOwner;
        var validTargets = usesFrozenEligibility ? [] :
            instructions.TargetSeats(ownerSeat, SkillProgramTargetKind.OtherLivingWithHand);
        var takenSeats = new List<int>();
        foreach (var targetSeat in frame.SelectedTargetSeats)
        {
            if (!usesFrozenEligibility && !validTargets.Contains(targetSeat))
                throw new InvalidOperationException("A selected random hand-card target is no longer legal.");
            var target = participants.At(targetSeat);
            if (usesFrozenEligibility && (!target.IsAlive || cards.CardsAt(CardLocation.Hand(target.Seat)).Count == 0))
                continue;
            var hand = cards.CardsAt(CardLocation.Hand(target.Seat));
            var card = hand[random.Next(hand.Count)];
            native.MoveToProcessingWithOwnedTransferReceipt(card, CardLocation.Hand(targetSeat), reason,
                ownerSeat, frame.Id, CardMovementDisclosure.FaceDown);
            movement.Move(card, CardLocation.Processing, CardLocation.Hand(ownerSeat), reason);
            takenSeats.Add(targetSeat);
        }
        events.AdvanceFact(new ProgramRandomHandCardsTakenEvent(
            frame.Id,
            frame.SkillId,
            frame.TriggerId!,
            ownerSeat,
            Array.AsReadOnly(takenSeats.ToArray()),
            takenSeats.Count));
        native.AddLog("SkillEffect", takenSeats.Count == 0
            ? $"{participants.At(ownerSeat).Name} 未从所选目标获得手牌。"
            : $"{participants.At(ownerSeat).Name} 从 {string.Join("、", takenSeats.Select(seat => participants.At(seat).Name))} 各获得一张手牌。",
            ownerSeat);
    }

    /// <summary>
    /// Takes one random card from every other living character in turn order.
    /// Hand cards stay opaque in the public completion fact and log.
    /// </summary>
    internal void TakeProgramRandomCardFromEveryOtherCharacter(
        long frameId,
        int ownerSeat,
        IReadOnlyList<CardZoneKind> zones,
        CardMoveReason reason)
    {
        var frame = frames.ActiveProgram(frameId);
        if (frame.OwnerSeat != ownerSeat || zones.Count == 0 ||
            zones.Any(zone => zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment)) ||
            !participants.At(ownerSeat).IsAlive)
            throw new InvalidOperationException(
                "A multi-character random transfer requires the active living owner and declared areas.");

        var takenSeats = new List<int>();
        foreach (var target in facts.Players
                     .Where(player => player.IsAlive && player.Seat != ownerSeat)
                     .OrderBy(player => facts.RelativeTableOrder(ownerSeat, player.Seat)))
        {
            var candidates = zones
                .SelectMany(zone => zone switch
                {
                    CardZoneKind.Hand => cards.CardsAt(CardLocation.Hand(target.Seat)).Select(card =>
                        (Card: card, Location: CardLocation.Hand(target.Seat))),
                    CardZoneKind.Equipment => cards.CardsAt(CardLocation.Equipment(target.Seat)).Select(card =>
                        (Card: card, Location: CardLocation.Equipment(target.Seat))),
                    CardZoneKind.Judgment => cards.CardsAt(CardLocation.Judgment(target.Seat)).Select(card =>
                        (Card: card, Location: CardLocation.Judgment(target.Seat))),
                    _ => throw new InvalidOperationException($"Unsupported random-transfer area '{zone}'.")
                })
                .OrderBy(entry => entry.Card.Id)
                .ToArray();
            if (candidates.Length == 0) continue;
            var pick = candidates[random.Next(candidates.Length)];
            native.MoveToProcessingWithOwnedTransferReceipt(pick.Card, pick.Location, reason,
                ownerSeat, frame.Id, pick.Location.Zone == CardZoneKind.Hand
                    ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
            if (native.MoveProcessingUnlessDestroyed(pick.Card, CardLocation.Hand(ownerSeat), reason))
                takenSeats.Add(target.Seat);
        }

        events.AdvanceFact(new ProgramRandomCardsTakenFromCharactersEvent(
            frame.Id,
            frame.SkillId,
            RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame),
            ownerSeat,
            Array.AsReadOnly(takenSeats.ToArray()),
            Array.AsReadOnly(zones.ToArray()),
            takenSeats.Count));
        native.AddLog("SkillEffect", takenSeats.Count == 0
            ? $"{participants.At(ownerSeat).Name} 未从其他角色处获得牌。"
            : $"{participants.At(ownerSeat).Name} 从 {string.Join("、", takenSeats.Select(seat => participants.At(seat).Name))} 各随机获得一张牌。",
            ownerSeat);
    }
}
