#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMouOlRuntime
{
    internal static CardConversionSource MouDongZhaoSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasMouDongZhao => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouDongZhaoShunji) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouDongZhaoYishi);

    internal Card[] MouDongZhaoHE(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).ToArray();
    internal MouDongZhaoNameCount[] MouDongZhaoNames(int owner) => MouDongZhaoHE(owner).GroupBy(c => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c)).Select(g => new MouDongZhaoNameCount(g.Key, g.Count())).ToArray();
    internal bool MouShunjiSpent(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoShunjiAcceptedEvent>().Any(e => e.Source.OwnerSeat == owner && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.TargetSeat == target);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33500, SkillProgramEffectOp.ResolveMouDongZhaoShunji)]
    internal bool CanOfferMouShunji(ProgramTriggerLegalityContext c) => c.SkillId == MouDongZhaoComposition.Shunji && c.Owner.IsAlive && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && c.Context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !MouShunjiSpent(c.OwnerSeat, target);
    internal bool ExactMouShunjiWindow(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c && _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == c.ParentFrameId)is DamageTriggerWindowFrame window && window.TriggerWindow == c.Window && window.ParentFrameId == c.DamageFrameId && window.TargetSeat == c.TargetSeat && window.Candidates.Any(candidate => candidate.ToProgramCandidate()is { } native && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, native)) && _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == window.ParentFrameId)is DamageFrame damage && damage.TargetSeat == c.TargetSeat && damage.Amount == c.Amount;
    internal SkillProgramStepOutcome BeginMouShunji(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDongZhao is not null)
        {
            ContinueMouDongZhao(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouDongZhaoComposition.Shunji || f.TriggerId != "damage-draw-and-gift" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || !ExactMouShunjiWindow(f) || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target } c || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualRoundProgressRoundNumberAccess._roundNumber <= 0 || MouShunjiSpent(f.OwnerSeat, target))
            throw new InvalidOperationException("Shunji requires its unused native Round-victim damage occurrence.");
        var receipt = new MouDongZhaoReceipt(MouDongZhaoSource(f), f.GameplayHash, SkillProgramEffectOp.ResolveMouDongZhaoShunji, MouDongZhaoStage.ChoosingDraw, c.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoShunjiAcceptedEvent(f.Id, receipt.Source, receipt.GameplayHash, receipt.WindowId, receipt.RoundNumber, target));
        PublishMouDongZhao(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouDongZhaoChoice(ProgramSkillFrame f, string verb, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, IReadOnlyDictionary<string, string>? native = null) => new(new($"mou-dong-zhao.{(f.Id)}.{(f.MouDongZhao!.Stage)}.{(verb)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}.{(native?.GetValueOrDefault("source-zone"))}.{(native?.GetValueOrDefault("slot-index"))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string>(native ?? new Dictionary<string, string>()) { ["program-action"] = "mou-dong-zhao", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["verb"] = verb });
    internal IReadOnlyList<PromptChoice> MouDongZhaoChoices(ProgramSkillFrame f)
    {
        var r = f.MouDongZhao!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        if (r.Stage == MouDongZhaoStage.ChoosingDraw)
            return Array.AsReadOnly(Enumerable.Range(0, 3).Select(n => MouDongZhaoChoice(f, "draw-" + n, $"摸{(n)}张牌，再交给{(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)}等量牌")).ToArray());
        if (r.Stage == MouDongZhaoStage.ChoosingGift)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || MouDongZhaoHE(f.OwnerSeat).Length < r.GiftCount)
                return[];
            var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => r.Selected.Contains(card.Id) || r.Selected.Count < r.GiftCount, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: r.TargetSeat).Select(c => MouDongZhaoChoice(f, "gift-toggle", c.Description, c.Cards, native: c.Parameters)).ToList();
            if (r.Selected.Count == r.GiftCount)
                choices.Add(MouDongZhaoChoice(f, "gift-confirm", $"交出{(r.GiftCount)}张牌"));
            return Array.AsReadOnly(choices.ToArray());
        }

        return MouYishiChoices(f);
    }

    internal void PublishMouDongZhao(ProgramSkillFrame f)
    {
        var choices = MouDongZhaoChoices(f);
        if (choices.Count == 0)
        {
            CompleteMouDongZhao(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：选择本次效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-dong-zhao")]
    internal void AnswerMouDongZhao(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDongZhao: { } r } f || !ValidMouDongZhao(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouDongZhaoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("OL Dong Zhao requires its current canonical native choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var verb = selected.Parameters["verb"];
        if (r.Stage == MouDongZhaoStage.ChoosingDraw)
        {
            var count = int.Parse(verb[5..], CultureInfo.InvariantCulture);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { RequestedDraw = count, Stage = count == 0 ? MouDongZhaoStage.Complete : MouDongZhaoStage.Drawing } });
            if (count == 0)
                CompleteMouDongZhao(f);
            else
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("skill-program.mou-ol:shunji.draw"));
            return;
        }

        if (r.Stage == MouDongZhaoStage.ChoosingGift)
        {
            if (verb == "gift-toggle")
            {
                var id = selected.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray() } });
                PublishMouDongZhao(f);
                return;
            }

            var names = MouDongZhaoNames(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { NamesFrozen = true, Names = names } });
            // Giving one's own Hand back to oneself does not fabricate a move or loss.
            var moving = r.Selected.Where(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.TargetSeat)).ToArray();
            MutateMouDongZhao(f, MouDongZhaoStage.GiftChildren, () =>
            {
                if (moving.Length > 0)
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(moving, CardLocation.Hand(r.TargetSeat), new("skill-program.mou-ol:shunji.gift"), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.TargetSeat, moving, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, moves) =>
                    {
                        foreach (var m in moves)
                            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoGiftSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
                        SpendMouShunjiNames(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                    }));
            });
            return;
        }

        AnswerMouYishi(f, selected);
    }

    internal void SpendMouShunjiNames(ProgramSkillFrame f)
    {
        var r = f.MouDongZhao!;
        if (r.TargetSeat == f.OwnerSeat)
            return;
        var after = MouDongZhaoNames(f.OwnerSeat);
        foreach (var name in r.Names.Where(n => n.Count > 0 && !after.Any(a => a.Kind == n.Kind && a.Count > 0) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoNameSpentEvent>().Any(e => e.OwnerSeat == f.OwnerSeat && e.Kind == n.Kind)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoNameSpentEvent(f.Id, f.OwnerSeat, name.Kind));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouDongZhaoShunji, SkillProgramEffectOp.DrawMouDongZhaoYishiLoss)]
    internal void ReturnMouDongZhaoDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        var r = f.MouDongZhao;
        if (r is not { Stage: MouDongZhaoStage.Drawing, Draw: null } || !ValidMouDongZhao(f) || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.RequestedDraw)
            throw new InvalidOperationException("OL Dong Zhao's native draw must return to its single issued owner.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Stage = MouDongZhaoStage.DrawChildren, Draw = draw, GiftCount = draw.ActualDrawIds.Count, Before = draw.SequenceBefore, After = draw.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoDrawReturnedEvent(f.Id, draw.RequestedCount, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter, draw.AttemptFrameId));
        SealMouDongZhaoMovements(f.Id, draw.SequenceBefore);
        ContinueMouDongZhao(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouDongZhaoShunji, SkillProgramEffectOp.ResolveMouDongZhaoYishi, SkillProgramEffectOp.DrawMouDongZhaoYishiLoss)]
    internal PromptChoice AiMouDongZhao(PendingDecision prompt, ProgramSkillFrame f)
    {
        var r = f.MouDongZhao!;
        if (r.Stage == MouDongZhaoStage.ChoosingDraw)
            return prompt.Choices.Single(c => c.Parameters["verb"] == "draw-2");
        if (r.Stage == MouDongZhaoStage.ChoosingGift)
            return prompt.Choices.FirstOrDefault(c => c.Parameters["verb"] == "gift-confirm") ?? prompt.Choices.First(c => c.Parameters["verb"] == "gift-toggle" && !r.Selected.Contains(c.Cards.Single()));
        return AiMouYishi(prompt, f);
    }

    SkillProgramStepOutcome IMouDongZhaoHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation switch
    {
        SkillProgramEffectOp.ResolveMouDongZhaoShunji => BeginMouShunji(frame),
        SkillProgramEffectOp.ResolveMouDongZhaoYishi => BeginMouYishi(frame),
        _ => BeginMouYishiLoss(frame)};
}
