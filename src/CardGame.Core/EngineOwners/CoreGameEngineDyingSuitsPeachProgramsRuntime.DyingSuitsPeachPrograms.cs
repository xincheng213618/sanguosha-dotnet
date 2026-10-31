#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDyingSuitsPeachProgramsRuntime
{
    internal static CardConversionSource DyingSuitsSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_DyingSuitsPeachPrograms.DyingSuitsSource(f);
    internal static string DyingSuitsDrawReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_DyingSuitsPeachPrograms.DyingSuitsDrawReason(f);
    internal static string DyingSuitsDiscardReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_DyingSuitsPeachPrograms.DyingSuitsDiscardReason(f);
    internal bool HasIssuedDyingSuitsRound(int owner, string skill) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DyingSuitsDrawIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.ActualRound == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal int[] DyingSuitsRecipients(int owner, int victim) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && p.Seat != victim).Select(p => p.Seat).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 70)]
    internal bool CanRunDyingSuitsAndEndingHistory(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawThenDiscardSuitsForDyingPeach))
            return context.Window == SkillProgramTriggerWindow.DyingEntering && context.TargetSeat is { } victim && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreMatchParticipantsPlayersAccess._players[victim].Hp <= 0 && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && !HasIssuedDyingSuitsRound(candidate.OwnerSeat, candidate.SkillId) && DyingSuitsRecipients(candidate.OwnerSeat, victim).Length > 0;
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.UseOwnPlayHistoryAtEnding))
            return context.Window == SkillProgramTriggerWindow.TurnEnding && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreEndingHistoricalUsesOperations.OwnPlayHistory(candidate.OwnerSeat).Count > 0;
        return true;
    }

    internal SkillProgramStepOutcome BeginDyingSuits(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.DyingSuits is not null || !_coreDyingOwnedCardSelectionsOperations.ExactDyingOwnedCardEntry(f, out var dying, out var entry))
            throw new InvalidOperationException("Dying suits must start on the exact original entry/candidate/victim.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0 || HasIssuedDyingSuitsRound(f.OwnerSeat, f.SkillId))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var r = new DyingSuitsReceipt(1, DyingSuitsSource(f), f.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, entry.Id, dying.Id, dying.VictimSeat, _coreDyingSuitsAndHistoricalUseObserversOperations.CaptureDyingSuitsOriginalCursor(dying, entry), DyingSuitsStage.Recipient, [], [], []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DyingSuits = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingSuitsOriginalCursorIssuedEvent(f.Id, CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime.DyingSuitsCursorHash(r.OriginalCursor)));
        PublishDyingSuitsChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DyingSuitsChoices(ProgramSkillFrame f)
    {
        var r = f.DyingSuits!;
        var result = new List<PromptChoice>();
        PromptChoice Choice(string key, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"dying-suits.{(f.Id)}.{(key)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "dying-suits", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = key });
        if (r.Stage == DyingSuitsStage.Recipient)
            foreach (var seat in DyingSuitsRecipients(f.OwnerSeat, r.VictimSeat))
                result.Add(Choice($"recipient-{(seat)}", $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 摸四张牌，然后弃置四张牌", [], [seat]));
        else if (r.Stage == DyingSuitsStage.SelectingDiscard && r.RecipientSeat is { } payer)
            foreach (var c in DyingSuitsDiscardCandidates(f, payer).Where(c => !r.SelectedCardIds.Contains(c.Id)))
                result.Add(Choice($"discard-{(c.Id)}", $"弃置【{(c.DisplayName)}】（{(r.SelectedCardIds.Count + 1)}/{(r.RequiredDiscardCount)}）", [c.Id], []));
        return Array.AsReadOnly(result.ToArray());
    }

    internal Card[] DyingSuitsDiscardCandidates(ProgramSkillFrame f, int payer) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[payer]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[payer])).Where(c => !c.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(payer, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(payer, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !(payer == f.OwnerSeat && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(payer, f.SkillId, f.SkillInstanceId, c))).OrderBy(c => c.Id).ToArray();
    [ProgramAiChoice(SkillProgramEffectOp.DrawThenDiscardSuitsForDyingPeach)]
    internal PromptChoice SelectAiDyingSuits(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.DyingSuits!;
        var recipientChoice = r.Stage == DyingSuitsStage.Recipient;
        var suits = recipientChoice ? new Dictionary<int, Suit>() : DyingSuitsDiscardCandidates(f, decision.PlayerSeat).Where(c => decision.Choices.Any(choice => choice.Cards.Contains(c.Id))).ToDictionary(c => c.Id, c => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat], c));
        Suit[] selectedSuits = recipientChoice ? [] : r.SelectedCardIds.Select(id => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat], _coreCardAttackStateOperations.GetAttackCard(id))).ToArray();
        var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseDyingSuits(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, r.VictimSeat, recipientChoice, suits, selectedSuits, r.RequiredDiscardCount, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return choice;
    }

    internal void PublishDyingSuitsChoice(ProgramSkillFrame f)
    {
        var r = f.DyingSuits!;
        var seat = r.Stage == DyingSuitsStage.Recipient ? f.OwnerSeat : r.RecipientSeat!.Value;
        var choices = DyingSuitsChoices(f);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, r.Stage == DyingSuitsStage.Recipient ? "请选择另一名其他角色。" : "请选择本次实际弃置的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("dying-suits")]
    internal void ResolveDyingSuitsChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.DyingSuits!;
        var expected = DyingSuitsChoices(f).SingleOrDefault(c => c.Id == choice.Id);
        if (expected is null || !expected.Cards.SequenceEqual(choice.Cards) || !expected.Targets.SequenceEqual(choice.Targets))
            throw new InvalidOperationException("Dying suits lost its private exact published selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == DyingSuitsStage.Recipient)
        {
            var seat = choice.Targets.Single();
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].Hp > 0 || HasIssuedDyingSuitsRound(f.OwnerSeat, f.SkillId))
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, false);
                return;
            }

            var before = _coreUniqueHpPeerProgramsOperations.RecipientContestSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DyingSuits = r with { Stage = DyingSuitsStage.Drawing, RecipientSeat = seat, DrawBefore = before, DrawAfter = before }, PendingMovementContinuation = new(seat, 0, null) });
            var cards = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 4, log: true, reason: new(DyingSuitsDrawReason(f)));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.DyingSuits!with
            {
                DrawAfter = _coreUniqueHpPeerProgramsOperations.RecipientContestSequence,
                ActualDrawCount = cards.Count
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DyingSuits = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingSuitsDrawIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ActualRound, r.ActualTurn, r.TurnOwnerSeat, r.EntryFrameId, r.DyingFrameId, r.VictimSeat, seat, r.DrawBefore, r.DrawAfter, r.ActualDrawCount));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var payer = r.RecipientSeat!.Value;
        var id = choice.Cards.Single();
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        var ids = r.SelectedCardIds.Append(id).ToArray();
        var locations = r.SelectedLocations.Append(location).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DyingSuits = r with { SelectedCardIds = ids, SelectedLocations = locations } });
        if (ids.Length < r.RequiredDiscardCount)
        {
            PublishDyingSuitsChoice(f);
            return;
        }

        r = f.DyingSuits!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[payer].IsAlive || ids.Where((card, i) => _cardZonesOperations._cardZones.GetLocation(card) != locations[i]).Any() || ids.Any(card => !DyingSuitsDiscardCandidates(f, payer).Any(c => c.Id == card)))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var suits = ids.Select(card => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[payer], _coreCardAttackStateOperations.GetAttackCard(card))).ToArray();
        var costBefore = _coreUniqueHpPeerProgramsOperations.RecipientContestSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DyingSuits = r with { Stage = DyingSuitsStage.Discarding, DiscardSuits = suits, DiscardBefore = costBefore, DiscardAfter = costBefore }, PendingMovementContinuation = new(payer, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, payer, ids, new(DyingSuitsDiscardReason(f)), (_, records) =>
        {
            var root = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = root.DyingSuits!with
            {
                DiscardAfter = records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(root with { DyingSuits = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingSuitsDiscardPaidEvent(f.Id, payer, paid.DiscardBefore, paid.DiscardAfter, ids, suits));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 410)]
    internal bool ResumeDyingSuits(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.DyingSuits is not { } r)
            return false;
        if (r.Stage is DyingSuitsStage.Recipient or DyingSuitsStage.SelectingDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDyingSuitsChoice(f);
            return true;
        }

        if (r.Stage is DyingSuitsStage.Drawing or DyingSuitsStage.Discarding)
        {
            if (TryDrainDyingSuits(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            var recipient = r.RecipientSeat!.Value;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            if (r.Stage == DyingSuitsStage.Drawing)
            {
                var count = Math.Min(4, DyingSuitsDiscardCandidates(f, recipient).Length);
                if (count == 0)
                {
                    _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DyingSuits = r with { Stage = DyingSuitsStage.SelectingDiscard, RequiredDiscardCount = count } });
                PublishDyingSuitsChoice(f);
                return true;
            }

            if (r.SelectedCardIds.Count != 4 || r.DiscardSuits.Distinct().Count() != 4 || r.DiscardSuits.Contains(Suit.None) || _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].Hp > 0 || !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(recipient, r.VictimSeat) || _coreCardUseModulesOperations.IsCardUseForbidden(recipient, CardKind.Peach, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(recipient, r.VictimSeat, CardKind.Peach) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[r.VictimSeat], CardKind.Peach, Suit.None) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(recipient, r.VictimSeat, Suit.None))
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            IssueDyingSuitsPeach(f);
            return true;
        }

        if (r.Stage == DyingSuitsStage.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        throw new InvalidOperationException("A dying-suits Peach cannot resume its root before its typed card-use return.");
    }

    internal bool TryDrainDyingSuits(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 430)]
    internal bool ReturnDyingSuitsMovement(ProgramSkillFrame f)
    {
        if (f.DyingSuits is null)
            return false;
        if (!IsDyingSuitsMovement(f, null, f.PendingMovementContinuation!))
            throw new InvalidOperationException("Dying suits lost its paid movement return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    internal bool IsDyingSuitsMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => pending is not null && f.DyingSuits is { Stage: DyingSuitsStage.Drawing or DyingSuitsStage.Discarding, RecipientSeat: { } recipient } && (effect is null || effect.Op == SkillProgramEffectOp.DrawThenDiscardSuitsForDyingPeach) && pending.SubjectSeat == recipient && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreDyingSuitsAndHistoricalUseObserversOperations.ValidDyingSuitsReceipt(f);
    internal void IssueDyingSuitsPeach(ProgramSkillFrame f)
    {
        var r = f.DyingSuits!;
        var actor = r.RecipientSeat!.Value;
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor, actor, null, null, null, CardKind.Peach, [r.VictimSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var ret = new DyingSuitsPeachReturn(f.Id, f.InstructionIndex, r.Source, f.GameplayHash, r.DyingFrameId, r.VictimSeat, actor, useId, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DyingSuits = r with { Stage = DyingSuitsStage.PeachIssued, PeachReturn = ret } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor, 0, CardKind.Peach, [r.VictimSeat], PhysicalCardIds: []) { Action = action, DyingSuitsPeachReturn = ret });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingSuitsPeachIssuedEvent(ret));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Peach, actor));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, [r.VictimSeat]));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor, CardKind.Peach);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor, CardKind.Peach);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(0, SimpleCardUseEffect.Recovery));
    }

    internal bool ContinueDyingSuitsPeach(long id, ProgramSimpleCardContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.DyingSuitsPeachReturn is not { } ret)
            return false;
        if (continuation.CardId != 0 || continuation.Effect != SimpleCardUseEffect.Recovery || !_coreDyingSuitsAndHistoricalUseObserversOperations.ValidDyingSuitsPeachUse(use, ret))
            throw new InvalidOperationException("Dying suits lost its genuine zero-material Peach producer.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[ret.ActorSeat].IsAlive)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(_coreMatchParticipantsPlayersAccess._players[ret.ActorSeat], _coreMatchParticipantsPlayersAccess._players[ret.VictimSeat], new(0, CardKind.Peach, Suit.None, 0), id, CardKind.Peach, 1, []);
        else
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, new(0, CardKind.Peach, Suit.None, 0), CardKind.Peach);
        return true;
    }

    [NativeUseReturn(280)]
    internal void ReturnDyingSuitsPeach(CardUseFrame use)
    {
        if (use.DyingSuitsPeachReturn is not { } ret)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(ret.ProgramFrameId);
        if (f.DyingSuits is not { Stage: DyingSuitsStage.PeachIssued } r || r.PeachReturn != ret || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Original-Dying Peach returned without its complete exact use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DyingSuits = r with { Stage = DyingSuitsStage.Complete } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingSuitsPeachReturnedEvent(ret));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    SkillProgramStepOutcome IDyingSuitsAndEndingHistoryHost.DrawDiscardSuits(ProgramSkillFrame f) => BeginDyingSuits(f);
    SkillProgramStepOutcome IDyingSuitsAndEndingHistoryHost.EndingHistoricalUses(ProgramSkillFrame f) => _coreEndingHistoricalUsesOperations.BeginEndingHistoricalUses(f);
}
