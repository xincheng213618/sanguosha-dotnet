#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenMobileShiRuntime
{
    internal const string ShiCaoZSifeng = "mobile:shi-sifeng";
    internal const string ShiCaoZTopReason = "skill.mobile-shi-cao-zhen.top", ShiCaoZPlaceReason = "skill.mobile-shi-cao-zhen.place", ShiCaoZRemoveReason = "skill.mobile-shi-cao-zhen.remove", ShiCaoZDiscardReason = "skill.mobile-shi-cao-zhen.discard", ShiCaoZObtainReason = "skill.mobile-shi-cao-zhen.obtain", ShiCaoZDeathReason = "skill.mobile-shi-cao-zhen.holder-death";
    internal bool HasShiCaoZ => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace);

    internal static CardConversionSource ShiCaoZSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool ValidShiCaoZMaterial(ShiCaoZMaterial m) => m.Source.SkillId == ShiCaoZSifeng && m.Source.BindingId == "place-forecast" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(m.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(m.Holder) && m.Holder != m.Source.OwnerSeat && m.Index is >= 0 and < 3 && m.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, m.Holder, $"shi-cao-zhen:{(m.PlacementId)}") && _contentRegistryOperations._contentRegistry.GetSkill(ShiCaoZSifeng).Program?.GameplayHash == m.Hash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZStartedEvent>().Count(e => e.FrameId == m.SetupId && e.Source == m.Source && e.Hash == m.Hash && e.Operation == SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == m.Sequence && move.CardId == m.CardId && move.CardKind == m.Kind && move.From == CardLocation.Processing && move.To == m.Location && move.Reason.Value == ShiCaoZPlaceReason);
    internal ShiCaoZMaterial[] LiveShiCaoZMaterials() => !HasShiCaoZ ? [] : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZPlacedEvent>().Select(e => e.Material).Where(m => ValidShiCaoZMaterial(m) && _cardZonesOperations._cardZones.GetLocation(m.CardId) == m.Location && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZRemovedEvent>().Any(e => e.PlacementId == m.PlacementId)).OrderBy(m => m.PlacementId).ToArray();
    internal bool ShiCaoZSameSource(ShiCaoZMaterial m, int owner, string instance, string hash) => m.Source.OwnerSeat == owner && m.Source.SkillInstanceId == instance && m.Hash == hash;
    internal bool TryShiCaoZCompletedUse(ProgramSkillWindowContext context, out CardActionContext action, out long origin)
    {
        action = null !;
        origin = 0;
        if (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCompleted || window.Action is not { } a || a.ActionId != identity.CardActionId || a.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || a.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
            return false;
        if (a.Type == CardActionType.Use)
        {
            if (window.CompletedResponseReturn is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Step: ResolutionFrameStep.Completed, Action: { } native } use || native.ActionId != a.ActionId || native.ActorSeat != a.ActorSeat || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Any(e => e.ResolutionId == use.Id))
                return false;
        }
        else if (window.CompletedResponseReturn is not { IsCommitted: false } response || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, response.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window))
            return false;
        action = a;
        origin = window.ParentFrameId;
        return true;
    }

    internal bool IsIssuedShiCaoZUseCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HasShiCaoZ && c.SkillId == ShiCaoZSifeng && c.BindingId == "issued-use" && context.OwnerSeat == c.OwnerSeat && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash && TryShiCaoZCompletedUse(context, out var action, out _) && (LiveShiCaoZMaterials().FirstOrDefault(m => m.Holder == action.ActorSeat && m.Source.OwnerSeat == c.OwnerSeat)is { } head && ShiCaoZSameSource(head, c.OwnerSeat, c.SkillInstanceId, c.GameplayHash) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZUseIssuedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.ActionId == action.ActionId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShiCaoZhen is { Operation: SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengUse } r && r.ParentId == context.ParentFrameId && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.ActionId == action.ActionId && f.WindowContext == context));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 49900)]
    internal void CollectShiCaoZIssuedUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasShiCaoZ || window != SkillProgramTriggerWindow.CardUseCompleted || action.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || action.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            return;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(ShiCaoZSifeng).Program!.Triggers.Single(t => t.Id == "issued-use");
        // Candidate collection precedes construction of the native window. Bind
        // one original head per issuer now; validate its actual window on entry.
        result.RemoveAll(c => c.SkillId == ShiCaoZSifeng && c.TriggerId == trigger.Id);
        foreach (var material in LiveShiCaoZMaterials().Where(m => m.Holder == action.ActorSeat).DistinctBy(m => m.Source.OwnerSeat))
        {
            if (result.Any(c => c.OwnerSeat == material.Source.OwnerSeat && c.SkillId == ShiCaoZSifeng && c.TriggerId == trigger.Id && c.SkillInstanceId == material.Source.SkillInstanceId))
                continue;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, material.Source.OwnerSeat, material.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[material.Source.OwnerSeat], action));
            result.Add(new(material.Source.OwnerSeat, material.Source.OwnerSeat, ShiCaoZSifeng, trigger.Id, material.Hash, material.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ObservesResponseUseCompletion, 49900)]
    internal bool ObservesShiCaoZResponseUse(CardActionContext action) => HasShiCaoZ && action.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && action.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && LiveShiCaoZMaterials().Any(m => m.Holder == action.ActorSeat);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeShiCaoZAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (!HasShiCaoZ)
            return ordinary;
        var items = parent.AfterTurnEnded is null ? ordinary.Where(i => i.Candidate is not { SkillId: ShiCaoZSifeng, BindingId: "issued-end" }).ToList() : ordinary.ToList();
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(ShiCaoZSifeng).Program!.Triggers.Single(t => t.Id == "issued-end");
        foreach (var material in LiveShiCaoZMaterials().Where(m => m.Holder == parent.OwnerSeat).DistinctBy(m => m.Source.OwnerSeat))
        {
            if (items.Any(i => i.Candidate is { SkillId: ShiCaoZSifeng, BindingId: "issued-end" } c && c.OwnerSeat == material.Source.OwnerSeat && c.SkillInstanceId == material.Source.SkillInstanceId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZEndIssuedEvent>().Any(e => e.Turn == parent.TurnNumber && e.Holder == parent.OwnerSeat && e.Source.OwnerSeat == material.Source.OwnerSeat && e.Source.SkillInstanceId == material.Source.SkillInstanceId))
                continue;
            var candidate = new ProgramTriggerCandidate(material.Source.OwnerSeat, ShiCaoZSifeng, trigger.Id, material.Source.SkillInstanceId, material.Hash, trigger.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, trigger.Priority, $"shi-cao-zhen:{(parent.TurnNumber)}:{(parent.OwnerSeat)}:{(material.Source.OwnerSeat)}:{(material.Source.SkillInstanceId)}", candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])));
        }

        return items.SequenceEqual(ordinary) ? ordinary : Array.AsReadOnly(items.ToArray());
    }

    [ProgramIssuedTrigger(360)]
    internal bool IsIssuedShiCaoZEndCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasShiCaoZ || c.SkillId != ShiCaoZSifeng || c.BindingId != "issued-end" || context.Window != SkillProgramTriggerWindow.AfterTurnEnded || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not DeferredTurnEndFrame parent || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) != c || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.Facts != parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash)
            return false;
        return LiveShiCaoZMaterials().Any(m => m.Holder == parent.OwnerSeat && ShiCaoZSameSource(m, c.OwnerSeat, c.SkillInstanceId, c.GameplayHash)) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShiCaoZhen is { Operation: SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengEnd } r && r.ParentId == parent.Id && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.Hash == c.GameplayHash && f.WindowContext == context);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 49900)]
    internal bool IsIssuedShiCaoZCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => IsIssuedShiCaoZUseCandidate(c, context) || IsIssuedShiCaoZEndCandidate(c, context);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49900)]
    internal bool QualifyShiCaoZ(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => c.SkillId != ShiCaoZSifeng || (trigger.Id switch
    {
        "issued-use" => IsIssuedShiCaoZUseCandidate(c, context),
        "issued-end" => IsIssuedShiCaoZEndCandidate(c, context),
        _ => context.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat)});
    internal IReadOnlyList<int> DiscardShiCaoZHolderDeathPiles(DeathFrame death)
    {
        if (!HasShiCaoZ)
            return[];
        var materials = LiveShiCaoZMaterials().Where(m => m.Holder == death.VictimSeat).ToArray();
        foreach (var m in materials)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.Location, CardLocation.DiscardPile, new(ShiCaoZDeathReason), move => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiCaoZRemovedEvent(m.PlacementId, death.Id, m.CardId, move.Sequence, ShiCaoZDeathReason)), disclosure: CardMovementDisclosure.FaceUp);
        return Array.AsReadOnly(materials.Select(m => m.CardId).ToArray());
    }
}
