#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSameTypeActualUseAidRuntime
{
    internal const string SameTypeAidGiftReason = "program.same-type-actual-use-aid.gift";
    internal bool HasSameTypeActualUseAid => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget);

    internal static bool IsSameTypeAidEffectiveCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_SameTypeActualUseAid.IsSameTypeAidEffectiveCard(kind);
    internal static bool IsSameTypeAidCard(CardKind paid, CardKind effective) => RuntimeSkills.RuntimeRules_Core_SameTypeActualUseAid.IsSameTypeAidCard(paid, effective);
    internal CardConversionSource SameTypeAidSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SameTypeAidIdentity? FreezeSameTypeAidUse(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (!HasSameTypeActualUseAid)
            return null;
        if (context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.Id == context.ParentFrameId && CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) == context.Window && window.Action is { Type: CardActionType.Use } action && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { } use && use.Action?.ActionId == action.ActionId && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind && use.TargetSeats.Contains(ownerSeat) && action.TargetSeats.Contains(ownerSeat) && IsSameTypeAidEffectiveCard(action.EffectiveKind))
            return new(use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, ownerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        // Mature virtual Shensu has no Action. Only this true producer enters the
        // second trigger; physical/action-bearing uses are served by Finalized.
        if (context.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && context.ActualUseTarget is { ActionId: null, LegacyProducerProgramId: { } producer } legacy && legacy.TargetSeat == ownerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(legacy.EffectiveKind) && _coreActualUseTargetProgramsOperations.MatchesActualUseTarget(legacy))
            return new(legacy.CardUseFrameId, null, legacy.ActorSeat, legacy.ProviderSeat, legacy.EffectiveKind, ownerSeat, legacy.ActualTurnNumber, legacy.ActualTurnOwnerSeat, producer);
        return null;
    }

    internal bool MatchesSameTypeAidUse(SameTypeAidIdentity identity)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is not { } use || use.SourceSeat != identity.ActorSeat || use.CardKind != identity.EffectiveKind || !use.TargetSeats.Contains(identity.OriginalOwnerTarget) || identity.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || identity.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !IsSameTypeAidEffectiveCard(identity.EffectiveKind))
            return false;
        if (identity.CardActionId is { } actionId)
            return identity.LegacyProducerProgramId is null && use.Action is { Type: CardActionType.Use } action && action.ActionId == actionId && action.ActorSeat == identity.ActorSeat && action.ProviderSeat == identity.ProviderSeat && action.EffectiveKind == identity.EffectiveKind && action.TargetSeats.Contains(identity.OriginalOwnerTarget);
        return identity.LegacyProducerProgramId is { } producer && use.Action is null && (_coreDamageJudgmentSuitPaymentsOperations.LegacyDamageJudgmentVirtualProducer(use, identity.OriginalOwnerTarget) == producer || HasSameTypeAidTargetTail(use) && MatchesSameTypeAidLegacyProducer(use, producer));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 710)]
    internal bool CanRunSameTypeAid(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget))
            return true;
        return _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp > 0 && FreezeSameTypeAidUse(candidate.OwnerSeat, context)is { } identity && CharacterFuHuangHouClassicRuntime.MatchesClassicQiuyuanAidContext(candidate.SkillId, candidate.OwnerSeat, context, identity) && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(identity.CardUseFrameId, candidate.OwnerSeat) && SameTypeAidRecipients(identity, candidate.SkillId).Length > 0;
    }

    internal bool CanAddSameTypeAidTarget(SameTypeAidIdentity identity, int seat, bool ignoreSlashDistance = false)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is not { } use || !MatchesSameTypeAidUse(identity) || use.TargetSeats.Contains(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || seat == identity.ActorSeat || _coreCardUseModulesOperations.HasTurnCardTargetRestriction(identity.ActorSeat, SkillProgramCardTargetRestriction.SelfOnly) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(identity.ActorSeat, seat, identity.EffectiveKind, use.Id))
            return false;
        var action = use.Action;
        var suit = action?.EffectiveSuit ?? Suit.None;
        var color = action is null ? null : CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action);
        var actor = _coreMatchParticipantsPlayersAccess._players[identity.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, identity.EffectiveKind, suit, color) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, seat, suit))
            return false;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(identity.EffectiveKind))
        {
            if (action is null)
                return !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target); // exact legacy producer already uses unlimited distance
            var materials = action.PhysicalCards.Select(c => c.CardId).ToArray();
            var card = new Card(0, identity.EffectiveKind, suit, action.EffectiveRank ?? 0);
            return !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, identity.EffectiveKind, CardActionType.Use, ignoreIssuedPlayBan: true) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) && (ignoreSlashDistance || SameTypeAidIssuedSlashIgnoresDistance(use) || _coreIssuedPlayUsePoliciesOperations.HasIssuedFirstPlayUseDistance(use.Id) || _coreProvenanceCardPoliciesOperations.HasIssuedProvenanceUseDistance(use.Id, actor.Seat) || _coreGrantedEntityPhasesOperations.HasIssuedGrantedPhaseEntityDistance(use.Id, actor.Seat) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, materials) || _coreGrantedEntityPhasesOperations.HasGrantedPhaseEntityDistance(actor, materials) || _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(actor, identity.EffectiveKind, action.EffectiveSuit) || _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashPolicyForColor(actor.Seat, identity.EffectiveKind, color) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(actor.Seat, action.EffectiveSuit) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, target, identity.EffectiveKind, use.Id) || action.ConversionChain.Any(source => _coreCardConversionsOperations.IgnoresProgramSlashDistance(actor, source)) || _coreCardConversionsOperations.IgnoresSpGuanYuWushengDistance(actor, card) || _coreCardConversionsOperations.IgnoresSlashUseDistance(actor) || _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(actor, target, identity.EffectiveKind, action.EffectiveRank, use.Id));
        }

        return identity.EffectiveKind switch
        {
            CardKind.Duel => true,
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            // Their mature declared targets already include all living others.
            // No duplicate target or fictitious extra attack is invented.
            CardKind.BarbarianAssault or CardKind.ArrowBarrage => false,
            _ => false
        };
    }

    internal bool SameTypeAidIssuedSlashIgnoresDistance(CardUseFrame use)
    {
        if (use.KuangfuSlashReturn is not null)
            return _coreKuangfuReceiptsOperations.HasIssuedKuangfuDistance(use.Id, use.KuangfuSlashReturn.Source.OwnerSeat);
        if (_characterDianWeiMouOperations.IsMouDianWeiSlashUse(use.Id))
            return true;
        if (use.PindianWinnerSlashReturn is { } winner)
            return _coreRecipientContestReceiptsOperations.Value.MatchesPindianWinnerUse(use, winner);
        if (use.ForeignTurnContestSlashReturn is not null)
            return true;
        if (use.CardAttack?.ProgramSkillCardUseFrameId is not { } id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is not { } parent || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash || parent.InstructionIndex < 1)
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect;
        return paused.Op is SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash && paused.OutputKind == CardKind.Slash && paused.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget;
    }

    internal int[] SameTypeAidRecipients(SameTypeAidIdentity identity, string skillId) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != identity.OriginalOwnerTarget && p.Seat != identity.ActorSeat && (!CharacterFuHuangHouClassicRuntime.IsClassicQiuyuanAid(skillId) || !_coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)!.TargetSeats.Contains(p.Seat)) && (CanAddSameTypeAidTarget(identity, p.Seat, CharacterFuHuangHouClassicRuntime.SameTypeAidIgnoresSlashDistance(skillId)) || _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count + (CharacterFuHuangHouClassicRuntime.IsClassicQiuyuanAid(skillId) ? 0 : _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count) > 0)).Select(p => p.Seat).Order().ToArray();
    internal bool SameTypeAidUnpaidSourceValid(ProgramSkillFrame f) => f.SameTypeAid is { } r && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && MatchesSameTypeAidUse(r.Use) && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(r.Use.CardUseFrameId, f.OwnerSeat) && (r.RecipientSeat is null || _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat.Value].IsAlive);
    internal SkillProgramStepOutcome BeginSameTypeAid(ProgramSkillFrame frame)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.SameTypeAid is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || FreezeSameTypeAidUse(f.OwnerSeat, context)is not { } use || !CharacterFuHuangHouClassicRuntime.MatchesClassicQiuyuanAidContext(f.SkillId, f.OwnerSeat, context, use))
            throw new InvalidOperationException("Aid requires one exact original target/use opportunity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameTypeAid = new(f.InstructionIndex, SameTypeAidSource(f), f.GameplayHash, use, SameTypeAidStage.ChoosingRecipient) });
        PublishSameTypeAidPrompt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Dictionary<string, string> SameTypeAidParameters(ProgramSkillFrame f, string option) => new()
    {
        ["program-action"] = "same-type-actual-use-aid",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["aid-option"] = option
    };
    internal IReadOnlyList<PromptChoice> SameTypeAidChoices(ProgramSkillFrame f)
    {
        var r = f.SameTypeAid!;
        if (r.Stage == SameTypeAidStage.ChoosingRecipient)
            return Array.AsReadOnly(SameTypeAidRecipients(r.Use, f.SkillId).Select(seat => new PromptChoice(new($"same-type-aid.{(f.Id)}.recipient-{(seat)}"), $"请 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 给牌或成为额外目标", [], [seat], SameTypeAidParameters(f, "recipient"))).ToArray());
        if (r.Stage != SameTypeAidStage.ChoosingAnswer || r.RecipientSeat is not { } recipient)
            return[];
        var choices = new List<PromptChoice>();
        foreach (var raw in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, recipient, recipient, CharacterFuHuangHouClassicRuntime.SameTypeAidPaymentZones(f.SkillId), OwnedCardMoveIntent.Transfer, canSelect: (_, card) => !card.IsGeneralWeapon && CharacterFuHuangHouClassicRuntime.IsSameTypeAidPaymentKind(f.SkillId, card.Kind, r.Use.EffectiveKind), destination: SkillProgramCardDestination.OwnerHand, destinationSeat: f.OwnerSeat))
        {
            var parameters = new Dictionary<string, string>(raw.Parameters);
            foreach (var pair in SameTypeAidParameters(f, "give"))
                parameters[pair.Key] = pair.Value;
            choices.Add(raw with { Parameters = parameters });
        }

        if (CanAddSameTypeAidTarget(r.Use, recipient, CharacterFuHuangHouClassicRuntime.SameTypeAidIgnoresSlashDistance(f.SkillId)))
            choices.Add(new(new($"same-type-aid.{(f.Id)}.become-target"), "成为此牌的额外目标", [], [recipient], SameTypeAidParameters(f, "target")));
        // Candidate seats use only public HE counts. A private hand without a
        // matching card must not leak its eligibility to the original chooser.
        if (choices.Count == 0)
            choices.Add(new(new($"same-type-aid.{(f.Id)}.unavailable"), "没有可交给的牌且不能成为额外目标", [], [], SameTypeAidParameters(f, "unavailable")));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSameTypeAidPrompt(ProgramSkillFrame f)
    {
        var r = f.SameTypeAid!;
        var choices = SameTypeAidChoices(f);
        if (choices.Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var chooser = r.Stage == SameTypeAidStage.ChoosingRecipient ? f.OwnerSeat : r.RecipientSeat!.Value;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == SameTypeAidStage.ChoosingRecipient ? "选择一名其他角色求援。" : CharacterFuHuangHouClassicRuntime.IsClassicQiuyuanAid(f.SkillId) ? "交给原目标一张手牌【闪】，或成为此【杀】的额外目标。" : f.SkillId == "boundary:qiuyuan-mobile" ? "交给原目标一张【杀】以外的基本牌，或成为此【杀】的额外目标。" : "交给原目标同类型不同牌名的一张牌，或成为原牌额外目标。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("same-type-actual-use-aid")]
    internal void ResolveSameTypeAidChoice(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack.Last().Id);
        _coreForeignContestAidObserversOperations.Value.AssertSameTypeAid(f);
        var r = f.SameTypeAid ?? throw new InvalidOperationException("Aid answer lost its owning receipt.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == SameTypeAidStage.ChoosingRecipient ? f.OwnerSeat : r.RecipientSeat) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices, SameTypeAidChoices(f)) || !SameTypeAidChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Aid changed its frozen chooser, payment slot or current target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!SameTypeAidUnpaidSourceValid(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var option = selected.Parameters.GetValueOrDefault("aid-option");
        if (r.Stage == SameTypeAidStage.ChoosingRecipient)
        {
            if (option != "recipient" || selected.Targets is not [var recipient] || !SameTypeAidRecipients(r.Use, f.SkillId).Contains(recipient))
                throw new InvalidOperationException("Aid requires one publicly eligible recipient.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameTypeAid = r with { RecipientSeat = recipient, Stage = SameTypeAidStage.ChoosingAnswer } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameTypeAidOfferedEvent(f.Id, r.Source, f.GameplayHash, r.Use, recipient));
            PublishSameTypeAidPrompt(f);
            return;
        }

        if (r.Stage != SameTypeAidStage.ChoosingAnswer || r.RecipientSeat is not { } donor)
            throw new InvalidOperationException("Aid is not awaiting its original recipient.");
        if (option == "unavailable")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameTypeAid = r with { Stage = SameTypeAidStage.Complete } });
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        if (option == "target")
        {
            if (!CanAddSameTypeAidTarget(r.Use, donor, CharacterFuHuangHouClassicRuntime.SameTypeAidIgnoresSlashDistance(f.SkillId)))
                throw new InvalidOperationException("Aid cannot add an illegal or duplicate target.");
            AddSameTypeAidTarget(f, donor);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SameTypeAid = r with { AddedTarget = true, Stage = SameTypeAidStage.Complete } });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        if (option != "give" || selected.Cards is not [var id] || _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: { } sourceOwner } from || sourceOwner != donor || !CharacterFuHuangHouClassicRuntime.SameTypeAidPaymentZones(f.SkillId).Contains(from.Zone))
            throw new InvalidOperationException("Aid requires one real recipient-owned HE entity.");
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        if (card.IsGeneralWeapon || !CharacterFuHuangHouClassicRuntime.IsSameTypeAidPaymentKind(f.SkillId, card.Kind, r.Use.EffectiveKind))
            throw new InvalidOperationException("Aid payment changed its exact type or card name.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameTypeAid = r with { Stage = SameTypeAidStage.GiftChildren }, PendingMovementContinuation = new(donor, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(f.OwnerSeat), new(SameTypeAidGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, donor, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            if (records is not [var moved] || moved.From != from || moved.To != CardLocation.Hand(f.OwnerSeat) || moved.CardId != id)
                throw new InvalidOperationException("Aid cannot claim payment without its real HE delivery.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { SameTypeAid = current.SameTypeAid!with { Payment = new(id, card.Kind, from, moved.Sequence, moved.Sequence, true) } });
        }));
        // Removal hooks can replace the root with queued recovery attempts.
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var paid = f.SameTypeAid!.Payment!with
        {
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last().Sequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SameTypeAid = f.SameTypeAid with { Payment = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameTypeAidGiftPaidEvent(f.Id, r.Source, r.Use, donor, paid.MovementSequence, paid.SequenceAfter, paid.Delivered));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AddSameTypeAidTarget(ProgramSkillFrame f, int target)
    {
        var r = f.SameTypeAid!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.Use.CardUseFrameId)!;
        var targets = Array.AsReadOnly(use.TargetSeats.Append(target).ToArray());
        var action = use.Action is { } original ? new CardActionContext(original.ActionId, original.ParentActionId, original.Type, original.ActorSeat, original.ProviderSeat, original.RequesterSeat, original.ResponderSeat, original.OpponentSeat, original.EffectiveKind, targets, original.PhysicalCards, original.ConversionChain, targets, original.EffectiveSuit, original.EffectiveRank, original.EffectiveIsRed, original.FactionOrigin) : null;
        if (action is not null)
        {
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action }, action);
            AppendSameTypeAidFinalizedCandidates(use.Id, action, target);
        }
        else
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { TargetSeats = targets });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameTypeAidTargetAddedEvent(f.Id, r.Source, r.Use, target));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(f.Id, f.SkillId, f.OwnerSeat, use.Id, target));
    }

    internal void AppendSameTypeAidFinalizedCandidates(long useId, CardActionContext action, int newTarget)
    {
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.ParentFrameId == useId && CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == SkillProgramTriggerWindow.CardUseTargetsFinalized);
        if (window is null)
            return;
        var additions = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, [newTarget], null, window.Continuation).Where(c => c.OwnerSeat == newTarget && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Single(t => t.Id == c.TriggerId).Effects.Any(e => e.Op == SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget) && !window.Candidates.Any(old => old.OwnerSeat == c.OwnerSeat && old.SkillId == c.SkillId && old.TriggerId == c.TriggerId && old.SkillInstanceId == c.SkillInstanceId)).Select(c => c.FrozenContext is { } context ? c with { FrozenContext = context with { ParentFrameId = window.Id, CardUse = context.CardUse!with { ParentCardUseFrameId = useId } } } : c).ToArray();
        if (additions.Length > 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Candidates = Array.AsReadOnly(window.Candidates.Concat(additions).ToArray()) });
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 560)]
    internal bool ResumeSameTypeAid(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.SameTypeAid is not { } r)
            return false;
        _coreForeignContestAidObserversOperations.Value.AssertSameTypeAid(f);
        if (r.Stage is SameTypeAidStage.ChoosingRecipient or SameTypeAidStage.ChoosingAnswer)
        {
            if (!SameTypeAidUnpaidSourceValid(f))
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            }
            else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSameTypeAidPrompt(f);
            return true;
        }

        if (r.Stage == SameTypeAidStage.GiftChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id, subjectSeat: r.RecipientSeat))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, SameTypeAid = r with { Stage = SameTypeAidStage.Complete } });
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget)]
    internal PromptChoice SelectAiSameTypeAid(PendingDecision decision, ProgramSkillFrame f)
    {
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var r = f.SameTypeAid!;
        if (r.Stage == SameTypeAidStage.ChoosingRecipient)
        {
            var harm = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
            return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(snapshot, c.Targets.Single(), harm)).ThenBy(c => c.Targets.Single()).First();
        }

        var give = decision.Choices.Where(c => c.Parameters.GetValueOrDefault("aid-option") == "give").OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat])).ThenBy(c => c.Cards.Single()).FirstOrDefault();
        var target = decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("aid-option") == "target");
        var ownerBenefit = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(snapshot, f.OwnerSeat, new(0, 0, 0, 1, 0, 0, false, false));
        return give is not null && (ownerBenefit > 0 || target is null || _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].Hp <= 1) ? give : target ?? give ?? decision.Choices.Single();
    }

    internal bool IsSameTypeAidTargetFact(CardUseFrame use, SameTypeAidTargetAddedEvent fact)
    {
        if (fact.Use.CardUseFrameId != use.Id || fact.Use.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || fact.Use.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !use.TargetSeats.Contains(fact.RecipientSeat) || fact.Use.OriginalOwnerTarget == fact.RecipientSeat || fact.Use.ActorSeat == fact.RecipientSeat || fact.Source.OwnerSeat != fact.Use.OriginalOwnerTarget || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(fact.Source.SkillId)?.Program is not { } program || ProgramInstructionResolver.Default.FindTrigger(program, fact.Source.BindingId)is not { } trigger || trigger.Effects is not [{ Op: SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget }])
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<SameTypeAidOfferedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.Source == fact.Source && e.GameplayHash == program.GameplayHash && e.Use == fact.Use && e.RecipientSeat == fact.RecipientSeat) != 1 || history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.SkillId == fact.Source.SkillId && e.SkillInstanceId == fact.Source.SkillInstanceId && e.BindingId == fact.Source.BindingId && e.OwnerSeat == fact.Source.OwnerSeat && e.Window == trigger.Window) != 1)
            return false;
        // The same ActionId survives legitimate role and color rewrites. Its
        // original material/actor is not replaced by a later mutable use actor.
        return fact.Use.CardActionId is { } actionId ? use.Action?.ActionId == actionId && history.OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == actionId && e.Action.Type == CardActionType.Use) && (use.CardKind == fact.Use.EffectiveKind || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) && CoreNativeCardClassificationsRuntime.IsSlashCard(fact.Use.EffectiveKind)) : use.Action is null && fact.Use.LegacyProducerProgramId is { } producer && MatchesSameTypeAidLegacyProducer(use, producer);
    }

    internal bool HasSameTypeAidTargetTail(CardUseFrame use) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameTypeAidTargetAddedEvent>().Any(e => IsSameTypeAidTargetFact(use, e));
    internal bool MatchesSameTypeAidLegacyProducer(CardUseFrame use, long producer)
    {
        if (use.Action is not null || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.TargetSeats.Count == 0 || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || use.CardAttack is not { IsSourceLess: false, EffectiveCardKind: CardKind.Slash, CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != producer || attack.SourceSeat != use.SourceSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != producer || parent.OwnerSeat != use.SourceSeat || parent.TriggerId is null || parent.SelectedTargetSeats is not [var original] || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Count(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([original])) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == CardKind.Slash && e.SourceSeat == use.SourceSeat) != 1)
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect;
        return paused is { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash, UseCardActionWindows: false, TargetRestriction: SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget };
    }

    internal long? SameTypeAidLegacyCurrentProducer(CardUseFrame use, int target) => use.TargetSeats.Contains(target) && use.CardAttack is { Active: true, ProgramSkillCardUseFrameId: { } producer } attack && attack.TargetSeat == target && HasSameTypeAidTargetTail(use) && MatchesSameTypeAidLegacyProducer(use, producer) ? producer : null;
}
