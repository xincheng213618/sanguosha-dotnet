#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuFuAnniversaryRuntime
{
    internal static CardConversionSource WuFuSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason WuFuDrawReason => new("skill-program.anniversary:zhonge.draw");
    internal static CardMoveReason WuFuDiscardReason => new("skill-program.anniversary:zhonge.adjust");

    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryWuFuHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWuFu(f, op);
    internal SkillProgramStepOutcome BeginWuFu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Wu Fu requires his original current actual-Play activation.");
        if (op == SkillProgramEffectOp.ResolveAnniversaryWuFuJuekai)
        {
            if (f.SkillId != AnniversaryWuFuComposition.Juekai || f.ActivationId != "reset-and-restrict" || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuJuekaiEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Juekai cannot repeat its accepted limited activation.");
            var program = _contentRegistryOperations._contentRegistry.GetSkill(AnniversaryWuFuComposition.Zhonge).Program ?? throw new InvalidOperationException("Juekai lost its paired Zhonge definition.");
            var activation = program.Activations.Single(a => a.Id == "adjust-and-commit");
            var key = (f.OwnerSeat, AnniversaryWuFuComposition.Zhonge, activation.UsageGroup);
            var before = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key);
            if (before > 0)
                _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = 0;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuJuekaiEvent(f.Id, WuFuSource(f), f.GameplayHash, phase, before));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.SkillId != AnniversaryWuFuComposition.Zhonge || f.ActivationId != "adjust-and-commit" || f.AnniversaryWuFu is not null)
            throw new InvalidOperationException("Zhonge requires a fresh adjustment and simultaneous commitment.");
        var difference = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp) - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var count = difference < 0 ? Math.Min(-difference, _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).Count) : difference;
        var r = new AnniversaryWuFuReceipt(WuFuSource(f), f.GameplayHash, phase)
        {
            Count = count,
            Stage = difference > 0 ? AnniversaryWuFuStage.AdjustDraw : count > 0 ? AnniversaryWuFuStage.AdjustDiscard : AnniversaryWuFuStage.Target
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuStartedEvent(f.Id, r.Source, r.Hash, r.Phase));
        if (difference > 0)
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, WuFuDrawReason);
        else
            ContinueWuFu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal AnniversaryWuFuTask[] WuFuTasks(ProgramSkillFrame f)
    {
        var r = f.AnniversaryWuFu!;
        if (r.Votes.Count != 2)
            return[];
        var different = r.Votes[0].Draw != r.Votes[1].Draw;
        var result = new List<AnniversaryWuFuTask>();
        foreach (var draw in new[]
        {
            true,
            false
        }

        )
        {
            if (!r.Votes.Any(v => v.Draw == draw))
                continue;
            result.Add(new(f.OwnerSeat, r.Peer, draw));
            if (different)
                result.Add(new(f.OwnerSeat, r.Peer, draw));
            result.Add(new(r.Peer, f.OwnerSeat, draw));
        }

        return result.ToArray();
    }

    internal AnniversaryWuFuTask WuFuTask(ProgramSkillFrame f) => WuFuTasks(f)[f.AnniversaryWuFu!.Cursor];
    internal int WuFuChooser(ProgramSkillFrame f) => f.AnniversaryWuFu is { Stage: AnniversaryWuFuStage.Commit, Votes.Count: 1 } r ? r.Peer : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> WuFuChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryWuFu!;
        PromptChoice C(string branch, string text, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"anniversary-wu-fu.{(f.Id)}.{(r.Stage)}.{(r.Cards.Count)}.{(r.Votes.Count)}.{(branch)}"), text, cards, targets, new Dictionary<string, string> { ["program-action"] = "anniversary-wu-fu", ["branch"] = branch });
        if (r.Stage == AnniversaryWuFuStage.Target)
            return Array.AsReadOnly(_coreTableOrderOperations.CurrentTableOrder().Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Select(s => C("peer-" + s, _coreMatchParticipantsPlayersAccess._players[s].Name, [], [s])).ToArray());
        if (r.Stage == AnniversaryWuFuStage.Commit)
            return[C("draw", "依次摸一张牌", [], []), C("slash", "依次视为对对方使用杀", [], [])];
        if (r.Stage != AnniversaryWuFuStage.AdjustDiscard)
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !r.Cards.Contains(c.Id)).Select(c => C("card-" + c.Cards.Single(), c.Label, c.Cards, [])).ToArray());
    }

    internal void PublishWuFu(ProgramSkillFrame f)
    {
        var choices = WuFuChoices(f);
        var chooser = WuFuChooser(f);
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || choices.Count == 0)
        {
            FinishWuFu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, f.AnniversaryWuFu!.Stage switch
        {
            AnniversaryWuFuStage.AdjustDiscard => "忠锷：弃置超出体力上限的手牌。",
            AnniversaryWuFuStage.Target => "忠锷：选择一名其他角色。",
            _ => "忠锷：双方密选，均提交后公开并依次执行。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
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

    [ProgramAnswer("anniversary-wu-fu")]
    internal void AnswerWuFu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryWuFu: { } r } f || !ValidWuFu(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != WuFuChooser(f) || !WuFuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zhonge rejects a changed private choice or another participant's slot.");
        var chooser = WuFuChooser(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == AnniversaryWuFuStage.Target)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r with { Peer = choice.Targets.Single(), Stage = AnniversaryWuFuStage.Commit } });
            PublishWuFu(f);
            return;
        }

        if (r.Stage == AnniversaryWuFuStage.Commit)
        {
            var votes = r.Votes.Append(new AnniversaryWuFuVote(chooser, choice.Parameters["branch"] == "draw")).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r with { Votes = votes } });
            if (votes.Length == 1)
            {
                PublishWuFu(f);
                return;
            }

            for (var i = 0; i < votes.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuVoteRevealedEvent(f.Id, i, votes[i].Seat, votes[i].Draw));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryWuFu = f.AnniversaryWuFu!with { Stage = AnniversaryWuFuStage.Item } });
            ContinueWuFu(f.Id);
            return;
        }

        var ids = r.Cards.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r with { Cards = ids } });
        if (ids.Length < r.Count)
        {
            PublishWuFu(f);
            return;
        }

        if (ids.Length != r.Count || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(f.OwnerSeat), OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Zhonge must discard the complete original currently legal excess Hand payment.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = f.AnniversaryWuFu!with { Stage = AnniversaryWuFuStage.AdjustChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, WuFuDiscardReason, (_, _) =>
        {
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(live with { AnniversaryWuFu = live.AnniversaryWuFu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuAdjustedEvent(f.Id, ids.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, false));
        });
        ContinueWuFu(f.Id);
    }

    internal bool DrainWuFu(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueWuFu(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryWuFu: { } r } f && f.Id == id)
        {
            if (!ValidWuFu(f))
                throw new InvalidOperationException("Zhonge lost its original phase, commitments or native return.");
            if (r.Stage is AnniversaryWuFuStage.AdjustDraw or AnniversaryWuFuStage.Drawing or AnniversaryWuFuStage.Issuing or AnniversaryWuFuStage.NativeUse)
                return;
            if (r.Stage is AnniversaryWuFuStage.AdjustChildren or AnniversaryWuFuStage.Children or AnniversaryWuFuStage.NativeChildren)
            {
                if (DrainWuFu(f))
                    return;
                if (r.Stage == AnniversaryWuFuStage.AdjustChildren)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.Target, Draw = null } });
                    continue;
                }

                CompleteWuFuItem(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), r.UseId > 0);
                return;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishWuFu(f);
                return;
            }

            if (r.Stage == AnniversaryWuFuStage.Item)
            {
                if (r.Cursor >= WuFuTasks(f).Length)
                {
                    FinishWuFu(f);
                    return;
                }

                var item = WuFuTask(f);
                if (!_coreMatchParticipantsPlayersAccess._players[item.Actor].IsAlive)
                {
                    CompleteWuFuItem(f, false);
                    return;
                }

                if (!item.Draw)
                {
                    IssueWuFuSlash(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.Drawing } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, item.Actor, 1, WuFuDrawReason);
                return;
            }

            PublishWuFu(f);
            return;
        }
    }

    internal void CompleteWuFuItem(ProgramSkillFrame f, bool used)
    {
        var r = f.AnniversaryWuFu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuItemReturnedEvent(f.Id, r.Cursor, used));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, AnniversaryWuFu = r with { Stage = AnniversaryWuFuStage.Item, Cursor = r.Cursor + 1, Before = 0, After = 0, UseId = 0, ActionId = 0, Returned = false, Draw = null, NativeBatches = [] } });
        ContinueWuFu(f.Id);
    }

    internal void FinishWuFu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryWuFuZhonge)]
    internal void ReturnWuFuDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        var r = f.AnniversaryWuFu ?? throw new InvalidOperationException("Zhonge draw lost its producer.");
        var adjustment = r.Stage == AnniversaryWuFuStage.AdjustDraw;
        if (r.Stage is not (AnniversaryWuFuStage.AdjustDraw or AnniversaryWuFuStage.Drawing) || o.RecipientSeat != (adjustment ? f.OwnerSeat : WuFuTask(f).Actor) || o.RequestedCount != (adjustment ? r.Count : 1) || o.SequenceAfter < o.SequenceBefore || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Draw is not null)
            throw new InvalidOperationException("Zhonge lost the exact requested native draw or its original recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryWuFu = r with { Stage = adjustment ? AnniversaryWuFuStage.AdjustChildren : AnniversaryWuFuStage.Children, Draw = o, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (adjustment)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuAdjustedEvent(f.Id, r.Count, o.SequenceBefore, o.SequenceAfter, true));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuDrawReturnedEvent(f.Id, r.Cursor, o.RecipientSeat, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.AttemptFrameId));
        ContinueWuFu(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46600)]
    internal bool ResumeWuFu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryWuFu: not null } f || f.Id != id)
            return false;
        ContinueWuFu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46600)]
    internal bool ReturnWuFuMovement(ProgramSkillFrame f)
    {
        if (f.AnniversaryWuFu is null)
            return false;
        ContinueWuFu(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryWuFuZhonge)]
    internal PromptChoice ChooseWuFu(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "draw") ?? p.Choices.First();
}
