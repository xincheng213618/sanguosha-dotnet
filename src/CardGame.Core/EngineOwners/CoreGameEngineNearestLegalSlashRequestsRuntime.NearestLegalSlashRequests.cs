#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNearestLegalSlashRequestsRuntime
{
    internal SkillProgramStepOutcome RequestLegalNearestSlashes(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        // An assisted faction child leaves exactly one result on this same
        // frame. Consume it before advancing the already committed actor cursor.
        if (active.NearestLegalSlashRequest is { AwaitingFaction: true } draft)
        {
            var answer = active.ChoiceBindings.SingleOrDefault(binding => binding.Name == draft.ResultBind) ?? throw new InvalidOperationException("A nearest faction child returned without its typed result.");
            if (answer.OptionId is not ("declined" or "used-slash"))
                throw new InvalidOperationException("A nearest faction child returned an unknown result.");
            if (!IsNearestFactionChoiceResult(active, answer))
                throw new InvalidOperationException("A nearest faction result lost its committed producer.");
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { NearestLegalSlashRequest = null, ChoiceBindings = Array.AsReadOnly(active.ChoiceBindings.Where(binding => binding.Name != draft.ResultBind).ToArray()) });
            active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            if (answer.OptionId == "declined" && _coreMatchParticipantsPlayersAccess._players[draft.ActorSeat].IsAlive)
            {
                // The executor already consumed the previous reexecute flag
                // before dispatching this returned faction result. Re-arm only
                // the next participant: the stored cursor already paid this one.
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { ReexecuteParticipantInstruction = true });
                return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, draft.ActorSeat, effect.Amount);
            }
        }

        return _coreParticipantAndReserveProgramsOperations.ExecuteParticipantReserve(effect, active);
    }

    internal SkillProgramStepOutcome RequestLegalSlashByNearestActor(ProgramSkillFrame frame, int actorSeat, int hpAmount, bool trickEffectReturned = false)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.NearestLegalSlashRequest is not null || active.AssistedSlashRequest is not null || actorSeat == active.OwnerSeat && !_coreWenheChaosOperations.IsExactWenheChaos(active))
            throw new InvalidOperationException("A nearest legal Slash requires one clean other actor.");
        if ((!_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId)) && !_coreWenheChaosOperations.IsExactWenheChaos(active))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "技能来源已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { SelectedTargetSeats = Array.AsReadOnly(new[] { actorSeat }) });
        if (!trickEffectReturned && _characterLuXunMouMobileOperations.TryBeginMouLuXunWenheTarget(active, actorSeat, hpAmount))
            return SkillProgramStepOutcome.AwaitChild;
        var nearest = _coreProgramSlashRequestOperations.GetNearestLivingCharacterSeats(actorSeat);
        active = active with
        {
            SelectedTargetSeats = Array.AsReadOnly(new[] { actorSeat }),
            NearestLegalSlashRequest = new(actorSeat, Array.AsReadOnly(nearest), $"nearest-{(active.InstructionIndex)}-{(actorSeat)}")
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        var choices = NearestLegalSlashChoices(active);
        if (choices.All(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline") && !_coreChainedStateBasicConversionsOperations.HasChainedStateBasicProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_coreRoundOnceCardlessProgramRequestsOperations.HasRoundOnceCardlessProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_characterMaXiuMaTieOlOperations.HasKenshangProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_characterChengJiChengCuiOlOperations.HasTousuiProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_characterGuoTuOlOperations.HasWeijieProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) && !(_characterLiuZhangOlOperations.HasZonghuProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest) || _characterHuanShuJunOlOperations.HasHuanpeiProgramSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], nearest)))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { NearestLegalSlashRequest = null });
            return _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, actorSeat, hpAmount);
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, actorSeat, "对距离最小的另一名角色使用一张【杀】，或失去1点体力。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), nearest, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actorSeat,
            SkillPrompt = new(active.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[actorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> NearestLegalSlashChoices(ProgramSkillFrame frame)
    {
        var draft = frame.NearestLegalSlashRequest ?? throw new InvalidOperationException("Nearest Slash draft disappeared.");
        if (draft.AwaitingFaction)
            return[];
        var choices = new List<PromptChoice>();
        foreach (var target in draft.NearestSeats)
        {
            // Pure payment enumeration reuses the existing conversion, Zhangba,
            // range, restriction and faction rules. This view is never stored.
            var view = frame with
            {
                AssistedSlashRequest = new(draft.ActorSeat, target, ActorChoosesTarget: true)
            };
            foreach (var choice in NearestPhysicalSlashChoices(view).Where(choice => choice.Parameters["request-option"] != "decline"))
            {
                var parameters = choice.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
                parameters["program-action"] = "nearest-legal-slash";
                choices.Add(choice with { Id = new ChoiceId($"nearest.{(frame.Id)}.{(draft.ActorSeat)}.{(target)}.{(choice.Id.Value)}"), Description = choice.Description + $" → {(_coreMatchParticipantsPlayersAccess._players[target].Name)}", Parameters = parameters });
            }
        }

        choices.AddRange(_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramSlashChoices(frame, draft.ActorSeat, draft.NearestSeats).Concat(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(frame, draft.ActorSeat, draft.NearestSeats)).Concat(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(frame, draft.ActorSeat, draft.NearestSeats)));
        choices.Add(new(new ChoiceId($"nearest.{(frame.Id)}.{(draft.ActorSeat)}.decline"), "失去1点体力。", [], [], new Dictionary<string, string> { ["program-action"] = "nearest-legal-slash", ["request-option"] = "decline", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        return choices;
    }

    internal IReadOnlyList<PromptChoice> NearestPhysicalSlashChoices(ProgramSkillFrame view)
    {
        var draft = view.AssistedSlashRequest!;
        var actor = _coreMatchParticipantsPlayersAccess._players[draft.ActorSeat];
        var target = draft.TargetSeat!.Value;
        bool ExactSingleSourceLegal(PromptChoice choice)
        {
            if (choice.Parameters["request-option"] != "use" || choice.Cards.Count != 1)
                return true;
            var card = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(actor)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileSlashUseCards(actor)).Single(item => item.Id == choice.Cards[0]);
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var conversion);
            var kind = Enum.Parse<CardKind>(choice.Parameters["effective-kind"]);
            return NearestSlashPaymentHasRange(actor, target, [card], conversion, kind) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], card, conversion, kind) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && !_coreWuJingQueriesOperations.IsTurnSlashChoiceUseForbidden(actor.Seat, choice);
        }

        var choices = _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashChoices(view).Where(ExactSingleSourceLegal).ToList();
        // Historical assisted consumers perform a native range precheck before
        // creating variants. Only this new opt-in path adds variants whose exact
        // identity/view-as source supplies its own distance exemption.
        foreach (var card in _coreActionQueriesOperations.GetSlashUseCards(actor))
        {
            var identity = _coreActionQueriesOperations.GetProgramCardIdentityMatches(actor, card).FirstOrDefault(item => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(item.Identity.OutputKind));
            var baseKind = identity?.Identity.OutputKind ?? _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(actor, card);
            foreach (var kind in _coreNativeTargetLegalityOperations.GetSlashUseKinds(actor, baseKind))
            {
                var parameters = new Dictionary<string, string>
                {
                    ["program-action"] = "assisted-physical-slash",
                    ["frame-id"] = view.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["request-option"] = "use",
                    ["effective-kind"] = kind.ToString()
                };
                choices.AddRange(_coreCardConversionsOperations.CreateConversionChoiceVariants(actor, card, kind, false, $"assisted-slash.{(view.Id)}.card-{(card.Id)}.{(kind)}", $"使用【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [target], parameters).Where(ExactSingleSourceLegal));
            }
        }

        return Array.AsReadOnly(choices.DistinctBy(choice => choice.Id).ToArray());
    }

    internal bool IsNearestFactionChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding)
    {
        if (frame.NearestLegalSlashRequest is not { AwaitingFaction: true, TargetSeat: { } target } draft || binding.Name != draft.ResultBind)
            return false;
        if (!IsNearestLegalFactionOrigin(frame, draft.ActorSeat, target, binding.Name) || binding.ChooserSeat != draft.ActorSeat || binding.OptionId is not ("declined" or "used-slash") || !_coreCommandJournalStorageEventsPendingEventsAccess._events.Select(item => item.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramOptionChosenEvent>().Any(fact => fact.FrameId == frame.Id && fact.SkillId == frame.SkillId && fact.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) && fact.OwnerSeat == frame.OwnerSeat && fact.ResultBind == binding.Name && fact.ChooserSeat == binding.ChooserSeat && fact.OptionId == binding.OptionId))
            throw new InvalidOperationException("A nearest faction result changed its exact owning instruction, actor or committed fact.");
        return true;
    }

    internal void ResolveNearestLegalSlashChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Nearest legal Slash parent disappeared.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var draft = frame.NearestLegalSlashRequest ?? throw new InvalidOperationException("Nearest legal Slash draft disappeared.");
        if (effect.Op != SkillProgramEffectOp.RequestLegalSlashByNearest || draft.AwaitingFaction || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.ActorSeat || !draft.NearestSeats.SequenceEqual(_coreProgramSlashRequestOperations.GetNearestLivingCharacterSeats(draft.ActorSeat)))
            throw new InvalidOperationException("Nearest legal Slash lost its exact instruction, actor or distance frontier.");
        var canonical = NearestLegalSlashChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("Nearest legal Slash payment or target is no longer available.");
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [selected]))
            throw new InvalidOperationException("Nearest legal Slash answer changed its frozen payload.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.ActorSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "受令角色或技能来源已失效。");
            return;
        }

        if (canonical.Parameters["request-option"] == "decline")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NearestLegalSlashRequest = null });
            if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, draft.ActorSeat, effect.Amount) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var target = canonical.Targets.Single();
        if (canonical.Parameters["request-option"] == "faction")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NearestLegalSlashRequest = draft with { TargetSeat = target, AwaitingFaction = true } });
            _coreAssistedFactionSlashOperations.BeginAssistedProgramFactionSlashRequest(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), draft.ActorSeat, target, draft.ResultBind);
            return;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[draft.ActorSeat];
        var(cards, kind, conversion) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, canonical);
        if (!draft.NearestSeats.Contains(target) || !NearestSlashPaymentHasRange(actor, target, cards, conversion, kind) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], cards[0], conversion, kind, noEffectiveRank: cards.Count > 1, specificEffectiveRank: canonical.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString() ? _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards) : null, physicalCardIds: cards.Select(c => c.Id).ToArray()))
            throw new InvalidOperationException("Nearest legal Slash target or material became illegal.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NearestLegalSlashRequest = null });
        _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[target], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, usesZhuqueFan: kind == CardKind.FireSlash && (cards[0].Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(actor, cards[0])) && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), conversionSource: conversion, programSkillCardUseFrameId: frame.Id);
    }

    internal bool NearestSlashPaymentHasRange(CharacterState actor, int target, IReadOnlyList<Card> cards, CardConversionSource? conversion, CardKind kind) => _coreAssistedPhysicalCardsOperations.AssistedSlashPaymentHasRange(actor, target, cards, conversion) || (_coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, cards.Select(c => c.Id).ToArray()) || _coreGrantedEntityPhasesOperations.HasGrantedPhaseEntityDistance(actor, cards.Select(c => c.Id).ToArray())) || _coreCardConversionsOperations.IgnoresProgramSlashDistance(actor, conversion) || _coreCardConversionsOperations.IgnoresSpGuanYuWushengDistance(actor, cards[0]) || _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(actor, kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, cards[0])) || _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashPolicy(actor.Seat, kind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, cards[0])) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, cards[0]) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, _coreMatchParticipantsPlayersAccess._players[target], kind) || _coreCardConversionsOperations.IgnoresSlashUseDistance(actor);
    internal bool IsNearestLegalFactionOrigin(ProgramSkillFrame frame, int actor, int target, string? bind) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.Op == SkillProgramEffectOp.RequestLegalSlashByNearest && frame.NearestLegalSlashRequest is { AwaitingFaction: true } draft && draft.ActorSeat == actor && draft.TargetSeat == target && draft.ResultBind == bind && draft.NearestSeats.Contains(target) && frame.SelectedTargetSeats.SequenceEqual([actor]) && actor != frame.OwnerSeat;
    internal bool ValidateNearestLegalFactionParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame)
    {
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.RequestLegalSlashByNearest)
            return false;
        var answer = frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == pending.AssistedResultBind);
        if (pending.TargetSeat is not { } target || !IsNearestLegalFactionOrigin(frame, pending.OwnerSeat, target, pending.AssistedResultBind) || pending.PolicySource is null || (pending.ActiveAttack is null ? answer is not null : answer?.OptionId != "used-slash"))
            throw new InvalidOperationException("Nearest faction child changed its frozen parent, actor, target or paid result.");
        return true;
    }

    internal int[] UnlimitedVirtualSlashTargets(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players.Where(target => _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && target.IsAlive && target.Seat != frame.OwnerSeat && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], target, ignoresCount: true, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target)).Select(target => target.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> UnlimitedVirtualSlashChoices(ProgramSkillFrame frame)
    {
        Dictionary<string, string> Parameters(string option) => new()
        {
            ["program-action"] = "unlimited-virtual-slash",
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["offer-option"] = option
        };
        return UnlimitedVirtualSlashTargets(frame).Select(target => new PromptChoice(new ChoiceId($"unlimited-slash.{(frame.Id)}.{(target)}"), $"视为对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用【杀】", [], [target], Parameters("use"))).Append(new(new ChoiceId($"unlimited-slash.{(frame.Id)}.decline"), "不使用【杀】。", [], [], Parameters("decline"))).ToArray();
    }

    internal SkillProgramStepOutcome OfferUnlimitedVirtualSlash(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.TriggerId is not null || active.NearestLegalSlashRequest is not null || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != active.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play)
            throw new InvalidOperationException("An unlimited Slash offer requires a completed current active sequence.");
        // The participant selection belonged to the previous instruction.
        // This optional use selects its own target only when answered.
        active = active with
        {
            SelectedTargetSeats = Array.AsReadOnly(Array.Empty<int>())
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        if (UnlimitedVirtualSlashTargets(active).Length == 0)
            return SkillProgramStepOutcome.Continue;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, "你可以视为使用一张无距离限制的【杀】。", [], UnlimitedVirtualSlashTargets(active), active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = Array.AsReadOnly(UnlimitedVirtualSlashChoices(active).ToArray()),
            SkillPrompt = new(active.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveUnlimitedVirtualSlashChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Unlimited Slash parent disappeared.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var canonical = UnlimitedVirtualSlashChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("Unlimited Slash target became unavailable.");
        if (effect.Op != SkillProgramEffectOp.OfferUnlimitedVirtualSlash || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], [canonical]))
            throw new InvalidOperationException("Unlimited Slash answer lost its exact frame or target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "虚拟杀的技能来源已失效。");
            return;
        }

        var used = canonical.Parameters["offer-option"] == "use";
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramUnlimitedSlashChoiceEvent(frame.Id, frame.SkillId, frame.SkillInstanceId, frame.OwnerSeat, used ? canonical.Targets.Single() : null, used));
        if (!used)
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = Array.AsReadOnly(canonical.Targets.ToArray()) });
        if (_coreProgramTurnPhaseOperations.BeginProgramVirtualCardUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), canonical.Targets.Single(), CardKind.Slash, ignoreDistance: true) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestLegalSlashByNearest)]
    internal PromptChoice SelectAiNearestLegalSlash(PendingDecision decision) => decision.Choices.Select(choice => (Choice: choice, Value: choice.Parameters.GetValueOrDefault("request-option") == "decline" ? -22d : _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), choice.Targets.Single(), new(0, 0, 0, 0, 0, 1, false, false)))).OrderByDescending(item => item.Value).ThenBy(item => item.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    [ProgramAiChoice(SkillProgramEffectOp.OfferUnlimitedVirtualSlash)]
    internal PromptChoice SelectAiUnlimitedSlash(PendingDecision decision) => decision.Choices.Select(choice => (Choice: choice, Value: choice.Parameters["offer-option"] == "decline" ? 0d : _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), choice.Targets.Single(), new(0, 0, 0, 0, 0, 1, false, false)))).OrderByDescending(item => item.Value).ThenBy(item => item.Choice.Id.Value, StringComparer.Ordinal).First().Choice;

    SkillProgramStepOutcome global::CardGame.Core.INearestLegalSlashProgramHost.RequestLegalNearestSlashes(SkillProgramEffect effect, ProgramSkillFrame frame) => RequestLegalNearestSlashes(effect, frame);
    SkillProgramStepOutcome global::CardGame.Core.INearestLegalSlashProgramHost.OfferUnlimitedVirtualSlash(ProgramSkillFrame frame) => OfferUnlimitedVirtualSlash(frame);
}
