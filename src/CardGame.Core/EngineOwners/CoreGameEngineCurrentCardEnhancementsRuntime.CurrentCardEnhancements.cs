#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentCardEnhancementsRuntime
{
    internal void CaptureProgramAdjustedSlashBaseDamage(NativeOwners.CardAttackHandle attack)
    {
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash))
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(attack.ResolutionId, frame => frame with { ProgramAdjustedSlashBaseDamage = frame.ProgramAdjustedSlashBaseDamage ?? attack.DamageAmount });
    }

    internal bool TryContinueEnhancedSlashTargets(NativeOwners.CardAttackHandle attack)
    {
        if (!HasRemainingEnhancedSlashTargets(attack.ResolutionId) || _coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(attack.ResolutionId) || _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd?.ResolutionId == attack.ResolutionId)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId);
        if (!ReferenceEquals(use, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            throw new InvalidOperationException("An adjusted Slash must finish its child before advancing its target cursor.");
        if (use.ProgramAdjustedSlashBaseDamage is not { } baseDamage)
            throw new InvalidOperationException("An adjusted Slash lost its initial damage amount.");
        var next = use.TargetIndex + 1;
        while (next < use.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[use.TargetSeats[next]].IsAlive)
            next++;
        var targetSeat = use.TargetSeats[next];
        _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(use.Id, next);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var continued = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, targetSeat, attack.Card, baseDamage, use.CardKind, ignoresArmor: use.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(use.Id, targetSeat) || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], use.CardKind), physicalCards: attack.PhysicalCards, conversionSource: attack.ConversionSource, programSkillCardUseFrameId: attack.ProgramSkillCardUseFrameId);
        if ((((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || _characterMaWanOlOperations.HasMaWanHunjiangTargetTail(use) || (_characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(use) || _characterMaXiuMaTieOlOperations.HasKenshangTargetTail(use))) || _coreOriginalTargetAdditionsOperations.HasIssuedOriginalTargetAdditionTail(use) || _coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || _coreShortRangeSlashTargetsOperations.HasShortRangeSlashTail(use)) && use.AdjustedSlashReturn is { } adjustedReturn && adjustedReturn.CardUseFrameId == use.Id)
            _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, state => state!with { AdjustedSlashReturn = adjustedReturn, PhysicalCardIds = use.PhysicalCardIds! });
        if (use.HpLossMaterialSlashReturn is not null)
            _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, state => state!with { PhysicalCardIds = use.PhysicalCardIds! });
        continued.SetCardUseCausedDamage(attack.CardUseCausedDamage);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = continued;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { ActiveAttack: { } factionAttack } faction && CoreNativeDamageHandlesRuntime.SameAttackOwner(factionAttack, attack))
            faction.ActiveAttack = continued;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } borrowedAttack } borrowed && CoreNativeDamageHandlesRuntime.SameAttackOwner(borrowedAttack, attack))
            borrowed.ActiveAttack = continued;
        _coreNativeSlashUseOperations.BeginSlashTargetResolution(continued);
        return true;
    }

    internal CardUseFrame EnhancementCardUse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext?.CardUse is not { } context || context.ActorSeat != frame.OwnerSeat)
            throw new InvalidOperationException("An enhancement requires its owner's frozen card action.");
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Id == context.ParentCardUseFrameId && use.Action?.ActionId == context.CardActionId);
    }

    internal SkillProgramStepOutcome ApplyProgramCurrentCardEnhancements(ProgramSkillFrame frame, int maximum)
    {
        var use = EnhancementCardUse(frame);
        // The host's ordinary Draw Two path stores its implicit self target as
        // an empty input list. Normalize that actual card target before adding
        // a second target; recasting and targetless cards never enter this path.
        if (use.CardKind == CardKind.DrawTwo && use.Action!.EffectiveDesignatedTargetSeats.Count == 0)
        {
            var action = use.Action;
            var targets = new[]
            {
                use.SourceSeat
            };
            use = use with
            {
                TargetSeats = targets,
                Action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, targets, action.PhysicalCards, action.ConversionChain, targets, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, action.FactionOrigin))
            };
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[_resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == use.Id)].Id, use);
            _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, action);
        }

        if (_coreActualTurnProgressCurrentSeatAccess._currentSeat != frame.OwnerSeat || use.Action!.EffectiveDesignatedTargetSeats.Count != 1 || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && CardUseCategoryCatalog.Get(use.CardKind) != CardUseCategories.InstantTrick || _coreMatchParticipantsPlayersAccess._players.Any(other => other.IsAlive && other.Seat != frame.OwnerSeat && _coreNativeRuleQueriesOperations.GetCombatDistance(frame.OwnerSeat, other.Seat) != 1))
            return SkillProgramStepOutcome.Continue;
        if (frame.CardEnhancementDraft is not null)
            throw new InvalidOperationException("A current-card enhancement was offered twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardEnhancementDraft = new(use.Id, []) });
        PublishCardEnhancementPrompt(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] CurrentCardExtraTargets(ProgramSkillFrame frame)
    {
        var use = EnhancementCardUse(frame);
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var card = use.Action!.PhysicalCards.Select(cost => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cost.CardId)).Single(item => item.Id == cost.CardId)).FirstOrDefault();
        return _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && !use.Action!.EffectiveDesignatedTargetSeats.Contains(target.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, use.CardKind, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, card?.Suit ?? Suit.Spade, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(use.Action!)) && (CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) ? target.Seat != actor.Seat && (card is null ? !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) : _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, card, effectiveKind: use.CardKind, ignoreDistance: true)) : use.CardKind switch
        {
            CardKind.DrawTwo or CardKind.IronChain => true,
            CardKind.StarShift or CardKind.HandRedistribution => _coreWenheCardsOperations.CanBeWenheAdditionalTarget(use, target),
            CardKind.Intoxication or CardKind.StoreGrain or CardKind.Reinforcements => _coreGuanduCardsOperations.CanBeGuanduAdditionalTarget(use, target),
            CardKind.SameBoat or CardKind.AgainstCurrent or CardKind.Zong => _coreDragonBoatCardsOperations.CanBeDragonBoatAdditionalTarget(use, target),
            CardKind.BorrowedSword => _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Any(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item) == EquipmentSlot.Weapon) && _coreMatchParticipantsPlayersAccess._players.Any(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, victim)),
            CardKind.Duel => target.Seat != actor.Seat,
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.Dismantlement or CardKind.Snatch => target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0,
            _ => false
        })).Select(target => target.Seat).ToArray();
    }

    internal IReadOnlyList<PromptChoice> CardEnhancementChoices(ProgramSkillFrame frame)
    {
        var draft = frame.CardEnhancementDraft ?? throw new InvalidOperationException("Enhancement draft disappeared.");
        Dictionary<string, string> Parameters(string option) => new()
        {
            ["program-action"] = "current-card-enhancement",
            ["enhancement-option"] = option,
            ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };
        if (draft.ChoosingExtraTarget && EnhancementCardUse(frame).CardKind == CardKind.BorrowedSword)
            return CurrentCardExtraTargets(frame).SelectMany(holder => _coreMatchParticipantsPlayersAccess._players.Where(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[holder], victim)).Select(victim => new PromptChoice(new ChoiceId($"enhancement.{(frame.Id)}.pair.{(holder)}.{(victim.Seat)}"), $"额外令 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 对 {(victim.Name)} 使用杀", [], [holder, victim.Seat], Parameters("target")))).ToArray();
        if (draft.ChoosingExtraTarget)
            return CurrentCardExtraTargets(frame).Select(target => new PromptChoice(new ChoiceId($"enhancement.{(frame.Id)}.target.{(target)}"), $"额外指定 {(_coreMatchParticipantsPlayersAccess._players[target].Name)}", [], [target], Parameters("target"))).ToArray();
        var result = new List<PromptChoice>();
        var mobileBenxi = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.Op == SkillProgramEffectOp.ApplyMobileWuYiBenxi;
        foreach (var option in new[]
        {
            CurrentCardEnhancement.ExtraTarget,
            CurrentCardEnhancement.IgnoreArmor,
            CurrentCardEnhancement.Uncancelable,
            CurrentCardEnhancement.DrawAfterDamage
        }.Except(draft.Selected))
        {
            if (mobileBenxi && option != CurrentCardEnhancement.ExtraTarget)
                continue;
            if (option == CurrentCardEnhancement.ExtraTarget && CurrentCardExtraTargets(frame).Length == 0)
                continue;
            var label = option switch
            {
                CurrentCardEnhancement.ExtraTarget => "目标数+1",
                CurrentCardEnhancement.IgnoreArmor => "无视防具",
                CurrentCardEnhancement.Uncancelable => "不能被抵消",
                _ => "造成伤害后摸一张牌"
            };
            result.Add(new(new ChoiceId($"enhancement.{(frame.Id)}.{(option)}"), label, [], [], Parameters(option.ToString())));
        }

        result.Add(new(new ChoiceId($"enhancement.{(frame.Id)}.finish"), "不再选择", [], [], Parameters("finish")));
        if (!mobileBenxi)
        {
            var remaining = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.Amount - draft.Selected.Count;
            var options = result.Where(choice => choice.Parameters["enhancement-option"] != "finish").ToArray();
            for (var mask = 1; mask < (1 << options.Length); mask++)
            {
                var selected = options.Where((_, index) => (mask & (1 << index)) != 0).ToArray();
                if (selected.Length > remaining)
                    continue;
                var flags = selected.Aggregate(CurrentCardEnhancement.None, (value, choice) => value | Enum.Parse<CurrentCardEnhancement>(choice.Parameters["enhancement-option"]));
                var parameters = Parameters("confirm");
                parameters["enhancement-selection"] = ((int)flags).ToString(System.Globalization.CultureInfo.InvariantCulture);
                parameters["maximum-options"] = remaining.ToString(System.Globalization.CultureInfo.InvariantCulture);
                result.Add(new(new ChoiceId($"enhancement.{(frame.Id)}.confirm.{((int)flags)}"), "确定：" + string.Join("、", selected.Select(choice => choice.Description)), [], [], parameters));
            }
        }

        return result;
    }

    internal void PublishCardEnhancementPrompt(ProgramSkillFrame frame)
    {
        var choices = CardEnhancementChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.CardEnhancementDraft!.ChoosingExtraTarget ? "选择本牌的一个额外目标。" : "依次选择至多两项。", [], choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = frame.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("current-card-enhancement")]
    internal void ResolveCardEnhancementChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Enhancement frame disappeared.");
        selected = CardEnhancementChoices(frame).SingleOrDefault(choice => choice.Id == selected.Id) ?? throw new InvalidOperationException("Enhancement option became illegal.");
        var draft = frame.CardEnhancementDraft!;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op is not (SkillProgramEffectOp.ApplyCurrentCardEnhancements or SkillProgramEffectOp.ApplyMobileWuYiBenxi) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Enhancement lost its frozen instruction or chooser.");
        var option = selected.Parameters["enhancement-option"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (option == "finish")
        {
            FinishProgramCardEnhancements(frame);
            return;
        }

        if (option == "confirm")
        {
            var flags = (CurrentCardEnhancement)int.Parse(selected.Parameters["enhancement-selection"], System.Globalization.CultureInfo.InvariantCulture);
            foreach (var selectedEnhancement in new[]
            {
                CurrentCardEnhancement.IgnoreArmor,
                CurrentCardEnhancement.Uncancelable,
                CurrentCardEnhancement.DrawAfterDamage
            }

            )
                if ((flags & selectedEnhancement) != 0)
                    frame = ApplyCardEnhancement(frame, selectedEnhancement, []);
            if ((flags & CurrentCardEnhancement.ExtraTarget) != 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardEnhancementDraft = frame.CardEnhancementDraft!with { ChoosingExtraTarget = true, FinishAfterExtraTarget = true } });
                PublishCardEnhancementPrompt(frame);
            }
            else
                FinishProgramCardEnhancements(frame);
            return;
        }

        if (option == "ExtraTarget")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardEnhancementDraft = draft with { ChoosingExtraTarget = true } });
            PublishCardEnhancementPrompt(frame);
            return;
        }

        var enhancement = option == "target" ? CurrentCardEnhancement.ExtraTarget : Enum.Parse<CurrentCardEnhancement>(option);
        frame = ApplyCardEnhancement(frame, enhancement, option == "target" ? selected.Targets : []);
        if (frame.CardEnhancementDraft!.Selected.Count >= effect.Amount || frame.CardEnhancementDraft.FinishAfterExtraTarget)
        {
            FinishProgramCardEnhancements(frame);
            return;
        }

        PublishCardEnhancementPrompt(frame);
    }

    internal ProgramSkillFrame ApplyCardEnhancement(ProgramSkillFrame frame, CurrentCardEnhancement enhancement, IReadOnlyList<int> extraTargets)
    {
        var draft = frame.CardEnhancementDraft!;
        var use = EnhancementCardUse(frame);
        var targets = use.TargetSeats.Concat(extraTargets).ToArray();
        var action = use.Action!;
        var updatedAction = _coreProgramCardUseRolesOperations.CloneRoleAction(action, action.ActorSeat, targets);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == use.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use with { TargetSeats = targets, Action = updatedAction, Enhancements = use.Enhancements | enhancement, TargetsAdjusted = use.TargetsAdjusted || extraTargets.Count > 0 && use.CardKind == CardKind.BorrowedSword, EnhancementOwnerSeat = frame.OwnerSeat, IgnoresArmor = use.IgnoresArmor || enhancement == CurrentCardEnhancement.IgnoreArmor });
        if (extraTargets.Count > 0)
            _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, action.ActionId, frame.OwnerSeat, enhancement, extraTargets.Count > 0 ? extraTargets[0] : null));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardEnhancementDraft = draft with { Selected = draft.Selected.Append(enhancement).ToArray(), ChoosingExtraTarget = false } });
        return frame;
    }

    internal void FinishProgramCardEnhancements(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CardEnhancementDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool HasCurrentCardEnhancement(long frameId, CurrentCardEnhancement flag) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { } use && (use.Enhancements & flag) != 0;
    internal bool HasRemainingEnhancedSlashTargets(long frameId) => !_characterDaQiaoLeOperations.IsTargetScopedZixiEffectRound(frameId) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { } use && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.TargetSeats.Count > 1 && use.TargetSeats.Skip(use.TargetIndex + 1).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive);

    SkillProgramStepOutcome ICurrentCardEnhancementProgramHost.ApplyCurrentCardEnhancements(ProgramSkillFrame frame, int maximum) => ApplyProgramCurrentCardEnhancements(frame, maximum);
}
