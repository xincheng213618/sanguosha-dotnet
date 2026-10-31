#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangMobileRuntime
{
    internal const string MobileDuanfaId = "mobile:duanfa", MobileDuanfaDiscard = "skill-program.mobile-duanfa.discard", MobileDuanfaDraw = "skill-program.mobile-duanfa.draw";
    internal static CardConversionSource MobileDuanfaSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillGrantPhaseExpiry? MobileDuanfaPhase(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0 ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal int MobileDuanfaSpent(int owner, SkillGrantPhaseExpiry phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDuanfaPaidEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MobileDuanfaId && e.Phase == phase).Sum(e => e.ActualCount);
    internal int MobileDuanfaRemaining(int owner) => MobileDuanfaPhase(owner)is { } phase ? Math.Max(0, _coreMatchParticipantsPlayersAccess._players[owner].MaxHp - MobileDuanfaSpent(owner, phase)) : 0;
    internal bool MobileDuanfaHasBlackCost(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Any(c => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, c)is Suit.Spade or Suit.Club && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner.Seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 82300, SkillProgramEffectOp.ResolveMobileDuanfa)]
    internal bool CanActivateMobileDuanfa(ProgramActivationLegalityContext g) => g.SkillId == MobileDuanfaId && g.Owner.IsAlive && MobileDuanfaRemaining(g.OwnerSeat) > 0 && MobileDuanfaHasBlackCost(g.Owner);
    SkillProgramStepOutcome IMobileZhouFangHost.Begin(ProgramSkillFrame f) => BeginMobileDuanfa(f);
    void IMobileZhouFangHost.PublishBait(ProgramSkillFrame f, string bind) => PublishMobileYoudiBait(f, bind);
    internal IReadOnlyList<PromptChoice> MobileDuanfaCosts(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1 && _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()))is Suit.Spade or Suit.Club).ToArray();
    internal SkillProgramStepOutcome BeginMobileDuanfa(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileDuanfaId || f.ActivationId != "phase-black-recycle" || f.WindowContext is not null || f.InstructionIndex != 1 || f.MobileDuanfa is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || MobileDuanfaPhase(f.OwnerSeat)is not { } phase || MobileDuanfaRemaining(f.OwnerSeat) < 1 || MobileDuanfaCosts(f).Count == 0)
            throw new InvalidOperationException("Mobile Duanfa requires its current real Play phase and at least one legal black HE payment.");
        var r = new MobileDuanfaReceipt(MobileDuanfaSource(f), f.GameplayHash, phase, MobileDuanfaStage.Cards);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileDuanfa = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuanfaIssuedEvent(f.Id, r.Source, r.Hash, phase));
        ContinueMobileDuanfa(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MobileDuanfaChoices(ProgramSkillFrame f)
    {
        var r = f.MobileDuanfa!;
        var choices = new List<PromptChoice>();
        void Add(string key, string label, string branch, IReadOnlyList<int> cards) => choices.Add(new(new($"mobile-duanfa.{(f.Id)}.{(key)}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "mobile-duanfa", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        foreach (var id in r.Cards)
            Add("unpick." + id, "取消已选牌", "unpick", [id]);
        if (r.Cards.Count < MobileDuanfaRemaining(f.OwnerSeat))
            foreach (var c in MobileDuanfaCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single())))
                Add("card." + c.Cards.Single(), c.Label, "card", c.Cards);
        if (r.Cards.Count > 0 && r.Cards.Count <= MobileDuanfaRemaining(f.OwnerSeat))
            Add("pay", "弃置所选黑色牌并摸等量牌", "pay", []);
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void FinishMobileDuanfa(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileDuanfa = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueMobileDuanfa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileDuanfa: { } r } f || f.Id != id)
            return;
        if (f.NativeDraw is not null)
            return;
        if (r.Stage == MobileDuanfaStage.Cards)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || MobileDuanfaPhase(f.OwnerSeat) != r.Phase)
            {
                FinishMobileDuanfa(f);
                return;
            }

            var choices = MobileDuanfaChoices(f);
            if (choices.Count == 0)
            {
                FinishMobileDuanfa(f);
                return;
            }

            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"断发：选择黑色手牌或装备牌，本阶段尚可弃置{(MobileDuanfaRemaining(f.OwnerSeat))}张。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MobileDuanfa!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.ActualCount == 0)
        {
            FinishMobileDuanfa(f);
            return;
        }
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileDuanfa = r with { Stage = MobileDuanfaStage.Drawing } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.ActualCount, new(MobileDuanfaDraw));
    }

    [ProgramAnswer("mobile-duanfa")]
    internal void AnswerMobileDuanfa(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Duanfa answer lost its original frame.");
        var r = f.MobileDuanfa!;
        if (r.Stage != MobileDuanfaStage.Cards || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MobileDuanfaChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duanfa's actual black HE payment or phase allowance changed.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileDuanfa = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } });
                break;
            case "unpick":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileDuanfa = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } });
                break;
            case "pay":
                var available = MobileDuanfaCosts(f).SelectMany(c => c.Cards).ToHashSet();
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                var spent = MobileDuanfaSpent(f.OwnerSeat, r.Phase);
                var limit = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
                if (MobileDuanfaPhase(f.OwnerSeat) != r.Phase || r.Cards.Count < 1 || r.Cards.Count > limit - spent || r.Cards.Any(c => !available.Contains(c)))
                    throw new InvalidOperationException("Duanfa cannot pay beyond its current actual Play-phase maximum-HP allowance.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileDuanfa = r with { Stage = MobileDuanfaStage.CostChildren, Limit = limit, SpentBefore = spent, Before = before, After = before } });
                var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, r.Cards, new(MobileDuanfaDiscard), f.Id);
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.DiscardPile, new(MobileDuanfaDiscard), (_, moves) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var paid = current.MobileDuanfa!with
                    {
                        After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                        ActualCount = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame)
                    };
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { MobileDuanfa = paid });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuanfaPaidEvent(f.Id, paid.Source, paid.Hash, paid.Phase, paid.Limit, paid.SpentBefore, paid.ActualCount, before, paid.After));
                });
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
                break;
            default:
                throw new InvalidOperationException("Unknown mobile Duanfa selection branch.");
        }

        ContinueMobileDuanfa(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 82300)]
    internal bool ResumeMobileDuanfa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileDuanfa: not null } f || f.Id != id)
            return false;
        ContinueMobileDuanfa(id);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileDuanfa)]
    internal void ReturnMobileDuanfaDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.MobileDuanfa ?? throw new InvalidOperationException("Duanfa draw lost its original payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuanfaDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        FinishMobileDuanfa(f);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileDuanfa)]
    internal PromptChoice SelectMobileDuanfaAi(PendingDecision d, ProgramSkillFrame f) => d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? d.Choices.First(c => c.Parameters["branch"] == "card");
}
