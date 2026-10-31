#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    internal const string WuyouBinding = "hand-contribution";
    internal const string WuyouContributionReason = "skill-program.wumiao.wuyou.contribute";
    internal const string WuyouReturnReason = "skill-program.wumiao.wuyou.return";
    internal bool HasWuyouCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuyouContributionAndNamedReturn);

    internal ActualDiscardRecoveryPhaseKey? WuyouPlay(int donor) => HasWuyouCapability && _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(donor) && _coreMatchParticipantsPlayersAccess._players[donor].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == donor && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == donor && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    internal IEnumerable<(CardConversionSource Source, string Hash)> WuyouSources(CharacterState owner) => _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == "wumiao:wuyou" && i.Program.Activations.Any(a => a.Id == WuyouBinding && a.Effects is [{ Op: SkillProgramEffectOp.ResolveWuyouContributionAndNamedReturn }])).OrderBy(i => i.SkillInstanceId, StringComparer.Ordinal).Take(1).Select(i => (new CardConversionSource(i.SkillId, WuyouBinding, owner.Seat, i.SkillInstanceId), i.Program.GameplayHash));
    internal bool WuyouPhaseSpent(int issuer, int donor, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<WuyouStartedEvent>().Any(e => e.Source.OwnerSeat == issuer && e.DonorSeat == donor && e.Phase == phase);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28520, SkillProgramEffectOp.ResolveWuyouContributionAndNamedReturn)]
    internal bool WuyouHasNativeContributionOnly(ProgramActivationLegalityContext context) => false;
    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 28520)]
    internal IEnumerable<LegalAction> BuildWuyouContributions(CharacterState donor)
    {
        if (WuyouPlay(donor.Seat)is not { } phase)
            yield break;
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(donor).Select(c => c.Id).Order().ToArray();
        if (cards.Length == 0)
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => p.Seat))
            foreach (var(source, _)in WuyouSources(owner))
            {
                if (WuyouPhaseSpent(owner.Seat, donor.Seat, phase) || _characterSunHongOlOperations.HasPrintedGeneralSkillTargetBan(source, donor.Seat))
                    continue;
                yield return new(LegalActionKind.UseProgramSkill, null, owner.Seat, $"武佑：交给 {(owner.Name)} 一张手牌", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 1, MaxTargetCount: 1)
                {
                    ProgramSkillId = source.SkillId,
                    ProgramActivationId = WuyouBinding,
                    ProgramSkillOwnerSeat = owner.Seat,
                    SelectableCardIds = Array.AsReadOnly(cards),
                    SelectableTargetSeats = Array.AsReadOnly(new[] { owner.Seat }),
                    ProgramAiHint = new(1, 0, 0, 0, 0, 0, true, false)
                };
            }
    }

    internal LegalAction? CurrentWuyouContribution(CharacterState donor, string? skill, string? binding, int? owner) => BuildWuyouContributions(donor).SingleOrDefault(a => a.ProgramSkillId == skill && a.ProgramActivationId == binding && a.ProgramSkillOwnerSeat == owner);
    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 28520)]
    internal bool SubmitWuyouContribution(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (command.SkillId != "wumiao:wuyou" || command.ActivationId != WuyouBinding)
            return false;
        if (_coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction)is { } error)
        {
            result = _kernelCoreMainOperations2.Reject(error.Code, error.Message);
            return true;
        }

        var donor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = CurrentWuyouContribution(donor, command.SkillId, command.ActivationId, command.SkillOwnerSeat);
        if (action is null)
        {
            result = _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "武佑必须来自当前出牌阶段的真实贡献菜单。");
            return true;
        }

        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, command.CardIds, command.TargetSeats)is { } invalid)
        {
            result = _kernelCoreMainOperations2.Reject(invalid.Code, invalid.Message);
            return true;
        }

        result = _kernelCoreMainOperations2.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            ExecuteWuyouContribution(donor, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 28520)]
    internal bool ExecuteWuyouContribution(CharacterState donor, LegalAction supplied, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (supplied.ProgramSkillId != "wumiao:wuyou" || supplied.ProgramActivationId != WuyouBinding)
            return false;
        var action = CurrentWuyouContribution(donor, supplied.ProgramSkillId, supplied.ProgramActivationId, supplied.ProgramSkillOwnerSeat) ?? throw new InvalidOperationException("Wuyou's contribution source or actual Play changed.");
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, cards, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ProgramSkillOwnerSeat!.Value];
        var source = WuyouSources(owner).Single();
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(donor).Single(c => c.Id == cards.Single());
        var phase = WuyouPlay(donor.Seat)!;
        var receipt = new WuyouReceipt(source.Source, source.Hash, phase, donor.Seat, card.Id, card.Kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Stage = WuyouStage.ContributionChildren,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, source.Source.SkillId, WuyouBinding, source.Hash, 0, [], [])
        {
            SkillInstanceId = source.Source.SkillInstanceId,
            Wuyou = receipt
        };
        f = _coreHpConditionalSkillGrantsOperations.CaptureHpConditionalProgramSource(f);
        f = _characterHeQiOlOperations.CaptureEquipmentSuitProgramSource(f);
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouStartedEvent(f.Id, receipt.Source, receipt.GameplayHash, phase, donor.Seat, card.Id, card.Kind, receipt.Before));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(donor.Name)} 发起了 {(owner.Name)} 的【武佑】。", donor.Seat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    SkillProgramStepOutcome IWuGuanYuWuyouHost.Resolve(ProgramSkillFrame frame) => BeginWuyou(frame);
    internal SkillProgramStepOutcome BeginWuyou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.Wuyou;
        if (r is null || !ValidWuyouFrame(f, false) || f.InstructionIndex != 1 || r.ContributionPaid || WuyouPlay(r.DonorSeat) != r.Phase || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(r.ContributionCardId) != CardLocation.Hand(r.DonorSeat))
            throw new InvalidOperationException("Wuyou requires its once-only original donor Hand entity and actual Play.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.DonorSeat == f.OwnerSeat)
        {
            // A self gift changes no zone and must not fabricate gain/loss events.
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Wuyou = r with { ContributionPaid = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouContributionPaidEvent(f.Id, r.DonorSeat, f.OwnerSeat, r.ContributionCardId, r.ContributionPrintedKind, 0, r.Before, r.Before, true));
        }
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([r.ContributionCardId], CardLocation.Hand(f.OwnerSeat), new(WuyouContributionReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.DonorSeat, f.OwnerSeat, [r.ContributionCardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, moves) =>
            {
                var move = moves.Single();
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { Wuyou = current.Wuyou!with { ContributionPaid = true, ContributionBatchId = batch, ContributionSequence = move.Sequence, After = move.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouContributionPaidEvent(f.Id, r.DonorSeat, f.OwnerSeat, move.CardId, move.CardKind, batch, r.Before, move.Sequence, false));
            }));
        ContinueWuyou(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueWuyou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Wuyou: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (!ValidWuyouFrame(f, true))
            throw new InvalidOperationException("Wuyou lost its original source, phase or once-paid Hand transfers.");
        if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Wuyou!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == WuyouStage.ReturnChildren)
        {
            CompleteWuyou(f);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.DonorSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CompleteWuyou(f);
            return;
        }

        if (r.Names.Count == 0)
        {
            var pool = WuyouComposition.Names.ToList();
            var names = new List<CardKind>();
            var before = _randomOperations._random.State;
            for (var i = 0; i < 5; i++)
            {
                var previous = _randomOperations._random.State;
                var index = _randomOperations._random.Next(pool.Count);
                var name = pool[index];
                pool.RemoveAt(index);
                names.Add(name);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouNameSampledEvent(id, i, name, previous, _randomOperations._random.State));
            }

            r = r with
            {
                Stage = WuyouStage.ReturnChoice,
                RandomBefore = before,
                RandomAfter = _randomOperations._random.State,
                Names = names
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Wuyou = r });
        }

        PublishWuyouReturn(f);
    }

    internal IReadOnlyList<PromptChoice> WuyouReturnChoices(ProgramSkillFrame f)
    {
        var r = f.Wuyou!;
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "wuyou-return",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        var choices = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.DonorSeat].IsAlive ? r.Names.SelectMany(kind => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).OrderBy(c => c.Id).Select(card => new PromptChoice(new($"wuyou.{(f.Id)}.{(kind)}.card-{(card.Id)}"), $"将【{(card.DisplayName)}】交还并视为【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [r.DonorSeat], new Dictionary<string, string>(Parameters("give")) { ["card-kind"] = kind.ToString() }))).ToList() : new List<PromptChoice>();
        choices.Add(new(new($"wuyou.{(f.Id)}.decline"), "不交还手牌", [], [], Parameters("decline")));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishWuyouReturn(ProgramSkillFrame f)
    {
        var choices = WuyouReturnChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "武佑：可选择一个随机牌名并交还一张手牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [f.Wuyou!.DonorSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wuyou-return")]
    internal void AnswerWuyouReturn(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Wuyou: { Stage: WuyouStage.ReturnChoice } r } f || !ValidWuyouFrame(f, true) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], WuyouReturnChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Wuyou's private return must match its exact five names, current Hand and original donor.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("branch") == "decline")
        {
            CompleteWuyou(f);
            return;
        }

        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == selected.Cards.Single());
        var kind = Enum.Parse<CardKind>(selected.Parameters["card-kind"]);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        r = r with
        {
            Stage = WuyouStage.ReturnChildren,
            ChosenKind = kind,
            ReturnCardId = card.Id,
            ReturnPrintedKind = card.Kind,
            ReturnBefore = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Wuyou = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouReturnSelectedEvent(f.Id, card.Id, card.Kind, kind, before));
        if (f.OwnerSeat == r.DonorSeat)
            CommitWuyouReturn(f, 0, before, true);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(r.DonorSeat), new(WuyouReturnReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.DonorSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, moves) => CommitWuyouReturn(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), batch, moves.Single().Sequence, false)));
        ContinueWuyou(f.Id);
    }

    internal void CommitWuyouReturn(ProgramSkillFrame f, long batch, long sequence, bool self)
    {
        var r = f.Wuyou!;
        if (r.ReturnPaid || r.ReturnCardId is null || r.ReturnPrintedKind is null || r.ChosenKind is null)
            throw new InvalidOperationException("Wuyou cannot issue an unpaid, different or duplicate returned entity lease.");
        var lease = new WuyouHandLease(f.Id, r.Source, r.GameplayHash, r.DonorSeat, r.ReturnCardId.Value, r.ReturnPrintedKind.Value, r.ChosenKind.Value, batch, sequence, self);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Wuyou = r with { ReturnPaid = true, ReturnBatchId = batch, ReturnSequence = sequence, After = sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouReturnPaidEvent(f.Id, lease.CardId, batch, r.ReturnBefore, sequence, self));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouLeaseIssuedEvent(lease));
    }

    internal void CompleteWuyou(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuyouCompletedEvent(f.Id, f.Wuyou!.DonorSeat, f.Wuyou.ReturnPaid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28520)]
    internal bool ResumeWuyou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Wuyou: { ContributionPaid: true } } f || f.Id != id)
            return false;
        ContinueWuyou(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28520)]
    internal bool ReturnWuyouMovement(ProgramSkillFrame f)
    {
        if (f.Wuyou is null)
            return false;
        ContinueWuyou(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28520)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28520)]
    internal bool ContinuePaidWuyou(ProgramSkillFrame f) => f.Wuyou is { ContributionPaid: true } && ValidWuyouFrame(f, true);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuyouContributionAndNamedReturn)]
    internal PromptChoice ChooseWuyouReturn(PendingDecision prompt, ProgramSkillFrame f) => prompt.Choices.OrderBy(c => c.Cards.Count == 0 ? int.MaxValue : CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenByDescending(c => c.Parameters.TryGetValue("card-kind", out var kind) ? CardCatalog.Get(Enum.Parse<CardKind>(kind)).AiPlayValue : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
