#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSimultaneousPindianRuntime
{
    internal const string SimultaneousDiscardReason = "skill-program.simultaneous-pindian.discard", SimultaneousDrawReason = "skill-program.simultaneous-pindian.draw";
    internal CardConversionSource SimultaneousSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome StartSimultaneousPindian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TriggerId is not null || f.InstructionIndex != 1 || f.SimultaneousPindian is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedCardIds.Count > 1 || f.SelectedCardIds.Count == 0 && !_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || f.SelectedTargetSeats.Count is < 1 or > 3 || f.SelectedTargetSeats.Distinct().Count() != f.SelectedTargetSeats.Count || f.SelectedTargetSeats.Any(s => !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(f.OwnerSeat, s)) || f.SelectedCardIds.Count == 1 && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(c => c.Id == f.SelectedCardIds[0]))
            throw new InvalidOperationException("A simultaneous contest lost its exact own card, other hand participants or phase activation.");
        var id = f.SelectedCardIds.SingleOrDefault();
        var card = id == 0 ? null : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == id);
        var rank = card is null ? 0 : _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        var r = new SimultaneousPindianReceipt
        {
            ProgramFrameId = f.Id,
            Source = SimultaneousSource(f),
            GameplayHash = f.GameplayHash,
            TurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            PhaseInstanceId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            Opponents = f.SelectedTargetSeats,
            OriginalSourceCardId = id,
            Cards = [new(f.OwnerSeat, id, rank, rank)],
            Stage = SimultaneousPindianStage.RandomPolicies,
            RandomSelection = new(Array.AsReadOnly(f.SelectedTargetSeats.SelectMany(s => _corePindianRandomSelectionsOperations.CollectPindianRandomPolicies(f.OwnerSeat, s)).ToArray()), Array.AsReadOnly(Array.Empty<PindianRandomReceipt>()))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimultaneousPindianStartedEvent(f.Id, r.Source, r.Opponents));
        _coreSimultaneousPindianSelectionsOperations.ContinueSimultaneousRandomPolicies(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int SimultaneousChooser(ProgramSkillFrame f)
    {
        var r = f.SimultaneousPindian!;
        if (r.Stage == SimultaneousPindianStage.Selecting)
            return r.Opponents[r.SelectionIndex];
        if (r.Stage == SimultaneousPindianStage.RandomPolicies)
            return r.RandomSelection!.Candidates[r.RandomSelection.Index].OwnerSeat;
        if (r.Stage == SimultaneousPindianStage.Claiming)
            return r.Claims!.Claims[r.Claims.Index].OwnerSeat;
        var pair = SimultaneousResult(f);
        return SimultaneousNonwinners(pair)[r.ConsequenceIndex];
    }

    internal static int[] SimultaneousNonwinners(PindianResult p) => RuntimeSkills.RuntimeRules_Core_SimultaneousPindian.SimultaneousNonwinners(p);
    internal PindianResult SimultaneousResult(ProgramSkillFrame f)
    {
        var r = f.SimultaneousPindian!;
        var source = r.Cards.Single(c => c.Seat == f.OwnerSeat);
        var opponent = r.Cards.Single(c => c.Seat == r.Opponents[r.ComparisonIndex]);
        return new(source.Seat, opponent.Seat, source.CardId, opponent.CardId, source.Rank, opponent.Rank);
    }

    internal IReadOnlyList<PromptChoice> SimultaneousChoices(ProgramSkillFrame f)
    {
        var r = f.SimultaneousPindian!;
        var seat = SimultaneousChooser(f);
        Dictionary<string, string> P(string branch) => new()
        {
            ["program-action"] = "simultaneous-pindian",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch,
            ["comparison"] = r.ComparisonIndex.ToString(CultureInfo.InvariantCulture),
            ["consequence"] = r.ConsequenceIndex.ToString(CultureInfo.InvariantCulture),
            ["selection"] = r.SelectionIndex.ToString(CultureInfo.InvariantCulture)
        };
        if (r.Stage == SimultaneousPindianStage.Selecting)
        {
            var selection = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).OrderBy(c => c.Id).Select(c => new PromptChoice(new($"simultaneous.{(f.Id)}.select.{(seat)}.{(c.Id)}"), $"以【{(c.DisplayName)}】（{(_corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], c))}）参与拼点", [c.Id], [], P("commit"))).ToList();
            if (_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[seat]))
                selection.Add(new(new($"simultaneous.{(f.Id)}.select.{(seat)}.maximum"), "弃置牌后使用牌堆最大点数的牌拼点", [], [], P("maximum")));
            if (_coreSimultaneousPindianSelectionsOperations.CanOfferSimultaneousTop(f, seat))
                selection.Add(new(new($"simultaneous.{(f.Id)}.select.{(seat)}.top"), "使用牌堆顶的牌拼点", [], [], P("top")));
            return Array.AsReadOnly(selection.ToArray());
        }

        if (r.Stage == SimultaneousPindianStage.RandomPolicies)
            return Array.AsReadOnly(new[] { new PromptChoice(new($"simultaneous.{(f.Id)}.random.{(r.RandomSelection!.Index)}"), "令对方使用随机手牌拼点", [], [], P("random")), new PromptChoice(new($"simultaneous.{(f.Id)}.random-decline.{(r.RandomSelection.Index)}"), "不发动", [], [], P("random-decline")) });
        if (r.Stage == SimultaneousPindianStage.Claiming)
        {
            var claim = r.Claims!.Claims[r.Claims.Index];
            var ids = _coreSimultaneousPindianClaimsOperations.Value.AvailableSimultaneousClaimCards(claim);
            return Array.AsReadOnly(new[] { new PromptChoice(new($"simultaneous.{(f.Id)}.{(r.ComparisonIndex)}.claim.{(r.Claims.Index)}"), $"获得拼点牌（{(ids.Length)}张）", ids, [], P("claim")), new PromptChoice(new($"simultaneous.{(f.Id)}.{(r.ComparisonIndex)}.claim-decline.{(r.Claims.Index)}"), "不获得拼点牌", [], [], P("claim-decline")) });
        }

        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, seat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c =>
        {
            var parameters = c.Parameters.ToDictionary(p => p.Key, p => p.Value, StringComparer.Ordinal);
            foreach (var(key, value)in P("discard"))
                parameters[key] = value;
            return new PromptChoice(new($"simultaneous.{(f.Id)}.{(r.ComparisonIndex)}.{(r.ConsequenceIndex)}.discard.{(c.Parameters["source-zone"])}.{(c.Parameters["slot-index"])}"), c.Label, c.Cards, [], parameters);
        }).ToList();
        choices.Add(new(new($"simultaneous.{(f.Id)}.{(r.ComparisonIndex)}.{(r.ConsequenceIndex)}.draw"), "令拼点发起者摸一张牌", [], [], P("draw")));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishSimultaneousChoice(ProgramSkillFrame f)
    {
        var choices = SimultaneousChoices(f);
        var seat = SimultaneousChooser(f);
        var skillId = f.SimultaneousPindian!.Stage == SimultaneousPindianStage.Claiming ? f.SimultaneousPindian.Claims!.Claims[f.SimultaneousPindian.Claims.Index].Source.SkillId : f.SimultaneousPindian.Stage == SimultaneousPindianStage.RandomPolicies ? f.SimultaneousPindian.RandomSelection!.Candidates[f.SimultaneousPindian.RandomSelection.Index].Source.SkillId : f.SkillId;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("A committed contest participant lost its hand before the common reveal.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, f.SimultaneousPindian!.Stage == SimultaneousPindianStage.Selecting ? "请选择一张手牌参与同时拼点；所有角色选定后共同亮出。" : f.SimultaneousPindian.Stage == SimultaneousPindianStage.RandomPolicies ? "请选择是否令对方使用随机手牌拼点；共同亮牌前不公开身份。" : f.SimultaneousPindian.Stage == SimultaneousPindianStage.Claiming ? "请选择是否获得本次比较的拼点牌。" : "拼点未赢，请选择弃置一张牌或令发起者摸一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(skillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("simultaneous-pindian")]
    internal void ResolveSimultaneousPindianChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A simultaneous answer lost its owning program.");
        var r = f.SimultaneousPindian!;
        var seat = SimultaneousChooser(f);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !SimultaneousChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("A simultaneous answer is no longer its exact private published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == SimultaneousPindianStage.RandomPolicies)
        {
            _coreSimultaneousPindianSelectionsOperations.ResolveSimultaneousRandomPolicy(f, choice);
            return;
        }

        if (r.Stage == SimultaneousPindianStage.Claiming)
        {
            _coreSimultaneousPindianClaimsOperations.Value.ResolveSimultaneousClaim(f, choice);
            return;
        }

        if (r.Stage == SimultaneousPindianStage.Selecting)
        {
            _coreSimultaneousPindianSelectionsOperations.CommitSimultaneousSelection(f, seat, choice);
            return;
        }

        var branch = choice.Parameters["branch"];
        var beforePayment = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        int? paidCard = null;
        CardLocation? from = null;
        if (branch == "discard")
        {
            if (!Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var slot) || choice.Parameters.GetValueOrDefault("card-owner-seat") != seat.ToString(CultureInfo.InvariantCulture))
                throw new InvalidOperationException("A simultaneous discard lost its actual payer, source zone or published slot.");
            from = new(zone, seat);
            var cards = _cardZonesOperations._cardZones.CardsAt(from.Value);
            if (slot < 0 || slot >= cards.Count || choice.Cards.Count > 1 || choice.Cards.Count == 1 && choice.Cards[0] != cards[slot].Id || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, cards[slot], from.Value, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("A simultaneous discard cannot replace or forbid its exact current entity.");
            paidCard = cards[slot].Id;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.MovementChildren, PaymentSeat = seat, PaidCardId = paidCard, PaidFrom = from, PaymentKind = branch, MovementBefore = beforePayment, MovementAfter = beforePayment }, PendingMovementContinuation = new(seat, 0, null) });
        if (paidCard is { } discard)
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, seat, _coreAdvancedLifecycleOperations.GetAdvancedCard(discard), from!.Value, new(SimultaneousDiscardReason));
        else if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(SimultaneousDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.SimultaneousPindian!with
        {
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimultaneousPindianConsequencePaidEvent(f.Id, r.ComparisonIndex, r.ConsequenceIndex, seat, branch, paidCard, beforePayment, r.MovementAfter));
        if (!DrainSimultaneousChildren(f))
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainSimultaneousChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 21500)]
    internal bool ResumeSimultaneousPindian(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SimultaneousPindian: { } r } f || f.Id != id)
            return false;
        if (r.Stage == SimultaneousPindianStage.RandomPolicies)
        {
            _coreSimultaneousPindianSelectionsOperations.ContinueSimultaneousRandomPolicies(f);
            return true;
        }

        if (r.Stage == SimultaneousPindianStage.Selecting)
        {
            _coreSimultaneousPindianSelectionsOperations.ContinueSimultaneousSelection(f);
            return true;
        }

        if (r.Stage == SimultaneousPindianStage.TopRefillChildren)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SimultaneousPindian = r with { Stage = SimultaneousPindianStage.Selecting, PaymentKind = null } });
            _coreSimultaneousPindianSelectionsOperations.ContinueSimultaneousSelection(f);
            return true;
        }

        if (r.Stage is SimultaneousPindianStage.Consequence or SimultaneousPindianStage.Claiming)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSimultaneousChoice(f);
            return true;
        }

        if (r.Stage == SimultaneousPindianStage.Revealing)
            return true;
        if (r.Stage == SimultaneousPindianStage.RevealChildren)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.Revealing, PaymentKind = null } });
            _corePindianRevealWindowsOperations.BeginPindianRevealWindow(f.Id, true, r.Cards);
            return true;
        }

        if (r.Stage == SimultaneousPindianStage.Cleanup)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimultaneousPindianCompletedEvent(f.Id, SimultaneousSource(f), r.ComparisonIndex));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = null, PendingMovementContinuation = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        if (r.Stage == SimultaneousPindianStage.DeathCleanup)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.DeathChildren, PaymentKind = null } });
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillCauseDeathDeclaredEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.OwnerSeat));
                _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
                return true;
            }
        }

        if (r.Stage == SimultaneousPindianStage.MovementChildren)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.Comparing, ConsequenceIndex = r.ConsequenceIndex + 1, PaymentSeat = null, PaidCardId = null, PaidFrom = null, PaymentKind = null } });
        }

        if (r.Stage == SimultaneousPindianStage.ClaimMovementChildren)
        {
            if (DrainSimultaneousChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.Comparing, Claims = r.Claims!with { Index = r.Claims.Index + 1 }, PaymentSeat = null, PaymentKind = null } });
        }

        if (r.Stage == SimultaneousPindianStage.DeathChildren)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { Stage = SimultaneousPindianStage.Comparing } });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishSimultaneousPindian(f);
            return true;
        }

        while (r.ComparisonIndex < r.Opponents.Count)
        {
            var pair = SimultaneousResult(f);
            if (!r.ComparisonIssued)
            {
                r = r with
                {
                    ComparisonIssued = true
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianResultDeterminedEvent(f.Id, f.SkillId, pair));
            }

            if (_characterZhangLiaoMobileGodOperations.TryBeginMobileZhitiSimultaneousPrograms(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), pair))
                return true;
            if (_characterPangJiOlOperations.TryBeginDouyuSimultaneousWinDraws(f, pair))
                return true;
            if (_characterLuKangWumiaoOperations.TryBeginWuLuKangPostComparison(f, pair, r.ComparisonIndex))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.SimultaneousPindian!;
            if (r.Claims is null)
            {
                var claims = _coreSimultaneousPindianClaimsOperations.Value.FreezeSimultaneousClaims(f, pair);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { Claims = claims } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimultaneousPindianClaimsIssuedEvent(f.Id, r.ComparisonIndex, claims));
            }

            if (_characterJianYongBoundaryMobileOperations.TryBeginBoundaryJianYongSimultaneousZongshi(f))
                return true;
            if (_coreSimultaneousPindianClaimsOperations.Value.OfferSimultaneousClaim(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.SimultaneousPindian!;
            var losers = SimultaneousNonwinners(pair);
            while (r.ConsequenceIndex < losers.Length)
            {
                var loser = losers[r.ConsequenceIndex];
                if (!_coreMatchParticipantsPlayersAccess._players[loser].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { ConsequenceIndex = r.ConsequenceIndex + 1 } });
                    continue;
                }

                if (loser == f.OwnerSeat && !r.SelfMarkerIssued)
                {
                    r = r with
                    {
                        SelfMarkerIssued = true
                    };
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r });
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.RaoShe, 1);
                    if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RaoShe) == 7)
                    {
                        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { Stage = SimultaneousPindianStage.DeathCleanup, PaymentKind = "cleanup", MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                        CleanupSimultaneousPindianCards(f);
                        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = f.SimultaneousPindian!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                        if (!DrainSimultaneousChildren(f))
                            ResumeSimultaneousPindian(f.Id);
                        return true;
                    }
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { Stage = SimultaneousPindianStage.Consequence } });
                PublishSimultaneousChoice(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { ComparisonIndex = r.ComparisonIndex + 1, ConsequenceIndex = 0, ComparisonIssued = false, SelfMarkerIssued = false, Claims = null } });
        }

        FinishSimultaneousPindian(f);
        return true;
    }

    internal void CleanupSimultaneousPindianCards(ProgramSkillFrame f)
    {
        foreach (var c in f.SimultaneousPindian!.Cards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.PindianFinish);
    }

    internal void FinishSimultaneousPindian(ProgramSkillFrame f)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = f.SimultaneousPindian!with { Stage = SimultaneousPindianStage.Cleanup, PaymentKind = "cleanup", MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        CleanupSimultaneousPindianCards(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = f.SimultaneousPindian!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (!DrainSimultaneousChildren(f))
            ResumeSimultaneousPindian(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 21500)]
    internal bool ReturnSimultaneousMovement(ProgramSkillFrame f) => f.SimultaneousPindian is not null && ResumeSimultaneousPindian(f.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 20)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 20)]
    internal bool CanContinueSimultaneousPindian(ProgramSkillFrame f) => f.SimultaneousPindian is { Revealed: true } && IsSimultaneousReceiptSource(f);
    internal bool IsSimultaneousReceiptSource(ProgramSkillFrame f) => f.SimultaneousPindian is { } r && r.ProgramFrameId == f.Id && r.Source == SimultaneousSource(f) && r.GameplayHash == f.GameplayHash;
    [ProgramAiChoice(SkillProgramEffectOp.StartSimultaneousPindian)]
    internal PromptChoice SelectAiSimultaneousChoice(PendingDecision p, ProgramSkillFrame f)
    {
        var seat = SimultaneousChooser(f);
        if (f.SimultaneousPindian!.Stage == SimultaneousPindianStage.Selecting)
            return p.Choices.OrderByDescending(c => c.Parameters["branch"] == "maximum" ? 10 : c.Parameters["branch"] == "top" ? 9 : _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Single(card => card.Id == c.Cards.Single()))).ThenBy(c => c.Cards.FirstOrDefault()).First();
        if (f.SimultaneousPindian.Stage == SimultaneousPindianStage.RandomPolicies)
            return p.Choices.Single(c => c.Parameters["branch"] == "random");
        if (f.SimultaneousPindian.Stage == SimultaneousPindianStage.Claiming)
            return p.Choices.Single(c => c.Parameters["branch"] == "claim");
        return CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[seat], _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !p.Choices.Any(c => c.Parameters["branch"] == "discard") ? p.Choices.Single(c => c.Parameters["branch"] == "draw") : p.Choices.First(c => c.Parameters["branch"] == "discard");
    }

    SkillProgramStepOutcome ISimultaneousPindianHost.StartSimultaneousPindian(ProgramSkillFrame f) => StartSimultaneousPindian(f);
    void ISimultaneousPindianHost.AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId) => _corePindianRevealWindowsOperations.AddRaoshePindianRankAndRestoreGushe(f, skillId);
}
