#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal const string HuanpeiOpen = global::CardGame.Core.RuntimeSkills.RuntimeRules_huan_shu_jun_ol_HuanpeiRequests.HuanpeiOpen;
    internal HuanpeiNativeRequest? CaptureHuanpeiRequest(PendingDecision p, ResolutionFrame? suppliedTop = null)
    {
        var top = suppliedTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (top is null || top is ProgramSkillFrame { HuanShuJun: { Op: SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse } })
            return null;
        HuanpeiNativeRequest? request = null;
        if (top is ProgramSkillFrame f && p.Kind == DecisionKind.ProgramTrigger)
        {
            if (IsWenyiPeachRequest(f) && p.PlayerSeat == f.OwnerSeat)
                request = new(HuanpeiIntent.WenyiPeach, f.Id, f.Id, null, p.PlayerSeat, f.InstructionIndex, CardKind.Peach, f.HuanShuJun!.TargetSeat, p.PromptId, p.Revision);
            else if (f.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicChoice } ke && ke.ActorSeat == p.PlayerSeat)
                request = new(HuanpeiIntent.KeBiNengBasic, f.Id, f.Id, null, p.PlayerSeat, f.InstructionIndex, null, null, p.PromptId, p.Revision);
            else if (f.FixedTargetSlash is { Issued: false } fixedSlash && f.OwnerSeat == p.PlayerSeat && (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner || _characterLeiBoAnniversaryOperations.IsAnniversaryLeiBoFixedSlash(f)))
                request = new(HuanpeiIntent.FixedSlash, f.Id, f.Id, null, p.PlayerSeat, f.InstructionIndex, CardKind.Slash, fixedSlash.TargetSeat, p.PromptId, p.Revision);
        }

        if (request is null && _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicContext(p, top)is { } n)
        {
            var intent = n.Intent switch
            {
                DrawFundedDistinctBasicIntent.OwnSlashDodge => HuanpeiIntent.OwnSlashDodge,
                DrawFundedDistinctBasicIntent.Dying => HuanpeiIntent.Dying,
                DrawFundedDistinctBasicIntent.BorrowedSword => HuanpeiIntent.BorrowedSword,
                DrawFundedDistinctBasicIntent.Qinglong => HuanpeiIntent.Qinglong,
                DrawFundedDistinctBasicIntent.ProgramSlash => HuanpeiIntent.ProgramSlash,
                DrawFundedDistinctBasicIntent.ProgramNearestSlash => HuanpeiIntent.ProgramNearestSlash,
                DrawFundedDistinctBasicIntent.NearestLegalSlash => HuanpeiIntent.NearestLegalSlash,
                _ => HuanpeiIntent.AssistedSlash
            };
            request = new(intent, n.ParentFrameId, n.RequestFrameId, n.ParentActionId, p.PlayerSeat, n.Cursor, intent == HuanpeiIntent.OwnSlashDodge ? CardKind.Dodge : intent == HuanpeiIntent.Dying ? null : CardKind.Slash, n.FixedTargetSeat, p.PromptId, p.Revision);
        }

        if (request is not null && request.ParentFrameId is { } parentId && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == parentId)is ProgramSkillFrame parent)
            request = request with
            {
                ParentSkillId = parent.SkillId,
                ParentSkillInstanceId = parent.SkillInstanceId,
                ParentGameplayHash = parent.GameplayHash,
                ParentOperation = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op
            };
        return request;
    }

    internal global::CardGame.Core.GameEngine.DrawFundedBasicNeed HuanpeiNativeNeed(HuanpeiNativeRequest n) => new(n.Intent switch
    {
        HuanpeiIntent.OwnSlashDodge => DrawFundedDistinctBasicIntent.OwnSlashDodge,
        HuanpeiIntent.Dying => DrawFundedDistinctBasicIntent.Dying,
        HuanpeiIntent.BorrowedSword => DrawFundedDistinctBasicIntent.BorrowedSword,
        HuanpeiIntent.Qinglong => DrawFundedDistinctBasicIntent.Qinglong,
        HuanpeiIntent.ProgramSlash => DrawFundedDistinctBasicIntent.ProgramSlash,
        HuanpeiIntent.ProgramNearestSlash => DrawFundedDistinctBasicIntent.ProgramNearestSlash,
        HuanpeiIntent.NearestLegalSlash => DrawFundedDistinctBasicIntent.NearestLegalSlash,
        _ => DrawFundedDistinctBasicIntent.AssistedSlash
    }, n.ParentFrameId, n.RequestFrameId, n.ParentActionId, n.Cursor, n.FixedTargetSeat);
    internal bool HuanpeiRequestParentCurrent(HuanpeiNativeRequest n)
    {
        if (n.Intent == HuanpeiIntent.Play)
            return _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat == n.ActorSeat;
        if (n.RequestFrameId is not { } id || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is not { } request)
            return false;
        if (n.ParentSkillId is null)
            return true;
        return request is ProgramSkillFrame p && p.Id == n.ParentFrameId && p.InstructionIndex == n.Cursor && p.SkillId == n.ParentSkillId && p.SkillInstanceId == n.ParentSkillInstanceId && p.GameplayHash == n.ParentGameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)?.Op == n.ParentOperation;
    }

    internal bool HuanpeiNativeLegal(HuanpeiNativeRequest n, CardKind kind, int? target)
    {
        if (!HuanpeiRequestParentCurrent(n) || !global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlDefinitions.HuanpeiKinds.Contains(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(n.ActorSeat, kind, CardActionType.Use) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(n.ActorSeat, [], null) || !_coreMatchParticipantsPlayersAccess._players[n.ActorSeat].IsAlive)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.ActorSeat];
        if (n.Intent == HuanpeiIntent.WenyiPeach)
            return kind == CardKind.Peach && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == n.ParentFrameId)is ProgramSkillFrame p && TryGetWenyiPeachRequest(p, out var a, out var t) && a == actor.Seat && t == target && WenyiTargetCanReceivePeach(p);
        if (n.Intent == HuanpeiIntent.FixedSlash)
            return global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && target is { } seat && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == n.ParentFrameId)is ProgramSkillFrame { FixedTargetSlash: { } fs } && seat == fs.TargetSeat && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[seat], new(0, kind, Suit.None, 0), effectiveKind: kind, physicalCardIds: [], ignoreDistance: fs.IgnoreDistance);
        if (n.Intent == HuanpeiIntent.KeBiNengBasic)
        {
            var source = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.HuanShuJun?.Request == n)?.HuanShuJun?.Source ?? HuanpeiSources(actor).FirstOrDefault();
            return source is not null && HuanpeiNativePlayActions(actor, source, true).Any(a => a.PlayedCardKind == kind && (a.TargetSeats.Count == 0 ? target is null || target == actor.Seat : a.TargetSeats.Contains(target ?? -1)));
        }

        return _coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicNativeResponseLegal(actor, HuanpeiNativeNeed(n), kind, target);
    }

    internal IReadOnlyList<LegalAction> HuanpeiNativePlayActions(CharacterState actor, CardConversionSource source, bool requested = false)
    {
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !requested && (_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat != actor.Seat) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [], null))
            return[];
        var result = new List<LegalAction>();
        foreach (var kind in global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlDefinitions.HuanpeiKinds.Where(k => k != CardKind.Dodge && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, k, CardActionType.Use)))
        {
            var card = new Card(0, kind, Suit.None, 0);
            if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            {
                var targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, p, card, source, kind, noEffectiveRank: true, physicalCardIds: [])).ToArray();
                result.AddRange(targets.Select(p => new LegalAction(LegalActionKind.Slash, 0, p.Seat, $"视为对{(p.Name)}使用【{(card.DisplayName)}】", kind) { ConversionSource = source }));
                _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(result, actor, card, targets, card.DisplayName, kind, source, null);
            }
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
                result.Add(new(LegalActionKind.Peach, 0, null, "视为使用【桃】", kind) { ConversionSource = source });
            else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None) && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, kind)))
                result.Add(new(LegalActionKind.Alcohol, 0, null, "视为使用【酒】", kind) { ConversionSource = source });
        }

        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(result, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(result, actor);
        return Array.AsReadOnly(result.ToArray());
    }

    internal IReadOnlyList<LegalAction> HuanpeiPaidPlayActions(ProgramSkillFrame f) => f.HuanShuJun is { Paid: true } r ? HuanpeiNativePlayActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.Source, r.Request?.Intent == HuanpeiIntent.KeBiNengBasic) : [];
    internal IReadOnlyList<PromptChoice> HuanpeiResponseChoices(PendingDecision p)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started || !HasHuanShuJunCapability || CaptureHuanpeiRequest(p)is not { } n || !CanAdjustHuanpeiHand(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]))
            return[];
        var result = new List<PromptChoice>();
        foreach (var source in HuanpeiSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]))
        {
            if (n.Intent == HuanpeiIntent.KeBiNengBasic)
            {
                if (HuanpeiNativePlayActions(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat], source, true).Count == 0)
                    continue;
                var args = new Dictionary<string, string>
                {
                    [HuanpeiOpen] = n.Intent.ToString()
                };
                global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
                result.Add(new(new($"huanpei.open.{(p.PromptId.Value)}.{(source.SkillInstanceId)}.basic"), "环佩：调整手牌，然后使用一张基本牌", [], [], args));
                continue;
            }

            var kinds = n.Intent == HuanpeiIntent.OwnSlashDodge ? new[]
            {
                CardKind.Dodge
            }

            : n.Intent == HuanpeiIntent.Dying ? new[]
            {
                CardKind.Peach,
                CardKind.Alcohol
            }

            : n.Intent == HuanpeiIntent.WenyiPeach ? new[]
            {
                CardKind.Peach
            }

            : new[]
            {
                CardKind.Slash,
                CardKind.FireSlash,
                CardKind.ThunderSlash
            };
            foreach (var kind in kinds)
                foreach (var target in n.FixedTargetSeat is { } fixedTarget ? new[]
                {
                    fixedTarget
                }

                : p.ValidTargetSeats)
                {
                    if (!HuanpeiNativeLegal(n, kind, target))
                        continue;
                    var args = new Dictionary<string, string>
                    {
                        [HuanpeiOpen] = n.Intent.ToString(),
                        ["output-kind"] = kind.ToString(),
                        ["native-target-seat"] = target.ToString(CultureInfo.InvariantCulture)
                    };
                    global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
                    IReadOnlyList<int> responseTargets = (n.Intent is HuanpeiIntent.OwnSlashDodge or HuanpeiIntent.Dying) ? Array.Empty<int>() : new[]
                    {
                        target
                    };
                    result.Add(new(new($"huanpei.open.{(p.PromptId.Value)}.{(source.SkillInstanceId)}.{(kind)}.{(target)}"), $"环佩：调整手牌，视为使用【{(CardCatalog.Get(kind).DisplayName)}】", [], responseTargets, args));
                }
        }

        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal PendingDecision? AddHuanpeiRequestChoices(PendingDecision? p)
    {
        if (p is null || p.Choices.Any(c => c.Parameters.ContainsKey(HuanpeiOpen)))
            return p;
        var choices = HuanpeiResponseChoices(p);
        return choices.Count == 0 ? p : p with
        {
            Choices = Array.AsReadOnly(p.Choices.Concat(choices).ToArray())
        };
    }

    internal static PendingDecision HuanpeiNativeDecision(PendingDecision p) => global::CardGame.Core.RuntimeSkills.RuntimeRules_huan_shu_jun_ol_HuanpeiRequests.HuanpeiNativeDecision(p);
    internal bool IsHuanpeiAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && c.Parameters.ContainsKey(HuanpeiOpen)) == true;
    internal CommandResult SubmitHuanpeiAnswer(int actor, PromptId prompt, ChoiceId id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != actor || p.PromptId != prompt)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidPrompt, "环佩必须回答当前真实请求。");
        var c = p.Choices.SingleOrDefault(c => c.Id == id && c.Parameters.ContainsKey(HuanpeiOpen));
        if (c is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidChoice, "环佩选择未发布。");
        return _kernelCoreMainOperations2.Accept(() =>
        {
            TryResolveHuanpeiResponse(c, true);
        });
    }

    internal bool TryResolveHuanpeiResponse(PromptChoice choice, bool advance)
    {
        if (!choice.Parameters.ContainsKey(HuanpeiOpen))
            return false;
        var p = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("Huanpei lost its original native prompt.");
        if (!HuanpeiResponseChoices(HuanpeiNativeDecision(p)).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || CaptureHuanpeiRequest(p)is not { } n)
            throw new InvalidOperationException("Huanpei changed its published native need.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        var definition = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        var r = new HuanShuJunReceipt(source, definition.GameplayHash, SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, n.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, HuanShuJunStage.AdjustHand)
        {
            Request = n,
            OriginalDecision = HuanpeiNativeDecision(p),
            Round = _coreActualRoundProgressRoundNumberAccess._roundNumber,
            TargetSeat = n.FixedTargetSeat ?? -1,
            Kind = choice.Parameters.TryGetValue("output-kind", out var raw) ? Enum.Parse<CardKind>(raw) : null,
            Targets = choice.Targets,
            FrozenHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]).Count,
            FrozenHp = _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].Hp
        };
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, p.PlayerSeat, source.SkillId, source.BindingId, definition.GameplayHash, 0, [], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            HuanShuJun = r
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanShuJunStartedEvent(f.Id, source, definition.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.TargetSeat, r.FrozenHandCount, r.FrozenHp));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiRequestCapturedEvent(f.Id, n, HuanShuJunDecisionHash(r.OriginalDecision!)));
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(f.Id, host, host);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance && _coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        return true;
    }

    internal void ContinueHuanpeiOriginalRequest(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        if (r.Request!.Intent == HuanpeiIntent.KeBiNengBasic)
        {
            PublishHuanShuJun(f);
            return;
        }

        var targets = r.Request.FixedTargetSeat is { } target ? new[]
        {
            target
        }

        : r.Targets;
        if (r.Kind is not { } kind || !HuanpeiNativeLegal(r.Request, kind, targets.FirstOrDefault(-1)))
        {
            FinishHuanShuJun(f);
            return;
        }

        ChooseHuanpeiOutput(f, kind, targets);
    }

    internal void ChooseHuanpeiOutput(ProgramSkillFrame f, CardKind kind, IReadOnlyList<int> targets)
    {
        var r = f.HuanShuJun!;
        if (!r.Paid || r.Stage != HuanShuJunStage.BasicChoice || r.Request is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Huanpei cannot issue an unpaid or dead user's output.");
        if (r.Request.Intent is HuanpeiIntent.Play or HuanpeiIntent.KeBiNengBasic ? !HuanpeiPaidPlayActions(f).Any(a => a.PlayedCardKind == kind && a.TargetSeats.SequenceEqual(targets)) : !HuanpeiNativeLegal(r.Request, kind, targets.FirstOrDefault(-1)))
            throw new InvalidOperationException("Huanpei's paid output changed its native target or current use legality.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiOutputChosenEvent(f.Id, kind, targets.Count));
        for (var i = 0; i < targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiTargetChosenEvent(f.Id, i, targets[i]));
        var origin = new HuanpeiPaidOrigin(f.Id, r.Source, r.GameplayHash, r.Round, r.ActualTurn, r.TurnOwner, r.Request, r.FrozenHandCount, r.FrozenHp, Math.Abs(r.FrozenHandCount - r.FrozenHp), r.Before, r.After, kind)
        {
            Targets = targets
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = null, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanShuJunCompletedEvent(f.Id, r.Op, true));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        ExecuteHuanpeiPaid(origin);
    }

    internal void RepublishHuanpeiOriginal(HuanShuJunReceipt r)
    {
        if (r.Request is null || r.OriginalDecision is null || !HuanpeiRequestParentCurrent(r.Request) || !_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = r.OriginalDecision with
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Revision = _coreCommandSessionQueriesOperations.Revision
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool TryAdvanceHuanpeiAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].IsHuman)
            return false;
        var c = p.Choices.FirstOrDefault(c => c.Parameters.ContainsKey(HuanpeiOpen));
        return c is not null && TryResolveHuanpeiResponse(c, false);
    }

    internal bool HasHuanpeiRequiredBasic(CharacterState actor, CardKind kind) => HuanpeiSources(actor).Any() && CanAdjustHuanpeiHand(actor) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [], null) && (kind != CardKind.Dodge || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind));
    internal bool HasHuanpeiDyingResponse(CharacterState actor) => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HuanpeiSources(actor).Any() && CanAdjustHuanpeiHand(actor) && new[]
    {
        CardKind.Peach,
        CardKind.Alcohol
    }.Any(kind => HuanpeiNativeLegal(new(HuanpeiIntent.Dying, d.FrameId, d.FrameId, null, actor.Seat, d.ResponderIndex, null, d.VictimSeat, new(0), _coreCommandSessionQueriesOperations.Revision), kind, d.VictimSeat));
    internal bool HasHuanpeiForcedSlash(CharacterState actor, CharacterState target, bool qinglong) => HuanpeiSources(actor).Any() && CanAdjustHuanpeiHand(actor) && new[]
    {
        CardKind.Slash,
        CardKind.FireSlash,
        CardKind.ThunderSlash
    }.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, Suit.None) && (qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, kind, null, false, Suit.None, 0) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, kind, Suit.None, false, null, 0, [])));
    internal bool HasHuanpeiProgramSlash(CharacterState actor, IEnumerable<int> targets)
    {
        if (!HuanpeiSources(actor).Any() || !CanAdjustHuanpeiHand(actor) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent)
            return false;
        var candidates = targets.ToArray();
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, actor.Seat, "", [], candidates)
        {
            Choices = []
        };
        var need = CaptureHuanpeiRequest(prompt, parent);
        var ignore = _characterLvKuangLvXiangOlOperations.IsQigongRequest(parent) || _characterWuJingOlOperations.IsWuJingHejiRequest(parent);
        return candidates.Any(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreMatchParticipantsPlayersAccess._players[t].IsAlive && global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlDefinitions.HuanpeiKinds.Where(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard).Any(kind => need is not null ? HuanpeiNativeLegal(need, kind, t) : _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: [], ignoreDistance: ignore)));
    }
}
