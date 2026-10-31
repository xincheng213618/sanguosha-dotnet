#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRequestedDeckBasicsRuntime
{
    internal IEnumerable<(CardConversionSource Source, SkillProgramCardPolicy Policy)> RequestedDeckBasicSources(CharacterState actor, CardKind required) => actor.IsAlive && actor.Seat != _coreActualTurnProgressCurrentSeatStartedAccess._currentSeat && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _coreProgramCardPoliciesOperations.CardPolicies(actor, SkillProgramCardPolicyKind.PrivateTopBasicRequest, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(required) ? CardKind.Slash : required).Select(p => (new CardConversionSource(p.Source.SkillId, p.Policy.Id, actor.Seat, p.Source.SkillInstanceId), p.Policy)) : [];
    internal bool HasRequestedDeckBasicSource(CharacterState actor, CardKind required) => RequestedDeckBasicSources(actor, required).Any(p => p.Policy.CardKinds.Contains(required) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(required) && p.Policy.CardKinds.Contains(CardKind.Slash));
    internal (RequestedDeckBasicIntent Intent, long OwnerId, long RequestId, int Cursor)? RequestedDeckBasicContext(PendingDecision p, ResolutionFrame? suppliedTop = null)
    {
        var top = suppliedTop ?? _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (top is null || top is RequestedDeckBasicFrame)
            return null;
        long owner = top is ResponseWindowFrame response ? response.ParentFrameId : top.Id;
        if (p.Kind == DecisionKind.RescueDying && _coreDyingCursorRuntimeOperations.ActiveDying is { } d && d.ResponderSeat == p.PlayerSeat && top.Id == d.Id)
            return (RequestedDeckBasicIntent.Dying, d.Id, top.Id, d.ResponderIndex);
        if (p.Kind == DecisionKind.RespondDodge)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } fd && fd.CurrentCandidateSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.FactionDodge, owner, top.Id, fd.CandidateIndex);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } g } && g.TargetSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.Group, owner, top.Id, g.SuccessfulDodgeResponses);
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } a && a.TargetSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.Dodge, owner, top.Id, a.SuccessfulDodgeResponses);
        }

        if (p.Kind == DecisionKind.RespondSlash)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } fr && fr.CurrentCandidateSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.FactionSlash, owner, top.Id, fr.CandidateIndex);
            if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } bs && bs.WeaponOwnerSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.BorrowedSword, owner, top.Id, 0);
            if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && duel.ResponderSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.Duel, owner, top.Id, 0);
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } ga } && ga.TargetSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.Group, owner, top.Id, 0);
        }

        if (p.Kind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } q && q.Attack.SourceSeat == p.PlayerSeat)
            return (RequestedDeckBasicIntent.Qinglong, owner, top.Id, 0);
        if (p.Kind == DecisionKind.ProgramTrigger && top is ProgramSkillFrame program)
        {
            if (_characterHuanShuJunOlOperations.IsWenyiPeachRequest(program) && _characterHuanShuJunOlOperations.TryGetWenyiPeachRequest(program, out var wenyiActor, out _) && p.PlayerSeat == wenyiActor)
                return (RequestedDeckBasicIntent.WenyiPeach, program.Id, program.Id, program.InstructionIndex);
            if (_characterSunYuanOlOperations.IsSayingSlashRequest(program) && _characterSunYuanOlOperations.TryGetSayingSlashRequest(program, out var sayingActor, out _) && p.PlayerSeat == sayingActor)
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            if (_characterLiYiOlOperations.IsChanshuangSlashRequest(program) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(program, out var requestedActor, out _) && p.PlayerSeat == requestedActor)
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(program) && p.PlayerSeat == program.LvKuangLvXiang!.Actor)
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(program) && p.PlayerSeat == program.OwnerSeat)
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            if (_characterMaTieAnniversaryOperations.AnniversaryMaTieRequestActor(program, p.PlayerSeat))
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            var actions = p.Choices.Select(c => c.Parameters.GetValueOrDefault("program-action")).ToArray();
            if (actions.Contains("request-slash-decline"))
                return (RequestedDeckBasicIntent.ProgramSlash, program.Id, program.Id, program.InstructionIndex);
            if (actions.Contains("request-slash-nearest-decline"))
                return (RequestedDeckBasicIntent.ProgramNearestSlash, program.Id, program.Id, program.InstructionIndex);
            if (program.AssistedSlashRequest is { TargetSeat: not null } assisted && assisted.ActorSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.AssistedSlash, program.Id, program.Id, program.InstructionIndex);
            if (program.NearestLegalSlashRequest is { AwaitingFaction: false } nearest && nearest.ActorSeat == p.PlayerSeat)
                return (RequestedDeckBasicIntent.NearestLegalSlash, program.Id, program.Id, program.InstructionIndex);
        }

        return null;
    }

    internal CardKind RequestedDeckBasicKind(PendingDecision p, RequestedDeckBasicIntent intent) => intent switch
    {
        RequestedDeckBasicIntent.Dying or RequestedDeckBasicIntent.WenyiPeach => CardKind.Peach,
        RequestedDeckBasicIntent.Dodge or RequestedDeckBasicIntent.FactionDodge => CardKind.Dodge,
        RequestedDeckBasicIntent.Group => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!.RequiredCardKind!.Value,
        _ => CardKind.Slash
    };
    internal PendingDecision? AddRequestedDeckBasicChoices(PendingDecision? p)
    {
        if (p is null || !_coreActualTurnProgressCurrentSeatStartedAccess._started || _resolutionStackOperations._resolutionStack.LastOrDefault() is RequestedDeckBasicFrame || p.Choices.Any(c => c.Parameters.ContainsKey("deck-basic-source")) || RequestedDeckBasicContext(p)is not { } context)
            return p;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat];
        var required = RequestedDeckBasicKind(p, context.Intent);
        if (context.Intent == RequestedDeckBasicIntent.Dying ? !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(actor.Seat, _coreDyingCursorRuntimeOperations.ActiveDying!.VictimSeat) && !(actor.Seat == _coreDyingCursorRuntimeOperations.ActiveDying.VictimSeat && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Alcohol, CardActionType.Use)) : _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, required, _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(context.Intent)))
            return p;
        var owner = _resolutionStackOperations._resolutionStack.Single(f => f.Id == context.OwnerId);
        if (owner.RequestedDeckBasicMaterial is { } seen && seen.OriginalPromptId == p.PromptId && seen.RequestFrameId == context.RequestId && seen.ActorSeat == actor.Seat && seen.Cursor == context.Cursor)
            return p;
        var sources = RequestedDeckBasicSources(actor, required).Where(s => s.Policy.CardKinds.Contains(required) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(required) && s.Policy.CardKinds.Contains(CardKind.Slash)).ToArray();
        if (sources.Length == 0)
            return p;
        // Older AI producers use PromptId zero. Only this opt-in real need gets
        // an issued prompt token, so a later need is not confused with a refusal.
        if (p.PromptId == default)
            p = p with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId()
            };
        var choices = p.Choices.ToList();
        foreach (var(source, _)in sources)
        {
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId);
            var parameters = new Dictionary<string, string>
            {
                ["deck-basic-source"] = "activate"
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            choices.Add(new(new($"deck-basic.activate.{(source.SkillId)}.{(source.BindingId)}.{(source.SkillInstanceId)}"), $"发动【{(skill.Name)}】，观看牌堆顶可用基本牌", [], [], parameters));
        }

        return p with
        {
            Choices = Array.AsReadOnly(choices.ToArray())
        };
    }

    internal bool IsRequestedDeckBasicAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && c.Parameters.ContainsKey("deck-basic-source")) == true;
    internal CommandResult SubmitRequestedDeckBasicAnswer(int actor, PromptId prompt, ChoiceId choice)
    {
        if (!_coreActualTurnProgressCurrentSeatStartedAccess._started)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != actor || p.PromptId != prompt)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "顶牌选择必须回答当前精确提示。");
        var selected = p.Choices.SingleOrDefault(c => c.Id == choice);
        if (selected is null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "顶牌选择未公布。");
        return _kernelCoreMainOperations.Accept(() =>
        {
            ResolveRequestedDeckBasicAnswer(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void ResolveRequestedDeckBasicAnswer(PromptChoice c)
    {
        if (c.Parameters.GetValueOrDefault("deck-basic-source") == "activate")
        {
            BeginRequestedDeckBasicView(c);
            return;
        }

        var view = _resolutionStackOperations._resolutionStack.LastOrDefault() as RequestedDeckBasicFrame ?? throw new InvalidOperationException("A private top view lost its typed request owner.");
        _coreRequestedDeckBasicOwnershipOperations.AssertRequestedDeckBasicFrame(view);
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], RequestedDeckBasicChoices(view).Where(x => x.Id == c.Id).ToArray()))
            throw new InvalidOperationException("The private top choice changed.");
        var r = view.Receipt;
        if (c.Parameters["deck-basic-source"] == "decline")
        {
            _coreRuntimeOperations.PopResolutionFrame(view.Id, ResolutionFrameKind.RequestedDeckBasic);
            var owner = _resolutionStackOperations._resolutionStack.Single(f => f.Id == r.OwnerFrameId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { RequestedDeckBasicMaterial = r });
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(view.OriginalDecision);
            _coreMatchOutcomeStatusWinnerAccess._status = r.Intent == RequestedDeckBasicIntent.Dying ? EngineStatus.AwaitingHumanDying : _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(x => x.Id == c.Cards.Single());
        if (_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(r.Intent) == CardActionType.Response)
            _coreCardConversionsOperations.CaptureSelectedResponseConversion(c);
        _coreRuntimeOperations.PopResolutionFrame(view.Id, ResolutionFrameKind.RequestedDeckBasic);
        var original = _resolutionStackOperations._resolutionStack.Single(f => f.Id == r.OwnerFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(original.Id, original with { RequestedDeckBasicMaterial = r with { SelectedCardId = card.Id, SelectedKind = card.Kind, Claiming = true } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(view.OriginalDecision);
        DispatchRequestedDeckBasicMaterial(r.Intent, card, c.Targets);
    }

    internal void BeginRequestedDeckBasicView(PromptChoice choice)
    {
        var original = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("A deck-basic offer requires a current real need.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        var context = RequestedDeckBasicContext(original) ?? throw new InvalidOperationException("The basic need no longer has its original producer.");
        var actor = _coreMatchParticipantsPlayersAccess._players[original.PlayerSeat];
        var policy = RequestedDeckBasicSources(actor, RequestedDeckBasicKind(original, context.Intent)).SingleOrDefault(s => s.Source == source).Policy;
        if (policy is null || !original.Choices.Any(c => c.Id == choice.Id && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("The optional top view source lost current qualification.");
        var count = global::CardGame.Core.EngineOwners.CharacterZhugeKeMobileRuntime.IsMobileZhugeKeAocai(source.SkillId) ? policy.Value : _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count == 0 ? policy.Value * 2 : policy.Value;
        // This is the same bounded refresh used by actual draws. Never fill a short nonempty pile by moving looked-at cards.
        _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        var ids = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Take(count).Select(c => c.Id).ToArray();
        var viewId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var r = new RequestedDeckBasicMaterial(context.OwnerId, context.RequestId, original.PromptId, original.Revision, context.Intent, actor.Seat, context.Cursor, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, ids.Length, null, null)
        {
            ViewFrameId = viewId
        };
        var view = new RequestedDeckBasicFrame(viewId, context.RequestId, r, original, ids);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(view);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RequestedDeckBasicViewedEvent(view.Id, r.OwnerFrameId, actor.Seat, source.SkillId, ids.Length, r.Intent));
        PublishRequestedDeckBasicView(view);
    }

    internal int[] RequestedDeckBasicTargets(RequestedDeckBasicFrame view, Card card)
    {
        var r = view.Receipt;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        if (r.Intent == RequestedDeckBasicIntent.WenyiPeach)
            return _coreWenyiPeachSuppliesOperations.WenyiRequestedDeckPeachTargets(view, card);
        var targets = r.Intent switch
        {
            RequestedDeckBasicIntent.BorrowedSword => new[]
            {
                _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!.SlashTargetSeat
            },
            RequestedDeckBasicIntent.Qinglong => new[]
            {
                _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack.TargetSeat
            },
            RequestedDeckBasicIntent.ProgramSlash => new[]
            {
                ((ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == r.OwnerFrameId)).OwnerSeat
            },
            RequestedDeckBasicIntent.AssistedSlash => _resolutionStackOperations._resolutionStack.Single(f => f.Id == r.OwnerFrameId)is ProgramSkillFrame cao && (_characterCaoChunOlOperations.IsCaoChunInitialRequest(cao) || _characterLvKuangLvXiangOlOperations.IsQigongRequest(cao) || _characterLiYiOlOperations.IsChanshuangSlashRequest(cao) || _characterSunYuanOlOperations.IsSayingSlashRequest(cao) || _characterMaTieAnniversaryOperations.AnniversaryMaTieRequestActor(cao, r.ActorSeat)) ? view.OriginalDecision.ValidTargetSeats.ToArray() : new[]
            {
                ((ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == r.OwnerFrameId)).AssistedSlashRequest!.TargetSeat!.Value
            },
            RequestedDeckBasicIntent.ProgramNearestSlash or RequestedDeckBasicIntent.NearestLegalSlash => view.OriginalDecision.ValidTargetSeats.ToArray(),
            _ => Array.Empty<int>()};
        return targets.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive && (r.Intent == RequestedDeckBasicIntent.Qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], card.Kind, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), false, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), _coreFinalTargetSlashOperations.SpecificSlashRank(actor, card, card.Kind)) : _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], card, null, card.Kind, physicalCardIds: [card.Id], ignoreDistance: r.Intent == RequestedDeckBasicIntent.AssistedSlash && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.OwnerFrameId)is ProgramSkillFrame request && _characterLvKuangLvXiangOlOperations.QigongAdapterActor(request, actor.Seat, t, r.Cursor, r.OwnerFrameId, r.RequestFrameId)))).ToArray();
    }

    internal bool RequestedDeckBasicCardMatches(RequestedDeckBasicFrame view, Card card)
    {
        var r = view.Receipt;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        if (_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(r.Intent) == CardActionType.Response && !_characterHanDangMouMobileOperations.IsMouHanDangNativeResponseAllowed(actor, card))
            return false;
        if (_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(r.Intent) == CardActionType.Use ? !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, card.Kind, [card.Id])) : !_coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(actor, card.Kind, [card.Id], dyingUse: r.Intent == RequestedDeckBasicIntent.Dying))
            return false;
        if (r.Intent == RequestedDeckBasicIntent.WenyiPeach)
            return _coreWenyiPeachSuppliesOperations.WenyiRequestedDeckPeachTargets(view, card).Length == 1;
        if (r.Intent == RequestedDeckBasicIntent.Dying)
        {
            var dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Single(d => d.Id == r.OwnerFrameId);
            return card.Kind == CardKind.Peach && _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(actor.Seat, dying.VictimSeat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, dying.VictimSeat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)) || card.Kind == CardKind.Alcohol && actor.Seat == dying.VictimSeat && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Alcohol, CardActionType.Use);
        }

        var required = RequestedDeckBasicKind(view.OriginalDecision, r.Intent);
        if (required == CardKind.Slash ? !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) : card.Kind != required)
            return false;
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, card.Kind, _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(r.Intent)))
            return false;
        if (required == CardKind.Dodge && !_coreRecordedSuitSlashOperations.IsRecordedSuitResponseAllowed(actor, [card], required))
            return false;
        if (r.Intent is RequestedDeckBasicIntent.BorrowedSword or RequestedDeckBasicIntent.Qinglong or RequestedDeckBasicIntent.ProgramSlash or RequestedDeckBasicIntent.ProgramNearestSlash or RequestedDeckBasicIntent.AssistedSlash or RequestedDeckBasicIntent.NearestLegalSlash)
            return RequestedDeckBasicTargets(view, card).Length > 0;
        if (r.Intent == RequestedDeckBasicIntent.FactionSlash && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } faction)
            return _coreTurnRedSlashBenefitsOperations.IsRedSlashProviderPaymentLegal(faction, card.Kind, [card]);
        return true;
    }

    internal IReadOnlyList<PromptChoice> RequestedDeckBasicChoices(RequestedDeckBasicFrame view)
    {
        var list = new List<PromptChoice>();
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["deck-basic-source"] = branch,
            ["deck-frame-id"] = view.Id.ToString(CultureInfo.InvariantCulture)
        };
        Dictionary<string, string> CardParameters()
        {
            var parameters = Parameters("card");
            if (_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicActionType(view.Receipt.Intent) == CardActionType.Response)
            {
                var required = RequestedDeckBasicKind(view.OriginalDecision, view.Receipt.Intent);
                parameters["response-card-kind"] = required.ToString();
                parameters["response"] = view.Receipt.Intent switch
                {
                    RequestedDeckBasicIntent.FactionDodge => "faction-defense-dodge",
                    RequestedDeckBasicIntent.FactionSlash => "faction-slash-slash",
                    _ => required == CardKind.Dodge ? "dodge" : "slash"
                };
            }

            return parameters;
        }

        foreach (var id in view.CardIds)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == id);
            if (!RequestedDeckBasicCardMatches(view, card))
                continue;
            var targets = RequestedDeckBasicTargets(view, card);
            if (targets.Length == 0)
                list.Add(new(new($"deck-basic.{(view.Id)}.{(id)}"), $"使用或打出【{(card.DisplayName)}】", [id], [], CardParameters()));
            else
                foreach (var target in targets)
                    list.Add(new(new($"deck-basic.{(view.Id)}.{(id)}.{(target)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用【{(card.DisplayName)}】", [id], [target], CardParameters()));
        }

        list.Add(new(new($"deck-basic.{(view.Id)}.decline"), "不使用或打出顶牌", [], [], Parameters("decline")));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishRequestedDeckBasicView(RequestedDeckBasicFrame view)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(view.Receipt.Source.SkillId);
        var choices = RequestedDeckBasicChoices(view);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, view.Receipt.ActorSeat, $"{(skill.Name)}：观看牌堆顶{(view.CardIds.Count)}张牌，选择符合本次需求的基本牌。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), view.OriginalDecision.SourceSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = view.Receipt.Intent == RequestedDeckBasicIntent.Dying && _coreMatchParticipantsPlayersAccess._players[view.Receipt.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanDying : _coreMatchParticipantsPlayersAccess._players[view.Receipt.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void DispatchRequestedDeckBasicMaterial(RequestedDeckBasicIntent intent, Card card, IReadOnlyList<int> targets)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat];
        switch (intent)
        {
            case RequestedDeckBasicIntent.WenyiPeach:
                _coreWenyiPeachSuppliesOperations.ExecuteWenyiRequestedDeckPeach(card, targets);
                break;
            case RequestedDeckBasicIntent.Dodge:
                var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!;
                _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
                _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreNativeDodgeOperations.ResolveDodgeResponse(attack, actor, card);
                break;
            case RequestedDeckBasicIntent.Group:
                var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!;
                _coreNativeResponseWindowsOperations.PopResponseWindow(group.ResolutionId);
                _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, actor, card);
                break;
            case RequestedDeckBasicIntent.Duel:
                var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel!;
                _coreNativeResponseWindowsOperations.PopResponseWindow(duel.ResolutionId);
                _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreNativeDuelOperations.ResolveDuelResponse(duel, actor, card);
                break;
            case RequestedDeckBasicIntent.Dying:
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreNativeDyingOperations.ApplyDyingResponse(actor, card.Kind == CardKind.Peach, card.Kind == CardKind.Peach ? card.Id : null, card.Kind == CardKind.Alcohol, card.Kind == CardKind.Alcohol ? card.Id : null);
                break;
            case RequestedDeckBasicIntent.FactionDodge:
                _coreNativeFactionDefenseOperations.ResolveFactionDefenseCandidateResponse(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense!, true, false, card.Id);
                break;
            case RequestedDeckBasicIntent.FactionSlash:
                _coreNativeFactionSlashOperations.ResolveFactionSlashCandidateResponse(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!, true, card.Id, card.Kind);
                break;
            case RequestedDeckBasicIntent.BorrowedSword:
                _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordSlashChoice(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, card, card.Kind);
                break;
            case RequestedDeckBasicIntent.Qinglong:
                _coreNativeQinglongOperations.ResolveQinglongCrescentBladeChoice(new(new("deck-basic.qinglong"), "使用杀", [card.Id], targets, new Dictionary<string, string> { ["action"] = "qinglong-slash", ["response-card-kind"] = card.Kind.ToString() }));
                break;
            default:
                DispatchRequestedDeckProgramSlash(intent, card, targets);
                break;
        }
    }

    internal void DispatchRequestedDeckProgramSlash(RequestedDeckBasicIntent intent, Card card, IReadOnlyList<int> targets)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A deck basic program request lost its original owner.");
        if (_characterMaTieAnniversaryOperations.TryDispatchAnniversaryMaTieDeckSlash(f, card, targets))
            return;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(f) && _characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var sayingActor, out var sayingAllowed) && targets is [var sayingTarget] && sayingAllowed.Contains(sayingTarget))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(f, sayingActor, sayingActor, sayingTarget, card.Kind, [card], null);
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[sayingActor], _coreMatchParticipantsPlayersAccess._players[sayingTarget], card, card.Kind, sayingActor, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: f.Id);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(f) && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(f, out var actor, out var allowed) && targets is [var target] && allowed.Contains(target))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(f, actor, actor, target, card.Kind, [card], null);
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], card, card.Kind, actor, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: f.Id);
            return;
        }

        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(f))
        {
            _characterLvKuangLvXiangOlOperations.DispatchQigongRequestedDeckSlash(f, card, targets);
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(f))
        {
            _characterCaoChunOlOperations.DispatchCaoChunRequestedDeckSlash(f, card, targets);
            return;
        }

        var e = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        var p = new Dictionary<string, string>
        {
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture)
        };
        if (intent == RequestedDeckBasicIntent.ProgramSlash)
        {
            p["program-action"] = "request-slash";
            p["result-bind"] = e.ResultBind!;
            _coreProgramSlashRequestOperations.ResolveProgramRequestSlashChoice(new(new("deck-basic.request"), "使用杀", [card.Id], targets, p));
        }
        else if (intent == RequestedDeckBasicIntent.ProgramNearestSlash)
        {
            p["program-action"] = "request-slash-nearest";
            p["seat"] = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat.ToString(CultureInfo.InvariantCulture);
            p["target-seat"] = targets.Single().ToString(CultureInfo.InvariantCulture);
            _coreProgramSlashRequestOperations.ResolveProgramRequestSlashByNearestChoice(new(new("deck-basic.nearest"), "使用杀", [card.Id], targets, p));
        }
        else
        {
            var choices = intent == RequestedDeckBasicIntent.AssistedSlash ? _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashChoices(f) : _coreNearestLegalSlashRequestsOperations.NearestLegalSlashChoices(f);
            var selected = choices.Single(c => c.Cards.SequenceEqual(new[] { card.Id }) && c.Targets.SequenceEqual(targets) && !c.Parameters.ContainsKey("conversion-skill-id") && c.Parameters.GetValueOrDefault("effective-kind") == card.Kind.ToString());
            if (intent == RequestedDeckBasicIntent.AssistedSlash)
                _coreAssistedPhysicalCardsOperations.ResolveAssistedPhysicalSlashChoice(selected);
            else
                _coreNearestLegalSlashRequestsOperations.ResolveNearestLegalSlashChoice(selected);
        }
    }

    internal IEnumerable<Card> SelectedRequestedDeckBasicCards(CharacterState actor) => _resolutionStackOperations._resolutionStack.Where(f => f.RequestedDeckBasicMaterial is { Claiming: true, SelectedCardId: not null } r && r.OwnerFrameId == f.Id && r.ActorSeat == actor.Seat).Where(_coreRequestedDeckBasicOwnershipOperations.HasValidRequestedDeckBasicMaterial).Select(f => f.RequestedDeckBasicMaterial!).Where(r => _cardZonesOperations._cardZones.GetLocation(r.SelectedCardId!.Value) == CardLocation.DrawPile).Select(r => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == r.SelectedCardId && c.Kind == r.SelectedKind));
    internal IReadOnlyList<Card> AppendRequestedDeckBasicCards(CharacterState actor, IReadOnlyList<Card> original)
    {
        var selected = SelectedRequestedDeckBasicCards(actor).ToArray();
        return selected.Length == 0 ? original : original.Concat(selected).DistinctBy(c => c.Id).ToArray();
    }

    internal bool IsSelectedRequestedDeckBasicMaterial(int actor, int id) => SelectedRequestedDeckBasicCards(_coreMatchParticipantsPlayersAccess._players[actor]).Any(c => c.Id == id);
    internal CardSnapshot[] RequestedDeckBasicPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<RequestedDeckBasicFrame>().Where(f => f.Receipt.ActorSeat == viewer).SelectMany(f => f.CardIds).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == id))).ToArray();
    internal bool TryAdvanceRequestedDeckBasicAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat].IsHuman)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is RequestedDeckBasicFrame view)
        {
            var selected = RequestedDeckBasicChoices(view).FirstOrDefault(c => c.Parameters["deck-basic-source"] == "card") ?? RequestedDeckBasicChoices(view).Last();
            ResolveRequestedDeckBasicAnswer(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        var activate = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("deck-basic-source") == "activate");
        if (activate is null)
        {
            if (prompt.Kind != DecisionKind.RescueDying || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != dying.Id || dying.ResponderSeat != prompt.PlayerSeat || !(HasRequestedDeckBasicSource(_coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat], CardKind.Peach) || dying.RequestedDeckBasicMaterial is { Intent: RequestedDeckBasicIntent.Dying, Claiming: false } attempted && attempted.ActorSeat == prompt.PlayerSeat && attempted.Cursor == dying.ResponderIndex && attempted.OriginalPromptId == prompt.PromptId))
                return false;
            // The original request remains declined on its Dying owner. Consume
            // the mature native response while skipping this opt-in entry once.
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeDyingOperations.RunOneDyingStep(skipRequestedDeckBasic: true);
            return true;
        }

        // Activation does not inspect hidden cards. Once activated, only the viewer sees legal matching choices.
        ResolveRequestedDeckBasicAnswer(activate);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal void CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to)
    {
        if (from != CardLocation.DrawPile || to != CardLocation.Processing)
            return;
        var owner = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.RequestedDeckBasicMaterial is { Claiming: true } r && r.SelectedCardId == card.Id);
        if (owner?.RequestedDeckBasicMaterial is not { } receipt)
            return;
        if (receipt.SelectedKind != card.Kind || receipt.OwnerFrameId != owner.Id)
            throw new InvalidOperationException("A top basic payment changed its exact original material.");
        if (receipt.PaidMovementSequence is not null)
            throw new InvalidOperationException("A top basic request cannot pay twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { RequestedDeckBasicMaterial = receipt with { PaidMovementSequence = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RequestedDeckBasicPaidEvent(owner.Id, receipt.ActorSeat, card.Id, card.Kind, receipt.Source.SkillId, receipt.Intent));
    }
}
