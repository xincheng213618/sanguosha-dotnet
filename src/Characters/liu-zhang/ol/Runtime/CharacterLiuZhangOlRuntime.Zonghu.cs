#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    SkillProgramStepOutcome ILiuZhangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveZonghuRequiredUseRequest ? ResolveZonghu(f) : ResolveFengwei(f, op);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27512, SkillProgramEffectOp.ResolveZonghuRequiredUseRequest)]
    internal bool IsZonghuOrdinaryActivation(ProgramActivationLegalityContext c) => c.Activation.Id == "native-use-request" && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && ZonghuSources(c.Owner).Any() && ZonghuVictims(c.OwnerSeat).Any() && ZonghuOutputs(ZonghuPlayNeed(c.OwnerSeat)).Any();
    internal static string ZonghuDecisionHash(PendingDecision p) => RuntimeSkills.RuntimeRules_liu_zhang_ol_Zonghu.ZonghuDecisionHash(p);
    internal static bool SameZonghuNeed(ZonghuNeed a, ZonghuNeed b) => RuntimeSkills.RuntimeRules_liu_zhang_ol_Zonghu.SameZonghuNeed(a, b);
    internal bool IsZonghuAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && (c.Parameters.ContainsKey(ZonghuOpen) || c.Parameters.ContainsKey(ZonghuChoice))) == true;
    internal CommandResult SubmitZonghuAnswer(int actor, PromptId prompt, ChoiceId id)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != actor || p.PromptId != prompt)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidPrompt, "宗护必须回答当前真实请求。");
        var c = p.Choices.SingleOrDefault(c => c.Id == id && (c.Parameters.ContainsKey(ZonghuOpen) || c.Parameters.ContainsKey(ZonghuChoice)));
        if (c is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidChoice, "宗护选择未公布。");
        return _kernelCoreMainOperations2.Accept(() =>
        {
            AnswerZonghu(c);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void OpenZonghu(PromptChoice c)
    {
        var p = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("Zonghu has no real native request.");
        var n = CaptureZonghuNeed(p) ?? throw new InvalidOperationException("Zonghu lost its exact native requester.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(c);
        if (!p.Choices.Any(x => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])) || !ZonghuSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]).Any(s => s.Source == source) || !ZonghuVictims(p.PlayerSeat).Any() || !ZonghuOutputs(n).Any())
            throw new InvalidOperationException("Zonghu cannot replace an unavailable native request or source.");
        var hash = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash;
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, p.PlayerSeat, source.SkillId, source.BindingId, hash, 0, [], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            Zonghu = new(source, hash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n, p, ZonghuStage.Declare)
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuStartedEvent(f.Id, source, hash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n.Intent, n.OwnerFrameId, n.RequestFrameId, p.PromptId, p.Revision, ZonghuDecisionHash(p)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(f.Id, host, host);
    }

    internal SkillProgramStepOutcome ResolveZonghu(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.InstructionIndex != 1)
            throw new InvalidOperationException("Zonghu must enter its registered instruction.");
        if (f.Zonghu is null)
        {
            if (f.ActivationId != "native-use-request" || f.TriggerId is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || !ZonghuSources(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any())
                throw new InvalidOperationException("Zonghu requires a real Play activation.");
            var original = new PendingDecision(DecisionKind.PlayCard, f.OwnerSeat, "宗护真实出牌阶段", [], [])
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                Revision = _coreCommandSessionQueriesOperations.Revision
            };
            var source = ZonghuSource(f);
            var n = ZonghuPlayNeed(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = new(source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n, original, ZonghuStage.Declare) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuStartedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n.Intent, 0, 0, original.PromptId, original.Revision, ZonghuDecisionHash(original)));
        }

        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        PublishZonghu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ZonghuChoices(ProgramSkillFrame f)
    {
        var r = f.Zonghu!;
        var result = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            [ZonghuChoice] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        PromptChoice C(string branch, string label, IReadOnlyList<int>? targets = null, string suffix = "") => new(new($"zonghu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(suffix)}"), label, [], targets ?? [], Args(branch));
        if (r.Stage == ZonghuStage.Declare)
            foreach (var(kind, target)in ZonghuOutputs(r.Need))
            {
                var args = Args("declare");
                args["kind"] = kind.ToString();
                args["target"] = target.ToString(CultureInfo.InvariantCulture);
                result.Add(new(new($"zonghu.{(f.Id)}.declare.{(kind)}.{(target)}"), $"需要【{(CardCatalog.Get(kind).DisplayName)}】" + (ZonghuIsPlayed(r.Need) || r.Need.Intent == ZonghuIntent.Dodge ? "" : $"，目标：{(_coreMatchParticipantsPlayersAccess._players[target].Name)}"), [], [], args));
            }
        else if (r.Stage == ZonghuStage.Victim)
            foreach (var seat in ZonghuVictims(f.OwnerSeat))
                result.Add(C("victim", "交牌给" + _coreMatchParticipantsPlayersAccess._players[seat].Name, [seat], seat.ToString(CultureInfo.InvariantCulture)));
        else if (r.Stage == ZonghuStage.Card && r.RecipientSeat is { } recipient)
        {
            foreach (var card in ZonghuGiftCards(f.OwnerSeat))
            {
                var selected = r.Selected.Contains(card.Id);
                if (!selected && r.Selected.Count >= ZonghuCost)
                    continue;
                result.Add(new(new($"zonghu.{(f.Id)}.gift.{(card.Id)}.{(selected)}"), (selected ? "取消" : "选择") + card.DisplayName, [card.Id], [], Args(selected ? "unpick" : "pick")));
            }

            if (r.Selected.Count == ZonghuCost)
                result.Add(C("give", $"交给{(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}{(ZonghuCost)}张牌"));
        }

        if (r.Stage == ZonghuStage.Declare)
            result.Add(C("cancel", "取消，返回原请求"));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishZonghu(ProgramSkillFrame f)
    {
        var r = f.Zonghu!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !ZonghuNativeNeedCurrent(r) || r.Stage != ZonghuStage.Declare && !ZonghuNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value))
        {
            FinishZonghuAttempt(f, false);
            return;
        }

        var choices = ZonghuChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == ZonghuStage.Declare ? "宗护：选择本人使用的杀或闪。" : r.Stage == ZonghuStage.Victim ? "宗护：选择交牌对象。" : $"宗护：选择{(ZonghuCost)}张手牌或装备牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void AnswerZonghu(PromptChoice c)
    {
        if (c.Parameters.ContainsKey(ZonghuOpen))
        {
            OpenZonghu(c);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zonghu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ZonghuChoices(f).Any(x => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])))
            throw new InvalidOperationException("Zonghu changed its exact private declaration or opaque native slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = c.Parameters[ZonghuChoice];
        if (branch == "cancel")
        {
            FinishZonghuAttempt(f, false);
            return;
        }

        if (branch == "declare")
        {
            var kind = Enum.Parse<CardKind>(c.Parameters["kind"]);
            var target = int.Parse(c.Parameters["target"], CultureInfo.InvariantCulture);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuDeclaredEvent(f.Id, kind, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = r with { OutputKind = kind, TargetSeat = target, Stage = ZonghuStage.Victim } });
            PublishZonghu(f);
            return;
        }

        if (branch == "victim")
        {
            var seat = c.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuRecipientSelectedEvent(f.Id, seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = r with { RecipientSeat = seat, Stage = ZonghuStage.Card } });
            PublishZonghu(f);
            return;
        }

        if (branch is "pick" or "unpick")
        {
            var id = c.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = r with { Selected = branch == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(x => x != id).ToArray() } });
            PublishZonghu(f);
            return;
        }

        if (branch != "give" || r.RecipientSeat is not { } recipient || !ZonghuVictims(f.OwnerSeat).Contains(recipient) || r.Selected.Count != ZonghuCost || r.Selected.Any(id => !ZonghuGiftCards(f.OwnerSeat).Any(card => card.Id == id)) || !ZonghuNativeNeedCurrent(r) || !ZonghuNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value))
            throw new InvalidOperationException("Zonghu lost its exact gift or original true-use request.");
        var materials = r.Selected.Select(id => (Card: _coreAdvancedLifecycleOperations.GetAdvancedCard(id), From: _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var grain = materials.Any(m => m.Card.Kind == CardKind.WoodenOx && m.From == CardLocation.Equipment(f.OwnerSeat)) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => card.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = r with { Stage = ZonghuStage.CostChildren, Before = before, After = before, GrainIds = grain }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < materials.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuGiftSelectedEvent(f.Id, i, materials[i].Card.Id));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, CardLocation.Hand(recipient), new(ZonghuGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, r.Selected, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, moves) =>
        {
            var gifts = moves.Select(m => new ZonghuGift(m.CardId, materials.Single(x => x.Card.Id == m.CardId).Card.Kind, m.From, m.Sequence, batch)).OrderBy(g => Array.IndexOf(r.Selected.ToArray(), g.CardId)).ToArray();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = new ZonghuPaidOrigin(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, r.Need, r.OutputKind!.Value, r.TargetSeat!.Value, recipient, gifts, ZonghuCost, before, moves.Max(m => m.Sequence));
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { Zonghu = current.Zonghu!with { Paid = paid, After = paid.After } });
            for (var i = 0; i < gifts.Length; i++)
            {
                var g = gifts[i];
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuGiftPaidEvent(f.Id, i, g.CardId, g.Kind, g.From, recipient, g.Sequence, g.BatchId));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuCostPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, recipient, ZonghuCost, before, paid.After));
        }));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = f.Zonghu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueZonghu(f.Id);
    }

    internal bool DrainZonghu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueZonghu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zonghu: { } r } f || f.Id != id)
            return;
        if (r.Stage != ZonghuStage.CostChildren)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishZonghu(f);
            return;
        }

        if (DrainZonghu(f))
            return;
        var matched = r.Paid is { } p && ValidZonghuPayment(p);
        var legal = matched && ZonghuNativeNeedCurrent(r) && ZonghuNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuAttemptResolvedEvent(f.Id, matched, legal));
        FinishZonghuAttempt(f, legal);
    }

    internal void FinishZonghuAttempt(ProgramSkillFrame f, bool success)
    {
        var r = f.Zonghu!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zonghu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        if (success)
            DispatchZonghu(r.Paid!);
        else
            RepublishZonghuNative(r);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27512)]
    internal bool ResumeZonghu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zonghu: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueZonghu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27512)]
    internal bool ReturnZonghuMovement(ProgramSkillFrame f)
    {
        if (f.Zonghu is null)
            return false;
        ContinueZonghu(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27512)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27512)]
    internal bool ContinuePaidZonghu(ProgramSkillFrame f)
    {
        if (f.Zonghu?.Paid is null)
            return false;
        return true;
    }

    internal bool TryAdvanceZonghuAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].IsHuman)
            return false;
        var choices = p.Choices.Where(c => c.Parameters.ContainsKey(ZonghuOpen) || c.Parameters.ContainsKey(ZonghuChoice)).ToArray();
        if (choices.Length == 0)
            return false;
        // No private victim card is inspected here. An actual opaque slot is selected.
        var selected = choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault(ZonghuChoice) != "cancel" && c.Parameters.GetValueOrDefault(ZonghuChoice) != "unpick") ?? choices[0];
        AnswerZonghu(selected);
        return true;
    }
}
