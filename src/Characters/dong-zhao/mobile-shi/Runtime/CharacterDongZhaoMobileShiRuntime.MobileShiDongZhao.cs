#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMobileShiRuntime
{
    internal const string ShiDzMiaolue = "mobile:shi-miaolue", ShiDzYingjia = "mobile:shi-yingjia";
    internal const string ShiDzGain = "skill-program.mobile-shi-dong-zhao.gain", ShiDzDraw = "skill-program.mobile-shi-dong-zhao.draw", ShiDzDiscard = "skill.mobile-shi-dong-zhao.discard";
    internal static CardConversionSource ShiDzSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IMobileShiDongZhaoHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginShiDz(frame, op);
    internal int ShiDzSameNameMaximum(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnTrickUseRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == seat).GroupBy(e => e.EffectiveKind).Select(g => g.Count()).DefaultIfEmpty(0).Max();
    internal IReadOnlyList<PromptChoice> ShiDzPayments(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, new(zone, f.OwnerSeat), OwnedCardMoveIntent.Discard));
    internal MobileShiDongZhaoExtraGrantedEvent? ShiDzPendingExtra(int beneficiary)
    {
        if (_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat != beneficiary || !_coreMatchParticipantsPlayersAccess._players[beneficiary].IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramExtraTurnPendedEvent>().LastOrDefault()is not { SkillId: ShiDzYingjia } last || last.Seat != beneficiary)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiDongZhaoExtraGrantedEvent>().SingleOrDefault(e => e.FrameId == last.FrameId && e.TargetSeat == beneficiary && e.IssuedTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Source.SkillId == ShiDzYingjia && e.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShiDzYingjia).Program!.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiDongZhaoExtraEnteredEvent>().Any(x => x.GrantFrameId == e.FrameId));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 47700)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiDzExtra(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (window != SkillProgramTriggerWindow.TurnBeforeStart || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(ShiDzYingjia))
            return ordinary;
        var rest = ordinary.Where(c => c.SkillId != ShiDzYingjia || c.BindingId != "issued-extra-start").ToArray();
        var grant = ShiDzPendingExtra(owner.Seat);
        return grant is null ? Array.AsReadOnly(rest) : Array.AsReadOnly(rest.Append(new ProgramTriggerCandidate(owner.Seat, ShiDzYingjia, "issued-extra-start", grant.Source.SkillInstanceId, grant.GameplayHash, 100)).ToArray());
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 47700)]
    internal bool IsIssuedShiDzExtra(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == ShiDzYingjia && c.BindingId == "issued-extra-start" && w.Window == SkillProgramTriggerWindow.TurnBeforeStart && w.BeforeTurnPreludeProof is { } proof && proof.UpcomingOwnerSeat == c.OwnerSeat && _characterBeiMiHuOlOperations.ValidBeiMiHuBeforeTurnPrelude(proof) && ShiDzPendingExtra(c.OwnerSeat)is { } grant && grant.Source.SkillInstanceId == c.SkillInstanceId && grant.GameplayHash == c.GameplayHash && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is ProgramLifecycleTriggerWindowFrame parent && parent.Window == w.Window && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 47700)]
    internal bool CanOfferShiDz(ProgramTriggerLegalityContext q)
    {
        if (q.SkillId != ShiDzYingjia)
            return true;
        if (q.Trigger.Id == "issued-extra-start")
            return ShiDzPendingExtra(q.OwnerSeat)is not null;
        return ShiDzSameNameMaximum(q.OwnerSeat) >= 2 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[q.OwnerSeat]).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(q.OwnerSeat, card, CardLocation.Hand(q.OwnerSeat), OwnedCardMoveIntent.Discard));
    }

    internal SkillProgramStepOutcome BeginShiDz(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.MobileShiDongZhao is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Shi Dong Zhao requires the original unstarted native trigger.");
        var grant = f.TriggerId == "issued-extra-start" ? ShiDzPendingExtra(f.OwnerSeat) ?? throw new InvalidOperationException("Yingjia lost the exact pending extra-turn grant.") : null;
        var stage = grant is not null ? MobileShiDongZhaoStage.ExtraDrawChildren : f.TriggerId == "opening-two" ? MobileShiDongZhaoStage.GainChildren : op == SkillProgramEffectOp.ResolveMobileShiDongZhaoMiaolue ? MobileShiDongZhaoStage.Benefit : MobileShiDongZhaoStage.Payment;
        var r = new MobileShiDongZhaoReceipt(grant?.Source ?? ShiDzSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, stage)
        {
            Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            GrantFrameId = grant?.FrameId ?? 0,
            TargetSeat = grant?.TargetSeat ?? -1,
            Branch = f.TriggerId == "opening-two" ? "opening" : grant is not null ? "extra" : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDongZhaoStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, f.TriggerId!));
        if (stage is MobileShiDongZhaoStage.Benefit or MobileShiDongZhaoStage.Payment)
        {
            PublishShiDz(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (grant is not null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDongZhaoExtraEnteredEvent(grant.FrameId, f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber + 1));
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new(ShiDzDraw));
        }
        else
            _coreMantianGuohaiOperations.CreateMobileShiMantianGuohaiCards(f.OwnerSeat);
        SealShiDz(f.Id);
        ContinueShiDz(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiDzChoices(ProgramSkillFrame f)
    {
        var r = f.MobileShiDongZhao!;
        PromptChoice C(string branch, string label, int? target = null) => new(new($"shi-dong-zhao.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target ?? -1)}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-dong-zhao", ["branch"] = branch });
        if (r.Stage == MobileShiDongZhaoStage.Benefit)
            return[C("draw", "摸两张牌"), C("DrawTwo", "获得一张【无中生有】"), C("Dismantlement", "获得一张【过河拆桥】"), C("Nullification", "获得一张【无懈可击】")];
        if (r.Stage == MobileShiDongZhaoStage.Target)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => C("target", "令 " + p.Name + " 执行额外回合（开始时摸两张牌）", p.Seat)).ToArray());
        if (r.Stage != MobileShiDongZhaoStage.Payment)
            return[];
        return Array.AsReadOnly(ShiDzPayments(f).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-shi-dong-zhao", ["branch"] = "payment" } }).ToArray());
    }

    internal void PublishShiDz(ProgramSkillFrame f)
    {
        var choices = ShiDzChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishShiDz(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, rText(f.MobileShiDongZhao!.Stage), choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        static string rText(MobileShiDongZhaoStage stage) => stage == MobileShiDongZhaoStage.Benefit ? "妙略：摸两张牌，或指定一种智囊牌名。" : stage == MobileShiDongZhaoStage.Payment ? "迎驾：弃置一张手牌。" : "迎驾：选择额外回合的执行者。";
    }

    [ProgramAnswer("mobile-shi-dong-zhao")]
    internal void AnswerShiDz(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiDongZhao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiDzChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shi Dong Zhao lost the exact current benefit, private payment or target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == MobileShiDongZhaoStage.Target)
        {
            var target = choice.Targets.Single();
            _coreSkillProgramsOperations.PendProgramExtraTurn(f, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDongZhaoExtraGrantedEvent(f.Id, r.Source, r.GameplayHash, target, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber));
            FinishShiDz(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        r = r with
        {
            Branch = branch,
            Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            Stage = r.Stage == MobileShiDongZhaoStage.Payment ? MobileShiDongZhaoStage.CostChildren : MobileShiDongZhaoStage.GainChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (branch == "payment")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = r with { CardId = id } });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new(ShiDzDiscard));
            SealShiDz(f.Id);
            var after = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).MobileShiDongZhao!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDongZhaoCostPaidEvent(f.Id, id, after.Before, after.After));
        }
        else if (branch == "draw")
        {
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new(ShiDzDraw));
            SealShiDz(f.Id);
        }
        else
        {
            var kind = Enum.Parse<CardKind>(branch, false);
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(c => c.Kind == kind) ?? _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).FirstOrDefault(c => c.Kind == kind);
            if (card is not null)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = r with { CardId = card.Id } });
                _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(f.OwnerSeat), new(ShiDzGain));
            }

            SealShiDz(f.Id);
        }

        if (branch != "payment")
        {
            var after = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).MobileShiDongZhao!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDongZhaoBenefitEvent(f.Id, branch, after.CardId, after.Before, after.After));
        }

        ContinueShiDz(f.Id);
    }

    internal void SealShiDz(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiDongZhao = f.MobileShiDongZhao!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
    }

    internal void ContinueShiDz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiDongZhao: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage is MobileShiDongZhaoStage.Benefit or MobileShiDongZhaoStage.Payment or MobileShiDongZhaoStage.Target)
        {
            PublishShiDz(f);
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        if (r.Stage == MobileShiDongZhaoStage.CostChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = f.MobileShiDongZhao!with { Stage = MobileShiDongZhaoStage.Target } });
            PublishShiDz(f);
            return;
        }

        FinishShiDz(f);
    }

    internal void FinishShiDz(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiDongZhao = f.MobileShiDongZhao!with { Stage = MobileShiDongZhaoStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 47700)]
    internal bool ResumeShiDz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiDongZhao: not null } f || f.Id != id)
            return false;
        ContinueShiDz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 47700)]
    internal bool ReturnShiDz(ProgramSkillFrame f)
    {
        if (f.MobileShiDongZhao is null)
            return false;
        ContinueShiDz(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 47700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 47700)]
    internal bool PaidShiDzCanReturn(ProgramSkillFrame f) => f.MobileShiDongZhao?.Stage is MobileShiDongZhaoStage.GainChildren or MobileShiDongZhaoStage.CostChildren or MobileShiDongZhaoStage.Target or MobileShiDongZhaoStage.ExtraDrawChildren || f.MobileShiDongZhao is null && f.InstructionIndex is 0 or 1 && f.TriggerId == "issued-extra-start" && f.WindowContext is { } w && IsIssuedShiDzExtra(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 100), w);

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiDongZhaoMiaolue, SkillProgramEffectOp.ResolveMobileShiDongZhaoYingjia)]
    internal PromptChoice ChooseShiDzAi(PendingDecision p, ProgramSkillFrame f) => f.MobileShiDongZhao!.Stage switch
    {
        MobileShiDongZhaoStage.Benefit => p.Choices.First(c => c.Parameters["branch"] == "draw"),
        MobileShiDongZhaoStage.Payment => p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First(),
        _ => p.Choices.OrderByDescending(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).ThenByDescending(c => _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()].Hp).First()};
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiDongZhaoPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != ShiDzYingjia)
            return[];
        return Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "same-tricks-ended", ProgramPublicRuleStateKind.ActivationLimit, 1, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, "same-tricks-ended@" + i.SkillInstanceId, SkillUsageScope.Game), SkillUsageScope.Game), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "same-name-tricks", ProgramPublicRuleStateKind.ActivationLimit, 2, ShiDzSameNameMaximum(owner.Seat), SkillUsageScope.Turn) }).ToArray());
    }
}
