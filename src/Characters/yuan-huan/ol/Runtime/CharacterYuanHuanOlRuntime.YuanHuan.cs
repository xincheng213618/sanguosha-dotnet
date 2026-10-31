#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanHuanOlRuntime
{
    internal const string DeruTakeReason = "skill-program.yuan-huan.deru.take", DeruFillReason = "skill-program.yuan-huan.deru.fill", DeruRecoverReason = "skill-program.yuan-huan.deru.recover", LinjieDiscardReason = "skill-program.yuan-huan.linjie.discard", LinjieDrawReason = "skill-program.yuan-huan.linjie.draw";
    internal static CardConversionSource YuanHuanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_yuan_huan_ol_YuanHuan.YuanHuanSource(f);
    SkillProgramStepOutcome global::CardGame.Core.IYuanHuanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginYuanHuan(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26920)]
    internal bool QualifyYuanHuan(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveLinjieSourceHandDiscard))
            return true;
        return context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0 } && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageWasApplied: true } attack && attack.SourceSeat == source && attack.TargetSeat == target && attack.DamageAmount == context.Amount && target == c.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[source]).Any(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(source, card, CardLocation.Hand(source), OwnedCardMoveIntent.Discard));
    }

    internal SkillProgramStepOutcome BeginYuanHuan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.YuanHuan is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0)
            throw new InvalidOperationException("YuanHuan requires a new exact zero-card initial instruction.");
        var deru = op == SkillProgramEffectOp.ResolveDeruBasicNames;
        int target;
        long parent = 0;
        long? damage = null;
        var amount = 0;
        if (deru)
        {
            var a = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
            if (a is null || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var seat] || seat == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, a.UsageGroup)) != 1)
                throw new InvalidOperationException("Deru lost its actual once-per-Play activation or other living target.");
            YuanHuanComposition.Activation(f.SkillId, a);
            target = seat;
        }
        else
        {
            if (f.TriggerId is null || f.WindowContext is not { SourceSeat: { } source } context || !QualifyYuanHuan(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
                throw new InvalidOperationException("Linjie requires its actual damage source with a discardable hand card.");
            target = source;
            parent = context.ParentFrameId;
            damage = context.DamageFrameId;
            amount = context.Amount;
        }

        var r = new YuanHuanReceipt(YuanHuanSource(f), f.GameplayHash, op, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, target, parent, damage, amount, deru ? YuanHuanStage.Guess : YuanHuanStage.SourceDiscard, Movements: []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanHuanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.PhaseInstance, target, parent, damage, amount));
        PublishYuanHuanChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> YuanHuanChoices(ProgramSkillFrame f)
    {
        var r = f.YuanHuan!;
        if (r.Stage == YuanHuanStage.Guess)
        {
            var names = new[]
            {
                "杀",
                "闪",
                "桃",
                "酒"
            };
            return Array.AsReadOnly(Enumerable.Range(0, 16).Select(mask => new PromptChoice(new($"deru.{(f.Id)}.guess.{(mask)}"), "猜测有：" + (mask == 0 ? "均没有" : string.Join("、", names.Where((_, index) => (mask & (1 << index)) != 0))), [], [], new Dictionary<string, string> { ["program-action"] = "yuan-huan", ["guess-mask"] = mask.ToString(CultureInfo.InvariantCulture) })).ToArray());
        }

        if (r.Stage == YuanHuanStage.SourceDiscard)
            return Array.AsReadOnly(LinjieCards(f).Select(c => new PromptChoice(new($"linjie.{(f.Id)}.discard.{(c.Cards.Single())}"), "临节：弃置【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).DisplayName + "】", c.Cards, [], new Dictionary<string, string> { ["program-action"] = "yuan-huan", ["discard"] = "true" })).ToArray());
        return[];
    }

    internal void PublishYuanHuanChoice(ProgramSkillFrame f)
    {
        var r = f.YuanHuan!;
        var choices = YuanHuanChoices(f);
        if (choices.Count == 0)
        {
            FinishYuanHuan(f);
            return;
        }

        var chooser = r.Stage == YuanHuanStage.Guess ? f.OwnerSeat : r.TargetSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == YuanHuanStage.Guess ? "德辱：分别猜测其手牌是否有杀、闪、桃、酒。未选的牌名表示猜测没有。" : "临节：弃置一张自己的手牌。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("yuan-huan")]
    internal void AnswerYuanHuan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YuanHuan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == YuanHuanStage.Guess ? f.OwnerSeat : r.TargetSeat) || !YuanHuanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("YuanHuan requires the exact chooser's published private guess or owned hand discard.");
        AssertYuanHuan(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == YuanHuanStage.Guess)
        {
            var guess = int.Parse(choice.Parameters["guess-mask"], CultureInfo.InvariantCulture);
            var actual = DeruActualNames(r.TargetSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = r with { GuessMask = guess, ActualMask = actual, Stage = YuanHuanStage.RecoveryChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeruGuessCommittedEvent(f.Id));
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            if ((guess ^ actual) != 15 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            RecordYuanHuanStep(f.Id, DeruRecoverReason, before, 0);
            ContinueYuanHuan(f.Id);
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
        var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = r with { Stage = YuanHuanStage.DiscardChildren, DiscardedKind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card), DiscardedPrintedKind = card.Kind }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, r.TargetSeat, card, CardLocation.Hand(r.TargetSeat), new(LinjieDiscardReason));
        RecordYuanHuanStep(f.Id, LinjieDiscardReason, sequence, 1, r.TargetSeat, card.Id, CardLocation.Hand(r.TargetSeat), CardLocation.DiscardPile);
        ContinueYuanHuan(f.Id);
    }

    internal void RecordYuanHuanStep(long id, string reason, long before, int actual, int? subject = null, int? card = null, CardLocation? from = null, CardLocation? to = null)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.YuanHuan!;
        var m = new YuanHuanMovement(r.Movements!.Count, subject ?? f.OwnerSeat, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual, card, from, to);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YuanHuan = r with { Movements = r.Movements.Append(m).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanHuanStepIssuedEvent(id, r.Stage, m.Index, m.SubjectSeat, reason, before, m.After, actual, card, from, to));
    }

    internal void IssueYuanHuanDraw(ProgramSkillFrame f, bool linjie)
    {
        var r = f.YuanHuan!;
        var count = linjie ? (r.DiscardedKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? 1 : 0) : r.GuessMask == r.ActualMask ? Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count) : 0;
        var reason = linjie ? LinjieDrawReason : DeruFillReason;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = r with { Stage = linjie ? YuanHuanStage.LinjieDrawChildren : YuanHuanStage.FillChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var actual = count > 0 ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], count, true, new(reason)).Count : 0;
        RecordYuanHuanStep(f.Id, reason, before, actual);
    }

    internal bool DrainYuanHuanChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueYuanHuan(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { YuanHuan: { } r } f && f.Id == id)
        {
            AssertYuanHuan(f);
            if (r.Stage is YuanHuanStage.Guess or YuanHuanStage.SourceDiscard)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishYuanHuanChoice(f);
                return;
            }

            if (DrainYuanHuanChildren(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishYuanHuan(f);
                return;
            }

            if (r.Stage == YuanHuanStage.RecoveryChildren)
                IssueDeruTake(f);
            else if (r.Stage is YuanHuanStage.TakeChildren or YuanHuanStage.DiscardChildren)
                IssueYuanHuanDraw(f, r.Stage == YuanHuanStage.DiscardChildren);
            else
            {
                FinishYuanHuan(f);
                return;
            }
        }
    }

    internal void FinishYuanHuan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanHuanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanHuan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26920)]
    internal bool ResumeYuanHuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YuanHuan: not null } f || f.Id != id)
            return false;
        ContinueYuanHuan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26920)]
    internal bool ReturnYuanHuanMovement(ProgramSkillFrame f)
    {
        if (f.YuanHuan is null)
            return false;
        ContinueYuanHuan(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveDeruBasicNames, SkillProgramEffectOp.ResolveLinjieSourceHandDiscard)]
    internal PromptChoice ChooseYuanHuan(PendingDecision p, ProgramSkillFrame f) => f.YuanHuan!.Stage == YuanHuanStage.Guess ? p.Choices.Single(c => c.Parameters["guess-mask"] == (_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat).Players[f.YuanHuan.TargetSeat].HandCount == 0 ? "0" : "15")) : p.Choices.OrderBy(c => c.Cards.Single()).First();
}
