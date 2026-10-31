#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralNativeCardsRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 44003)]
    internal void CaptureGuanduDamagingCards(IGameEvent payload)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || payload is not DamageAppliedEvent { Amount: > 0 } applied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || !attack.DamageWasApplied || attack.IsDelayedJudgmentDamage || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != applied.TargetSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { Type: CardActionType.Use } action } use || action.EffectiveKind != use.CardKind || (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind)) || _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduDamagingCardRecordedEvent>().Any(e => e.DamageFrameId == damage.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduDamagingCardRecordedEvent(damage.Id, use.Id, action.ActionId, applied.TargetSeat, use.CardKind));
    }

    internal CardKind? GuanduLastDamageCard(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduDamagingCardRecordedEvent>().LastOrDefault(e => e.TargetSeat == seat)?.Kind;
    internal static string GuanduKindName(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GuanduGeneralNativeCards.GuanduKindName(kind);
    internal bool IsGuanduChoiceResult(ProgramSkillFrame f, ProgramChoiceResultBinding binding)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || f.SkillId != "guandu:choulue" || f.InstructionIndex < 2 || binding.Name != "choulue-kind" || binding.ChooserSeat != f.OwnerSeat || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting || !_coreProgramEventHistoryOperations.ProgramEventHistory<GuanduGeneralCompletedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId) || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions[0].Op != SkillProgramEffectOp.ResolveGuanduChoulue)
            return false;
        var armed = _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChoulueArmedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (binding.OptionId == "none")
            return armed.Length == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduGeneralCompletedEvent>().Any(e => e.FrameId == f.Id && !e.Completed);
        return armed is [var receipt] && receipt.OwnerSeat == f.OwnerSeat && receipt.InstanceId == f.SkillInstanceId && receipt.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && receipt.PhaseBoundaryId == f.WindowContext.ParentFrameId && binding.OptionId == GuanduKindName(receipt.Kind) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == receipt.GiftCardId && m.Sequence > receipt.Before && m.Sequence <= receipt.After && m.From.OwnerSeat == receipt.ProviderSeat && m.To == CardLocation.Hand(f.OwnerSeat)) == 1;
    }

    internal CardKind? GuanduChoulueFrozenBasicKind(ProgramSkillFrame f)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || f.SkillId != "guandu:choulue" || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting)
            return null;
        var armed = _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChoulueArmedEvent>().SingleOrDefault(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.InstanceId == f.SkillInstanceId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseBoundaryId == f.WindowContext.ParentFrameId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(e.Kind));
        return armed is not null && f.ChoiceBindings.Count(b => b.Name == "choulue-kind" && b.OptionId == GuanduKindName(armed.Kind) && b.ChooserSeat == f.OwnerSeat) == 1 ? armed.Kind : null;
    }

    internal void ContinueGuanduChoulue(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage is "begin" or "choulue-target")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "choulue-target");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreGuanduGeneralsOperations.GuanduOwnedCards(p.Seat).Length > 0).Select(p => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "target", $"请 {(p.Name)} 交给你一张牌", targets: [p.Seat])).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "不发动")).ToArray());
            return;
        }

        if (r.Stage == "choulue-gift")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, r.TargetSeat, _coreGuanduGeneralChoicesOperations.GuanduOwnCardChoices(f, r.TargetSeat, "gift"));
            return;
        }

        if (r.Stage != "choulue-arm")
            throw new InvalidOperationException("Choulue lost its paid physical gift.");
        var kind = GuanduLastDamageCard(f.OwnerSeat);
        if (kind is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.ChoiceBindings.Any(b => b.Name == "choulue-kind"))
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
            return;
        }

        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == r.CardId && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From.OwnerSeat == r.TargetSeat && m.To == CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Choulue cannot arm without its exact paid gift.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduChoulueArmedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext!.ParentFrameId, r.TargetSeat, r.CardId, r.MovementBefore, r.MovementAfter, kind.Value));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("choulue-kind", GuanduKindName(kind.Value), f.OwnerSeat)).ToArray()) });
        _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
    }

    internal bool IsGuanduChoulueVirtualOrdinaryProgram(ProgramSkillFrame f, SkillProgramEffect effect) => _coreGuanduActivitiesOperations.IsGuanduActivity && f.SkillId == "guandu:choulue" && effect.Op == SkillProgramEffectOp.UseVirtualOrdinaryTrick && effect.Condition is { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "choulue-kind" } && effect.OutputKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && effect.ActorReference?.Kind == ProgramParticipantRef.Owner && effect.TargetReference?.Kind == ProgramParticipantRef.Owner && f.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && f.ChoiceBindings.Count(b => b.Name == "choulue-kind" && b.OptionId == GuanduKindName(kind) && b.ChooserSeat == f.OwnerSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduChoulueArmedEvent>().Count(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.InstanceId == f.SkillInstanceId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseBoundaryId == f.WindowContext.ParentFrameId && e.Kind == kind) == 1;
    internal IReadOnlyList<PromptChoice> GuanduGiftChoices(ProgramSkillFrame f, bool nonEquipment)
    {
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !nonEquipment || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind));
        return cards.SelectMany(c => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => _coreGuanduGeneralChoicesOperations.GuanduChoice(f, "gift", $"将【{(c.DisplayName)}】交给 {(p.Name)}", [c.Id], [p.Seat]))).Append(_coreGuanduGeneralChoicesOperations.GuanduChoice(f, "skip", "不执行")).ToArray();
    }

    internal void ContinueGuanduYuanlve(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (r.Stage is "begin" or "yuanlve-give")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "yuanlve-give");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, GuanduGiftChoices(f, true));
            return;
        }

        if (r.Stage == "yuanlve-offer")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(r.CardId) != CardLocation.Hand(r.TargetSeat))
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
                return;
            }

            var source = _characterHuangQuanOlOperations.HuangQuanSource(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduYuanlveGiftEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.OwnerSeat, r.TargetSeat, r.CardId, r.MovementBefore, r.MovementAfter));
            var pending = new HuangQuanPending(source, f.GameplayHash, f.InstructionIndex, "jianji", r.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, r.MovementBefore, r.MovementAfter, Array.AsReadOnly(new[] { r.CardId }), null, "offer");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanduGeneral = r with { Stage = "native-use" }, HuangQuan = pending });
            _characterHuangQuanOlOperations.PublishJianjiChoice(f);
            return;
        }

        if (r.Stage == "yuanlve-reward")
        {
            if (r.Used && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, 1, "finish");
            else
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
            return;
        }

        throw new InvalidOperationException("Yuanlve lost its exact offered entity or completed native return.");
    }

    internal bool ValidGuanduYuanlveOffer(ProgramSkillFrame f)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || f.SkillId != "guandu:yuanlve" || f.InstructionIndex != 1 || f.GuanduGeneral is not { Operation: SkillProgramEffectOp.ResolveGuanduYuanlve, Stage: "native-use" } r || f.HuangQuan is not { Kind: "jianji", CardIds: [var id], Stage: "offer" or "issuing" or "native-use" or "returned" } p || p.Source != _characterHuangQuanOlOperations.HuangQuanSource(f) || p.GameplayHash != f.GameplayHash || p.InstructionIndex != f.InstructionIndex || p.RecipientSeat != r.TargetSeat || id != r.CardId || p.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || p.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || p.ActualPhase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || p.Before != r.MovementBefore || p.After != r.MovementAfter || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || p.Occurrence is not null || f.PendingMovementContinuation is not null)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduYuanlveGiftEvent>().Count(e => e == new GuanduYuanlveGiftEvent(f.Id, p.Source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.OwnerSeat, p.RecipientSeat, id, p.Before, p.After)) == 1 && (p.RecipientSeat == f.OwnerSeat && p.Before == p.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > p.Before && m.Sequence <= p.After && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(p.RecipientSeat)) == 1);
    }

    internal bool FinishGuanduYuanlveUse(ProgramSkillFrame f)
    {
        if (f.SkillId != "guandu:yuanlve")
            return false;
        if (!ValidGuanduYuanlveOffer(f) || f.HuangQuan is not { } p || p.Stage is not ("offer" or "returned"))
            throw new InvalidOperationException("Yuanlve must return from its exact native use or declined offer.");
        var used = p.Stage == "returned" && p.NativeUse is { } native && _coreProgramEventHistoryOperations.ProgramEventHistory<JianjiUseReturnedEvent>().Count(e => _characterHuangQuanOlOperations.SameJianjiReceipt(e.Receipt, native)) == 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuangQuanCompletedEvent(f.Id, p.Source, p.Kind, p.NativeUse?.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = null, PendingMovementContinuation = null, GuanduGeneral = f.GuanduGeneral!with { Stage = "yuanlve-reward", Used = used } });
        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
        return true;
    }

    internal GuanduMoushiGiftedEvent? GuanduMoushiOccurrence(int owner, string instance, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.AfterDamageApplied || c.Amount <= 0 || c.SourceSeat is not { } source || c.TargetSeat is not { } victim || source != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } current || current.ActorSeat != source)
            return null;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        foreach (var gift in history.OfType<GuanduMoushiGiftedEvent>().Where(e => e.OwnerSeat == owner && e.InstanceId == instance && e.RecipientSeat == source))
        {
            var index = Array.FindIndex(history, e => e is GuanduMoushiGiftedEvent candidate && candidate == gift);
            var nextTurn = history.Skip(index + 1).OfType<TurnStartedEvent>().FirstOrDefault(e => e.ActorSeat == source);
            if (nextTurn?.TurnNumber == current.TurnNumber && !history.OfType<GuanduMoushiDrawnEvent>().Any(d => d.ProducerFrameId == gift.FrameId && d.ActualPlayToken == current.Token && d.VictimSeat == victim))
                return gift;
        }

        return null;
    }

    internal bool GuanduMoushiQualifies(int owner, string instance, ProgramSkillWindowContext c) => GuanduMoushiOccurrence(owner, instance, c)is not null;
    internal void ContinueGuanduMoushi(ProgramSkillFrame f)
    {
        var r = f.GuanduGeneral!;
        if (f.TriggerId is not null)
        {
            var gift = GuanduMoushiOccurrence(f.OwnerSeat, f.SkillInstanceId, f.WindowContext!);
            if (gift is null)
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduMoushiDrawnEvent(gift.FrameId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.WindowContext!.TargetSeat!.Value, f.WindowContext.DamageFrameId!.Value, _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()!.Token));
            _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, 1, "finish");
            return;
        }

        if (r.Stage is "begin" or "moushi-give")
        {
            f = _coreGuanduGeneralsOperations.GuanduStage(f, "moushi-give");
            _coreGuanduGeneralChoicesOperations.PublishGuanduGeneralChoice(f, f.OwnerSeat, GuanduGiftChoices(f, false));
            return;
        }

        if (r.Stage != "moushi-arm")
            throw new InvalidOperationException("Moushi lost its gift payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduMoushiGiftedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, r.TargetSeat, r.CardId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        _coreGuanduGeneralsOperations.FinishGuanduGeneral(f);
    }
}
