#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZuMobileShiRuntime
{
    internal const string ShiHzChizhang = "mobile:shi-chizhang", ShiHzDuanyang = "mobile:shi-duanyang";
    internal static CardConversionSource ShiHzSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasShiHz => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuStore);

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 47900)]
    internal bool ShiHzFinalizedTrickWindow(ProgramTargetCardWindowQuery q) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuChizhang) && q.Category == SkillProgramCardCategory.Trick;
    SkillProgramStepOutcome IShiHuangZuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiHz(f, op);
    internal CardUseFrame? ShiHzHandDamage(ProgramSkillWindowContext c, int owner) => c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && c.CardUse is { } identity && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } a } use && use.CardKind != CardKind.Lightning && CoreGameEngineMouLuMengRuntime.IsDamageCardKind(use.CardKind) && a.ActionId == identity.CardActionId && a.ActorSeat == owner && a.ProviderSeat == owner && a.PhysicalCards.Count > 0 && a.PhysicalCards.All(m => m.From == CardLocation.Hand(owner)) ? use : null;
    internal CardMovementRecord[] ShiHzNonUseHandSlashes(ProgramSkillWindowContext? c, int owner)
    {
        return c?.MovementBatch is { } batch ? ShiHzNonUseHandSlashes(batch, owner) : [];
    }

    internal CardMovementRecord[] ShiHzNonUseHandSlashes(CardMovementBatchContext batch, int owner)
    {
        if (batch.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber)
            return[];
        return batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From != m.To && CoreNativeCardClassificationsRuntime.IsSlashCard(m.CardKind) && _cardZonesOperations._cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile && !_coreNativeUsedDiscardOriginsOperations.Value.IsNativeTrueUseDiscardMovement(batch, m) && (m.From == CardLocation.Hand(owner) || m.From == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.CardId == m.CardId && p.Sequence < m.Sequence)is { } previous && previous.To == CardLocation.Processing && previous.From == CardLocation.Hand(owner))).DistinctBy(m => m.CardId).OrderBy(m => m.Sequence).ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 47900)]
    internal int[]? ShiHzDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate c, ProgramInstructionFeatures features)
    {
        if (!features.HasOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuStore))
            return null;
        var eligible = ShiHzNonUseHandSlashes(batch, c.OwnerSeat).Select(m => m.Sequence).ToHashSet();
        return batch.Movements.Select((m, i) => (m, i)).Where(x => eligible.Contains(x.m.Sequence)).Select(x => x.i).ToArray();
    }

    internal ShiHzStoredEvent? ShiHzGrant(long id) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzStoredEvent>().SingleOrDefault(e => e.GrantId == id);
    internal bool ShiHzStoredAvailable(ShiHzStoredEvent g) => _coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive && g.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShiHzDuanyang).Program!.GameplayHash && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((g.Pile.OwnerSeat, g.Pile.SkillId, g.Pile.SkillInstanceId)) == g.Pile && _cardZonesOperations._cardZones.GetLocation(g.CardId) == g.Pile.Location && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzGrantConsumedEvent>().Any(e => e.GrantId == g.GrantId);
    internal bool IsIssuedShiHzPile(PublicPersistentPileSource source) => HasShiHz && _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzStoredEvent>().Any(g => g.Pile == source && g.Phase.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzGrantConsumedEvent>().Any(e => e.GrantId == g.GrantId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShiHuangZu?.GrantId == g.GrantId)));
    internal IReadOnlyList<ActualPhaseEndItem> CollectShiHzPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase)
    {
        if (!HasShiHz)
            return[];
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(ShiHzDuanyang).Program!.Triggers.Single(t => t.Id == "use-current-phase-end");
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzStoredEvent>().Where(g => g.Phase == phase && ShiHzStoredAvailable(g)).OrderBy(g => g.GrantId).Select((g, i) => new ActualPhaseEndItem(new(g.Source.OwnerSeat, ShiHzDuanyang, trigger.Id, g.Source.SkillInstanceId, g.GameplayHash, trigger.Priority, i), g.GrantId)).ToArray());
    }

    internal bool ShiHzIssued(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == ShiHzDuanyang && c.BindingId == "use-current-phase-end" && context.ActualPhaseEnd is { } due && _coreActualPhaseEndsOperations.IsExactActualPhaseEndCandidate(c, context) && ShiHzGrant(due.EntitlementId)is { } g && g.Source.OwnerSeat == c.OwnerSeat && g.Source.SkillInstanceId == c.SkillInstanceId && g.GameplayHash == c.GameplayHash && g.Phase == due.Phase && (ShiHzStoredAvailable(g) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WindowContext?.ParentFrameId == context.ParentFrameId && f.OwnerSeat == c.OwnerSeat && f.SkillId == c.SkillId && f.SkillInstanceId == c.SkillInstanceId && f.GameplayHash == c.GameplayHash && f.ShiHuangZu is { } r && r.GrantId == g.GrantId && r.Source == g.Source && r.CardId == g.CardId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzGrantConsumedEvent>().Count(e => e.GrantId == g.GrantId && e.FrameId == f.Id) == 1));
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 47900)]
    internal bool IsIssuedShiHz(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => ShiHzIssued(c, context);
    internal CardUseFrame? ShiHzDamageUse(ProgramSkillWindowContext context, int owner)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.SourceSeat != owner || context.Amount <= 0 || context.DamageFrameId is not { } id || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == id)is not { } damage || damage.SourceSeat != owner || damage.TargetSeat != context.TargetSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ParentFrameId)is not { ShiHuangZuUse: { } n } use || n.Source.OwnerSeat != owner || use.Action?.ActionId != n.ActionId || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return null;
        return use;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 47900)]
    internal bool CanOfferShiHz(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (c.SkillId == ShiHzChizhang)
            return ShiHzHandDamage(context, c.OwnerSeat)is not null;
        if (c.SkillId != ShiHzDuanyang)
            return true;
        return t.Id switch
        {
            "store-non-use-slash" => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not null && ShiHzNonUseHandSlashes(context, c.OwnerSeat).Length > 0,
            "use-current-phase-end" => ShiHzIssued(c, context),
            "damage-recast" => ShiHzDamageUse(context, c.OwnerSeat)is not null,
            _ => false
        };
    }

    internal SkillProgramStepOutcome BeginShiHz(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WindowContext is not { } context || f.ShiHuangZu is not null)
            throw new InvalidOperationException("Shi Huang Zu requires his exact original unpaid trigger instruction.");
        if (op == SkillProgramEffectOp.ResolveMobileShiHuangZuChizhang)
        {
            var use = ShiHzHandDamage(context, f.OwnerSeat) ?? throw new InvalidOperationException("Chizhang lost the actual hand damage Use.");
            var cost = _coreProgramLifecycleOperations.GetProgramCardSet(f, "chizhang-payment");
            if (cost.CardIds.Count != cost.SourceLocations.Count || cost.SourceLocations.Any(l => l != CardLocation.Hand(f.OwnerSeat)) || cost.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)))
                throw new InvalidOperationException("Chizhang requires its original selected hand cost before its native movement children.");
            var colors = cost.CardIds.Select(id => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
            var grant = new ShiHzColorsBoundEvent(use.Id, use.Action!.ActionId, f.Id, f.OwnerSeat, colors.Contains(true), colors.Contains(false));
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ShiHuangZuColors = grant });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(grant);
            return SkillProgramStepOutcome.Continue;
        }

        var stage = op == SkillProgramEffectOp.ResolveMobileShiHuangZuStore ? ShiHzStage.StoreChildren : op == SkillProgramEffectOp.ResolveMobileShiHuangZuPhaseUse ? ShiHzStage.UseChoice : ShiHzStage.RecastChoice;
        var r = new ShiHzReceipt(ShiHzSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, context.ParentFrameId, stage);
        if (stage == ShiHzStage.StoreChildren)
        {
            var candidates = ShiHzNonUseHandSlashes(context, f.OwnerSeat);
            if (candidates.Length == 0)
                return SkillProgramStepOutcome.Continue;
            var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() ?? throw new InvalidOperationException("Duanyang lost its actual phase identity.");
            var before = _randomOperations._random.State;
            var index = _randomOperations._random.Next(candidates.Length);
            var chosen = candidates[index];
            var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, 0);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { GrantId = f.Id, CardId = chosen.CardId, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzStoredEvent(f.Id, r.Source, r.GameplayHash, phase, context.MovementBatch!.Id, chosen.CardId, chosen.Sequence, pile, before, _randomOperations._random.State, candidates.Length, index));
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(chosen.CardId), CardLocation.DiscardPile, pile.Location, new("skill-program.mobile-shi-huang-zu.store"));
            SealShiHz(f.Id);
            ContinueShiHz(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == ShiHzStage.UseChoice)
        {
            var g = ShiHzGrant(context.ActualPhaseEnd!.EntitlementId) ?? throw new InvalidOperationException("Duanyang lost its issued current-phase card.");
            r = r with
            {
                Source = g.Source,
                GrantId = g.GrantId,
                CardId = g.CardId
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzGrantConsumedEvent(g.GrantId, f.Id));
            if (ShiHzTargets(f).Count == 0)
            {
                CleanupShiHzStored(f);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }
        else
        {
            var use = ShiHzDamageUse(context, f.OwnerSeat) ?? throw new InvalidOperationException("Duanyang recast needs damage from its exact original stored Slash.");
            r = r with
            {
                Target = context.TargetSeat!.Value,
                UseId = use.Id,
                ActionId = use.Action!.ActionId
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r });
        }

        PublishShiHz(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<int> ShiHzTargets(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        var g = ShiHzGrant(r.GrantId);
        if (g is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(r.CardId) != g.Pile.Location || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
        var kind = card.Kind;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, kind, CardActionType.Use) || _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, [card.Id]) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner.Seat, kind, [card.Id])))
            return[];
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner.Seat && _coreNativeTargetLegalityOperations.CanUseSlashTarget(owner, p, card, effectiveKind: kind, physicalCardIds: [card.Id])).Select(p => p.Seat).ToArray());
    }

    internal IReadOnlyList<PromptChoice> ShiHzChoices(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        PromptChoice C(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, Dictionary<string, string>? extra = null) => new(new($"shi-huang-zu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(extra?.GetValueOrDefault("source-zone") ?? "none")}.{(extra?.GetValueOrDefault("slot-index") ?? string.Join("-", cards ?? []))}.{(string.Join("-", targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string>(extra ?? []) { ["program-action"] = "shi-huang-zu", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == ShiHzStage.UseChoice)
            return Array.AsReadOnly(ShiHzTargets(f).Select(seat => C("use", "使用武将牌上的【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).DisplayName + "】对 " + _coreMatchParticipantsPlayersAccess._players[seat].Name, [r.CardId], [seat])).ToArray());
        if (r.Stage != ShiHzStage.RecastChoice)
            return[];
        var choices = new List<PromptChoice>();
        if (r.Selected.Count < 2 && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            foreach (var zone in new[]
            {
                CardZoneKind.Hand,
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
            {
                var cards = _cardZonesOperations._cardZones.CardsAt(new(zone, r.Target));
                for (var i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];
                    if (r.Selected.Any(s => s.CardId == card.Id))
                        continue;
                    var opaque = zone == CardZoneKind.Hand && f.OwnerSeat != r.Target;
                    var label = opaque ? $"选择 {(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)} 的第{(i + 1)}张暗手牌" : $"选择{((zone == CardZoneKind.Judgment ? "判定区" : zone == CardZoneKind.Equipment ? "装备区" : "手牌"))}【{(card.DisplayName)}】";
                    choices.Add(C("card", label, opaque ? [] : [card.Id], extra: new() { ["source-zone"] = zone.ToString(), ["slot-index"] = i.ToString(CultureInfo.InvariantCulture) }));
                }
            }
        }

        choices.Add(C("finish", $"重铸已选 {(r.Selected.Count)}/2 张，然后摸四张牌"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiHz(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        var choices = ShiHzChoices(f);
        if (choices.Count == 0)
        {
            FinishShiHz(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == ShiHzStage.UseChoice ? "断鞅：选择目标，使用储存的原实体【杀】（不计次数）。" : "断鞅：选择受伤角色区域内至多两张牌，完成后同时重铸，再摸四张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = r.Stage == ShiHzStage.RecastChoice,
            TargetSeat = r.Stage == ShiHzStage.RecastChoice ? r.Target : f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shi-huang-zu")]
    internal void AnswerShiHz(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHuangZu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiHzChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Duanyang lost its original native target or opaque owned card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == ShiHzStage.UseChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = ShiHzStage.IssuingUse, Target = choice.Targets.Single() } });
            IssueShiHzNativeUse(f);
            return;
        }

        if (branch == "card")
        {
            var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
            var from = new CardLocation(zone, r.Target);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Selected = r.Selected.Append(new NativeRecastMaterial(card.Id, card.Kind, from, card.IsGeneralWeapon)).ToArray() } });
            PublishShiHz(f);
            return;
        }

        if (branch != "finish" || r.Selected.Count > 2 || r.Selected.Any(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) != m.From))
            throw new InvalidOperationException("Duanyang recast changed its selected original HEJ entities.");
        if (r.Selected.Count == 0)
        {
            IssueShiHzReward(f);
            return;
        }

        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, r.Target, r.Selected.Select(m => m.CardId).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = ShiHzStage.RecastCostChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.Target, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected.Select(m => m.CardId).ToArray(), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        SealShiHz(f.Id);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        foreach (var m in r.Selected)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzRecastPaidEvent(f.Id, r.Target, m.CardId, m.From, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.First(move => move.Sequence > f.ShiHuangZu!.Before && move.CardId == m.CardId && move.From == m.From && move.Reason == CardMoveReasons.RecastDiscard).Sequence));
        ContinueShiHz(f.Id);
    }

    internal void SealShiHz(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f with { ShiHuangZu = f.ShiHuangZu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiHz(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiHz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHuangZu: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage is ShiHzStage.UseChoice or ShiHzStage.RecastChoice)
        {
            PublishShiHz(f);
            return;
        }

        if (r.Stage is ShiHzStage.IssuingUse or ShiHzStage.NativeUse or ShiHzStage.RecastDrawing or ShiHzStage.RewardDrawing)
            return;
        if (DrainShiHz(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.ShiHuangZu!;
        if (r.Stage == ShiHzStage.RecastCostChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                for (var i = 0; i < r.Selected.Count; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(r.Target, r.Selected[i].CardId, r.Selected[i].PrintedKind, 0));
                f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, 0);
                IssueShiHzReward(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = ShiHzStage.RecastDrawing, Draw = null, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, r.Selected.Count, CardMoveReasons.RecastDraw);
            return;
        }

        if (r.Stage == ShiHzStage.RecastDrawChildren)
        {
            IssueShiHzReward(f);
            return;
        }

        FinishShiHz(f);
    }

    internal void IssueShiHzReward(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishShiHz(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = ShiHzStage.RewardDrawing, Draw = null, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 4, new("skill-program.mobile-shi-huang-zu.reward"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiHuangZuRecast)]
    internal void ReturnShiHzDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.ShiHuangZu is not { Stage: ShiHzStage.RecastDrawing or ShiHzStage.RewardDrawing, Draw: null } r || outcome.RecipientSeat != (r.Stage == ShiHzStage.RecastDrawing ? r.Target : f.OwnerSeat) || outcome.RequestedCount != (r.Stage == ShiHzStage.RecastDrawing ? r.Selected.Count : 4))
            throw new InvalidOperationException("Duanyang lost the exact once-issued recast or four-card native draw.");
        var recast = r.Stage == ShiHzStage.RecastDrawing;
        var actual = outcome.ActualDrawIds.Count;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = recast ? ShiHzStage.RecastDrawChildren : ShiHzStage.RewardChildren, Draw = outcome, ActualDraw = actual, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter) }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        if (recast)
        {
            for (var i = 0; i < r.Selected.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(r.Target, r.Selected[i].CardId, r.Selected[i].PrintedKind, i < actual ? 1 : 0));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, actual);
        }
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzRewardIssuedEvent(f.Id, f.OwnerSeat, actual));
        ContinueShiHz(f.Id);
    }

    internal void FinishShiHz(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = f.ShiHuangZu!with { Stage = ShiHzStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal void CleanupShiHzStored(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        var g = ShiHzGrant(r.GrantId);
        if (g is null || _cardZonesOperations._cardZones.GetLocation(r.CardId) != g.Pile.Location)
        {
            FinishShiHz(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiHuangZu = r with { Stage = ShiHzStage.CleanupChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), g.Pile.Location, CardLocation.DiscardPile, new("skill-program.mobile-shi-huang-zu.expired"));
        SealShiHz(f.Id);
        ContinueShiHz(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 47900)]
    internal bool ResumeShiHz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHuangZu: not null } f || f.Id != id)
            return false;
        ContinueShiHz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 47900)]
    internal bool ReturnShiHzMovement(ProgramSkillFrame f)
    {
        if (f.ShiHuangZu is null)
            return false;
        ContinueShiHz(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 47900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 47900)]
    internal bool PaidShiHzContinues(ProgramSkillFrame f) => f.ShiHuangZu is { Stage: not (ShiHzStage.Finished or ShiHzStage.RecastChoice) };
    internal bool ShiHzMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => f.ShiHuangZu is { Stage: ShiHzStage.StoreChildren or ShiHzStage.RecastCostChildren or ShiHzStage.RecastDrawChildren or ShiHzStage.RewardChildren or ShiHzStage.ReturnedUse or ShiHzStage.CleanupChildren } && movement.BeforeCount == 0 && movement.CoverageResultBind is null && effect?.Op is SkillProgramEffectOp.ResolveMobileShiHuangZuStore or SkillProgramEffectOp.ResolveMobileShiHuangZuRecast or SkillProgramEffectOp.ResolveMobileShiHuangZuPhaseUse;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiHuangZuPhaseUse, SkillProgramEffectOp.ResolveMobileShiHuangZuRecast)]
    internal PromptChoice ShiHzAi(PendingDecision p, ProgramSkillFrame f) => f.ShiHuangZu!.Stage == ShiHzStage.UseChoice ? p.Choices.OrderBy(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).First() : p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "card") ?? p.Choices.Last();

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiHuangZuPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != ShiHzDuanyang)
            return[];
        return Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "store-non-use-slash", ProgramPublicRuleStateKind.ActivationLimit, 1, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, "store-non-use-slash@" + i.SkillInstanceId, SkillUsageScope.Turn), SkillUsageScope.Turn), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "stored-current-phase", ProgramPublicRuleStateKind.GameActivationProgress, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHzStoredEvent>().Count(g => g.Source.OwnerSeat == owner.Seat && g.Source.SkillInstanceId == i.SkillInstanceId && ShiHzStoredAvailable(g))) }).ToArray());
    }
}
