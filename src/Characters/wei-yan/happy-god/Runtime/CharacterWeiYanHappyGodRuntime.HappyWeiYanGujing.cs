#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal void BeginHappyGujingPayments(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyWeiYan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.GujingDiscardChildren, Cursor = 0, SlotAbolished = false } });
        IssueNextHappyGujingPayment(frame);
    }

    internal void IssueNextHappyGujingPayment(ProgramSkillFrame initial)
    {
        var frame = initial;
        var receipt = frame.HappyWeiYan!;
        while (receipt.Cursor < receipt.SelectedBones.Count)
        {
            var bone = receipt.SelectedBones[receipt.Cursor];
            var canDiscard = _coreMatchParticipantsPlayersAccess._players[bone.Holder].IsAlive && HasExactHappyBonePlacement(_coreAdvancedLifecycleOperations.GetAdvancedCard(bone.CardId), bone.Holder) && _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, bone.Holder, [CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Any(c => c.Cards.Contains(bone.CardId));
            if (!canDiscard)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt = receipt with { Cursor = receipt.Cursor + 1 } });
                continue;
            }

            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { SelectedBone = bone, CardId = bone.CardId, Before = before, After = before, Paid = [], SlotAbolished = false }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, [bone.CardId], new(HappyGujingDiscard), (_, _) => SaveHappyWeiYanMove(frame.Id, HappyWeiYanStage.GujingDiscardChildren, before));
            ContinueHappyWeiYan(frame.Id);
            return;
        }

        var issue = new HappyWeiYanGujingIssue(frame.Id, receipt.Source, receipt.GameplayHash);
        _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].HappyWeiYanGujing = issue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanGujingIssuedEvent(frame.Id, issue.Source, issue.GameplayHash));
        FinishHappyWeiYan(frame);
    }

    internal bool ContinueHappyGujingPayment(ProgramSkillFrame frame)
    {
        if (DrainHappyWeiYan(frame))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = frame.HappyWeiYan!;
        if (receipt.SelectedBone is not { } bone || receipt.Cursor >= receipt.SelectedBones.Count || bone != receipt.SelectedBones[receipt.Cursor])
            throw new InvalidOperationException("Gujing lost the next selected role's real bone payment.");
        if (!receipt.SlotAbolished)
        {
            if (!receipt.Paid.Any(m => m.CardId == bone.CardId && m.From == CardLocation.Equipment(bone.Holder) && m.Reason.Value == HappyGujingDiscard))
                throw new InvalidOperationException("Gujing cannot abolish a slot without first discarding its selected physical bone.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { SlotAbolished = true } });
            _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[bone.Holder], bone.Slot, 0);
            if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence != receipt.After)
                SaveHappyWeiYanMove(frame.Id, HappyWeiYanStage.GujingDiscardChildren, receipt.Before);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanGujingAbolishedEvent(frame.Id, receipt.Cursor, bone.Holder, bone.Slot, receipt.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            return ContinueHappyWeiYan(frame.Id);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, HappyWeiYan = receipt with { Cursor = receipt.Cursor + 1, SelectedBone = null, SlotAbolished = false, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = [] } });
        IssueNextHappyGujingPayment(frame);
        return true;
    }

    internal CardLocation NormalizeHappyWeiYanGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason)
    {
        if (to is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
            return to;
        var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].EquipmentAreaAbolished && _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentSlotCapacity(slot) > 0)
            return to;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HappyWeiYanGujing is not null).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, p.Seat)))
        {
            var issue = owner.HappyWeiYanGujing!;
            if (issue.Source.OwnerSeat != owner.Seat || issue.Source.SkillId != HappyWeiYanComposition.Gujing || issue.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(issue.Source.SkillId).Program!.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanGujingIssuedEvent>().Count(e => e == new HappyWeiYanGujingIssuedEvent(issue.FrameId, issue.Source, issue.GameplayHash)) != 1)
                throw new InvalidOperationException("Gujing's permanent gain replacement lost its accepted limited issue.");
            return CardLocation.Hand(owner.Seat);
        }

        return to;
    }
}
