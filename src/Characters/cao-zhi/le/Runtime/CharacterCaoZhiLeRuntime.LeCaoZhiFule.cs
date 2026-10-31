#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiLeRuntime
{
    internal const string LeCaoZhiFuleSkill = "le:fule";
    internal const string FuleInitialBinding = "mark-initial-entities";
    internal bool HasLeCaoZhiFuleCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeLeCaoZhiFule);

    internal static CardConversionSource FuleSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cao_zhi_le_LeCaoZhiFule.FuleSource(f);
    internal FuEntityNameSnapshot? CaptureLeCaoZhiFuEntity(int cardId)
    {
        var completed = _coreProgramEventHistoryOperations.ProgramEventHistory<FuMarkingCompletedEvent>().Select(e => (e.FrameId, e.Origin)).ToHashSet();
        var mark = _coreProgramEventHistoryOperations.ProgramEventHistory<FuEntityAssignedEvent>().LastOrDefault(e => e.CardId == cardId && completed.Contains((e.FrameId, e.Origin)));
        return mark is null ? null : new(mark.CardId, mark.PrintedKind, mark.AliasKind, mark.Revision);
    }

    internal bool IsFuleHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, LeCaoZhiFuleSkill) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && CaptureLeCaoZhiFuEntity(cardId)is not null;
    // Registration only; the query still belongs to legacy GameEngine.LeCaoZhiFule.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 190)]
    internal void ProjectFuleCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(FulePublicLabel(cardId));
    }

    internal string? FulePublicLabel(int cardId) => CaptureLeCaoZhiFuEntity(cardId)is null ? null : "赋";
    internal string? FuleHandLabel(CharacterState owner, Card card) => CaptureLeCaoZhiFuEntity(card.Id)is { } fu ? fu.AliasKind is { } alias ? $"赋·{(CardCatalog.Get(alias).DisplayName)}" : "赋" : null;
    [ProgramPipelineHook(ProgramPipelinePhase.HandCardProjection, 10)]
    internal CardSnapshot ProjectFuleHandCard(CharacterState owner, Card card, CardSnapshot snapshot)
    {
        var label = FuleHandLabel(owner, card);
        return label is null ? snapshot : snapshot with
        {
            SkillMarkLabel = snapshot.SkillMarkLabel is { } existing ? existing.Replace("赋", label, StringComparison.Ordinal) : label
        };
    }

    internal CardKind[] FuleModeNames() => LeCaoZhiModeNamePool();
    internal ProgramLifecycleTriggerWindowFrame FreezeFuleInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeCaoZhiFuleCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.FuleInitialEntities is not null)
            throw new InvalidOperationException("Fule cannot recapture its original opening Hand.");
        var owners = window.Candidates.Where(c => c.SkillId == LeCaoZhiFuleSkill && c.BindingId == FuleInitialBinding && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.InitializeLeCaoZhiFule }]).Select(c => c.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(card =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == card.Id && move.To == CardLocation.Hand(owner));
            if (card.Id <= 0 || entry is null || entry.CardKind != card.Kind)
                throw new InvalidOperationException("Fule needs each original physical opening-Hand entry.");
            return new FuleInitialEntity(owner, card.Id, card.Kind, entry.Sequence);
        })).ToArray();
        return window with
        {
            FuleInitialEntities = entities
        };
    }

    internal bool IsIssuedFuleInitialCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == LeCaoZhiFuleSkill && candidate.BindingId == FuleInitialBinding && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is { } program && program.GameplayHash == candidate.GameplayHash && LeCaoZhiComposition.IsFuleProgram(program) && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == candidate.OwnerSeat && _coreActualTurnProgressPhaseAccess._phase == TurnPhase.NotStarted && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, FuleInitialEntities: not null } window && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate && context.SourceSeat == window.OwnerSeat && context.TargetSeat == window.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<FuleInitializedEvent>().Any(done => done.Source.OwnerSeat == candidate.OwnerSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29080, SkillProgramEffectOp.InitializeLeCaoZhiFule)]
    internal bool CanOfferFuleInitial(ProgramTriggerLegalityContext gate) => IsIssuedFuleInitialCandidate(gate.Candidate, gate.Context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29080)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29080)]
    internal bool PermitIssuedFuleInitial(ProgramSkillFrame f)
    {
        if (f.SkillId != LeCaoZhiFuleSkill || f.TriggerId != FuleInitialBinding || f.ActivationId != f.TriggerId || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.GameStarting) || !_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(f, SkillProgramTriggerWindow.GameStarting))
            return false;
        return IsIssuedFuleInitialCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) || f.InstructionIndex == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FuleInitializedEvent>().Count(done => done.FrameId == f.Id && done.Source == FuleSource(f) && done.GameplayHash == f.GameplayHash) == 1;
    }

    internal SkillProgramStepOutcome InitializeLeCaoZhiFule(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || !PermitIssuedFuleInitial(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<FuleInitializedEvent>().Any(done => done.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Fule requires its once-issued original opening-Hand mother.");
        var window = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(frame => frame.Id == f.WindowContext!.ParentFrameId);
        var entities = window.FuleInitialEntities!.Where(entity => entity.OwnerSeat == f.OwnerSeat).ToArray();
        if (entities.Select(entity => entity.CardId).Distinct().Count() != entities.Length || entities.Any(entity => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == entity.EntrySequence && move.CardId == entity.CardId && move.CardKind == entity.PrintedKind && move.To == CardLocation.Hand(f.OwnerSeat))))
            throw new InvalidOperationException("Fule cannot replace its frozen opening entity set with a later Hand.");
        AssignFuleEntities(f, entities.Select(entity => entity.CardId).ToArray(), FuMarkOrigin.OpeningHand);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuleInitializedEvent(f.Id, FuleSource(f), f.GameplayHash, window.Id, entities.Length));
        return SkillProgramStepOutcome.Continue;
    }

    internal void MarkWenlanFuEntities(ProgramSkillFrame supplied, IReadOnlyList<int> ids, FuMarkOrigin origin)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.SkillId != "le:wenlan" || f.GameplayHash != supplied.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || origin is not (FuMarkOrigin.WenlanObtain or FuMarkOrigin.WenlanHandRewrite) || !IsExactWenlanFuMarking(f, ids, origin) || ids.Distinct().Count() != ids.Count || ids.Any(id => id <= 0 || (origin == FuMarkOrigin.WenlanObtain ? _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat) : !CoreNativeCardClassificationsRuntime.IsOwnedPlayableLocation(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _cardZonesOperations._cardZones.GetLocation(id)))))
            throw new InvalidOperationException("Fu marking requires Wenlan's exact paid acquire or confirmed private Hand receipt.");
        AssignFuleEntities(f, ids, origin);
    }

    internal void AssignFuleEntities(ProgramSkillFrame f, IReadOnlyList<int> ids, FuMarkOrigin origin)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<FuMarkingCompletedEvent>().Any(done => done.FrameId == f.Id && done.Origin == origin) || _coreProgramEventHistoryOperations.ProgramEventHistory<FuEntityAssignedEvent>().Any(mark => mark.FrameId == f.Id && mark.Origin == origin))
            throw new InvalidOperationException("A Fu batch may assign its actual entities only once.");
        var pool = FuleModeNames();
        for (var index = 0; index < ids.Count; index++)
        {
            var id = ids[index];
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var old = CaptureLeCaoZhiFuEntity(id);
            if (old is not null && old.PrintedKind != card.Kind)
                throw new InvalidOperationException("A Fu entity cannot change its printed face.");
            var candidates = pool.Where(kind => kind != card.Kind && kind != old?.AliasKind).ToArray();
            var before = _randomOperations._random.State;
            var alias = old is not null && origin != FuMarkOrigin.WenlanHandRewrite || candidates.Length == 0 ? old?.AliasKind : candidates[_randomOperations._random.Next(candidates.Length)];
            var changed = old is null || old.AliasKind != alias;
            var revision = changed ? ++_coreResolutionIdentityBoundaryOperations._resolutionSequence : old!.Revision;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuEntityAssignedEvent(f.Id, FuleSource(f), f.GameplayHash, origin, index, id, card.Kind, old?.AliasKind, alias, old?.Revision ?? 0, revision, before, _randomOperations._random.State, changed));
        }

        // All scalar updates become observable together after this completion.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FuMarkingCompletedEvent(f.Id, origin, ids.Count));
    }

    internal bool CanUseFuleAlias(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse, int? requestedUseTarget)
    {
        if (instance.SkillId != LeCaoZhiFuleSkill)
            return true;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(instance.SkillId).Program;
        if (!owner.IsAlive || program is null || !LeCaoZhiComposition.IsFuleProgram(program) || FulePureProviderFrontier(owner.Seat) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, instance.SkillId, instance.SkillInstanceId) || _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(program).ViewAs.SingleOrDefault(view => view.Id == rule.Id) != rule || CaptureLeCaoZhiFuEntity(card.Id)is not { AliasKind: { } alias } || alias != rule.OutputKind || !CoreNativeCardClassificationsRuntime.IsOwnedPlayableLocation(owner, _cardZonesOperations._cardZones.GetLocation(card.Id)))
            return false;
        if (requestedUseTarget is { } requested)
            return rule.OutputKind == CardKind.Peach && !forResponse && !dyingUse && _coreWenyiPeachSuppliesOperations.IsWenyiPeachSupplyQuery(owner, requested);
        if (dyingUse || forResponse && rule.OutputKind is CardKind.Peach or CardKind.Alcohol)
            return _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == owner.Seat && (rule.OutputKind == CardKind.Peach || rule.OutputKind == CardKind.Alcohol && dying.VictimSeat == owner.Seat);
        if (!forResponse)
            return true;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind))
            return FuleTrueSlashUseFrontier(owner);
        return rule.OutputKind == CardKind.Nullification || rule.OutputKind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, CardKind.Dodge);
    }

    internal bool IsFuleAliasSource(CardConversionSource? source) => source is { SkillId: LeCaoZhiFuleSkill } && _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program is { } p && LeCaoZhiComposition.IsFuleProgram(p) && _corePlayCardHistoryOperations.ViewAsRule(source)is not null;
    internal bool FulePureProviderFrontier(int actor) => _coreProgramNativeSlashQueriesOperations.IsNativePureProviderFrontier(actor);
    internal bool FuleTrueSlashUseFrontier(CharacterState owner) => _coreProgramNativeSlashQueriesOperations.IsNativeForcedSlashUseFrontier(owner);
    internal bool FuleSlashAliasAllowed(CharacterState owner, bool forResponse) => FuleTrueSlashUseFrontier(owner);
    internal IReadOnlyList<SlashUseVariant> FulePhysicalSlashVariants(CharacterState owner, Card card) => CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(kind => IsFulePhysicalSlashKind(owner, card, kind)).Select(kind => new SlashUseVariant(kind, null, false)).ToArray();
    internal bool IsFulePhysicalSlashKind(CharacterState owner, Card card, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, kind, false).Any(source => IsFuleAliasSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind);
    internal bool FuleNativeConversionMatchesOutput(CharacterState owner, CardConversionSource source, CardKind kind) => !IsFuleAliasSource(source) || !FuleTrueSlashUseFrontier(owner) || _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind;
    internal bool FuleAllowsUnspecifiedForcedSlash(CharacterState owner, Card card, CardKind kind) => !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || !FuleTrueSlashUseFrontier(owner) || CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, kind) || FulePhysicalSlashVariants(owner, card).Count == 0;
    internal CardKind FuleSelectedSlashKind(CharacterState owner, Card card, CardKind fallback, CardConversionSource? explicitSource = null)
    {
        var source = explicitSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        return source is not null && IsFuleAliasSource(source) && _corePlayCardHistoryOperations.ViewAsRule(source)is { } rule && CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, rule.OutputKind, false).Contains(source) ? rule.OutputKind : fallback;
    }

    internal CardKind FuleForcedAiSlashKind(PendingDecision decision, Card card, CardKind fallback)
    {
        var choices = decision.Choices.Where(choice => choice.Cards is [var id] && id == card.Id).ToArray();
        if (choices.Any(choice => choice.Parameters.GetValueOrDefault("response-card-kind") == fallback.ToString()))
            return fallback;
        var alias = choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("conversion-skill-id") == LeCaoZhiFuleSkill && Enum.TryParse<CardKind>(choice.Parameters.GetValueOrDefault("response-card-kind"), out var kind) && CoreNativeCardClassificationsRuntime.IsSlashCard(kind));
        return alias is null ? fallback : Enum.Parse<CardKind>(alias.Parameters["response-card-kind"]);
    }

    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 29080)]
    internal void AddFuleMissingNativeActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        foreach (var card in playable.Where(card => selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId))
        {
            if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card))
                continue;
            foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false).Where(source => source.SkillId == LeCaoZhiFuleSkill && _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind == kind))
                {
                    actions.RemoveAll(action => action.CardId == card.Id && action.ConversionSource == source);
                    foreach (var variant in _coreProgramCardUseRulesOperations.GetSlashUseVariants(actor, card, kind, source))
                    {
                        var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: variant.EffectiveKind);
                        IReadOnlyList<CardConversionSource>? additional = variant.AdditionalConversionSource is { } chained ? [chained] : null;
                        var name = CardCatalog.Get(variant.EffectiveKind).DisplayName;
                        foreach (var target in targets)
                            actions.Add(new LegalAction(LegalActionKind.Slash, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"以【{(name)}】使用【{(card.DisplayName)}】"), variant.EffectiveKind) { ConversionSource = source, AdditionalConversionSources = additional });
                        _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, card, targets, name, variant.EffectiveKind, source, additional);
                        _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, name, variant.EffectiveKind, source, additional);
                    }
                }

            if (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.WoodenOxGrain(actor.Seat))
                AddFuleGrainUseActions(actions, actor, card);
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Lightning, false).Where(source => source.SkillId == LeCaoZhiFuleSkill))
                if (!actor.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(actor, CardKind.Lightning) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Lightning, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, CardKind.Lightning, [actor.Seat]))
                    actions.Add(new LegalAction(LegalActionKind.Lightning, card.Id, actor.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"以【闪电】使用【{(card.DisplayName)}】"), CardKind.Lightning) { ConversionSource = source });
        }
    }

    internal void AddFuleGrainUseActions(List<LegalAction> actions, CharacterState actor, Card card)
    {
        if (_coreTreatedWoundedOperations.IsRuleWounded(actor))
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, CardKind.Peach, false).Where(IsFuleAliasSource))
                actions.Add(new(LegalActionKind.Peach, card.Id, actor.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"以【桃】使用【{(card.DisplayName)}】"), CardKind.Peach) { ConversionSource = source });
        foreach (var kind in new[]
        {
            CardKind.Indulgence,
            CardKind.SupplyShortage
        }

        )
            foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false).Where(IsFuleAliasSource))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != actor.Seat && !target.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(target, kind) && (kind != CardKind.SupplyShortage || _characterLuKangWumiaoOperations.HasWuLuKangDixianMaterialPrivilege(actor, kind, [card.Id], source) || _characterGuanYuRuntimeOperations.GuanFirstRedDistance(actor, card) || _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor, card) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, target, kind, excludedEquipmentId: card.Id) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, kind) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance(actor, card) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, target.Seat) <= _coreRuleQueriesOperations.GetCardUseDistanceLimit(actor, kind))))
                    actions.Add(new(kind == CardKind.Indulgence ? LegalActionKind.Indulgence : LegalActionKind.SupplyShortage, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"以【{(CardCatalog.Get(kind).DisplayName)}】使用【{(card.DisplayName)}】"), kind) { ConversionSource = source });
    }

    internal IEnumerable<Card> FuleDyingAlcoholGrainCards(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(owner).Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.WoodenOxGrain(owner.Seat) && _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, CardKind.Alcohol, true, dyingUse: true).Any(IsFuleAliasSource));
    SkillProgramStepOutcome ILeCaoZhiFuleHost.Initialize(ProgramSkillFrame frame) => InitializeLeCaoZhiFule(frame);
    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 30)]
    internal bool FuleConversionUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseFuleAlias(query.Owner, query.Card, query.Instance, query.Rule, query.ForResponse, query.DyingUse, query.RequestedUseTarget);
}
