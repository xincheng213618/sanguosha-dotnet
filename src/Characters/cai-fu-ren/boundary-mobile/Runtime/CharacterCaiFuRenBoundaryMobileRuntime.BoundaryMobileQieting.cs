#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenBoundaryMobileRuntime
{
    internal const string BoundaryMobileQietingSkill = "boundary:qieting-mobile", BoundaryMobileQietingHandReason = "skill-program.cai-fu-ren.qieting.hand";
    internal static bool IsBoundaryMobileQieting(ProgramSkillFrame frame) => frame.SkillId == BoundaryMobileQietingSkill;
    internal static bool IsKnownCaiFuRenQietingBinding(string skill, string? binding) => skill == "ol:qieting" && binding == "ordinary-turn-end" || skill == BoundaryMobileQietingSkill && binding == "mobile-turn-end";
    internal static bool IsCaiFuRenQietingOperation(ProgramSkillFrame frame, SkillProgramEffect? effect) => effect?.Op == (IsBoundaryMobileQieting(frame) ? SkillProgramEffectOp.ResolveBoundaryMobileQieting : SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting);
    internal bool CaiFuRenQietingDisqualified(string skill, OrdinaryQietingTurnContext qualification)
    {
        if (skill != BoundaryMobileQietingSkill)
            return qualification.UsedOnAnother || _characterCaiFuRenOlOperations.OrdinaryQietingUsedOnAnother(qualification);
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(checked((int)qualification.HistoryCutoff)).ToArray();
        var end = Array.FindLastIndex(history, entry => entry is TurnEndedEvent ended && ended.TurnNumber == qualification.ActualTurn && ended.ActorSeat == qualification.TurnOwnerSeat);
        var start = end < 0 ? -1 : Array.FindLastIndex(history, end, entry => entry is TurnStartedEvent begun && begun.TurnNumber == qualification.ActualTurn && begun.ActorSeat == qualification.TurnOwnerSeat);
        return start < 0 || end <= start || history.Skip(start + 1).Take(end - start - 1).OfType<DamageAppliedEvent>().Any(damage => !damage.SourceLess && damage.Amount > 0 && damage.SourceSeat == qualification.TurnOwnerSeat && damage.TargetSeat != qualification.TurnOwnerSeat);
    }

    SkillProgramStepOutcome global::CardGame.Core.IBoundaryMobileQietingHost.Begin(ProgramSkillFrame frame) => _characterCaiFuRenOlOperations.BeginOrdinaryQieting(frame);
    internal IReadOnlyList<PromptChoice> BoundaryMobileQietingHandChoices(ProgramSkillFrame frame) => frame.OrdinaryQieting is { Paid: true, Option: "hand", Stage: OrdinaryQietingStage.Viewing, MobileSelection: { } selection } receipt ? Array.AsReadOnly(selection.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(receipt.Qualification.TurnOwnerSeat)).Select(id => new PromptChoice(new($"ordinary-qieting.{(frame.Id)}.hand.{(id)}"), $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [id], [frame.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "ordinary-qieting", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["option"] = "take-hand", ["source-zone"] = "Hand", ["card-owner-seat"] = receipt.Qualification.TurnOwnerSeat.ToString(CultureInfo.InvariantCulture) })).ToArray()) : [];
    internal void AnswerBoundaryMobileQieting(ProgramSkillFrame frame, PromptChoice choice)
    {
        _characterCaiFuRenOlOperations.AssertOrdinaryQieting(frame);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !_characterCaiFuRenOlOperations.OrdinaryQietingChoices(frame).Any(candidate => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([candidate], [choice])) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("Mobile Qieting requires the current original single option or one viewed entity.");
        var receipt = frame.OrdinaryQieting!;
        var option = choice.Parameters["option"];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (receipt.Stage == OrdinaryQietingStage.Viewing)
        {
            if (option != "take-hand" || choice.Cards is not [var id] || receipt.MobileSelection is not { } selection || !selection.Viewed.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(receipt.Qualification.TurnOwnerSeat))
                throw new InvalidOperationException("Mobile Qieting cannot obtain any card outside its original two-card private view.");
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Stage = OrdinaryQietingStage.MovementChildren, CardId = id, PrintedKind = card.Kind, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(receipt.Qualification.TurnOwnerSeat), CardLocation.Hand(frame.OwnerSeat), new(BoundaryMobileQietingHandReason));
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = frame.OrdinaryQieting!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _characterCaiFuRenOlOperations.ContinueOrdinaryQieting(frame.Id);
            return;
        }

        if (receipt.Paid || receipt.Stage != OrdinaryQietingStage.Choice)
            throw new InvalidOperationException("Mobile Qieting cannot issue a second benefit.");
        if (option == "hand")
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.Qualification.TurnOwnerSeat]).Select(card => card.Id).ToArray();
            var candidates = hand.ToList();
            var viewed = new List<int>();
            var randomBefore = _randomOperations._random.State;
            while (viewed.Count < 2 && candidates.Count > 0)
            {
                var index = _randomOperations._random.Next(candidates.Count);
                viewed.Add(candidates[index]);
                candidates.RemoveAt(index);
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = receipt with { Paid = true, Option = "hand", Stage = OrdinaryQietingStage.Viewing, MobileSelection = new(randomBefore, _randomOperations._random.State) { Hand = hand, Viewed = viewed } } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "hand", null, null, before));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryMobileQietingViewedEvent(frame.Id, frame.OwnerSeat, receipt.Qualification.TurnOwnerSeat, viewed.Count));
            _characterCaiFuRenOlOperations.PublishOrdinaryQieting(frame);
            return;
        }

        if (option == "draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Paid = true, Option = "draw", Stage = OrdinaryQietingStage.Draw, Before = before, After = before } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "draw", null, null, before));
            _characterCaiFuRenOlOperations.ContinueOrdinaryQieting(frame.Id);
            return;
        }

        var equipment = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
        if (!_characterCaiFuRenOlOperations.CanMoveOrdinaryQietingEquipment(frame, equipment))
            throw new InvalidOperationException("Mobile Qieting lost its real current equipment.");
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(equipment);
        var current = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == slot).ToArray();
        var replaced = current.Length >= _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].EquipmentSlotCapacity(slot) ? current[0] : null;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryQieting = receipt with { Paid = true, Option = "equipment", Stage = OrdinaryQietingStage.MovementChildren, CardId = equipment.Id, PrintedKind = equipment.Kind, Before = before, After = before, MobileSelection = new(0, 0) { ReplacedCardId = replaced?.Id, ReplacedKind = replaced?.Kind }, GrainIds = equipment.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[receipt.Qualification.TurnOwnerSeat]).Select(card => card.Id).ToArray() : [] }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingOptionIssuedEvent(frame.Id, "equipment", equipment.Id, equipment.Kind, before));
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(equipment, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        var from = CardLocation.Equipment(receipt.Qualification.TurnOwnerSeat);
        _corePhysicalMovementFacadesOperations.MoveCard(equipment, from, CardLocation.Equipment(frame.OwnerSeat), new("skill-program.cai-fu-ren.qieting.move"), movement =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { OrdinaryQieting = active.OrdinaryQieting!with { After = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryQietingEquipmentMovedEvent(frame.Id, equipment.Id, equipment.Kind, from, movement.To, movement.Sequence));
        });
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OrdinaryQieting = frame.OrdinaryQieting!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, frame.OwnerSeat, slot, equipment.Id, equipment.Kind, replaced?.Id));
        _characterCaiFuRenOlOperations.ContinueOrdinaryQieting(frame.Id);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 56600)]
    internal CardSnapshot[] BoundaryMobileQietingPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.OwnerSeat == viewer && IsBoundaryMobileQieting(frame) && frame.OrdinaryQieting is { Stage: OrdinaryQietingStage.Viewing, MobileSelection: not null }).SelectMany(frame => frame.OrdinaryQieting!.MobileSelection!.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.OrdinaryQieting.Qualification.TurnOwnerSeat)).Select(id => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[frame.OrdinaryQieting.Qualification.TurnOwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryMobileQieting)]
    internal PromptChoice ChooseBoundaryMobileQietingAi(PendingDecision decision, ProgramSkillFrame frame) => _characterCaiFuRenOlOperations.ChooseOrdinaryQietingAi(decision, frame);
}
