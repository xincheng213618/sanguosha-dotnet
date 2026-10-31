#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    internal const string ZhaohuoAction = "zhaohuo";
    internal const string ZhaohuoGiftReason = "skill-program.tao-qian.zhaohuo.give";
    internal bool HasZhaohuoCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhaohuoFirstVictimGift) == true;

    internal static bool IsZhaohuoTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tao_qian_ol_Zhaohuo.IsZhaohuoTrigger(trigger);
    internal bool ZhaohuoActualTurnOpen => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal static CardConversionSource ZhaohuoSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tao_qian_ol_Zhaohuo.ZhaohuoSource(f);
    // DamageApplied precedes before-dying/recovery and other after-damage
    // children. Capture once here, including empty-Hand victims and turns in
    // which the current turn owner does not yet own Zhaohuo.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28121)]
    internal void CaptureZhaohuoFirstVictim(IGameEvent payload)
    {
        if (!HasZhaohuoCapability || !ZhaohuoActualTurnOpen || payload is not DamageAppliedEvent { Amount: > 0 } applied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat || attack.DamageAmount != applied.Amount || attack.IsSourceLess != applied.SourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(applied.TargetSeat))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoFirstVictimDamageEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.VictimSeat == applied.TargetSeat))
            return;
        var first = new ZhaohuoFirstVictimDamageEvent(damage.Id, attack.ResolutionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, applied.SourceLess ? null : applied.SourceSeat, applied.TargetSeat, applied.Amount, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[applied.TargetSeat]).Count);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(first);
        var owner = _coreMatchParticipantsPlayersAccess._players[first.TurnOwner];
        if (!owner.IsAlive || first.HandCount == 0)
            return;
        var candidate = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(i => i.Program.Triggers.Where(IsZhaohuoTrigger).Select(t => new ProgramTriggerCandidate(owner.Seat, i.SkillId, t.Id, i.SkillInstanceId, i.Program.GameplayHash, t.Priority))).Where(c => _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, c.SkillId, c.SkillInstanceId)).OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        if (candidate is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoQualifiedEvent(first.DamageFrameId, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, first.ActualTurn, first.TurnOwner));
    }

    internal ZhaohuoFirstVictimDamageEvent? ZhaohuoFirst(long damageId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoFirstVictimDamageEvent>().SingleOrDefault(e => e.DamageFrameId == damageId);
    internal bool ZhaohuoQualified(ProgramTriggerCandidate c, ZhaohuoFirstVictimDamageEvent first) => first.Amount > 0 && first.HandCount > 0 && c.OccurrenceIndex == 0 && c.OwnerSeat == first.TurnOwner && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(c.SkillId)?.Program is { } p && p.GameplayHash == c.GameplayHash && p.Triggers.Any(t => t.Id == c.BindingId && IsZhaohuoTrigger(t)) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoQualifiedEvent>().Count(e => e.DamageFrameId == first.DamageFrameId && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.GameplayHash == c.GameplayHash && e.ActualTurn == first.ActualTurn && e.TurnOwner == first.TurnOwner) == 1;
    internal void AppendZhaohuoDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> candidates)
    {
        candidates.RemoveAll(c => !string.IsNullOrEmpty(c.ProgramId) && IsZhaohuoTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c.ToProgramCandidate())));
        if (!HasZhaohuoCapability || window != SkillProgramTriggerWindow.AfterDamageApplied || !ZhaohuoActualTurnOpen || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]).Count == 0)
            return;
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.ParentFrameId == attack.ResolutionId && d.SourceSeat == attack.SourceSeat && d.TargetSeat == attack.TargetSeat && d.Amount == attack.DamageAmount);
        if (damage is null || ZhaohuoFirst(damage.Id)is not { } first || first.HandCount == 0 || first.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || first.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return;
        foreach (var q in _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoQualifiedEvent>().Where(q => q.DamageFrameId == damage.Id))
        {
            var c = new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, 0);
            if (!ZhaohuoQualified(c, first))
                continue;
            var t = _coreProgramLifecycleOperations.GetProgramTrigger2(c);
            candidates.Add(new(c.OwnerSeat, $"program:{(c.SkillId)}:{(c.BindingId)}:{(c.SkillInstanceId)}:occurrence-0", t.Priority, false, ProgramId: c.SkillId, ProgramTriggerId: c.BindingId, SkillInstanceId: c.SkillInstanceId, GameplayHash: c.GameplayHash));
        }
    }

    internal ProgramSkillWindowContext CreateZhaohuoDamageContext(DamageTriggerWindowFrame window, ProgramTriggerCandidate candidate)
    {
        var first = ZhaohuoFirst(window.ParentFrameId) ?? throw new InvalidOperationException("Zhaohuo requires the actual producer-frozen damage occurrence.");
        if (!ZhaohuoQualified(candidate, first) || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || first.VictimSeat != window.TargetSeat || first.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Zhaohuo cannot replace its original damaged donor.");
        return new(SkillProgramTriggerWindow.AfterDamageApplied, window.Id, candidate.OwnerSeat, SourceSeat: first.DamageSourceSeat, TargetSeat: first.VictimSeat, DamageFrameId: first.DamageFrameId, Amount: first.Amount, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { EventTargetDamageInstancesTakenThisTurn = 1, EventTargetHandCount = first.HandCount });
    }

    [ProgramIssuedTrigger(250)]
    internal bool IsIssuedZhaohuoCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!HasZhaohuoCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !IsZhaohuoTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) || !ZhaohuoActualTurnOpen || context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.OwnerSeat != candidate.OwnerSeat || context.DamageFrameId is not { } damageId || ZhaohuoFirst(damageId)is not { } first || !ZhaohuoQualified(candidate, first) || first.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || first.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || context.SourceSeat != first.DamageSourceSeat || context.TargetSeat != first.VictimSeat || context.Amount != first.Amount || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[first.VictimSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[first.VictimSeat]).Count == 0 || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not DamageTriggerWindowFrame w || w.TriggerWindow != context.Window || w.ParentFrameId != damageId || w.TargetSeat != first.VictimSeat || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex].ToProgramCandidate() != candidate || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is not DamageFrame damage || damage.ParentFrameId != first.AttackId || damage.TargetSeat != first.VictimSeat || damage.Amount != first.Amount)
            return false;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhaohuoStartedEvent>().Any(e => e.DamageFrameId == damageId && e.Source.OwnerSeat == candidate.OwnerSeat);
    }

    SkillProgramStepOutcome global::CardGame.Core.IZhaohuoHost.Resolve(ProgramSkillFrame frame) => ResolveZhaohuoFirstVictimGift(frame);
    internal SkillProgramStepOutcome ResolveZhaohuoFirstVictimGift(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Zhaohuo is not null || f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { } c)
            throw new InvalidOperationException("Zhaohuo requires its original unexecuted native damage binding.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!IsIssuedZhaohuoCandidate(candidate, c))
            throw new InvalidOperationException("Zhaohuo lost its first positive damage and original Hand donor.");
        var first = ZhaohuoFirst(c.DamageFrameId!.Value)!;
        var r = new ZhaohuoReceipt(ZhaohuoSource(f), f.GameplayHash, first.DamageFrameId, c.ParentFrameId, first.AttackId, first.VictimSeat, first.ActualTurn, first.TurnOwner, ZhaohuoStage.Choosing);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhaohuo = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoStartedEvent(f.Id, r.DamageFrameId, r.DamageWindowId, r.Source, r.GameplayHash, r.VictimSeat, r.ActualTurn, r.TurnOwner));
        PublishZhaohuo(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ZhaohuoChoices(ProgramSkillFrame f)
    {
        var r = f.Zhaohuo!;
        if (r.Stage != ZhaohuoStage.Choosing || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive)
            return[];
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.VictimSeat]).Select(card => PromptChoiceProjection.Freeze(new PromptChoice(new($"zhaohuo.{(f.Id)}.give.{(card.Id)}"), "交给" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name + "：" + card.DisplayName, [card.Id], [], new Dictionary<string, string> { ["program-action"] = ZhaohuoAction, [ZhaohuoAction] = "give", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) }))).ToArray());
    }

    internal void PublishZhaohuo(ProgramSkillFrame f)
    {
        var choices = ZhaohuoChoices(f);
        if (choices.Count == 0)
        {
            FinishZhaohuo(f);
            return;
        }

        var r = f.Zhaohuo!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.VictimSeat, "招祸：选择一张手牌交给" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name + "。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            IsPrivate = true,
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("zhaohuo")]
    internal void AnswerZhaohuo(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zhaohuo: { Stage: ZhaohuoStage.Choosing } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.VictimSeat || !ZhaohuoChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zhaohuo requires the damaged donor's exact private original Hand choice.");
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.VictimSeat]).Single(c => c.Id == choice.Cards.Single());
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        r = r with
        {
            Paid = true,
            Stage = ZhaohuoStage.GiftChildren,
            CardId = card.Id,
            PrintedKind = card.Kind,
            SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            SelfGift = r.VictimSeat == f.OwnerSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, Zhaohuo = r, PendingMovementContinuation = r.SelfGift ? null : new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoGiftCommittedEvent(f.Id, r.DamageFrameId, r.Source, r.GameplayHash, r.VictimSeat, card.Id, card.Kind, r.ActualTurn, r.TurnOwner));
        if (r.SelfGift)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoSelfGiftCommittedEvent(f.Id, r.DamageFrameId, r.Source, r.GameplayHash, card.Id, card.Kind, r.ActualTurn, r.TurnOwner, r.SequenceBefore, r.SequenceBefore));
            ContinueZhaohuo(f.Id);
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(r.VictimSeat), CardLocation.Hand(f.OwnerSeat), new(ZhaohuoGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, r.VictimSeat, f.OwnerSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, movement => RecordZhaohuoGift(f.Id, movement)));
        ContinueZhaohuo(f.Id);
    }

    internal void RecordZhaohuoGift(long frameId, CardMovementRecord movement)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = f.Zhaohuo!;
        if (!r.Paid || r.SelfGift || r.Stage != ZhaohuoStage.GiftChildren || r.PaymentSequence is not null || movement.CardId != r.CardId || movement.CardKind != r.PrintedKind || movement.From != CardLocation.Hand(r.VictimSeat) || movement.To != CardLocation.Hand(f.OwnerSeat) || movement.Reason.Value != ZhaohuoGiftReason || movement.Sequence <= r.SequenceBefore)
            throw new InvalidOperationException("Zhaohuo lost its one original donor Hand payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Zhaohuo = r with { PaymentSequence = movement.Sequence, SequenceAfter = movement.Sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoGiftPaidEvent(f.Id, r.DamageFrameId, r.Source, r.GameplayHash, r.VictimSeat, movement.CardId, movement.CardKind, r.ActualTurn, r.TurnOwner, movement.From, movement.To, movement.Sequence));
    }

    internal bool DrainZhaohuo(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueZhaohuo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zhaohuo: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == ZhaohuoStage.Choosing)
        {
            PublishZhaohuo(f);
            return;
        }

        if (DrainZhaohuo(id))
            return;
        FinishZhaohuo(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishZhaohuo(ProgramSkillFrame f)
    {
        var r = f.Zhaohuo!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaohuoCompletedEvent(f.Id, r.DamageFrameId, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhaohuo = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28121)]
    internal bool ResumeZhaohuo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zhaohuo: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueZhaohuo(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28121)]
    internal bool ReturnZhaohuoMovement(ProgramSkillFrame f)
    {
        if (f.Zhaohuo is null)
            return false;
        ContinueZhaohuo(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveZhaohuoFirstVictimGift)]
    internal PromptChoice ChooseZhaohuoAi(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.First();
}
