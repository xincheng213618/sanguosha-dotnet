#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinXianYingMobileShiRuntime
{
    internal void CompleteShiXxyJiejie(ProgramSkillFrame f)
    {
        var r = f.ShiXinXianYing!;
        var count = ShiXxySuitCount(ShiXxyRoundMask(f.OwnerSeat, r.Actor));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive && ShiXxyRoundUsed(f.OwnerSeat) < 2 && _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != r.Actor).All(p => ShiXxySuitCount(ShiXxyRoundMask(f.OwnerSeat, p.Seat)) < count))
        {
            IssueShiXxyQingshi(f, r.Actor);
            return;
        }

        FinishShiXxy(f);
    }

    internal void IssueShiXxyQingshi(ProgramSkillFrame f, int target)
    {
        var r = f.ShiXinXianYing!;
        if (r.QingIssued || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            throw new InvalidOperationException("Qingshi cannot replace or repeat its original target.");
        var same = ShiXxySameCamp(f.OwnerSeat, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r with { Target = target, SameCamp = same, QingIssued = true, Stage = r.Jiejie ? ShiXinXianYingStage.QingshiInvocation : same ? ShiXinXianYingStage.DrawChildren : ShiXinXianYingStage.OwnerDiscard, DrawCursor = 0, QingshiInstance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).ProgramInstances.FirstOrDefault(i => i.SkillId == ShiXxyQingshi)?.SkillInstanceId ?? "" }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingQingshiIssuedEvent(f.Id, r.Source, target, _coreActualRoundProgressRoundNumberAccess._roundNumber, r.Jiejie, same));
        ContinueShiXxy(f.Id);
    }

    internal void SealShiXxy(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f with { ShiXinXianYing = f.ShiXinXianYing!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiXxy(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiXxy(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShiXinXianYing: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (r.Stage is ShiXinXianYingStage.Suit or ShiXinXianYingStage.Target)
            {
                PublishShiXxy(f);
                return;
            }

            if (r.Stage == ShiXinXianYingStage.Drawing)
                return;
            if (r.Stage == ShiXinXianYingStage.QingshiInvocation)
            {
                if (_characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(f.Id, f.OwnerSeat, ShiXxyQingshi, "shi-jiejie-qingshi:" + f.Id, r.QingshiInstance))
                    return;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { ShiXinXianYing = r with { Stage = r.SameCamp ? ShiXinXianYingStage.DrawChildren : ShiXinXianYingStage.OwnerDiscard } });
                continue;
            }

            if (r.Stage == ShiXinXianYingStage.OwnerDiscard)
            {
                if (ShiXxyChoices(f).Count > 0)
                {
                    PublishShiXxy(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiXinXianYing = r with { Stage = ShiXinXianYingStage.OtherDiscard } });
                continue;
            }

            if (r.Stage == ShiXinXianYingStage.OtherDiscard)
            {
                if (ShiXxyChoices(f).Count > 0)
                {
                    PublishShiXxy(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiXinXianYing = r with { Stage = ShiXinXianYingStage.QingDiscardChildren } });
                continue;
            }

            if (r.Stage == ShiXinXianYingStage.QingDiscardChildren && !r.DiscardPaid)
            {
                var ids = r.Selected.Where(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) == m.From && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, OwnedCardMoveIntent.Discard)).Select(m => m.CardId).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r = r with { DiscardPaid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                if (ids.Length > 0)
                    _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new("skill-program.mobile-shi-qingshi.discard"));
                SealShiXxy(id);
                continue;
            }

            if (DrainShiXxy(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.ShiXinXianYing!;
            if (r.Stage == ShiXinXianYingStage.JiejieChildren)
            {
                if (_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingSuitChosenEvent>().Single(e => e.FrameId == id).Included)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingSuitAllowanceEvent(id, r.Source, r.GameplayHash, r.Actor, r.ActualTurn, r.ChosenSuit));
                CompleteShiXxyJiejie(f);
                return;
            }

            if (r.Stage == ShiXinXianYingStage.DrawChildren)
            {
                var maximum = r.Target == f.OwnerSeat ? 1 : 2;
                if (r.DrawCursor >= maximum || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishShiXxy(f);
                    return;
                }

                var target = r.DrawCursor == 0 ? f.OwnerSeat : r.Target;
                var requested = r.Target == f.OwnerSeat ? 2 : 1;
                if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiXinXianYing = r with { DrawCursor = r.DrawCursor + 1 } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r with { Stage = ShiXinXianYingStage.Drawing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingDrawIssuedEvent(id, r.DrawCursor, target, requested));
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, requested, new("skill-program.mobile-shi-qingshi.draw"));
                return;
            }

            FinishShiXxy(f);
            return;
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiXinXianYingJiejie, SkillProgramEffectOp.ResolveMobileShiXinXianYingQingshi)]
    internal void ReturnShiXxyDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.ShiXinXianYing ?? throw new InvalidOperationException("Qingshi draw lost its owning frame.");
        var request = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingDrawIssuedEvent>().SingleOrDefault(e => e.FrameId == f.Id && e.Cursor == r.DrawCursor);
        if (r.Stage != ShiXinXianYingStage.Drawing || request is null || request.Recipient != outcome.RecipientSeat || request.Requested != outcome.RequestedCount || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.DrawCursor))
            throw new InvalidOperationException("Qingshi rejects a repeated or foreign draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r with { Stage = ShiXinXianYingStage.DrawChildren, DrawCursor = r.DrawCursor + 1, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingDrawReturnedEvent(f.Id, r.DrawCursor, outcome.RecipientSeat, outcome.ActualDrawIds.Count));
        ContinueShiXxy(f.Id);
    }

    internal void FinishShiXxy(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = f.ShiXinXianYing!with { Stage = ShiXinXianYingStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 48200)]
    internal bool ResumeShiXxy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXinXianYing: not null } f || f.Id != id)
            return false;
        ContinueShiXxy(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48200)]
    internal bool ReturnShiXxyMovement(ProgramSkillFrame f)
    {
        if (f.ShiXinXianYing is null)
            return false;
        ContinueShiXxy(f.Id);
        return true;
    }
    internal bool ShiXxyMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.ShiXinXianYing is { Stage: ShiXinXianYingStage.JiejieChildren or ShiXinXianYingStage.QingDiscardChildren or ShiXinXianYingStage.DrawChildren } && p.BeforeCount == 0 && p.CoverageResultBind is null && effect?.Op is SkillProgramEffectOp.ResolveMobileShiXinXianYingJiejie or SkillProgramEffectOp.ResolveMobileShiXinXianYingQingshi;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48200)]
    internal bool PaidShiXxy(ProgramSkillFrame f) => f.ShiXinXianYing is { Stage: ShiXinXianYingStage.JiejieChildren or ShiXinXianYingStage.QingshiInvocation or ShiXinXianYingStage.QingDiscardChildren or ShiXinXianYingStage.Drawing or ShiXinXianYingStage.DrawChildren };

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiXinXianYingJiejie, SkillProgramEffectOp.ResolveMobileShiXinXianYingQingshi)]
    internal PromptChoice ShiXxyAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.ShiXinXianYing!;
        if (r.Stage == ShiXinXianYingStage.Suit)
        {
            var same = ShiXxySameCamp(f.OwnerSeat, r.Actor);
            var missing = CharacterXinXianYingMobileShiDefinitions.ShiXxySuits.FirstOrDefault(s => r.Viewed.All(c => c.Suit != s));
            var suit = same && missing != Suit.None ? missing : r.Viewed.GroupBy(c => c.Suit).OrderBy(g => g.Count()).FirstOrDefault()?.Key ?? Suit.Spade;
            return p.Choices.First(c => c.Parameters["branch"] == suit.ToString());
        }

        if (r.Stage == ShiXinXianYingStage.Target)
            return p.Choices.First(c => c.Targets.SequenceEqual([f.OwnerSeat]));
        return p.Choices.First();
    }

    internal bool HasShiXxySuitAllowance(int actor, Suit? suit) => suit is { } s && s != Suit.None && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingSuitAllowanceEvent>().Any(e => e.Actor == actor && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Suit == s);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 48200)]
    internal bool ShiXxySlashQuota(ProgramSlashQuotaQuery q)
    {
        var cards = q.PhysicalCardIds is { Count: > 0 } ids ? ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray() : q.PhysicalCard is { } card ? [card] : [];
        return cards.Length > 0 && HasShiXxySuitAllowance(q.Actor.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(q.Actor, cards));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 48200)]
    internal bool ShiXxyAlcoholDebit(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Alcohol } action } && action.ActorSeat == actor && HasShiXxySuitAllowance(actor, action.EffectiveSuit);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> ShiXinXianYingPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != ShiXxyJiejie)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "qingshi-round", ProgramPublicRuleStateKind.GameActivationProgress, ShiXxyRoundUsed(owner.Seat)), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "jiejie-active-suits", ProgramPublicRuleStateKind.GameActivationProgress, ShiXxyMask(_coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingSuitAllowanceEvent>().Where(e => e.Actor == owner.Seat && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber).Select(e => e.Suit))) }).ToArray();
    }
}
