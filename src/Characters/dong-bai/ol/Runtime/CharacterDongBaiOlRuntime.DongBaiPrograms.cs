#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongBaiOlRuntime
{
    internal static CardConversionSource LianZhuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_dong_bai_ol_DongBaiPrograms.LianZhuSource(f);
    internal static string LianZhuReason(LianZhuStage stage) => RuntimeSkills.RuntimeRules_dong_bai_ol_DongBaiPrograms.LianZhuReason(stage);
    internal SkillProgramStepOutcome BeginLianZhu(ProgramSkillFrame supplied, int recipient)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LianZhu is not null || f.InstructionIndex != 1 || f.TriggerId is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedTargetSeats is not [var selected] || selected != recipient || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || f.SelectedCardIds is not [var id])
            throw new InvalidOperationException("LianZhu lost its exact unpaid hand gift.");
        var dynamic = _coreLianZhuCountsOperations.UsesRecipientHpLianZhu(f);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || (!dynamic ? from != CardLocation.Hand(f.OwnerSeat) : !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue || !_coreMatchParticipantsPlayersAccess._players[recipient].HasHpValue || _coreMatchParticipantsPlayersAccess._players[recipient].Hp <= 0 || !_coreLianZhuCountsOperations.CanDeliverLianZhuGift(f.OwnerSeat, recipient, id)))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "连诛赠牌前来源失效。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = new LianZhuReceipt(LianZhuSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, recipient, id, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), before, before, LianZhuStage.GiftChildren, before, before, 1)
        {
            GiftFrom = dynamic ? from : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LianZhu = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "lianzhu-gift-hand"))
            return SkillProgramStepOutcome.AwaitChild;
        ContinueLianzhuAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void IssueLianZhuSegment(long id, LianZhuReceipt r, int actor) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianZhuSegmentPaidEvent(id, r.Source, r.GameplayHash, r.Stage, actor, r.Before, r.After, r.Count));
    internal IReadOnlyList<PromptChoice> LianZhuDiscardOptions(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.LianZhu!.RecipientSeat, f.LianZhu.RecipientSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !f.LianZhu.Selected.Contains(c.Id));
    internal PromptChoice LianZhuChoice(ProgramSkillFrame f, string branch, string text, IReadOnlyList<int>? cards = null) => new(new($"lianzhu.{(f.Id)}.{(branch)}"), text, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "lianzhu", ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> LianZhuChoices(ProgramSkillFrame f)
    {
        if (f.LianZhu!.Stage == LianZhuStage.SelectingDiscard)
            return Array.AsReadOnly(LianZhuDiscardOptions(f).Select(c => LianZhuChoice(f, "card-" + c.Cards.Single(), c.Label, c.Cards)).ToArray());
        var amount = CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(f.LianZhu!);
        var choices = new List<PromptChoice>
        {
            LianZhuChoice(f, "draw", f.LianZhu!.CountPolicy is null ? "令连诛拥有者摸两张牌" : $"令连诛拥有者摸{(amount)}张牌")
        };
        if (LianZhuDiscardOptions(f).Count >= amount)
            choices.Add(LianZhuChoice(f, "discard", f.LianZhu.CountPolicy is null ? "弃置自己的两张牌" : $"弃置自己的{(amount)}张牌"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishLianZhuChoice(ProgramSkillFrame f)
    {
        var seat = f.LianZhu!.RecipientSeat;
        var choices = LianZhuChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse, PendingMovementContinuation = null });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "请选择连诛结算。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)]
    internal PromptChoice LianZhuAiChoice(ProgramSkillFrame f) => _coreLianZhuCountsOperations.ChooseLianZhuAiChoice(f);
    [ProgramAnswer("lianzhu")]
    internal void ResolveLianZhuChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireLianZhu(f);
        var r = f.LianZhu!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.RecipientSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LianZhuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("LianZhu lost recipient authority.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
        {
            FinishLianZhu(f);
            return;
        }

        if (choice.Parameters["branch"] == "draw")
        {
            PayLianZhuDraw(f);
            return;
        }

        if (choice.Parameters["branch"] == "discard")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LianZhu = r with { Stage = LianZhuStage.SelectingDiscard } });
            PublishLianZhuChoice(f);
            return;
        }

        var selected = r.Selected.Append(choice.Cards.Single()).ToArray();
        r = r with
        {
            Selected = selected
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LianZhu = r });
        if (selected.Length < CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r))
        {
            PublishLianZhuChoice(f);
            return;
        }

        r = r with
        {
            Stage = LianZhuStage.DiscardChildren,
            Locations = selected.Select(_cardZonesOperations._cardZones.GetLocation).ToArray(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Count = CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LianZhu = r, PendingMovementContinuation = new(r.RecipientSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.RecipientSeat, selected, new(LianZhuReason(r.Stage)), (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = r with
            {
                After = records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { LianZhu = paid });
            IssueLianZhuSegment(f.Id, paid, r.RecipientSeat);
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void PayLianZhuDraw(ProgramSkillFrame f)
    {
        var r = f.LianZhu!with
        {
            Stage = LianZhuStage.DrawChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Count = 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LianZhu = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var count = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r), true, new(LianZhuReason(r.Stage))).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = r with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Count = count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LianZhu = r });
        IssueLianZhuSegment(f.Id, r, f.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void FinishLianZhu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LianZhu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool DrainLianZhu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 22400)]
    internal bool ResumeLianZhu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LianZhu: { } r } f || f.Id != id || r.Stage is LianZhuStage.RecipientChoice or LianZhuStage.SelectingDiscard)
            return false;
        RequireLianZhu(f);
        if (DrainLianZhu(f))
            return true;
        if (r.Stage == LianZhuStage.GiftChildren && r.GiftSuit is Suit.Spade or Suit.Club && _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive && (!_coreLianZhuCountsOperations.UsesRecipientHpLianZhu(f) || _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].Hp > 0))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LianZhu = r with { Stage = LianZhuStage.RecipientChoice } });
            f = _coreLianZhuCountsOperations.FreezeLianZhuChoiceAmount(f);
            PublishLianZhuChoice(f);
        }
        else
            FinishLianZhu(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 22400)]
    internal bool ReturnLianZhuMovement(ProgramSkillFrame f) => f.LianZhu is not null && ResumeLianZhu(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 80)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 80)]
    internal bool CanContinueLianZhu(ProgramSkillFrame f)
    {
        if (f.LianZhu is null)
            return false;
        RequireLianZhu(f);
        return true;
    }
    internal void RequireLianZhu(ProgramSkillFrame f)
    {
        if (f.LianZhu is { Stage: LianZhuStage.GiftChildren } revealing && _characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, "lianzhu-gift-hand") && revealing.Source == LianZhuSource(f) && revealing.GameplayHash == f.GameplayHash && revealing.InstructionIndex == f.InstructionIndex && f.SelectedCardIds.SequenceEqual([revealing.GiftCardId]) && f.SelectedTargetSeats.SequenceEqual([revealing.RecipientSeat]) && revealing.GiftBefore == revealing.GiftAfter && revealing.Before == revealing.After && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardsRevealedEvent>().Any(e => e.ResolutionId == f.Id && e.Cards.Select(c => c.Id).SequenceEqual([revealing.GiftCardId])) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<LianZhuSegmentPaidEvent>().Any(e => e.FrameId == f.Id))
            return;
        if (f.LianZhu is not { } r || r.Source != LianZhuSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwnerSeat != f.OwnerSeat || f.SelectedCardIds is not [var id] || id != r.GiftCardId || f.SelectedTargetSeats is not [var seat] || seat != r.RecipientSeat || r.GiftAfter <= r.GiftBefore || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Count > CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r))
            throw new InvalidOperationException("LianZhu lost its frozen source, recipient or exact gift invoice.");
        _coreLianZhuCountsOperations.RequireLianZhuChoiceAmount(f, r);
        if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.GiftBefore && m.Sequence <= r.GiftAfter && m.CardId == r.GiftCardId && m.From == (r.GiftFrom ?? CardLocation.Hand(f.OwnerSeat)) && m.To == CardLocation.Hand(r.RecipientSeat) && m.Reason.Value == LianZhuReason(LianZhuStage.GiftChildren)) != 1)
            throw new InvalidOperationException("LianZhu gift was not paid exactly once.");
        if (!Enum.IsDefined(r.Stage) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LianZhuSegmentPaidEvent>().Count(e => e == new LianZhuSegmentPaidEvent(f.Id, r.Source, r.GameplayHash, LianZhuStage.GiftChildren, f.OwnerSeat, r.GiftBefore, r.GiftAfter, 1)) != 1)
            throw new InvalidOperationException("LianZhu requires its once-issued original gift fact in every continuation stage.");
        if (r.Stage is LianZhuStage.RecipientChoice or LianZhuStage.SelectingDiscard)
        {
            if (f.PendingMovementContinuation is not null || r.Stage == LianZhuStage.RecipientChoice && r.Selected.Count != 0 || r.Stage == LianZhuStage.SelectingDiscard && r.Selected.Count >= CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r))
                throw new InvalidOperationException("LianZhu choice cannot masquerade as a paid movement.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } decision && (decision.PlayerSeat != r.RecipientSeat || !decision.IsPrivate || decision.SkillPrompt?.SkillId != f.SkillId || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices, LianZhuChoices(f))))
                throw new InvalidOperationException("LianZhu paid continuation lost its exact recipient prompt.");
            return;
        }

        var actor = r.Stage == LianZhuStage.DiscardChildren ? r.RecipientSeat : f.OwnerSeat;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LianZhuSegmentPaidEvent>().Count(e => e == new LianZhuSegmentPaidEvent(f.Id, r.Source, r.GameplayHash, r.Stage, actor, r.Before, r.After, r.Count)) != 1 || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != actor)
            throw new InvalidOperationException("LianZhu cannot resume an unissued payment.");
        if (r.Stage == LianZhuStage.DrawChildren && (r.Count < 0 || r.Count > CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == LianZhuReason(r.Stage)) != r.Count))
            throw new InvalidOperationException("LianZhu draw invoice changed.");
        if (r.Stage == LianZhuStage.DiscardChildren && (r.Selected.Count != CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r) || r.Locations.Count != CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r) || r.Count != CoreGameEngineLianZhuCountsRuntime.LianZhuChoiceAmount(r) || r.Selected.Select((card, n) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == card && m.From == r.Locations[n] && m.Reason.Value == LianZhuReason(r.Stage) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(card).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m))))).Any(count => count != 1)))
            throw new InvalidOperationException("LianZhu discard invoice changed.");
    }

    SkillProgramStepOutcome IDongBaiProgramHost.BeginLianZhu(ProgramSkillFrame f, int recipient) => BeginLianZhu(f, recipient);
    [PublicHandRevealReturn("lianzhu-gift-hand")]
    internal void ContinueLianzhuAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.LianZhu ?? throw new InvalidOperationException("LianZhu lost its original selected real gift.");
        var id = f.SelectedCardIds.Single();
        var recipient = r.RecipientSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != (r.GiftFrom ?? CardLocation.Hand(f.OwnerSeat)))
        {
            FinishLianZhu(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        r = r with
        {
            GiftBefore = before,
            GiftAfter = before,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LianZhu = r });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), new(LianZhuReason(r.Stage)), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = r with
            {
                GiftAfter = records.Single().Sequence,
                After = records.Single().Sequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { LianZhu = paid });
            IssueLianZhuSegment(active.Id, paid, f.OwnerSeat);
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return;
    }
}
