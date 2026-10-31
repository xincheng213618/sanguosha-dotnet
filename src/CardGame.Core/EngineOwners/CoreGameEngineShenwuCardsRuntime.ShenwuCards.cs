#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShenwuCardsRuntime
{
    internal const string ShenwuTransplantSkill = "shenwu:transplant-wood-resolution";
    internal static string ShenwuTransplantGrant(long id) => RuntimeSkills.RuntimeRules_Core_ShenwuCards.ShenwuTransplantGrant(id);
    internal string ShenwuTransplantSource(long id) => $"turn:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)}:card-use:shenwu-transplant:{(id)}";
    internal void AddShenwuTrickActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards)
    {
        foreach (var card in playableCards.Where(card => card.Kind is CardKind.SowBeans or CardKind.TransplantWood))
        {
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, card.Kind, CardActionType.Use))
                continue;
            if (card.Kind == CardKind.SowBeans)
            {
                if (!_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, card.Kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, card.Kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))))
                    actions.Add(new(LegalActionKind.SowBeans, card.Id, null, "使用【撒豆成兵】"));
                continue;
            }

            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count != 0 && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, card.Kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, card.Kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))))
                actions.Add(new(LegalActionKind.TransplantWood, card.Id, target.Seat, $"对 {(target.Name)} 使用【移花接木】"));
        }
    }

    internal void ResolveShenwuSowBeans(CharacterState source, Card card)
    {
        RequireShenwuTrickAction(source, card, LegalActionKind.SowBeans, null);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [source.Seat]);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, source.Seat, [source.Seat], LegalActionKind.SowBeans);
    }

    internal void ResolveShenwuTransplantWood(CharacterState source, CharacterState target, Card card)
    {
        RequireShenwuTrickAction(source, card, LegalActionKind.TransplantWood, target.Seat);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [target.Seat]);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, source.Seat, [target.Seat], LegalActionKind.TransplantWood);
    }

    internal void RequireShenwuTrickAction(CharacterState source, Card card, LegalActionKind kind, int? target)
    {
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: card.Id).Any(action => action.Kind == kind && action.CardId == card.Id && action.TargetSeat == target))
            throw new InvalidOperationException("A Shenwu trick lost its legal original entity and target.");
    }

    internal void ResolveShenwuSowBeansEffect(NullificationWindowFrame pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("Sow beans lost its native owner.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var seat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? use.SourceSeat;
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (target.IsAlive)
        {
            var limit = Math.Min(5, target.MaxHp);
            var count = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(target) == "god" ? limit : Math.Max(0, limit - _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count);
            _corePhysicalDrawFacadesOperations.DrawCards(target, count, log: true);
        }

        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CardKind.SowBeans);
    }

    internal void ResolveShenwuTransplantWoodEffect(NullificationWindowFrame pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("Transplant wood lost its native owner.");
        var actor = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Transplant wood lost its exact target.");
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || actor == use.SourceSeat || use.ShenwuTransplant is not null || use.Action is null)
            throw new InvalidOperationException("Transplant wood changed its native target or issuance.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        if (!_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[actor]).Count == 0)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CardKind.TransplantWood);
            return;
        }

        var program = _contentRegistryOperations._contentRegistry.GetSkill(ShenwuTransplantSkill).Program ?? throw new InvalidOperationException("Transplant wood requires its registered request program.");
        var grant = ShenwuTransplantGrant(use.Id);
        _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].SkillGrants.Grant(new(grant, ShenwuTransplantSkill, grant, ShenwuTransplantSource(use.Id), TurnExpiry: new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)));
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShenwuTransplant = new(frameId, use.Action.ActionId, use.TargetIndex, actor) });
        var frame = new ProgramSkillFrame(frameId, use.SourceSeat, ShenwuTransplantSkill, "card-effect", program.GameplayHash, 0, [], Array.AsReadOnly(new[] { actor }))
        {
            SkillInstanceId = grant,
            ShenwuTransplantReturn = new(use.Id, use.Action.ActionId, use.TargetIndex, actor)
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    // This internal activation cannot be voluntarily invoked, even if a foreign
    // skill-copy effect were to expose its definition or a checkpoint is forged.
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 27800, SkillProgramEffectOp.RequestSlashAgainstChosenTarget)]
    internal bool AllowsShenwuTransplantActivation(ProgramActivationLegalityContext gate) => gate.SkillId != ShenwuTransplantSkill;
    internal bool IsShenwuTransplantProgram(ProgramSkillFrame frame)
    {
        if (frame.ShenwuTransplantReturn is not { } receipt || frame.SkillId != ShenwuTransplantSkill || frame.TriggerId is not null || frame.ActivationId != "card-effect" || frame.SelectedCardIds.Count != 0 || !frame.SelectedTargetSeats.SequenceEqual(new[] { receipt.ActorSeat }) || frame.SkillInstanceId != ShenwuTransplantGrant(receipt.CardUseFrameId) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.CardUseFrameId)is not { Step: ResolutionFrameStep.ResolvingEffect, ShenwuTransplant: { } state, Action: { } action } use || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.TransplantWood || state.ProgramFrameId != frame.Id || state.ActionId != action.ActionId || state.ActionId != receipt.ActionId || state.TargetIndex != receipt.TargetIndex || use.TargetIndex != receipt.TargetIndex || state.ActorSeat != receipt.ActorSeat || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != receipt.ActorSeat || frame.OwnerSeat != use.SourceSeat || frame.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(ShenwuTransplantSkill).Program!.GameplayHash)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == use.Id && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Any(grant => grant.GrantId == frame.SkillInstanceId && grant.SkillId == frame.SkillId && grant.SkillInstanceId == frame.SkillInstanceId && grant.SourceId == ShenwuTransplantSource(receipt.CardUseFrameId) && grant.TurnExpiry == new SkillGrantTurnExpiry(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
    }

    internal bool TryCompleteShenwuTransplantProgram(ProgramSkillFrame frame, bool completed)
    {
        if (frame.ShenwuTransplantReturn is not { } receipt)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !IsShenwuTransplantProgram(frame))
            throw new InvalidOperationException("Transplant wood lost its exact issued program return.");
        var usedSlash = frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == "slash-result")?.OptionId == "used-slash";
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, completed));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.RemoveGrant(frame.SkillInstanceId);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.CardUseFrameId)!;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShenwuTransplant = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenwuTransplantResolvedEvent(use.Id, use.SourceSeat, receipt.ActorSeat, usedSlash, completed));
        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true), CardKind.TransplantWood);
        return true;
    }

    internal bool ShenwuTrickFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => ride is ProgramSkillFrame { ShenwuTransplantReturn: { } receipt } frame && receipt.CardUseFrameId == beneath.Id && IsShenwuTransplantProgram(frame);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 27800)]
    internal bool TryAdvanceShenwuTransplantAi()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AssistedSlashRequest: { } request } frame || !IsShenwuTransplantProgram(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != request.ActorSeat || _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[request.ActorSeat];
        bool Enemy(int seat) => actor.TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[seat].TeamId is { } other && team != other;
        PromptChoice selected;
        if (request.TargetSeat is null)
            selected = decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("request-option") == "target").OrderBy(choice => Enemy(choice.Targets.Single()) ? 0 : 1).ThenBy(choice => _coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()].Hp).ThenBy(choice => choice.Targets.Single()).First();
        else if (actor.TeamId is not null && !Enemy(request.TargetSeat.Value))
            selected = decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline");
        else
            selected = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("request-option") != "decline") ?? decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline");
        _coreProgramLifecycleOperations.ProgramInteractions.Resolve(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
