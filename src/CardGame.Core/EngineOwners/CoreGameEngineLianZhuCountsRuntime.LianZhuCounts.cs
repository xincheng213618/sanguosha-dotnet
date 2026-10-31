#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLianZhuCountsRuntime
{
    bool global::CardGame.Core.ILianZhuGiftSelectionQueries.CanDeliver(int ownerSeat, int recipientSeat, int cardId) => CanDeliverLianZhuGift(ownerSeat, recipientSeat, cardId);
    internal bool CanDeliverLianZhuGift(int ownerSeat, int recipientSeat, int cardId)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipientSeat) || ownerSeat == recipientSeat)
            return false;
        // Inspect only the giver's own published source regions; a forged ID
        // must not turn this legality query into a foreign-zone probe.
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(ownerSeat)).SingleOrDefault(c => c.Id == cardId);
        if (card is null)
            return false;
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var to = CardLocation.Hand(recipientSeat);
        return !(card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment) && !global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, to) && _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, to) == to;
    }

    internal SkillProgramEffect LianZhuPausedEffect(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
    internal bool UsesRecipientHpLianZhu(ProgramSkillFrame frame) => LianZhuPausedEffect(frame).NumberExpression == SkillProgramNumberExpression.RecipientCurrentHp;
    internal static int LianZhuChoiceAmount(LianZhuReceipt receipt) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_LianZhuCounts.LianZhuChoiceAmount(receipt);
    internal ProgramSkillFrame FreezeLianZhuChoiceAmount(ProgramSkillFrame frame)
    {
        if (!UsesRecipientHpLianZhu(frame))
            return frame;
        var r = frame.LianZhu!;
        if (r.CountPolicy is not null)
            throw new InvalidOperationException("A gift recipient amount can be frozen only once after gift children.");
        var recipient = _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat];
        if (!recipient.IsAlive || !recipient.HasHpValue || recipient.Hp <= 0)
            throw new InvalidOperationException("The gift recipient amount requires a living character's actual positive HP.");
        var count = new LianZhuChoiceAmountReceipt(recipient.Hp, recipient.Hp);
        var current = frame with
        {
            LianZhu = r with
            {
                CountPolicy = count
            },
            PendingMovementContinuation = null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(current);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianZhuChoiceAmountCapturedEvent(frame.Id, r.Source, r.GameplayHash, r.RecipientSeat, count.RecipientHp, count.Amount, r.GiftAfter));
        return current;
    }

    internal void RequireLianZhuChoiceAmount(ProgramSkillFrame frame, LianZhuReceipt r)
    {
        var dynamic = UsesRecipientHpLianZhu(frame);
        if (!dynamic)
        {
            if (r.GiftFrom is not null || r.CountPolicy is not null)
                throw new InvalidOperationException("A fixed gift choice cannot acquire dynamic amount metadata.");
            return;
        }

        var activation = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Activation!;
        if (r.GiftFrom is not { OwnerSeat: var owner, Zone: var zone } || owner != frame.OwnerSeat || !activation.SourceZones.Contains(zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("A dynamic gift lost its exact original HE source.");
        var counts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LianZhuChoiceAmountCapturedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (r.CountPolicy is not { } count)
        {
            if (r.Stage != LianZhuStage.GiftChildren || counts.Length != 0)
                throw new InvalidOperationException("A dynamic recipient choice must retain its issued amount.");
            return;
        }

        if (r.Stage == LianZhuStage.GiftChildren || count.Amount <= 0 || count.Amount != count.RecipientHp || counts is not [var captured] || captured.Source != r.Source || captured.GameplayHash != r.GameplayHash || captured.RecipientSeat != r.RecipientSeat || captured.GiftAfter != r.GiftAfter || captured.RecipientHp != count.RecipientHp || captured.Amount != count.Amount)
            throw new InvalidOperationException("A dynamic recipient choice changed its once-issued HP amount.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 22401)]
    internal void RequireNativeLianZhuChoiceAmount(LianZhuChoiceAmountCapturedEvent captured)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LianZhu: { CountPolicy: { } count } r } frame || frame.Id != captured.FrameId || frame.GameplayHash != captured.GameplayHash || r.Source != captured.Source || !UsesRecipientHpLianZhu(frame) || r.Stage != LianZhuStage.RecipientChoice || frame.PendingMovementContinuation is not null || frame.PendingRecoveryAttempts is { Count: > 0 } || r.RecipientSeat != captured.RecipientSeat || r.GiftAfter != captured.GiftAfter || count.RecipientHp != captured.RecipientHp || count.Amount != captured.Amount || count.Amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].Hp != count.RecipientHp || count.Amount != count.RecipientHp || _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(change => change.ParentFrameId == frame.Id) || _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(batch => batch.ParentFrameId == frame.Id))
            throw new InvalidOperationException("A recipient HP amount must be issued at its exact drained native gift boundary.");
    }

    internal PromptChoice ChooseLianZhuAiChoice(ProgramSkillFrame frame)
    {
        var choices = _characterDongBaiOlOperations.LianZhuChoices(frame);
        if (frame.LianZhu is not { CountPolicy: { } count } r)
            return choices.First();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(r.RecipientSeat);
        var recipient = view.Players.Single(p => p.Seat == r.RecipientSeat);
        var owned = recipient.Hand.Concat(recipient.Equipment).ToDictionary(card => card.Id);
        if (r.Stage == LianZhuStage.SelectingDiscard)
            return choices.OrderBy(choice => choice.Cards.Sum(id => CardCatalog.Get(owned[id].Kind).HandKeepValue)).ThenBy(choice => choice.Cards.Single()).First();
        var draw = choices.Single(choice => choice.Parameters["branch"] == "draw");
        var discard = choices.SingleOrDefault(choice => choice.Parameters["branch"] == "discard");
        if (discard is null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return draw;
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains.GetValueOrDefault(r.RecipientSeat) ?? new SimpleAiBrain(r.RecipientSeat, unchecked(_coreMatchDefinitionOptionsAccess._options.Seed * 397) ^ (r.RecipientSeat + 1), _coreMatchDefinitionOptionsAccess._options.AiPolicyVersion);
        var feedCost = brain.ScoreCardEffectIntervention(view, CardKind.FiveGrains, frame.OwnerSeat) * count.Amount;
        var discardCost = _characterDongBaiOlOperations.LianZhuDiscardOptions(frame).Select(choice => CardCatalog.Get(owned[choice.Cards.Single()].Kind).HandKeepValue).Order().Take(count.Amount).Sum();
        return feedCost > discardCost ? discard : draw;
    }
}
