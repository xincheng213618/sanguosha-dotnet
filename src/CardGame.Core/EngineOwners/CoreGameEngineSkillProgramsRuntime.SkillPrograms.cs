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
    internal bool IsBoundEquipmentAwaitedMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index >= 0 && index + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is CardUseFrame use && BoundEquipmentUseMatches(frame, effect, pending, use);
    }

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
    internal void AssertProgramSkillState()
    {
        _coreActualPhaseEndsOperations.HasActualPhaseEndBoundaryFrame();
        _coreOriginalHandEntityObserversOperations.AssertOriginalHandEntityState();
        _coreTurnDefaultStatsValidationOperations.AssertTurnDefaultStatState();
        _coreOffTurnUsedCardObserversOperations.AssertOffTurnUsedCardGiftObservers();
        _coreOrderedPrintedSkillLossObserversOperations.AssertOrderedPrintedSkillLossState();
        _coreOrderedPrintedSkillLossObserversOperations.AssertOrderedPrintedSkillLossObservers();
        _coreRecipientCategoryMarkObserversOperations.AssertRecipientCategoryMarkState();
        _coreAdjacentDiscardStorageOperations.AssertPendingAdjacentDiscardOrigins();
        _coreForeignContestAidObserversOperations.AssertForeignContestAidPrograms();
        _coreRecipientContestReceiptsOperations.AssertPindianWinnerUses();
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _coreUnnullifiableOrdinaryTricksOperations.AssertOrdinaryTrickCannotNullify(use);
        _coreUniqueHpTargetAnnouncementsOperations.AssertUniqueHpTargetAnnouncements();
        _coreTieredRoundZeroReturnsOperations.AssertCappedConversionBenefits();
        _coreTieredRoundZeroUsesOperations.AssertTieredRoundConversionUses();
        _coreTieredRoundZeroResponsesOperations.AssertTieredRoundResponseUses();
        _coreRoundOnceCardlessNativeResponsesOperations.AssertRoundOnceCardlessNativeResponses();
        _coreRoundOnceCardlessProgramRequestsOperations.AssertRoundOnceCardlessProgramRequests();
        _characterHuangQuanOlOperations.AssertHuangQuanNativeUses();
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            if (use.RoundOnceCardlessBasicUse is not null && !_coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(use.Id))
                throw new InvalidOperationException("Cardless native use lost its exact independently paid source invoice.");
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            _characterCaoChunOlOperations.AssertCaoChunNativeUse(use);
            if (_characterWangCanOlOperations.IsDenglouNativeUse(use))
                _characterWangCanOlOperations.AssertDenglouNativeUse(use);
            if (_characterGuoSiOlOperations.IsGuoSiNativeUse(use))
                _characterGuoSiOlOperations.AssertGuoSiNativeUse(use);
            if (use.WangYunNativeOrigin is not null && !_characterWangYunOlOperations.ValidWangYunNativeUse(use))
                throw new InvalidOperationException("A Wang Yun native use lost its owning invoice.");
            if (use.ZhangJiNativeOrigin is not null && !_characterZhangJiOlOperations.ValidZhangJiNativeUse(use))
                throw new InvalidOperationException("A Zhang Ji native use lost its owning invoice.");
            _coreDyingSuitsAndHistoricalUseObserversOperations.AssertHistoricalEndingUse(use);
            _coreRoundPricedPileAlcoholOperations.AssertRoundPileAlcoholUse(use);
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            _coreComparedBlackSlashPoliciesOperations.AssertComparedBlackSlashReceipt(use);
            _coreRoundDistinctBasicUseOperations.AssertRoundDistinctBasicUses(use);
            _characterKongShuOlOperations.AssertLeiluanPayments(use);
            _coreShanheS5XuncanOperations.AssertShanheS5XuncanNativeUse(use);
        }

        var frames = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().ToArray();
        foreach (var frame in frames)
        {
            var program = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(frame.SkillId)?.Program;
            if (frame.EquipmentSuitSourceReceipt is not null && !_characterHeQiOlOperations.ValidateEquipmentSuitProgramSource(frame))
                throw new InvalidOperationException("An equipment-suit program lost its exact issued source receipt.");
            if (frame.HpConditionalSourceReceipt is not null && !_coreHpConditionalSkillGrantsOperations.CanContinueHpConditionalProgram(frame))
                throw new InvalidOperationException("An HP-conditional program lost its exact issued source receipt.");
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(frame.OwnerSeat) || string.IsNullOrWhiteSpace(frame.SkillInstanceId) || program?.GameplayHash != frame.GameplayHash)
                throw new InvalidOperationException("An active skill program has an invalid cursor or selection.");
            ProgramExecutionPlan plan;
            try
            {
                plan = ProgramInstructionResolver.Default.Resolve(frame, program);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("An active skill program has an invalid cursor or selection.");
            }

            if (frame.InstructionIndex == 0 && _characterHuaTuoAnniversaryGodOperations.IsProgramWaitingForSkillInvocation(frame))
                continue;
            _coreProgramRuntimeHooksOperations.RuntimeHooks.AssertFrame(ProgramFrameInvariantPhase.BeforeSelectionState, frame, plan);
            if (frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count || frame.SelectedCardIds.Distinct().Count() != frame.SelectedCardIds.Count || frame.SelectedTargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || frame.CardSetBindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != frame.CardSetBindings.Count || frame.CardSetBindings.Any(binding => binding.SelectionActorSeat is { } selectionActor && (!_coreForeignEquipmentDiscardPoliciesOperations.HasForeignDiscardCapability && !_coreNativeLogicalDiscardsOperations.TracksNativeLogicalDiscards || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(selectionActor) || !plan.Instructions.Take(frame.InstructionIndex).Any(e => e.ResultBind == binding.Name && (e.Op is SkillProgramEffectOp.SelectOwnedCards or SkillProgramEffectOp.SelectOwnedHandRankSum or SkillProgramEffectOp.SelectSourceCard or SkillProgramEffectOp.SelectCardSubset or SkillProgramEffectOp.FilterBoundCards or SkillProgramEffectOp.SelectTurnDamageUseDebtPayment or SkillProgramEffectOp.SelectEquipmentPairAndPayment or SkillProgramEffectOp.SelectDyingOwnedCard or SkillProgramEffectOp.BindBoundaryYuJinRemainder || e.Op == SkillProgramEffectOp.SelectFrozenHandExchangeDebtPayment && selectionActor == frame.OwnerSeat && frame.PhaseHandDebtPayment is { } debt && debt.ResultBind == binding.Name && binding.CardIds.Count == debt.RequiredPaymentCount && binding.SourceLocations.All(location => location.OwnerSeat == frame.OwnerSeat && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) && _coreHalfHandPhaseDebtReceiptsOperations.ValidPhaseHandDebtPayment(frame)))) || binding.CardIds.Count != binding.SourceLocations.Count || binding.CardIds.Distinct().Count() != binding.CardIds.Count || plan.Instructions.Any(instruction => instruction is { FreezeMovedCardSuit: true } frozen && frozen.ResultBind == binding.Name) && (binding.CardIds.Count != 1 || binding.FrozenRevealedSuit is not { } frozenSuit || !Enum.IsDefined(frozenSuit))) || frame.PindianResultBindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != frame.PindianResultBindings.Count || frame.AttackRangeCoverageBindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != frame.AttackRangeCoverageBindings.Count || frame.AttackRangeCoverageBindings.Any(binding => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(binding.SubjectSeat) || binding.BeforeCount < 0 || binding.AfterCount < 0 || binding.BeforeCount >= _coreMatchParticipantsPlayersAccess._players.Count || binding.AfterCount >= _coreMatchParticipantsPlayersAccess._players.Count) || frame.PendingMovementContinuation is { } movement && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(movement.SubjectSeat) || movement.BeforeCount < 0 || movement.BeforeCount >= _coreMatchParticipantsPlayersAccess._players.Count))
                throw new InvalidOperationException("An active skill program has an invalid cursor or selection.");
            if (frame.TopReorder?.RequiredTopCount is not null)
                _coreExactTopReorderProgramsOperations.ValidateExactTopReorder(frame);
            if (frame.TopReorder?.Population is not null)
                _corePopulationTopReorderProgramsOperations.ValidatePopulationTopReorder(frame);
            if (frame.DomainCrossing is { } domain && (plan.Instructions[frame.InstructionIndex - 1].Op != SkillProgramEffectOp.ResolveFirstGameDomainCrossing || frame.WindowContext?.Window != SkillProgramTriggerWindow.FirstGameDomainCrossing || domain.Stage is not ("target" or "participants" or "discard" or "heal" or "complete") || domain.ReferenceSkillId != plan.Instructions[frame.InstructionIndex - 1].SkillIds.Single() || domain.ReferenceSkillInstanceId is not null && string.IsNullOrWhiteSpace(domain.ReferenceSkillInstanceId) || domain.ReferencePileLocation is { } referencedLocation && (domain.ReferenceSkillInstanceId is null || referencedLocation.Zone != CardZoneKind.PublicPersistentPile || referencedLocation.OwnerSeat != frame.OwnerSeat) || domain.ParticipantIndex is < 0 or > 2 || domain.OtherSeat != -1 && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(domain.OtherSeat) || domain.OtherSeat == frame.OwnerSeat)))
                throw new InvalidOperationException("A first-domain reward lost its typed participant cursor.");
            if (frame.PileEquipment is { } equipment && (plan.Instructions[frame.InstructionIndex - 1].Op is not (SkillProgramEffectOp.StoreArbitraryOwnedPublicPile or SkillProgramEffectOp.UsePublicPileEquipmentSequence) || equipment.PileLocation.Zone != CardZoneKind.PublicPersistentPile || equipment.PileLocation.OwnerSeat != frame.OwnerSeat || string.IsNullOrWhiteSpace(equipment.PileInstance) || equipment.SelectedIds.Distinct().Count() != equipment.SelectedIds.Count || equipment.RecipientSeat is { } recipient && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || equipment.Stage is not ("store-choice" or "gain" or "recipient" or "equip-choice" or "equipment-use" or "hp-paid" or "complete") || equipment.Stage == "hp-paid" && !equipment.HadActualEquipmentUse || equipment.Stage == "equipment-use" && (equipment.ActiveCardId is null || equipment.ActiveUseFrameId is null || equipment.RecipientSeat is null)))
                throw new InvalidOperationException("A public-pile equipment sequence lost its typed paid-use cursor.");
            if (frame.PublicPileColorPayment is { } colorPayment && (plan.Instructions[frame.InstructionIndex - 1].Op != SkillProgramEffectOp.PublicPileColorDamage || frame.ConversionPreviousPolarity != SkillPolarity.Yang || colorPayment.From.OwnerSeat != frame.OwnerSeat || colorPayment.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || colorPayment.PileLocation.Zone != CardZoneKind.PublicPersistentPile || colorPayment.PileLocation.OwnerSeat != frame.OwnerSeat || string.IsNullOrWhiteSpace(colorPayment.PileInstance) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(colorPayment.TargetSeat) || colorPayment.TargetSeat == frame.OwnerSeat))
                throw new InvalidOperationException("A public-pile color payment lost its owning paid cursor.");
            if (frame.SelectedCardPayment is { } selectedPayment)
            {
                if (selectedPayment.InstructionIndex < 1 || selectedPayment.InstructionIndex > frame.InstructionIndex || plan.Instructions[selectedPayment.InstructionIndex - 1].Op != selectedPayment.Operation || selectedPayment.Operation is not (SkillProgramEffectOp.GiveSelected or SkillProgramEffectOp.DiscardSelected or SkillProgramEffectOp.PlaceSelectedEquipment) || !selectedPayment.CardIds.SequenceEqual(frame.SelectedCardIds) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(selectedPayment.RecipientSeat) || selectedPayment.Operation == SkillProgramEffectOp.DiscardSelected && selectedPayment.RecipientSeat != frame.OwnerSeat || frame.SelectedCardPaymentResult is null && frame.PendingMovementContinuation is not null || !selectedPayment.MovementCommitted && (selectedPayment.ActiveChildFrameId is not null || frame.SelectedCardPaymentResult is not null) || frame.SelectedCardPaymentResult is { } result && (result.InstructionIndex != selectedPayment.InstructionIndex || result.Operation != selectedPayment.Operation || !result.Completed || result.LastChildFrameId != selectedPayment.LastCompletedChildFrameId || selectedPayment.ActiveChildFrameId is not null))
                    throw new InvalidOperationException("A selected-card payment lost its paid instruction or child result.");
            }
            else if (frame.SelectedCardPaymentResult is not null)
                throw new InvalidOperationException("A selected-card movement result has no paid instruction.");
            _coreProgramRuntimeHooksOperations.RuntimeHooks.AssertFrame(ProgramFrameInvariantPhase.AfterSelectionState, frame, plan);
            if (frame.PendingMovementContinuation is { } pendingMovement)
            {
                var paidEffect = frame.InstructionIndex > 0 ? plan.Instructions[frame.InstructionIndex - 1] : null;
                var awaitsSuitPlacement = pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.BeforeCount == 0 && pendingMovement.CoverageResultBind is null && (paidEffect?.Op == SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage && frame.SignedDamagePayment is { Stage: not SignedDamagePaymentStage.ChoosingCost } || paidEffect?.Op == SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit && frame.SuitPreventionBenefit is not null || paidEffect?.Op == SkillProgramEffectOp.PlaceMatchedJudgmentCard && frame.MatchedJudgmentPlacement is not null);
                var awaitsShownGift = paidEffect?.Op == SkillProgramEffectOp.GiveShownBoundCardsAndGrantTurnHandLimit && frame.ShownGiftReceipt?.InstructionIndex == frame.InstructionIndex - 1 && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsRandomTransfer = paidEffect?.Op == SkillProgramEffectOp.TransferRandomOwnedCard && pendingMovement.CoverageResultBind is null && frame.SelectedTargetSeats.Count == 1 && pendingMovement.SubjectSeat == frame.SelectedTargetSeats[0];
                var awaitsJudgmentClaim = paidEffect?.Op == SkillProgramEffectOp.ClaimJudgmentCard && frame.WindowContext?.Window == SkillProgramTriggerWindow.JudgmentFinalized && pendingMovement.CoverageResultBind is null && pendingMovement.SubjectSeat == frame.OwnerSeat;
                var awaitsRepeatedJudgment = paidEffect?.Op == SkillProgramEffectOp.RepeatJudgment && frame.RepeatedJudgment is { LastMatched: not null } && pendingMovement.CoverageResultBind is null && pendingMovement.SubjectSeat == frame.OwnerSeat;
                var awaitsLostHpOwnedGift = paidEffect?.Op == SkillProgramEffectOp.DrawLostHpThenOfferOwnedCardsUpTo && frame.ReexecuteParticipantInstruction && frame.LostHpOwnedGift is { Stage: LostHpOwnedGiftStage.Drawing or LostHpOwnedGiftStage.Moving } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.BeforeCount == 0 && pendingMovement.CoverageResultBind is null && _coreLostHpOwnedGiftsOperations.LostHpOwnedGiftObserverRoot()?.Id == frame.Id;
                var awaitsOwnedMovement = paidEffect is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, AwaitMovementTriggers: true } && paidEffect.CoverageResultBind == pendingMovement.CoverageResultBind;
                // A hand-exchange effect suspends its program behind the nested
                // cards-moved trigger window opened by the exchanged cards.
                var awaitsExchangedMovement = paidEffect?.Op is (SkillProgramEffectOp.ExchangeSelectedTargetHands or SkillProgramEffectOp.ExchangeSelectedTargetEquipment) && pendingMovement.CoverageResultBind is null && pendingMovement.SubjectSeat == frame.OwnerSeat;
                var awaitsConvertingGift = paidEffect?.Op is (SkillProgramEffectOp.GiveOwnedCardToOtherFinalTargetAndDraw or SkillProgramEffectOp.GiveSelectedOwnedCardAndDamage or SkillProgramEffectOp.ObserveDamageSourceHandAndGive or SkillProgramEffectOp.DrawToHandCount) && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsQuotaTop = paidEffect?.Op == SkillProgramEffectOp.PeekTurnQuotaTop && frame.QuotaTop is { Stage: "gain-movement" } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsBudgetOrGift = paidEffect?.Op is (SkillProgramEffectOp.ResolveDiscardBudgetParticipants or SkillProgramEffectOp.DiscardOutsideRangeAfterInsufficientUses or SkillProgramEffectOp.OfferCompletedFactionCostGift) && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsAlternatingSuitTop = paidEffect?.Op is SkillProgramEffectOp.AlternatingSuitDrawDiscard or SkillProgramEffectOp.FirstCategoryCompletedTop && frame.AlternatingSuitTop is { Stage: "draw" or "discard-movement" or "top-movement" or "reward-draw" } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsNamedTurnFlow = paidEffect?.Op is SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw or SkillProgramEffectOp.DiscardHandToNamedTurnCount && frame.NamedTurnCountFlow is { Stage: "target-movement" or "target-draw" or "hand-movement" } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsFinalTargetPublicPile = paidEffect?.Op is SkillProgramEffectOp.CollectFinalTargetCardInPublicPile or SkillProgramEffectOp.ExchangePublicPileHand or SkillProgramEffectOp.ObtainPublicPileCard or SkillProgramEffectOp.DiscardPublicZoneAfterHandPayment && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsPrivateHold = paidEffect?.Op == SkillProgramEffectOp.HoldOwnerHandUntilTurnEnd && frame.PrivateTurnHoldDraft is { Paid: true } hold && hold.Location.PrivateTurnHold?.HoldId == frame.Id && hold.Location.OwnerSeat == frame.OwnerSeat && hold.Location.PrivateTurnHold.SkillId == frame.SkillId && hold.Location.PrivateTurnHold.SkillInstanceId == frame.SkillInstanceId && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsAppliedDamageBenefit = paidEffect?.Op == SkillProgramEffectOp.DrawOwnerAtAppliedDamage && frame.AppliedDamageBenefit is { } appliedBenefit && appliedBenefit.InstructionIndex == frame.InstructionIndex && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsParticipantDiscard = paidEffect?.Op == SkillProgramEffectOp.DiscardDistinctFactionParticipants && frame.DistinctFactionDiscards is { Stage: "discard-children" or "reward-children" } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsDeclaredDeck = paidEffect?.Op == SkillProgramEffectOp.DeclareDeckCriterionAndGiveMatchingCard && frame.DeckCriterion is { Stage: "search" or "recipient" or "gift" } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null;
                var awaitsCappedHandRefresh = paidEffect?.Op == SkillProgramEffectOp.DrawThenDiscardHandToMaximumHp && frame.CappedHandRefresh is { Stage: ProgramCappedHandRefreshStage.Drawing or ProgramCappedHandRefreshStage.Discarding } refresh && pendingMovement.SubjectSeat == refresh.TargetSeat && pendingMovement.CoverageResultBind is null;
                if (!(paidEffect?.Op == SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeSuitDiscard && frame.SlashSuitDiscard is { Stage: ProgramSlashSuitDiscardStage.JudgmentMovement or ProgramSlashSuitDiscardStage.PaymentMovement } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.UseDiamondDelayedOrDiscard && frame.DiamondDelayed is { Stage: ProgramDiamondDelayedStage.Moving or ProgramDiamondDelayedStage.Drawing } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(awaitsParticipantDiscard || awaitsPrivateHold || _coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesMovement(ProgramMovementProofPhase.NativeCost, frame, paidEffect, pendingMovement) || paidEffect?.Op is SkillProgramEffectOp.ResolveFirstGameDomainCrossing or SkillProgramEffectOp.StoreArbitraryOwnedPublicPile or SkillProgramEffectOp.UsePublicPileEquipmentSequence && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !awaitsAppliedDamageBenefit && !awaitsNamedTurnFlow && !awaitsConvertingGift && !(paidEffect?.Op is SkillProgramEffectOp.AwaitOwnedCardMovement or SkillProgramEffectOp.StoreBoundHandInPublicPile or SkillProgramEffectOp.PublicPileColorDamage or SkillProgramEffectOp.RewardDiscardedActionColor && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !awaitsAlternatingSuitTop && !awaitsFinalTargetPublicPile && !awaitsQuotaTop && !awaitsBudgetOrGift && !(paidEffect?.Op == SkillProgramEffectOp.DrawOnFirstProgramTargetEncounter && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op is SkillProgramEffectOp.ExchangeOwnedCardThroughDeckEnd or SkillProgramEffectOp.UseDeckSlashesThenShuffle && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op is SkillProgramEffectOp.ExchangeRespondedCardEntities or SkillProgramEffectOp.DrawPublicSuitThenEscalatingDiscard && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.CompareSelectedHandWithHpHand && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op is SkillProgramEffectOp.DeclareNameForTargetDefense or SkillProgramEffectOp.DrawAndDraftLowHandPopulation or SkillProgramEffectOp.ObtainDeckCardWithConsecutiveTarget && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.ChooseDifferentActionCategoryGift && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op is SkillProgramEffectOp.DepositBoundCardsUntilNextTurn or SkillProgramEffectOp.ObtainDeferredPile or SkillProgramEffectOp.ViewTopCardsAndObtainMatchingCards or SkillProgramEffectOp.StoreTopCardInPublicPile or SkillProgramEffectOp.ExchangePublicPile or SkillProgramEffectOp.DistributeBoundCards && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !awaitsRandomTransfer && !(paidEffect?.Op == SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory && frame.DamageTargetObtain is not null && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.DiscardDamageTargetAndClaimMount && frame.DamageTargetMount is { Receipt: not null } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(frame.FriendEventFlow is { } friendFlow && paidEffect?.Op == friendFlow.Operation && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !_coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesMovement(ProgramMovementProofPhase.BeforePrivatePayments, frame, paidEffect, pendingMovement) && !awaitsSuitPlacement && !awaitsCappedHandRefresh && !awaitsDeclaredDeck && !awaitsShownGift && !awaitsJudgmentClaim && !awaitsRepeatedJudgment && !awaitsOwnedMovement && !awaitsLostHpOwnedGift && !(paidEffect?.Op == SkillProgramEffectOp.MouZhijueUse && frame.MouZhijue is not null && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !_coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesMovement(ProgramMovementProofPhase.PrivatePayments, frame, paidEffect, pendingMovement) && !_coreTieredRoundZeroReturnsOperations.IsCappedConversionAwaitedMovement(frame, pendingMovement) && !_coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesMovement(ProgramMovementProofPhase.EffectPayments, frame, paidEffect, pendingMovement) && !awaitsExchangedMovement && !(paidEffect?.Op == SkillProgramEffectOp.ResolveDeferredHandAlignment && frame.DeferredHandAlignmentResolution is { } deferredAlignment && pendingMovement.SubjectSeat == deferredAlignment.TargetSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.UseRandomDeckEquipment && pendingMovement.CoverageResultBind is null && (frame.SelectedTargetSeats is [var equipmentUser] && pendingMovement.SubjectSeat == equipmentUser || paidEffect.TargetReference?.Kind == ProgramParticipantRef.Owner && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.BeforeCount == 0 && paidEffect.ResultBind is { } arrivalEquipmentBind && frame.CardSetBindings.SingleOrDefault(binding => binding.Name == arrivalEquipmentBind)is { CardIds: [var arrivalEquipmentId], SourceLocations: [var arrivalEquipmentSource] } && arrivalEquipmentId > 0 && arrivalEquipmentSource == CardLocation.DrawPile)) && !(paidEffect?.Op is SkillProgramEffectOp.DiscardSelectedParticipantCards or SkillProgramEffectOp.OfferBoundCardsForDamagePrevention && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.TakeSelectedTargetCards && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect is { Op: SkillProgramEffectOp.MoveBoundCards, AwaitMovementTriggers: true } && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op == SkillProgramEffectOp.ObtainBoundCardsAndArmNextRevealBonus && pendingMovement.SubjectSeat == frame.OwnerSeat && pendingMovement.BeforeCount == 0 && pendingMovement.CoverageResultBind is null) && !(paidEffect?.Op is SkillProgramEffectOp.ChooseCategoryAlternativeDiscard or SkillProgramEffectOp.EscalatingDiscardOrDamage && frame.DiscardChallenge is { } challenge && challenge.ChooserSeat == pendingMovement.SubjectSeat && pendingMovement.CoverageResultBind is null) && !(paidEffect is { } sequentialEffect && global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime.IsSequentialDiscardOp(sequentialEffect.Op) && frame.SequentialDiscard is { Stage: ProgramSequentialDiscardStage.AwaitingMovement, Payment: { } sequentialPayment } && _coreSequentialDiscardReceiptsOperations.ValidSequentialDiscard(frame) && sequentialPayment.ChooserSeat == pendingMovement.SubjectSeat && pendingMovement.BeforeCount == 0 && pendingMovement.CoverageResultBind is null) && !IsBoundEquipmentAwaitedMovement(frame, paidEffect, pendingMovement) && !(paidEffect is { } exchangeEffect && _coreCurrentTurnHandExchangeOperations.IsCurrentTurnHandExchangeMovement(frame, exchangeEffect, pendingMovement)) && !_coreProgramRuntimeHooksOperations.RuntimeHooks.MatchesMovement(ProgramMovementProofPhase.BoundEquipment, frame, paidEffect, pendingMovement))
                    throw new InvalidOperationException("A movement continuation lost its paid instruction.");
            }

            foreach (var coverage in frame.AttackRangeCoverageBindings)
            {
                if (plan.Instructions.Take(frame.InstructionIndex).Count(effect => effect.Op == SkillProgramEffectOp.SelectAndMoveOwnedCard && effect.CoverageResultBind == coverage.Name) != 1 || frame.PendingMovementContinuation?.CoverageResultBind == coverage.Name)
                    throw new InvalidOperationException("An attack-range coverage result has no completed producer.");
            }

            if (plan.Instructions.Take(frame.InstructionIndex).Any(effect => effect is { Op: SkillProgramEffectOp.SelectTarget or SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair, Condition.Kind: SkillProgramConditionKind.ChoiceIs } && !frame.ChoiceBindings.Any(binding => binding.Name == effect.Condition.SourceBind)))
                throw new InvalidOperationException("A choice-guarded target selection lost its committed choice.");
            var executedSelection = plan.Instructions.Take(frame.InstructionIndex).LastOrDefault(effect => (effect.Op is not (SkillProgramEffectOp.SelectTarget or SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair) || effect.Condition.Kind != SkillProgramConditionKind.ChoiceIs || frame.ChoiceBindings.Any(binding => binding.Name == effect.Condition.SourceBind && binding.OptionId == effect.Condition.OptionId)) && effect.Op is (SkillProgramEffectOp.BindOwnTurnEnemyHandGain or SkillProgramEffectOp.SelectRelativeZoneDemandTarget or SkillProgramEffectOp.SelectTarget or SkillProgramEffectOp.SelectTargets or SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget or SkillProgramEffectOp.SelectChainedByMarker or SkillProgramEffectOp.SelectOneSelectedTarget or SkillProgramEffectOp.DamageFarthestCharacter or SkillProgramEffectOp.OfferCompletedCardGift or SkillProgramEffectOp.RequestLegalSlashByNearest or SkillProgramEffectOp.OfferUnlimitedVirtualSlash or SkillProgramEffectOp.SelectIssuedFixedRecipientWithDeathReturn or SkillProgramEffectOp.SelectEquipmentPairAndPayment or SkillProgramEffectOp.SelectBoundaryBuLianShiAnxuLeast or SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair or SkillProgramEffectOp.SelectDyingOwnedCard or SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash or SkillProgramEffectOp.SelectRedJudgmentFireTarget));
            var awaitingCurrentSelection = executedSelection is not null && ReferenceEquals(executedSelection, plan.Instructions[frame.InstructionIndex - 1]) && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } pending && pending.Choices.Count > 0 && pending.Choices.All(choice => choice.Parameters.GetValueOrDefault("frame-id") == frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) && choice.Parameters.GetValueOrDefault("program-action") == (executedSelection.Op == SkillProgramEffectOp.SelectTarget ? "select-target" : executedSelection.Op == SkillProgramEffectOp.SelectRedJudgmentFireTarget ? "red-judgment-fire-target" : executedSelection.Op == SkillProgramEffectOp.SelectRelativeZoneDemandTarget ? "relative-zone-target" : executedSelection.Op == SkillProgramEffectOp.SelectEquipmentPairAndPayment ? "equipment-pair-payment" : executedSelection.Op == SkillProgramEffectOp.SelectDyingOwnedCard ? "dying-owned-card" : executedSelection.Op == SkillProgramEffectOp.SelectTargets ? "select-targets" : executedSelection.Op == SkillProgramEffectOp.DamageFarthestCharacter ? "advanced-lifecycle" : "strategic-choice"));
            var dynamicTargetsValid = executedSelection is null || executedSelection.Op switch
            {
                SkillProgramEffectOp.SelectRedJudgmentFireTarget => _coreGhostActivityProgramsOperations.IsValidRedJudgmentFireSelection(frame, executedSelection, awaitingCurrentSelection),
                SkillProgramEffectOp.BindOwnTurnEnemyHandGain => _coreShanheS5ZhangliangOperations.HasExactOwnTurnEnemyHandGainSelection(frame),
                SkillProgramEffectOp.SelectBoundaryBuLianShiAnxuLeast => _characterBuLianShiBoundaryMobileOperations.ValidBoundaryBuLianShiAnxuLeastSelection(frame),
                SkillProgramEffectOp.SelectBoundaryLingTongEquipmentPair => _characterLingTongBoundaryMobileOperations.ValidBoundaryLingTongEquipmentSelection(frame),
                SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget => _coreTeamActivityProgramsOperations.IsExactPublicTeamTargetSelection(frame, executedSelection, plan),
                SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash => _characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(frame),
                SkillProgramEffectOp.RequestLegalSlashByNearest or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => _coreNearestLegalSlashRequestsOperations.IsValidNearestSlashProgramSelection(frame, executedSelection, plan.Instructions),
                SkillProgramEffectOp.SelectIssuedFixedRecipientWithDeathReturn => _coreOwnedDeathBenefitReturnsOperations.ExactOwnedDeathBenefitReturn(frame) && _coreFixedRecipientBenefitsOperations.ValidFixedRecipientReceipt(frame),
                SkillProgramEffectOp.SelectEquipmentPairAndPayment => _coreEquipmentPairPaymentsOperations.IsValidEquipmentPairProgramSelection(frame, awaitingCurrentSelection),
                SkillProgramEffectOp.SelectDyingOwnedCard => _coreDyingOwnedCardSelectionsOperations.IsValidDyingOwnedCardProgramSelection(frame, awaitingCurrentSelection),
                SkillProgramEffectOp.SelectRelativeZoneDemandTarget => frame.SelectedTargetSeats.Count == 1 || awaitingCurrentSelection && frame.SelectedTargetSeats.Count == 0,
                SkillProgramEffectOp.OfferCompletedCardGift => frame.SelectedTargetSeats.Count == 0 && frame.CompletedCardGiftDraft?.RecipientSeat is null || _coreCompletedCardGiftsOperations.IsValidCompletedGiftTargetSelection(frame),
                SkillProgramEffectOp.SelectTarget => frame.SelectedTargetSeats.Count == 1 || awaitingCurrentSelection && frame.SelectedTargetSeats.Count == 0,
                SkillProgramEffectOp.DamageFarthestCharacter => frame.SelectedTargetSeats.Count == 1 || awaitingCurrentSelection && frame.SelectedTargetSeats.Count == 0,
                SkillProgramEffectOp.SelectTargets => awaitingCurrentSelection && frame.SelectedTargetSeats.Count == 0 || frame.SelectedTargetSeats.Count >= executedSelection.MinimumTargets && frame.SelectedTargetSeats.Count <= executedSelection.MaximumTargets && frame.SelectedTargetSeats.Distinct().Count() == frame.SelectedTargetSeats.Count,
                SkillProgramEffectOp.SelectChainedByMarker => frame.SelectedTargetSeats.Count <= _coreMatchParticipantsPlayersAccess._players.Count && frame.SelectedTargetSeats.Distinct().Count() == frame.SelectedTargetSeats.Count,
                SkillProgramEffectOp.SelectOneSelectedTarget => frame.SelectedTargetSeats.Count <= 1 || awaitingCurrentSelection,
                _ => false
            };
            if (!dynamicTargetsValid)
                throw new InvalidOperationException("An active skill program has an invalid cursor or selection.");
            if (frame.ChoiceBindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != frame.ChoiceBindings.Count)
                throw new InvalidOperationException("An active program contains duplicate named choices.");
            foreach (var binding in frame.ChoiceBindings)
            {
                if (_coreProgramChoiceProofsOperations.HasDeclaredProgramChoiceProof(frame, plan, binding))
                    continue;
                if (_characterBuLianShiMobileOperations.IsMobileBuLianShiAnxuEqualityResult(frame, binding))
                    continue;
                if (_characterCaoZhenBoundaryMobileOperations.IsBoundarySidiChoiceResult(frame, binding))
                    continue;
                if (_characterGuYongBoundaryMobileOperations.IsBoundaryGuYongChoiceResult(frame, binding))
                    continue;
                if (_characterSunLuBanBoundaryMobileOperations.IsBoundarySunLuBanChoiceResult(frame, binding))
                    continue;
                if (_characterZhangNiBoundaryMobileOperations.IsBoundaryWurongChoiceResult(frame, binding))
                    continue;
                if (_characterCaoPiBoundaryMobileOperations.IsBoundaryCaoPiChoiceResult(frame, binding))
                    continue;
                if (_characterManChongBoundaryMobileOperations.IsBoundaryManChongChoiceResult(frame, binding))
                    continue;
                if (_coreNearestLegalSlashRequestsOperations.IsNearestFactionChoiceResult(frame, binding))
                    continue;
                if (_coreShenwuNpcProgramsOperations.IsShenwuChoiceResult(frame, binding))
                    continue;
                if (_coreHellKingProgramsOperations.IsHellKingChoiceResult(frame, binding))
                    continue;
                if (_coreGuanduGeneralNativeCardsOperations.IsGuanduChoiceResult(frame, binding))
                    continue;
                if (_characterHuaXiongMouMobileOperations.IsMouHuaXiongChoiceResult(frame, binding))
                    continue;
                if (_characterZhuZhiAnniversaryOperations.IsAnniversaryZhuZhiChoiceResult(frame, binding))
                    continue;
                if (_characterLeiBoAnniversaryOperations.IsAnniversaryLeiBoFactionChoiceResult(frame, binding))
                    continue;
                if (_characterChenQunMobileShiOperations.IsMobileShiChenQunFaenChoiceResult(frame, binding))
                    continue;
                if (_characterWangChangMobileShiOperations.IsMobileShiWangChangChoiceResult(frame, binding))
                    continue;
                if (_characterChenDaoMobileShiOperations.IsMobileShiChenDaoChoiceResult(frame, binding))
                    continue;
                if (_characterChengPuMobileShiOperations.IsMobileShiChengPuChoiceResult(frame, binding))
                    continue;
                if (_characterLuYuShengMobileShiOperations.IsMobileShiLuYuShengChoiceResult(frame, binding))
                    continue;
                var producer = plan.Instructions.Take(frame.InstructionIndex).SingleOrDefault(effect => (effect.Op is SkillProgramEffectOp.AccumulatePaidPhaseGift or SkillProgramEffectOp.SelectRelativeZoneDemandTarget or SkillProgramEffectOp.ObtainDeckCardWithConsecutiveTarget or SkillProgramEffectOp.ChooseDifferentActionCategoryGift or SkillProgramEffectOp.ChooseOption or SkillProgramEffectOp.ChooseDifferentCategoryDiscard or SkillProgramEffectOp.RequestSlashByTarget or SkillProgramEffectOp.RequestSlashAgainstChosenTarget or SkillProgramEffectOp.RevealSelectedHandAgainstTarget) && effect.ResultBind == binding.Name || effect.Op == SkillProgramEffectOp.OfferCompletedCardGift && binding.Name == "completed-card-gift-slash" && _coreCompletedCardGiftsOperations.IsValidCompletedGiftTargetSelection(frame) || effect.Op == SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner && binding.Name == "fixed-target-slash" && frame.FixedTargetSlash is { Issued: true });
                var validOption = producer?.Op switch
                {
                    SkillProgramEffectOp.AccumulatePaidPhaseGift => binding.OptionId is "crossed" or "not-crossed",
                    SkillProgramEffectOp.SelectRelativeZoneDemandTarget => binding.OptionId is "hand" or "equipment",
                    SkillProgramEffectOp.ObtainDeckCardWithConsecutiveTarget => binding.OptionId is "repeat" or "new",
                    SkillProgramEffectOp.ChooseDifferentActionCategoryGift => binding.OptionId is "gifted" or "declined",
                    SkillProgramEffectOp.ChooseOption => producer.Options.Any(option => option.Id == binding.OptionId),
                    SkillProgramEffectOp.ChooseDifferentCategoryDiscard => binding.OptionId is ChooseDifferentCategoryDiscardProgramOperationDescriptor.DiscardedOption or ChooseDifferentCategoryDiscardProgramOperationDescriptor.DeclinedOption,
                    SkillProgramEffectOp.RequestSlashByTarget => binding.OptionId is RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption or RequestSlashByTargetProgramOperationDescriptor.DeclinedOption,
                    SkillProgramEffectOp.RequestSlashAgainstChosenTarget => binding.OptionId is "used-slash" or "declined",
                    SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner => binding.OptionId is "used-slash" or "declined",
                    SkillProgramEffectOp.OfferCompletedCardGift => binding.OptionId is "used-slash" or "declined",
                    SkillProgramEffectOp.RevealSelectedHandAgainstTarget => binding.OptionId is "damage" or "obtain" or "none",
                    _ => false
                };
                var chooserSeat = producer?.ChooserRef is { } producerChooser ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, producerChooser) : producer?.Op == SkillProgramEffectOp.OfferCompletedCardGift ? frame.CompletedCardGiftDraft!.RecipientSeat!.Value : producer is null ? -1 : global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(frame, producer.Target);
                if (producer is null || !validOption || binding.ChooserSeat != chooserSeat)
                    throw new InvalidOperationException("An active program choice does not match its committed producer.");
            }

            var paused = plan.Instructions[frame.InstructionIndex - 1];
            _coreOwnedExactCountAndNamedTargetsOperations.AssertProgramNamedBoundTargetSelection(frame, paused);
            if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()))
                _coreProgramCompoundCardTargetsOperations.AssertProgramCompoundCardTarget(frame, paused);
            _coreDistinctFactionDiscardsOperations.AssertDistinctFactionDiscardDraft(frame, paused);
            _coreParticipantAndReserveProgramsOperations.AssertParticipantReserveDraft(frame, paused);
            _characterLiuHongOlOperations.AssertYujuePending(frame, paused);
            _coreLiejieSourceDiscardsOperations.AssertLiejieSourceDiscard(frame, paused);
            _corePublicPersistentPilesOperations.AssertPublicPileDraft(frame, paused);
            _coreHandComparisonOperations.AssertHandComparison(frame, paused);
            _coreDeckProgramsOperations.AssertDeckProgramDrafts(frame, paused);
            _coreFixedSlashAndDeclaredDeckProgramsOperations.AssertFixedSlashAndDeclaredDeckDrafts(frame, paused);
            _characterZhugeLiangMouOperations.AssertMouZhijueProgram(frame, paused);
            _coreCappedHandRefreshProgramsOperations.AssertCappedHandRefresh(frame, paused);
            _coreAppliedDamageBenefitObserversOperations.AssertAppliedDamageBenefitReceipt(frame);
            _coreFireTargetBenefitObserversOperations.AssertFireTargetBenefit(frame);
            _coreNamedCardAcquisitionsOperations.AssertNamedCardAcquisition(frame);
            _coreProgramLostHpChainedPaymentsOperations.AssertLostHpChainedPayment(frame);
            _coreProgramTargetCommitOperations.AssertProgramTargetDraft(frame, paused);
            _coreProgramDiscardChallengesOperations.ValidateProgramDiscardChallengeState(frame);
            _corePrepDiscardReceiptsOperations.AssertPrepDiscardReceipts(frame);
            _coreSlashTargetBenefitObserversOperations.AssertSlashTargetBenefitProgram(frame);
            _coreSequentialDiscardReceiptsOperations.AssertSequentialDiscard(frame);
            _coreHalfHandPhaseDebtReceiptsOperations.AssertHalfHandPhaseDebt(frame);
            _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizure(frame);
            _coreProgramDiscardTopPlacementOperations.AssertProgramDiscardTopPlacement(frame);
            _coreStrategicProgramsOperations.AssertStrategicProgramSelection(frame, paused);
            _coreConfiguredConversionsOperations.AssertConfiguredCardDeclaration(frame, paused);
            _coreDeferredCardsOperations.AssertParticipantTopView(frame, paused);
            _coreActionCategoryGiftsOperations.AssertDifferentActionCategoryGift(frame, paused);
            _coreAlternatingSuitAndCompletedTopOperations.AssertAlternatingSuitTop(frame, paused);
            _coreSlashSuitDiscardOperations.AssertProgramSlashSuitDiscard(frame, paused);
            _coreFinalTargetTurnCountsOperations.AssertNamedTurnFlow(frame, paused);
            _coreQuotaTopProgramsOperations.AssertQuotaTop(frame, paused);
            _coreDiamondDelayedProgramsOperations.AssertDiamondDelayed(frame, paused);
            _coreConvertingGiftsOperations.AssertConvertingGift(frame, paused);
            _coreFinalTargetGiftsOperations.AssertFinalTargetGift(frame, paused);
            _coreNamedDefenseAndPublicDraftOperations.AssertNamedDefenseAndPublicDraft(frame, paused);
            _coreResponseEntityExchangeOperations.AssertResponseEntityExchange(frame, paused);
            _coreProgramOwnedCardsOperations.AssertProgramOwnedCardSelection(frame, paused);
            _coreProgramHoldCardsOperations.AssertProgramHoldCardSelection(frame, paused);
            _coreProgramRevealsOperations.AssertProgramRevealCardSelection(frame, paused);
            _coreProgramOwnedCardDistributionsOperations.AssertProgramOwnedCardDistribution(frame, paused);
            _coreProgramAttackRangeAidOperations.AssertProgramAttackRangeAid(frame, paused);
            _coreDiscardedProvenanceOperations.AssertProvenanceClaim(frame);
            _coreProvenanceVirtualAlcoholOperations.AssertProvenanceAlcohol(frame);
            _coreAdjacentDiscardStorageOperations.AssertAdjacentDiscardStorage(frame);
            _coreCompletedUsePaymentsOperations.AssertCompletedUsePayment(frame);
            _coreRoundPricedPileAlcoholOperations.AssertRoundPileAlcohol(frame);
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFireDraft(frame);
            _coreVirtualBasicOffersOperations.AssertVirtualBasicDraft(frame, paused);
            _coreZhaoguMirrorOperations.AssertZhaoguMirror(frame, paused);
            _coreProgramVirtualSlashOffersOperations.AssertProgramVirtualSlashOffer(frame, paused);
            if (!frame.CardSetBindings.Any(binding => binding.Name == paused.ResultBind))
                _coreTriggeredCardProgramsOperations.AssertProgramEquipmentColorDiscardChoice(frame, paused);
            _coreAssistedPhysicalCardsOperations.AssertAssistedPhysicalCardDrafts(frame, paused);
            _coreNearestLegalSlashRequestsOperations.AssertNearestLegalSlashState(frame, paused);
            _coreParticipantHandProgramsOperations.ValidateParticipantHandDrafts(frame, paused);
            _coreProgramPrivateDamageOfferOperationsOperations.AssertPrivateOfferDrafts(frame, paused);
            _coreDiscardBudgetAndFactionGiftsOperations.AssertBudgetGiftDraft(frame, paused);
            _coreRedDiscardRecoveryOperations.AssertRedDiscardRecoveryDraft(frame, paused);
            _coreCompletedCardGiftsOperations.AssertCompletedCardGiftDraft(frame, paused);
            _coreCurrentCardEnhancementsOperations.AssertCurrentCardEnhancementDraft(frame, paused);
            _coreOriginalTargetAdditionsOperations.AssertOriginalTargetAdditionDraft(frame, paused);
            _coreDeferredBenefitsProgramsOperations.ValidateFactionRecoveryDraft(frame);
            _coreDeferredBenefitsProgramsOperations.ValidateWeaponDamageDraft(frame);
            _coreProgramHandControlOperations.AssertProgramHandControlDraft(frame, paused);
            _coreRevealRecastProgramsOperations.AssertPairedHandRevealChoice(frame, paused);
            if (paused.Op == SkillProgramEffectOp.ChooseOption && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && !frame.ChoiceBindings.Any(binding => binding.Name == paused.ResultBind) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } choiceDecision || choiceDecision.PlayerSeat != (paused.ChooserRef is { } chooser ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, chooser) : global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(frame, paused.Target)) || choiceDecision.Choices.Count == 0 || choiceDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") != "choose-option" || choice.Parameters.GetValueOrDefault("result-bind") != paused.ResultBind || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))))
                throw new InvalidOperationException("An unanswered program choice lost its matching prompt.");
            if (paused.Op == SkillProgramEffectOp.ChooseDifferentCategoryDiscard && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && !frame.ChoiceBindings.Any(binding => binding.Name == paused.ResultBind) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } categoryDecision || categoryDecision.PlayerSeat != _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, paused.ChooserRef!) || categoryDecision.Choices.Count == 0 || categoryDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action")is not ("different-category-discard" or "different-category-decline") || choice.Parameters.GetValueOrDefault("result-bind") != paused.ResultBind || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))))
                throw new InvalidOperationException("An unanswered category challenge lost its matching prompt.");
            if (paused.Op == SkillProgramEffectOp.UseAllHandCardsAsOrdinaryTrick && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } trickDecision || trickDecision.PlayerSeat != frame.OwnerSeat || trickDecision.Choices.Count == 0 || trickDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") != "use-all-hand-as-ordinary-trick" || choice.Parameters.GetValueOrDefault("view-as-id") != paused.SourceBind || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))))
                throw new InvalidOperationException("An unanswered ordinary-trick conversion lost its matching prompt.");
            if (frame.TriggerId is { } triggerId)
            {
                var trigger = plan.Trigger;
                if (trigger is null || frame.WindowContext is not { } context || context.OwnerSeat != frame.OwnerSeat || context.Window != trigger.Window || frame.ActivationId != triggerId || frame.SelectedCardIds.Count != 0 || executedSelection is null && frame.SelectedTargetSeats.Count != 0 && !(trigger.DeferredTurnEndOnly && _coreDeferredHandAlignmentsOperations.IsExactDeferredChild(frame)) && !_coreShenwuNpcProgramsOperations.HasExactShenwuBoundParticipantSelection(frame) && !_coreShenwuExpandedPoliciesOperations.HasExactShenwuYanyuParticipant(frame) && !_coreHellKingProgramsOperations.HasExactHellKingParticipantSelection(frame) && !_characterLuMengHappyGodOperations.HasExactHappyGongxinParticipantSelection(frame) && !_characterHuaTuoHappyGodOperations.HasExactHappyHuaTuoParticipantSelection(frame) && !_characterDianWeiHappyGodOperations.HasExactHappyDianWeiElaiSelection(frame) && !_characterYangBiaoAnniversaryOperations.HasExactAnniversaryYangBiaoParticipantSelection(frame) && !_characterZhangXunAnniversaryOperations.HasExactAnniversaryZhangXunFollowupSelection(frame) && !_characterXiahouShangMobileShiOperations.HasExactMobileShiTanfengParticipantSelection(frame) && !_characterZhuZhiBoundaryMobileOperations.HasExactBoundaryAnguoRescueTarget(frame) && !_characterZhangRenMobileShiOperations.HasExactShiZrFuanStrikeTarget(frame) && !_coreNativeDrawThresholdOperations.HasExactNativeDrawThresholdSelection(frame))
                    throw new InvalidOperationException("An active trigger program has an invalid cursor or context.");
                continue;
            }

            var activation = plan.Activation;
            if (activation is null || frame.SelectedCardIds.Count < activation.MinCards && !_characterLuKangWumiaoOperations.HasWuLuKangPindianSelection(frame, activation) || frame.SelectedCardIds.Count > activation.MaxCards || frame.WangYunPlan is not { CostPaid: true } && frame.FriendEventFlow is null && frame.MatchingRecast is null && frame.PhaseHandSeizure is null && frame.SourceCurseReceipt is null && frame.DiamondDelayed is null && frame.BlackGiftContest is null && frame.YangHong is null && !_characterQianZhaoOlOperations.HasPaidWeifuInitialSelection(frame) && !_characterHanFuOlOperations.HasPaidShuziInitialSelection(frame) && !_characterLiuYanOlOperations.HasPaidLiuYanInitialSelection(frame) && !_characterDaQiaoLeOperations.HasPaidZixiPlacement(frame) && !_characterXiaoQiaoLeOperations.HasPaidWeiwanSelection(frame) && !_characterZhouFeiLeOperations.HasPaidLeZhouFeiXianshuSelection(frame) && !_characterXiahouShiMouMobileOperations.HasPaidMouXiahouShiSelection(frame) && !_characterChenQunBoundaryMobileOperations.HasPaidBoundaryChenQunInitialSelection(frame) && !_coreProgramRuntimeHooksOperations.RuntimeHooks.CanContinue(ProgramRuntimePhase.PaidInitialSelection, frame) && frame.SelectedCardIds.Any(id => !CanSelectProgramActivationCard(plan, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id), frame.OwnerSeat, frame.SkillId)) || activation.CardCountExpression == SkillProgramCardCountExpression.NextPhaseActivationOrdinal && frame.SelectedCardIds.Count != _coreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault((frame.OwnerSeat, frame.SkillId, activation.UsageGroup)) || activation.CardCountExpression == SkillProgramCardCountExpression.OwnerHpAtLeastOne && !_characterZhaoYunOlGod2016Operations.HpScaledProgramSelectionIsExact(frame) || executedSelection is null && (frame.SelectedTargetSeats.Count < activation.MinTargets || frame.SelectedTargetSeats.Count > activation.MaxTargets) && !_coreNextActualUseTargetAdjustmentsOperations.HasExactNextActualUseProgramSelection(frame, plan) && !_coreHpLossSlashMaterialsOperations.HasExactHpLossSlashSelection(frame, plan) && !_coreKuangfuReceiptsOperations.HasExactKuangfuProgramSelection(frame, plan))
                throw new InvalidOperationException("An active skill program has an invalid cursor or selection.");
        }

        foreach (var scopeId in _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Select(batch => batch.AwaitingProgramFrameId).Concat(_resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(window => window.Batch.AwaitingProgramFrameId)).OfType<long>())
        {
            if (frames.Count(frame => frame.Id == scopeId && IsAwaitingProgramMovement(frame)) != 1)
                throw new InvalidOperationException("A scoped card-movement batch lost its waiting program frame.");
        }

        for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not CardsMovedTriggerWindowFrame { Batch.AwaitingProgramFrameId: { } scopeId })
                continue;
            if (!_resolutionStackOperations._resolutionStack.Take(index).OfType<ProgramSkillFrame>().Any(frame => frame.Id == scopeId && IsAwaitingProgramMovement(frame)))
                throw new InvalidOperationException("A scoped movement window lost its waiting ancestor.");
        }

        foreach (var dying in _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Where(dying => dying.ResumesProgramSkill && (_coreDyingCursorRuntimeOperations.ActiveDying?.FrameId == dying.Id || (_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(dying) || _coreRecipientCategoryMarkContinuationsOperations.IsOriginalDyingSuspendedByRecipientCategoryMark(dying) || _characterZhangFeiWumiaoOperations.IsOriginalDyingSuspendedByWuZishengMovement(dying)))))
        {
            var parentIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(item => item is ProgramSkillFrame program && program.Id == dying.ParentFrameId);
            if (parentIndex < 0 || parentIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[parentIndex + 1] is not DyingFrame child || child.Id != dying.FrameId || child.ParentFrameId != dying.ParentFrameId || !dying.ResumesProgramSkill || dying.DamageFrameId is not null)
                throw new InvalidOperationException("A program dying continuation is missing its parent program frame.");
        }
    }

    internal static PromptChoice CreateProgramPlayChoice(LegalAction action) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SkillPrograms.CreateProgramPlayChoice(action);
}
