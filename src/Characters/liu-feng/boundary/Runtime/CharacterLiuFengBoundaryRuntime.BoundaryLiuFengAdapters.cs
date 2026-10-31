#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuFengBoundaryRuntime
{
    internal void PrepareBoundaryLiuFengRequest(ProgramSkillFrame f)
    {
        if (f.BoundaryLiuFeng is not { Need: { } n } r)
            return;
        if (r.Stage != BoundaryLiuFengStage.Issuing || r.CostSequence <= 0 || r.Targets is not [var target] || !BoundaryLiuFengNeedCurrent(r) || !BoundaryLiuFengRequestTarget(n, target) || !ValidBoundaryLiuFeng(f))
            throw new InvalidOperationException("Qinling cannot consume another or no longer legal native request.");
        // This synchronous reparenting only consumes the old native response
        // cursor. The same paid skill frame is restored before a new Use exists.
        _coreRuntimeOperations.PopResolutionFrame(f.Id, ResolutionFrameKind.ProgramSkill);
        if (n.Intent == BoundaryLiuFengIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            if (b.ResolutionId != n.OwnerFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != n.RequestFrameId)
                throw new InvalidOperationException("Qinling lost its exact Borrowed Sword response.");
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            b.AwaitingSlashChoice = false;
            b.SlashCardId = 0;
            b.EffectiveSlashKind = CardKind.Slash;
            _coreNativeCardUseFramesOperations.SetCardUseStep(b.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        }
        else if (n.Intent == BoundaryLiuFengIntent.Qinglong)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, n.ActorSeat, target, true, [], CardKind.Slash));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
        }
        else
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
            var e = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)!;
            if (parent.Id != n.RequestFrameId || parent.InstructionIndex != n.InstructionIndex || e.Op != n.RequestOp)
                throw new InvalidOperationException("Qinling changed the original native request instruction.");
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { Step = ResolutionFrameStep.ResolvingEffect, BoundaryLiuFengCaller = new(f.Id, r.Source, r.Hash, n, r.Cards.Single(), r.From.Single(), r.CostSequence, target) });
            if (_characterMaTieAnniversaryOperations.PrepareAnniversaryMaTieSuppliedSlash(parent, n.ActorSeat, n.ActorSeat, target, CardKind.Slash, [], r.Source))
            {
            }
            else if (_characterSunYuanOlOperations.IsSayingSlashRequest(parent))
                _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(parent, n.ActorSeat, n.ActorSeat, target, CardKind.Slash, [], r.Source);
            else if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parent))
                _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(parent, n.ActorSeat, n.ActorSeat, target, CardKind.Slash, [], r.Source);
            else if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent))
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { LvKuangLvXiang = parent.LvKuangLvXiang!with { Stage = "issuing", IssuingIds = [], IssuingKind = CardKind.Slash, IssuingProvider = n.ActorSeat } });
            else if (_characterWuJingOlOperations.IsWuJingHejiRequest(parent))
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WuJing = parent.WuJing!with { Stage = WuJingStage.IssuingUse, Selection = new(CardKind.Slash, false, false) { Conversions = [r.Source] }, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            else if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(parent))
            {
                var cc = parent.CaoChun!;
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CaoChun = cc with { Stage = CaoChunStage.NativeSlash, Slash = new(parent.Id, cc.Source, cc.GameplayHash, cc.InstructionIndex, cc.ActualTurn, cc.PhaseInstance, false, target, CardKind.Slash) { ProviderSeat = n.ActorSeat, Materials = [], PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } } });
            }
            else if (n.Intent == BoundaryLiuFengIntent.GenericBasic)
            {
                var action = new LegalAction(LegalActionKind.Slash, 0, target, "侵凌：弃置后视为使用杀", CardKind.Slash, TargetSeats: [target])
                {
                    ConversionSource = r.Source
                };
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengKeActionEvent(parent.Id, parent.InstructionIndex, f.Id, target));
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { KeBiNengPingduan = parent.KeBiNengPingduan!with { BasicActions = parent.KeBiNengPingduan.BasicActions.Append(action).ToArray(), SelectedBasicAction = action, Stage = KeBiNengPingduanStage.BasicIssuing } });
            }
            else if (n.Intent == BoundaryLiuFengIntent.ProgramSlash)
                _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, n.ActorSeat, "侵凌：真实弃置后使用杀。");
            else if (n.Intent == BoundaryLiuFengIntent.AssistedSlash)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { AssistedSlashRequest = null });
                _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, e.ResultBind!, "used-slash", n.ActorSeat, "侵凌：真实弃置后使用杀。");
            }
            else if (n.Intent == BoundaryLiuFengIntent.FixedSlash)
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { FixedTargetSlash = parent.FixedTargetSlash!with { Issued = true } });
            else if (n.Intent == BoundaryLiuFengIntent.NearestLegalSlash)
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NearestLegalSlashRequest = null });
        }

        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengRequestReleasedEvent(f.Id, n.OwnerFrameId, n.RequestFrameId, true));
    }

    internal ProgramSkillFrame? BoundaryLiuFengNativeParent(CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not { } o || !IsBoundaryLiuFengUse(use))
            return null;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 2 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { BoundaryLiuFeng: { Need: { } n } r } cost || cost.Id != o.FrameId || !ValidBoundaryLiuFeng(cost) || n.RequestFrameId != n.OwnerFrameId || r.UseId != use.Id || r.ActionId != use.Action!.ActionId || r.Source != o.Source || r.Hash != o.Hash || r.CostSequence <= 0 || _resolutionStackOperations._resolutionStack[index - 2] is not ProgramSkillFrame parent || parent.Id != n.OwnerFrameId || parent.InstructionIndex != n.InstructionIndex || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op != n.RequestOp || n.ActorSeat != o.Actor || !r.Targets.SequenceEqual(use.TargetSeats) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengRequestReleasedEvent>().Count(e => e == new BoundaryLiuFengRequestReleasedEvent(cost.Id, n.OwnerFrameId, n.RequestFrameId, true)) != 1)
            return null;
        return parent;
    }

    internal ProgramSkillFrame? BoundaryLiuFengReturnedProgramParent(long parentId, CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not { } o || !IsBoundaryLiuFengUse(use) || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiuFeng: { Need: { } n, Returned: true } r } cost || cost.Id != o.FrameId || n.OwnerFrameId != parentId || n.RequestFrameId != parentId || r.UseId != use.Id || r.ActionId != use.Action!.ActionId || !ValidBoundaryLiuFeng(cost))
            return null;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == cost.Id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1] is ProgramSkillFrame parent && parent.Id == parentId && ValidBoundaryLiuFengCaller(parent) && parent.BoundaryLiuFengCaller!.Returned ? parent : null;
    }

    internal bool HasBoundaryLiuFengProgramPayment(ProgramSkillFrame parent, CardUseFrame? use = null)
    {
        if (!ValidBoundaryLiuFengCaller(parent) || parent.BoundaryLiuFengCaller is not { } r)
            return false;
        var n = r.Need;
        var target = r.TargetSeat;
        if (use is not null && (use.BoundaryLiuFeng?.FrameId != r.FrameId || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || !IsBoundaryLiuFengUse(use)))
            return false;
        if (parent.SunYuan is { } sy)
            return sy.Selection is { Materials.Count: 0, Sources: [var s] } sel && s == r.Source && sel.ActorSeat == n.ActorSeat && sel.ProviderSeat == n.ActorSeat && sel.TargetSeat == target && sel.Kind == CardKind.Slash;
        if (parent.Chanshuang is { } cs)
            return cs.Selection is { Materials.Count: 0, Sources: [var s] } sel && s == r.Source && sel.ActorSeat == n.ActorSeat && sel.ProviderSeat == n.ActorSeat && sel.TargetSeat == target && sel.Kind == CardKind.Slash;
        if (parent.WuJing is { } wu)
            return wu.Source == _characterWuJingOlOperations.WuJingSource(parent) && wu.Selection is { CardIds.Count: 0, Conversions: [var s], Kind: CardKind.Slash } && s == r.Source && wu.TargetSeat == target;
        if (parent.LvKuangLvXiang is { } qi)
            return qi.Source == CharacterLvKuangLvXiangOlRuntime.LvKuangLvXiangSource(parent) && qi.Actor == n.ActorSeat && qi.Target == target && qi.IssuingIds.Count == 0 && qi.IssuingKind == CardKind.Slash && qi.IssuingProvider == n.ActorSeat;
        if (parent.CaoChun is { Slash: { } cc })
            return cc.ParentFrameId == parent.Id && cc.Materials.Count == 0 && !cc.Ending && cc.TargetSeat == target && cc.ProviderSeat == n.ActorSeat && cc.Kind == CardKind.Slash;
        return parent.AnniversaryMaTie is { } mt && mt.Actors[mt.Cursor] == n.ActorSeat && mt.Target == target && mt.Materials.Count == 0 && mt.Provider == n.ActorSeat || n.Intent == BoundaryLiuFengIntent.GenericBasic && HasAcceptedBoundaryLiuFengKeAction(parent, parent.KeBiNengPingduan!.SelectedBasicAction!);
    }

    internal bool HasAcceptedBoundaryLiuFengKeAction(ProgramSkillFrame parent, LegalAction selected)
    {
        if (parent.KeBiNengPingduan is not { } ke || selected.ConversionSource is not { SkillId: BoundaryLiuFengComposition.Qinling } source || selected.CardId != 0 || selected.PlayedCardKind != CardKind.Slash || selected.Kind != LegalActionKind.Slash || selected.TargetSeats is not [var target])
            return false;
        return ValidBoundaryLiuFengCaller(parent) && parent.BoundaryLiuFengCaller is { Need: { Intent: BoundaryLiuFengIntent.GenericBasic } n } r && r.Source == source && r.TargetSeat == target && n.ActorSeat == ke.ActorSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengKeActionEvent>().Count(e => e == new BoundaryLiuFengKeActionEvent(parent.Id, parent.InstructionIndex, r.FrameId, target)) == 1;
    }

    internal bool ValidBoundaryLiuFengCaller(ProgramSkillFrame parent)
    {
        if (parent.BoundaryLiuFengCaller is not { } r || r.Source.SkillId != BoundaryLiuFengComposition.Qinling || r.Source.BindingId != BoundaryLiuFengComposition.Activation || r.Source.OwnerSeat != r.Need.ActorSeat || r.Need.OwnerFrameId != parent.Id || r.Need.RequestFrameId != parent.Id || r.Need.InstructionIndex != parent.InstructionIndex || r.Need.RequestOp != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.Hash || r.CostSequence <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengCostPaidEvent>().Where(e => e.FrameId == r.FrameId).ToArray()is not [var paid] || paid.Source != r.Source || paid.Hash != r.Hash || paid.CardId != r.CardId || paid.From != r.From || paid.Sequence != r.CostSequence || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.CostSequence && m.CardId == r.CardId && m.From == r.From) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengRequestReleasedEvent>().Count(e => e == new BoundaryLiuFengRequestReleasedEvent(r.FrameId, parent.Id, parent.Id, true)) != 1)
            return false;
        if (r.UseId > 0 && (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengUseIssuedEvent>().Where(e => e.Origin.FrameId == r.FrameId).ToArray()is not [var issue] || issue.Origin.Source != r.Source || issue.Origin.Hash != r.Hash || issue.Origin.UseId != r.UseId || issue.Origin.ActionId != r.ActionId || issue.Origin.Actor != r.Need.ActorSeat))
            return false;
        return !r.Returned || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryLiuFengUseReturnedEvent>().Count(e => e.FrameId == r.FrameId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
    }

    internal bool BoundaryLiuFengRequestChild(ProgramSkillFrame parent, ResolutionFrame child)
    {
        if (child is not ProgramSkillFrame { BoundaryLiuFeng: { Need: { } n } r } cost || n.OwnerFrameId != parent.Id || n.RequestFrameId != parent.Id || n.InstructionIndex != parent.InstructionIndex || n.RequestOp != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op || !ValidBoundaryLiuFeng(cost))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == cost.Id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1].Id == parent.Id && (r.UseId == 0 && BoundaryLiuFengNeedCurrent(r) || ValidBoundaryLiuFengCaller(parent) && parent.BoundaryLiuFengCaller!.FrameId == cost.Id);
    }

    internal void ResumeBoundaryLiuFengRequest(BoundaryLiuFengReceipt r)
    {
        var n = r.Need!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Qinling cannot resume its native caller before its own whole Slash returns.");
        if (n.Intent == BoundaryLiuFengIntent.BorrowedSword)
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordAfterSlash(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.Qinglong)
        {
            if (!_coreQinglongInsertionOperations.RestoreContinuationAfterQinglongFollowup(r.UseId))
                throw new InvalidOperationException("Qinling lost its exact suspended Qinglong parent.");
            return;
        }

        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
        if (parent.InstructionIndex != n.InstructionIndex || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op != n.RequestOp)
            throw new InvalidOperationException("Qinling returned to another native instruction.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { RequestedSlashDamagedOwner = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == r.UseId && e.TargetSeat == parent.OwnerSeat && e.Amount > 0) });
        if (parent.AnniversaryMaTie is not null)
        {
            _characterMaTieAnniversaryOperations.ContinueMaTie(parent.Id);
            return;
        }

        if (parent.SunYuan is not null)
        {
            _characterSunYuanOlOperations.ResumeSayingSuppliedSlash(parent);
            return;
        }

        if (parent.Chanshuang is not null)
        {
            _characterLiYiOlOperations.ResumeChanshuangSuppliedSlash(parent);
            return;
        }

        if (parent.LvKuangLvXiang is not null)
        {
            _characterLvKuangLvXiangOlOperations.RestoreQigongParentAttack(parent);
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        if (parent.WuJing is { NativeOrigin: { } wu } wr)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuJingHejiReturnedEvent(parent.Id, r.UseId));
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WuJing = wr with { Stage = WuJingStage.NativeChildren }, PendingMovementContinuation = new(parent.OwnerSeat, 0, null) });
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        if (parent.CaoChun is { Slash: { } cc } cr)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaNativeSlashReturnedEvent(parent.Id, r.UseId, r.ActionId));
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { CaoChun = cr with { Slash = cc with { Returned = true } } });
            _characterCaoChunOlOperations.ResumeCaoChun(parent.Id);
            return;
        }

        if (parent.FixedTargetSlash is { Issued: true })
            _coreFixedSlashAndDeclaredDeckProgramsOperations.TryResumeFixedSlashAndDeclaredDeck(parent.Id);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
    }

    internal void RepublishBoundaryLiuFengRequest(BoundaryLiuFengReceipt r)
    {
        var n = r.Need!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_resolutionStackOperations._resolutionStack.Any(f => f.Id == n.OwnerFrameId))
            return;
        if (n.Intent == BoundaryLiuFengIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            _coreNativeBorrowedSwordOperations.BeginBorrowedSwordSlashChoice(b, _coreNativeBorrowedSwordOperations.CanRequestBorrowedSwordFactionSlash(b));
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.Qinglong)
        {
            var q = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!;
            if (!_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive)
            {
                var attack = q.Attack;
                _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
                _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(attack.ResolutionId, n.ActorSeat, attack.TargetSeat, false, [], null));
                _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(attack.ResolutionId);
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }
            else
                _coreNativeQinglongOperations.PublishQinglongCrescentBladeChoice(q, _coreNativeQinglongOperations.CanRequestQinglongCrescentBladeFactionSlash(q));
            return;
        }

        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
        var e = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)!;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(parent))
        {
            if (!_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive)
                _characterSunYuanOlOperations.FinishSayingSuppliedSlashFailure(parent);
            else
                _characterSunYuanOlOperations.PublishSaying(parent);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parent))
        {
            if (!_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive)
                _characterLiYiOlOperations.FinishChanshuangSuppliedSlashFailure(parent);
            else
                _characterLiYiOlOperations.PublishChanshuang(parent);
            return;
        }

        if (parent.AnniversaryMaTie is not null)
        {
            _characterMaTieAnniversaryOperations.ContinueMaTie(parent.Id);
            return;
        }

        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent))
        {
            _characterLvKuangLvXiangOlOperations.PublishLvKuangLvXiang(parent);
            return;
        }

        if (_characterWuJingOlOperations.IsWuJingHejiRequest(parent))
        {
            _characterWuJingOlOperations.PublishWuJing(parent);
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(parent))
        {
            _characterCaoChunOlOperations.PublishCaoChun(parent);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.GenericBasic)
        {
            _characterKeBiNengOlOperations.PublishKeBiNeng(parent);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.ProgramSlash)
        {
            if (_coreProgramSlashRequestOperations.RequestProgramSlashByTarget(parent, n.ActorSeat, e.ResultBind!) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.ProgramNearestSlash)
        {
            if (_coreProgramSlashRequestOperations.RequestProgramSlashByNearest(parent, n.ActorSeat, e.Amount) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.AssistedSlash)
        {
            _coreAssistedPhysicalCardsOperations.PublishAssistedPhysicalSlashPrompt(parent);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.FixedSlash)
        {
            _coreFixedSlashAndDeclaredDeckProgramsOperations.PublishFixedSlashAndDeclaredDeckPrompt(parent, _coreFixedSlashAndDeclaredDeckProgramsOperations.FixedTargetSlashChoices(parent), "选择杀响应原请求。", n.Targets);
            return;
        }

        if (n.Intent == BoundaryLiuFengIntent.NearestLegalSlash)
        {
            var choices = _coreNearestLegalSlashRequestsOperations.NearestLegalSlashChoices(parent);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = BoundaryLiuFengNativeDecision(r.OriginalDecision!)with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = choices.SelectMany(c => c.Cards).Distinct().ToArray(),
                Choices = choices
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        }
    }
}
