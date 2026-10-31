#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoOlSpRuntime
{
    internal const string MubingReveal = "mubing-public-reveal";
    internal CardConversionSource SpZhangLiaoSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.ISpZhangLiaoProgramHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginSpZhangLiao(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26070)]
    internal bool CanOfferSpZhangLiao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.AwakenMubing))
            return !HasDiaolingAwakened(c.OwnerSeat) && (MubingMilitaryCount(c.OwnerSeat) >= 6 || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(c.OwnerSeat, c.SkillId, c.BindingId));
        if (!t.Effects.Any(e => e.Op == SkillProgramEffectOp.PreventDamageForHighestRankGift))
            return true;
        return context is { SourceSeat: { } source, TargetSeat: { } target } && source == c.OwnerSeat && target != source && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZiquDamagePreventedEvent>().Any(e => e.Source.OwnerSeat == source && e.VictimSeat == target);
    }

    internal SkillProgramStepOutcome BeginSpZhangLiao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.AwakenMubing)
        {
            if (HasDiaolingAwakened(f.OwnerSeat) || MubingMilitaryCount(f.OwnerSeat) < 6 && !_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(f.OwnerSeat, f.SkillId, f.TriggerId))
                throw new InvalidOperationException("Diaoling has no six-card acquisition receipt or its exact Huishi awakening grant.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiaolingAwakenedEvent(f.Id, f.OwnerSeat, MubingMilitaryCount(f.OwnerSeat)));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.SpZhangLiao is not null || f.WindowContext is not { } context)
            throw new InvalidOperationException("The rank exchange requires one owning native program frame.");
        var ziqu = op == SkillProgramEffectOp.PreventDamageForHighestRankGift;
        var r = new SpZhangLiaoReceipt(SpZhangLiaoSource(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, ziqu ? SpZhangLiaoStage.HighestCard : SpZhangLiaoStage.Payment, HasDiaolingAwakened(f.OwnerSeat), VictimSeat: ziqu ? context.TargetSeat : null, SequenceBefore: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, SequenceAfter: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r });
        if (ziqu)
        {
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZiquDamagePreventedEvent(f.Id, r.Source, r.VictimSeat!.Value, r.ParentFrameId));
        }
        else
        {
            _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, r.Upgraded ? 4 : 3, null, MubingReveal, SkillProgramCardSetVisibility.Public);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = f.SpZhangLiao!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        }

        ContinueSpZhangLiao(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainSpZhangLiaoChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueSpZhangLiao(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SpZhangLiao: { } r } f && f.Id == id)
        {
            if (DrainSpZhangLiaoChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.SpZhangLiao!;
            if (r.Stage is SpZhangLiaoStage.HighestChildren or SpZhangLiaoStage.CleanupChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CleanupSpZhangLiao(f);
                return;
            }

            switch (r.Stage)
            {
                case SpZhangLiaoStage.PaymentChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.Obtain, Selected = [] } });
                    break;
                case SpZhangLiaoStage.ObtainChildren:
                    if (!r.Upgraded)
                    {
                        CleanupSpZhangLiao(f);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.GiftCards, Selected = [] } });
                    break;
                case SpZhangLiaoStage.GiftChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.GiftCards, Selected = [] } });
                    break;
            }

            PublishSpZhangLiao(f);
            return;
        }
    }

    internal IReadOnlyList<PromptChoice> SpZhangLiaoChoices(ProgramSkillFrame f)
    {
        var r = f.SpZhangLiao!;
        PromptChoice Choice(string action, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"sp-zhang-liao.{(f.Id)}.{(r.Stage)}.{(action)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "sp-zhang-liao", ["option"] = action });
        var result = new List<PromptChoice>();
        if (r.Stage == SpZhangLiaoStage.HighestCard)
        {
            var victim = _coreMatchParticipantsPlayersAccess._players[r.VictimSeat!.Value];
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(victim).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(victim)).ToArray();
            var max = cards.Length == 0 ? 0 : cards.Max(c => c.Rank);
            return Array.AsReadOnly(cards.Where(c => c.Rank == max).Select(c => Choice("highest", $"交出【{(c.DisplayName)}】 {(c.RankText)}", [c.Id])).ToArray());
        }

        if (r.Stage == SpZhangLiaoStage.GiftTarget)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => Choice("recipient", $"交给 {(p.Name)}", targets: [p.Seat])).ToArray());
        IEnumerable<int> available = r.Stage switch
        {
            SpZhangLiaoStage.Payment => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, c, CardLocation.Hand(f.OwnerSeat), OwnedCardMoveIntent.Discard)).Select(c => c.Id),
            SpZhangLiaoStage.Obtain => MubingAvailableReveal(f),
            SpZhangLiaoStage.GiftCards => MubingAvailableGifts(f),
            _ => []
        };
        foreach (var id in available)
        {
            var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var picked = r.Selected.Contains(id);
            if (r.Stage == SpZhangLiaoStage.Obtain && !picked && r.SpentRank + c.Rank > r.Budget)
                continue;
            result.Add(Choice("card", $"{((picked ? "取消" : "选择"))}【{(c.DisplayName)}】 {(c.RankText)}", [id]));
        }

        if (r.Stage != SpZhangLiaoStage.GiftCards || r.Selected.Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat))
            result.Add(Choice("confirm", r.Stage == SpZhangLiaoStage.Payment ? $"弃置所选手牌（点数和 {(r.Selected.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank))}）" : r.Stage == SpZhangLiaoStage.Obtain ? $"获得所选牌（点数和 {(r.SpentRank)} / {(r.Budget)}）" : "分配所选牌", r.Selected));
        if (r.Stage == SpZhangLiaoStage.GiftCards)
            result.Add(Choice("finish", "保留其余牌，结束募兵"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishSpZhangLiao(ProgramSkillFrame f)
    {
        var r = f.SpZhangLiao!;
        var choices = SpZhangLiaoChoices(f);
        if (choices.Count == 0)
        {
            if (r.Stage == SpZhangLiaoStage.HighestCard)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.HighestChildren } });
                ContinueSpZhangLiao(f.Id);
            }
            else
                CleanupSpZhangLiao(f);
            return;
        }

        var chooser = r.Stage == SpZhangLiaoStage.HighestCard ? r.VictimSeat!.Value : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var description = r.Upgraded && f.SkillId == "ol:mubing" ? "出牌阶段开始时，你可以亮出牌堆顶四张牌，然后你可以弃置任意张手牌，获得任意张亮出的牌（弃置牌的点数之和不得小于获得牌的点数之和），然后你可以将以此法获得的牌以任意方式交给其他角色。" : skill.Description;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"{(skill.Name)}：{(r.Stage switch {SpZhangLiaoStage.Payment => "选择要弃置的手牌", SpZhangLiaoStage.Obtain => "选择要获得的亮出牌", SpZhangLiaoStage.GiftCards => "分配本次获得的牌或保留", SpZhangLiaoStage.GiftTarget => "选择收牌角色", _ => "交出一张点数最大的牌"})}", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, r.Upgraded && f.SkillId == "ol:mubing" ? "募兵 · 调令后" : skill.Name, description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("sp-zhang-liao")]
    internal void AnswerSpZhangLiao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SpZhangLiao: { } r } f || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], SpZhangLiaoChoices(f).Where(c => c.Id == choice.Id).ToArray()) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == SpZhangLiaoStage.HighestCard ? r.VictimSeat : f.OwnerSeat))
            throw new InvalidOperationException("The rank exchange lost its actual chooser, physical source or budget.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = choice.Parameters["option"];
        if (option == "card")
        {
            var id = choice.Cards.Single();
            var selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = r with { Selected = selected, SpentRank = r.Stage == SpZhangLiaoStage.Obtain ? selected.Sum(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c).Rank) : r.SpentRank } });
        }
        else if (option == "confirm" && r.Stage == SpZhangLiaoStage.Payment)
        {
            var budget = r.Selected.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.PaymentChildren, Budget = budget, SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            MoveSpZhangLiaoCards(f.Id, r.Selected, CardLocation.DiscardPile, "skill-program.ol:mubing.DiscardSelected");
        }
        else if (option == "confirm" && r.Stage == SpZhangLiaoStage.Obtain)
        {
            var ids = r.Selected;
            var military = ids.Count(id => IsMubingMilitaryCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)));
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.ObtainChildren, Obtained = ids, Selected = [], SequenceBefore = before } });
            MoveSpZhangLiaoCards(f.Id, ids, CardLocation.Hand(f.OwnerSeat), "skill-program.ol:mubing.ObtainSelected");
            var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.To == CardLocation.Hand(f.OwnerSeat) && m.From == CardLocation.Processing && ids.Contains(m.CardId)).ToArray();
            if (actual.Length != ids.Count)
                throw new InvalidOperationException("Mubing did not acquire its exact paid physical set.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MubingCardsAcquiredEvent(f.Id, r.Source, ids.Count, military, r.Budget, r.SpentRank, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }
        else if (option == "confirm" && r.Stage == SpZhangLiaoStage.GiftCards)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.GiftTarget } });
        else if (option == "recipient")
        {
            var recipient = choice.Targets.Single();
            if (r.Selected.Any(id => !MubingAvailableGifts(f).Contains(id)))
                throw new InvalidOperationException("Only this acquisition's remaining physical cards may be gifted.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.GiftChildren, Obtained = r.Obtained.Where(id => !r.Selected.Contains(id)).ToArray() } });
            MoveSpZhangLiaoCards(f.Id, r.Selected, CardLocation.Hand(recipient), "skill-program.ol:mubing.GiveSelected");
        }
        else if (option == "highest")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = r with { Stage = SpZhangLiaoStage.HighestChildren, Selected = choice.Cards } });
            MoveSpZhangLiaoCards(f.Id, choice.Cards, CardLocation.Hand(f.OwnerSeat), "skill-program.ol:ziqu.GiveSelected");
        }
        else if (option == "finish")
        {
            CleanupSpZhangLiao(f);
            return;
        }
        else
            throw new InvalidOperationException("Unknown rank exchange answer.");
        ContinueSpZhangLiao(f.Id);
    }

    internal void CleanupSpZhangLiao(ProgramSkillFrame f)
    {
        if (f.SpZhangLiao!.Stage != SpZhangLiaoStage.CleanupChildren)
        {
            var remaining = f.CardSetBindings.Any(b => b.Name == MubingReveal) ? MubingAvailableReveal(f) : [];
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = f.SpZhangLiao with { Stage = SpZhangLiaoStage.CleanupChildren, Selected = [] } });
            MoveSpZhangLiaoCards(f.Id, remaining, CardLocation.DiscardPile, "skill-program.ol:mubing.reveal-cleanup");
        }

        ContinueSpZhangLiao(f.Id);
    }

    internal void MoveSpZhangLiaoCards(long id, IReadOnlyList<int> cards, CardLocation destination, string reason)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = f.SpZhangLiao!with { SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (destination == CardLocation.DiscardPile && reason == "skill-program.ol:mubing.DiscardSelected")
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, cards, new(reason));
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, destination, new(reason), (f.SpZhangLiao!.Stage is SpZhangLiaoStage.HighestChildren or SpZhangLiaoStage.GiftChildren ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.SpZhangLiao!.Stage == SpZhangLiaoStage.HighestChildren ? f.SpZhangLiao.VictimSeat!.Value : f.OwnerSeat, destination.OwnerSeat!.Value, cards, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) : null));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpZhangLiao = f.SpZhangLiao!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26070)]
    internal bool ResumeSpZhangLiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SpZhangLiao: not null } f || f.Id != id)
            return false;
        ContinueSpZhangLiao(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26070)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26070)]
    internal bool ContinueIssuedSpZhangLiao(ProgramSkillFrame f) => f.SpZhangLiao is not null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26070)]
    internal void AssertSpZhangLiao(ProgramSkillFrame f)
    {
        if (f.SpZhangLiao is not { } r)
            return;
        if (r.Source != SpZhangLiaoSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.ParentFrameId != f.WindowContext?.ParentFrameId || r.Budget < 0 || r.SpentRank < 0 || r.SpentRank > r.Budget || r.Selected.Distinct().Count() != r.Selected.Count || r.Obtained.Distinct().Count() != r.Obtained.Count || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore && r.SequenceAfter != 0)
            throw new InvalidOperationException("SP Zhang Liao lost the owning native window or immutable rank payment.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMubingRankExchange, SkillProgramEffectOp.PreventDamageForHighestRankGift)]
    internal PromptChoice AiSpZhangLiao(PendingDecision d, ProgramSkillFrame f)
    {
        var r = f.SpZhangLiao!;
        if (r.Stage == SpZhangLiaoStage.Payment)
        {
            var target = MubingAvailableReveal(f).Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank);
            var current = r.Selected.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank);
            return d.Choices.Where(c => c.Parameters["option"] == "card" && !r.Selected.Contains(c.Cards.Single())).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault()is { } add && current < target ? add : d.Choices.Single(c => c.Parameters["option"] == "confirm");
        }

        if (r.Stage == SpZhangLiaoStage.Obtain)
            return d.Choices.FirstOrDefault(c => c.Parameters["option"] == "card" && !r.Selected.Contains(c.Cards.Single())) ?? d.Choices.Single(c => c.Parameters["option"] == "confirm");
        if (r.Stage == SpZhangLiaoStage.GiftCards)
            return d.Choices.Single(c => c.Parameters["option"] == "finish");
        return d.Choices.First();
    }
}
