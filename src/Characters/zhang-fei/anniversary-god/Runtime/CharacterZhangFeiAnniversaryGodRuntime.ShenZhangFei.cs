#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    internal const string ShenZfReason = "skill-program.anniversary:shencai.";
    internal static CardConversionSource ShenZfSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    internal int ShenZfMark(int seat, PlayerMarkerKind kind) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(kind);
    internal string ShenZfIssuedInstance(int seat) => "shencai-issued:" + seat.ToString(CultureInfo.InvariantCulture);
    internal bool ShenZfValidSentence(ShenZhangFeiSentenceEvent e) => ShenZhangFeiComposition.IsShencai(e.Source.SkillId) && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(e.Source.SkillId, out var skill) && e.GameplayHash == skill.Program?.GameplayHash;
    internal string? ShenZfIssuedSkill(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) ? _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiSentenceEvent>().LastOrDefault(e => e.TargetSeat == seat && ShenZfValidSentence(e))?.Source.SkillId : null;
    internal bool ShenZfIssued(int seat) => ShenZfIssuedSkill(seat)is not null;
    internal int ShenZfLimit(int seat) => Math.Min(5, 1 + _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiXunshiEvent>().Count(e => e.Source.OwnerSeat == seat));
    SkillProgramStepOutcome IShenZhangFeiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenZf(f, op);
    int IShenZhangFeiSelection.Limit(int owner) => ShenZfLimit(owner);
    internal SkillProgramStepOutcome BeginShenZf(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenZhangFei is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Shencai requires the original unpaid instruction.");
        if (op == SkillProgramEffectOp.ResolveShenZhangFeiXunshi)
        {
            var action = ShenZfUseAction(f.WindowContext!);
            if (action is null || !ShenZfColorless(action) || action.ActorSeat != f.OwnerSeat)
                return SkillProgramStepOutcome.Continue;
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhangFeiXunshiEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.ActionId == action.ActionId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiXunshiEvent(ShenZfSource(f), f.GameplayHash, action.ActionId, Math.Min(5, ShenZfLimit(f.OwnerSeat) + 1)));
            return SkillProgramStepOutcome.Continue;
        }

        var claiming = f.TriggerId == "judgment-first-gain";
        var target = claiming ? f.WindowContext!.Judgment!.SubjectSeat : op == SkillProgramEffectOp.ResolveShenZhangFeiShencai ? f.SelectedTargetSeats.Single() : f.OwnerSeat;
        var amount = f.TriggerId == "chi-damage" ? f.WindowContext!.Amount : 1;
        var r = new ShenZhangFeiReceipt(ShenZfSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, target, amount, op == SkillProgramEffectOp.ResolveShenZhangFeiShencai && !claiming ? ShenZhangFeiStage.Judging : claiming ? ShenZhangFeiStage.GainChildren : ShenZhangFeiStage.PenaltyChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            BatchId = f.WindowContext?.MovementBatch?.Id ?? 0,
            TakeBeforeDeath = f.SkillId == ShenZhangFeiComposition.HappyShencai && f.TriggerId is null ? true : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, target, amount));
        if (claiming)
            ClaimShenZfJudgment(f);
        else if (op == SkillProgramEffectOp.ResolveShenZhangFeiShencai)
        {
            if (target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Shencai requires its selected other living subject.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { JudgmentId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 } });
            _ = _coreNativeJudgmentOperations.BeginJudgment(null, target, "anniversary-shencai", f.Id, null, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "shencai-judgment", programResultVisibility: SkillProgramCardSetVisibility.Public);
        }
        else
            PayShenZfPunishment(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenZf(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueShenZf(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenZhangFei: { } r } f && f.Id == id)
        {
            if (r.Stage == ShenZhangFeiStage.Judging)
                return;
            if (DrainShenZf(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenZhangFei!;
            if (r.Stage == ShenZhangFeiStage.JudgmentCleanupChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.JudgmentReturned }, PendingMovementContinuation = null });
                continue;
            }

            if (r.Stage == ShenZhangFeiStage.JudgmentReturned && r.ResultCardId is { } resultId && _cardZonesOperations._cardZones.GetLocation(resultId) == CardLocation.Processing)
            {
                if (!HasUnclaimedShenZfJudgmentResult(f, resultId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
                    throw new InvalidOperationException("Shencai cannot dispose a result without its exact native judgment return and unclaimed processing transfer.");
                MoveShenZfCard(f, _coreAdvancedLifecycleOperations.GetAdvancedCard(resultId), CardLocation.Processing, CardLocation.DiscardPile, ShenZhangFeiStage.JudgmentCleanupChildren, "judgment-unclaimed");
                continue;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShenZf(f);
                return;
            }

            if (r.Stage == ShenZhangFeiStage.JudgmentReturned)
            {
                if (r.TakeBeforeDeath == true && r.ResultKind is { } resultKind && (CardPrintedInfo.Terms(resultKind) & (PrintedCardTerms.Hp | PrintedCardTerms.Weapon | PrintedCardTerms.Play | PrintedCardTerms.Distance)) == PrintedCardTerms.None && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.TakeChoice }, PendingMovementContinuation = null });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.MarkChildren }, PendingMovementContinuation = null });
                SentenceShenZf(f);
                continue;
            }

            if (r.Stage == ShenZhangFeiStage.TakeChildren && r.TakeBeforeDeath == true)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.MarkChildren }, PendingMovementContinuation = null });
                SentenceShenZf(f);
                continue;
            }

            if (r.Stage is ShenZhangFeiStage.MarkChildren or ShenZhangFeiStage.TakeChoice)
            {
                var terms = r.ResultKind is { } kind ? CardPrintedInfo.Terms(kind) & (PrintedCardTerms.Hp | PrintedCardTerms.Weapon | PrintedCardTerms.Play | PrintedCardTerms.Distance) : PrintedCardTerms.None;
                if (r.ResultCardId is not null && terms == PrintedCardTerms.None && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && !(r.TakeBeforeDeath == true && r.Stage == ShenZhangFeiStage.MarkChildren))
                {
                    var choices = ShenZfTakeChoices(f);
                    if (choices.Count > 0)
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.TakeChoice } });
                        PublishShenZfTake(f, choices);
                        return;
                    }

                    if (r.TakeBeforeDeath == true)
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Stage = ShenZhangFeiStage.MarkChildren }, PendingMovementContinuation = null });
                        SentenceShenZf(f);
                        continue;
                    }
                }
            }

            FinishShenZf(f);
            return;
        }
    }

    internal void FinishShenZf(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void SentenceShenZf(ProgramSkillFrame f)
    {
        var r = f.ShenZhangFei!;
        if (r.ResultCardId is null || r.ResultKind is not { } kind || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            return;
        var terms = CardPrintedInfo.Terms(kind) & (PrintedCardTerms.Hp | PrintedCardTerms.Weapon | PrintedCardTerms.Play | PrintedCardTerms.Distance);
        if (terms == PrintedCardTerms.None)
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.TargetSeat, PlayerMarkerKind.ShencaiSi, 1);
        else
        {
            foreach (var m in CharacterZhangFeiAnniversaryGodDefinitions.ShenZfFour)
                if (ShenZfMark(r.TargetSeat, m)is> 0 and var count)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.TargetSeat, m, -count);
            for (var index = 0; index < CharacterZhangFeiAnniversaryGodDefinitions.ShenZfFour.Length; index++)
                if (terms.HasFlag(CharacterZhangFeiAnniversaryGodDefinitions.ShenZfTerms[index]))
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.TargetSeat, CharacterZhangFeiAnniversaryGodDefinitions.ShenZfFour[index], 1);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiSentenceEvent(f.Id, r.Source, r.GameplayHash, r.TargetSeat, terms, ShenZfMark(r.TargetSeat, PlayerMarkerKind.ShencaiSi)));
    }

    internal IReadOnlyList<PromptChoice> ShenZfTakeChoices(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.ShenZhangFei!.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "shen-zf-take" } }).ToArray();
    internal void PublishShenZfTake(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "神裁：获得受判角色区域内的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [f.ShenZhangFei!.TargetSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.ShenZhangFei.TargetSeat,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shen-zf-take")]
    internal void AnswerShenZfTake(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Shencai lost its card-choice parent.");
        var r = f.ShenZhangFei!;
        if (r.Stage != ShenZhangFeiStage.TakeChoice || !ShenZfTakeChoices(f).Any(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) && c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(chosen.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Shencai requires the exact currently published public card or hidden hand slot.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(chosen.Parameters["source-zone"]), r.TargetSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(chosen.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        MoveShenZfCard(f, card, from, card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat), ShenZhangFeiStage.TakeChildren, "take");
        ContinueShenZf(f.Id);
    }

    internal void MoveShenZfCard(ProgramSkillFrame f, Card card, CardLocation from, CardLocation to, ShenZhangFeiStage stage, string purpose)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = f.ShenZhangFei!with { Stage = stage, CardId = card.Id, From = from, To = to, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, new(ShenZfReason + purpose), m =>
        {
            var now = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(now with { ShenZhangFei = now.ShenZhangFei!with { Paid = true, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiMovementPaidEvent(f.Id, stage, card.Id, from, to, before, m.Sequence));
        });
    }

    internal void PayShenZfPunishment(ProgramSkillFrame f)
    {
        var r = f.ShenZhangFei!;
        if (!CanRunIssuedShenZf(ShenZfFrameCandidate(f), f.WindowContext!))
            throw new InvalidOperationException("The issued punishment lost its original marked participant and native occurrence.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhangFei = r with { Paid = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiPunishmentPaidEvent(f.Id, f.TriggerId!, f.OwnerSeat, r.Amount, r.BatchId));
        if (f.TriggerId == "chi-damage")
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, r.Amount);
        else if (f.TriggerId == "tu-hand-loss")
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
            if (hand.Count > 0)
                MoveShenZfCard(f, hand[_randomOperations._random.Next(hand.Count)], CardLocation.Hand(f.OwnerSeat), CardLocation.DiscardPile, ShenZhangFeiStage.PenaltyChildren, "tu-discard");
        }
        else if (f.TriggerId == "liu-turn-ending")
            _coreProgramLifecycleOperations.TurnOverProgramTarget(f.Id, f.OwnerSeat, f.OwnerSeat);
        else if (f.TriggerId == "si-turn-ended")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillCauseDeathDeclaredEvent(f.Id, f.SkillId, f.TriggerId, f.OwnerSeat, f.OwnerSeat));
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
            return;
        }

        ContinueShenZf(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38600)]
    internal bool ResumeShenZf(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenZhangFei: not null } f || f.Id != id)
            return false;
        ContinueShenZf(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38600)]
    internal bool ReturnShenZfMovement(ProgramSkillFrame f) => f.ShenZhangFei is not null && ResumeShenZf(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38600)]
    internal bool KeepShenZfPaid(ProgramSkillFrame f) => f.ShenZhangFei is not null || ShenZhangFeiComposition.IsShencai(f.SkillId) && f.TriggerId is { } binding && ShenZfPenaltyMarker(binding)is not null && f.ActivationId == binding && f.InstructionIndex == 0 && f.WindowContext is { } context && CanRunIssuedShenZf(ShenZfFrameCandidate(f), context);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenZhangFeiShencai)]
    internal PromptChoice AiShenZf(PendingDecision d, ProgramSkillFrame f) => d.Choices.First();
}
