#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    internal const string TaoQianAction = "tao-qian";
    internal const string ZongluanDiscardReason = "skill-program.tao-qian.zongluan.discard";
    internal const string WenrenDrawReason = "skill-program.tao-qian.wenren.draw";
    internal bool HasTaoQianCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZongluanPreparationSlashAndDiscard) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhaohuoFirstVictimGift) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWenrenSequentialDraw);

    internal static CardConversionSource TaoQianSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_tao_qian_ol_TaoQian.TaoQianSource(f);
    SkillProgramStepOutcome ITaoQianHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginTaoQian(f, op);
    internal SkillProgramStepOutcome BeginTaoQian(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TaoQian is not null)
        {
            ContinueTaoQian(f.Id);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var r = new TaoQianReceipt(TaoQianSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, op == SkillProgramEffectOp.ResolveZongluanPreparationSlashAndDiscard ? TaoQianStage.ActorChoice : TaoQianStage.RecipientChoice);
        if (op == SkillProgramEffectOp.ResolveWenrenSequentialDraw)
        {
            if (f.TriggerId is not null || f.ActivationId != "distribute-draw" || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } p || p.ActorSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<WenrenCommittedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == p))
                throw new InvalidOperationException("Wenren requires an unspent real own Play-phase draft.");
            r = r with
            {
                Phase = p
            };
        }
        else if (f.TriggerId != "preparation-slash" || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, OwnerSeat: var owner } || owner != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Zongluan requires its real own Preparation window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TaoQianStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner));
        PublishTaoQian(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int TaoQianChooser(ProgramSkillFrame f) => f.TaoQian!.Stage == TaoQianStage.SlashTargets ? f.TaoQian.ActorSeat : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> TaoQianChoices(ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"tao-qian.{(f.Id)}.{(r.Stage)}.{((branch.EndsWith("confirm", StringComparison.Ordinal) ? "0" : "1"))}.{(branch)}.{(string.Join('-', cards ?? []))}.{(string.Join('-', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = TaoQianAction, [TaoQianAction] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == TaoQianStage.ActorChoice)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                choices.Add(Choice("actor", "令" + p.Name + "使用纵乱杀", targets: [p.Seat]));
        if (r.Stage is TaoQianStage.SlashTargets or TaoQianStage.RecipientChoice)
        {
            var seats = r.Stage == TaoQianStage.SlashTargets ? ZongluanTargets(r.ActorSeat) : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
            foreach (var seat in seats)
            {
                var after = r.Targets.Contains(seat) ? r.Targets.Where(s => s != seat).ToArray() : r.Targets.Append(seat).ToArray();
                choices.Add(Choice(r.Stage == TaoQianStage.SlashTargets ? "slash-toggle" : "recipient-toggle", (r.Targets.Contains(seat) ? "取消选择" : "选择") + _coreMatchParticipantsPlayersAccess._players[seat].Name, targets: after));
            }

            if (r.Targets.Count > 0 && r.Targets.All(seats.Contains))
                choices.Add(Choice(r.Stage == TaoQianStage.SlashTargets ? "slash-confirm" : "recipient-confirm", "确认所选角色", targets: r.Targets));
        }

        if (r.Stage == TaoQianStage.DiscardChoice)
        {
            if (r.Selected.Count < r.DiscardRequired)
                foreach (var c in ZongluanDiscardCards(f))
                    choices.Add(Choice("discard-card", c.Label, c.Cards));
            if (r.Selected.Count == r.DiscardRequired)
                choices.Add(Choice("discard-confirm", "确认弃置", cards: r.Selected));
        }

        if (!r.Paid)
            choices.Add(Choice("cancel", "取消发动"));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishTaoQian(ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        if (!r.Paid && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)))
        {
            FinishTaoQian(f);
            return;
        }

        if (r.Stage == TaoQianStage.SlashTargets && (!_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive || ZongluanTargets(r.ActorSeat).Length == 0))
        {
            FinishTaoQian(f);
            return;
        }

        var choices = TaoQianChoices(f);
        if (choices.Count == 0)
        {
            FinishTaoQian(f);
            return;
        }

        var chooser = TaoQianChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage switch
        {
            TaoQianStage.ActorChoice => "纵乱：选择使用杀的角色。",
            TaoQianStage.SlashTargets => "纵乱：选择至少一名当前合法目标，然后确认。",
            TaoQianStage.DiscardChoice => $"纵乱：弃置{(r.DiscardRequired)}张牌。",
            _ => "温仁：选择至少一名角色，然后确认。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            IsPrivate = true,
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("tao-qian")]
    internal void AnswerTaoQian(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TaoQian: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != TaoQianChooser(f) || !TaoQianChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Tao Qian requires the actual chooser's exact published private input.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters[TaoQianAction])
        {
            case "cancel":
                if (r.Paid)
                    throw new InvalidOperationException("An issued Tao Qian action cannot be cancelled.");
                FinishTaoQian(f);
                return;
            case "actor":
                var actor = choice.Targets.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Paid = true, ActorSeat = actor, Stage = TaoQianStage.SlashTargets } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanCommittedEvent(f.Id, r.Source, r.GameplayHash, actor, r.ActualTurn));
                PublishTaoQian(f);
                return;
            case "slash-toggle":
            case "recipient-toggle":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Targets = choice.Targets } });
                PublishTaoQian(f);
                return;
            case "slash-confirm":
                IssueZongluanSlash(f);
                return;
            case "recipient-confirm":
                if (r.Paid || r.Targets.Count == 0 || r.Phase is not { } phase || _coreProgramEventHistoryOperations.ProgramEventHistory<WenrenCommittedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == phase))
                    throw new InvalidOperationException("Wenren repeated its confirmed actual Play queue.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Paid = true, Stage = TaoQianStage.Drawing, Cursor = 0 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenrenCommittedEvent(f.Id, r.Source, r.GameplayHash, phase, r.Targets.Count));
                for (var i = 0; i < r.Targets.Count; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenrenRecipientFrozenEvent(f.Id, i, r.Targets[i]));
                ContinueTaoQian(f.Id);
                return;
            case "discard-card":
                if (r.Selected.Count >= r.DiscardRequired)
                    throw new InvalidOperationException("Zongluan cannot select more than its actual remaining discard obligation.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Selected = r.Selected.Append(choice.Cards.Single()).ToArray() } });
                PublishTaoQian(f);
                return;
            case "discard-confirm":
                PayZongluanDiscard(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Tao Qian choice.");
        }
    }

    internal bool DrainTaoQian(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueTaoQian(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TaoQian: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (f.NativeDraw is not null || r.Stage == TaoQianStage.NativeSlash)
                return;
            if (r.Stage is TaoQianStage.NativeChildren or TaoQianStage.DiscardChildren or TaoQianStage.DrawChildren)
            {
                if (DrainTaoQian(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.TaoQian!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishTaoQian(f);
                return;
            }

            if (r.Stage == TaoQianStage.DiscardChildren)
            {
                FinishTaoQian(f);
                return;
            }

            if (r.Stage == TaoQianStage.NativeChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishTaoQian(f);
                    return;
                }

                var required = Math.Min(r.DamageRecipientCount, _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).SelectMany(c => c.Cards).Distinct().Count());
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanDiscardIssuedEvent(f.Id, r.DamageRecipientCount, required, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                if (required == 0)
                {
                    FinishTaoQian(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Stage = TaoQianStage.DiscardChoice, DiscardRequired = required, Selected = [] } });
                PublishTaoQian(f);
                return;
            }

            if (r.Stage == TaoQianStage.DrawChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = r with { Stage = TaoQianStage.Drawing, Cursor = r.Cursor + 1, DrawOutcome = null } });
                continue;
            }

            if (r.Stage == TaoQianStage.Drawing)
            {
                if (r.Cursor >= r.Targets.Count)
                {
                    FinishTaoQian(f);
                    return;
                }

                var seat = r.Targets[r.Cursor];
                if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = r with { Cursor = r.Cursor + 1 } });
                    continue;
                }

                var ownHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
                var amount = (hand == 0 ? 1 : 0) + (hand <= ownHand ? 1 : 0);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenrenRecipientEvaluatedEvent(f.Id, r.Cursor, seat, ownHand, hand, amount));
                if (amount == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = r with { Cursor = r.Cursor + 1 } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { DrawAmount = amount, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, amount, new(WenrenDrawReason));
                return;
            }

            PublishTaoQian(f);
            return;
        }
    }

    internal void FinishTaoQian(ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TaoQianCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28120)]
    internal bool ResumeTaoQian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TaoQian: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueTaoQian(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28120)]
    internal bool ReturnTaoQianMovement(ProgramSkillFrame f)
    {
        if (f.TaoQian is null)
            return false;
        ContinueTaoQian(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveZongluanPreparationSlashAndDiscard, SkillProgramEffectOp.ResolveWenrenSequentialDraw)]
    internal PromptChoice ChooseTaoQianAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
        if (r.Stage == TaoQianStage.ActorChoice)
        {
            var support = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
            var harm = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
            var actors = p.Choices.Where(c => c.Targets is [var actor] && ZongluanTargets(actor).Length > 0 && (actor == f.OwnerSeat || _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, actor, support) > 0)).ToArray();
            if (actors.Length == 0)
                return p.Choices.Single(c => c.Parameters[TaoQianAction] == "cancel");
            return actors.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, c.Targets[0], support) + ZongluanTargets(c.Targets[0]).Select(t => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, t, harm)).Where(v => v > 0).Sum()).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        }

        if (r.Stage is TaoQianStage.SlashTargets or TaoQianStage.RecipientChoice)
        {
            var hint = r.Stage == TaoQianStage.SlashTargets ? new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false) : new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
            return p.Choices.Where(c => r.Stage != TaoQianStage.SlashTargets || c.Targets.Count > 0).OrderByDescending(c => c.Targets.Sum(s => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, s, hint))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        }

        return p.Choices.First();
    }
}
