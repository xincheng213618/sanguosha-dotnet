#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuFengBoundaryRuntime
{
    internal const string BoundaryLiuFengAction = "boundary-liu-feng";
    internal static CardConversionSource BoundaryLiuFengSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryLiuFengHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginBoundaryLiuFeng(f, op);
    internal bool BoundaryLiuFengPaidThisTurn(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengCostPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal int[] BoundaryLiuFengTargets(int actor) => _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], p, CardKind.Slash, Suit.None, null, 0, [], conversionSource: null)).Select(p => p.Seat).ToArray();
    internal int[] BoundaryLiuFengCurrentTargets(ProgramSkillFrame f) => f.BoundaryLiuFeng?.Need is { } n ? n.Targets.Where(t => BoundaryLiuFengRequestTarget(n, t)).ToArray() : BoundaryLiuFengTargets(f.OwnerSeat);
    internal bool BoundaryLiuFengCountsSlash(ProgramSkillFrame f) => _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat == f.OwnerSeat && (f.BoundaryLiuFeng?.Need is null || f.BoundaryLiuFeng.Need.Intent == BoundaryLiuFengIntent.BorrowedSword);
    internal void CancelBoundaryLiuFeng(ProgramSkillFrame f)
    {
        if (f.BoundaryLiuFeng is not { Need: not null, OriginalDecision: { } original } r)
        {
            FinishBoundaryLiuFeng(f);
            return;
        }

        CompleteBoundaryLiuFengFrame(f);
        if (r.CostSequence > 0)
            throw new InvalidOperationException("A paid Qinling cannot return as an unpaid cancellation.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = BoundaryLiuFengNativeDecision(original)with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 50201, SkillProgramEffectOp.ResolveBoundaryLiuFengQinling)]
    internal bool CanActivateBoundaryLiuFeng(ProgramActivationLegalityContext c) => !BoundaryLiuFengPaidThisTurn(c.OwnerSeat) && BoundaryLiuFengTargets(c.OwnerSeat).Length > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count > 0;
    internal SkillProgramStepOutcome BeginBoundaryLiuFeng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.BoundaryLiuFeng is { Need: not null } existing)
        {
            if (op != SkillProgramEffectOp.ResolveBoundaryLiuFengQinling || f.InstructionIndex != 1 || !ValidBoundaryLiuFeng(f) || !BoundaryLiuFengNeedCurrent(existing))
                throw new InvalidOperationException("Qinling lost its original native request instruction.");
            PublishBoundaryLiuFeng(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (f.InstructionIndex != 1 || f.BoundaryLiuFeng is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Liu Feng requires its original empty-selection instruction.");
        var r = new BoundaryLiuFengReceipt(BoundaryLiuFengSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, op == SkillProgramEffectOp.ResolveBoundaryLiuFengXiansi ? BoundaryLiuFengStage.Collect : BoundaryLiuFengStage.Targets);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengStartedEvent(f.Id, r.Source, r.Hash, op, r.ParentId, r.Turn, r.TurnOwner));
        PublishBoundaryLiuFeng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryLiuFengChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiuFeng!;
        var result = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = BoundaryLiuFengAction,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == BoundaryLiuFengStage.Collect)
        {
            var slots = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, p.Seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer)).ToArray();
            void Add(PromptChoice[] selection)
            {
                var args = Args("collect");
                args["count"] = selection.Length.ToString(CultureInfo.InvariantCulture);
                for (var i = 0; i < selection.Length; i++)
                    foreach (var key in new[]
                    {
                        "card-owner-seat",
                        "source-zone",
                        "slot-index"
                    }

                    )
                        args[$"{(i)}.{(key)}"] = selection[i].Parameters[key];
                result.Add(new(new($"boundary-liu-feng.{(f.Id)}.collect.{(result.Count)}"), string.Join("；", selection.Select(x => _coreMatchParticipantsPlayersAccess._players[int.Parse(x.Parameters["card-owner-seat"], CultureInfo.InvariantCulture)].Name + "：" + x.Description)), selection.SelectMany(x => x.Cards).ToArray(), selection.Select(x => int.Parse(x.Parameters["card-owner-seat"], CultureInfo.InvariantCulture)).Distinct().ToArray(), args));
            }

            for (var i = 0; i < slots.Length; i++)
            {
                Add([slots[i]]);
                for (var j = i + 1; j < slots.Length; j++)
                    Add([slots[i], slots[j]]);
            }
        }
        else if (r.Stage == BoundaryLiuFengStage.Targets)
            foreach (var target in BoundaryLiuFengCurrentTargets(f))
                result.Add(new(new($"boundary-liu-feng.{(f.Id)}.target.{(target)}"), "侵凌：对" + _coreMatchParticipantsPlayersAccess._players[target].Name + "使用杀", [], [target], Args("target")));
        else if (r.Stage == BoundaryLiuFengStage.Cost)
            foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard))
            {
                if (!BoundaryLiuFengCostLegal(f, old))
                    continue;
                var args = new Dictionary<string, string>(old.Parameters);
                foreach (var pair in Args("cost"))
                    args[pair.Key] = pair.Value;
                result.Add(old with { Id = new($"boundary-liu-feng.{(f.Id)}.cost.{(old.Id.Value)}"), Parameters = args });
            }

        if (r.Stage is BoundaryLiuFengStage.Collect or BoundaryLiuFengStage.Targets or BoundaryLiuFengStage.Cost)
            result.Add(new(new($"boundary-liu-feng.{(f.Id)}.cancel"), r.Stage == BoundaryLiuFengStage.Collect ? "不放置逆" : "取消侵凌", [], [], Args("cancel")));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal bool BoundaryLiuFengCostLegal(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.BoundaryLiuFeng!;
        if (r.Targets is not [var target] || !BoundaryLiuFengCurrentTargets(f).Contains(target))
            return false;
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var from = new CardLocation(zone, f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        if (zone != CardZoneKind.Equipment)
            return true;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var victim = _coreMatchParticipantsPlayersAccess._players[target];
        var n = r.Need;
        if (n?.Intent is BoundaryLiuFengIntent.BorrowedSword or BoundaryLiuFengIntent.Qinglong && _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon)
            return false;
        if (n?.Intent == BoundaryLiuFengIntent.Qinglong)
            return true;
        if (n?.Intent == BoundaryLiuFengIntent.BorrowedSword)
            return _coreProgramDistanceRulesOperations.DistanceRules.AllowsSlashDistance(ProgramDistanceStage.BorrowedSlash, new(actor, victim, CardKind.Slash, PhysicalCardIds: [], PhysicalSuit: Suit.None, EffectiveRank: 0, ExcludedEquipmentId: card.Id));
        var parent = n is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == n.OwnerFrameId);
        var ignore = n?.Intent is BoundaryLiuFengIntent.ProgramSlash or BoundaryLiuFengIntent.ProgramNearestSlash || parent is not null && (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent) || _characterWuJingOlOperations.IsWuJingHejiRequest(parent) || n?.Intent == BoundaryLiuFengIntent.FixedSlash && parent.FixedTargetSlash?.IgnoreDistance == true);
        return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, victim, new(0, CardKind.Slash, Suit.None, 0), ignoreDistance: ignore, noEffectiveRank: true, physicalCardIds: [], excludedEquipmentId: card.Id) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, victim, ignoresCount: !BoundaryLiuFengCountsSlash(f), effectiveRank: 0, excludedEquipmentId: card.Id, physicalCardIds: []);
    }

    internal void PublishBoundaryLiuFeng(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            CancelBoundaryLiuFeng(f);
            return;
        }

        var choices = BoundaryLiuFengChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryLiuFeng!.Stage == BoundaryLiuFengStage.Collect ? "陷嗣：将至多两张手牌或装备牌置为逆，可以来自同一角色。" : f.BoundaryLiuFeng.Stage == BoundaryLiuFengStage.Targets ? "侵凌：选择杀的目标。" : "侵凌：真实弃置一张手牌或装备牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = f.OwnerSeat,
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryLiuFengAction)]
    internal void AnswerBoundaryLiuFeng(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Liu Feng lost its owning frame.");
        if (f.BoundaryLiuFeng is not { } r || !ValidBoundaryLiuFeng(f) || !BoundaryLiuFengChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Liu Feng changed its published real card slots or original target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch == "cancel")
        {
            CancelBoundaryLiuFeng(f);
            return;
        }

        if (branch == "target")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r with { Targets = selected.Targets, Stage = BoundaryLiuFengStage.Cost } });
            PublishBoundaryLiuFeng(f);
            return;
        }

        if (branch == "collect")
        {
            var count = int.Parse(selected.Parameters["count"], CultureInfo.InvariantCulture);
            var ids = Enumerable.Range(0, count).Select(i =>
            {
                var seat = int.Parse(selected.Parameters[$"{(i)}.card-owner-seat"], CultureInfo.InvariantCulture);
                var zone = Enum.Parse<CardZoneKind>(selected.Parameters[$"{(i)}.source-zone"]);
                return _cardZonesOperations._cardZones.CardsAt(new(zone, seat))[int.Parse(selected.Parameters[$"{(i)}.slot-index"], CultureInfo.InvariantCulture)].Id;
            }).ToArray();
            BeginBoundaryLiuFengMovement(f, ids, CardLocation.Authority(f.OwnerSeat), "collect");
            return;
        }

        if (branch != "cost" || BoundaryLiuFengPaidThisTurn(f.OwnerSeat) || !r.Targets.All(BoundaryLiuFengCurrentTargets(f).Contains))
            throw new InvalidOperationException("Qinling lost its unpaid actual-turn allowance or native target.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r with { Cards = [card.Id], From = [from], Movement = "cost", Before = before, After = before, Stage = BoundaryLiuFengStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new("skill-program.boundary:qinling.cost"), (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var m = moves.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { BoundaryLiuFeng = current.BoundaryLiuFeng!with { Moves = moves, After = m.Sequence, CostSequence = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengCostPaidEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, card.Id, from, m.Sequence));
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = f.BoundaryLiuFeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueBoundaryLiuFeng(f.Id);
    }

    internal void BeginBoundaryLiuFengMovement(ProgramSkillFrame f, int[] ids, CardLocation destination, string movement)
    {
        var r = f.BoundaryLiuFeng!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r with { Cards = ids, From = ids.Select(id => _cardZonesOperations._cardZones.GetLocation(id)).ToArray(), Before = before, After = before, Movement = movement, Stage = BoundaryLiuFengStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new("skill-program.boundary:liu-feng." + movement), (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { BoundaryLiuFeng = current.BoundaryLiuFeng!with { Moves = moves, After = moves.Max(m => m.Sequence) } });
        }, disclosure: CardMovementDisclosure.FaceUp);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = f.BoundaryLiuFeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengMovementEvent(f.Id, movement, ids.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryLiuFeng(f.Id);
    }

    internal bool DrainBoundaryLiuFeng(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueBoundaryLiuFeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiuFeng: { } r } f || f.Id != id)
            return;
        if (!ValidBoundaryLiuFeng(f))
            throw new InvalidOperationException("Liu Feng lost its once-paid native continuation.");
        if (r.Stage is BoundaryLiuFengStage.Collect or BoundaryLiuFengStage.Targets or BoundaryLiuFengStage.Cost)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBoundaryLiuFeng(f);
            return;
        }

        if (r.Stage is BoundaryLiuFengStage.NativeUse or BoundaryLiuFengStage.RewardDraw)
            return;
        if (r.Stage is BoundaryLiuFengStage.MovementChildren or BoundaryLiuFengStage.NativeTail)
        {
            if (DrainBoundaryLiuFeng(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.BoundaryLiuFeng!;
            if (r.Stage == BoundaryLiuFengStage.NativeTail && !r.ConsequencePaid)
            {
                SettleBoundaryLiuFeng(f);
                return;
            }

            if (r.Movement == "cost" && r.UseId == 0 && r.CostSequence > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Targets.All(BoundaryLiuFengCurrentTargets(f).Contains))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, BoundaryLiuFeng = r with { Stage = BoundaryLiuFengStage.Issuing } });
                PrepareBoundaryLiuFengRequest(f);
                IssueBoundaryLiuFengSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            FinishBoundaryLiuFeng(f);
        }
    }

    internal void CompleteBoundaryLiuFengFrame(ProgramSkillFrame f)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = f.BoundaryLiuFeng!with { Stage = BoundaryLiuFengStage.Completed }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void FinishBoundaryLiuFeng(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiuFeng!;
        CompleteBoundaryLiuFengFrame(f);
        if (r.Need is not null)
        {
            if (r.UseId > 0 && r.Returned)
                ResumeBoundaryLiuFengRequest(r);
            else if (r.CostSequence > 0)
                RepublishBoundaryLiuFengRequest(r);
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 50200)]
    internal bool ResumeBoundaryLiuFeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiuFeng: { Stage: not BoundaryLiuFengStage.Completed } } f || f.Id != id)
            return false;
        ContinueBoundaryLiuFeng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 50200)]
    internal bool ReturnBoundaryLiuFengMovement(ProgramSkillFrame f) => f.BoundaryLiuFeng is not null && ResumeBoundaryLiuFeng(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 50200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 50200)]
    internal bool KeepBoundaryLiuFengPaid(ProgramSkillFrame f) => f.BoundaryLiuFeng is { Stage: not (BoundaryLiuFengStage.Collect or BoundaryLiuFengStage.Targets or BoundaryLiuFengStage.Cost) } && ValidBoundaryLiuFeng(f);
}
