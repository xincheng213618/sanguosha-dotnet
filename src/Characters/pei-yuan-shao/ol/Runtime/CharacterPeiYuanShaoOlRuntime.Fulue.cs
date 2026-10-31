#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPeiYuanShaoOlRuntime
{
    internal const string FulueObtainReason = "skill-program.pei-yuan-shao.fulue.obtain";
    internal const string FulueCounterBind = "fulue-counter";
    internal static FulueOption OtherFulueOption(FulueOption option) => RuntimeSkills.RuntimeRules_pei_yuan_shao_ol_Fulue.OtherFulueOption(option);
    internal bool FulueOptionAvailable(CardConversionSource source, int turn, FulueOption option) => !_coreProgramEventHistoryOperations.ProgramEventHistory<FulueOptionExecutedEvent>().Any(e => e.Entry.Source.OwnerSeat == source.OwnerSeat && e.Entry.Source.SkillId == source.SkillId && e.Entry.ActualTurn == turn && e.Entry.Option == option);
    internal static bool FulueCardKind(CardKind kind) => RuntimeSkills.RuntimeRules_pei_yuan_shao_ol_Fulue.FulueCardKind(kind);
    internal CardUseFrame? FulueContextUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.CardUse is not { } card || context.OwnerSeat != c.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != card.CardActionId || !FulueCardKind(use.CardKind) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != use.Id || window.Action.ActionId != action.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var current = window.Candidates[window.CandidateIndex];
        return current.OwnerSeat == c.OwnerSeat && current.SkillId == c.SkillId && current.TriggerId == c.BindingId && current.SkillInstanceId == c.SkillInstanceId && current.GameplayHash == c.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) == context ? use : null;
    }

    internal bool FulueOriginalCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || FulueContextUse(c, context)is not { } use || use.SourceSeat != c.OwnerSeat || use.TargetSeats is not [var target] || target == c.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || use.Action!.EffectiveDesignatedTargetSeats.Distinct().Count() != 1 || !use.Action.EffectiveDesignatedTargetSeats.Contains(target) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) || use.FulueEntries?.Any(e => !e.Counter && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId) == true)
            return false;
        var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
        return FulueOptionAvailable(source, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, FulueOption.Damage) || FulueOptionAvailable(source, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, FulueOption.Obtain) && HasFulueObtainableCard(c.OwnerSeat, target);
    }

    internal bool FulueUseProof(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action || use.FulueEntries is not { Count: > 0 } entries || !FulueCardKind(use.CardKind) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action) || entries.Distinct().Count() != entries.Count)
            return false;
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<FulueOptionExecutedEvent>().Where(e => e.Entry.UseId == use.Id).Select(e => e.Entry).ToArray();
        return entries.SequenceEqual(facts) && entries.All(e => e.UseId == use.Id && e.ActionId == action.ActionId && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.UserSeat != e.TargetSeat && e.ProgramFrameId > use.Id && Enum.IsDefined(e.Option) && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, e.UserSeat, action.ProviderSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(b => b.FrameId == e.ProgramFrameId && b.SkillId == e.Source.SkillId && b.SkillInstanceId == e.Source.SkillInstanceId && b.OwnerSeat == e.Source.OwnerSeat && b.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && (e.Counter ? use.FulueCounter is { } counter && counter.Source == e.Source && counter.OtherOption == e.Option && counter.ActorSeat == e.UserSeat && counter.TargetSeat == e.TargetSeat : use.Action.EffectiveDesignatedTargetSeats.SequenceEqual([e.TargetSeat]) && e.Source.OwnerSeat == e.UserSeat));
    }

    internal bool FulueZeroDamage(CardUseFrame use) => !_coreProgramEventHistoryOperations.ProgramEventHistory<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.CardUseFrameId == use.Id && e.Amount > 0);
    internal FulueUseEntry? FulueOriginalEntry(CardUseFrame use, int owner, string skill, string instance) => use.FulueEntries?.SingleOrDefault(e => !e.Counter && e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance);
    internal bool IsIssuedFulueCandidate(ProgramTriggerCandidate c) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(c.SkillId)?.Program?.Triggers.Any(t => t.Id == c.BindingId && t.Effects.FirstOrDefault()?.Op is SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash or SkillProgramEffectOp.ResolveFulueCounterOption) == true;
    internal bool CanRunIssuedFulueCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedFulueCandidate(c) || FulueContextUse(c, context)is not { } use)
            return false;
        var op = _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects[0].Op;
        if (op == SkillProgramEffectOp.ResolveFulueCounterOption)
            return context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && use.FulueCounter is { Applied: false } r && ValidFulueCounterInvoice(use, r) && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillId == c.SkillId && r.Source.SkillInstanceId == c.SkillInstanceId && r.GameplayHash == c.GameplayHash && FulueOptionAvailable(r.Source, r.ActualTurn, r.OtherOption);
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted && use.Step == ResolutionFrameStep.Completed && FulueUseProof(use) && FulueZeroDamage(use) && FulueOriginalEntry(use, c.OwnerSeat, c.SkillId, c.SkillInstanceId)is { } entry && _coreMatchParticipantsPlayersAccess._players[entry.UserSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[entry.TargetSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<FulueCounterRequestedEvent>().Any(e => e.Invoice.OriginalUseId == use.Id && e.Invoice.Source == entry.Source) && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 13)]
    internal void CollectIssuedFulueCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Action?.ActionId != action.ActionId)
            return;
        var issued = new List<(CardConversionSource Source, string Hash, SkillProgramEffectOp Op)>();
        if (window == SkillProgramTriggerWindow.CardUseCompleted && use.FulueEntries is { } entries && FulueUseProof(use) && FulueZeroDamage(use))
            issued.AddRange(entries.Where(e => !e.Counter && _coreMatchParticipantsPlayersAccess._players[e.UserSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[e.TargetSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<FulueCounterRequestedEvent>().Any(f => f.Invoice.OriginalUseId == use.Id && f.Invoice.Source == e.Source)).Select(e => (e.Source, e.GameplayHash, SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash)));
        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized && use.FulueCounter is { Applied: false } counter && ValidFulueCounterInvoice(use, counter) && FulueOptionAvailable(counter.Source, counter.ActualTurn, counter.OtherOption))
            issued.Add((counter.Source, counter.GameplayHash, SkillProgramEffectOp.ResolveFulueCounterOption));
        foreach (var(source, hash, op)in issued)
        {
            var p = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
            var trigger = p.Triggers.Single(t => t.Effects[0].Op == op);
            if (result.Any(c => c.OwnerSeat == source.OwnerSeat && c.SkillId == source.SkillId && c.SkillInstanceId == source.SkillInstanceId && c.TriggerId == trigger.Id))
                continue;
            result.Add(new(source.OwnerSeat, source.OwnerSeat, source.SkillId, trigger.Id, hash, source.SkillInstanceId, trigger.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, source.OwnerSeat, source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], action))));
        }
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27120)]
    internal bool QualifyFulue(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => PeiYuanShaoComposition.Handles(e.Op)) || (t.Effects[0].Op == SkillProgramEffectOp.OfferFulueSingleTargetOption ? FulueOriginalCandidate(c, context) : CanRunIssuedFulueCandidate(c, context));
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 150)]
    internal int FulueDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { FulueEntries: not null } use && FulueUseProof(use) ? use.FulueEntries!.Count(e => e.Option == FulueOption.Damage) : 0;
    SkillProgramStepOutcome IPeiYuanShaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginFulue(f, op);
    internal SkillProgramStepOutcome BeginFulue(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext ?? throw new InvalidOperationException("Fulue requires its genuine native card window.");
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.Fulue is not null || f.InstructionIndex != 1 || !(op == SkillProgramEffectOp.OfferFulueSingleTargetOption ? FulueOriginalCandidate(c, context) : CanRunIssuedFulueCandidate(c, context)))
            throw new InvalidOperationException("Fulue lost its exact unconsumed native candidate.");
        var use = FulueContextUse(c, context)!;
        var action = use.Action!;
        if (op == SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash)
        {
            var entry = FulueOriginalEntry(use, f.OwnerSeat, f.SkillId, f.SkillInstanceId)!;
            var invoice = new FulueCounterInvoice(f.Id, use.Id, action.ActionId, entry.Source, entry.GameplayHash, entry.ActualTurn, entry.TurnOwner, entry.TargetSeat, entry.UserSeat, OtherFulueOption(entry.Option));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Fulue = new(use.Id, action.ActionId, entry.Source, f.GameplayHash, entry.ActualTurn, entry.TurnOwner, entry.UserSeat, entry.TargetSeat, false, FulueStage.CounterRequest) { Option = entry.Option }, SelectedTargetSeats = Array.AsReadOnly(new[] { entry.TargetSeat }) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FulueCounterRequestedEvent(f.Id, invoice));
            return SkillProgramStepOutcome.Continue;
        }

        var counter = op == SkillProgramEffectOp.ResolveFulueCounterOption;
        var source = counter ? use.FulueCounter!.Source : new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var user = counter ? use.FulueCounter!.ActorSeat : action.ActorSeat;
        var target = counter ? use.FulueCounter!.TargetSeat : use.TargetSeats.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fulue = new(use.Id, action.ActionId, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, user, target, counter, FulueStage.Option) });
        if (counter)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { FulueCounter = u.FulueCounter!with { Applied = true } });
            ExecuteFulueOption(f, use.FulueCounter!.OtherOption);
        }
        else
            PublishFulue(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> FulueObtainChoices(long frame, int user, int target) => _coreMatchParticipantsPlayersAccess._players[user].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame, user, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain) : [];
    internal bool HasFulueObtainableCard(int user, int target) => _coreMatchParticipantsPlayersAccess._players[user].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(target)).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment2(target)).Any(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(user, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Obtain));
    internal IReadOnlyList<PromptChoice> FulueChoices(ProgramSkillFrame f)
    {
        var r = f.Fulue!;
        var list = new List<PromptChoice>();
        if (r.Stage == FulueStage.Obtain)
            return FulueObtainChoices(f.Id, r.UserSeat, r.TargetSeat).Select(c => c with { Id = new("fulue." + c.Id.Value), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "fulue", ["branch"] = "obtain" } }).ToArray();
        if (r.Stage != FulueStage.Option)
            return[];
        foreach (var option in Enum.GetValues<FulueOption>().Where(o => FulueOptionAvailable(r.Source, r.ActualTurn, o) && (o != FulueOption.Obtain || HasFulueObtainableCard(r.UserSeat, r.TargetSeat))))
            list.Add(new(new($"fulue.{(f.Id)}.{(option)}"), option == FulueOption.Damage ? "此牌造成伤害+1" : "获得目标角色的一张牌", [], [], new Dictionary<string, string> { { "program-action", "fulue" }, { "branch", option.ToString() } }));
        list.Add(new(new($"fulue.{(f.Id)}.skip"), "不发动复掠", [], [], new Dictionary<string, string> { { "program-action", "fulue" }, { "branch", "skip" } }));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishFulue(ProgramSkillFrame f)
    {
        var r = f.Fulue!;
        var choices = FulueChoices(f);
        if (choices.Count == 0)
        {
            FinishFulue(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.UserSeat, r.Stage == FulueStage.Obtain ? "复掠：获得目标角色的一张牌。" : "复掠：选择本回合未执行的一项。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == FulueStage.Obtain,
            TargetSeat = r.UserSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.UserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ExecuteFulueOption(ProgramSkillFrame f, FulueOption option)
    {
        var r = f.Fulue!;
        if (!FulueOptionAvailable(r.Source, r.ActualTurn, option))
            throw new InvalidOperationException("Fulue cannot execute either original owner's option twice in one actual turn.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)!;
        var entry = new FulueUseEntry(f.Id, use.Id, r.ActionId, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.UserSeat, r.TargetSeat, option, r.Counter);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { FulueEntries = (u.FulueEntries ?? []).Append(entry).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FulueOptionExecutedEvent(entry));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fulue = r with { Option = option, Stage = option == FulueOption.Obtain ? FulueStage.Obtain : FulueStage.Option } });
        if (option == FulueOption.Obtain && HasFulueObtainableCard(r.UserSeat, r.TargetSeat))
            PublishFulue(f);
        else
            FinishFulue(f);
    }

    [ProgramAnswer("fulue")]
    internal void AnswerFulue(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fulue: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.UserSeat || !FulueChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Fulue needs its exact original user's current published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == FulueStage.Option)
        {
            if (choice.Parameters["branch"] == "skip")
                FinishFulue(f);
            else
                ExecuteFulueOption(f, Enum.Parse<FulueOption>(choice.Parameters["branch"]));
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"]);
        var from = new CardLocation(zone, r.TargetSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        var material = new CardActionCost(card.Id, card.Kind, from);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fulue = r with { Stage = FulueStage.MovementChildren, Obtained = material, Before = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(r.UserSeat), new(FulueObtainReason));
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card.Id && m.Reason.Value == FulueObtainReason);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fulue = f.Fulue!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, MoveSequence = move.Sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FulueObtainPaidEvent(f.Id, r.UseId, r.ActionId, r.UserSeat, material, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, move.Sequence));
        ContinueFulue(f.Id);
    }

    internal void FinishFulue(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fulue = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal void ContinueFulue(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fulue: { Stage: FulueStage.MovementChildren } } f || f.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishFulue(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27120)]
    internal bool ResumeFulue(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fulue: { Stage: FulueStage.MovementChildren } } f || f.Id != id)
            return false;
        ContinueFulue(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27120)]
    internal bool ReturnFulueMovement(ProgramSkillFrame f)
    {
        if (f.Fulue?.Stage != FulueStage.MovementChildren)
            return false;
        ContinueFulue(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferFulueSingleTargetOption, SkillProgramEffectOp.ResolveFulueCounterOption)]
    internal PromptChoice ChooseFulueAi(PendingDecision p, ProgramSkillFrame f) => f.Fulue?.Stage == FulueStage.Obtain ? p.Choices.OrderByDescending(c => c.Cards.Count > 0 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.Fulue.TargetSeat]) : 2).First() : p.Choices.First(c => c.Parameters["branch"] != "skip");
    [ProgramIssuedTrigger(590, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger590(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedFulueCandidate(candidate) ? CanRunIssuedFulueCandidate(candidate, context) : null;
}
