#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileGodRuntime
{
    internal IReadOnlyList<PromptChoice> ShenLsChoices(ProgramSkillFrame f)
    {
        var r = f.ShenLuSu!;
        PromptChoice C(string option, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new("shen-lu-su." + f.Id + "." + option), label, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", "shen-lu-su" }, { "option", option }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } });
        if (r.Stage == ShenLuSuStage.SeatChoice)
            return r.OriginalOrder.Except(r.Order).Select(s => C("seat:" + s, "下一座次：“" + _coreMatchParticipantsPlayersAccess._players[s].Name + "”", targets: [s])).ToArray();
        if (r.Stage == ShenLuSuStage.PeerChoice)
            return ShenLsPeers(f.OwnerSeat, f.SkillId).Select(s => C("peer:" + s, "与“" + _coreMatchParticipantsPlayersAccess._players[s].Name + "”随机平分双方手牌", targets: [s])).ToArray();
        if (r.Stage == ShenLuSuStage.GiftChoice)
            return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Where(c => c.Cards is [var id] && ShenLsGiveable(f.OwnerSeat).Contains(id) && !r.Cards.Contains(id)).Select(c => C("give:" + c.Cards.Single(), c.Description, c.Cards)).ToArray();
        return[];
    }

    internal void PublishShenLs(ProgramSkillFrame f)
    {
        var choices = ShenLsChoices(f);
        if (choices.Count == 0)
        {
            FinishShenLs(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shen-lu-su")]
    internal void AnswerShenLs(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Shen Lu Su lost his active choice parent.");
        var r = f.ShenLuSu ?? throw new InvalidOperationException("Shen Lu Su lost his issued choice.");
        var current = ShenLsChoices(f).SingleOrDefault(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) && c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(chosen.Parameters.OrderBy(x => x.Key))) ?? throw new InvalidOperationException("Shen Lu Su requires the exact current private entity or public participant.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == ShenLuSuStage.SeatChoice)
        {
            var seat = current.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuSeatChosenEvent(f.Id, r.Order.Count, seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenLuSu = r with { Order = Array.AsReadOnly(r.Order.Append(seat).ToArray()) } });
        }
        else if (r.Stage == ShenLuSuStage.GiftChoice)
        {
            var card = current.Cards.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuGiftChosenEvent(f.Id, r.Cards.Count));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenLuSu = r with { Cards = Array.AsReadOnly(r.Cards.Append(card).ToArray()) } });
        }
        else if (r.Stage == ShenLuSuStage.PeerChoice)
        {
            DivideShenLs(f, current.Targets.Single());
            return;
        }
        else
            throw new InvalidOperationException("Shen Lu Su cannot answer a paid movement.");
        ContinueShenLs(f.Id);
    }

    internal void DivideShenLs(ProgramSkillFrame f, int target)
    {
        if (!ShenLsPeers(f.OwnerSeat, f.SkillId).Contains(target))
            throw new InvalidOperationException("Zhimeng lost its legal other participant.");
        var pool = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target])).Select(c => c.Id).ToArray();
        var random = pool.ToArray();
        for (var i = random.Length - 1; i > 0; i--)
        {
            var j = _randomOperations._random.Next(i + 1);
            (random[i], random[j]) = (random[j], random[i]);
        }

        var ownerShare = random.Take((random.Length + 1) / 2).ToHashSet();
        var division = pool.Select((id, i) => new ShenLuSuDivisionCard(i, id, _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat!.Value, ownerShare.Contains(id) ? f.OwnerSeat : target)).ToArray();
        var r = f.ShenLuSu!with
        {
            Target = target,
            Amount = pool.Length,
            Cards = Array.AsReadOnly(pool),
            OwnerShare = Array.AsReadOnly(random.Take((random.Length + 1) / 2).ToArray()),
            Division = Array.AsReadOnly(division),
            Stage = ShenLuSuStage.DivideChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenLuSu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuDivisionEvent(f.Id, target, pool.Length, ownerShare.Count));
        var transfers = pool.Where(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != (ownerShare.Contains(id) ? f.OwnerSeat : target)).ToArray();
        if (transfers.Length == 0)
        {
            FinishShenLs(f);
            return;
        }

        MoveShenLs(f, transfers, ShenLuSuStage.DivideChildren, CardLocation.Hand(f.OwnerSeat), (c, _) => CardLocation.Hand(ownerShare.Contains(c.Id) ? f.OwnerSeat : target));
        ContinueShenLs(f.Id);
    }

    internal static int[] ShenLsRotate(IReadOnlyList<int> order, int first)
    {
        var index = CoreGameEngineTableOrderRuntime.IndexOfTableSeat(order, first);
        return order.Skip(index).Concat(order.Take(index)).ToArray();
    }

    internal void ApplyShenLsOrder(ProgramSkillFrame f)
    {
        var r = f.ShenLuSu!;
        if (!ShenLsMode || r.Stage != ShenLuSuStage.SeatChoice || f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } w || !_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(p => p.Id == w.ParentFrameId && p.Window == w.Window) || !_coreTableOrderOperations.CurrentTableOrder().SequenceEqual(r.OriginalOrder) || r.Order.Count != r.OriginalOrder.Count || r.Order.Distinct().Count() != r.Order.Count || !r.Order.Order().SequenceEqual(r.OriginalOrder.Order()) || r.FixedSeat != (_coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ModeKind == ContentModeKind.Identity ? _coreMatchParticipantsPlayersAccess._players.Single(p => p.Role == Role.Lord).Seat : -1) || r.FixedSeat >= 0 && (_coreMatchParticipantsPlayersAccess._players[r.FixedSeat].Role != Role.Lord || r.Order[0] != r.FixedSeat))
            throw new InvalidOperationException("Tamo lost its frozen complete permutation or fixed lord position.");
        // Insert each chosen participant behind its frozen predecessor. The
        // identity lord is the anchor and is never the moved participant.
        for (var i = 1; i < r.Order.Count; i++)
        {
            var seat = r.Order[i];
            var previous = r.Order[i - 1];
            var remaining = _coreTableOrderOperations.CurrentTableOrder().Where(s => s != seat).ToArray();
            var next = remaining[(Array.IndexOf(remaining, previous) + 1) % remaining.Length];
            if (_coreTableOrderOperations.TableStepSeat(previous, 1) == seat)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TableSeatMovedEvent(seat, previous, next, f.Id));
        }

        if (r.FixedSeat < 0)
            while (_coreTableOrderOperations.CurrentTableOrder()[0] != r.Order[0])
            {
                var current = _coreTableOrderOperations.CurrentTableOrder();
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TableSeatMovedEvent(current[0], current[^1], current[1], f.Id));
            }

        if (!ShenLsRotate(_coreTableOrderOperations.CurrentTableOrder(), r.Order[0]).SequenceEqual(r.Order))
            throw new InvalidOperationException("Tamo did not install its complete real table order.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuSeatsAppliedEvent(f.Id, r.ParentId, f.OwnerSeat, r.Order[0], r.Order.Count));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 38900)]
    internal bool CompleteShenLsSetup(ProgramLifecycleTriggerWindowFrame frame)
    {
        if (!ShenLsTeam || frame.Window != SkillProgramTriggerWindow.GameStarting || frame.Continuation != ProgramLifecycleContinuation.CompleteGameStarting || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.NotStarted || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber != 0 || _resolutionStackOperations._resolutionStack.Any(p => p.Id == frame.Id))
            return false;
        var applied = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenLuSuSeatsAppliedEvent>().LastOrDefault(e => e.WindowId == frame.Id);
        if (applied is null)
            return false;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenLuSuStartedEvent>().Where(e => e.FrameId == applied.FrameId).ToArray();
        if (frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || applied.Count != _coreMatchDefinitionModeDefinitionPlayerCountAccess._playerCount || _coreTableOrderOperations.CurrentTableOrder()[0] != applied.FirstSeat || issued is not [var source] || source.Operation != SkillProgramEffectOp.ResolveShenLuSuTamo || source.ParentId != frame.Id || source.Source.OwnerSeat != applied.OwnerSeat || source.Source.SkillId is not (ShenLuSuComposition.Tamo or "happy:tamo") || source.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(source.Source.SkillId).Program!.GameplayHash || !frame.Candidates.Any(c => c.OwnerSeat == applied.OwnerSeat && c.SkillId == source.Source.SkillId && c.SkillInstanceId == source.Source.SkillInstanceId && c.BindingId == source.Source.BindingId && c.GameplayHash == source.GameplayHash) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingResolvedEvent>().Count(e => e.FrameId == applied.FrameId && e.OwnerSeat == applied.OwnerSeat && e.SkillId == source.Source.SkillId && e.SkillInstanceId == source.Source.SkillInstanceId && e.BindingId == "redistribute-seats" && e.Activated && e.Completed) != 1)
            throw new InvalidOperationException("Tamo lost its completed original setup permutation before selecting the real first actor.");
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat = applied.FirstSeat;
        return true;
    }

    internal static PromptChoice AiShenLs(PendingDecision p) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenLuSuTamo)]
    internal PromptChoice AiShenLsTamo(PendingDecision p, ProgramSkillFrame f) => AiShenLs(p);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenLuSuDingzhou)]
    internal PromptChoice AiShenLsDingzhou(PendingDecision p, ProgramSkillFrame f) => AiShenLs(p);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenLuSuZhimeng)]
    internal PromptChoice AiShenLsZhimeng(PendingDecision p, ProgramSkillFrame f) => AiShenLs(p);
}
