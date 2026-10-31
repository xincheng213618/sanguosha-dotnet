#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengDaOlRuntime
{
    internal bool HasGoudeCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGoudeActualTurnEnd);

    internal static CardConversionSource MengDaSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_meng_da_ol_MengDaLedger.MengDaSource(f);
    internal IReadOnlyList<int> GoudeFactionSeats(int owner)
    {
        var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[owner]);
        return faction is null ? [] : Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == faction).Select(p => p.Seat).ToArray());
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27240)]
    internal void ObserveGoudeZeroSlash(IGameEvent fact)
    {
        if (!HasGoudeCapability || _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber < 1 || fact is not CardUseDeclaredEvent { CardId: 0 } declared || !CoreNativeCardClassificationsRuntime.IsSlashCard(declared.CardKind) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == declared.ResolutionId)is not { CardId: 0, PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use } action } use || action.ActorSeat != declared.SourceSeat || use.SourceSeat != declared.SourceSeat || action.EffectiveKind != declared.CardKind || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeZeroSlashDeclaredEvent>().Any(e => e.UseId == use.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeZeroSlashDeclaredEvent(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat));
    }

    internal int GoudeZeroSlashActor(GoudeZeroSlashDeclaredEvent observed, IReadOnlyList<IGameEvent> history)
    {
        if (history.OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == observed.UseId && e.CardId == 0 && CoreNativeCardClassificationsRuntime.IsSlashCard(e.CardKind) && e.SourceSeat == observed.ActorSeat) != 1)
            throw new InvalidOperationException("Goude lost its true native zero-entity Slash declaration.");
        var actor = observed.ActorSeat;
        foreach (var e in history.OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == observed.UseId))
        {
            if (e.PreviousActorSeat != actor || e.OwnerSeat != actor || e.ProviderSeat != observed.ProviderSeat || e.FrameId <= observed.UseId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.ActorSeat) || e.ActorSeat == actor || !history.OfType<ProgramBindingStartedEvent>().Any(b => b.FrameId == e.FrameId && b.OwnerSeat == e.OwnerSeat && b.SkillId == e.SkillId && b.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized))
                throw new InvalidOperationException("Goude's zero-entity use lost its exact native actor-replacement chain.");
            actor = e.ActorSeat;
        }

        return actor;
    }

    internal int GoudeExecutedMask(int turn, IReadOnlyList<int> sameFaction, int cutoff)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(cutoff).ToArray();
        var mask = 0;
        if (history.OfType<NativeDrawInvocationRecordedEvent>().Any(e => e.TurnNumber == turn && e.ActualCount == 1 && sameFaction.Contains(e.OwnerSeat)))
            mask |= (int)GoudeOption.Draw;
        if (history.OfType<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.TurnNumber == turn && e.ActualCount == 1 && e.HandCount == 1 && (sameFaction.Contains(e.InitiatorSeat) || sameFaction.Contains(e.ExecutorSeat))))
            mask |= (int)GoudeOption.Discard;
        if (history.OfType<GoudeZeroSlashDeclaredEvent>().Any(e => e.ActualTurn == turn && sameFaction.Contains(GoudeZeroSlashActor(e, history))))
            mask |= (int)GoudeOption.Slash;
        if (history.OfType<NativeEffectiveFactionChangedEvent>().Any(e => e.TurnNumber == turn && e.PreviousFactionId != e.CurrentFactionId && sameFaction.Contains(e.TargetSeat)))
            mask |= (int)GoudeOption.Faction;
        return mask;
    }

    internal bool ExactGoudeCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterTurnEnded || context.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) != candidate || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat || context.Facts != parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts || context.OccurrenceIndex != candidate.OccurrenceIndex)
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        return trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveGoudeActualTurnEnd }] && trigger.TurnOwnerScope == (candidate.OwnerSeat == parent.OwnerSeat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27240)]
    internal bool QualifyGoude(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.ResolveGoudeActualTurnEnd }])
            return true;
        if (!ExactGoudeCandidate(candidate, context) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        var seats = GoudeFactionSeats(candidate.OwnerSeat);
        var mask = GoudeExecutedMask(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, seats, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count());
        return mask is> 0 and < 15 && GoudeAvailableOptions(candidate.OwnerSeat, mask).Count > 0;
    }

    internal IReadOnlyList<int> GoudeDiscardTargets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && GoudeDiscardMaterials(owner, p.Seat).Count > 0).Select(p => p.Seat).ToArray());
    internal IReadOnlyList<GoudeHandMaterial> GoudeDiscardMaterials(int owner, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            return[];
        var location = CardLocation.Hand(target);
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select((card, slot) => (Card: card, Slot: slot)).Where(item => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, item.Card, location, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, item.Card, location, OwnedCardMoveIntent.Discard)).Select(item => new GoudeHandMaterial(item.Card.Id, item.Card.Kind, target, item.Slot, _characterLuoXianOlOperations.CurrentHandEntry(target, item.Card.Id) ?? throw new InvalidOperationException("Goude's actual Hand entity lost its native residency entry."))).ToArray());
    }

    internal IReadOnlyList<int> GoudeSlashTargets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], p)).Select(p => p.Seat).ToArray());
    internal IReadOnlyList<string> GoudeFactionChoices(int owner) => Array.AsReadOnly(_coreNativeFactionChangesOperations.AvailableEffectiveFactionIds().Where(f => f != _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[owner])).ToArray());
    internal IReadOnlyList<GoudeOption> GoudeAvailableOptions(int owner, int mask) => Array.AsReadOnly(Enum.GetValues<GoudeOption>().Where(o => (mask & (int)o) == 0 && (o switch
    {
        GoudeOption.Discard => GoudeDiscardTargets(owner).Count > 0,
        GoudeOption.Slash => GoudeSlashTargets(owner).Count > 0,
        GoudeOption.Faction => GoudeFactionChoices(owner).Count > 0,
        _ => true
    })).ToArray());
}
