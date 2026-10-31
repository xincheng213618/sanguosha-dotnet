#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal const string HuaManDiscardReason = "skill-program.huaman.discard", HuaManDrawReason = "skill-program.huaman.draw", HuaManClaimReason = "skill-program.huaman.claim";
    internal SkillProgramStepOutcome ExecuteHuaMan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var w = f.WindowContext;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.HuaMan is not null || f.InstructionIndex != 1 || w is null || !HuaManCurrent(f))
            throw new InvalidOperationException("Hua Man requires the current unpaid native source and original instruction.");
        if (op == SkillProgramEffectOp.DrawAndCountMansiActualBarbarianDamage)
        {
            if (w.DamageFrameId is not { } damage || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(d => d.Id == w.ParentFrameId)is not { } window || window.ParentFrameId != damage || _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window)is not { EffectiveCardKind: CardKind.BarbarianAssault, DamageWasApplied: true } attack || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damage)is not { } paid || paid.Amount != w.Amount || paid.Amount < 1 || attack.ResolutionId != paid.ParentFrameId)
                throw new InvalidOperationException("Mansi cannot claim another card kind, event or damage amount.");
            BeginHuaMan(f, HuaManStage.MansiDraw, op);
            PayHuaManDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), f.OwnerSeat, 1, HuaManStage.MansiDraw);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.AwakenZhanyuanAndOfferXili)
        {
            var progress = owner.HuaManProgress ?? new HuaManProgress(0);
            if (progress.Awakened || progress.MansiCards <= 7 && !_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(f.OwnerSeat, f.SkillId, f.TriggerId) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Preparation } phase || phase.ActorSeat != f.OwnerSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
                throw new InvalidOperationException("Zhanyuan requires actual preparation and its natural or issued awakening condition.");
            owner.HuaManProgress = progress with
            {
                Awakened = true
            };
            BeginHuaMan(f, HuaManStage.MaximumChildren, op, true);
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), 1);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.PaySouyingDirectionalSoleTarget)
        {
            if (!ExactSouyingUse(f, out var use) || use.HuaManSouyingMutation is not null)
                throw new InvalidOperationException("Souying requires its actual directional sole designation.");
            BeginHuaMan(f, HuaManStage.DiscardChoice, op);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = f.HuaMan!with { Souying = w.HuaManSouying } });
            PublishHuaManChoice(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (op != SkillProgramEffectOp.PayXiliActualOtherOwnerDamage || w.SourceSeat is not { } source || source == f.OwnerSeat || source != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || w.TargetSeat is not { } target || !HasRuntimeXili(source) || HasRuntimeXili(target) || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame beforeDamage || beforeDamage.Id != w.ParentFrameId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, IsDelayedJudgmentDamage: false } a || a.SourceSeat != source || a.TargetSeat != target)
            throw new InvalidOperationException("Xili requires its other actual-turn owner's native source damage against a non-Xili target.");
        BeginHuaMan(f, HuaManStage.DiscardChoice, op);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = f.HuaMan!with { OtherSeat = source, DamageTargetSeat = target, DamageAttemptResolutionId = a.ResolutionId } });
        PublishHuaManChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void BeginHuaMan(ProgramSkillFrame f, HuaManStage stage, SkillProgramEffectOp op, bool paid = false)
    {
        var r = new HuaManReceipt(HuaManSource(f), f.GameplayHash, f.InstructionIndex, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage, Paid: paid);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuaMan = r, PendingMovementContinuation = paid ? new(f.OwnerSeat, 0, null) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuaManStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, op));
    }

    internal IReadOnlyList<PromptChoice> HuaManChoices(ProgramSkillFrame f)
    {
        var r = f.HuaMan!;
        if (r.Stage == HuaManStage.MaleChoice)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Gender == GeneralGender.Male).Select(p => new PromptChoice(new($"huaman.{(f.Id)}.male.{(p.Seat)}"), p.Name, [], [p.Seat], new Dictionary<string, string> { { "program-action", "huaman" }, { "branch", "male" } })).Append(new(new($"huaman.{(f.Id)}.cancel"), "不选择男性角色", [], [], new Dictionary<string, string> { { "program-action", "huaman" }, { "branch", "cancel" } })).ToArray());
        if (r.Stage != HuaManStage.DiscardChoice)
            return[];
        var payments = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !c.IsGeneralWeapon).ToList();
        foreach (var card in HuaManDiscardCards(f.OwnerSeat).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.WoodenOxGrain(f.OwnerSeat)))
            payments.Add(new(new($"huaman.{(f.Id)}.grain.{(card.Id)}"), $"弃置【{(card.DisplayName)}】", [card.Id], [], new Dictionary<string, string> { { "source-zone", CardZoneKind.WoodenOxGrain.ToString() }, { "frame-id", f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) } }));
        return Array.AsReadOnly(payments.Select(c => c with { Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>(c.Parameters) { ["program-action"] = "huaman", ["branch"] = "discard" }) }).ToArray());
    }

    internal void PublishHuaManChoice(ProgramSkillFrame f)
    {
        var choices = HuaManChoices(f);
        if (choices.Count == 0)
        {
            FinishHuaMan(f);
            return;
        }

        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, s.Name, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.HuaMan!.Stage == HuaManStage.DiscardChoice,
            Choices = choices,
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.PaySouyingDirectionalSoleTarget, SkillProgramEffectOp.PayXiliActualOtherOwnerDamage, SkillProgramEffectOp.AwakenZhanyuanAndOfferXili)]
    internal PromptChoice SelectHuaManAi(ProgramSkillFrame f) => HuaManChoices(f).First();
    [ProgramAnswer("huaman")]
    internal void ResolveHuaManChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        AssertHuaManFrame(f);
        var r = f.HuaMan!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HuaManChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Hua Man choice lost the actual owner and current physical payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == HuaManStage.MaleChoice)
        {
            CompleteZhanyuan(f, choice.Targets.Count == 1 ? choice.Targets.Single() : null);
            return;
        }

        if (!HuaManCurrent(f) || r.Stage != HuaManStage.DiscardChoice || choice.Cards is not [var id] || !HuaManDiscardLocation(f.OwnerSeat, _cardZonesOperations._cardZones.GetLocation(id)) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), _cardZonesOperations._cardZones.GetLocation(id), OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Hua Man cannot pay a moved, forbidden or foreign entity.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = HuaManStage.DiscardChildren, Before = before, After = before, DiscardedId = id, Paid = true, NativeOwnerHpBefore = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, NativeOwnerHpAfter = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), _cardZonesOperations._cardZones.GetLocation(id), new(HuaManDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = f.HuaMan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 1, NativeOwnerHpAfter = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp } });
        RecordHuaManMovement(f, f.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void RecordHuaManMovement(ProgramSkillFrame f, int subject)
    {
        var r = f.HuaMan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuaManMovementPaidEvent(f.Id, r.Source, r.GameplayHash, r.Stage, r.Before, r.After, r.ActualCount, subject));
    }

    internal void PayHuaManDraw(ProgramSkillFrame f, int subject, int amount, HuaManStage stage)
    {
        var r = f.HuaMan!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = stage, Before = before, After = before, ActualCount = 0, Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = _coreMatchParticipantsPlayersAccess._players[subject].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[subject], amount, true, new(HuaManDrawReason)) : [];
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = f.HuaMan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = ids.Count } });
        RecordHuaManMovement(f, subject);
        if (stage == HuaManStage.MansiDraw)
        {
            var progress = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HuaManProgress ?? new(0);
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HuaManProgress = progress with
            {
                MansiCards = checked(progress.MansiCards + ids.Count)
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MansiActualCardsObtainedEvent(f.Id, r.Source, progress.MansiCards, ids.Count, progress.MansiCards + ids.Count));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainHuaMan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 25200)]
    internal bool ResumeHuaMan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuaMan: { } r } f || f.Id != id || r.Stage is HuaManStage.DiscardChoice or HuaManStage.MaleChoice)
            return false;
        AssertHuaManFrame(f);
        if (DrainHuaMan(f))
            return true;
        if (r.Stage == HuaManStage.DiscardChildren)
        {
            if (r.Souying is not null)
                PaySouyingMutation(f);
            else
                PayXiliIncrease(f);
            return true;
        }

        if (r.Stage == HuaManStage.XiliPayerDraw)
        {
            PayHuaManDraw(f, r.OtherSeat!.Value, 2, HuaManStage.XiliSourceDraw);
            return true;
        }

        if (r.Stage == HuaManStage.MaximumChildren)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var amount = owner.IsAlive && owner.HasHpValue ? Math.Min(1, owner.MaxHp - owner.Hp) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = HuaManStage.RecoveryChildren, RecoveryRequested = amount } });
            if (amount > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (r.Stage == HuaManStage.RecoveryChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = HuaManStage.MaleChoice }, PendingMovementContinuation = null });
                PublishHuaManChoice(f);
            }
            else
                FinishHuaMan(f);
            return true;
        }

        FinishHuaMan(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 25200)]
    internal bool ReturnHuaManMovement(ProgramSkillFrame f) => f.HuaMan is not null && ResumeHuaMan(f.Id);
    internal void FinishHuaMan(ProgramSkillFrame f)
    {
        var r = f.HuaMan!;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HuaManCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Hua Man cannot finish twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuaManCompletedEvent(f.Id, r.Source, r.GameplayHash));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IHuaManProgramHost.ExecuteHuaMan(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteHuaMan(f, op);
}
