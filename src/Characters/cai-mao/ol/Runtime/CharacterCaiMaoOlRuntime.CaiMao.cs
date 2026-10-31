#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiMaoOlRuntime
{
    internal const string CaiMaoExchangeReason = "skill-program.cai-mao.zuolian.exchange";
    internal const string CaiMaoRevealPrefix = "cai-mao.reveal.";
    internal bool HasCaiMaoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveZuolianWholeRevealExchange) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles);

    internal static CardConversionSource CaiMaoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cai_mao_ol_CaiMao.CaiMaoSource(f);
    SkillProgramStepOutcome ICaiMaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCaiMao(f, op);
    internal SkillProgramStepOutcome BeginCaiMao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CaiMao is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Cai Mao requires an original living, unpaid binding.");
        long parent = 0;
        if (op == SkillProgramEffectOp.ResolveZuolianWholeRevealExchange)
        {
        }
        else
        {
            if (op != SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles || f.SkillId != "ol:jingzhou" || f.TriggerId != "before-own-damage" || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || !CaiMaoDamageParentMatches(f, context))
                throw new InvalidOperationException("Jingzhou requires its exact pending native damage before HP is lost.");
            parent = context.ParentFrameId;
        }

        var r = new CaiMaoReceipt(CaiMaoSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp), parent, CaiMaoStage.Targets)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.FrozenHp, parent));
        PublishCaiMao(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool CaiMaoSourceLive(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal IReadOnlyList<PromptChoice> CaiMaoChoices(ProgramSkillFrame f)
    {
        var r = f.CaiMao!;
        var result = new List<PromptChoice>();
        if (r.Stage == CaiMaoStage.Targets)
        {
            if (r.Targets.Count < r.FrozenHp)
                foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !r.Targets.Contains(p.Seat) && (r.Op == SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles || _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0)))
                    result.Add(new(new($"cai-mao.{(f.Id)}.target.{(p.Seat)}"), $"选择{(p.Name)}", [], [p.Seat], new Dictionary<string, string> { { "program-action", "cai-mao" }, { "branch", "target" } }));
            result.Add(new(new($"cai-mao.{(f.Id)}.done"), r.Targets.Count == 0 ? "不选择角色" : "确认所选角色", [], [], new Dictionary<string, string> { { "program-action", "cai-mao" }, { "branch", "done" } }));
        }
        else if (r.Stage == CaiMaoStage.Exchange)
        {
            foreach (var accept in new[]
            {
                true,
                false
            }

            )
                result.Add(new(new($"cai-mao.{(f.Id)}.exchange.{(accept)}"), accept ? "全体交换" : "全体不换", [], [], new Dictionary<string, string> { { "program-action", "cai-mao" }, { "branch", accept ? "exchange" : "decline" } }));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishCaiMao(ProgramSkillFrame f)
    {
        var choices = CaiMaoChoices(f);
        if (choices.Count == 0)
        {
            FinishCaiMao(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.CaiMao!.Stage == CaiMaoStage.Exchange ? "佐练：是否将本次全体展示牌与属性【杀】交换？" : $"{(skill.Name)}：选择至多{(f.CaiMao.FrozenHp)}名角色，再确认。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("cai-mao")]
    internal void AnswerCaiMao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaiMao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CaiMaoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Cai Mao requires the exact current private whole-operation choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!CaiMaoSourceLive(f))
        {
            FinishCaiMao(f);
            return;
        }

        var branch = choice.Parameters["branch"];
        if (branch == "target")
        {
            var seat = choice.Targets.Single();
            var targets = r.Targets.Append(seat).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r with { Targets = targets } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoTargetChosenEvent(f.Id, targets.Length - 1, seat));
            PublishCaiMao(f);
            return;
        }

        if (branch == "done")
        {
            var targets = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(r.TurnOwner, s)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r = r with { Targets = targets, Committed = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoTargetsCommittedEvent(f.Id, targets.Length));
            if (targets.Length == 0)
            {
                FinishCaiMao(f);
                return;
            }

            if (r.Op == SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r with { Stage = CaiMaoStage.ToggleChildren } });
                ContinueCaiMao(f.Id);
                return;
            }

            var reveals = new List<CaiMaoReveal>();
            foreach (var holder in targets)
            {
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).OrderBy(c => c.Id).ToArray();
                if (!_coreMatchParticipantsPlayersAccess._players[holder].IsAlive || hand.Length == 0)
                    continue;
                var card = hand[_randomOperations._random.Next(hand.Length)];
                var entry = _characterLuoXianOlOperations.CurrentHandEntry(holder, card.Id) ?? throw new InvalidOperationException("Zuolian needs a real original hand entry.");
                var reveal = new CaiMaoReveal(holder, card.Id, card.Kind, entry, CaiMaoRevealPrefix + reveals.Count);
                reveals.Add(reveal);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = (_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { CaiMao = r = r with { Reveals = reveals.ToArray() } }));
                _coreProgramRevealsOperations.CommitProgramHandReveal(f.Id, holder, reveal.Bind, [card.Id]);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoHandRevealedEvent(f.Id, reveals.Count - 1, reveal));
            }

            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = f.CaiMao!with { Stage = CaiMaoStage.Exchange } });
            PublishCaiMao(f);
            return;
        }

        if (r.Stage != CaiMaoStage.Exchange || branch is not ("exchange" or "decline"))
            throw new InvalidOperationException("Zuolian requires one whole-roster exchange decision.");
        var accepted = branch == "exchange";
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r = r with { ExchangeAccepted = accepted } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoExchangeChosenEvent(f.Id, accepted));
        if (!accepted)
        {
            FinishCaiMao(f);
            return;
        }

        PayCaiMaoExchange(f);
    }

    internal bool DrainCaiMao(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    internal void ContinueCaiMao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaiMao: { } r } f || f.Id != id)
            return;
        if (r.Stage is CaiMaoStage.Targets or CaiMaoStage.Exchange)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCaiMao(f);
            return;
        }

        if (DrainCaiMao(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.CaiMao!;
        if (r.Stage == CaiMaoStage.ToggleChildren)
        {
            while (r.Cursor < r.Targets.Count)
            {
                var index = r.Cursor;
                var target = r.Targets[index];
                var before = _coreMatchParticipantsPlayersAccess._players[target].IsChained;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = r = r with { Cursor = index + 1 } });
                if (_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    _coreProgramLifecycleOperations.SetProgramChainedState(f, !before, target);
                var actual = _coreMatchParticipantsPlayersAccess._players[target].IsChained;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoToggleIssuedEvent(id, index, target, before, !before, actual));
                if (DrainCaiMao(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.CaiMao!;
            }
        }

        FinishCaiMao(f);
    }

    internal void FinishCaiMao(ProgramSkillFrame f)
    {
        var r = f.CaiMao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiMaoCompletedEvent(f.Id, r.Op, r.Exchanges.Count, r.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaiMao = null, CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => !b.Name.StartsWith(CaiMaoRevealPrefix, StringComparison.Ordinal)).ToArray()) });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27250)]
    internal bool ResumeCaiMao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaiMao: not null } f || f.Id != id)
            return false;
        ContinueCaiMao(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27250)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27250)]
    internal bool ContinuePaidCaiMao(ProgramSkillFrame f) => f.CaiMao is { Stage: CaiMaoStage.ExchangeChildren or CaiMaoStage.ToggleChildren } && ValidCaiMao(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveZuolianWholeRevealExchange, SkillProgramEffectOp.ResolveJingzhouBeforeDamageToggles)]
    internal PromptChoice ChooseCaiMaoAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.CaiMao!;
        if (r.Stage == CaiMaoStage.Exchange)
        {
            var good = r.Reveals.Sum(e => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), e.HolderSeat, new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false)));
            return p.Choices.Single(c => c.Parameters["branch"] == (good >= 0 ? "exchange" : "decline"));
        }

        var candidates = p.Choices.Where(c => c.Targets.Count == 1).Select(c => (Choice: c, Score: CaiMaoTargetScore(f, c.Targets[0]))).OrderByDescending(x => x.Score).ThenBy(x => x.Choice.Targets[0]).ToArray();
        return candidates.Length > 0 && candidates[0].Score > 0 ? candidates[0].Choice : p.Choices.Single(c => c.Parameters["branch"] == "done");
    }

    internal double CaiMaoTargetScore(ProgramSkillFrame f, int target)
    {
        var support = _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), target, new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false));
        if (f.CaiMao!.Op == SkillProgramEffectOp.ResolveZuolianWholeRevealExchange)
            return support;
        var elemental = f.WindowContext?.ParentFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(w => w.Id == id && w.Nature is DamageNature.Fire or DamageNature.Thunder or DamageNature.Cold);
        if (target == f.OwnerSeat && elemental)
            return _coreMatchParticipantsPlayersAccess._players[target].IsChained ? 0 : 100;
        return _coreMatchParticipantsPlayersAccess._players[target].IsChained ? support : -support;
    }
}
