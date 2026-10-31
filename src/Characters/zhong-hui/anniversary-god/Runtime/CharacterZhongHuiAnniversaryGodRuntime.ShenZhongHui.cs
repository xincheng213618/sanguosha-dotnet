#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhongHuiAnniversaryGodRuntime
{
    internal static CardConversionSource ShenZhSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int LinCold(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(PlayerMarkerKind.LinCold);
    internal bool HasLishiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseLishiPunishments);

    SkillProgramStepOutcome IShenZhongHuiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenZh(f, op);
    internal CardActionContext? DuzhangAction(ProgramSkillWindowContext? c) => c is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)?.Action;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 39300, SkillProgramEffectOp.ResolveLinjieRound, SkillProgramEffectOp.AwakenJianghuo)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39300, SkillProgramEffectOp.ResolveLinjieRound, SkillProgramEffectOp.ResolveLinjieDamage, SkillProgramEffectOp.ResolveDuzhang, SkillProgramEffectOp.AwakenJianghuo, SkillProgramEffectOp.ResolveLishiPhase)]
    internal bool CanTriggerShenZh(ProgramTriggerLegalityContext gate)
    {
        var t = _coreProgramLifecycleOperations.GetProgramTrigger2(gate.Candidate);
        var c = gate.WindowContext;
        return t.Effects[0].Op switch
        {
            SkillProgramEffectOp.ResolveLinjieRound => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && LinCold(p.Seat) == 0),
            SkillProgramEffectOp.ResolveLinjieDamage => c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } seat } && seat != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && LinCold(seat) > 0 && LinjieDiscardCards(gate.OwnerSeat, seat).Count > 0,
            SkillProgramEffectOp.ResolveDuzhang => DuzhangAction(c)is { EffectiveIsRed: false, TargetSeats: [var target] } a && (a.ActorSeat == gate.OwnerSeat || target == gate.OwnerSeat),
            SkillProgramEffectOp.AwakenJianghuo => _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(gate.OwnerSeat, gate.SkillId, gate.Candidate.BindingId) || _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Any(d => d.TargetSeat == p.Seat && d.Amount > 0)),
            SkillProgramEffectOp.ResolveLishiPhase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { } phase && phase.ActorSeat == gate.OwnerSeat && LishiActiveEffects(phase).Any(e => e.Option == (c?.Window == SkillProgramTriggerWindow.JudgmentPhaseStarting ? 2 : 3)),
            _ => true
        };
    }

    internal IReadOnlyList<Card> LinjieDiscardCards(int owner, int target) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, CardLocation.Hand(target), OwnedCardMoveIntent.Discard)).ToArray());
    internal SkillProgramStepOutcome BeginShenZh(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenZhongHui is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Shen Zhong Hui lost his original unpaid native binding.");
        var target = op == SkillProgramEffectOp.ResolveLinjieDamage ? w.TargetSeat!.Value : f.OwnerSeat;
        var r = new ShenZhongHuiReceipt(ShenZhSource(f), f.GameplayHash, w.ParentFrameId, op, ShenZhongHuiStage.Complete, target);
        r = r with
        {
            Stage = op switch
            {
                SkillProgramEffectOp.ResolveLinjieRound => ShenZhongHuiStage.RoundChoice,
                SkillProgramEffectOp.ResolveLinjieDamage => ShenZhongHuiStage.DamageChildren,
                SkillProgramEffectOp.ResolveDuzhang => ShenZhongHuiStage.DrawAttempt,
                SkillProgramEffectOp.AwakenJianghuo => ShenZhongHuiStage.AwakenFinish,
                SkillProgramEffectOp.ChooseLishiPunishments => LinCold(f.OwnerSeat) > 0 ? ShenZhongHuiStage.LishiChoice : ShenZhongHuiStage.Complete,
                _ => w.Window == SkillProgramTriggerWindow.JudgmentPhaseStarting ? ShenZhongHuiStage.PhaseChoice : ShenZhongHuiStage.Complete
            }
        };
        if (op == SkillProgramEffectOp.ResolveDuzhang)
            r = r with
            {
                Count = 1,
                ActionId = DuzhangAction(w)!.ActionId
            };
        if (op == SkillProgramEffectOp.ResolveLishiPhase)
            r = r with
            {
                Phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() ?? throw new InvalidOperationException("Lishi lost its actual phase token.")
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhongHuiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, target));
        if (op == SkillProgramEffectOp.ResolveLinjieDamage)
        {
            var choices = LinjieDiscardCards(f.OwnerSeat, target);
            if (choices.Count == 0)
            {
                FinishShenZh(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var card = choices[_randomOperations._random.Next(choices.Count)];
            var chosen = card.Id;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var last = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r = r with { LastHandDiscarded = last, Before = before, After = before }, PendingMovementContinuation = new(target, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, target, card, CardLocation.Hand(target), new($"skill-program.{(ShenZhongHuiComposition.SameEdition(f.SkillId, ShenZhongHuiComposition.Linjie))}.random-discard"));
            var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > before && m.CardId == chosen && m.From == CardLocation.Hand(target) && m.To == CardLocation.DiscardPile);
            last &= paid && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShenZhongHui = r with { LastHandDiscarded = last, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LinjieRandomDiscardedEvent(f.Id, target, chosen, last, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }
        else if (op == SkillProgramEffectOp.ResolveDuzhang)
        {
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new($"skill-program.{(f.SkillId)}.draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }
        else if (op == SkillProgramEffectOp.AwakenJianghuo)
        {
            var count = _coreMatchParticipantsPlayersAccess._players.Sum(p => LinCold(p.Seat));
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                if (LinCold(p.Seat)is> 0 and var marks)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, p.Seat, PlayerMarkerKind.LinCold, -marks);
            if (count > 0)
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.LinCold, count);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShenZhongHui = r with { Count = count, Stage = count > 0 ? ShenZhongHuiStage.DrawAttempt : ShenZhongHuiStage.AwakenFinish } });
            if (count > 0)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new($"skill-program.{(f.SkillId)}.draw"));
                return SkillProgramStepOutcome.AwaitChild;
            }
        }
        else if (op == SkillProgramEffectOp.ChooseLishiPunishments && LinCold(f.OwnerSeat) == 0)
        {
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 1, nature: DamageNature.Thunder, sourceLess: !ShenZhongHuiComposition.IsHappy(f.SkillId));
            return SkillProgramStepOutcome.AwaitChild;
        }
        else if (op == SkillProgramEffectOp.ResolveLishiPhase && w.Window == SkillProgramTriggerWindow.DrawPhaseEnded)
            SettleLishiDrawPhase(f);
        ContinueShenZh(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenZh(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 39300)]
    internal bool ResumeShenZh(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenZhongHui: not null } f || f.Id != id)
            return false;
        ContinueShenZh(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39300)]
    internal bool ReturnShenZhMovement(ProgramSkillFrame f) => f.ShenZhongHui is not null && ResumeShenZh(f.Id);
    internal void ContinueShenZh(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenZhongHui: { } r } f && f.Id == id)
        {
            if (r.Stage is ShenZhongHuiStage.DrawAttempt or ShenZhongHuiStage.PhaseJudging)
                return;
            if (DrainShenZh(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenZhongHui!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishShenZh(f);
                return;
            }

            if (r.Stage is ShenZhongHuiStage.RoundChoice or ShenZhongHuiStage.LishiChoice or ShenZhongHuiStage.PhaseChoice)
            {
                PublishShenZh(f);
                return;
            }

            if (r.Stage == ShenZhongHuiStage.RoundMark)
            {
                if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.TargetSeat, PlayerMarkerKind.LinCold, 1);
                FinishShenZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            if (r.Stage == ShenZhongHuiStage.DamageChildren)
            {
                if (r.LastHandDiscarded && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.DamageFollowup } });
                    _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1);
                    return;
                }

                FinishShenZh(f);
                return;
            }

            if (r.Stage == ShenZhongHuiStage.DamageFollowup)
            {
                if (LinCold(r.TargetSeat) > 0)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, r.TargetSeat, PlayerMarkerKind.LinCold, -1);
                FinishShenZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            if (r.Stage == ShenZhongHuiStage.DrawChildren)
            {
                if (r.Operation == SkillProgramEffectOp.ResolveDuzhang)
                {
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.LinCold, 1);
                    FinishShenZh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r = r with { Stage = ShenZhongHuiStage.AwakenFinish } });
            }

            if (r.Stage == ShenZhongHuiStage.AwakenFinish)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.Complete } });
                var linjie = ShenZhongHuiComposition.SameEdition(f.SkillId, ShenZhongHuiComposition.Linjie);
                var lishi = ShenZhongHuiComposition.SameEdition(f.SkillId, ShenZhongHuiComposition.Lishi);
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
                _coreProgramSkillChangesOperations.GrantProgramSkills(f, [lishi]);
                var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                foreach (var grant in owner.SkillGrants.Grants.Where(g => g.IsEnabled && g.SkillId == linjie).ToArray())
                    owner.SkillGrants.RemoveGrant(grant.GrantId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramOwnerSkillsReplacedEvent(f.Id, f.SkillId, f.OwnerSeat, Array.AsReadOnly(new[] { linjie }), lishi));
                continue;
            }

            if (r.Stage is ShenZhongHuiStage.PhaseResult or ShenZhongHuiStage.PhaseCleanup or ShenZhongHuiStage.PhaseNext)
            {
                ContinueLishiJudgments(f);
                return;
            }

            FinishShenZh(f);
            return;
        }
    }

    internal void FinishShenZh(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenZhongHui = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveDuzhang, SkillProgramEffectOp.AwakenJianghuo)]
    internal void ReturnShenZhDraw(ProgramSkillFrame supplied, NativeDrawOutcome o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenZhongHui ?? throw new InvalidOperationException("Shen Zhong Hui's native draw lost its original receipt.");
        if (r.Stage != ShenZhongHuiStage.DrawAttempt || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Count || o.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenZhongHuiDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Shen Zhong Hui's draw return cannot change its paid count.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenZhongHui = r with { Stage = ShenZhongHuiStage.DrawChildren, Before = o.SequenceBefore, After = o.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhongHuiDrawReturnedEvent(f.Id, f.OwnerSeat, o.RequestedCount, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter));
        ContinueShenZh(f.Id);
    }

    internal IReadOnlyList<PromptChoice> ShenZhChoices(ProgramSkillFrame f)
    {
        var r = f.ShenZhongHui!;
        PromptChoice C(string value, string text, IReadOnlyList<int>? targets = null) => new(new($"shen-zh.{(f.Id)}.{(r.Stage)}.{(value)}"), text, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "shen-zh-choice", ["value"] = value });
        if (r.Stage == ShenZhongHuiStage.RoundChoice)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && LinCold(p.Seat) == 0).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, p.Seat)).Select(p => C(p.Seat.ToString(CultureInfo.InvariantCulture), $"对{(p.Name)}造成1点伤害，然后令其获得凛", [p.Seat])).ToArray());
        if (r.Stage == ShenZhongHuiStage.LishiChoice)
            return Array.AsReadOnly(Enumerable.Range(0, 32).Where(mask => System.Numerics.BitOperations.PopCount((uint)mask) <= LinCold(f.OwnerSeat)).Select(mask => C(mask.ToString(CultureInfo.InvariantCulture), mask == 0 ? "不移去凛" : "移去" + System.Numerics.BitOperations.PopCount((uint)mask) + "个凛，执行第" + string.Join("、", Enumerable.Range(1, 5).Where(n => (mask & (1 << (n - 1))) != 0)) + "项")).ToArray());
        if (r.Stage == ShenZhongHuiStage.PhaseChoice)
            return Array.AsReadOnly(new[] { CardKind.Lightning, CardKind.Indulgence, CardKind.SupplyShortage }.Where(k => !r.Judgments.Contains(k)).Select(k => C(((int)k).ToString(CultureInfo.InvariantCulture), "选择【" + CardCatalog.Get(k).DisplayName + "】判定")).ToArray());
        return[];
    }

    internal void PublishShenZh(ProgramSkillFrame f)
    {
        var choices = ShenZhChoices(f);
        if (choices.Count == 0)
        {
            FinishShenZh(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：选择结算。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shen-zh-choice")]
    internal void AnswerShenZh(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Shen Zhong Hui lost his original choice owner.");
        if (!ShenZhChoices(f).Any(c => c.Id == choice.Id && c.Targets.SequenceEqual(choice.Targets) && c.Cards.SequenceEqual(choice.Cards) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Shen Zhong Hui rejects a stale or forged original choice.");
        var r = f.ShenZhongHui!;
        var value = int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == ShenZhongHuiStage.RoundChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { TargetSeat = value, Stage = ShenZhongHuiStage.RoundMark } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LinjieTargetSelectedEvent(f.Id, value));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, value, 1);
            return;
        }

        if (r.Stage == ShenZhongHuiStage.LishiChoice)
        {
            var count = System.Numerics.BitOperations.PopCount((uint)value);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Mask = value, Count = count, Stage = ShenZhongHuiStage.LishiIssue } });
            if (count > 0)
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.LinCold, -count);
            var token = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()?.Token ?? 0;
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, p.Seat)))
                foreach (var option in Enumerable.Range(1, 5).Where(n => (value & (1 << (n - 1))) != 0))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LishiPunishmentIssuedEvent(f.Id, r.Source, r.GameplayHash, p.Seat, option, token));
                    _coreCharacterHostOperations.AcquireRuntimeSkills(p, f.SkillId, [ShenZhongHuiComposition.SameEdition(f.SkillId, ShenZhongHuiComposition.Curse)]);
                }
        }
        else if (r.Stage == ShenZhongHuiStage.PhaseChoice)
        {
            var judgments = r.Judgments.Append((CardKind)value).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenZhongHui = r with { Judgments = judgments } });
            if (judgments.Length == 2)
            {
                BeginLishiJudgment(f);
                return;
            }
        }

        ContinueShenZh(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveLinjieRound, SkillProgramEffectOp.ChooseLishiPunishments, SkillProgramEffectOp.ResolveLishiPhase)]
    internal PromptChoice ChooseShenZhAi(PendingDecision prompt, ProgramSkillFrame f) => f.ShenZhongHui?.Stage == ShenZhongHuiStage.LishiChoice ? prompt.Choices.OrderByDescending(c => System.Numerics.BitOperations.PopCount((uint)int.Parse(c.Parameters["value"], CultureInfo.InvariantCulture))).First() : prompt.Choices.FirstOrDefault(c => c.Targets.Count == 1 && c.Targets[0] != f.OwnerSeat) ?? prompt.Choices[0];

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39300)]
    internal bool RetainShenZh(ProgramSkillFrame f) => f.ShenZhongHui is not null;
}
