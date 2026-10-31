#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal void StoreZonghuResponse(ZonghuPaidOrigin p)
    {
        var owner = _resolutionStackOperations._resolutionStack.Single(f => f.Id == p.Need.OwnerFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner switch
        {
            CardUseFrame u => u with { ZonghuResponseOrigin = p },
            ProgramSkillFrame f => f with { ZonghuResponseOrigin = p },
            _ => throw new InvalidOperationException("Zonghu response lost its typed incoming native owner.")});
    }

    internal static ZonghuPaidOrigin? ZonghuResponseOrigin(ResolutionFrame f) => RuntimeSkills.RuntimeRules_liu_zhang_ol_ZonghuResponses.ZonghuResponseOrigin(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27512)]
    internal void ObserveZonghuResponse(CardActionAcceptedEvent accepted)
    {
        var a = accepted.Action;
        if (!HasZonghuCapability || a.Type != CardActionType.Response || a.PhysicalCards.Count != 0 || a.ConversionChain is not [var source] || source.SkillId != "ol:zonghu")
            return;
        var owner = _resolutionStackOperations._resolutionStack.LastOrDefault(f => ZonghuResponseOrigin(f)is { } p && p.Source == source && p.Need.ParentActionId == a.ParentActionId);
        if (owner is null)
            return;
        var p = ZonghuResponseOrigin(owner)!;
        if (!ValidZonghuPayment(p) || p.OutputKind != a.EffectiveKind || p.Need.ActorSeat != a.ActorSeat || p.Need.ProviderSeat != a.ProviderSeat || p.Need.RequesterSeat != a.RequesterSeat || a.ResponderSeat != a.ActorSeat || a.TargetSeats.Count != 0 || a.OpponentSeat != p.Need.OpponentSeat || a.EffectiveIsRed is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuResponseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuUseIssuedEvent>().Any(e => e.AttemptId == p.AttemptId))
            throw new InvalidOperationException("Zonghu zero response changed its original native participants or already-issued cost.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZonghuResponseIssuedEvent(p.AttemptId, owner.Id, a.ActionId, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, p.OutputKind, ZonghuResponseContinuation(p)));
    }

    internal ProgramCardContinuation ZonghuResponseContinuation(ZonghuPaidOrigin p) => p.Need.Intent switch
    {
        ZonghuIntent.Dodge => ProgramCardContinuation.Dodge,
        ZonghuIntent.Duel => ProgramCardContinuation.DuelSlash,
        ZonghuIntent.FactionDodge => ProgramCardContinuation.FactionDefenseDodge,
        ZonghuIntent.FactionSlashPlayed => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest?.Purpose == FactionCardRequestPurpose.DuelResponse || _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResolutionId == p.Need.OwnerFrameId ? ProgramCardContinuation.FactionSlashDuelSlash : ProgramCardContinuation.FactionSlashGroupResponse,
        _ => ProgramCardContinuation.GroupResponse
    };
    internal void DispatchZonghuResponse(ZonghuPaidOrigin p)
    {
        var n = p.Need;
        StoreZonghuResponse(p);
        if (n.Intent == ZonghuIntent.FactionDodge)
        {
            _coreNativeFactionDefenseOperations.CompleteFactionDefenseResponse(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense!, _coreMatchParticipantsPlayersAccess._players[n.ProviderSeat], null, false, p.Source);
            return;
        }

        var attack = n.Intent == ZonghuIntent.Duel ? _coreNativeCardContinuationCursorsOperations.ActiveDuel!.Attack : n.Intent == ZonghuIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!.CurrentAttack! : n.Intent == ZonghuIntent.FactionSlashPlayed ? _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.ResponseAttack! : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!;
        var continuation = ZonghuResponseContinuation(p);
        var faction = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest;
        GameEngine.FactionProviderRewardReceipt? reward = null;
        if (n.Intent == ZonghuIntent.FactionSlashPlayed)
        {
            reward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(faction!);
            _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(faction!.ResolutionId, n.ActorSeat, true, n.ProviderSeat, 0, p.OutputKind, false));
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(n.OwnerFrameId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeResponseWindowsOperations.SetResponseParentStep(n.OwnerFrameId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, n.ActorSeat, n.OpponentSeat!.Value, p.OutputKind));
        if (n.Intent == ZonghuIntent.Duel || continuation == ProgramCardContinuation.FactionSlashDuelSlash)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(n.OwnerFrameId, n.ActorSeat, true, 0, p.OutputKind));
        else if (n.Intent == ZonghuIntent.Group || continuation == ProgramCardContinuation.FactionSlashGroupResponse)
        {
            var g = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(g.ResolutionId, g.Card.Kind, g.RequiredCardKind!.Value, n.ActorSeat, true, 0, p.OutputKind));
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind))
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(n.ActorSeat, p.OutputKind);
        if (reward is not null)
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(reward, n.ProviderSeat);
        if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, _coreMatchParticipantsPlayersAccess._players[n.ActorSeat], _coreMatchParticipantsPlayersAccess._players[n.ProviderSeat], n.RequesterSeat, n.OpponentSeat!.Value, p.OutputKind, [], continuation, p.Source))
            return;
        if (continuation is ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge)
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
        else if (continuation is ProgramCardContinuation.DuelSlash or ProgramCardContinuation.FactionSlashDuelSlash)
            _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(_coreNativeCardContinuationCursorsOperations.ActiveDuel!, n.ActorSeat);
        else
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal bool IsZonghuZeroResponse(CardActionContext a, ProgramCardContinuation c)
    {
        if (a.Type != CardActionType.Response || a.PhysicalCards.Count != 0 || a.ConversionChain is not [var source] || source.SkillId != "ol:zonghu")
            return false;
        return _resolutionStackOperations._resolutionStack.Any(f => ZonghuResponseOrigin(f)is { } p && ValidZonghuPayment(p) && p.Source == source && p.Need.ActorSeat == a.ActorSeat && p.Need.ProviderSeat == a.ProviderSeat && p.Need.RequesterSeat == a.RequesterSeat && p.Need.ParentActionId == a.ParentActionId && p.OutputKind == a.EffectiveKind && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZonghuResponseIssuedEvent>().Count(e => e.AttemptId == p.AttemptId && e.OwnerFrameId == f.Id && e.ActionId == a.ActionId && e.Continuation == c) == 1);
    }

    internal void DispatchZonghuFactionUse(ZonghuPaidOrigin p)
    {
        var n = p.Need;
        var f = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("Zonghu lost its actual faction provider request.");
        var reward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(f);
        var borrowed = f.BorrowedSword;
        long? parent = f.ProgramSkillFrameId;
        if (f.IsBorrowedSwordUse)
        {
            _coreNativeResponseWindowsOperations.PopResponseWindow(borrowed!.ResolutionId);
            borrowed.AwaitingSlashChoice = false;
            borrowed.SlashCardId = 0;
            borrowed.EffectiveSlashKind = p.OutputKind;
            _coreNativeCardUseFramesOperations.SetCardUseStep(borrowed.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        }
        else if (f.IsQinglongCrescentBladeUse)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, n.ActorSeat, p.TargetSeat, true, [], p.OutputKind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
        }
        else if (parent is { } id)
        {
            var program = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(program = program with { ZonghuReturn = new(p, 0, 0), Step = ResolutionFrameStep.ResolvingEffect });
            if (f.IsAssistedProgramUse)
            {
                if (_characterLiYiOlOperations.IsChanshuangSlashRequest(program) || _characterLvKuangLvXiangOlOperations.IsQigongRequest(program) || _characterWuJingOlOperations.IsWuJingHejiRequest(program) || _characterCaoChunOlOperations.IsCaoChunInitialRequest(program))
                    PrepareZonghuProgram(program, p);
                else
                    _coreProgramSlashRequestOperations.CommitProgramChoiceResult(program.Id, f.AssistedResultBind!, "used-slash", n.ActorSeat, "宗护供给真实杀。");
            }
        }

        f.AwaitingProviders = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(f.ResolutionId, n.ActorSeat, true, n.ProviderSeat, 0, p.OutputKind, true, p.TargetSeat));
        BeginZonghuSlash(p, borrowed, !f.IsAssistedProgramUse && !f.IsQinglongCrescentBladeUse, parent, f);
        _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(reward, n.ProviderSeat);
    }

    internal bool IsZonghuZeroSupply(CardUseFrame use, NativeOwners.FactionCardRequestHandle request) => use.ZonghuOrigin is { } p && p.Need.Intent == ZonghuIntent.FactionSlashUse && IsIssuedZonghuUse(use) && p.Need.OwnerFrameId == request.OwnerFrameId && p.Need.ActorSeat == request.OwnerSeat && p.Need.ProviderSeat == request.CurrentCandidateSeat && p.Need.RequesterSeat == request.OwnerSeat && p.Need.Cursor == request.CandidateIndex && request.TargetSeat == p.TargetSeat && request.ActiveAttack?.ResolutionId == use.Id;
    internal void RepublishZonghuNative(ZonghuReceipt r)
    {
        var n = r.Need;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ProviderSeat];
        if (!_resolutionStackOperations._resolutionStack.Any(f => f.Id == n.RequestFrameId) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        var keepCurrentRequest = actor.IsAlive && r.Paid is { } requestPayment && ValidZonghuPayment(requestPayment) && SameZonghuNeed(requestPayment.Need, n) && ZonghuNativeNeedCurrent(r);
        if (!actor.IsAlive && n.Intent is ZonghuIntent.Dodge or ZonghuIntent.Group or ZonghuIntent.Duel)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == n.OwnerFrameId)
                _coreNativeResponseWindowsOperations.PopResponseWindow(n.OwnerFrameId);
            _coreNativeResponseWindowsOperations.SetResponseParentStep(n.OwnerFrameId, ResolutionFrameStep.ResolvingEffect);
            var attack = n.Intent == ZonghuIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.CurrentAttack : n.Intent == ZonghuIntent.Duel ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.Attack : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
            if (attack is not null && attack.ResolutionId == n.OwnerFrameId)
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (!actor.IsAlive && n.Intent == ZonghuIntent.Dying)
        {
            _coreNativeDyingOperations.RunOneDyingStep();
            return;
        }

        if (n.Intent == ZonghuIntent.Dying)
        {
            _coreNativeDyingOperations.RequestHumanDyingResponse(actor, _coreNativeDyingOperations.GetDyingPeaches(actor), _coreNativeDyingOperations.GetDyingAlcohols(actor, _coreDyingCursorRuntimeOperations.Value.ActiveDying!.VictimSeat));
            return;
        }

        if (n.Intent == ZonghuIntent.FactionDodge)
        {
            _coreNativeFactionDefenseOperations.AdvanceFactionDefenseCandidate(keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent is ZonghuIntent.FactionSlashPlayed or ZonghuIntent.FactionSlashUse)
        {
            _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate(keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent == ZonghuIntent.Qinglong)
        {
            if (!actor.IsAlive)
            {
                if (r.Paid is not { } paid || !ValidZonghuPayment(paid) || !ZonghuNativeNeedCurrent(r) || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || blade.Attack.ResolutionId != n.OwnerFrameId || blade.Attack.SourceSeat != n.ActorSeat || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack))
                    throw new InvalidOperationException("A dead Zonghu payer lost its exact still-unconsumed Qinglong parent.");
                // This is the native no-follow-up return: no card or dead-player menu is issued.
                var attack = blade.Attack;
                _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(attack.ResolutionId, n.ActorSeat, attack.TargetSeat, false, [], null));
                _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(attack.ResolutionId);
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                return;
            }

            _coreNativeQinglongOperations.PublishQinglongCrescentBladeChoice(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!, _coreNativeQinglongOperations.CanRequestQinglongCrescentBladeFactionSlash(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!));
            return;
        }

        if (n.Intent == ZonghuIntent.BorrowedSword)
        {
            var b = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(b.ResolutionId);
            _coreNativeBorrowedSwordOperations.BeginBorrowedSwordSlashChoice(b, _coreNativeBorrowedSwordOperations.CanRequestBorrowedSwordFactionSlash(b), keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent == ZonghuIntent.Duel)
        {
            var d = _coreNativeCardContinuationCursorsOperations.ActiveDuel!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(d.ResolutionId);
            _coreNativeDuelOperations.BeginDuelResponse(d, keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent is ZonghuIntent.Dodge or ZonghuIntent.Group)
        {
            var attack = n.Intent == ZonghuIntent.Group ? _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!.CurrentAttack! : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!;
            var required = n.Intent == ZonghuIntent.Dodge ? CardKind.Dodge : _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!.RequiredCardKind!.Value;
            var cards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(actor, required);
            var choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(cards, required, attack.EffectiveCardKind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(actor, card, required), includeBagua: required == CardKind.Dodge && !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(actor), includeFactionDefense: required == CardKind.Dodge && _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionDefense(actor, attack), includeFactionSlash: required == CardKind.Slash && _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionSlashResponse(actor, attack), responder: actor);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = ZonghuNativeDecision(r.OriginalDecision)with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = choices.SelectMany(c => c.Cards).Distinct().ToArray(),
                Choices = choices
            };
            _coreMatchOutcomeStatusWinnerAccess._status = actor.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.OwnerFrameId);
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)!;
        if (!actor.IsAlive && n.Intent == ZonghuIntent.NearestLegalSlash)
        {
            if (r.Paid is not { } paid || !ValidZonghuPayment(paid) || !ZonghuNativeNeedCurrent(r) || effect.Op != SkillProgramEffectOp.RequestLegalSlashByNearest || n.RequestOp != effect.Op || f.InstructionIndex != n.InstructionIndex || n.ActorSeat != n.ProviderSeat || !f.ReexecuteParticipantInstruction || f.NearestLegalSlashRequest is not { AwaitingFaction: false } draft || draft.ActorSeat != n.ActorSeat || !draft.NearestSeats.SequenceEqual(n.Targets) || f.NumberBindings.SingleOrDefault(b => b.Name == "participant-cursor-" + (f.InstructionIndex - 1))is not { } cursor || cursor.Value < 1 || cursor.Value >= _coreMatchDefinitionOptionsPlayerCountAccess._playerCount || n.ActorSeat != _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, cursor.Value))
                throw new InvalidOperationException("A dead Zonghu payer lost its exact committed nearest-participant cursor.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { NearestLegalSlashRequest = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (_characterSunYuanOlOperations.IsSayingSlashRequest(f))
        {
            if (!actor.IsAlive)
                _characterSunYuanOlOperations.FinishSayingSuppliedSlashFailure(f);
            else
                _characterSunYuanOlOperations.PublishSaying(f);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(f))
        {
            if (!actor.IsAlive)
                _characterLiYiOlOperations.FinishChanshuangSuppliedSlashFailure(f);
            else
                _characterLiYiOlOperations.PublishChanshuang(f);
            return;
        }

        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(f))
        {
            _characterLvKuangLvXiangOlOperations.PublishLvKuangLvXiang(f);
            return;
        }

        if (_characterWuJingOlOperations.IsWuJingHejiRequest(f))
        {
            _characterWuJingOlOperations.PublishWuJing(f, keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(f))
        {
            _characterCaoChunOlOperations.PublishCaoChun(f);
            return;
        }

        if (n.Intent == ZonghuIntent.GenericBasic)
        {
            _characterKeBiNengOlOperations.PublishKeBiNeng(f);
            return;
        }

        if (n.Intent == ZonghuIntent.ProgramSlash)
        {
            _coreProgramSlashRequestOperations.RequestProgramSlashByTarget(f, n.ActorSeat, effect.ResultBind!, keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent == ZonghuIntent.ProgramNearestSlash)
        {
            _coreProgramSlashRequestOperations.RequestProgramSlashByNearest(f, n.ActorSeat, effect.Amount, keepCurrentRequest: keepCurrentRequest);
            return;
        }

        if (n.Intent == ZonghuIntent.AssistedSlash)
        {
            _coreAssistedPhysicalCardsOperations.PublishAssistedPhysicalSlashPrompt(f);
            return;
        }

        if (n.Intent == ZonghuIntent.FixedSlash)
        {
            _coreFixedSlashAndDeclaredDeckProgramsOperations.PublishFixedSlashAndDeclaredDeckPrompt(f, _coreFixedSlashAndDeclaredDeckProgramsOperations.FixedTargetSlashChoices(f), "选择杀响应原请求。", n.Targets);
            return;
        }

        if (n.Intent == ZonghuIntent.NearestLegalSlash)
        {
            var choices = _coreNearestLegalSlashRequestsOperations.NearestLegalSlashChoices(f);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = ZonghuNativeDecision(r.OriginalDecision)with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = choices.SelectMany(c => c.Cards).Distinct().ToArray(),
                Choices = choices
            };
            _coreMatchOutcomeStatusWinnerAccess._status = actor.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        throw new InvalidOperationException("Zonghu has no native failed-request return.");
    }
}
