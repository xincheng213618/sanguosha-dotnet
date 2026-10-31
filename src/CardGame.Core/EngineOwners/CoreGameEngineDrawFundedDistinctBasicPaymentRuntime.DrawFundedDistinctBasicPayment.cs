#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentRuntime
{
    internal bool IsDrawFundedDistinctBasicAnswer(ChoiceId choice) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == choice && CoreGameEngineDrawFundedDistinctBasicOffersRuntime.IsDrawFundedDistinctBasicAnswer(c)) == true;
    internal CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, PromptId promptId, ChoiceId choiceId)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "游戏已经结束。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.PlayerSeat != actorSeat || prompt.PromptId != promptId)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "摸牌转换必须回答当前精确提示。");
        var choice = prompt.Choices.SingleOrDefault(c => c.Id == choiceId && CoreGameEngineDrawFundedDistinctBasicOffersRuntime.IsDrawFundedDistinctBasicAnswer(c));
        return choice is null ? _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "该摸牌转换选择未发布。") : SubmitDrawFundedDistinctBasicAnswer(actorSeat, prompt, choice);
    }

    internal CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, PendingDecision prompt, PromptChoice choice)
    {
        if (prompt.PlayerSeat != actorSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId != prompt.PromptId || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || choice.Cards.Count != 0 || !Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("output-kind"), out var kind) || !int.TryParse(choice.Parameters.GetValueOrDefault("native-target-seat"), out var target) || _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicContext(prompt)is not { } need || !_coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicResponseChoices(prompt, need).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The draw-funded answer changed its published original need, qualification, source, name or target.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        return _kernelCoreMainOperations.Accept(() =>
        {
            BeginDrawFundedDistinctBasic(_coreMatchParticipantsPlayersAccess._players[actorSeat], source, kind, (need.Intent is DrawFundedDistinctBasicIntent.OwnSlashDodge or DrawFundedDistinctBasicIntent.Dying) ? [target] : choice.Targets, prompt, need);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void BeginDrawFundedDistinctBasic(CharacterState actor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets, PendingDecision original, GameEngine.DrawFundedBasicNeed need)
    {
        var accepted = _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicRules(actor, kind, need.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge, need.Intent == DrawFundedDistinctBasicIntent.Dying).SingleOrDefault(r => r.Source == source);
        if (accepted.Rule is null || original.PlayerSeat != actor.Seat || _resolutionStackOperations._resolutionStack.OfType<DrawFundedDistinctBasicFrame>().Any(f => f.Payment.OriginalPromptId == original.PromptId))
            throw new InvalidOperationException("A draw-funded use cannot accept an unavailable source or pay the same original prompt twice.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var payment = new DrawFundedDistinctBasicPayment(id, source, accepted.Hash, accepted.Rule.DrawFundedDistinctBasic!.MethodLedgerId, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, need.Intent, kind, CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind), need.ParentFrameId, need.RequestFrameId, need.ParentActionId, targets.Count == 1 ? targets[0] : need.FixedTargetSeat, need.Cursor, original.PromptId, original.Revision, 0, 0, 0);
        if (need.Intent == DrawFundedDistinctBasicIntent.WenyiPeach)
            _coreWenyiPeachSuppliesOperations.ReserveWenyiDrawFundedPeach(original, need, source, kind);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(new DrawFundedDistinctBasicFrame(id, payment, original, targets));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawFundedDistinctBasicStartedEvent(id, actor.Seat, source, accepted.Hash, payment.MethodLedgerId, payment.ActualTurnNumber, payment.ActualTurnOwnerSeat, need.Intent, kind, need.ParentFrameId, need.RequestFrameId, need.ParentActionId, payment.TargetSeat, need.Cursor, original.PromptId, original.Revision));
        ContinueDrawFundedDistinctBasic(id);
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 90, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(DrawFundedDistinctBasicFrame), 380)]
    internal void ContinueDrawFundedDistinctBasic(long frameId)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is DrawFundedDistinctBasicFrame frame && frame.Id == frameId && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (frame.ActiveChildFrameId is not null)
                throw new InvalidOperationException("A draw-funded producer resumed before its exact child returned.");
            if (frame.Stage == DrawFundedDistinctBasicStage.Drawing)
            {
                var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
                // Persist the completed-attempt direction before native draw/rule facts can run children.
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = DrawFundedDistinctBasicStage.DrawChildren, Payment = frame.Payment with { SequenceBefore = before, SequenceAfter = before } });
                var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.Payment.Source.OwnerSeat], 1, true, new("skill-program.draw-funded-distinct-basic.draw"));
                var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
                var current = (DrawFundedDistinctBasicFrame)_resolutionStackOperations._resolutionStack.Last();
                var paid = current.Payment with
                {
                    SequenceAfter = after,
                    ActualDrawCount = drawn.Count
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { Payment = paid });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawFundedDistinctBasicPaidEvent(frameId, paid.Source.OwnerSeat, before, after, drawn.Count));
                continue;
            }

            if (frame.Stage != DrawFundedDistinctBasicStage.DrawChildren || !ValidDrawFundedDistinctBasicPayment(frame.Payment))
                throw new InvalidOperationException("A draw-funded producer lost its unique native draw attempt.");
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.DrawFundedDistinctBasic) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
                return;
            var legal = DrawFundedDistinctBasicOriginalNeedCurrent(frame);
            _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.DrawFundedDistinctBasic);
            if (!legal)
            {
                CancelDrawFundedDistinctBasicAttempt(frame);
                return;
            }

            // No new decision after payment: return only to the accepted use and its exact original native parent.
            _coreDrawFundedDistinctBasicUsesOperations.ExecuteDrawFundedDistinctBasicPaid(frame);
            return;
        }
    }

    internal void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DrawFundedDistinctBasicFrame frame || frame.Id != ownerFrameId || frame.Stage != DrawFundedDistinctBasicStage.DrawChildren || frame.ActiveChildFrameId is not null || !ValidDrawFundedDistinctBasicPayment(frame.Payment))
            throw new InvalidOperationException("A draw-funded cost cannot replace an active or unpaid child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActiveChildFrameId = childFrameId });
    }

    internal void ResumeDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DrawFundedDistinctBasicFrame frame || frame.Id != ownerFrameId || frame.ActiveChildFrameId != childFrameId)
            throw new InvalidOperationException("A draw-funded child returned to another payment frame.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActiveChildFrameId = null });
        ContinueDrawFundedDistinctBasic(frame.Id);
    }

    internal void ResumeDrawFundedDistinctBasicMovement(CardsMovedTriggerWindowFrame completed)
    {
        if (completed.ResumeDrawFundedDistinctBasicFrameId is not { } id || completed.Batch.ParentFrameId != id || completed.Batch.AwaitingProgramFrameId is not null || completed.ResumeProgramFrameId is not null || completed.Batch.Id != completed.Id)
            throw new InvalidOperationException("A draw-funded movement lost its exact native cost owner.");
        ResumeDrawFundedDistinctBasicChild(id, completed.Id);
    }

    internal bool IsDrawFundedDistinctBasicHpParent(ResolutionFrame? parent, HpChangedTriggerWindowFrame child) => parent is DrawFundedDistinctBasicFrame paid && child.Continuation == PostEventContinuation.DrawFundedDistinctBasic && child.ResumeFrameId == paid.Id && child.Change.ParentFrameId == paid.Id && paid.ActiveChildFrameId == child.Id && child.CardId is null && child.CardKind is null && ValidDrawFundedDistinctBasicPayment(paid.Payment);
    internal void ResumeDrawFundedDistinctBasicHpChange(HpChangedTriggerWindowFrame completed)
    {
        if (!IsDrawFundedDistinctBasicHpParent(_resolutionStackOperations._resolutionStack.LastOrDefault(), completed))
            throw new InvalidOperationException("A draw-funded HP-change child lost its owning payment.");
        ResumeDrawFundedDistinctBasicChild(completed.ResumeFrameId!.Value, completed.Id);
    }

    internal bool IsDrawFundedDistinctBasicRecoveryParent(ResolutionFrame? parent, RecoveryReplacementFrame child) => parent is DrawFundedDistinctBasicFrame paid && child.ParentFrameId == paid.Id && child.Return.ResumeFrameId == paid.Id && child.Return.Continuation == PostEventContinuation.DrawFundedDistinctBasic && paid.ActiveChildFrameId == child.Id && child.Return.CardId is null && child.Return.CardKind is null && ValidDrawFundedDistinctBasicPayment(paid.Payment);
    internal void ResumeDrawFundedDistinctBasicRecovery(RecoveryReplacementFrame completed)
    {
        if (!IsDrawFundedDistinctBasicRecoveryParent(_resolutionStackOperations._resolutionStack.LastOrDefault(), completed))
            throw new InvalidOperationException("A draw-funded recovery child lost its owning payment.");
        ResumeDrawFundedDistinctBasicChild(completed.ParentFrameId, completed.Id);
    }

    internal bool ValidDrawFundedDistinctBasicPayment(DrawFundedDistinctBasicPayment p)
    {
        if (p.PaymentFrameId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.Source.OwnerSeat) || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || p.NormalizedName != CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(p.EffectiveKind) || p.Cursor < 0 || p.ActualDrawCount is < 0 or > 1 || p.SequenceAfter < p.SequenceBefore || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } program || program.GameplayHash != p.GameplayHash || program.ViewAs.SingleOrDefault(r => r.Id == p.Source.BindingId)is not { DrawFundedDistinctBasic: { } policy } rule || policy.MethodLedgerId != p.MethodLedgerId || rule.OutputKind != p.EffectiveKind || rule.InputCount != 0)
            return false;
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicStartedEvent>().Where(e => e.PaymentFrameId == p.PaymentFrameId).ToArray();
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicPaidEvent>().Where(e => e.PaymentFrameId == p.PaymentFrameId).ToArray();
        return started is [var s] && s.ActorSeat == p.Source.OwnerSeat && s.Source == p.Source && s.GameplayHash == p.GameplayHash && s.MethodLedgerId == p.MethodLedgerId && s.ActualTurnNumber == p.ActualTurnNumber && s.ActualTurnOwnerSeat == p.ActualTurnOwnerSeat && s.Intent == p.Intent && s.EffectiveKind == p.EffectiveKind && s.ParentFrameId == p.ParentFrameId && s.RequestFrameId == p.RequestFrameId && s.ParentActionId == p.ParentActionId && s.TargetSeat == p.TargetSeat && s.Cursor == p.Cursor && s.OriginalPromptId == p.OriginalPromptId && s.OriginalRevision == p.OriginalRevision && paid is [var fact] && fact.ActorSeat == p.Source.OwnerSeat && fact.SequenceBefore == p.SequenceBefore && fact.SequenceAfter == p.SequenceAfter && fact.ActualDrawCount == p.ActualDrawCount && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.Source.OwnerSeat) && m.Reason.Value == "skill-program.draw-funded-distinct-basic.draw") == p.ActualDrawCount;
    }

    internal bool DrawFundedDistinctBasicOriginalNeedCurrent(DrawFundedDistinctBasicFrame frame)
    {
        var p = frame.Payment;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || p.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || p.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return false;
        if (p.Intent == DrawFundedDistinctBasicIntent.Play)
            return (_resolutionStackOperations._resolutionStack.Count == 1 && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat == actor.Seat || _characterKeBiNengOlOperations.IsKeBiNengPingduanPaymentFrame2(frame) || (_characterKongRongMouOlOperations.IsMouKrLiwenPaymentFrame2(frame) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTiePaymentFrame2(frame)) || _characterLuSuMouOperations.IsMouLuSuPaymentFrame2(frame)) && _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicNativePlayActions(actor, p.Source, p.EffectiveKind).Any(a => a.TargetSeats.SequenceEqual(frame.TargetSeats));
        var request = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.RequestFrameId);
        if (request is null || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[_resolutionStackOperations._resolutionStack.Count - 2].Id != request.Id || _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicContext(frame.OriginalDecision, request)is not { } current || current.Intent != p.Intent || current.ParentFrameId != p.ParentFrameId || current.RequestFrameId != p.RequestFrameId || current.ParentActionId != p.ParentActionId || current.Cursor != p.Cursor)
            return false;
        if (p.Intent == DrawFundedDistinctBasicIntent.WenyiPeach)
            return _coreWenyiPeachSuppliesOperations.WenyiDrawFundedNeedStillLegal(frame);
        if (request is ProgramSkillFrame program)
        {
            var effect = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
            if (!_coreDrawFundedDistinctBasicReturnsOperations.IsDrawFundedDistinctBasicProgramSelection(program, effect, frame))
                return false;
        }

        return _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicNativeResponseLegal(actor, current, p.EffectiveKind, p.TargetSeat);
    }

    internal void CancelDrawFundedDistinctBasicAttempt(DrawFundedDistinctBasicFrame frame)
    {
        var p = frame.Payment;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawFundedDistinctBasicCancelledEvent(p.PaymentFrameId, p.Source.OwnerSeat, p.Intent, p.EffectiveKind));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (p.Intent == DrawFundedDistinctBasicIntent.WenyiPeach && _coreWenyiPeachSuppliesOperations.TryCancelWenyiPeachSupply(p.ParentFrameId))
            return;
        if (p.ParentFrameId is { } pingduanParent && _characterKeBiNengOlOperations.TryCancelKeBiNengPingduanBasicPayment(pingduanParent))
            return;
        if (p.ParentFrameId is { } luSuParent && _characterLuSuMouOperations.TryCancelMouLuSuPayment(luSuParent))
            return;
        if (p.ParentFrameId is { } liwenParent && (_characterKongRongMouOlOperations.TryCancelMouKrLiwenPayment(liwenParent) || _characterMaTieAnniversaryOperations.TryCancelAnniversaryMaTiePayment(liwenParent)))
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        if (p.Intent == DrawFundedDistinctBasicIntent.BorrowedSword && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } b && b.ResolutionId == p.ParentFrameId && b.ActiveAttack is null)
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(b, transferWeapon: _coreMatchParticipantsPlayersAccess._players[b.WeaponOwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[b.SlashTargetSeat].IsAlive);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.Qinglong && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.ResolutionId == p.ParentFrameId)
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(frame.OriginalDecision);
            _coreNativeQinglongOperations.ResolveQinglongCrescentBladeChoice(frame.OriginalDecision.Choices.Single(c => c.Parameters.GetValueOrDefault("action") == "qinglong-skip"));
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.OwnSlashDodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.ResolutionId == p.ParentFrameId)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == a.ResolutionId)
                _coreNativeResponseWindowsOperations.PopResponseWindow(a.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(a.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreMatchParticipantsPlayersAccess._players[a.TargetSeat].IsAlive || !_coreNativeDamageApplicationOperations.ApplyAttackDamage(a))
                _coreNativeAttackCompletionOperations.CompleteAttack(a);
            return;
        }

        if (p.Intent == DrawFundedDistinctBasicIntent.Dying && _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.FrameId == p.ParentFrameId && d.ResponderIndex == p.Cursor)
        {
            _coreNativeDyingOperations.ApplyDyingResponse(_coreMatchParticipantsPlayersAccess._players[d.ResponderSeat], false, null, false, null);
            return;
        }

        if (p.Intent is DrawFundedDistinctBasicIntent.ProgramSlash or DrawFundedDistinctBasicIntent.ProgramNearestSlash or DrawFundedDistinctBasicIntent.AssistedSlash or DrawFundedDistinctBasicIntent.NearestLegalSlash && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame program && program.Id == p.ParentFrameId && program.InstructionIndex == p.Cursor)
        {
            if (_characterSunYuanOlOperations.FinishSayingSuppliedSlashFailure(program))
                return;
            if (_characterLiYiOlOperations.FinishChanshuangSuppliedSlashFailure(program))
                return;
            if (_characterWuJingOlOperations.FinishWuJingPaidAdapter(program))
                return;
            if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(program))
            {
                _characterLvKuangLvXiangOlOperations.FinishLvKuangLvXiang(program);
                return;
            }

            if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(program))
            {
                _characterCaoChunOlOperations.FinishCaoChun(program);
                return;
            }

            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(program, "原用牌对象或请求已失效，已支付摸牌尝试取消。");
        }
    }
}
