#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSkillProgramsRuntime
{
    internal ProgramLegalityRouter ProgramLegality => _programLegalityOperations._programLegality ??= _nativeCardGameCoreProgramLegalityRouterOperations.CreateProgramLegalityRouter(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsDefinitions.ProgramLegalityMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.ResolveRuntimeHandlerTarget);

    internal const string SimpleProgramPindianResultBind = "__active-pindian-result";
    internal IReadOnlyList<SkillProgram> EnabledSkillPrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).Programs;
    internal IReadOnlyList<SkillProgram> EnabledActivationPrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).ActivationPrograms;
    internal IReadOnlyList<SkillProgram> EnabledContributionPrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).ContributionPrograms;
    internal IReadOnlyList<SkillProgram> EnabledViewAsPrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).ViewAsPrograms;
    internal IReadOnlyList<SkillProgram> EnabledCardIdentityPrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).CardIdentityPrograms;
    internal IReadOnlyList<SkillProgram> EnabledPassiveRulePrograms(CharacterState player) => _coreNationalSkillsOperations.GetSkillBindingShard(player).PassiveRulePrograms;
    internal IReadOnlyList<IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(CharacterState player, SkillProgramTriggerWindow window) => _coreNationalSkillsOperations.GetSkillBindingShard(player).GetUniqueTriggers(window);
    internal SkillProgram GetEnabledSkillProgram(CharacterState player, string skillId) => _coreNationalSkillsOperations.GetSkillBindingShard(player).GetProgram(skillId) ?? throw new InvalidOperationException($"Player {(player.Seat)} does not own enabled skill program '{(skillId)}'.");
    internal bool EvaluateProgramActivationCondition(SkillProgramCondition condition, PlayerSkillContext context, string skillId, string skillInstanceId) => condition.Kind switch
    {
        SkillProgramConditionKind.BooleanState => condition.Evaluate(context, static _ => throw new InvalidOperationException("A Pindian condition requires a running program frame."), stateId => _coreProgramPersistentStateOperations.GetProgramBooleanState2(context.Seat, skillId, skillInstanceId, stateId)),
        SkillProgramConditionKind.All => condition.Children.All(child => EvaluateProgramActivationCondition(child, context, skillId, skillInstanceId)),
        SkillProgramConditionKind.Any => condition.Children.Any(child => EvaluateProgramActivationCondition(child, context, skillId, skillInstanceId)),
        SkillProgramConditionKind.Not => !condition.MissingEquipmentPresence(context) && !EvaluateProgramActivationCondition(condition.Children[0], context, skillId, skillInstanceId),
        _ => condition.Evaluate(context)};
    internal IEnumerable<LegalAction> BuildProgramActions(CharacterState owner) => _coreHealthQueriesOperations.HappyQiaoActionQueryUnion(() => BuildProgramActionsCore(owner).ToArray());
    internal IEnumerable<LegalAction> BuildProgramActionsCore(CharacterState owner)
    {
        if (!owner.IsAlive || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play)
            yield break;
        PlayerSkillContext? ordinaryContext = null;
        PlayerSkillContext? handLimitContext = null;
        foreach (var program in EnabledActivationPrograms(owner))
            foreach (var activation in program.Activations)
            {
                if (activation.OtherCharactersOnly)
                    continue;
                var plan = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Activation, activation.Id);
                var features = plan.Features;
                // Usability enumerates native actions. Only conditions that read it
                // need that second action query; ordinary activations do not.
                var context = activation.Condition.RequiresHandLimitContext || features.RequiresHandLimitContext ? handLimitContext ??= _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner, includeHandLimit: true) : ordinaryContext ??= _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
                var legality = new ProgramActivationLegalityContext(owner, program, plan, context);
                var selection = new ProgramSelectionContext(owner.Seat, program, plan)
                {
                    PlayerContext = context
                };
                if (!ProgramLegality.CanActivate(ProgramActivationLegalityPhase.BeforeOwnerFacts, legality))
                    continue;
                var ownerLegality = _coreEquipmentSlotGroupProgramsOperations.CreateProgramLegalityParticipant(owner.Seat);
                if (!ProgramLegality.CanActivate(ProgramActivationLegalityPhase.BeforeCardCount, legality))
                    continue;
                var requiredCards = GetProgramActivationMinimumCards(owner.Seat, program.Id, activation);
                if (!ProgramLegality.CanActivate(ProgramActivationLegalityPhase.BeforeCondition, legality))
                    continue;
                if (!EvaluateProgramActivationCondition(activation.Condition, context, program.Id, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id)) || !global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime.CanPayProgramMarkerCost(owner, activation.MarkerCost) || activation.UsesPerTurn is { } limit && _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault((owner.Seat, program.Id, activation.UsageGroup)) >= limit || _coreProgramSelectionModulesOperations.ProgramSelections.PhaseLimit(selection)is { } phaseLimit && _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, program.Id, activation.UsageGroup)) >= phaseLimit || activation.UsesPerGame is { } gameLimit && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, program.Id, activation.UsageGroup, SkillUsageScope.Game) >= gameLimit)
                    continue;
                if (!features.Legality.CanStart(ownerLegality))
                    continue;
                if (!ProgramLegality.CanActivate(ProgramActivationLegalityPhase.AfterCommonStart, legality))
                    continue;
                if (features.ForOperation(SkillProgramEffectOp.SelectTargets).Any(effect => effect.TargetKind is { } kind && _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, kind).Count < effect.MinimumTargets))
                    continue;
                var candidates = new ProgramSelectionCandidates
                {
                    MinCardCount = requiredCards,
                    MaxTargetCount = activation.MaxTargets
                };
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.BeforeSources, selection, candidates))
                    continue;
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.TryBuildSource(selection, candidates))
                    candidates.Cards = activation.MaxCards == 0 ? [] : activation.SourceZones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, owner.Seat))).Where(card => CanSelectProgramActivationCard(plan, card, owner.Seat, program.Id)).Where(card => _coreProgramSelectionModulesOperations.ProgramSelections.AllowsCard(ProgramSelectionStage.CardAfterDeclaration, selection, card)).Where(card => activation.EquipmentSlots.Count == 0 || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && activation.EquipmentSlots.Contains(_coreFieldEquipmentOperations.FieldEquipmentSlot(card))).Select(card => card.Id).Distinct().Order().ToArray();
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.AfterCards, selection, candidates))
                    continue;
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.TryBuildSourceTargets(selection, candidates))
                    candidates.Targets = activation.MaxTargets == 0 ? [] : _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && _coreProgramSelectionModulesOperations.ProgramSelections.AllowsTarget(selection, target.Seat) && features.Legality.CanSelectTarget(ownerLegality, _coreEquipmentSlotGroupProgramsOperations.CreateProgramLegalityParticipant(target.Seat)) && (activation.TargetKind is SkillProgramTargetKind.AnyLiving or SkillProgramTargetKind.AnyWounded or SkillProgramTargetKind.AnyLivingHighestHp or SkillProgramTargetKind.AnyLivingHighestHand or SkillProgramTargetKind.AnyLivingLeastHandCount or SkillProgramTargetKind.AnyLivingMale || target.Seat != owner.Seat) && (activation.TargetKind switch
                    {
                        SkillProgramTargetKind.ExclusiveRendeBeneficiary or SkillProgramTargetKind.ExclusiveQingnangBeneficiary => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, activation.TargetKind).Contains(target.Seat),
                        SkillProgramTargetKind.OtherLiving => target.Seat != owner.Seat,
                        SkillProgramTargetKind.OtherLivingMale => target.Seat != owner.Seat && target.Gender == GeneralGender.Male,
                        SkillProgramTargetKind.OtherWoundedMale => target.Seat != owner.Seat && target.Gender == GeneralGender.Male && _coreTreatedWoundedOperations.IsRuleWounded(target),
                        SkillProgramTargetKind.OtherLivingInAttackRange => target.Seat != owner.Seat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner.Seat, target.Seat),
                        SkillProgramTargetKind.OtherLivingWhoseAttackRangeIncludesOwner => target.Seat != owner.Seat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(target.Seat, owner.Seat),
                        SkillProgramTargetKind.OtherLivingWithQinggangSword => target.Seat != owner.Seat && _coreAdvancedWeaponsOperations.HasWeaponAbility(target, CardKind.QinggangSword),
                        SkillProgramTargetKind.OtherLivingSlashable => _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(owner, target),
                        SkillProgramTargetKind.OtherLivingVirtualSlashTarget => !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Slash, CardActionType.Use) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(owner.Seat, SkillProgramCardTargetRestriction.SelfOnly) && _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, activation.TargetKind).Contains(target.Seat),
                        SkillProgramTargetKind.AnyLivingWithHand => _characterLuKangWumiaoOperations.WuLuKangActivationTargetHasPindianSource(plan, target),
                        SkillProgramTargetKind.OtherLivingWithHand => target.Seat != owner.Seat && _characterLuKangWumiaoOperations.WuLuKangActivationTargetHasPindianSource(plan, target),
                        SkillProgramTargetKind.OtherLivingWuFactionWithHand => target.Seat != owner.Seat && _characterLuKangWumiaoOperations.WuLuKangActivationTargetHasPindianSource(plan, target) && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(target) == "wu",
                        SkillProgramTargetKind.OtherLivingWithHandOrEquipment => target.Seat != owner.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count > 0,
                        SkillProgramTargetKind.OtherLivingWithDiscardableHandOrEquipment => target.Seat != owner.Seat && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(owner.Seat, target.Seat),
                        SkillProgramTargetKind.OtherLivingEmptyHand => target.Seat != owner.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0,
                        SkillProgramTargetKind.OtherLivingWithHandHpGreaterThanOwner => target.Seat != owner.Seat && _characterLuKangWumiaoOperations.WuLuKangActivationTargetHasPindianSource(plan, target) && target.HasHpValue && owner.HasHpValue && target.Hp > owner.Hp,
                        SkillProgramTargetKind.OtherLivingAtDistanceOne => target.Seat != owner.Seat && _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, target.Seat) == 1,
                        SkillProgramTargetKind.AnyLiving => true,
                        SkillProgramTargetKind.AnyLivingMale => target.Gender == GeneralGender.Male,
                        SkillProgramTargetKind.AnyLivingHighestHp or SkillProgramTargetKind.AnyLivingHighestHand or SkillProgramTargetKind.AnyLivingLeastHandCount or SkillProgramTargetKind.OtherLivingHighestHand => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, activation.TargetKind).Contains(target.Seat),
                        SkillProgramTargetKind.OtherWounded => target.Seat != owner.Seat && _coreTreatedWoundedOperations.IsRuleWounded(target),
                        SkillProgramTargetKind.AnyWounded => _coreTreatedWoundedOperations.IsRuleWounded(target),
                        SkillProgramTargetKind.AnyLivingHandBelowMaxHp => target.HasHpValue && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count < target.MaxHp,
                        SkillProgramTargetKind.OtherLivingPair => target.Seat != owner.Seat && _coreMatchParticipantsPlayersAccess._players.Count(peer => peer.IsAlive && peer.Seat != owner.Seat) >= 2,
                        SkillProgramTargetKind.EventTarget => false,
                        _ => false
                    }) && (!features.ProhibitsEquipmentReplacement || HasFreeEquipmentSlotForOwnedHandEquipment(owner, target))).Select(target => target.Seat).Order().ToArray();
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.AfterTargets, selection, candidates))
                    continue;
                var cards = candidates.Cards;
                var targets = candidates.Targets;
                var effectiveSameSuit = ProgramSelectionUsesEffectiveSameSuit(program, plan);
                if (cards.Length < requiredCards || targets.Length < activation.MinTargets || activation.SelectedCardsDistinctSuits && cards.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id).Suit).Distinct().Count() < activation.MinCards || activation.SelectedCardsSameSuit && !cards.Select(id => ProgramSameSuitInputSuit(owner, id, effectiveSameSuit)).GroupBy(suit => suit).Any(group => group.Count() >= activation.MinCards))
                    continue;
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.Availability, selection, candidates))
                    continue;
                if (features.FirstInstruction is { Op: SkillProgramEffectOp.SelectTarget, TargetKind: { } dynamicKind } dynamicSelection && !_coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, dynamicKind, marker: dynamicSelection.Marker).Any(seat => _coreProgramLifecycleOperations.IsProgramTargetEligible(owner.Seat, dynamicKind, dynamicSelection.Zones, seat, dynamicSelection.Marker)))
                    continue;
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.AfterAvailability, selection, candidates) || !_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.FinalTargets, selection, candidates))
                    continue;
                candidates.MaxCardCount = activation.CardCountExpression is not null ? requiredCards : activation.MaxCards == int.MaxValue ? candidates.Cards.Length : activation.MaxCards;
                var activationLabel = _contentRegistryOperations._contentRegistry!.Skills[program.Id].ProgramPresentation?.ActivationLabels.GetValueOrDefault(activation.Id);
                if (!_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.Relationships, selection, candidates) || !_coreProgramSelectionModulesOperations.ProgramSelections.Apply(ProgramSelectionStage.Projection, selection, candidates))
                    continue;
                yield return new LegalAction(LegalActionKind.UseProgramSkill, null, null, $"发动【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】" + (activationLabel is null ? string.Empty : $" · {(activationLabel)}"), MinCardCount: candidates.MinCardCount, MaxCardCount: candidates.MaxCardCount, MinTargetCount: activation.MinTargets, MaxTargetCount: candidates.MaxTargetCount)
                {
                    ProgramSkillId = program.Id,
                    ProgramActivationId = activation.Id,
                    SelectedCardsSameSuit = activation.SelectedCardsSameSuit,
                    SelectedCardsDistinctSuits = activation.SelectedCardsDistinctSuits,
                    SelectableCardSuitOverrides = effectiveSameSuit ? candidates.Cards.ToDictionary(id => id, id => ProgramSameSuitInputSuit(owner, id, useEffectiveSuit: true)) : null,
                    SelectedCardsSameNameOrAllEquipment = candidates.SelectedCardsSameNameOrAllEquipment,
                    ProgramAiHint = CreateProgramAiHint(program, activation, context, owner.IsFaceDown),
                    SelectableCardIds = Array.AsReadOnly(candidates.Cards),
                    SelectableTargetSeats = Array.AsReadOnly(candidates.Targets),
                    SelectableCardTargets = candidates.CardTargets,
                    SelectableTargetSequences = candidates.TargetSequences
                };
            }

        foreach (var exchange in _coreProgramActionModulesOperations.ProgramActions.Build(ProgramActionSourcePhase.ProgramTail, owner))
            yield return exchange;
        foreach (var skillOwner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).OrderBy(player => player.Seat))
            foreach (var program in EnabledContributionPrograms(skillOwner))
                foreach (var contribution in program.Contributions)
                {
                    var providerFaction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(owner);
                    var key = (owner.Seat, skillOwner.Seat, program.Id, contribution.Id);
                    if (providerFaction is null || !contribution.ProviderFactions.Contains(providerFaction, StringComparer.Ordinal) || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(skillOwner, program.Id, null, contribution.OwnerRole) || _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses.GetValueOrDefault(key) >= contribution.UsesPerPlayPhase)
                        continue;
                    var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => ProgramCardKindMatch.Contains(contribution.CardKinds, card.Kind) || contribution.CardSuits.Contains(card.Suit)).Select(card => card.Id).Order().ToArray();
                    if (cards.Length == 0)
                        continue;
                    yield return new LegalAction(LegalActionKind.UseProgramSkill, null, skillOwner.Seat, $"响应【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】，将一张牌交给 {(skillOwner.Name)}", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 1, MaxTargetCount: 1)
                    {
                        ProgramSkillId = program.Id,
                        ProgramActivationId = contribution.Id,
                        ProgramSkillOwnerSeat = skillOwner.Seat,
                        ProgramAiHint = new SkillProgramAiHint(0, 0, 0, 0, 0, 0, true, false),
                        SelectableCardIds = Array.AsReadOnly(cards),
                        SelectableTargetSeats = Array.AsReadOnly(new[] { skillOwner.Seat })
                    };
                }
    }

    internal bool HasFreeEquipmentSlotForOwnedHandEquipment(CharacterState owner, CharacterState target)
    {
        var occupiedSlots = _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Select(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item)).ToHashSet();
        return _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && !occupiedSlots.Contains(_coreFieldEquipmentOperations.FieldEquipmentSlot(card)));
    }

    internal int GetProgramActivationMinimumCards(int ownerSeat, string skillId, SkillProgramActivation activation) => _characterLuKangWumiaoOperations.WuLuKangCanOmitPindianSource(ownerSeat, activation) ? 0 : activation.CardCountExpression switch
    {
        null => activation.MinCards,
        SkillProgramCardCountExpression.NextPhaseActivationOrdinal => checked(_coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault((ownerSeat, skillId, activation.UsageGroup)) + 1),
        SkillProgramCardCountExpression.OwnerHpAtLeastOne => global::CardGame.Core.EngineOwners.CharacterZhaoYunOlGod2016Runtime.HpScaledCardCount(_coreMatchParticipantsPlayersAccess._players[ownerSeat]),
        _ => throw new InvalidOperationException("Unsupported activation card-count expression.")};
    internal bool CanSelectProgramActivationCard(ProgramExecutionPlan plan, Card card, int ownerSeat, string skillId)
    {
        var activation = plan.Activation!;
        var features = plan.Features;
        var selection = new ProgramSelectionContext(ownerSeat, _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!, plan);
        if (!_coreProgramSelectionModulesOperations.ProgramSelections.AllowsCard(ProgramSelectionStage.CardBeforeDiscard, selection, card))
            return false;
        if ((features.ForOperation(SkillProgramEffectOp.DiscardSelected).Any(effect => effect.Condition.Kind == SkillProgramConditionKind.Always) || features.ForOperation(SkillProgramEffectOp.ResolveMouXiahouShiYanyuDiscard).Any()) && _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(ownerSeat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard))
            return false;
        if (!_coreProgramSelectionModulesOperations.ProgramSelections.AllowsCard(ProgramSelectionStage.CardAfterDiscard, selection, card))
            return false;
        return (activation.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(activation.CardKinds, card.Kind)) && (activation.CardSuits.Count == 0 || activation.CardSuits.Contains(_coreProgramSelectionModulesOperations.ProgramSelections.CardSuit(selection, card))) && (activation.CardCategories.Count == 0 || activation.CardCategories.Any(category => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(card.Kind, category)));
    }

    internal SkillProgramAiHint CreateProgramAiHint(SkillProgram program, SkillProgramActivation activation, PlayerSkillContext context, bool faceDown)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[context.Seat];
        var instanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id);
        var features = ProgramInstructionResolver.Default.Features(activation);
        var hint = ProgramCompositionAi.Estimate(activation.Effects, context, faceDown, _coreProgramLifecycleOperations.CreateProgramAiPublicContext(owner)with { ActivationCardCount = GetProgramActivationMinimumCards(owner.Seat, program.Id, activation), EligibleTargetCount = features.First(SkillProgramEffectOp.SelectTargets)is { TargetKind: { } selectedKind } ? _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, selectedKind).Count : features.UsesArrowBarrageSelection ? _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive && player.Seat != owner.Seat) : null, PhaseUsageCount = usageId => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, program.Id, usageId, SkillUsageScope.Phase), BooleanState = stateId => _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, program.Id, instanceId, stateId) }).Hint;
        var actorPolicy = _coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(owner.Seat, out var brain) ? brain.PolicyVersion : _coreMatchDefinitionOptionsAccess._options.AiPolicyVersion;
        if (_coreWushuangModeProjectionOperations.IsWushuangMode && actorPolicy >= AiDecisionPolicy.WushuangRefinementVersion)
            hint = WushuangDeferredPhaseAi.Describe(activation, hint);
        return _coreProgramSelectionModulesOperations.ProgramSelections.DecorateAi(new(context.Seat, program, ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Activation, activation.Id)) { PlayerContext = context }, hint);
    }

    internal CommandError? ValidateProgramSelection(LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (cards.Count < action.MinCardCount || cards.Count > action.MaxCardCount || cards.Distinct().Count() != cards.Count || cards.Any(id => !action.SelectableCardIds.Contains(id)))
            return new CommandError(CommandErrorCode.InvalidCard, "The program's card selection is invalid.");
        if (targets.Count < action.MinTargetCount || targets.Count > action.MaxTargetCount || targets.Distinct().Count() != targets.Count || targets.Any(seat => !action.SelectableTargetSeats.Contains(seat)))
            return new CommandError(CommandErrorCode.InvalidTarget, "The program's target selection is invalid.");
        var program = action.ProgramSkillOwnerSeat is null && action.ProgramSkillId is { } skillId ? _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program : null;
        var plan = program is null ? null : ProgramInstructionResolver.Default.Find(program, ProgramInstructionSourceKind.Activation, action.ProgramActivationId);
        var activation = plan?.Activation;
        var effectiveSameSuit = program is not null && plan is not null && ProgramSelectionUsesEffectiveSameSuit(program, plan);
        if (action.SelectedCardsSameSuit && cards.Select(id => ProgramSameSuitInputSuit(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat], id, effectiveSameSuit)).Distinct().Skip(1).Any())
            return new CommandError(CommandErrorCode.InvalidCard, effectiveSameSuit ? "The selected physical cards must share one effective suit." : "The selected physical cards must share one printed suit.");
        ProgramSelectionContext? validationContext = program is not null && plan is not null ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program, plan) : null;
        if (validationContext is { } beforeSuitContext && _coreProgramSelectionValidationOperations.ProgramSelectionValidators.Validate(ProgramSelectionValidationStage.BeforeDistinctSuits, beforeSuitContext, action, cards, targets)is { } initialSelectionError)
            return initialSelectionError;
        if (activation is { SelectedCardsDistinctSuits: true } && cards.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id).Suit).Distinct().Count() != cards.Count)
            return new CommandError(CommandErrorCode.InvalidCard, "The selected cards must have distinct printed suits.");
        if (activation is not null && cards.Any(id => !CanSelectProgramActivationCard(plan!, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id), _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program!.Id)))
            return new CommandError(CommandErrorCode.InvalidCard, "The selected cards do not satisfy the activation filters.");
        if (validationContext is { } filteredContext && _coreProgramSelectionValidationOperations.ProgramSelectionValidators.Validate(ProgramSelectionValidationStage.AfterCardFilters, filteredContext, action, cards, targets)is { } filteredSelectionError)
            return filteredSelectionError;
        if (activation is { CategoryTargetLedgerId: { } ledgerId } && !_coreProgramCategoryTargetLedgerOperations.CanActivateCategoryTargetLedger(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program!.Id, ledgerId, cards, targets))
            return new CommandError(CommandErrorCode.IllegalAction, "This card category or target was already used in this phase.");
        if (activation is not null && _coreAppliedDamageBenefitsOperations.DistinctTurnTargetUsage(activation)is { } actualTurnLedger && (targets.Count != 1 || !_coreAppliedDamageBenefitsOperations.CanActivateDistinctTurnTarget(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program!.Id, actualTurnLedger, targets[0])))
            return new CommandError(CommandErrorCode.IllegalAction, "This target was already designated by this ledger in the actual turn.");
        if (activation is { TargetPhaseLedgerId: { } targetLedger } && (targets.Count != 1 || !_coreProgramParticipantPhaseOperationsOperations.CanActivateTargetPhaseLedger(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program!.Id, targetLedger, targets[0])))
            return new CommandError(CommandErrorCode.IllegalAction, "This target was already used in this play phase.");
        if (plan is not null)
        {
            if (_coreProgramSelectionValidationOperations.ProgramSelectionValidators.Validate(ProgramSelectionValidationStage.BeforePublicPrerequisites, new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, program!, plan), action, cards, targets)is { } prerequisiteError)
                return prerequisiteError;
            var owner = _coreEquipmentSlotGroupProgramsOperations.CreateProgramLegalityParticipant(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);
            if (!plan.Features.Legality.CanStart(owner))
                return new CommandError(CommandErrorCode.IllegalAction, "The program's public prerequisites are no longer satisfied.");
            if (targets.Any(seat => !plan.Features.Legality.CanSelectTarget(owner, _coreEquipmentSlotGroupProgramsOperations.CreateProgramLegalityParticipant(seat))))
                return new CommandError(CommandErrorCode.InvalidTarget, "The program's target prerequisites are no longer satisfied.");
        }

        return null;
    }

    internal static bool ProgramSelectionUsesEffectiveSameSuit(SkillProgram program, ProgramExecutionPlan plan) => plan.Activation is { SelectedCardsSameSuit: true } && plan.Features.Single(SkillProgramEffectOp.UseSelectedCardsAs)is { SourceBind: { } binding } && program.ViewAs.FirstOrDefault(rule => rule.Id == binding)is { SameSuit: true, UseEffectiveInputSuit: true };
    internal Suit ProgramSameSuitInputSuit(CharacterState owner, int cardId, bool useEffectiveSuit)
    {
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(item => item.Id == cardId);
        return useEffectiveSuit ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card) : card.Suit;
    }

    internal CommandResult SubmitUseProgramSkill(UseProgramSkillCommand command)
    {
        if (_coreProgramActionModulesOperations.ProgramActions.TrySubmit(command, out var registeredResult))
            return registeredResult;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(error.Code, error.Message);
        var owner = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var candidates = BuildProgramActions(owner).Where(candidate => candidate.ProgramSkillId == command.SkillId && candidate.ProgramActivationId == command.ActivationId).ToArray();
        var action = command.SkillOwnerSeat is { } skillOwnerSeat ? candidates.SingleOrDefault(candidate => candidate.ProgramSkillOwnerSeat == skillOwnerSeat) : candidates.Length == 1 ? candidates[0] : candidates.SingleOrDefault(candidate => candidate.ProgramSkillOwnerSeat is { } candidateOwner && command.TargetSeats.Count == 1 && command.TargetSeats[0] == candidateOwner);
        if (action is null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The skill activation is not currently available.");
        error = ValidateProgramSelection(action, command.CardIds, command.TargetSeats);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(error.Code, error.Message);
        return _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            ExecuteValidatedProgramSkill(owner, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void ExecuteProgramSkill(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (_coreProgramActionModulesOperations.ProgramActions.TryExecute(owner, action, cards, targets))
            return;
        var current = BuildProgramActions(owner).SingleOrDefault(candidate => candidate.ProgramSkillId == action.ProgramSkillId && candidate.ProgramActivationId == action.ProgramActivationId && candidate.ProgramSkillOwnerSeat == action.ProgramSkillOwnerSeat) ?? throw new InvalidOperationException("The skill activation is no longer available.");
        if (ValidateProgramSelection(current, cards, targets)is { } error)
            throw new InvalidOperationException(error.Message);
        ExecuteValidatedProgramSkill(owner, current, cards, targets);
    }

    internal void ExecuteValidatedProgramSkill(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (_coreSharedPhaseSkillsOperations.TryExecuteSharedProgramActivation(owner, action, cards, targets))
            return;
        if (action.ProgramSkillOwnerSeat is { } skillOwnerSeat)
        {
            if (_coreProgramActionModulesOperations.ProgramActions.TryContribute(owner, _coreMatchParticipantsPlayersAccess._players[skillOwnerSeat], action, cards[0]))
                return;
            ExecuteProgramContribution(owner, _coreMatchParticipantsPlayersAccess._players[skillOwnerSeat], action, cards[0]);
            return;
        }

        var program = GetEnabledSkillProgram(owner, action.ProgramSkillId ?? throw new InvalidOperationException("The program action has no skill identity."));
        var plan = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Activation, action.ProgramActivationId!);
        var activation = plan.Activation!;
        _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(owner, activation.MarkerCost, program.Id, activation.Id);
        if (activation.UsesPerGame is { } gameLimit)
        {
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(owner.Seat, program.Id, activation.UsageGroup, SkillUsageScope.Game, gameLimit))
                throw new InvalidOperationException("The skill activation already exhausted its game usage limit.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(owner.Seat, program.Id, activation.UsageGroup, SkillUsageScope.Game, Count: 1));
        }

        var key = (owner.Seat, program.Id, activation.UsageGroup);
        _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programUses[key] = _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault(key) + 1;
        if (_coreProgramSelectionModulesOperations.ProgramSelections.PhaseLimit(new(owner.Seat, program, plan))is not null || activation.CardCountExpression is not null)
            _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses[key] = _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault(key) + 1;
        var frame = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, program.Id, activation.Id, program.GameplayHash, 0, Array.AsReadOnly(cards.ToArray()), Array.AsReadOnly(targets.ToArray()))
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id),
            NextActualUseAdjustment = _coreNextActualUseTargetAdjustmentsOperations.FreezeNextActualUseProgramSelection(owner, plan, targets),
            HpLossSlashSelection = _coreHpLossSlashMaterialsOperations.FreezeHpLossSlashSelection(owner, plan, targets),
            SelectedAllOwnerHandCards = plan.Features.HasOperation(SkillProgramEffectOp.DrawAllHandSelectedBonus) ? _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).All(card => cards.Contains(card.Id)) : null
        };
        frame = _coreHpConditionalSkillGrantsOperations.CaptureHpConditionalProgramSource(frame);
        frame = _characterHeQiOlOperations.CaptureEquipmentSuitProgramSource(frame);
        frame = _characterLuKangWumiaoOperations.CaptureWuLuKangPindianSelection(frame, activation);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _characterLuKangWumiaoOperations.IssueWuLuKangPindianSelection(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, owner.Seat, program.Id, activation.Id));
        _characterZhaoYunOlGod2016Operations.IssueHpScaledProgramSelection(frame, activation);
        _coreMatchLogOperations.AddLog("ActiveSkill", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】。", owner.Seat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void ExecuteProgramContribution(CharacterState provider, CharacterState skillOwner, LegalAction action, int cardId)
    {
        var program = GetEnabledSkillProgram(skillOwner, action.ProgramSkillId ?? throw new InvalidOperationException("The contribution action has no skill identity."));
        var contribution = program.Contributions.Single(item => item.Id == action.ProgramActivationId);
        var instanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(skillOwner, program.Id);
        if (!_coreLordSkillProjectionsOperations.HasSkillRoleQualification(skillOwner, program.Id, instanceId, contribution.OwnerRole))
            throw new InvalidOperationException("The contribution's exact skill source no longer qualifies.");
        var projectedInstanceId = skillOwner.SkillGrants.Grants.Any(g => g.SkillId == program.Id && g.SkillInstanceId == instanceId && g.LordProjection is not null) ? instanceId : null;
        var key = (provider.Seat, skillOwner.Seat, program.Id, contribution.Id);
        if (_coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses.GetValueOrDefault(key) >= contribution.UsesPerPlayPhase)
            throw new InvalidOperationException("The contribution was already used in this play phase.");
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(provider).Single(candidate => candidate.Id == cardId);
        if (!ProgramCardKindMatch.Contains(contribution.CardKinds, card.Kind) && !contribution.CardSuits.Contains(card.Suit))
            throw new InvalidOperationException("The contributed physical hand card no longer matches the binding.");
        _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses[key] = _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses.GetValueOrDefault(key) + 1;
        var actionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var reason = new CardMoveReason($"skill-program.{(program.Id)}.{(contribution.Id)}.contribute");
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(provider.Seat), CardLocation.Processing, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, skillOwner.Seat, actionId));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.Hand(skillOwner.Seat), reason, _characterHuangQuanMouOperations.NativeGiveCompletion(actionId, provider.Seat, skillOwner.Seat, [card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillContributionResolvedEvent(actionId, provider.Seat, skillOwner.Seat, program.Id, contribution.Id, card.Id, card.Kind, card.Suit, projectedInstanceId));
        _coreMatchLogOperations.AddLog("ActiveSkill", $"{(provider.Name)} 响应【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】，将【{(card.DisplayName)}】交给 {(skillOwner.Name)}。", provider.Seat, skillOwner.Seat);
    }

    internal void ResetProgramContributionUsesForPlayPhase(int providerSeat)
    {
        foreach (var key in _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses.Keys.Where(key => key.ProviderSeat == providerSeat).ToArray())
            _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programContributionUses.Remove(key);
    }

    internal void FinishProgramSkill(ProgramSkillFrame frame, bool completed) => _programCompletionOperations.Get().Finish(frame, completed);
    internal void BeginProgramSkillDying(long parentFrameId, CharacterState victim)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != parentFrameId)
            throw new InvalidOperationException("A program dying continuation requires its program frame.");
        var responders = Array.AsReadOnly(_coreNativeDyingOperations.BuildDyingResponderSeats(victim.Seat).ToArray());
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new DyingFrame(id, parentFrameId, victim.Seat, null, responders, 0, DyingContinuationKind.ProgramSkill) { HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(victim.Seat, parentFrameId) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerDyingEvent(id, victim.Seat, null));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (!_coreProgramLifecycleOperations.TryBeginMandatorySelfDyingProgram(_coreDyingCursorRuntimeOperations.ActiveDying!) && !_coreDyingEntryProgramsOperations.TryBeginDyingEntryProgramWindow(_coreDyingCursorRuntimeOperations.ActiveDying!))
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
    }

    internal SkillProgramStepOutcome BeginProgramSkillDamage(ProgramSkillFrame frame, int targetSeat, int amount, ProgramParticipantReference? sourceReference = null, DamageNature? nature = null, bool sourceLess = false, int? explicitSourceSeat = null)
    {
        var insertion = _coreProgramDamageInsertionRuntimeOperations.DamageInsertions.Describe(frame);
        var judgmentNested = frame.WindowContext?.Judgment is { } frozenJudgment && _coreJudgmentCursorRuntimeOperations.ActiveJudgment is { } pendingJudgment && pendingJudgment.Id == frozenJudgment.JudgmentFrameId && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, _coreNativeDamageHandlesOperations.GetJudgmentAttack(pendingJudgment));
        var damageWindowNested = frame.WindowContext?.Window == SkillProgramTriggerWindow.AfterDamageApplied && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } outerWindow && outerWindow.Id == frame.WindowContext.ParentFrameId;
        _ = _coreRecoveryDamageEdgesOperations.TryDescribeRecoveryDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out var recoveryOrigin, out var recoveryDamageWindowId);
        NativeDrawThresholdDyingOrigin? drawDyingOrigin = null;
        _ = frame.AttackAttempt is null && _coreNativeDrawThresholdDyingOperations.TryDescribeNativeDrawThresholdDyingDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat, out drawDyingOrigin);
        if (insertion is null && (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id || amount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("A program damage effect requires one active program and living target.");
        var state = new AttackAttemptState(explicitSourceSeat ?? (sourceReference is { } reference ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference) : frame.OwnerSeat), targetSeat, amount, nature ?? DamageNature.Normal, sourceLess, frame.WindowContext?.Window == SkillProgramTriggerWindow.JudgmentFinalized ? frame.WindowContext.ParentFrameId : null);
        var parentAttackId = _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId;
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { AttackAttempt = state, AttackReturn = new(parentAttackId, damageWindowNested ? frame.WindowContext!.ParentFrameId : recoveryDamageWindowId, judgmentNested ? frame.WindowContext!.Judgment!.JudgmentFrameId : null) { RecoveryOrigin = recoveryOrigin, NativeDrawDyingOrigin = drawDyingOrigin, Insertion = insertion } });
        global::CardGame.Core.GameEngine.IDamageAttempt attack = _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(frame.Id);
        if (frame.WindowContext?.Judgment is { } judgment)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentDamageRequestedEvent(frame.WindowContext.ParentFrameId, judgment.JudgmentFrameId, frame.SkillId, frame.TriggerId!, attack.SourceSeat, targetSeat, amount, nature ?? DamageNature.Normal));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.Skills[frame.SkillId].Name)}】将对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 造成 {(amount)} 点{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetDamageNatureLabel(nature ?? DamageNature.Normal))}伤害。", attack.SourceSeat, targetSeat);
        }

        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome BeginProgramSkillPindian(ProgramSkillFrame frame, int targetSeat)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id || frame.PindianResultBindings.Any(item => item.Name == SimpleProgramPindianResultBind) || frame.SelectedCardIds.Count != 1 && !_characterLuKangWumiaoOperations.HasWuLuKangPindianSelection(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Activation!) || frame.SelectedTargetSeats.Count != 1 || frame.SelectedTargetSeats[0] != targetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(frame.OwnerSeat, targetSeat) || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[targetSeat]) || frame.SelectedCardIds.Count == 1 && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Any(card => card.Id == frame.SelectedCardIds[0]))
        {
            throw new InvalidOperationException("A program Pindian requires one current owner hand card and one living other target with hand cards.");
        }

        var definition = _contentRegistryOperations._contentRegistry!.Skills[frame.SkillId];
        _corePindianOperations.BeginSharedPindian(frame.Id, new(frame.SkillId, definition.Name, definition.Name, "选择拼点牌"), frame.OwnerSeat, targetSeat, frame.SelectedCardIds.SingleOrDefault(), programResultBind: SimpleProgramPindianResultBind, programResultVisibility: SkillProgramCardSetVisibility.Public);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome UseProgramSelectedCardsAs(ProgramSkillFrame frame, int targetSeat, string viewAsId, CardKind outputKind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.TriggerId is not null || active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId || outputKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.Peach or CardKind.ArrowBarrage) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id)
            throw new InvalidOperationException("A selected-card use requires the current active program activation.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var source = new CardConversionSource(frame.SkillId, viewAsId, owner.Seat, frame.SkillInstanceId);
        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, frame.SelectedCardIds, outputKind, forResponse: false, source) ?? throw new InvalidOperationException("The selected physical cards no longer satisfy the configured view-as rule.");
        if (outputKind == CardKind.ArrowBarrage)
            return _coreProgramSelectedGlobalCardUsesOperations.UseProgramSelectedCardsAsGlobal(frame, selection);
        if (outputKind == CardKind.Peach)
        {
            var rule = GetEnabledSkillProgram(owner, frame.SkillId).ViewAs.Single(item => item.Id == viewAsId);
            _coreNextActualUseTargetAdjustmentsOperations.PrepareNextActualUseSelectedRecovery(frame, selection);
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, selection.Cards[0], "桃", CardKind.Peach, 1 + rule.RecoveryBonus, conversionSource: source, physicalCards: selection.Cards);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.HpLossSlashSelection is not null)
            return _coreHpLossSlashMaterialsOperations.BeginHpLossSelectedSlash(frame, selection);
        if (frame.NextActualUseAdjustment is { Kind: ProgramNextActualUseAdjustmentKind.AddSlashTarget })
            return _coreNextActualUseTargetAdjustmentsOperations.BeginNextActualUseSelectedSlash(frame, selection);
        var target = _coreMatchParticipantsPlayersAccess._players.SingleOrDefault(player => player.Seat == targetSeat) ?? throw new InvalidOperationException("The selected card-use target no longer exists.");
        var slashRule = GetEnabledSkillProgram(owner, frame.SkillId).ViewAs.FirstOrDefault(item => item.Id == viewAsId);
        if (!_coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, target, physicalSuit: _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection), effectiveColor: _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, selection), physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), ignoreDistance: slashRule is { DistanceUnlimited: true } || _coreFriendTrioOperations.FriendXuanjianIgnoresDistance(owner, source)))
            throw new InvalidOperationException("The selected Slash target is no longer legal.");
        _coreNativeSlashUseOperations.ResolveSlashCore(owner, target, _characterGuanHaiOlOperations.IsXieweiSelection(selection) ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selection.Cards[0], selection.Source) : selection.Cards[0], outputKind, owner.Seat, physicalCards: selection.Cards, conversionSource: selection.Source);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PendProgramExtraTurn(ProgramSkillFrame frame, int? targetSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId)
            throw new InvalidOperationException("An extra turn requires the active program frame.");
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        // The singular typed progression names the beneficiary. Without an
        // explicit target reference (Lianpo) the owner benefits; a later pend
        // overwrites an earlier one ("latest declaration wins").
        var beneficiarySeat = targetSeat ?? active.OwnerSeat;
        var beneficiary = _coreMatchParticipantsPlayersAccess._players[beneficiarySeat];
        if (!beneficiary.IsAlive)
            return;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("An extra turn lost its actual turn progression.");
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression with
        {
            PendingBeneficiarySeat = beneficiarySeat
        };
        _coreMatchLogOperations.AddLog("ExtraTurnPended", beneficiarySeat == active.OwnerSeat ? $"{(owner.Name)} 将在当前回合结束后获得一个额外回合。" : $"{(owner.Name)} 令 {(beneficiary.Name)} 将在当前回合结束后获得一个额外回合。", active.OwnerSeat, beneficiarySeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramExtraTurnPendedEvent(active.Id, active.SkillId, beneficiarySeat));
    }

    internal SkillProgramStepOutcome UseProgramBoundCardByTarget(ProgramSkillFrame frame, int userSeat, string sourceBind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id)
            throw new InvalidOperationException("A bound-card use requires the current active program frame.");
        var user = _coreMatchParticipantsPlayersAccess._players[userSeat];
        var binding = frame.CardSetBindings.SingleOrDefault(item => item.Name == sourceBind) ?? throw new InvalidOperationException("A bound-card use references a missing card set.");
        var cancel = (string message) =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), message);
        };
        if (binding.CardIds.Count != 1 || binding.Visibility != SkillProgramCardSetVisibility.Public)
            throw new InvalidOperationException("A bound-card use requires exactly one public card.");
        if (!user.IsAlive)
        {
            cancel("用牌角色已失效，技能剩余结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var cardId = binding.CardIds[0];
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(item => item.Id == cardId);
        if (location.OwnerSeat != user.Seat || location.Zone != CardZoneKind.Hand || card is null)
        {
            cancel("赠出的牌已离开用牌者手牌，使用步骤取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
            throw new InvalidOperationException("A bound-card use requires an equipment card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new ProgramMovementContinuation(user.Seat, 0, null) });
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.UseBoundCardByTarget)}");
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, user.Seat, []);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.Processing, reason);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(user, card, resolutionId))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(user, card, resolutionId);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static bool BoundEquipmentUseMatches(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending, CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SkillPrograms.BoundEquipmentUseMatches(frame, effect, pending, use);

    [NativeUseReturn(150)]
    internal void ContinueProgramAfterBoundEquipmentUse(CardUseFrame completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PendingMovementContinuation: { } pending } frame)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (frame.InstructionIndex == 0 || plan.Instructions[frame.InstructionIndex - 1].Op != SkillProgramEffectOp.UseBoundCardByTarget)
            return;
        if (!BoundEquipmentUseMatches(frame, plan.Instructions[frame.InstructionIndex - 1], pending, completed))
            throw new InvalidOperationException("A bound equipment use lost its exact material, participant or owning instruction.");
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    [NativeUseReturn(160)]
    internal void ContinueAfterSelectedNativeUse(NativeUseReturnContext context)
    {
        var use = context.Use;
        if (use.ChosenAssault is null && !_coreNextActualUseTargetAdjustmentsOperations.HasExactNextActualUseAdjustedProgramReturn(use) && !context.HpLossMaterialReturn)
            ContinueProgramAfterSelectedCardUse();
    }

    internal void ContinueProgramAfterSelectedCardUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.TriggerId is not null)
            return;
        var program = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(frame.SkillId)?.Program;
        if (frame.EquipmentSuitSourceReceipt is not null && !_characterHeQiOlOperations.ValidateEquipmentSuitProgramSource(frame))
            throw new InvalidOperationException("An equipment-suit program lost its exact issued source receipt.");
        var activation = program is null ? null : ProgramInstructionResolver.Default.FindActivation(program, frame.ActivationId);
        if (activation is null || frame.InstructionIndex == 0 || activation.Effects[frame.InstructionIndex - 1].Op is not (SkillProgramEffectOp.UseSelectedCardsAs or SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick))
            return;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CompleteProgramSkillAfterDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != dying.ParentFrameId)
            throw new InvalidOperationException("Program dying resolution lost its continuation.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal static bool IsAwaitingProgramMovement(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(frame);

    internal static PromptChoice CreateProgramPlayChoice(LegalAction action) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SkillPrograms.CreateProgramPlayChoice(action);
}
