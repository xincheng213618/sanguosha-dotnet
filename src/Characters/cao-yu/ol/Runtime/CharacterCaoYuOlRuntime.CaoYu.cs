#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal const string GongjieGiftReason = "skill-program.cao-yu.gongjie.obtain", GongjieDrawReason = "skill-program.cao-yu.gongjie.draw", XiangxuDrawReason = "skill-program.cao-yu.xiangxu.draw", XiangxuDiscardReason = "skill-program.cao-yu.xiangxu.discard", XiangzuGiftReason = "skill-program.cao-yu.xiangzu.give";
    SkillProgramStepOutcome ICaoYuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCaoYu(f, op);
    internal IReadOnlyList<Card> CaoYuGiftCards(int owner, int chooser) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).OrderBy(c => _cardZonesOperations._cardZones.GetLocation(c.Id).Zone).ThenBy(c => c.Id).ToArray());
    internal IReadOnlyList<Card> CaoYuDiscardCards(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard)).OrderBy(c => c.Id).ToArray());
    internal SkillProgramStepOutcome BeginCaoYu(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
        if (f.CaoYu is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !QualifyCaoYu(c, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("CaoYu requires its exact original round, causal minimum, issued ending or dying candidate.");
        if (op == SkillProgramEffectOp.ResolveXiangxuMinimumTransition)
        {
            var edge = CaoYuTransition(c, context.CaoYuOpportunityId)!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumArmedEvent(f.Id, edge.OpportunityId, edge.Source, edge.GameplayHash));
            return SkillProgramStepOutcome.Continue;
        }

        var dying = 0L;
        if (op == SkillProgramEffectOp.ResolveXiangzuDyingGiftAndRecovery)
        {
            if (!CaoYuDyingEntry(f, out var d))
                throw new InvalidOperationException("Xiangzu lost its genuine original dying occurrence.");
            dying = d.Id;
        }

        var stage = op switch
        {
            SkillProgramEffectOp.ResolveGongjieRoundFirstGifts => CaoYuStage.GongjieTargets,
            SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment => CaoYuStage.XiangxuChoice,
            _ => CaoYuStage.XiangzuRecipient
        };
        var r = new CaoYuReceipt(CaoYuSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, context.CaoYuOpportunityId ?? 0, dying, stage, [], [], [], []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.Round, r.OpportunityId, r.DyingId));
        PublishCaoYu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CaoYuChoices(ProgramSkillFrame f)
    {
        var r = f.CaoYu!;
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "cao-yu",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        PromptChoice C(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string suffix = "") => new(new($"cao-yu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(suffix)}"), label, cards ?? [], targets ?? [], Args(branch));
        if (r.Stage == CaoYuStage.GongjieTargets)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
            {
                var selected = r.Targets.Contains(p.Seat);
                choices.Add(C(selected ? "unselect-target" : "target", (selected ? "取消" : "选择") + p.Name, targets: [p.Seat], suffix: p.Seat.ToString(CultureInfo.InvariantCulture)));
            }

            if (r.Targets.Count > 0)
                choices.Add(C("targets-done", "确认受益者"));
            choices.Add(C("cancel", "取消恭节"));
        }
        else if (r.Stage == CaoYuStage.GongjieTake)
        {
            var target = r.Targets[r.Index];
            foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, target, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => !card.IsGeneralWeapon, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: target))
                choices.Add(old with { Id = new($"cao-yu.{(f.Id)}.take.{(old.Id.Value)}"), Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>(old.Parameters) { ["program-action"] = "cao-yu", ["branch"] = "take" }) });
        }
        else if (r.Stage == CaoYuStage.XiangxuChoice)
        {
            var own = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            var other = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TurnOwner]).Count;
            if (other >= own || CaoYuDiscardCards(f.OwnerSeat).Count >= own - other)
                choices.Add(C("adjust", "按当前回合角色的手牌调整"));
            choices.Add(C("decline", "放弃这一次相胥"));
        }
        else if (r.Stage == CaoYuStage.XiangxuDiscardChoice)
        {
            foreach (var card in CaoYuDiscardCards(f.OwnerSeat))
            {
                var selected = r.Selected.Contains(card.Id);
                if (!selected && r.Selected.Count >= r.Requested)
                    continue;
                choices.Add(C(selected ? "unpick" : "pick", (selected ? "取消" : "选择") + card.DisplayName, [card.Id], suffix: card.Id.ToString(CultureInfo.InvariantCulture)));
            }

            if (r.Selected.Count == r.Requested)
                choices.Add(C("discard", "确认整次弃牌"));
        }
        else if (r.Stage == CaoYuStage.XiangzuRecipient)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                choices.Add(C("recipient", "交给" + p.Name, targets: [p.Seat], suffix: p.Seat.ToString(CultureInfo.InvariantCulture)));
            choices.Add(C("cancel", "取消襄胙"));
        }
        else if (r.Stage == CaoYuStage.XiangzuCards)
        {
            foreach (var card in CaoYuGiftCards(f.OwnerSeat, f.OwnerSeat))
            {
                var selected = r.Selected.Contains(card.Id);
                choices.Add(C(selected ? "unpick" : "pick", (selected ? "取消" : "选择") + card.DisplayName, [card.Id], suffix: card.Id.ToString(CultureInfo.InvariantCulture)));
            }

            if (r.Selected.Count > 0)
                choices.Add(C("give", "确认交牌"));
            choices.Add(C("cancel", "取消襄胙"));
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishCaoYu(ProgramSkillFrame f)
    {
        var r = f.CaoYu!;
        if (r.Stage == CaoYuStage.GongjieTake && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !r.Paid && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || r.Index >= r.Targets.Count))
        {
            AfterGongjieGifts(f);
            return;
        }

        if (r.Stage == CaoYuStage.GongjieTake && (!_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Index]].IsAlive || CaoYuGiftCards(f.OwnerSeat, r.Targets[r.Index]).Count == 0))
        {
            AdvanceGongjieDonor(f);
            return;
        }

        var chooser = r.Stage == CaoYuStage.GongjieTake ? r.Targets[r.Index] : f.OwnerSeat;
        var choices = CaoYuChoices(f);
        if (choices.Count == 0)
        {
            FinishCaoYu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage != CaoYuStage.GongjieTargets && r.Stage != CaoYuStage.XiangzuRecipient,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("cao-yu")]
    internal void AnswerCaoYu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoYu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == CaoYuStage.GongjieTake ? r.Targets[r.Index] : f.OwnerSeat) || !CaoYuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("CaoYu requires its actual chooser and exact current opaque slot or private material set.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "cancel")
        {
            FinishCaoYu(f);
            return;
        }

        if (branch is "target" or "unselect-target")
        {
            var seat = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuTargetChoiceEvent(f.Id, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoYuTargetChoiceEvent>().Count(e => e.FrameId == f.Id), seat, branch == "target"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Targets = branch == "target" ? r.Targets.Append(seat).ToArray() : r.Targets.Where(s => s != seat).ToArray() } });
            PublishCaoYu(f);
            return;
        }

        if (branch == "targets-done")
        {
            var targets = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(r.TurnOwner, s)).ToArray();
            for (var i = 0; i < targets.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuTargetFrozenEvent(f.Id, i, targets[i]));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Targets = targets, Stage = CaoYuStage.GongjieTake } });
            PublishCaoYu(f);
            return;
        }

        if (branch == "take")
        {
            if (!Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
                throw new InvalidOperationException("Gongjie lost the original source slot.");
            var card = _cardZonesOperations._cardZones.CardsAt(new(zone, f.OwnerSeat))[slot];
            PayCaoYuCards(f, [card.Id], r.Targets[r.Index], CaoYuStage.GongjieGiftChildren, GongjieGiftReason);
            ContinueCaoYu(f.Id);
            return;
        }

        if (branch == "recipient")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Target = choice.Targets.Single(), Stage = CaoYuStage.XiangzuCards } });
            PublishCaoYu(f);
            return;
        }

        if (branch is "pick" or "unpick")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Selected = branch == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(c => c != id).ToArray() } });
            PublishCaoYu(f);
            return;
        }

        if (branch == "decline")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuOpportunityConsumedEvent(f.Id, r.OpportunityId, r.Source, false));
            FinishCaoYu(f);
            return;
        }

        if (branch == "adjust")
        {
            var own = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            var other = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TurnOwner]).Count;
            var discard = own > other;
            var requested = discard ? own - other : Math.Max(0, Math.Min(5, other) - own);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuOpportunityConsumedEvent(f.Id, r.OpportunityId, r.Source, true));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuXiangxuComparisonEvent(f.Id, r.OpportunityId, r.TurnOwner, own, other, discard ? "discard" : "draw", requested));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuRecipientHistoryEvent(f.Id, f.OwnerSeat, "ol:xiangxu", r.TurnOwner, r.ActualTurn));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Paid = true, Requested = requested, Selected = [], Stage = discard ? CaoYuStage.XiangxuDiscardChoice : CaoYuStage.XiangxuDrawChildren } });
            if (discard)
                PublishCaoYu(f);
            else
                DrawCaoYu(f, requested, CaoYuStage.XiangxuDrawChildren, XiangxuDrawReason);
            return;
        }

        if (branch == "discard")
        {
            PayCaoYuCards(f, r.Selected, null, CaoYuStage.XiangxuDiscardChildren, XiangxuDiscardReason);
            ContinueCaoYu(f.Id);
            return;
        }

        if (branch == "give")
        {
            if (XiangzuSpent(f.OwnerSeat) || r.Target is not { } target || r.Selected.Count == 0)
                throw new InvalidOperationException("Xiangzu cannot spend its permanent limited right twice or give zero entities.");
            var history = CaoYuHistory(f.OwnerSeat, "ol:gongjie", target) && CaoYuHistory(f.OwnerSeat, "ol:xiangxu", target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuXiangzuSpentEvent(f.Id, r.Source, r.GameplayHash, r.DyingId, target, r.Selected.Count, history));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Paid = true, HistoryQualified = history } });
            PayCaoYuCards(f, r.Selected, target, CaoYuStage.XiangzuGiftChildren, XiangzuGiftReason);
            ContinueCaoYu(f.Id);
            return;
        }

        throw new InvalidOperationException("Unknown CaoYu choice.");
    }

    internal void PayCaoYuCards(ProgramSkillFrame f, IReadOnlyList<int> ids, int? target, CaoYuStage stage, string reason)
    {
        var r = f.CaoYu!;
        var materials = ids.Select(id =>
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            return new CaoYuMaterial(id, card.Kind, _cardZonesOperations._cardZones.GetLocation(id), _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card));
        }).ToArray();
        var grains = materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(f.OwnerSeat)) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Stage = stage, Materials = materials, GrainIds = grains, Before = before, After = before, Count = 0, Target = target ?? r.Target, Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < materials.Length; i++)
        {
            var c = materials[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMaterialFrozenEvent(f.Id, stage, r.Index, i, c.CardId, c.Kind, c.From, c.Suit));
        }

        for (var i = 0; i < grains.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuOxGrainFrozenEvent(f.Id, stage, r.Index, i, grains[i]));
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            foreach (var m in moves)
            {
                var c = materials.Single(c => c.CardId == m.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuCardPaidEvent(f.Id, stage, r.Index, c.CardId, c.Kind, c.From, m.To, c.Suit, m.Sequence, batch));
            }

            if (stage == CaoYuStage.GongjieGiftChildren && moves.Count == 1)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuRecipientHistoryEvent(f.Id, f.OwnerSeat, "ol:gongjie", target!.Value, r.ActualTurn));
        }

        if (target is { } seat)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(seat), new(reason), (stage == CaoYuStage.XiangzuGiftChildren ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, seat, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid) : Paid));
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(reason), Paid);
        RecordCaoYuMovement(f.Id);
    }

    internal void RecordCaoYuMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.CaoYu!;
        var count = r.Stage is CaoYuStage.GongjieDrawChildren or CaoYuStage.XiangxuDrawChildren ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == (r.Stage == CaoYuStage.GongjieDrawChildren ? GongjieDrawReason : XiangxuDrawReason)) : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoYuCardPaidEvent>().Count(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index && e.Sequence > r.Before && e.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMovementPaidEvent(id, r.Stage, r.Index, f.OwnerSeat, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
    }

    internal void DrawCaoYu(ProgramSkillFrame f, int count, CaoYuStage stage, string reason)
    {
        var r = f.CaoYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = 0, Materials = [], GrainIds = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && count > 0)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new(reason));
        RecordCaoYuMovement(f.Id);
        ContinueCaoYu(f.Id);
    }

    internal void RecoverCaoYu(ProgramSkillFrame f, int count, CaoYuStage stage)
    {
        var r = f.CaoYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = r with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = 0, Recovery = count, Materials = [], GrainIds = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuRecoveryIssuedEvent(f.Id, stage, count));
        if (count > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, count, null, null);
        RecordCaoYuMovement(f.Id);
        ContinueCaoYu(f.Id);
    }

    internal bool DrainCaoYu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueCaoYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoYu: { } r } f || f.Id != id)
            return;
        if (r.Stage is CaoYuStage.GongjieTargets or CaoYuStage.GongjieTake or CaoYuStage.XiangxuChoice or CaoYuStage.XiangxuDiscardChoice or CaoYuStage.XiangzuRecipient or CaoYuStage.XiangzuCards)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCaoYu(f);
            return;
        }

        if (DrainCaoYu(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishCaoYu(f);
            return;
        }

        if (r.Stage == CaoYuStage.GongjieGiftChildren)
        {
            AdvanceGongjieDonor(f);
            return;
        }

        if (r.Stage == CaoYuStage.XiangxuDiscardChildren && r.Count >= 2)
        {
            RecoverCaoYu(f, 1, CaoYuStage.XiangxuRecoveryChildren);
            return;
        }

        if (r.Stage == CaoYuStage.XiangzuGiftChildren && r.HistoryQualified)
        {
            RecoverCaoYu(f, r.Count, CaoYuStage.XiangzuRecoveryChildren);
            return;
        }

        FinishCaoYu(f);
    }

    internal void FinishCaoYu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoYu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27440)]
    internal bool ResumeCaoYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoYu: not null } f || f.Id != id)
            return false;
        ContinueCaoYu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27440)]
    internal bool ReturnCaoYu(ProgramSkillFrame f)
    {
        if (f.CaoYu is null)
            return false;
        ContinueCaoYu(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27440)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27440)]
    internal bool ContinuePaidCaoYu(ProgramSkillFrame f)
    {
        if (f.CaoYu is not { } r || !r.Paid && !(r.Op == SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive))
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27441)]
    internal bool ContinueIssuedCaoYu(ProgramSkillFrame f) => f.CaoYu is null && f.InstructionIndex is 0 or 1 && f.WindowContext is { } c && (IsXiangxuMinimumCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c) || IsIssuedXiangxuEndingCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c));
    [ProgramAiChoice(SkillProgramEffectOp.ResolveGongjieRoundFirstGifts, SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment, SkillProgramEffectOp.ResolveXiangzuDyingGiftAndRecovery)]
    internal PromptChoice AiCaoYu(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.CaoYu!;
        if (r.Stage == CaoYuStage.GongjieTargets)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "target" && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "targets-done") ?? p.Choices.Last();
        if (r.Stage == CaoYuStage.XiangzuRecipient)
            return p.Choices.FirstOrDefault(c => c.Targets.Count > 0 && CaoYuHistory(f.OwnerSeat, "ol:gongjie", c.Targets[0]) && CaoYuHistory(f.OwnerSeat, "ol:xiangxu", c.Targets[0])) ?? p.Choices.First();
        if (r.Stage == CaoYuStage.XiangzuCards)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pick") ?? p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "give");
        if (r.Stage == CaoYuStage.XiangxuDiscardChoice)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "pick");
        return p.Choices.First();
    }
}
