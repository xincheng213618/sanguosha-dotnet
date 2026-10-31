#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiLieAnniversaryRuntime
{
    internal const string CuiLieAction = "anniversary-cui-lie", CuiLieGive = "skill-program.anniversary:zijue.give", CuiLieDraw = "skill-program.anniversary:zijue.draw", CuiLieClaim = "skill-program.anniversary:chibi.claim", CuiLieContest = "anniversary-cui-lie-contest";
    internal static CardConversionSource CuiLieSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IAnniversaryCuiLieHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCuiLie(f, op);
    internal AnniversaryCuiLieLeaseEvent[] CuiLieLeases(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryCuiLieLeaseEvent>().Where(l => l.Source.OwnerSeat == owner && l.Amount is >= 2 and <= 4 && l.Source.SkillId == AnniversaryCuiLieComposition.Zijue && l.Source.BindingId == "declare-number" && _contentRegistryOperations._contentRegistry.GetSkill(l.Source.SkillId).Program?.GameplayHash == l.Hash && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Any(t => t.ActorSeat == owner && t.TurnNumber > l.Turn) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryCuiLieGiftEvent>().Count(g => g.FrameId == l.FrameId && g.Count == l.Amount) == 1).ToArray();
    internal int AnniversaryCuiLiePindianBonus(int owner) => CuiLieLeases(owner).Sum(l => l.Amount);
    [ProgramDistanceRule(ProgramDistanceStage.IncomingContributions, 46900)]
    internal IEnumerable<RuleQueryContribution> CuiLieIncoming(int target)
    {
        foreach (var lease in CuiLieLeases(target))
            yield return new FiniteRuleQueryContribution($"issued:{(lease.FrameId)}:anniversary:zijue:incoming", SkillRuleOperation.Add, lease.Amount);
    }

    internal IEnumerable<ProgramTriggerCandidate> AnniversaryCuiLieIssuedPindianCandidates(IReadOnlyList<PindianRevealedCard> cards) => cards.Select(c => c.Seat).Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).SelectMany(s => CuiLieLeases(s).DistinctBy(l => l.Source.SkillInstanceId).Select(l => new ProgramTriggerCandidate(s, AnniversaryCuiLieComposition.Zijue, "paid-pindian-draw", l.Source.SkillInstanceId, l.Hash, 0)));
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 46900)]
    internal bool IsIssuedCuiLieDraw(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == AnniversaryCuiLieComposition.Zijue && c.BindingId == "paid-pindian-draw" && w.Window == SkillProgramTriggerWindow.PindianCardsRevealed && _resolutionStackOperations._resolutionStack.OfType<PindianRevealWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } p && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && p.Candidates[p.CandidateIndex] == c && p.Cards.Any(card => card.Seat == c.OwnerSeat) && CuiLieLeases(c.OwnerSeat).Any(l => l.Source.SkillInstanceId == c.SkillInstanceId && l.Hash == c.GameplayHash);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46900, SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi, SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)]
    internal bool CanOfferCuiLie(ProgramTriggerLegalityContext c) => c.SkillId == AnniversaryCuiLieComposition.Chibi ? CuiLieUse(c.WindowContext)is { } u && CuiLieContestEligible(c.OwnerSeat, u) : CuiLieLeases(c.OwnerSeat).Any(l => l.Source.SkillInstanceId == c.Candidate.SkillInstanceId);
    internal CardUseFrame? CuiLieUse(ProgramSkillWindowContext? w) => w is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } && _coreCardUseLifecycleOperations.LifecycleCardUse(id.ParentCardUseFrameId)is { Action: { } a } u && a.ActionId == id.CardActionId ? u : null;
    internal bool CuiLieContestEligible(int owner, CardUseFrame use) => use.Action is { Type: CardActionType.Use } a && a.ActorSeat != owner && _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[owner], AnniversaryCuiLieComposition.Chibi) && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[owner]) && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat]) && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(owner, a.ActorSeat) && CoreGameEngineOverflowTargetCancellationRuntime.OverflowPrimaryTargets(use.CardKind, a.EffectiveDesignatedTargetSeats).Any(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreMatchParticipantsPlayersAccess._players[t].IsAlive && _coreNativeRuleQueriesOperations.GetCombatDistance(a.ActorSeat, t) > 1);
    internal SkillProgramStepOutcome BeginCuiLie(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryCuiLie is not null || f.InstructionIndex != 1 || !CuiLieOriginal(f, op))
            throw new InvalidOperationException("Cui Lie requires his original phase, paid-lease reveal or far-target native candidate.");
        var actor = op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue ? f.SelectedTargetSeats.Single() : op == SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi ? CuiLieUse(f.WindowContext)!.SourceSeat : f.OwnerSeat;
        var r = new AnniversaryCuiLieReceipt(CuiLieSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor, op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue ? AnniversaryCuiLieStage.Number : op == SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi ? AnniversaryCuiLieStage.Pindian : AnniversaryCuiLieStage.Drawing);
        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi)
        {
            var use = CuiLieUse(f.WindowContext)!;
            r = r with
            {
                UseId = use.Id,
                ActionId = use.Action!.ActionId,
                PindianId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
        }

        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)
            r = r with
            {
                Number = 1,
                RewardLeases = CuiLieLeases(f.OwnerSeat).Where(l => l.Source.SkillInstanceId == f.SkillInstanceId).Select(l => l.FrameId).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieStartedEvent(f.Id, r.Source, r.Hash, op, r.ParentId, r.Turn, r.Phase, actor));
        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLiePindianIssuedEvent(f.Id, r.PindianId, actor));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), f.OwnerSeat, actor, programResultBind: CuiLieContest);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)
        {
            IssueCuiLieDraw(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishCuiLie(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int CuiLieChooser(ProgramSkillFrame f) => f.AnniversaryCuiLie!.Stage == AnniversaryCuiLieStage.Number ? f.AnniversaryCuiLie.Actor : f.OwnerSeat;
    internal PromptChoice CuiLieChoice(ProgramSkillFrame f, string verb, string value, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"cui-lie.{(f.Id)}.{(verb)}.{(value)}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", CuiLieAction }, { "verb", verb }, { "value", value }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } });
    internal Card[] CuiLieTransferable(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).SelectMany(c => c.Cards).Distinct().Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).OrderBy(c => c.Id).ToArray();
    internal IReadOnlyList<PromptChoice> CuiLieChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryCuiLie!;
        if (r.Stage == AnniversaryCuiLieStage.Number)
            return Array.AsReadOnly(Enumerable.Range(2, 3).Select(n => CuiLieChoice(f, "number", n.ToString(CultureInfo.InvariantCulture), "声明 " + n)).ToArray());
        if (r.Stage == AnniversaryCuiLieStage.Payment)
        {
            var choices = new List<PromptChoice>();
            if (CuiLieTransferable(f).Length >= r.Number)
                choices.AddRange(CuiLieTransferable(f).Where(c => !r.Cards.Contains(c.Id)).Select(c => CuiLieChoice(f, "card", c.Id.ToString(CultureInfo.InvariantCulture), "交出【" + c.DisplayName + "】", [c.Id])));
            choices.Add(CuiLieChoice(f, "draw", "done", "不交牌，摸声明数量的牌"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == AnniversaryCuiLieStage.BorrowedVictim)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], p)).Select(p => CuiLieChoice(f, "victim", p.Seat.ToString(CultureInfo.InvariantCulture), "追加借刀目标", targets: [f.OwnerSeat, p.Seat])).ToArray());
        return[];
    }

    internal void PublishCuiLie(ProgramSkillFrame f)
    {
        var choices = CuiLieChoices(f);
        if (choices.Count == 0)
        {
            FinishCuiLie(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var seat = CuiLieChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, rPrompt(f), Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal static string rPrompt(ProgramSkillFrame f) => f.AnniversaryCuiLie!.Stage == AnniversaryCuiLieStage.Number ? "赀爵：由所选角色声明2、3或4。" : f.AnniversaryCuiLie.Stage == AnniversaryCuiLieStage.Payment ? $"赀爵：交出{(f.AnniversaryCuiLie.Number)}张牌，已选{(f.AnniversaryCuiLie.Cards.Count)}张；或不交牌摸牌。" : "斥避：选择完整借刀追加目标。";
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue, SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi)]
    internal PromptChoice CuiLieAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer(CuiLieAction)]
    internal void AnswerCuiLie(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCuiLie: { } r } f || !ValidCuiLie(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != CuiLieChooser(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], CuiLieChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Cui Lie rejects another participant, private cost or native target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (answer.Parameters["verb"])
        {
            case "number":
                var n = int.Parse(answer.Parameters["value"], CultureInfo.InvariantCulture);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r with { Number = n, Stage = AnniversaryCuiLieStage.Payment } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieNumberEvent(f.Id, r.Actor, n));
                PublishCuiLie(f);
                return;
            case "card":
                var ids = r.Cards.Append(answer.Cards.Single()).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r = r with { Cards = ids } });
                if (ids.Length < r.Number)
                {
                    PublishCuiLie(f);
                    return;
                }

                GiveCuiLie(f);
                return;
            case "draw":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r with { Cards = [], Stage = AnniversaryCuiLieStage.Drawing } });
                IssueCuiLieDraw(f);
                return;
            case "victim":
                ApplyCuiLieMutation(f, answer.Targets);
                return;
        }

        throw new InvalidOperationException("Unknown Cui Lie native selection.");
    }

    internal ProgramSkillFrame CuiLieChildren(ProgramSkillFrame f, AnniversaryCuiLieStage stage)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = f.AnniversaryCuiLie!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal void GiveCuiLie(ProgramSkillFrame f)
    {
        var r = f.AnniversaryCuiLie!;
        if (r.Cards.Count != r.Number || !r.Cards.All(id => CuiLieTransferable(f).Any(c => c.Id == id)))
            throw new InvalidOperationException("Zijue lost its exact dynamically transferable HE payment.");
        f = CuiLieChildren(f, AnniversaryCuiLieStage.GiveChildren);
        r = f.AnniversaryCuiLie!;
        var native = _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.Actor, r.Cards, r.Before, null);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(r.Actor), new(CuiLieGive), (batch, moves) =>
        {
            native?.Invoke(batch, moves);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryCuiLie = current.AnniversaryCuiLie!with { Moves = moves, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieGiftEvent(f.Id, moves.Count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        ContinueCuiLie(f.Id);
    }

    internal void IssueCuiLieDraw(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = f.AnniversaryCuiLie!with { Stage = AnniversaryCuiLieStage.Drawing, Draw = null, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, f.AnniversaryCuiLie!.Number, new(CuiLieDraw));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue, SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)]
    internal void ReturnCuiLieDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryCuiLie!;
        if (!ValidCuiLie(f) || r.Stage != AnniversaryCuiLieStage.Drawing || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Number || draw.SequenceBefore != r.Before)
            throw new InvalidOperationException("Zijue lost its once-issued original native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r with { Stage = AnniversaryCuiLieStage.DrawChildren, Draw = draw, After = draw.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieDrawReturnedEvent(f.Id, f.OwnerSeat, r.Number, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        ContinueCuiLie(f.Id);
    }

    internal void ContinueCuiLie(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryCuiLie: { } r } f && f.Id == id)
        {
            if (!ValidCuiLie(f))
                throw new InvalidOperationException("Cui Lie lost his original paid frame or native return.");
            if (r.Stage == AnniversaryCuiLieStage.Pindian)
            {
                var result = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Single(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId).Result;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r with { Stage = AnniversaryCuiLieStage.PindianChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Result = result }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLiePindianReturnedEvent(f.Id, r.PindianId, result.SourceRank, result.OpponentRank, result.SourceWon));
                continue;
            }

            if (r.Stage is AnniversaryCuiLieStage.Number or AnniversaryCuiLieStage.Payment or AnniversaryCuiLieStage.BorrowedVictim)
            {
                PublishCuiLie(f);
                return;
            }

            if (r.Stage is AnniversaryCuiLieStage.Drawing or AnniversaryCuiLieStage.Completed)
                return;
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.AnniversaryCuiLie!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == AnniversaryCuiLieStage.GiveChildren)
            {
                f = CuiLieChildren(f, AnniversaryCuiLieStage.RecoveryChildren);
                _coreProgramLifecycleOperations.RecoverProgramTargetTo(f.Id, f.OwnerSeat, f.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp + 1, true);
                continue;
            }

            if (r.Stage == AnniversaryCuiLieStage.RecoveryChildren)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieLeaseEvent(f.Id, r.Source, r.Hash, r.Turn, r.Number));
                FinishCuiLie(f);
                return;
            }

            if (r.Stage == AnniversaryCuiLieStage.PindianChildren)
            {
                SettleCuiLieContest(f);
                return;
            }

            if (r.Stage == AnniversaryCuiLieStage.DrawChildren && r.Operation == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw && r.Cursor + 1 < r.RewardLeases.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = r with { Cursor = r.Cursor + 1 } });
                IssueCuiLieDraw(f);
                return;
            }

            FinishCuiLie(f);
            return;
        }
    }

    internal void FinishCuiLie(ProgramSkillFrame f)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCuiLie = f.AnniversaryCuiLie!with { Stage = AnniversaryCuiLieStage.Completed }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuiLieCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46900)]
    internal bool ResumeCuiLie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCuiLie: { Stage: not AnniversaryCuiLieStage.Completed } } f || f.Id != id)
            return false;
        ContinueCuiLie(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46900)]
    internal bool ReturnCuiLieMovement(ProgramSkillFrame f) => f.AnniversaryCuiLie is not null && ResumeCuiLie(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46900)]
    internal bool KeepCuiLie(ProgramSkillFrame f) => f.AnniversaryCuiLie is { } r ? ValidCuiLie(f) && r.Stage is not (AnniversaryCuiLieStage.Number or AnniversaryCuiLieStage.Payment) : f.SkillId == AnniversaryCuiLieComposition.Zijue && f.TriggerId == "paid-pindian-draw" && CuiLieLeases(f.OwnerSeat).Any(l => l.Source.SkillInstanceId == f.SkillInstanceId);
}
