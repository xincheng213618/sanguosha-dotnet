#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal bool HasQushiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveQushiDrawAndPrivateSeal);

    internal const string QushiDrawReason = "skill-program.guo-tu.qushi.draw", QushiSealReason = "skill-program.guo-tu.qushi.seal", QushiRemoveReason = "skill-program.guo-tu.qushi.remove", QushiDeathReason = "skill-program.guo-tu.qushi.holder-death";
    internal static CardConversionSource QushiSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guo_tu_ol_QushiLedger.QushiSource(f);
    internal bool ValidQushiSeal(QushiSeal s) => s.Source.SkillId == "ol:qushi" && s.Source.BindingId == "draw-seal" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(s.HolderSeat) && s.HolderSeat != s.Source.OwnerSeat && s.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, s.HolderSeat, $"qushi:{(s.SealId)}") && _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program?.GameplayHash == s.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<QushiStartedEvent>().Count(e => e.FrameId == s.SealId && e.Source == s.Source && e.GameplayHash == s.GameplayHash && e.Op == SkillProgramEffectOp.ResolveQushiDrawAndPrivateSeal) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == s.MovementSequence && m.CardId == s.CardId && m.CardKind == s.PrintedKind && m.From == CardLocation.Hand(s.Source.OwnerSeat) && m.To == s.Location && m.Reason.Value == QushiSealReason) == 1;
    internal QushiSeal[] LiveQushiSeals() => !HasQushiCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<QushiSealPlacedEvent>().Select(e => e.Seal).Where(s => ValidQushiSeal(s) && !_coreProgramEventHistoryOperations.ProgramEventHistory<QushiSealRemovedEvent>().Any(e => e.SealId == s.SealId) && _cardZonesOperations._cardZones.GetLocation(s.CardId) == s.Location).ToArray();
    internal IReadOnlyList<NamedAmbushPileSnapshot>? ProjectQushiSeals(int holderSeat, int viewerSeat, bool revealAll)
    {
        var seals = LiveQushiSeals().Where(s => s.HolderSeat == holderSeat).ToArray();
        if (seals.Length == 0)
            return null;
        return Array.AsReadOnly(seals.GroupBy(s => s.Source.OwnerSeat).Select(g => new NamedAmbushPileSnapshot("ol:qushi", "趋", g.Count(), revealAll || g.Key == viewerSeat ? Array.AsReadOnly(g.Select(s => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(s.CardId)), s.PrintedKind)).ToArray()) : null)).ToArray());
    }

    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 10)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeQushiSealSnapshots(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll)
    {
        var own = ProjectQushiSeals(holder, viewer, revealAll);
        if (own is null)
            return ordinary;
        return Array.AsReadOnly((ordinary ?? []).Concat(own).Select(p => p with { Entries = p.Entries is { } entries ? Array.AsReadOnly(entries.ToArray()) : null }).ToArray());
    }

    internal IReadOnlyList<int> DiscardQushiHolderDeathSeals(DeathFrame death)
    {
        if (!HasQushiCapability)
            return[];
        if (_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == death.Id)is not { } live || live.VictimSeat != death.VictimSeat || _coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive)
            throw new InvalidOperationException("Qushi death removal requires its real current dead holder.");
        var seals = LiveQushiSeals().Where(s => s.HolderSeat == death.VictimSeat).ToArray();
        foreach (var s in seals)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(s.CardId), s.Location, CardLocation.DiscardPile, new(QushiDeathReason), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QushiSealRemovedEvent(death.Id, s.SealId, s.HolderSeat, s.CardId, m.Sequence, true)));
        return Array.AsReadOnly(seals.Select(s => s.CardId).ToArray());
    }

    internal void AppendQushiEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!HasQushiCapability || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat].IsAlive)
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill("ol:qushi").Program!.Triggers.Single(t => t.Id == "issued-holder-ending");
        foreach (var s in LiveQushiSeals().Where(s => s.HolderSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat, s.Source.OwnerSeat)).ThenBy(s => s.SealId))
        {
            var c = new ProgramTriggerCandidate(s.Source.OwnerSeat, "ol:qushi", t.Id, s.Source.SkillInstanceId, s.GameplayHash, t.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, $"qushi:{(s.SealId)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }
    }

    internal ProgramSkillWindowContext CreateQushiEndingContext(TurnEndingBoundaryFrame parent, ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:qushi" || c.BindingId != "issued-holder-ending")
            return context;
        var item = parent.Items[parent.ItemIndex];
        var s = _coreProgramEventHistoryOperations.ProgramEventHistory<QushiSealPlacedEvent>().Select(e => e.Seal).SingleOrDefault(s => item.StableIdentity == $"qushi:{(s.SealId)}");
        return context with
        {
            QushiSealId = s?.SealId
        };
    }

    internal QushiSeal? CurrentQushiEndingSeal(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasQushiCapability || c.SkillId != "ol:qushi" || c.BindingId != "issued-holder-ending" || context.Window != SkillProgramTriggerWindow.TurnEnding || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.QushiSealId is not { } id || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } p || p.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || p.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate != c || p.Items[p.ItemIndex].StableIdentity != $"qushi:{(id)}")
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<QushiSealPlacedEvent>().Select(e => e.Seal).SingleOrDefault(s => s.SealId == id && s.Source.OwnerSeat == c.OwnerSeat && s.Source.SkillInstanceId == c.SkillInstanceId && s.GameplayHash == c.GameplayHash && s.HolderSeat == p.OwnerSeat && ValidQushiSeal(s));
    }

    [ProgramIssuedTrigger(290)]
    internal bool IsIssuedQushiEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && CurrentQushiEndingSeal(c, context)is { } s && (LiveQushiSeals().Any(d => d == s) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Qushi is { Seal: { } seal, Paid: true } && seal == s && f.WindowContext == context && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c)));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27460)]
    internal void ObserveQushiUses(IGameEvent e)
    {
        if (!HasQushiCapability || e is QushiActualUseEvent || _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber <= 0)
            return;
        if (e is CardActionAcceptedEvent { Action: { Type: CardActionType.Response } a })
        {
            var trueUse = false;
            long origin = 0;
            if (a.EffectiveKind == CardKind.Nullification && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is { } n && a.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(n) && a.OpponentSeat == n.SourceSeat)
            {
                trueUse = true;
                origin = n.Id;
            }

            if (a.EffectiveKind == CardKind.Dodge && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == a.ParentActionId)is { CardAttack: { } attack } u && attack.TargetSeat == a.ActorSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? u.CardKind) && a.OpponentSeat == attack.SourceSeat)
            {
                trueUse = true;
                origin = u.Id;
            }

            if (trueUse && a.ActorSeat == a.ProviderSeat && a.RequesterSeat is null && a.ResponderSeat == a.ActorSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<QushiActualUseEvent>().Any(x => x.ResponseUse && x.ActionId == a.ActionId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QushiActualUseEvent(_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat, origin, a.ActionId, true, a.ActorSeat, a.EffectiveKind, 0));
        }

        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray())
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(d => d.ResolutionId == use.Id))
                RecordQushiUse(use);
    }

    [NativeUsePreparation(27460)]
    internal bool CaptureQushiFinalUse(CardUseFrame use)
    {
        if (HasQushiCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(d => d.ResolutionId == use.Id))
            RecordQushiUse(use);
        return false;
    }

    internal void RecordQushiUse(CardUseFrame use)
    {
        if (use.Action is { Type: not CardActionType.Use })
            return;
        var targets = use.Action?.EffectiveDesignatedTargetSeats ?? use.TargetSeats;
        var count = targets.Distinct().Count();
        if (count == 0 && !use.TargetsAdjusted && (global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Equipment || use.CardKind is CardKind.DrawTwo or CardKind.Peach or CardKind.Alcohol))
            count = 1;
        var old = _coreProgramEventHistoryOperations.ProgramEventHistory<QushiActualUseEvent>().LastOrDefault(e => !e.ResponseUse && e.OriginId == use.Id);
        var next = new QushiActualUseEvent(old?.ActualTurn ?? _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, old?.TurnOwner ?? _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat, use.Id, use.Action?.ActionId ?? 0, false, use.SourceSeat, use.CardKind, count);
        if (next != old)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(next);
    }

    internal QushiActualUseEvent[] QushiActualUses(int holder) => _coreProgramEventHistoryOperations.ProgramEventHistory<QushiActualUseEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat).GroupBy(e => (e.ResponseUse, e.ResponseUse ? e.ActionId : e.OriginId)).Select(g => g.Last()).Where(e => e.ActorSeat == holder).ToArray();
}
