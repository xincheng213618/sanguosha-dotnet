#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPiOlRuntime
{
    internal const string LiuPiTableBind = "yicheng-public-table";
    internal CardConversionSource LiuPiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static string LiuPiMovementReason(LiuPiExchangeStage stage) => RuntimeSkills.RuntimeRules_liu_pi_ol_LiuPi.LiuPiMovementReason(stage);
    SkillProgramStepOutcome ILiuPiProgramHost.Exchange(ProgramSkillFrame f) => BeginLiuPiExchange(f);
    internal SkillProgramStepOutcome BeginLiuPiExchange(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LiuPiExchange is not null || f.TriggerId is not null || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "exchange")
            throw new InvalidOperationException("Yicheng needs its one original active program frame.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue)
            return SkillProgramStepOutcome.Continue;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = new(LiuPiSource(f), f.GameplayHash, f.InstructionIndex, LiuPiExchangeStage.RevealChildren, Math.Max(0, owner.MaxHp), before, before) });
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, f.LiuPiExchange!.RevealLimit, null, LiuPiTableBind, SkillProgramCardSetVisibility.Public);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = f.LiuPiExchange!with { Table = _coreProgramLifecycleOperations.GetProgramCardSet(f, LiuPiTableBind).CardIds.Select(LiuPiEntry).ToArray(), SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueLiuPiExchange(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal LiuPiTableEntry LiuPiEntry(int id) => new(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id)?.Sequence ?? 0);
    internal bool LiuPiEntryAvailable(LiuPiTableEntry e) => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Processing && (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == e.CardId)?.Sequence ?? 0) == e.MovementSequence && _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Rank == e.Rank;

    internal IReadOnlyList<LiuPiTableEntry> LiuPiAvailableTable(ProgramSkillFrame f) => f.LiuPiExchange!.Table.Where(LiuPiEntryAvailable).ToArray();
    internal ProgramSkillFrame ReplaceLiuPiPublicTable(long id, IReadOnlyList<int> ids)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.LiuPiExchange is null || _coreProgramLifecycleOperations.GetProgramCardSet(f, LiuPiTableBind).Visibility != SkillProgramCardSetVisibility.Public || ids.Distinct().Count() != ids.Count || ids.Any(card => _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Processing))
            throw new InvalidOperationException("Only the owning Yicheng frame may replace its exact public processing row.");
        var table = new ProgramSkillCardSetBinding(LiuPiTableBind, Array.AsReadOnly(ids.ToArray()), SkillProgramCardSetVisibility.Public, Array.AsReadOnly(ids.Select(_ => CardLocation.Processing).ToArray()));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Select(binding => binding.Name == LiuPiTableBind ? table : binding).ToArray()) });
        return f;
    }

    internal bool DrainLiuPiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueLiuPiExchange(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { LiuPiExchange: { } r } f && f.Id == id)
        {
            if (DrainLiuPiChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.LiuPiExchange!;
            if (r.Stage == LiuPiExchangeStage.CleanupChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CleanupLiuPiExchange(f);
                return;
            }

            var available = LiuPiAvailableTable(f);
            if (available.Count != r.Table.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = r = r with { Table = available } });
                f = ReplaceLiuPiPublicTable(id, available.Select(e => e.CardId).ToArray());
            }

            switch (r.Stage)
            {
                case LiuPiExchangeStage.RevealChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = r with { Stage = LiuPiExchangeStage.HandSelection } });
                    break;
                case LiuPiExchangeStage.EqualExchangeChildren:
                    if (r.EqualRankAfter <= r.EqualRankBefore || available.Count == 0)
                    {
                        CleanupLiuPiExchange(f);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = r with { Stage = LiuPiExchangeStage.FullOffer, SelectedHand = [], SelectedTable = [] } });
                    break;
                case LiuPiExchangeStage.FullExchangeChildren:
                    CleanupLiuPiExchange(f);
                    return;
            }

            PublishLiuPiExchange(f);
            return;
        }
    }

    internal IReadOnlyList<PromptChoice> LiuPiChoices(ProgramSkillFrame f)
    {
        var r = f.LiuPiExchange!;
        var result = new List<PromptChoice>();
        void Add(string action, string label, IReadOnlyList<int>? cards = null) => result.Add(new(new($"liu-pi.{(f.Id)}.{(r.Stage)}.{(action)}.{(string.Join('.', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "liu-pi", ["option"] = action }));
        if (r.Stage == LiuPiExchangeStage.HandSelection)
        {
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                if (r.SelectedHand.Contains(card.Id) || r.SelectedHand.Count < LiuPiAvailableTable(f).Count)
                    Add("hand", $"{((r.SelectedHand.Contains(card.Id) ? "取消" : "选择"))}【{(card.DisplayName)}】{(card.RankText)}", [card.Id]);
            Add("select-table", r.SelectedHand.Count == 0 ? "不交换，结束易城" : $"已选{(r.SelectedHand.Count)}张手牌，选择等量展示牌", r.SelectedHand);
        }
        else if (r.Stage == LiuPiExchangeStage.TableSelection)
        {
            foreach (var e in LiuPiAvailableTable(f))
                if (r.SelectedTable.Contains(e.CardId) || r.SelectedTable.Count < r.SelectedHand.Count)
                    Add("table", $"{((r.SelectedTable.Contains(e.CardId) ? "取消" : "选择"))}【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).DisplayName)}】{(e.Rank)}", [e.CardId]);
            if (r.SelectedTable.Count == r.SelectedHand.Count)
                Add("equal-exchange", "完成等量交换", r.SelectedTable);
            Add("back", "重新选择手牌");
        }
        else if (r.Stage == LiuPiExchangeStage.FullOffer)
        {
            if (r.EqualRankAfter > r.EqualRankBefore)
                Add("full-order", "以全部手牌交换全部展示牌，先排列回顶顺序");
            Add("finish", "保留手牌，将展示牌按原顺序放回牌堆顶");
        }
        else if (r.Stage == LiuPiExchangeStage.FullOrder)
        {
            foreach (var id in r.FullHand.Where(id => !r.FullOrder.Contains(id)))
                Add("order", $"第{(r.FullOrder.Count + 1)}张回到牌堆顶：【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [id]);
            if (r.FullOrder.Count == r.FullHand.Count)
                Add("full-exchange", "按所选顺序完成全手交换", r.FullOrder);
            if (r.FullOrder.Count > 0)
                Add("reset-order", "重新排列回顶顺序");
            Add("finish", "取消第二次交换，按原顺序放回展示牌");
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishLiuPiExchange(ProgramSkillFrame f)
    {
        var choices = LiuPiChoices(f);
        if (choices.Count == 0)
        {
            CleanupLiuPiExchange(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "易城：选择交换牌或回顶顺序", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("liu-pi")]
    internal void AnswerLiuPiExchange(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuPiExchange: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LiuPiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Yicheng lost its private chooser or an exact current physical selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var action = choice.Parameters["option"];
        static IReadOnlyList<int> Toggle(IReadOnlyList<int> ids, int id) => ids.Contains(id) ? ids.Where(e => e != id).ToArray() : ids.Append(id).ToArray();
        if (action == "hand")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { SelectedHand = Toggle(r.SelectedHand, choice.Cards.Single()) } });
        else if (action == "table")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { SelectedTable = Toggle(r.SelectedTable, choice.Cards.Single()) } });
        else if (action == "select-table")
        {
            if (r.SelectedHand.Count == 0)
            {
                CleanupLiuPiExchange(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { Stage = LiuPiExchangeStage.TableSelection, SelectedTable = [] } });
        }
        else if (action == "back")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { Stage = LiuPiExchangeStage.HandSelection, SelectedTable = [] } });
        else if (action == "equal-exchange")
            IssueLiuPiExchange(f, false);
        else if (action == "full-order")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { Stage = LiuPiExchangeStage.FullOrder, FullHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray(), FullOrder = [] } });
        else if (action == "order")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { FullOrder = r.FullOrder.Append(choice.Cards.Single()).ToArray() } });
        else if (action == "reset-order")
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = r with { FullOrder = [] } });
        else if (action == "full-exchange")
            IssueLiuPiExchange(f, true);
        else if (action == "finish")
        {
            CleanupLiuPiExchange(f);
            return;
        }
        else
            throw new InvalidOperationException("Unknown Yicheng answer.");
        ContinueLiuPiExchange(f.Id);
    }

    internal void IssueLiuPiExchange(ProgramSkillFrame f, bool full)
    {
        var r = f.LiuPiExchange!;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).ToArray();
        var table = LiuPiAvailableTable(f).ToArray();
        var outgoing = full ? r.FullOrder.ToArray() : hand.Where(c => r.SelectedHand.Contains(c.Id)).Select(c => c.Id).ToArray();
        var incoming = table.Where(e => full || r.SelectedTable.Contains(e.CardId)).Select(e => e.CardId).ToArray();
        if (full ? r.EqualRankAfter <= r.EqualRankBefore || !hand.Select(c => c.Id).Order().SequenceEqual(r.FullHand.Order()) || outgoing.Length != r.FullHand.Count || !outgoing.Order().SequenceEqual(r.FullHand.Order()) : outgoing.Length != r.SelectedHand.Count || outgoing.Length != incoming.Length || outgoing.Length == 0)
            throw new InvalidOperationException("Yicheng must pay the exact equal subset, or its entire current hand after a real strict rank increase.");
        var beforeRank = table.Sum(e => e.Rank);
        var afterRank = full ? outgoing.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank) : beforeRank - table.Where(e => incoming.Contains(e.CardId)).Sum(e => e.Rank) + outgoing.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank);
        var stage = full ? LiuPiExchangeStage.FullExchangeChildren : LiuPiExchangeStage.EqualExchangeChildren;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = r with { Stage = stage, SequenceBefore = before, SequenceAfter = before, EqualRankBefore = full ? r.EqualRankBefore : beforeRank, EqualRankAfter = full ? r.EqualRankAfter : afterRank } });
        // The existing native helper commits both physical directions in ONE rules
        // batch. No observer sees a fictional empty hand between two exchange legs.
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(outgoing.Concat(incoming).ToArray(), CardLocation.Processing, new(LiuPiMovementReason(stage)), afterPhysicalRecords: (_, movements) =>
        {
            if (movements.Count != outgoing.Length + incoming.Length || movements.Any(m => m.From != (outgoing.Contains(m.CardId) ? CardLocation.Hand(f.OwnerSeat) : CardLocation.Processing) || m.To != (outgoing.Contains(m.CardId) ? CardLocation.Processing : CardLocation.Hand(f.OwnerSeat))))
                throw new InvalidOperationException("Yicheng did not commit its exact two-direction physical batch.");
            var replacements = new Queue<int>(outgoing);
            var resulting = full ? outgoing.Select(LiuPiEntry).ToArray() : table.Select(e => incoming.Contains(e.CardId) ? LiuPiEntry(replacements.Dequeue()) : e).ToArray();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { LiuPiExchange = current.LiuPiExchange!with { Table = resulting, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ReplaceLiuPiPublicTable(f.Id, resulting.Select(e => e.CardId).ToArray());
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, LiuPiTableBind, Array.AsReadOnly(resulting.Select(e => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId))).ToArray())));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuPiExchangeCommittedEvent(f.Id, r.Source, full, outgoing.Length, incoming.Length, beforeRank, afterRank, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, destinationForCard: (_, source) => source == CardLocation.Hand(f.OwnerSeat) ? CardLocation.Processing : CardLocation.Hand(f.OwnerSeat));
    }

    internal void CleanupLiuPiExchange(ProgramSkillFrame f)
    {
        if (f.LiuPiExchange!.Stage != LiuPiExchangeStage.CleanupChildren)
        {
            var returning = LiuPiAvailableTable(f).Select(e => e.CardId).ToArray();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuPiExchange = f.LiuPiExchange with { Stage = LiuPiExchangeStage.CleanupChildren, SequenceBefore = before, SequenceAfter = before } });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(returning, CardLocation.DrawPile, new(LiuPiMovementReason(LiuPiExchangeStage.CleanupChildren)), afterPhysicalRecords: (_, _) => _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(returning));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuPiExchange = f.LiuPiExchange!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        }

        ContinueLiuPiExchange(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26120)]
    internal bool ResumeLiuPiExchange(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuPiExchange: not null } f || f.Id != id)
            return false;
        ContinueLiuPiExchange(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26120)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26120)]
    internal bool ContinueIssuedLiuPiExchange(ProgramSkillFrame f) => f.LiuPiExchange is not null;

    [ProgramAiChoice(SkillProgramEffectOp.ResolveYichengPublicExchange)]
    internal PromptChoice AiLiuPiExchange(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.LiuPiExchange!;
        if (r.Stage == LiuPiExchangeStage.HandSelection)
        {
            if (r.SelectedHand.Count == 0 && LiuPiAvailableTable(f)is { Count: > 0 } table && p.Choices.Where(c => c.Parameters["option"] == "hand").OrderByDescending(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Rank).FirstOrDefault()is { } pick && _coreAdvancedLifecycleOperations.GetAdvancedCard(pick.Cards.Single()).Rank > table.Min(e => e.Rank))
                return pick;
            return p.Choices.Single(c => c.Parameters["option"] == "select-table");
        }

        if (r.Stage == LiuPiExchangeStage.TableSelection)
            return r.SelectedTable.Count == r.SelectedHand.Count ? p.Choices.Single(c => c.Parameters["option"] == "equal-exchange") : p.Choices.Where(c => c.Parameters["option"] == "table" && !r.SelectedTable.Contains(c.Cards.Single())).OrderBy(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Rank).First();
        if (r.Stage == LiuPiExchangeStage.FullOffer)
            return p.Choices.Single(c => c.Parameters["option"] == (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count < LiuPiAvailableTable(f).Count ? "full-order" : "finish"));
        return p.Choices.FirstOrDefault(c => c.Parameters["option"] == "order") ?? p.Choices.Single(c => c.Parameters["option"] == "full-exchange");
    }
}
