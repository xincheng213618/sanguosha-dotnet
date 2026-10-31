#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConfiguredConversionsRuntime
{
    internal IReadOnlyDictionary<string, int>? GetConfiguredConversionTierSnapshot(CharacterState owner)
    {
        var result = _coreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._configuredConversionTiers.Where(item => item.Key.Seat == owner.Seat).ToDictionary(item => item.Key.State, item => item.Value, StringComparer.Ordinal);
        return result.Count == 0 ? null : result;
    }

    internal bool IsConfiguredConversionAvailable(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool dyingUse, int? requestedUseTarget = null)
    {
        if (!_coreLastZoneConversionsOperations.MatchesLastSourceZoneConversion(owner, card, rule) || !_coreOwnPublicPileConversionsOperations.MatchesOwnPublicPileConversion(owner, card, instance, rule))
            return false;
        if (rule.ExcludeOwnerEffects && (requestedUseTarget is { } requested ? requested == owner.Seat : dyingUse ? _coreDyingCursorRuntimeOperations.Value.ActiveDying?.VictimSeat == owner.Seat : rule.OutputKind is CardKind.Peach or CardKind.Alcohol))
            return false;
        if (rule.ActivationUsageGroup is { } group && rule.UsesPerPhase is { } limit && _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, instance.SkillId, group)) >= limit)
            return false;
        if (rule.ConversionStateId is not { } state)
            return true;
        var tier = _coreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._configuredConversionTiers.GetValueOrDefault((owner.Seat, state));
        if (!rule.DeclaredEntity)
            return tier >= rule.MinimumTier && tier <= rule.MaximumTier;
        return _coreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._declaredConversionCards.TryGetValue((owner.Seat, state), out var declared) && declared.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && declared.Card == card.Id && declared.Kind == rule.OutputKind && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner.Seat);
    }

    internal SkillProgramStepOutcome BeginConfiguredCardDeclaration(ProgramSkillFrame frame, string bind, string state)
    {
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (binding.CardIds.Count != 1 || binding.Visibility != SkillProgramCardSetVisibility.Public || binding.SourceLocations.Single() != CardLocation.Hand(frame.OwnerSeat) || _cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("Declaring requires the publicly revealed real owner hand card.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var tier = _coreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._configuredConversionTiers.GetValueOrDefault((frame.OwnerSeat, state));
        if (effect.SelfDeclarationFromTier is { } selfTier && tier >= selfTier)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardDeclarationDraft = new(state, bind, binding.CardIds[0], tier, frame.OwnerSeat) { NearestSeats = [frame.OwnerSeat] } });
            PublishConfiguredCardDeclaration(frame);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var others = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat).ToArray();
        if (others.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var distance = others.Min(player => _coreNativeRuleQueriesOperations.GetCombatDistance(frame.OwnerSeat, player.Seat));
        var nearest = others.Where(player => _coreNativeRuleQueriesOperations.GetCombatDistance(frame.OwnerSeat, player.Seat) == distance).Select(player => player.Seat).Order().ToArray();
        if (tier >= 2)
            throw new InvalidOperationException("The direct tier does not reveal or declare a card.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardDeclarationDraft = new(state, bind, binding.CardIds[0], tier) { NearestSeats = nearest } });
        PublishConfiguredCardDeclaration(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<CardKind> DeclarationNames(ProgramSkillFrame frame, ProgramCardDeclarationDraft draft) => _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).ViewAs.Where(rule => rule.ConversionStateId == draft.StateId && rule.DeclaredEntity && rule.MinimumTier <= draft.Tier && rule.MaximumTier >= draft.Tier).Select(rule => rule.OutputKind).Distinct().Order().ToArray();
    internal void PublishConfiguredCardDeclaration(ProgramSkillFrame frame)
    {
        var draft = frame.CardDeclarationDraft ?? throw new InvalidOperationException("A declaration lost its draft.");
        var seat = draft.DeclarerSeat ?? frame.OwnerSeat;
        PromptChoice Choice(string key, string label, IReadOnlyList<int> targets) => new(new ChoiceId($"declaration.{(frame.Id)}.{(seat)}.{(key)}"), label, [], targets, new Dictionary<string, string> { ["program-action"] = "configured-card-declaration", ["frame-id"] = frame.Id.ToString(), ["choice"] = key });
        var choices = draft.DeclarerSeat is null ? draft.NearestSeats.Select(target => Choice($"seat-{(target)}", $"请 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 声明牌名", [target])).ToArray() : DeclarationNames(frame, draft).Select(kind => Choice(kind.ToString(), $"声明【{(CardCatalog.Get(kind).DisplayName)}】", [])).ToArray();
        if (choices.Length == 0)
            throw new InvalidOperationException("A declaration has no configured legal names.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, seat, draft.DeclarerSeat is null ? "选择距离最近的其他角色。" : "为公开展示的手牌声明牌名。", [], draft.DeclarerSeat is null ? draft.NearestSeats : [], SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 声明", skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("configured-card-declaration")]
    internal void ResolveConfiguredCardDeclaration(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A declaration lost its frame.");
        var draft = frame.CardDeclarationDraft ?? throw new InvalidOperationException("A declaration lost its draft.");
        if (choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString() || _cardZonesOperations._cardZones.GetLocation(draft.CardId) != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("A declaration is stale.");
        if (draft.DeclarerSeat is null)
        {
            var target = choice.Targets.Single();
            if (!draft.NearestSeats.Contains(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("The declarer is not a frozen nearest other character.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CardDeclarationDraft = draft with { DeclarerSeat = target } });
            PublishConfiguredCardDeclaration(frame);
            return;
        }

        if (!Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("choice"), out var kind) || !DeclarationNames(frame, draft).Contains(kind))
            throw new InvalidOperationException("The declared name is not available at the frozen tier.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._declaredConversionCards[(frame.OwnerSeat, draft.StateId)] = (_coreActualTurnProgressTurnNumberAccess._turnNumber, draft.CardId, kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhysicalCardNameDeclaredEvent(frame.OwnerSeat, draft.DeclarerSeat.Value, draft.StateId, draft.CardId, kind, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[draft.DeclarerSeat.Value].Name)} 为 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 展示的牌声明【{(CardCatalog.Get(kind).DisplayName)}】。", draft.DeclarerSeat.Value, frame.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { CardDeclarationDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 60)]
    internal bool ConfiguredConversionUnavailable(ProgramConfiguredViewAsQuery query) => !IsConfiguredConversionAvailable(query.Owner, query.Card, query.Instance, query.Rule, query.DyingUse, query.RequestedUseTarget);
}
