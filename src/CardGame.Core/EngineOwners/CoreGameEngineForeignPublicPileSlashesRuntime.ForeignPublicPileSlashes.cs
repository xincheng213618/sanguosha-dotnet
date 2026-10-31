#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineForeignPublicPileSlashesRuntime
{
    internal int GetForeignPublicPileSlashBaseDamage(long resolutionId) => _coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.ForeignPublicPileSlashBaseDamage ?? throw new InvalidOperationException("The public-pile Slash lost its initial damage amount.");
    internal bool IsForeignPublicPileSlashUse(long resolutionId) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == resolutionId && frame.CardId == 0 && frame.Action is { } action && action.PhysicalCards.Count == 2 && action.PhysicalCards.All(cost => cost.From.Zone == CardZoneKind.Authority) && action.PhysicalCards.Select(cost => cost.From.OwnerSeat).Distinct().Count() == 1);
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 10)]
    internal IEnumerable<LegalAction> BuildForeignPublicPileSlashActions(CharacterState actor)
    {
        if (!actor.IsAlive || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play)
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat))
        {
            var pile = _cardZonesOperations._cardZones.CardsAt(CardLocation.Authority(owner.Seat));
            if (pile.Count < 2 || !_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, owner) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, owner.Seat, CardKind.Slash))
                continue;
            foreach (var binding in _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.ForeignPublicPileSlash))
            {
                var baseAction = new LegalAction(LegalActionKind.UseProgramSkill, null, owner.Seat, $"移去 {(owner.Name)} 的两张公开牌堆牌，视为对其使用【杀】", CardKind.Slash, MinCardCount: 2, MaxCardCount: 2, MinTargetCount: 1, MaxTargetCount: 1)
                {
                    ProgramSkillId = binding.Source.SkillId,
                    ProgramActivationId = binding.Policy.Id,
                    ProgramSkillOwnerSeat = owner.Seat,
                    SelectableCardIds = pile.Select(card => card.Id).ToArray(),
                    SelectableTargetSeats = [owner.Seat],
                    ProgramAiHint = new(0, 0, 1, 0, 0, 0, true, false)
                };
                yield return baseAction;
                if (_coreRedAdditionalTargetsOperations.HasNextCardTargetAdjustment(actor))
                    foreach (var extra in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != actor.Seat && player.Seat != owner.Seat && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(player) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, player.Seat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(player, CardKind.Slash, Suit.None)))
                        yield return baseAction with
                        {
                            Description = baseAction.Description + $"（下一张牌增加目标：{(extra.Name)}）",
                            TargetSeats = new[]
                            {
                                owner.Seat,
                                extra.Seat
                            },
                            SelectableTargetSeats = new[]
                            {
                                owner.Seat,
                                extra.Seat
                            },
                            MinTargetCount = 2,
                            MaxTargetCount = 2
                        };
            }
        }
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 20)]
    internal bool TrySubmitForeignPublicPileSlash(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (command.SkillOwnerSeat is not { } ownerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.ForeignPublicPileSlash).Any(binding => binding.Source.SkillId == command.SkillId && binding.Policy.Id == command.ActivationId))
            return false;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
        {
            result = _kernelCoreMainOperations2.Reject(error.Code, error.Message);
            return true;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = BuildForeignPublicPileSlashActions(actor).SingleOrDefault(item => item.ProgramSkillOwnerSeat == ownerSeat && item.ProgramSkillId == command.SkillId && item.ProgramActivationId == command.ActivationId && item.TargetSeats.SequenceEqual(command.TargetSeats));
        if (action is null)
        {
            result = _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "The public pile Slash is currently unavailable.");
            return true;
        }

        error = _coreSkillProgramsOperations.ValidateProgramSelection(action, command.CardIds, command.TargetSeats);
        if (error is not null)
        {
            result = _kernelCoreMainOperations2.Reject(error.Code, error.Message);
            return true;
        }

        result = _kernelCoreMainOperations2.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            TryExecuteForeignPublicPileSlash(actor, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 20)]
    internal bool TryExecuteForeignPublicPileSlash(CharacterState actor, LegalAction selected, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (selected.ProgramSkillOwnerSeat is not { } ownerSeat || !_coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.ForeignPublicPileSlash).Any(binding => binding.Source.SkillId == selected.ProgramSkillId && binding.Policy.Id == selected.ProgramActivationId))
            return false;
        var action = BuildForeignPublicPileSlashActions(actor).SingleOrDefault(item => item.ProgramSkillOwnerSeat == ownerSeat && item.ProgramSkillId == selected.ProgramSkillId && item.ProgramActivationId == selected.ProgramActivationId && item.TargetSeats.SequenceEqual(targets)) ?? throw new InvalidOperationException("The public pile Slash became unavailable.");
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, cards, targets)is { } error)
            throw new InvalidOperationException(error.Message);
        var from = CardLocation.Authority(ownerSeat);
        var payments = cards.Select(id => _cardZonesOperations._cardZones.CardsAt(from).Single(card => card.Id == id)).ToArray();
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramCardPolicyKind.ForeignPublicPileSlash).Single(item => item.Source.SkillId == selected.ProgramSkillId && item.Policy.Id == selected.ProgramActivationId);
        var conversion = new CardConversionSource(binding.Source.SkillId, binding.Policy.Id, ownerSeat, binding.Source.SkillInstanceId);
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var context = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, targets, payments.Select(card => new CardActionCost(card.Id, card.Kind, from, _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, card))).ToArray(), [conversion], effectiveSuit: Suit.None, effectiveRank: 0));
        _corePhysicalMovementFacadesOperations.MoveCards(payments, from, CardLocation.DiscardPile, new CardMoveReason("program.public-pile.slash-payment"));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, actor.Seat, 0, CardKind.Slash, targets, PhysicalCardIds: []) { Action = context });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(context));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, targets));
        if (!(_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.UnlimitedUse == true))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(resolutionId, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(resolutionId, frame => frame with { ForeignPublicPileSlashBaseDamage = 1 + actor.AlcoholDamageBonus });
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, actor.Seat, ownerSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[ownerSeat], CardKind.Slash));
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(resolutionId, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, ownerSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(resolutionId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, context, SkillProgramTriggerWindow.CardUseCommitted, context.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        return true;
    }
}
