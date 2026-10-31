#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenLinMouRuntime
{
    internal void RecordMouChenLinGift(ProgramSkillFrame f, int giver, int cardId, long before, IReadOnlyList<CardMovementRecord> movements)
    {
        if (f.MouChenLinInvitation is not { Closed: false } invitation || f.SkillId != MouChenLinRuntime.Yaozuo || invitation.Source != new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) || invitation.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || invitation.Phase.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(invitation.Phase) || invitation.Phase.ActorSeat != f.OwnerSeat || !invitation.Eligible.Contains(giver) || invitation.Answered.Contains(giver) || movements is not [var m] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.CardId != cardId || m.Sequence <= before || m.From.OwnerSeat != giver || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || m.To != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Yaozuo requires its own accepted participant's successful actual HE-to-Hand Give invoice.");
        var gift = new MouChenLinGift(giver, cardId, m.From, m.Sequence);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouChenLinInvitation = invitation with { FirstGiver = invitation.FirstGiver ?? giver, Answered = invitation.Answered.Append(giver).ToArray(), Gifts = invitation.Gifts.Append(gift).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinGiftPaidEvent(f.Id, invitation.Source, invitation.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, giver, cardId, m.From, m.Sequence));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinInvitationAnswerEvent(f.Id, giver, true));
        if (invitation.FirstGiver is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinFirstGiftEvent(f.Id, giver, cardId, m.Sequence));
    }

    internal bool IsMouChenLinHandLimitExempt(CharacterState owner, int cardId) => HasMouChenLin && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouChenLinGiftPaidEvent>().Any(e => e.Source.OwnerSeat == owner.Seat && e.CardId == cardId && e.Source.SkillId == MouChenLinRuntime.Yaozuo && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == e.CardId && m.Sequence == e.Sequence && m.From == e.From && m.From.OwnerSeat == e.Giver && m.To == CardLocation.Hand(owner.Seat)));
    internal void IssueMouChenLinNonHandDonorTickets(ProgramSkillFrame f)
    {
        if (f.MouChenLinInvitation is not { Closed: true } r || r.Phase.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.Answered.Count != r.Eligible.Count || r.Eligible.Except(r.Answered).Any())
            throw new InvalidOperationException("Yaozuo's next-damage tickets require the actual closed invitation and all real participant returns.");
        foreach (var target in r.Eligible.Where(s => !r.Gifts.Any(g => g.Giver == s && g.From == CardLocation.Hand(s))))
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouChenLinDamageTicketEvent>().Any(e => e.FrameId == f.Id && e.Target == target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinDamageTicketEvent(f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, target));
    }
}
