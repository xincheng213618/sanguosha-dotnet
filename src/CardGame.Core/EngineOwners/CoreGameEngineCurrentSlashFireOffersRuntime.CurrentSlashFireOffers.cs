#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentSlashFireOffersRuntime
{
    internal bool HasCommittedSlashFireCapability(CharacterState owner) => owner.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).Any(binding => ProgramInstructionResolver.Default.Features(binding.Trigger).HasOperation(SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget));
    internal CardUseFrame CurrentSlashFireUse(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } || context.ActorSeat != f.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != context.CardActionId || action.Type != CardActionType.Use || use.SourceSeat != f.OwnerSeat || !CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(use.CardKind) || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.CardAttack is not { } attack || attack.TargetSeat != use.TargetSeats[use.TargetIndex] || attack.EffectiveCardKind != use.CardKind)
            throw new InvalidOperationException("A current Slash conversion lost its exact committed action and attack.");
        return use;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 860)]
    internal bool CanOfferCurrentSlashFire(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
            return true;
        if (context.CardUse is not { } card || card.ActorSeat != candidate.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != card.CardActionId || !CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(use.CardKind) || use.CurrentSlashFirePolicy is not null || use.CardAttack is null || use.TargetSeats.Count == 0)
            return false;
        return CurrentFireSlashOriginalTargetsAreLegal(use) && (use.CardKind != CardKind.FireSlash || CanAddCurrentFireSlashTarget(use));
    }

    internal bool CurrentFireSlashOriginalTargetsAreLegal(CardUseFrame use) => !_coreCardUseModulesOperations.IsCardUseForbidden(use.SourceSeat, CardKind.FireSlash, CardActionType.Use) && use.TargetSeats.All(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[seat]) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(use.SourceSeat, seat, use.Action!.EffectiveSuit) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(use.SourceSeat, seat, CardKind.FireSlash, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.FireSlash, use.Action!.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(use.Action!)));
    internal int CurrentFireSlashMaximum(CardUseFrame use)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var baseMaximum = _coreNativeDamageHandlesOperations.Value.UsesFormalFangtianHalberd && use.Action!.PhysicalCards.Count == 1 && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count == 0 && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.FangtianHalberd) ? 3 : 1;
        return baseMaximum + ((FiniteRuleQueryValue)_coreRuleQueriesOperations.EvaluateCardTargetCount(actor, CardKind.FireSlash).Value).Value - 1 + _coreShanheSourceLihuo3160Operations.Value.ShanheSourceLihuoProspectiveTargetBonus(use);
    }

    internal bool CanAddCurrentFireSlashTarget(CardUseFrame use) => use.TargetSeats.Count < CurrentFireSlashMaximum(use) && CurrentFireSlashExtraTargets(use).Length != 0;
    internal int[] CurrentFireSlashExtraTargets(CardUseFrame use)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var action = use.Action!;
        // A granted virtual use can carry an explicit distance exemption on its exact producer.
        var ignoreDistance = _coreKuangfuReceiptsOperations.HasIssuedKuangfuDistance(use.Id, actor.Seat) || use.UnlimitedUse && !use.QuotaOnlyUseLimitExemption || use.CardAttack?.ProgramSkillCardUseFrameId is { } parentId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == parentId)is { } parent && ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect is { Op: SkillProgramEffectOp.UseVirtualCard, TargetRestriction: SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget };
        return _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && t.Seat != actor.Seat && !use.TargetSeats.Contains(t.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t.Seat, action.EffectiveSuit) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(t) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, t.Seat, CardKind.FireSlash, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, CardKind.FireSlash, action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) && (ignoreDistance || _coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(use.Id, actor.Seat) || _coreGrantedEntityPhasesOperations.HasIssuedGrantedPhaseEntityDistance(use.Id, actor.Seat) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, use.PhysicalCardIds) || _coreGrantedEntityPhasesOperations.HasGrantedPhaseEntityDistance(actor, use.PhysicalCardIds) || _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(actor, CardKind.FireSlash, action.EffectiveSuit) || _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashPolicyForColor(actor.Seat, CardKind.FireSlash, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(actor.Seat, action.EffectiveSuit) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, t, CardKind.FireSlash, use.Id) || _coreCardConversionsOperations.IgnoresSlashUseDistance(actor) || _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(actor, t, CardKind.FireSlash, action.EffectiveRank, use.Id))).Select(t => t.Seat).ToArray();
    }

    internal SkillProgramStepOutcome BeginCurrentSlashFireOffer(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var use = CurrentSlashFireUse(f);
        if (f.CurrentSlashFireDraft is not null || use.CurrentSlashFirePolicy is not null)
            throw new InvalidOperationException("A committed Slash fire offer was issued twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CurrentSlashFireDraft = new(use.Id) });
        PublishCurrentSlashFirePrompt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CurrentSlashFireChoices(ProgramSkillFrame f)
    {
        var use = CurrentSlashFireUse(f);
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "current-slash-fire",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        var choices = new List<PromptChoice>();
        if (use.CardKind != CardKind.FireSlash && CurrentFireSlashOriginalTargetsAreLegal(use))
            choices.Add(new(new ChoiceId($"slash-fire.{(f.Id)}.convert"), "改为火杀", [], [], Parameters("convert")));
        if (CurrentFireSlashOriginalTargetsAreLegal(use) && use.TargetSeats.Count < CurrentFireSlashMaximum(use))
            choices.AddRange(CurrentFireSlashExtraTargets(use).Select(t => new PromptChoice(new ChoiceId($"slash-fire.{(f.Id)}.extra.{(t)}"), use.CardKind == CardKind.FireSlash ? $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[t].Name)}" : $"改为火杀并额外指定 {(_coreMatchParticipantsPlayersAccess._players[t].Name)}", [], [t], Parameters("extra"))));
        choices.Add(new(new ChoiceId($"slash-fire.{(f.Id)}.skip"), "保持当前杀及目标", [], [], Parameters("skip")));
        return choices;
    }

    internal void PublishCurrentSlashFirePrompt(ProgramSkillFrame f)
    {
        var choices = CurrentSlashFireChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择改为火杀及至多一个额外合法目标。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("current-slash-fire")]
    internal void ResolveCurrentSlashFireChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Slash fire producer disappeared.");
        AssertCurrentSlashFireDraft(f);
        selected = CurrentSlashFireChoices(f).SingleOrDefault(c => c.Id == selected.Id) ?? throw new InvalidOperationException("The extra Slash target became illegal.");
        var use = CurrentSlashFireUse(f);
        var branch = selected.Parameters["branch"];
        var action = use.Action!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (branch != "skip" && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            var source = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
            var converted = use.CardKind != CardKind.FireSlash;
            var targets = use.TargetSeats.Concat(selected.Targets).ToArray();
            var chain = converted ? action.ConversionChain.Append(source).ToArray() : action.ConversionChain;
            var updated = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, CardKind.FireSlash, targets, action.PhysicalCards, chain, targets, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, action.FactionOrigin));
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { CardKind = CardKind.FireSlash, TargetSeats = Array.AsReadOnly(targets), Action = updated, CurrentSlashFirePolicy = new(f.Id, source, action, use.CardKind, selected.Targets.Count == 0 ? null : selected.Targets[0], converted), CardAttack = current.CardAttack!with { EffectiveCardKind = CardKind.FireSlash, DamageNatureOverride = DamageNature.Fire, IgnoresArmor = current.CardAttack.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[current.SourceSeat], _coreMatchParticipantsPlayersAccess._players[current.CardAttack.TargetSeat], CardKind.FireSlash) }, PreparedTargetAttacks = current.PreparedTargetAttacks is null ? null : Array.AsReadOnly(current.PreparedTargetAttacks.Select(a => a with { EffectiveCardKind = CardKind.FireSlash, DamageNatureOverride = DamageNature.Fire, IgnoresArmor = a.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[current.SourceSeat], _coreMatchParticipantsPlayersAccess._players[a.TargetSeat], CardKind.FireSlash) }).ToArray()), Continuations = current.Continuations with { FangtianHalberd = current.Continuations.FangtianHalberd is not { } multi ? null : multi with { EffectiveCardKind = CardKind.FireSlash, TargetSeats = Array.AsReadOnly(targets) } } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCurrentSlashFireChangedEvent(f.Id, use.Id, action.ActionId, source, use.CardKind, CardKind.FireSlash, selected.Targets.Count == 0 ? null : selected.Targets[0]));
        // Accepted/declared history retains its original action. This new fact records the typed effective-kind change.
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CurrentSlashFireDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AssertCurrentSlashFireDraft(ProgramSkillFrame f)
    {
        if (f.CurrentSlashFireDraft is not { } draft)
            return;
        var use = CurrentSlashFireUse(f);
        var e = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (e.Op != SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget || draft.CardUseFrameId != use.Id || use.CurrentSlashFirePolicy is not null || f.ParentFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == parentId)is not { Continuation: ProgramCardContinuation.CommittedSlash } window || window.ParentFrameId != use.Id || window.Action.ActionId != use.Action!.ActionId)
            throw new InvalidOperationException("A fire conversion draft lost its exact committed Slash producer.");
    }

    internal bool TryGetCurrentSlashFirePrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> primary)
    {
        primary = [];
        if (use.CurrentSlashFirePolicy is not { OriginalKind: CardKind.Slash } policy || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || policy.OriginalAction is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } action || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)?.TargetSeats is not [var target] || use.CardAttack is not { CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id || attack.SourceSeat != use.SourceSeat || use.SourceSeat != action.ActorSeat || action.ProviderSeat != use.SourceSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || parent.InstructionIndex < 1 || string.IsNullOrWhiteSpace(parent.SkillInstanceId) || string.IsNullOrWhiteSpace(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent)))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (index < 1 || !ReferenceEquals(_resolutionStackOperations._resolutionStack[index - 1], parent) || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } definition || definition.GameplayHash != parent.GameplayHash || history.OfType<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)is not { CardId: 0, CardKind: CardKind.Slash } declared || declared.SourceSeat != use.SourceSeat)
            return false;
        if (parent.TriggerId is { } triggerId)
        {
            if (parent.WindowContext is not { } context || history.OfType<ProgramBindingStartedEvent>().SingleOrDefault(e => e.FrameId == parent.Id)is not { } started || started.SkillId != parent.SkillId || started.BindingId != triggerId || started.SkillInstanceId != parent.SkillInstanceId || started.OwnerSeat != parent.OwnerSeat || started.Window != context.Window)
                return false;
        }
        else if (!history.OfType<ProgramSkillStartedEvent>().Any(e => e.FrameId == parent.Id && e.SkillId == parent.SkillId && e.ActivationId == parent.ActivationId && e.OwnerSeat == parent.OwnerSeat))
            return false;
        // The policy and its change fact tie this frozen original ActionId to
        // the same declared use, even after the actual target cursor advances.
        AssertCurrentSlashFirePolicy(use);
        var effect = ProgramInstructionResolver.Default.Resolve(parent, definition).GetPausedInstruction(parent.InstructionIndex).Effect;
        var producer = new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId);
        var originalSelection = effect.Op switch
        {
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.SelectedTargetSeats.SequenceEqual([target]),
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats.SequenceEqual([use.SourceSeat]),
            SkillProgramEffectOp.UseVirtualSlash => effect.TargetReference is null ? parent.SelectedTargetSeats.SequenceEqual([target]) : effect.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == target,
            _ => false
        };
        var exact = effect.Op switch
        {
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == use.SourceSeat && effect.OutputKind == CardKind.Slash && effect.TargetRestriction is (SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget or SkillProgramCardTargetRestriction.NormalSlashTarget) && (action.ConversionChain.Count == 0 || action.ConversionChain.SequenceEqual([producer])),
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == use.SourceSeat && action.ConversionChain.SequenceEqual([producer]),
            _ => false
        };
        if (!exact || !originalSelection || use.ShortRangeSlashTarget is not null && !_coreShortRangeSlashTargetsOperations.HasShortRangeSlashTail(use))
            return false;
        // Extra targets and redirects belong to the child use. Its exact
        // producer resumes with the original frozen singleton selection.
        primary = Array.AsReadOnly(new[] { target });
        return true;
    }

    internal bool IsCurrentSlashFireChangedUse(CardUseFrame use) => use.CurrentSlashFirePolicy is { Converted: true } && use.CardKind == CardKind.FireSlash;
    internal void AssertCurrentSlashFirePolicy(CardUseFrame use)
    {
        if (use.CurrentSlashFirePolicy is not { } policy)
            return;
        var original = policy.OriginalAction;
        if (use.CardKind != CardKind.FireSlash || use.Action is not { EffectiveKind: CardKind.FireSlash } current ||
            original.ActionId != current.ActionId || original.ActorSeat != current.ActorSeat ||
            original.ProviderSeat != current.ProviderSeat || original.Type != current.Type ||
            original.EffectiveKind != policy.OriginalKind ||
            !CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(policy.OriginalKind) ||
            !original.PhysicalCards.SequenceEqual(current.PhysicalCards) ||
            policy.Converted != (policy.OriginalKind != CardKind.FireSlash) ||
            !current.ConversionChain.SequenceEqual(policy.Converted ? original.ConversionChain.Append(policy.Source) : original.ConversionChain))
            throw new InvalidOperationException("A changed Slash lost its original action or issued effective-kind change.");
    }
}
