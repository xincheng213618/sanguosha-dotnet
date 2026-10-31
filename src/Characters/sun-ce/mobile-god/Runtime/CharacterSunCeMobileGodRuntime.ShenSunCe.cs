#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunCeMobileGodRuntime
{
    internal const string ShenSunCeYingba = "mobile:yingba", ShenSunCeFuhai = "mobile:fuhai", ShenSunCeFenghe = "mobile:fenghe";
    internal const string ShenSunCeGiftAction = "shen-sun-ce-gift", ShenSunCeGiftReason = "skill-program.mobile:fenghe.give";
    internal static CardConversionSource ShenSunCeSource(ProgramSkillFrame f) => new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    internal int Pingding(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.Pingding);
    SkillProgramStepOutcome global::CardGame.Core.IShenSunCeProgramHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenSunCe(f, op);
    bool global::CardGame.Core.IShenSunCeSelectionQueries.CanSubdue(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[seat].MaxHp > 1;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38100, SkillProgramEffectOp.ResolveShenSunCeFuhaiDraw, SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath, SkillProgramEffectOp.ResolveShenSunCeFenghe)]
    internal bool ShenSunCeTriggerLegal(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        var op = t.Effects.Single().Op;
        if (op == SkillProgramEffectOp.ResolveShenSunCeFuhaiDraw)
            return _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } use && use.Action.Type == CardActionType.Use && use.Action.ActorSeat == c.OwnerSeat && (use.Action.EffectiveKind == CardKind.BorrowedSword ? use.Action.TargetSeats.Where((_, index) => index % 2 == 0) : use.Action.EffectiveDesignatedTargetSeats).Any(s => Pingding(s) > 0) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenSunCeDrawReturnedEvent>().Where(e => e.Source.OwnerSeat == c.OwnerSeat && e.Operation == op && e.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber).Sum(e => e.Actual) < 2;
        if (op == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath)
            return w.TargetSeat is { } victim && Pingding(victim) > 0 && _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } kill && kill.VictimSeat == victim && _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(p => p.Id == kill.DeathFrameId)is { KillerProgramsResolved: false } death && death.VictimSeat == victim;
        return w is { SourceSeat: { } source, TargetSeat: { } target } && source != target && target == c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[target].HasHpValue && _coreMatchParticipantsPlayersAccess._players[target].MaxHp > 1 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > 0 && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { Prevented: false, Amount: > 0 } damage && !_coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(damage);
    }

    internal SkillProgramStepOutcome BeginShenSunCe(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenSunCe is not null || f.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Shen Sun Ce requires his original unpaid native program.");
        var subject = operation == SkillProgramEffectOp.ResolveShenSunCeYingba ? f.SelectedTargetSeats.Single() : operation == SkillProgramEffectOp.ResolveShenSunCeFenghe ? f.WindowContext!.SourceSeat!.Value : f.WindowContext!.TargetSeat ?? f.OwnerSeat;
        var amount = operation == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath ? Pingding(subject) : 1;
        var deathId = operation == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath ? _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single(w => w.Id == f.WindowContext!.ParentFrameId).DeathFrameId : 0;
        var r = new ShenSunCeReceipt(ShenSunCeSource(f), f.GameplayHash, operation, f.InstructionIndex, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressTurnNumberAccess._turnNumber, subject, amount, operation == SkillProgramEffectOp.ResolveShenSunCeYingba ? ShenSunCeStage.TargetChildren : ShenSunCeStage.MaximumChildren)
        {
            DeathId = deathId,
            SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenSunCeStartedEvent(f.Id, r.Source, r.GameplayHash, operation, r.ParentId, r.ActualTurn, subject, amount, deathId));
        if (operation == SkillProgramEffectOp.ResolveShenSunCeYingba)
        {
            if (subject == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[subject].IsAlive || !_coreMatchParticipantsPlayersAccess._players[subject].HasHpValue || _coreMatchParticipantsPlayersAccess._players[subject].MaxHp <= 1)
                throw new InvalidOperationException("Yingba requires another actual living role with maximum HP above one.");
            ShenSunCeChangeMaximum(f, subject, -1);
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, subject, PlayerMarkerKind.Pingding, 1);
        }
        else if (operation == SkillProgramEffectOp.ResolveShenSunCeFenghe)
        {
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            ShenSunCeChangeMaximum(f, f.OwnerSeat, -1);
        }
        else if (operation == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath)
            ShenSunCeChangeMaximum(f, f.OwnerSeat, amount);
        else
        {
            IssueShenSunCeDraw(f, 1);
            return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueShenSunCe(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ShenSunCeChangeMaximum(ProgramSkillFrame f, int seat, int delta)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = p.MaxHp;
        var hp = p.Hp;
        p.MaxHp = _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, 2020) ? 7 : Math.Max(0, checked(before + delta));
        p.Hp = Math.Min(p.Hp, p.MaxHp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, p.MaxHp - before, p.MaxHp, f.SkillId) { HpBeforeClamp = hp });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenSunCeMaximumPaidEvent(f.Id, seat, before, p.MaxHp, hp, p.Hp));
    }

    internal bool DrainShenSunCe(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueShenSunCe(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenSunCe: { } r } f && f.Id == id)
        {
            if (r.Stage == ShenSunCeStage.DrawAttempt)
                return;
            if (DrainShenSunCe(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenSunCe!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShenSunCe(f);
                return;
            }

            if (r.Stage == ShenSunCeStage.TargetChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = r with { Stage = ShenSunCeStage.OwnerChildren } });
                ShenSunCeChangeMaximum(f, f.OwnerSeat, -1);
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp == 0)
                {
                    _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
                    return;
                }

                continue;
            }

            if (r.Operation == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath && r.Stage == ShenSunCeStage.MaximumChildren)
            {
                IssueShenSunCeDraw(f, r.Amount);
                return;
            }

            if (r.Operation == SkillProgramEffectOp.ResolveShenSunCeFenghe && r.Stage == ShenSunCeStage.MaximumChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = r with { Stage = ShenSunCeStage.GiftChoice } });
                PublishShenSunCeGift(f);
                return;
            }

            if (r.Stage == ShenSunCeStage.GiftChoice)
            {
                PublishShenSunCeGift(f);
                return;
            }

            if (r.Stage == ShenSunCeStage.GiftChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = r with { Stage = ShenSunCeStage.MarkerChildren }, PendingMovementContinuation = null });
                if (_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], ShenSunCeYingba) && _coreMatchParticipantsPlayersAccess._players[r.SubjectSeat].IsAlive)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.SubjectSeat, PlayerMarkerKind.Pingding, 1);
                continue;
            }

            FinishShenSunCe(f);
            return;
        }
    }

    internal void FinishShenSunCe(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssueShenSunCeDraw(ProgramSkillFrame f, int count)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = f.ShenSunCe!with { Stage = ShenSunCeStage.DrawAttempt, RequestedDraw = count } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new($"skill-program.mobile:fuhai.{((f.ShenSunCe.Operation == SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath ? "death" : "target"))}-draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShenSunCeFuhaiDeath, SkillProgramEffectOp.ResolveShenSunCeFuhaiDraw)]
    internal void ReturnShenSunCeDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenSunCe!;
        if (r.Stage != ShenSunCeStage.DrawAttempt || r.DrawReturned || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.RequestedDraw || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Fuhai requires its exact typed native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = r with { Stage = ShenSunCeStage.DrawChildren, DrawReturned = true, SequenceBefore = outcome.SequenceBefore, SequenceAfter = outcome.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenSunCeDrawReturnedEvent(f.Id, r.Source, r.Operation, r.ActualTurn, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShenSunCe(f.Id);
    }

    internal IReadOnlyList<PromptChoice> ShenSunCeGiftChoices(ProgramSkillFrame f) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).SelectMany(card => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(target => new PromptChoice(new($"shen-sun-ce.{(f.Id)}.{(card.Id)}.{(target.Seat)}"), $"将【{(card.DisplayName)}】交给 {(target.Name)}", [card.Id], [target.Seat], new Dictionary<string, string> { ["program-action"] = ShenSunCeGiftAction }))).ToArray());
    internal void PublishShenSunCeGift(ProgramSkillFrame f)
    {
        var choices = ShenSunCeGiftChoices(f);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenSunCe = f.ShenSunCe!with { Stage = ShenSunCeStage.GiftChildren } });
            ContinueShenSunCe(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "冯河：将一张手牌交给一名其他角色。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ShenSunCeGiftAction)]
    internal void AnswerShenSunCeGift(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenSunCe: { Stage: ShenSunCeStage.GiftChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShenSunCeGiftChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("Fenghe requires its owner's exact current private hand gift and recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var card = choice.Cards.Single();
        var target = choice.Targets.Single();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ShenSunCe = r with { Stage = ShenSunCeStage.GiftChildren, CardId = card, RecipientSeat = target, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(target), new(ShenSunCeGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, target, [card], before, null));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(target) && m.Reason.Value == ShenSunCeGiftReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenSunCe = f.ShenSunCe!with { SequenceAfter = movement.Sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenSunCeGiftPaidEvent(f.Id, card, target, movement.Sequence));
        ContinueShenSunCe(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38100)]
    internal bool ResumeShenSunCe(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenSunCe: not null } f || f.Id != id)
            return false;
        ContinueShenSunCe(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38100)]
    internal bool ReturnShenSunCeMovement(ProgramSkillFrame f) => f.ShenSunCe is not null && ResumeShenSunCe(f.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38100)]
    internal bool KeepPaidShenSunCe(ProgramSkillFrame f) => f.ShenSunCe is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenSunCeFenghe)]
    internal PromptChoice AiShenSunCeGift(PendingDecision d, ProgramSkillFrame f) => d.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();

    internal bool ShenSunCeCannotRespond(int source, int responder, CardKind kind, IReadOnlyList<int> targets) => source != responder && (kind == CardKind.BorrowedSword ? targets.Where((_, index) => index % 2 == 0).Contains(responder) : targets.Contains(responder)) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreMatchParticipantsPlayersAccess._players[responder].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[source], ShenSunCeFuhai) && Pingding(responder) > 0;
    internal bool ShenSunCeDistance(CharacterState actor, CharacterState target) => actor.IsAlive && target.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(actor, ShenSunCeYingba) && Pingding(target.Seat) > 0;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 38100)]
    internal bool ShenSunCeCardDistance(ProgramCardDistanceQuery q) => ShenSunCeDistance(q.Actor, q.Target);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 38100)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 38100)]
    internal bool ShenSunCeSlashDistance(ProgramSlashDistanceQuery q) => ShenSunCeDistance(q.Actor, q.Target);
}
