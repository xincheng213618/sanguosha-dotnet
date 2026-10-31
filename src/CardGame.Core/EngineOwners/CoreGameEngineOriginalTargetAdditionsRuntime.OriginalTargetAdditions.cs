#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOriginalTargetAdditionsRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 20)]
    internal bool OriginalTargetAdditionFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferOriginalTargetAddition);
    internal void GrantOriginalTargetAddition(ProgramSkillFrame frame, int targetSeat)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || targetSeat == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat))
            throw new InvalidOperationException("An original-target grant requires its actual turn actor and a different target.");
        var grant = new OriginalTargetAdditionGrant(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), targetSeat, frame.GameplayHash);
        var old = _coreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants.SingleOrDefault(g => g.ParentFrameId == frame.Id && g.EffectIndex == grant.EffectIndex);
        if (old is not null)
        {
            if (old != grant)
                throw new InvalidOperationException("An original-target grant key changed its meaning.");
            return;
        }

        _coreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants.Add(grant);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalTargetAdditionGrantedEvent(grant));
    }

    internal void IssueOriginalTargetAdditionPolicy(long id, CardActionContext? action)
    {
        if (action is not { Type: CardActionType.Use } || action.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && CardUseCategoryCatalog.Get(action.EffectiveKind) != CardUseCategories.InstantTrick)
            return;
        var grants = _coreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants.Where(g => g.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && g.Source.OwnerSeat == action.ActorSeat && _coreMatchParticipantsPlayersAccess._players[g.TargetSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], g.Source.SkillId, g.Source.SkillInstanceId)).Select(g => g with { IssuedDirectedEffects = _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies.Where(p => p.TurnNumber == g.TurnNumber && p.TurnSeat == g.TurnSeat && p.ActorSeat == action.ActorSeat && p.TargetSeat == g.TargetSeat && p.Source.SkillId == g.Source.SkillId && p.Source.SkillInstanceId == g.Source.SkillInstanceId && p.Source.OwnerSeat == g.Source.OwnerSeat && (p.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(p.CardKinds, action.EffectiveKind))).Aggregate(DirectedTurnCardPolicyEffect.None, (flags, p) => flags | p.Effects) }).ToArray();
        if (grants.Length == 0)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { OriginalTargetAddition = new(Array.AsReadOnly(grants)), UnlimitedUse = use.UnlimitedUse || CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && action.TargetSeats.Count > 0 && action.TargetSeats.All(target => grants.Any(g => g.TargetSeat == target && g.IssuedDirectedEffects.HasFlag(DirectedTurnCardPolicyEffect.BypassSlashLimit))) });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalTargetAdditionIssuedEvent(id, action.ActionId, grant));
    }

    internal bool HasIssuedOriginalTargetArmorBypass(long id, int targetSeat) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == id)is { Action: { } action, OriginalTargetAddition: { } receipt } && action.TargetSeats.Contains(targetSeat) && receipt.Grants.Any(g => g.Source.OwnerSeat == action.ActorSeat && g.TargetSeat == targetSeat && g.IssuedDirectedEffects.HasFlag(DirectedTurnCardPolicyEffect.IgnoreArmor));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 1)]
    internal void CollectIssuedOriginalTargetCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { OriginalTargetAddition: { Added: false } receipt } use || use.Action?.ActionId != action.ActionId || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        foreach (var grant in receipt.Grants.DistinctBy(g => (g.Source.SkillId, g.Source.SkillInstanceId)))
        {
            if (_contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program is not { } program || program.GameplayHash != grant.GameplayHash)
                continue;
            foreach (var trigger in program.Triggers.Where(t => t.Window == window && t.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor && t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferOriginalTargetAddition)))
            {
                if (result.Any(c => c.OwnerSeat == action.ActorSeat && c.SkillId == grant.Source.SkillId && c.SkillInstanceId == grant.Source.SkillInstanceId && c.TriggerId == trigger.Id))
                    continue;
                var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, action.ActorSeat, action.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action));
                result.Add(new(action.ActorSeat, action.ActorSeat, grant.Source.SkillId, trigger.Id, program.GameplayHash, grant.Source.SkillInstanceId, trigger.Priority, context));
            }
        }
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 10)]
    internal bool HasIssuedOriginalTargetCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } card } || card.ActorSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != card.ParentCardUseFrameId || window.Action.ActionId != card.CardActionId || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == card.ParentCardUseFrameId)is not { OriginalTargetAddition: { } receipt } use || use.Action is not { Type: CardActionType.Use } currentAction || currentAction.ActionId != card.CardActionId || currentAction.ActorSeat != candidate.OwnerSeat || use.SourceSeat != currentAction.ActorSeat || window.Action.ActorSeat != candidate.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        if (current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash || _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) != context)
            return false;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program;
        return program?.GameplayHash == candidate.GameplayHash && program.Triggers.Any(t => t.Id == candidate.BindingId && t.Window == context.Window && t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferOriginalTargetAddition)) && receipt.Grants.Any(g => g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId && g.GameplayHash == candidate.GameplayHash);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 720)]
    internal bool CanContinueIssuedOriginalTargetAddition(ProgramSkillFrame frame) => frame.TriggerId is { } binding && frame.WindowContext is { } context && HasIssuedOriginalTargetCandidate(new(frame.OwnerSeat, frame.SkillId, binding, frame.SkillInstanceId, frame.GameplayHash, 0), context);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 810)]
    internal bool CanOfferOriginalTargetAddition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferOriginalTargetAddition) || HasIssuedOriginalTargetCandidate(candidate, context) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(f => f.Id == context.CardUse!.ParentCardUseFrameId).OriginalTargetAddition is { Added: false };
    internal int[] OriginalTargetAdditionTargets(ProgramSkillFrame frame)
    {
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        if (use.OriginalTargetAddition is not { Added: false } receipt || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return[];
        return receipt.Grants.Where(g => g.Source.OwnerSeat == frame.OwnerSeat && g.Source.SkillId == frame.SkillId && g.Source.SkillInstanceId == frame.SkillInstanceId && g.GameplayHash == frame.GameplayHash && CanAddIssuedOriginalTarget(use, g)).Select(g => g.TargetSeat).Distinct().Order().ToArray();
    }

    internal bool CanAddIssuedOriginalTarget(CardUseFrame use, OriginalTargetAdditionGrant grant)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != grant.Source.OwnerSeat || use.SourceSeat != action.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(grant.TargetSeat))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[grant.TargetSeat];
        if (!target.IsAlive || action.EffectiveDesignatedTargetSeats.Contains(target.Seat) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, use.CardKind, use.Id) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, action.EffectiveSuit))
            return false;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new Card(-1, use.CardKind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0), effectiveKind: use.CardKind, ignoreDistance: grant.IssuedDirectedEffects.HasFlag(DirectedTurnCardPolicyEffect.IgnoreDistance), existingUseFrameId: use.Id, specificEffectiveRank: action.EffectiveRank);
        return use.CardKind switch
        {
            CardKind.DrawTwo or CardKind.IronChain => true,
            CardKind.StarShift or CardKind.HandRedistribution => _coreWenheCardsOperations.CanBeWenheAdditionalTarget(use, target),
            CardKind.Intoxication or CardKind.StoreGrain or CardKind.Reinforcements => _coreGuanduCardsOperations.CanBeGuanduAdditionalTarget(use, target),
            CardKind.SameBoat or CardKind.AgainstCurrent or CardKind.Zong => _coreDragonBoatCardsOperations.CanBeDragonBoatAdditionalTarget(use, target),
            CardKind.BorrowedSword => _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Any(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == EquipmentSlot.Weapon) && _coreMatchParticipantsPlayersAccess._players.Any(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, victim)),
            CardKind.Duel => target.Seat != actor.Seat,
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.Dismantlement or CardKind.Snatch => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0,
            _ => false
        };
    }

    internal bool HasIssuedOriginalTargetAdditionTail(CardUseFrame use) => use.OriginalTargetAddition is { Added: true } receipt && use.Action is { Type: CardActionType.Use } action && use.SourceSeat == action.ActorSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && action.EffectiveKind == use.CardKind && receipt.Grants.Any(g => g.Source.OwnerSeat == action.ActorSeat && action.EffectiveDesignatedTargetSeats.Contains(g.TargetSeat));
    internal bool TryGetOriginalTargetVirtualSlashReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> primaryTarget)
    {
        primaryTarget = Array.Empty<int>();
        if (_characterQianZhaoOlOperations.TryGetWeifuVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_characterMaWanOlOperations.TryGetMaWanVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_characterZhangYanOlOperations.TryGetZhangYanVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_characterHuaManOlOperations.TryGetSouyingCancelledVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_coreDesignatedExtraTargetsOperations.TryGetDesignatedExtraTargetVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_coreShortRangeSlashTargetsOperations.TryGetShortRangeVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_coreForeignContestAidObserversOperations.TryGetSameTypeAidVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_coreCurrentSlashFireOffersOperations.TryGetCurrentSlashFirePrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (_coreCurrentEnhancementVirtualReturnOperations.TryGetCurrentEnhancementVirtualPrimaryReturn(use, parent, out primaryTarget))
            return true;
        if (!HasIssuedOriginalTargetAdditionTail(use) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.CardAttack is not { CardId: null, EffectiveCardKind: CardKind.Slash, PhysicalCardIds.Count: 0 } attack || attack.SourceSeat != use.SourceSeat || attack.ProgramSkillCardUseFrameId != parent.Id || use.TargetSeats.Count < 2 || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action || action.ActorSeat != use.SourceSeat || action.ProviderSeat != action.ActorSeat || action.PhysicalCards.Count != 0 || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || !action.TargetSeats.SequenceEqual(use.TargetSeats) || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || string.IsNullOrWhiteSpace(parent.SkillId) || string.IsNullOrWhiteSpace(parent.SkillInstanceId) || string.IsNullOrWhiteSpace(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent)) || parent.InstructionIndex <= 0)
            return false;
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (useIndex <= 0 || !ReferenceEquals(_resolutionStackOperations._resolutionStack[useIndex - 1], parent) || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declared = history.OfType<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id);
        var targets = history.OfType<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id);
        var accepted = history.OfType<CardActionAcceptedEvent>().Select(e => e.Action).FirstOrDefault(original => original.ActionId == action.ActionId && original.Type == CardActionType.Use);
        if (declared is not { CardId: 0, CardKind: CardKind.Slash } || declared.SourceSeat != action.ActorSeat || targets is not { TargetSeats.Count: 1 } || accepted is null || accepted.ActorSeat != action.ActorSeat || accepted.ProviderSeat != action.ActorSeat || accepted.EffectiveKind != CardKind.Slash || accepted.PhysicalCards.Count != 0 || accepted.TargetSeats.Count != 1 || accepted.TargetSeats[0] != use.TargetSeats[0] || !accepted.ConversionChain.SequenceEqual(action.ConversionChain))
            return false;
        var additionalTarget = use.TargetSeats[^1];
        if (!use.OriginalTargetAddition!.Grants.Any(grant => grant.Source.OwnerSeat == action.ActorSeat && grant.TargetSeat == additionalTarget && history.OfType<OriginalTargetAdditionIssuedEvent>().Any(issued => issued.CardUseFrameId == use.Id && issued.ActionId == action.ActionId && issued.Grant == grant)) || !history.OfType<OriginalTargetAdditionResolvedEvent>().Any(resolved => resolved.CardUseFrameId == use.Id && resolved.ActionId == action.ActionId && resolved.ActorSeat == action.ActorSeat && resolved.Added && resolved.TargetSeat == additionalTarget))
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect;
        var originalTarget = targets.TargetSeats[0];
        var exactProducer = paused.Op switch
        {
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats is [var actor] && actor == action.ActorSeat && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == action.ActorSeat && action.ConversionChain.Count == 1 && action.ConversionChain[0] == new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) && (paused.TargetReference is null ? parent.SelectedTargetSeats is [var selected] && selected == originalTarget : paused.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == originalTarget),
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == action.ActorSeat && parent.SelectedTargetSeats is [var selected] && selected == originalTarget && action.ConversionChain.Count == 0 && paused.UseCardActionWindows && paused.OutputKind == CardKind.Slash && paused.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget && (parent.TriggerId is not null || paused.Op == SkillProgramEffectOp.OfferUnlimitedVirtualSlash),
            _ => false
        };
        if (!exactProducer)
            return false;
        // Use the primary post-redirection target, exactly as the old single
        // target path does. The extra target must not overwrite its producer.
        primaryTarget = Array.AsReadOnly(new[] { use.TargetSeats[0] });
        return true;
    }

    internal int CompletedFangtianOriginalTargetIndex(NativeOwners.FangtianHalberdHandle multi)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(multi.ResolutionId);
        // The old group cursor counts completed original targets. Enhanced
        // targets use a last-resolved index, so resume at original count - 1.
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && multi.TargetSeats.Count > 0 && use is not null && (((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || _characterMaWanOlOperations.HasMaWanHunjiangTargetTail(use) || (_characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(use) || _characterMaXiuMaTieOlOperations.HasKenshangTargetTail(use))) || _coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || _coreShortRangeSlashTargetsOperations.HasShortRangeSlashTail(use) || (_coreDesignatedExtraTargetsOperations.HasDesignatedExtraTargetTail(use) || _coreRecipientCategorySlashTargetsOperations.HasRecipientCategorySlashTargetTail(use))) && multi.SourceSeat == use.SourceSeat && multi.EffectiveCardKind == use.CardKind && use.TargetSeats.Count > multi.TargetSeats.Count)
            return multi.TargetSeats.Count - 1;
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && multi.TargetSeats.Count > 0 && use is not null && HasIssuedOriginalTargetAdditionTail(use) && multi.SourceSeat == use.SourceSeat && multi.EffectiveCardKind == use.CardKind && use.TargetSeats.Count > multi.TargetSeats.Count && use.TargetSeats.Skip(multi.TargetSeats.Count).Any(target => use.OriginalTargetAddition!.Grants.Any(g => g.Source.OwnerSeat == use.SourceSeat && g.TargetSeat == target)))
            return multi.TargetSeats.Count - 1;
        return multi.TargetSeats.Count;
    }

    internal IReadOnlyList<PromptChoice> OriginalTargetAdditionChoices(ProgramSkillFrame frame)
    {
        Dictionary<string, string> Parameters() => new()
        {
            ["program-action"] = "original-target-addition",
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        var choices = OriginalTargetAdditionTargets(frame).SelectMany(target => use.CardKind == CardKind.BorrowedSword ? _coreMatchParticipantsPlayersAccess._players.Where(v => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[target], v)).Select(v => new PromptChoice(new ChoiceId($"original-target.{(frame.Id)}.{(target)}.{(v.Seat)}"), $"额外令 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 对 {(v.Name)} 使用杀", [], [target, v.Seat], Parameters())) : new[] { new PromptChoice(new ChoiceId($"original-target.{(frame.Id)}.{(target)}"), $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[target].Name)}", [], [target], Parameters()) }).ToList();
        choices.Add(new(new ChoiceId($"original-target.{(frame.Id)}.decline"), "不追加目标", [], [], Parameters()));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal SkillProgramStepOutcome OfferOriginalTargetAddition(ProgramSkillFrame frame)
    {
        if (OriginalTargetAdditionTargets(frame).Length == 0)
            return SkillProgramStepOutcome.Continue;
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        // Draw Two's actual implicit self target must survive adding an opponent.
        // Recasting and other targetless actions are never normalized here.
        if (use.CardKind == CardKind.DrawTwo && use.Action!.EffectiveDesignatedTargetSeats.Count == 0)
        {
            var targets = Array.AsReadOnly(new[] { use.SourceSeat });
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, old => old with { TargetSeats = targets, Action = _coreProgramCardUseRolesOperations.CloneRoleAction(old.Action!, old.SourceSeat, targets) });
            _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, use.Action!);
            use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OriginalTargetAdditionDraft = new(use.Id, use.Action!.ActionId) });
        var choices = OriginalTargetAdditionChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "可额外指定本回合拼点获胜的原对象。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("original-target-addition")]
    internal void ResolveOriginalTargetAdditionChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.Last() as ProgramSkillFrame ?? throw new InvalidOperationException("Original-target program is absent.");
        choice = OriginalTargetAdditionChoices(frame).SingleOrDefault(c => c.Id == choice.Id) ?? throw new InvalidOperationException("Original-target choice is no longer legal.");
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        var action = use.Action!;
        if (frame.OriginalTargetAdditionDraft != new OriginalTargetAdditionDraft(use.Id, action.ActionId) || ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.Op != SkillProgramEffectOp.OfferOriginalTargetAddition || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Original-target payment and program context changed.");
        if (choice.Targets.Count > 0)
        {
            var targets = Array.AsReadOnly(use.TargetSeats.Concat(choice.Targets).ToArray());
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, old => old with { TargetSeats = targets, Action = _coreProgramCardUseRolesOperations.CloneRoleAction(action, action.ActorSeat, targets), OriginalTargetAddition = old.OriginalTargetAddition!with { Added = true }, Enhancements = old.Enhancements | CurrentCardEnhancement.ExtraTarget, TargetsAdjusted = old.TargetsAdjusted || old.CardKind == CardKind.BorrowedSword, EnhancementOwnerSeat = frame.OwnerSeat });
            _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, action);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalTargetAdditionResolvedEvent(use.Id, action.ActionId, frame.OwnerSeat, choice.Targets.FirstOrDefault(-1), choice.Targets.Count > 0));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OriginalTargetAdditionDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferOriginalTargetAddition)]
    internal PromptChoice SelectAiOriginalTargetAddition(PendingDecision decision, ProgramSkillFrame frame)
    {
        // Use the existing public role attitude; hidden hands never enter this decision.
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(frame);
        var helpful = use.CardKind is CardKind.DrawTwo or CardKind.IronChain;
        return decision.Choices.Where(c => c.Targets.Count > 0).FirstOrDefault(c => helpful == CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }

    internal void ExpireOriginalTargetAdditionGrants(int turn, int seat) => _coreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess._originalTargetAdditionGrants.RemoveAll(g => g.TurnNumber == turn && g.TurnSeat == seat);
    void IOriginalTargetAdditionProgramHost.GrantOriginalTargetAddition(ProgramSkillFrame frame, int targetSeat) => GrantOriginalTargetAddition(frame, targetSeat);
    SkillProgramStepOutcome IOriginalTargetAdditionProgramHost.OfferOriginalTargetAddition(ProgramSkillFrame frame) => OfferOriginalTargetAddition(frame);
}
