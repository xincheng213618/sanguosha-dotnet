#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeActionDispatchRuntime
{
    internal void ExecuteAction(CharacterState actor, LegalAction action, IReadOnlyList<int>? activeSkillCardIds = null, IReadOnlyList<int>? activeSkillTargetSeats = null)
    {
        _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = _coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, action) ? action.TargetSeats : null;
        _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._selectedRedAdditionalTargetSource = action.ProgramActivationId == "red-additional-targets" && _coreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess._redAdditionalTargetGrants.TryGetValue(actor.Seat, out var redGrant) ? redGrant.Source : null;
        if (_coreHuxinMirrorOperations.TryPlaceHandHuxinMirror(actor, action))
            return;
        _coreCardConversionsOperations.SelectUseConversion(action);
        if (_characterLuBuMobileGodOperations.TryResolveMobileWumouUse(actor, action))
            return;
        if (action.Kind == LegalActionKind.UseProgramSkill)
        {
            if (_coreProgramActionModulesOperations.ProgramActions.TryExecute(actor, action, activeSkillCardIds ?? [], activeSkillTargetSeats ?? []))
                return;
            _coreSkillProgramsOperations.ExecuteProgramSkill(actor, action, activeSkillCardIds ?? [], activeSkillTargetSeats ?? []);
            return;
        }

        if (action.Kind == LegalActionKind.RevealGeneral)
        {
            _coreNationalGeneralRevealOperations.RevealNationalGeneral(actor, action.GeneralSlot ?? throw new InvalidOperationException("A general reveal action must identify its slot."));
            return;
        }

        if (action.Kind == LegalActionKind.UseEquipmentEffect)
        {
            var cardIds = activeSkillCardIds?.Distinct().ToArray() ?? [];
            var targets = activeSkillTargetSeats?.Distinct().ToArray() ?? [];
            if (cardIds.Length < action.MinCardCount || cardIds.Length > action.MaxCardCount || targets.Length < action.MinTargetCount || targets.Length > action.MaxTargetCount || cardIds.Any(cardId => !action.SelectableCardIds.Contains(cardId)) || targets.Any(target => !action.SelectableTargetSeats.Contains(target)))
            {
                throw new InvalidOperationException("The equipment-effect selection is invalid.");
            }

            if (!CoreGameEngineZhangbaSelectionsRuntime.MatchesPublishedCardGroupTarget(action, cardIds, targets))
                throw new InvalidOperationException("The equipment-effect materials have no published compatible target.");
            if (action.EquipmentKind == CardKind.WoodenOx)
            {
                _coreNativeWoodenOxOperations.ResolveWoodenOx(actor, cardIds[0], targets.SingleOrDefault(-1));
            }
            else
            {
                var cards = cardIds.Select(cardId => _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Single(card => card.Id == cardId)).ToArray();
                if (targets.Length > 1)
                    _coreNextActualUseTargetAdjustmentsOperations.ResolveNextActualUseZhangba(actor, targets, cards);
                else
                    _coreNativeSlashUseOperations.ResolveZhangbaSlash(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], cards);
            }

            return;
        }

        if (_coreChainedStateBasicConversionsOperations.TryExecuteChainedStateBasicPlay(actor, action))
            return;
        if (_coreDrawFundedDistinctBasicOffersOperations.TryExecuteDrawFundedDistinctBasicPlay(actor, action))
            return;
        if (_coreRoundOnceCardlessNativeUsesOperations.TryExecuteRoundOnceCardlessPlay(actor, action))
            return;
        if (_coreTieredRoundZeroUsesOperations.TryExecuteTieredRoundZeroPlay(actor, action))
            return;
        var card = _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, action.CardId) ?? throw new InvalidOperationException("The chosen card is no longer in the actor's playable zones.");
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(actor, card, action.PlayedCardKind ?? card.Kind, action.ConversionSource, _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Use, actor.Seat, action), action.TargetSeats.Count > 0 ? action.TargetSeats : action.TargetSeat is { } declaredTarget ? [declaredTarget] : []))
            return;
        if (_characterGuanYuWumiaoOperations.TryExecuteWumiaoFloodNaturalUse(actor, card, action))
            return;
        if (_characterXunYuMobileGodOperations.TryExecuteQizhengNaturalUse(actor, card, action))
            return;
        if (_characterGuanYuWumiaoOperations.TryExecuteWuyouIdentityUse(actor, card, action))
            return;
        if (_coreSingleCardTrickConversionsOperations.TryExecuteSingleCardTrickConversion(actor, card, action))
            return;
        card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, action.ConversionSource);
        switch (action.Kind)
        {
            case LegalActionKind.Recast:
                if (action.ConversionSource is { } recastSource)
                    _coreActualTurnViewAsOperations.ConsumeActualTurnLimitedViewAs(recastSource);
                _coreCardActionsOperations.ResolveRecast(actor, card, action.PlayedCardKind);
                break;
            case LegalActionKind.Slash:
                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException("Slash requires a target.");
                }

                if (action.TargetSeats.Count > 1)
                {
                    _coreNativeSlashUseOperations.ResolveFangtianHalberdSlash(actor, action.TargetSeats.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).ToArray(), card, action.PlayedCardKind ?? card.Kind, action.ConversionSource, action.AdditionalConversionSources);
                }
                else
                {
                    _coreNativeSlashUseOperations.ResolveSlash(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.PlayedCardKind ?? card.Kind, action.ConversionSource, action.AdditionalConversionSources);
                }

                break;
            case LegalActionKind.Peach:
                _coreNativeRecoveryCardsOperations.ResolvePeach2(actor, actor, card, allowDying: false, conversionSource: action.ConversionSource);
                break;
            case LegalActionKind.Duel:
                if (action.TargetSeats.Count > 1)
                {
                    _coreProgramMultiTargetDuelsOperations.ResolveProgramMultiTargetDuel(actor, card, action);
                    break;
                }

                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException("Duel requires a target.");
                }

                _coreNativeDuelOperations.ResolveDuel(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.PlayedCardKind);
                break;
            case LegalActionKind.DrawTwo:
                _coreNativeTrickUseOperations.ResolveDrawTwo(actor, card);
                break;
            case LegalActionKind.AllianceFeast:
                _coreHulaoCardsOperations.ResolveAllianceFeast(actor, card);
                break;
            case LegalActionKind.SowBeans:
                _coreShenwuCardsOperations.ResolveShenwuSowBeans(actor, card);
                break;
            case LegalActionKind.ReturnSoul:
                _coreHellSoulCardsOperations.ResolveHellReturnSoul(actor, card);
                break;
            case LegalActionKind.TransplantWood:
                _coreShenwuCardsOperations.ResolveShenwuTransplantWood(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat!.Value], card);
                break;
            case LegalActionKind.MantianGuohai:
                _coreMantianGuohaiOperations.ResolveMantianGuohai(actor, card, action);
                break;
            case LegalActionKind.Intoxication:
            case LegalActionKind.StoreGrain:
            case LegalActionKind.Reinforcements:
            case LegalActionKind.UnitedEffort:
                _coreGuanduCardsOperations.ResolveGuanduCard(actor, card, action);
                break;
            case LegalActionKind.Zong:
            case LegalActionKind.RealgarWine:
            case LegalActionKind.SameBoat:
            case LegalActionKind.StriveUpstream:
            case LegalActionKind.AgainstCurrent:
                _coreDragonBoatCardsOperations.ResolveDragonBoatCard(actor, card, action);
                break;
            case LegalActionKind.StarShift:
            case LegalActionKind.BeamSwap:
            case LegalActionKind.HandRedistribution:
            case LegalActionKind.WenheChaos:
                _coreWenheCardsOperations.ResolveWenheCard(actor, card, action.PlayedCardKind ?? card.Kind, action.TargetSeat);
                break;
            case LegalActionKind.Alcohol:
                _coreNativeTrickUseOperations.ResolveAlcohol(actor, card, action.ConversionSource);
                break;
            case LegalActionKind.Equip:
                _coreNativeEquipmentUseOperations.ResolveEquip(actor, card);
                break;
            case LegalActionKind.BarbarianAssault:
                _coreNativeTrickUseOperations.ResolveGroupCard(actor, card, LegalActionKind.BarbarianAssault, CardKind.Slash);
                break;
            case LegalActionKind.ArrowBarrage:
                _coreNativeTrickUseOperations.ResolveGroupCard(actor, card, LegalActionKind.ArrowBarrage, CardKind.Dodge);
                break;
            case LegalActionKind.PeachGarden:
                _coreNativeTrickUseOperations.ResolvePeachGarden(actor, card);
                break;
            case LegalActionKind.FiveGrains:
                _coreNativeTrickUseOperations.ResolveFiveGrains(actor, card);
                break;
            case LegalActionKind.IronChain:
                if (action.TargetSeats.Count < 1 || action.TargetSeats.Count > _coreProgramIronChainTargetsOperations.GetProgramIronChainTargetLimit(actor) + (_coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, action) ? _coreRedAdditionalTargetsOperations.GetAdditionalTargetAdjustmentLimit(actor, action) : 0))
                {
                    throw new InvalidOperationException("IronChain requires an exact target list within its current limit.");
                }

                _coreNativeTrickUseOperations.ResolveIronChain(actor, card, action.TargetSeats, action.PlayedCardKind);
                break;
            case LegalActionKind.Indulgence:
            case LegalActionKind.SupplyShortage:
            case LegalActionKind.Lightning:
                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException($"{(action.Kind)} requires a target.");
                }

                _coreNativeTrickUseOperations.ResolveDelayedCard(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.Kind, action.PlayedCardKind);
                break;
            case LegalActionKind.Dismantlement:
                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException("Dismantlement requires a target.");
                }

                _coreNativeTrickUseOperations.ResolveDismantlement(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.TargetCardId, action.PlayedCardKind, action.TargetHandPreselection);
                break;
            case LegalActionKind.Snatch:
                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException("Snatch requires a target.");
                }

                _coreNativeTrickUseOperations.ResolveSnatch(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.TargetCardId, action.PlayedCardKind, action.TargetHandPreselection);
                break;
            case LegalActionKind.UnexpectedAssault:
                if (action.TargetSeat is null)
                    throw new InvalidOperationException("UnexpectedAssault requires a target.");
                _coreUnexpectedAssaultOperations.ResolveUnexpectedAssault(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.PlayedCardKind);
                break;
            case LegalActionKind.FireAttack:
                if (action.TargetSeat is null)
                {
                    throw new InvalidOperationException("FireAttack requires a target.");
                }

                _coreNativeTrickUseOperations.ResolveFireAttack(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeat.Value], card, action.PlayedCardKind);
                break;
            case LegalActionKind.BorrowedSword:
                if (action.TargetSeats.Count is not (2 or 4) && !(action.TargetSeats.Count == 6 && action.ProgramActivationId == "red-additional-targets" && _coreRedAdditionalTargetsOperations.HasRedAdditionalTargets(actor) && _coreRedAdditionalTargetsOperations.GetAdditionalTargetAdjustmentLimit(actor, action) == 2))
                {
                    throw new InvalidOperationException("Borrowed Sword requires an ordered weapon owner and Slash target.");
                }

                _coreNativeTrickUseOperations.ResolveBorrowedSword(actor, _coreMatchParticipantsPlayersAccess._players[action.TargetSeats[0]], _coreMatchParticipantsPlayersAccess._players[action.TargetSeats[1]], card);
                break;
            default:
                throw new InvalidOperationException($"Unsupported action {(action.Kind)}.");
        }
    }
}
