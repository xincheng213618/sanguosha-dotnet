#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal IReadOnlyList<PromptChoice> MouXqJueyanResponseChoices(CharacterState actor, CardKind kind, bool dying = false)
    {
        if (!actor.IsAlive || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return[];
        if (dying)
        {
            if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } d || d.ResponderSeat != actor.Seat || !_coreMatchParticipantsPlayersAccess._players[d.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[d.VictimSeat].Hp > 0 || kind == CardKind.Peach && !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(actor.Seat, d.VictimSeat) || kind == CardKind.Alcohol && (d.VictimSeat != actor.Seat || _coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, d.VictimSeat, Suit.None))
                return[];
        }
        else if (kind == CardKind.Dodge)
        {
            if (!_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, kind) || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } slash && (_characterLiuQiOlOperations.IsWenjiResponseForbidden2(slash.ResolutionId, actor.Seat) || _coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(slash)is not null))
                return[];
        }
        else if (kind == CardKind.Nullification)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } n || n.CandidateIndex >= n.CandidateSeats.Count || n.CandidateSeats[n.CandidateIndex] != actor.Seat || n.ChainDepth == 0 && _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(n.SourceSeat, actor.Seat, n.EffectCardKind, n.TargetSeats))
                return[];
        }
        else
            return[];
        var source = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(actor, SkillProgramTriggerWindow.PublicHandRevealed).FirstOrDefault(c => c.SkillId == MouXiaoQiaoOlRuntime.Jueyan);
        if (source is null)
            return[];
        var conversion = new CardConversionSource(source.SkillId, "retained-hand-use", actor.Seat, source.SkillInstanceId);
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(card => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, card) == kind && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && MouXiaoQiaoOl.JueyanAvailable(actor.Seat, card.Id)).Select(card =>
        {
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "mou-xiao-qiao-jueyan",
                ["output-kind"] = kind.ToString(),
                ["dying-use"] = dying ? "true" : "false"
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, conversion);
            return new PromptChoice(new($"jueyan.response.{(actor.Seat)}.{(card.Id)}.{(kind)}.{(source.SkillInstanceId)}"), $"绝颜：展示并保留【{(card.DisplayName)}】，视为使用之。", [card.Id], [], parameters);
        }).ToArray());
    }

    internal bool BeginMouXqJueyanResponse(PromptChoice selected)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || selected.Cards is not [var id] || !Enum.TryParse<CardKind>(selected.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("Jueyan needs its actual published Hand response choice.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var dying = selected.Parameters.GetValueOrDefault("dying-use") == "true";
        if (!MouXqJueyanResponseChoices(actor, kind, dying).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || prompt.Kind != (dying ? DecisionKind.RescueDying : kind == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.Nullification))
            throw new InvalidOperationException("Jueyan cannot replace pure Played or another native responder.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        var parent = dying ? _coreDyingCursorRuntimeOperations.ActiveDying!.Id : kind == CardKind.Nullification ? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!.Id : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!.ResolutionId;
        var target = dying ? _coreDyingCursorRuntimeOperations.ActiveDying!.VictimSeat : kind == CardKind.Nullification ? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!.SourceSeat : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!.SourceSeat;
        var request = new MouXiaoQiaoJueyanResponseRequest(parent, prompt.Kind, kind, actor.Seat, target, _resolutionStackOperations._resolutionStack[^1].Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, actor.Seat, source.SkillId, source.BindingId, program.GameplayHash, 0, [id], [])
        {
            SkillInstanceId = source.SkillInstanceId!,
            ResponseDecision = prompt,
            MouXiaoQiaoJueyanResponse = request
        };
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, actor.Seat, f.SkillId, f.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool IsMouXqJueyanResponseRequest(ProgramSkillFrame f)
    {
        if (f.MouXiaoQiaoJueyanResponse is not { } r || f.ResponseDecision is not { } d || r.ActorSeat != f.OwnerSeat || d.PlayerSeat != f.OwnerSeat || d.Kind != r.DecisionKind || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.OriginalResponseFrameId)is not { } parent || _resolutionStackOperations._resolutionStack.IndexOf(parent) + 1 != _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id))
            return false;
        return r.DecisionKind switch
        {
            DecisionKind.RespondDodge => parent is ResponseWindowFrame w && w.ParentFrameId == r.ParentFrameId && w.ResponderSeat == f.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(r.ParentFrameId)is { CardAttack: { } attack } && attack.TargetSeat == f.OwnerSeat && attack.SourceSeat == r.TargetSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash),
            DecisionKind.Nullification => parent is NullificationWindowFrame n && n.Id == r.ParentFrameId && n.SourceSeat == r.TargetSeat && n.CandidateIndex < n.CandidateSeats.Count && n.CandidateSeats[n.CandidateIndex] == f.OwnerSeat,
            DecisionKind.RescueDying => parent is DyingFrame dying && dying.Id == r.ParentFrameId && dying.VictimSeat == r.TargetSeat && dying.ResponderSeat == f.OwnerSeat,
            _ => IsMouXqForcedRequest(r, parent)};
    }

    internal void ReturnMouXqJueyanResponse(ProgramSkillFrame f)
    {
        if (!IsMouXqJueyanResponseRequest(f) || f.MouXiaoQiaoOl is not { Paid: true } r || f.MouXiaoQiaoJueyanResponse is not { } request || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanUsedEvent>().Any(e => e.FrameId == f.Id && e.CardId == r.SelectedCard && e.OutputKind == request.OutputKind))
            throw new InvalidOperationException("Jueyan lost its actual public retained-Hand response invoice.");
        var grant = new MouXiaoQiaoJueyanResponseGrant(f.Id, r.Source, f.GameplayHash, r.Round, r.SelectedCard, request);
        var parent = _resolutionStackOperations._resolutionStack.Single(x => x.Id == request.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouXiaoQiaoJueyanResponseGrant = grant });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiaoQiaoJueyanResponseGrantedEvent(grant));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var kind = request.OutputKind;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        if (!actor.IsAlive)
        {
            CancelMouXqResponseAfterReveal(request, actor);
            return;
        }

        if (request.DecisionKind is DecisionKind.RespondSlash or DecisionKind.QinglongCrescentBlade or DecisionKind.ProgramTrigger)
        {
            IssueMouXqRequestedSlash(grant);
            return;
        }

        if (request.DecisionKind == DecisionKind.RescueDying)
        {
            var d = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Jueyan rescue lost its original unresolved Dying.");
            _coreNativeDyingFramesOperations.SetDyingFrameStep(d.Id, ResolutionFrameStep.ResolvingEffect);
            var victim = _coreMatchParticipantsPlayersAccess._players[d.VictimSeat];
            var policies = kind == CardKind.Peach && actor.Seat != victim.Seat ? _coreProgramCardPoliciesOperations.CardPolicies(victim, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(p => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(actor), p.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, victim, new(0, kind, Suit.None, 0), kind == CardKind.Peach ? "桃" : "酒", kind, 1 + policies.Sum(p => p.Policy.Value), recoveryPolicySources: policies.Select(p => (p.Source.SkillId, p.Policy.Id)).ToArray(), conversionSource: grant.Source, dyingResponse: new(d.Id, actor.Seat, kind == CardKind.Peach, kind == CardKind.Peach ? 0 : null, kind == CardKind.Alcohol, kind == CardKind.Alcohol ? 0 : null), physicalCards: []);
        }
        else if (kind == CardKind.Dodge)
            IssueMouXqDodge(actor, grant);
        else
            IssueMouXqCounterspell(actor, grant);
    }

    internal void CancelMouXqResponseAfterReveal(MouXiaoQiaoJueyanResponseRequest request, CharacterState actor)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (request.DecisionKind == DecisionKind.RescueDying && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.Id == request.ParentFrameId && dying.ResponderSeat == actor.Seat)
        {
            _coreNativeDyingOperations.ApplyDyingResponse(actor, false, null, false, null);
            return;
        }

        if (request.DecisionKind == DecisionKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counter && counter.Id == request.ParentFrameId)
        {
            _coreNativeNullificationOperations.ResolveNullificationChoice(counter, actor, null);
            return;
        }

        if (request.DecisionKind == DecisionKind.RespondDodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == request.ParentFrameId)
        {
            _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (request.DecisionKind == DecisionKind.RespondSlash && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { } borrowed && borrowed.ResolutionId == request.ParentFrameId)
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(borrowed, transferWeapon: false);
            return;
        }

        if (request.DecisionKind == DecisionKind.QinglongCrescentBlade && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } blade && blade.Attack.ResolutionId == request.ParentFrameId)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(blade.Attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(blade.Attack.ResolutionId, actor.Seat, request.TargetSeat, false, [], null));
            _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(blade.Attack.ResolutionId);
            _coreNativeAttackCompletionOperations.CompleteAttack(blade.Attack);
            return;
        }

        if (request.DecisionKind == DecisionKind.ProgramTrigger && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.Id == request.ParentFrameId)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "The accepted retained-card Slash responder died during the public reveal.");
            return;
        }

        throw new InvalidOperationException("Jueyan lost its exact unavailable responder's native return.");
    }

    internal void SignMouXqResponse(ResolutionFrame owner, MouXiaoQiaoJueyanResponseGrant grant, CardActionContext action)
    {
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { MouXiaoQiaoJueyanResponseGrant = grant with { ActionId = action.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiaoQiaoJueyanResponseIssuedEvent(owner.Id, grant.ProgramFrameId, action.ActionId, action.ActorSeat, action.EffectiveKind));
    }

    internal void IssueMouXqDodge(CharacterState actor, MouXiaoQiaoJueyanResponseGrant grant)
    {
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Jueyan Dodge lost its actual Slash.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, use.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [grant.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        SignMouXqResponse(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, grant, action);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Dodge);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, attack.SourceSeat, CardKind.Dodge));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(attack, action, ProgramCardContinuation.Dodge))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.Dodge))
            return;
        if (_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(action, ProgramCardContinuation.Dodge) || _coreResponseCompletionOperations.HasCardResponseCompletedObserver(action, ProgramCardContinuation.Dodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.Dodge);
        else
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal void IssueMouXqCounterspell(CharacterState actor, MouXiaoQiaoJueyanResponseGrant grant)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("Jueyan counterspell lost its actual trick response window.");
        var policy = _coreUnrespondableCounterspellsOperations.FreezeUnrespondableCounterspellSource(actor, []);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == pending.Id);
        pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { EffectNullified = !pending.EffectNullified, ChainDepth = pending.ChainDepth + 1, CandidateSeats = _coreNativeNullificationOperations.BuildNullificationCandidateSeats(actor.Seat), CandidateIndex = 0, Step = ResolutionFrameStep.AwaitingResponse });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _coreDelayedNullificationOperations.NullificationParentActionId(pending), CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, pending.SourceSeat, CardKind.Nullification, [], [], [grant.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        SignMouXqResponse(pending, grant, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, pending.SourceSeat, CardKind.Nullification));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRespondedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, actor.Seat, 0, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        _characterCaoXiOlOperations.CaptureCaoXiNativeCounterspellOffset(pending, action, false);
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(x => x.Id == pending.Id);
        if (_coreUnrespondableCounterspellsOperations.TryBeginPolicyCounterspellPayment(pending, action, policy, []))
            return;
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(null, action, ProgramCardContinuation.NullificationResponse))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseAccepted, [], ProgramCardContinuation.NullificationResponse))
            return;
        _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(action);
    }

    internal bool IsIssuedMouXqResponse(ResolutionFrame owner, CardActionContext action)
    {
        if (owner.MouXiaoQiaoJueyanResponseGrant is not { } g || g.Request.ParentFrameId != owner.Id || g.ActionId != action.ActionId || action.Type != CardActionType.Response || action.ActorSeat != g.Source.OwnerSeat || action.ProviderSeat != action.ActorSeat || action.RequesterSeat is not null || action.ResponderSeat != action.ActorSeat || action.PhysicalCards.Count != 0 || action.EffectiveKind != g.Request.OutputKind || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || action.EffectiveIsRed is not null || !action.ConversionChain.SequenceEqual([g.Source]))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanResponseGrantedEvent>().Count(e => e.Grant == (g with { ActionId = 0 })) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanResponseIssuedEvent>().Count(e => e == new MouXiaoQiaoJueyanResponseIssuedEvent(owner.Id, g.ProgramFrameId, action.ActionId, action.ActorSeat, action.EffectiveKind)) == 1 && (owner switch
        {
            NullificationWindowFrame n => action.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(n) && action.OpponentSeat == n.SourceSeat,
            CardUseFrame { CardAttack: { } attack } use => action.ParentActionId == use.Action?.ActionId && action.ActorSeat == attack.TargetSeat && action.OpponentSeat == attack.SourceSeat,
            _ => false
        });
    }
    internal bool HasMouXqJueyanDyingResponse(CharacterState actor) => MouXqJueyanResponseChoices(actor, CardKind.Peach, true).Count > 0 || MouXqJueyanResponseChoices(actor, CardKind.Alcohol, true).Count > 0;
    internal bool TryMouXqJueyanAiResponse(CardKind kind, bool dying = false)
    {
        if (dying ? _coreDyingCursorRuntimeOperations.ActiveDying is null : _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[dying ? _coreDyingCursorRuntimeOperations.ActiveDying!.ResponderSeat : _corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat];
        if (actor.IsHuman)
            return false;
        var choices = MouXqJueyanResponseChoices(actor, kind, dying);
        if (choices.Count == 0)
            return false;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat);
        bool use;
        AiThoughtRecord thought;
        if (dying)
        {
            (use, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseProgramDyingRescue(view, _coreDyingCursorRuntimeOperations.ActiveDying!.VictimSeat, "绝颜", ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        }
        else if (kind == CardKind.Nullification)
        {
            var n = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!;
            (use, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseTieredZeroCounterspell(view, n.EffectCardKind, n.SourceSeat, n.TargetSeats, n.EffectNullified, n.ChainDepth, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        }
        else
        {
            var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!;
            (use, _, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseDodgeResponse(view, attack.SourceSeat, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, true);
        }

        _coreNativeAiDecisionsOperations.AddThought(thought);
        if (!use)
            return false;
        if (dying)
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.RescueDying, actor.Seat, "绝颜：选择真实已发布的手牌展示救援。", choices.SelectMany(c => c.Cards).ToArray(), [], _coreDyingCursorRuntimeOperations.ActiveDying!.VictimSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                TargetSeat = _coreDyingCursorRuntimeOperations.ActiveDying!.VictimSeat,
                Choices = choices
            };
        BeginMouXqJueyanResponse(choices[0]);
        return true;
    }
}
