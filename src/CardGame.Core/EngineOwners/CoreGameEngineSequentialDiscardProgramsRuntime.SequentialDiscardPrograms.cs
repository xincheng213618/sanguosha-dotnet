#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSequentialDiscardProgramsRuntime
{
    internal long SequentialDiscardMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;

    internal static bool IsSequentialDiscardOp(SkillProgramEffectOp op) => RuntimeSkills.RuntimeRules_Core_SequentialDiscardPrograms.IsSequentialDiscardOp(op);
    internal SkillProgramEffect SequentialDiscardEffect(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
    internal string SequentialDiscardReason(ProgramSkillFrame frame) => $"skill-program.{(frame.SkillId)}.{(SequentialDiscardEffect(frame).Op)}";
    internal CardConversionSource SequentialDiscardSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IReadOnlyList<int> SequentialDiscardOrder(int owner, int start, bool ring) => Array.AsReadOnly((ring ? Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(start, offset)).Where(seat => seat != owner) : Enumerable.Repeat(start, 1)).ToArray());
    internal bool SequentialDiscardCanContinue(ProgramSkillFrame frame) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame);
    internal IEnumerable<(Card Card, CardLocation Location)> SequentialDiscardCards(ProgramSkillFrame frame, int chooser, SkillProgramEffect effect) => effect.Zones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, chooser)).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, new(zone, chooser), OwnedCardMoveIntent.Discard) && !(zone == CardZoneKind.Equipment && chooser == frame.OwnerSeat && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(chooser, frame.SkillId, frame.SkillInstanceId, card))).Select(card => (card, new CardLocation(zone, chooser))));
    internal SkillProgramStepOutcome RunSequentialDiscard(ProgramSkillFrame supplied, SkillProgramEffect effect, int selectedTarget)
    {
        if (CoreGameEngineAllHandChallengesRuntime.IsAllHandChallengeOp(effect.Op))
            return _coreAllHandChallengesOperations.Value.RunAllHandChallenge(supplied, effect);
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsSequentialDiscardOp(effect.Op) || effect.Target != (effect.Op == SkillProgramEffectOp.EscalatingDiscardOrDamageFromSelected ? SkillProgramEffectTarget.Owner : SkillProgramEffectTarget.SelectedTarget) || frame.TriggerId is not null || frame.WindowContext is not null || frame.SelectedTargetSeats is not [var start] || start == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(start))
            throw new InvalidOperationException("A sequential discard requires its original one-target activation.");
        if (frame.SequentialDiscard is null)
        {
            if (!SequentialDiscardCanContinue(frame) || !_coreMatchParticipantsPlayersAccess._players[start].IsAlive)
                return SkillProgramStepOutcome.Continue;
            var ring = effect.Op == SkillProgramEffectOp.EscalatingDiscardOrDamageFromSelected;
            var draft = new ProgramSequentialDiscardDraft(ring ? ProgramSequentialDiscardKind.SelectedStartEscalating : ProgramSequentialDiscardKind.CategoryOrSequential, frame.InstructionIndex - 1, SequentialDiscardSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, start, SequentialDiscardOrder(frame.OwnerSeat, start, ring), 0, start, 0, 0, null, ProgramSequentialDiscardStage.BranchChoice, [], []);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SequentialDiscard = draft, ReexecuteParticipantInstruction = true });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSequentialDiscardStartedEvent(frame.Id, draft.Kind, draft.Source, draft.GameplayHash, draft.ActualTurnNumber, draft.ActualTurnOwnerSeat, start));
        }
        else
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ReexecuteParticipantInstruction = true });
        if (!_coreSequentialDiscardReceiptsOperations.ValidSequentialDiscard(frame))
            throw new InvalidOperationException("A sequential discard lost its original producer or actual ledger.");
        var current = frame.SequentialDiscard!;
        if (current.Stage is ProgramSequentialDiscardStage.AwaitingMovement or ProgramSequentialDiscardStage.AwaitingDamage)
        {
            if (frame.PendingMovementContinuation is not null || frame.AttackAttempt is not null)
                throw new InvalidOperationException("The sequential discard resumed before its real child completed.");
            if (current.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating)
                current = current with
                {
                    Cursor = current.Cursor + 1,
                    Stage = ProgramSequentialDiscardStage.BranchChoice,
                    PrimaryBranch = null,
                    Payment = null
                };
            else
                current = current with
                {
                    Stage = ProgramSequentialDiscardStage.CardChoice
                };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SequentialDiscard = current });
        }

        if (!SequentialDiscardCanContinue(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "弃牌来源或原实例失效，已付结算保留。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (current.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating)
        {
            var cursor = current.Cursor;
            while (cursor < current.Order.Count && !_coreMatchParticipantsPlayersAccess._players[current.Order[cursor]].IsAlive)
                cursor++;
            if (cursor == current.Order.Count)
                return FinishSequentialDiscard(frame, 0);
            current = current with
            {
                Cursor = cursor,
                ChooserSeat = current.Order[cursor]
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SequentialDiscard = current });
        }
        else if (!_coreMatchParticipantsPlayersAccess._players[current.ChooserSeat].IsAlive || current.PrimaryBranch is not null && current.Remaining == 0)
            return FinishSequentialDiscard(frame, current.Remaining);
        return PublishSequentialDiscard(frame, effect);
    }

    internal SkillProgramStepOutcome FinishSequentialDiscard(ProgramSkillFrame frame, int remaining)
    {
        var d = frame.SequentialDiscard!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSequentialDiscardFinishedEvent(frame.Id, d.Kind is ProgramSequentialDiscardKind.SelectedStartEscalating or ProgramSequentialDiscardKind.SelectedStartAllHand ? d.Cursor : 1, remaining));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SequentialDiscard = null, ReexecuteParticipantInstruction = false });
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome PublishSequentialDiscard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var d = frame.SequentialDiscard!;
        var available = SequentialDiscardCards(frame, d.ChooserSeat, effect).ToArray();
        var choices = new List<PromptChoice>();
        void Add(string branch, string label, IReadOnlyList<int>? cards = null, CardLocation? location = null) => choices.Add(new(new ChoiceId($"sequential-discard.{(frame.Id)}.{(d.Cursor)}.{(d.Stage)}.{(branch)}.{(cards?.FirstOrDefault() ?? 0)}"), label, cards ?? [], cards is null ? [] : [d.ChooserSeat], new Dictionary<string, string> { ["program-action"] = "sequential-discard", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["cursor"] = d.Cursor.ToString(CultureInfo.InvariantCulture), ["stage"] = d.Stage.ToString(), ["branch"] = branch, ["source-zone"] = location?.Zone.ToString() ?? "", ["selection-index"] = d.SelectedCardIds.Count.ToString(CultureInfo.InvariantCulture) }));
        if (d.Stage == ProgramSequentialDiscardStage.BranchChoice)
        {
            if (d.Kind == ProgramSequentialDiscardKind.CategoryOrSequential)
            {
                if (available.Any(item => effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(item.Card.Kind, category))))
                    Add("primary", $"弃置 {(effect.Amount)} 张指定类别牌。");
                if (available.Length != 0)
                    Add("sequential", $"依次弃置 {(effect.MinimumValue)} 张任意牌。");
            }
            else
            {
                if (available.Length > d.PreviousCount)
                    Add("discard", $"弃置至少 {(d.PreviousCount + 1)} 张牌。");
                Add("damage", $"受到 {(effect.Amount)} 点{((effect.DamageNature == DamageNature.Fire ? "火焰" : effect.DamageNature == DamageNature.Thunder ? "雷电" : ""))}伤害。");
            }
        }
        else
        {
            foreach (var item in available.Where(item => !d.SelectedCardIds.Contains(item.Card.Id) && (d.PrimaryBranch != true || effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(item.Card.Kind, category)))))
                Add("card", $"{((d.Stage == ProgramSequentialDiscardStage.SelectingBatch ? "选择" : "弃置"))}【{(item.Card.DisplayName)}】。", [item.Card.Id], item.Location);
            if (d.Stage == ProgramSequentialDiscardStage.SelectingBatch && d.SelectedCardIds.Count > d.PreviousCount)
                Add("finish", $"一次弃置所选 {(d.SelectedCardIds.Count)} 张牌。");
        }

        if (choices.Count == 0)
            return FinishSequentialDiscard(frame, d.Remaining);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, d.ChooserSeat, d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating ? $"【{(skill.Name)}】上家实际弃牌 {(d.PreviousCount)} 张。" : $"【{(skill.Name)}】请选择弃牌分支；还需弃置 {(d.Remaining)} 张。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [d.ChooserSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = d.ChooserSeat,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[d.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("sequential-discard")]
    internal void ResolveSequentialDiscardChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing sequential discard owner.");
        var d = f.SequentialDiscard ?? throw new InvalidOperationException("Missing sequential discard receipt.");
        if (CoreGameEngineAllHandChallengesRuntime.IsAllHandChallengeKind(d.Kind))
        {
            _coreAllHandChallengesOperations.Value.ResolveAllHandChallengeChoice(f, choice);
            return;
        }

        var effect = SequentialDiscardEffect(f);
        if (!_coreSequentialDiscardReceiptsOperations.ValidSequentialDiscard(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } decision || decision.PlayerSeat != d.ChooserSeat || !decision.Choices.Any(c => c.Id == choice.Id) || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("cursor") != d.Cursor.ToString(CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("stage") != d.Stage.ToString() || choice.Parameters.GetValueOrDefault("selection-index") != d.SelectedCardIds.Count.ToString(CultureInfo.InvariantCulture))
            throw new InvalidOperationException("A sequential discard choice must belong to its exact private producer.");
        if (!SequentialDiscardCanContinue(f) || !_coreMatchParticipantsPlayersAccess._players[d.ChooserSeat].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "弃牌参与者或原实例失效。");
            return;
        }

        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (d.Stage == ProgramSequentialDiscardStage.BranchChoice)
        {
            if (choice.Cards.Count != 0 || choice.Targets.Count != 0)
                throw new InvalidOperationException("Choosing a discard branch cannot pay an entity.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating && branch == "damage")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SequentialDiscard = d with { Stage = ProgramSequentialDiscardStage.AwaitingDamage, PreviousCount = 0, SelectedCardIds = [], SelectedLocations = [] }, ReexecuteParticipantInstruction = true });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSequentialDiscardDamageChosenEvent(f.Id, d.Cursor, f.OwnerSeat, d.ChooserSeat, effect.Amount, effect.DamageNature ?? DamageNature.Normal));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, d.ChooserSeat, effect.Amount, nature: effect.DamageNature);
                return;
            }

            if (d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating && branch != "discard" || d.Kind == ProgramSequentialDiscardKind.CategoryOrSequential && branch is not ("primary" or "sequential"))
                throw new InvalidOperationException("Invalid sequential discard branch.");
            var primary = branch == "primary";
            d = d with
            {
                PrimaryBranch = primary,
                Remaining = d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating ? 0 : primary ? effect.Amount : effect.MinimumValue,
                Stage = d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating ? ProgramSequentialDiscardStage.SelectingBatch : ProgramSequentialDiscardStage.CardChoice
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SequentialDiscard = d });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSequentialDiscardBranchEvent(f.Id, d.Cursor, d.ChooserSeat, primary, d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating ? d.PreviousCount + 1 : d.Remaining));
            if (PublishSequentialDiscard(f, effect) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (branch == "card")
        {
            if (choice.Cards is not [var id] || choice.Targets is not [var chooser] || chooser != d.ChooserSeat || d.SelectedCardIds.Contains(id) || !Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone))
                throw new InvalidOperationException("Invalid sequential discard entity.");
            var item = SequentialDiscardCards(f, chooser, effect).SingleOrDefault(item => item.Card.Id == id && item.Location.Zone == zone);
            if (item.Card is null || d.PrimaryBranch == true && !effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(item.Card.Kind, category)))
                throw new InvalidOperationException("The selected entity no longer satisfies the frozen discard branch.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (d.Stage == ProgramSequentialDiscardStage.SelectingBatch)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SequentialDiscard = d with { SelectedCardIds = Array.AsReadOnly(d.SelectedCardIds.Append(id).ToArray()), SelectedLocations = Array.AsReadOnly(d.SelectedLocations.Append(item.Location).ToArray()) } });
                PublishSequentialDiscard(f, effect);
                return;
            }

            if (d.Stage != ProgramSequentialDiscardStage.CardChoice)
                throw new InvalidOperationException("The sequential discard is not choosing its next card.");
            PaySequentialDiscard(f, [id], [item.Location]);
            return;
        }

        if (branch != "finish" || d.Stage != ProgramSequentialDiscardStage.SelectingBatch || choice.Cards.Count != 0 || choice.Targets.Count != 0 || d.SelectedCardIds.Count <= d.PreviousCount)
            throw new InvalidOperationException("The escalating discard has not exceeded its real predecessor.");
        if (d.SelectedCardIds.Where((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != d.SelectedLocations[i] || !SequentialDiscardCards(f, d.ChooserSeat, effect).Any(item => item.Card.Id == id && item.Location == d.SelectedLocations[i])).Any())
            throw new InvalidOperationException("A privately selected discard entity left its frozen owned source.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        PaySequentialDiscard(f, d.SelectedCardIds, d.SelectedLocations);
    }

    internal void PaySequentialDiscard(ProgramSkillFrame frame, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> sources)
    {
        var d = frame.SequentialDiscard!;
        var before = SequentialDiscardMovementSequence;
        var payment = new ProgramSequentialDiscardPayment(d.ChooserSeat, d.Cursor, Array.AsReadOnly(ids.ToArray()), Array.AsReadOnly(sources.ToArray()), before, before, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SequentialDiscard = d with { Stage = ProgramSequentialDiscardStage.AwaitingMovement, SelectedCardIds = [], SelectedLocations = [], Payment = payment }, ReexecuteParticipantInstruction = true, PendingMovementContinuation = new(d.ChooserSeat, 0, null) });
        var reason = new CardMoveReason(SequentialDiscardReason(frame));
        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, d.ChooserSeat, ids, reason, frame.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, reason, (_, records) =>
        {
            var actual = records.Count(m => m.From.OwnerSeat == d.ChooserSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.DiscardPile || m.From.Zone == CardZoneKind.Equipment && m.To == CardLocation.OutsideGame));
            var paid = payment with
            {
                SequenceAfter = records.Max(m => m.Sequence),
                ActualCount = actual
            };
            var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var current = f.SequentialDiscard!;
            var remaining = d.Kind == ProgramSequentialDiscardKind.CategoryOrSequential ? Math.Max(0, d.Remaining - actual) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SequentialDiscard = current with { Payment = paid, Remaining = remaining, PreviousCount = d.Kind == ProgramSequentialDiscardKind.SelectedStartEscalating ? actual : current.PreviousCount } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSequentialDiscardPaidEvent(frame.Id, d.Cursor, d.ChooserSeat, paid.SequenceBefore, paid.SequenceAfter, ids.Count, actual, remaining));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseCategoryOrSequentialDiscard, SkillProgramEffectOp.EscalatingDiscardOrDamageFromSelected, SkillProgramEffectOp.ChooseTrickOrAllHandDiscard, SkillProgramEffectOp.AllHandDiscardOrDifferenceDamageFromSelected)]
    internal PromptChoice SelectAiSequentialDiscard(PendingDecision decision, ProgramSkillFrame frame)
    {
        var d = frame.SequentialDiscard!;
        var effect = SequentialDiscardEffect(frame);
        if (CoreGameEngineAllHandChallengesRuntime.IsAllHandChallengeKind(d.Kind))
            return _coreAllHandChallengesOperations.Value.SelectAiAllHandChallenge(decision, frame, effect);
        var branches = decision.Choices.Where(c => c.Cards.Count == 0).ToArray();
        if (d.Stage == ProgramSequentialDiscardStage.BranchChoice)
        {
            if (d.Kind == ProgramSequentialDiscardKind.CategoryOrSequential)
                return branches.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "primary") ?? branches.Single();
            var discard = branches.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard");
            if (discard is null)
                return branches.Single();
            // The actor sees only its own current entities and public HP/range. No other concealed hand is consulted.
            var cost = SequentialDiscardCards(frame, d.ChooserSeat, effect).Select(item => CoreNativeAiDecisionsRuntime.GetKeepValue(item.Card, _coreMatchParticipantsPlayersAccess._players[d.ChooserSeat])).Order().Take(d.PreviousCount + 1).Sum();
            return cost <= effect.Amount * 20d ? discard : branches.Single(c => c.Parameters.GetValueOrDefault("branch") == "damage");
        }

        if (branches.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "finish")is { } finish)
            return finish;
        return decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Cards[0])).Single(card => card.Id == c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[d.ChooserSeat])).ThenBy(c => c.Cards[0]).First();
    }

    // Opt in only after this operation's exact physical ledger has paid. A dead
    // forced participant may finish its child and leave the ring's next seat intact.
    [ProgramContinuation(ProgramContinuationKind.MovementTail, 50)]
    internal bool TryReturnSequentialDiscardMovement(ProgramSkillFrame frame)
    {
        if (frame.SequentialDiscard is not { Stage: ProgramSequentialDiscardStage.AwaitingMovement, Payment: not null })
            return false;
        if (!_coreSequentialDiscardReceiptsOperations.ValidSequentialDiscard(frame) || frame.PendingMovementContinuation is null)
            throw new InvalidOperationException("The sequential discard movement lost its original paid continuation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    SkillProgramStepOutcome ISequentialDiscardProgramHost.RunSequentialDiscard(ProgramSkillFrame f, SkillProgramEffect e, int target) => RunSequentialDiscard(f, e, target);
}
