#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiAnniversaryGodRuntime
{
    internal const string ShenDaTuoYu = "anniversary-god:tuoyu", ShenDaXianjin = "anniversary-god:xianjin", ShenDaQijing = "anniversary-god:qijing", ShenDaCuixin = "anniversary-god:cuixin";
    internal static CardConversionSource ShenDaSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal ShenDengAiDomain[] ShenDaDeveloped(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiDevelopedEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.Domain).Distinct().ToArray();
    internal int ShenDaDamageCount(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Count(e => e.Amount > 0 && (e.TargetSeat == owner || !e.SourceLess && e.SourceSeat == owner));
    internal bool ShenDaAwakened(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiAwakenedEvent>().Any(e => e.OwnerSeat == owner);
    internal ShenDengAiAssignedEvent? ShenDaAssignment(int owner, int card, bool beforePayment = false)
    {
        var a = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiAssignedEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.CardId == card);
        if (a is null)
            return null;
        var exits = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == card && m.Sequence > a.MovementSequence && m.From == CardLocation.Hand(owner) && m.To != m.From).ToArray();
        if (exits.Length == 0)
            return a;
        // A declared use is captured immediately before or after its own real hand payment.
        return beforePayment && exits.Length == 1 && exits[0].To == CardLocation.Processing && exits[0].Reason.Value == "card.use" && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card && m.Sequence > exits[0].Sequence) ? a : null;
    }

    internal int[] ShenDaUnassigned(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => ShenDaAssignment(owner, c.Id)is null).Select(c => c.Id).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.HandCardProjection, 30)]
    internal CardSnapshot ProjectShenDengAiHandCard(CharacterState owner, Card card, CardSnapshot snapshot) => _characterDengAiHappyGodOperations.ShenDaTuoYuId(owner)is not null && ShenDaAssignment(owner.Seat, card.Id)is { } a ? snapshot with
    {
        SkillMarkLabel = ShenDaDomainName(a.Domain)
    }

    : snapshot;
    internal static string ShenDaDomainName(ShenDengAiDomain d) => d switch
    {
        ShenDengAiDomain.Qingqu => "清渠",
        ShenDengAiDomain.Fengtian => "丰田",
        _ => "峻山"
    };
    SkillProgramStepOutcome IShenDengAiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenDa(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38500)]
    internal bool CanOfferShenDengAi(ProgramTriggerLegalityContext g)
    {
        if (CharacterDengAiHappyGodRuntime.IsShenDaTuoYu(g.SkillId))
            return g.Trigger.Id == "initialize-domains" || ShenDaDeveloped(g.OwnerSeat).Length > 0 && (g.SkillId == "happy:tuoyu" ? _corePhysicalOwnedCardQueriesOperations.GetHand(g.Owner).Count > 0 : ShenDaUnassigned(g.OwnerSeat).Length > 0);
        if (CharacterDengAiHappyGodRuntime.IsShenDaXianjin(g.SkillId))
        {
            var count = ShenDaDamageCount(g.OwnerSeat);
            return count > 0 && count % 2 == 0 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiXianjinReservedEvent>().Any(e => e.OwnerSeat == g.OwnerSeat && e.DamageCount >= count) && (g.Context.SourceSeat == g.OwnerSeat || g.Context.TargetSeat == g.OwnerSeat);
        }

        if (CharacterDengAiHappyGodRuntime.IsShenDaQijing(g.SkillId))
            return !ShenDaAwakened(g.OwnerSeat) && ShenDaDeveloped(g.OwnerSeat).Length == 3;
        if (CharacterDengAiHappyGodRuntime.IsShenDaCuixin(g.SkillId))
            return ShenDaCuixinOriginal(g.Context, g.OwnerSeat, g.SkillId)is { } use && ShenDaRepeatPlans(g.OwnerSeat, use.CardKind, use.TargetSeats).Count > 0;
        return true;
    }

    internal SkillProgramStepOutcome BeginShenDa(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WindowContext is not { } w || f.ShenDengAi is not null)
            throw new InvalidOperationException("Shen Deng Ai requires its unpaid native program occurrence.");
        if (op == SkillProgramEffectOp.InitializeShenDengAiTuoYu)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiInitializedEvent(f.Id, f.OwnerSeat));
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.AwakenShenDengAiQijing && f.InstructionIndex == 1)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiAwakenedEvent(f.Id, f.OwnerSeat));
            return SkillProgramStepOutcome.Continue;
        }

        var stage = op switch
        {
            SkillProgramEffectOp.AllocateShenDengAiTuoYu => ShenDengAiStage.Allocate,
            SkillProgramEffectOp.DevelopShenDengAiXianjin => ShenDengAiStage.Develop,
            SkillProgramEffectOp.AwakenShenDengAiQijing => ShenDengAiStage.MoveSeat,
            _ => ShenDengAiStage.RepeatChoice
        };
        var r = new ShenDengAiReceipt(ShenDaSource(f), f.GameplayHash, w.ParentFrameId, f.InstructionIndex, stage, DamageCount: stage == ShenDengAiStage.Develop ? ShenDaDamageCount(f.OwnerSeat) : 0);
        if (stage == ShenDengAiStage.RepeatChoice)
        {
            var original = ShenDaCuixinOriginal(w, f.OwnerSeat, f.SkillId) ?? throw new InvalidOperationException("Cuixin lost its edition-qualified genuine completed Use.");
            r = r with
            {
                OriginalUseId = original.Id,
                OriginalActionId = original.Action!.ActionId,
                OutputKind = original.CardKind
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenDengAi = r });
        if (stage == ShenDengAiStage.Develop)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiXianjinReservedEvent(f.Id, f.OwnerSeat, r.DamageCount));
            if (ShenDaDeveloped(f.OwnerSeat).Length == 3)
            {
                DrawShenDaXianjin(f);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        ContinueShenDa(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShenDaChoices(ProgramSkillFrame f)
    {
        var r = f.ShenDengAi!;
        PromptChoice Choice(string key, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new("shen-deng-ai." + f.Id + "." + key), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "shen-deng-ai", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = key });
        if (r.Stage == ShenDengAiStage.Allocate)
        {
            var cards = _characterDengAiHappyGodOperations.ShenDaAllocationCards(f);
            if (cards.Length == 0)
                return[];
            var choices = cards.SelectMany(cardId => ShenDaDeveloped(f.OwnerSeat).Select(d => Choice("assign:" + (int)d + (f.SkillId == ShenDaTuoYu && cardId == cards[0] ? "" : ":" + cardId), "分配至“" + ShenDaDomainName(d) + "”", [cardId]))).ToList();
            if (f.SkillId == "happy:tuoyu")
                choices.Add(Choice("skip", "完成分配，保留其余手牌的当前区域"));
            return _coreCardDistributionPresentationOperations.PresentCardDistribution(f, choices, "tuoyu", destination: c => "domain:" + c.Parameters["option"].Split(':')[1], label: c =>
            {
                var domain = (ShenDengAiDomain)int.Parse(c.Parameters["option"].Split(':')[1], CultureInfo.InvariantCulture);
                var effect = domain switch
                {
                    ShenDengAiDomain.Qingqu => "无次数与距离限制",
                    ShenDengAiDomain.Fengtian => "伤害与回复值 +1",
                    _ => "无法被响应"
                };
                var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count(card => ShenDaAssignment(f.OwnerSeat, card.Id)?.Domain == domain);
                return $"{(ShenDaDomainName(domain))} · {(count)} 张\n{(effect)}";
            });
        }

        if (r.Stage == ShenDengAiStage.Develop)
            return Enum.GetValues<ShenDengAiDomain>().Except(ShenDaDeveloped(f.OwnerSeat)).Select(d => Choice("develop:" + (int)d, "开发“" + ShenDaDomainName(d) + "”")).ToArray();
        if (r.Stage == ShenDengAiStage.MoveSeat)
        {
            var others = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray();
            return others.Length < 2 ? [] : others.Select((s, i) => Choice("seat:" + s + ":" + others[(i + 1) % others.Length], "移至“" + _coreMatchParticipantsPlayersAccess._players[s].Name + "”与“" + _coreMatchParticipantsPlayersAccess._players[others[(i + 1) % others.Length]].Name + "”之间", targets: [s, others[(i + 1) % others.Length]])).ToArray();
        }

        if (r.Stage == ShenDengAiStage.RepeatChoice)
        {
            var original = _coreCardUseLifecycleOperations.LifecycleCardUse(r.OriginalUseId);
            if (original is null)
                return[];
            return ShenDaRepeatPlans(f.OwnerSeat, r.OutputKind!.Value, original.TargetSeats).Select(p => Choice(p.Id, p.Description, targets: p.TargetSeats)).Append(Choice("skip", "不发动摧心")).ToArray();
        }

        return[];
    }

    internal void PublishShenDa(ProgramSkillFrame f)
    {
        var choices = ShenDaChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, (f.ShenDengAi!.Stage == ShenDengAiStage.Allocate ? "拓域：多选手牌，再选择已开发的副区域。" : skill.Name + "：请选择。"), choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shen-deng-ai")]
    internal void AnswerShenDa(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenDengAi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShenDaChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shen Deng Ai requires its original private chooser and current legal option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var parts = choice.Parameters["option"].Split(':');
        if (r.Stage == ShenDengAiStage.Allocate)
        {
            if (parts[0] == "skip")
            {
                FinishShenDa(f);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiAssignedEvent(f.Id, f.OwnerSeat, choice.Cards.Single(), (ShenDengAiDomain)int.Parse(parts[1], CultureInfo.InvariantCulture), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueShenDa(f.Id);
            return;
        }

        if (r.Stage == ShenDengAiStage.Develop)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiDevelopedEvent(f.Id, f.OwnerSeat, (ShenDengAiDomain)int.Parse(parts[1], CultureInfo.InvariantCulture)));
            DrawShenDaXianjin(f);
            return;
        }

        if (r.Stage == ShenDengAiStage.MoveSeat)
        {
            _coreTableOrderOperations.MoveTableSeatBetween(f.OwnerSeat, choice.Targets[0], choice.Targets[1], f.Id);
            _coreSkillProgramsOperations.PendProgramExtraTurn(f, null);
            FinishShenDa(f);
            return;
        }

        if (r.Stage == ShenDengAiStage.RepeatChoice)
        {
            if (parts[0] == "skip")
            {
                FinishShenDa(f);
                return;
            }

            IssueShenDaCuixin(f, choice.Parameters["option"]);
            return;
        }

        throw new InvalidOperationException("Shen Deng Ai cannot answer an already issued native child.");
    }

    internal void DrawShenDaXianjin(ProgramSkillFrame f)
    {
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var draw = f.SkillId != "happy:xianjin" && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= count) ? 1 : ShenDaDeveloped(f.OwnerSeat).Length;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenDengAi = f.ShenDengAi!with { Stage = ShenDengAiStage.DrawAttempt, DrawCount = draw }, PendingMovementContinuation = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, draw, new("skill-program.shen-deng-ai.xianjin.draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DevelopShenDengAiXianjin)]
    internal void ReturnShenDaDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenDengAi!;
        if (r.Stage != ShenDengAiStage.DrawAttempt || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.DrawCount)
            throw new InvalidOperationException("Xianjin lost its once-issued genuine draw request and typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenDengAi = r with { Stage = ShenDengAiStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenDengAiDrawReturnedEvent(f.Id, f.OwnerSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueShenDa(f.Id);
    }

    internal void ContinueShenDa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenDengAi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage is ShenDengAiStage.NativeUse or ShenDengAiStage.DrawAttempt)
            return;
        if (r.Stage is ShenDengAiStage.DrawChildren or ShenDengAiStage.NativeChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
                return;
            FinishShenDa(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishShenDa(f);
            return;
        }

        if (ShenDaChoices(f).Count == 0)
        {
            if (r.Stage == ShenDengAiStage.MoveSeat)
                _coreSkillProgramsOperations.PendProgramExtraTurn(f, null);
            FinishShenDa(f);
            return;
        }

        PublishShenDa(f);
    }

    internal void FinishShenDa(ProgramSkillFrame f)
    {
        RestoreShenDaCuixinCursor(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenDengAi = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38500)]
    internal bool ResumeShenDa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenDengAi: not null } f || f.Id != id)
            return false;
        ContinueShenDa(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38500)]
    internal bool ReturnShenDaMovement(ProgramSkillFrame f)
    {
        if (f.ShenDengAi is null)
            return false;
        ContinueShenDa(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38500)]
    internal bool ContinueIssuedShenDa(ProgramSkillFrame f) => f.ShenDengAi is { Stage: ShenDengAiStage.DrawAttempt or ShenDengAiStage.DrawChildren or ShenDengAiStage.NativeUse or ShenDengAiStage.NativeChildren } || CharacterDengAiHappyGodRuntime.IsShenDaQijing(f.SkillId) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenDengAiAwakenedEvent>().Any(e => e.FrameId == f.Id);

    [ProgramAiChoice(SkillProgramEffectOp.AllocateShenDengAiTuoYu)]
    internal PromptChoice ChooseShenDaAllocation(PendingDecision p, ProgramSkillFrame f) => ChooseShenDa(p, f);
    [ProgramAiChoice(SkillProgramEffectOp.DevelopShenDengAiXianjin)]
    internal PromptChoice ChooseShenDaDevelopment(PendingDecision p, ProgramSkillFrame f) => ChooseShenDa(p, f);
    [ProgramAiChoice(SkillProgramEffectOp.AwakenShenDengAiQijing)]
    internal PromptChoice ChooseShenDaAwakening(PendingDecision p, ProgramSkillFrame f) => ChooseShenDa(p, f);
    [ProgramAiChoice(SkillProgramEffectOp.RepeatShenDengAiCuixin)]
    internal PromptChoice ChooseShenDaRepeat(PendingDecision p, ProgramSkillFrame f) => ChooseShenDa(p, f);
    internal PromptChoice ChooseShenDa(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") != "skip") ?? p.Choices.First();
}
