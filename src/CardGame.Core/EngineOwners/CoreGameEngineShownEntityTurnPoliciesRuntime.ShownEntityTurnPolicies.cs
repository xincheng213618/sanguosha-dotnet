#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShownEntityTurnPoliciesRuntime
{
    internal bool TracksShownEntityTurnPolicies => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.IssueShownEntityTurnPolicy);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 910)]
    internal bool CanOfferShownEntityTurnPolicy(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.IssueShownEntityTurnPolicy) || context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
    internal bool HasShownEntityPlayStartParent(ProgramSkillFrame frame, ProgramSkillWindowContext context)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != context.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[index - 1] switch
        {
            PlayPhaseStartingBoundaryFrame parent when parent.OwnerSeat == frame.OwnerSeat && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } candidate => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate),
            ProgramLifecycleTriggerWindowFrame parent when parent.Window == SkillProgramTriggerWindow.PlayPhaseStarting && parent.OwnerSeat == frame.OwnerSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]),
            _ => false
        };
    }

    internal void IssueShownEntityTurnPolicy(ProgramSkillFrame frame, string bind, string stateId)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context || context.OwnerSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || set.CardIds is not [var id] || set.SourceLocations is not [var from] || from != CardLocation.Hand(owner.Seat) || set.Visibility != SkillProgramCardSetVisibility.Public || set.FrozenRevealedSuit is not { } suit || !HasShownEntityPlayStartParent(frame, context))
            throw new InvalidOperationException("Shown entity policy lost its exact actual Play start and public single-card reveal.");
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(id) != from || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCardsRevealedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == owner.Seat && e.SkillId == frame.SkillId && e.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) && e.Bind == bind && e.Cards.Count == 1 && e.Cards[0].Id == id) != 1)
            throw new InvalidOperationException("Shown entity policy requires its one actual public reveal fact.");
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownEntityTurnPolicyGrantedEvent>().Any(e => e.Policy.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("The shown entity policy cannot be reissued by its completed instruction.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
        var affected = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner.Seat && _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, p.Seat) == 1).Select(p => p.Seat).ToArray();
        var sequences = new List<long>();
        if (suit is Suit.Spade or Suit.Club or Suit.Heart or Suit.Diamond)
            foreach (var target in affected)
            {
                var restriction = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandColorRestriction(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, frame.Id, -1 - target, source, target, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(suit), requiresChromaticSuit: true);
                sequences.Add(restriction.GrantSequence);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HandCardColorRestrictionGrantedEvent(restriction));
            }

        var policy = new ShownEntityTurnPolicy(frame.Id, frame.InstructionIndex - 1, source, frame.GameplayHash, stateId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, context.ParentFrameId, id, from, suit, affected, sequences);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShownEntityTurnPolicyGrantedEvent(policy));
    }

    internal bool ShownEntityPoliciesMatch(ShownEntityTurnPolicy a, ShownEntityTurnPolicy b) => a.ProgramFrameId == b.ProgramFrameId && a.EffectIndex == b.EffectIndex && a.Source == b.Source && a.GameplayHash == b.GameplayHash && a.StateId == b.StateId && a.TurnNumber == b.TurnNumber && a.ActualTurnOwnerSeat == b.ActualTurnOwnerSeat && a.ActualPlayStartingFrameId == b.ActualPlayStartingFrameId && a.CardId == b.CardId && a.OriginalFrom == b.OriginalFrom && a.FrozenSuit == b.FrozenSuit && a.AffectedSeats.SequenceEqual(b.AffectedSeats) && a.RestrictionSequences.SequenceEqual(b.RestrictionSequences);
    internal void IssueShownEntityUseBenefits(long useId, CardActionContext? action)
    {
        if (!TracksShownEntityTurnPolicies || action is not { Type: CardActionType.Use } || action.PhysicalCards.Count == 0)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Shown entity benefit requires an actual owning use.");
        if (use.ShownEntityBenefits is not null || use.SourceSeat != action.ActorSeat || use.Action?.ActionId != action.ActionId || use.PhysicalCardIds is null || !use.PhysicalCardIds.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)))
            throw new InvalidOperationException("Shown entity benefit cannot change its actual accepted full material set.");
        var policies = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownEntityTurnPolicyGrantedEvent>().Select(e => e.Policy).Where(p => p.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && p.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && action.PhysicalCards.Any(c => c.CardId == p.CardId)).GroupBy(p => (p.Source.OwnerSeat, p.Source.SkillId, p.StateId, p.CardId)).Select(g => g.First()).ToArray();
        if (policies.Length == 0)
            return;
        // One actual Use receives +1 from this method even when two different
        // material entities were revealed in separate actual Play phases.
        var benefits = Array.AsReadOnly(policies.GroupBy(p => (p.Source.OwnerSeat, p.Source.SkillId, p.StateId)).Select(g => new ShownEntityUseBenefit(useId, action.ActionId, action.ActorSeat, action.ProviderSeat, g.First(), action.PhysicalCards)).ToArray());
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, f => f with { ShownEntityBenefits = benefits });
        foreach (var benefit in benefits)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShownEntityUseBenefitIssuedEvent(useId, action.ActionId, action.ActorSeat, action.ProviderSeat, benefit.Policy));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 30)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> GetShownEntityUseDamageBenefits(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.IsSourceLess || attack.SourceSeat != attack.CardUserSeat || attack.EffectiveCardKind is null)
            return[];
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        if (use?.ShownEntityBenefits is not { Count: > 0 } benefits || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != attack.CardUserSeat)
            return[];
        AssertShownEntityUseBenefits(use);
        return benefits.Where(b => b.Policy.Source.OwnerSeat == attack.SourceSeat).Select(b => (b.Policy.Source, 1));
    }

    internal void AssertShownEntityUseBenefits(CardUseFrame use)
    {
        if (use.ShownEntityBenefits is not { } benefits)
            return;
        if (!TracksShownEntityTurnPolicies || benefits.Count == 0 || benefits is not System.Collections.IList { IsReadOnly: true } || use.Action is not { Type: CardActionType.Use } action || use.SourceSeat != action.ActorSeat || use.PhysicalCardIds is null || !use.PhysicalCardIds.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || action.PhysicalCards.Select(c => c.CardId).Distinct().Count() != action.PhysicalCards.Count || benefits.GroupBy(b => (b.Policy.Source.OwnerSeat, b.Policy.Source.SkillId, b.Policy.StateId)).Any(g => g.Count() != 1))
            throw new InvalidOperationException("Shown entity benefit lost its exact actual use/material/source identity.");
        foreach (var b in benefits)
        {
            var p = b.Policy;
            if (b.CardUseFrameId != use.Id || b.ActionId != action.ActionId || !ShownEntityUseActorMatches(use, b.OriginalActorSeat, b.ProviderSeat) || p.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || p.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || p.OriginalFrom != CardLocation.Hand(p.Source.OwnerSeat) || b.PhysicalMaterials is not System.Collections.IList { IsReadOnly: true } || !b.PhysicalMaterials.SequenceEqual(action.PhysicalCards) || !action.PhysicalCards.Any(c => c.CardId == p.CardId) || p.AffectedSeats.Distinct().Count() != p.AffectedSeats.Count || p.AffectedSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == p.Source.OwnerSeat) || p.RestrictionSequences.Distinct().Count() != p.RestrictionSequences.Count || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownEntityTurnPolicyGrantedEvent>().Count(e => ShownEntityPoliciesMatch(e.Policy, p)) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownEntityUseBenefitIssuedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == action.ActionId && e.OriginalActorSeat == b.OriginalActorSeat && e.ProviderSeat == b.ProviderSeat && ShownEntityPoliciesMatch(e.Policy, p)) != 1)
                throw new InvalidOperationException("Shown entity benefit lost its frozen granted policy and one true issuance fact.");
            AssertShownEntityPolicyOrigin(p);
        }
    }

    internal bool ShownEntityUseActorMatches(CardUseFrame use, int originalActor, int provider)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ProviderSeat != provider || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == originalActor && e.CardId == use.CardId) != 1)
            return false;
        var changes = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (changes.Length > 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.Type == CardActionType.Use && e.Action.ActorSeat == originalActor && e.Action.ProviderSeat == provider && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards)))
            return false;
        var actor = originalActor;
        foreach (var changed in changes)
        {
            if (changed.PreviousActorSeat != actor || changed.OwnerSeat != actor || changed.ProviderSeat != action.ProviderSeat || changed.FrameId <= use.Id || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(changed.ActorSeat) || changed.ActorSeat == actor || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Any(e => e.FrameId == changed.FrameId && e.SkillId == changed.SkillId && e.OwnerSeat == changed.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized))
                return false;
            actor = changed.ActorSeat;
        }

        return actor == action.ActorSeat && actor == use.SourceSeat;
    }

    internal void AssertShownEntityPolicyOrigin(ShownEntityTurnPolicy p)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program;
        var trigger = program?.Triggers.SingleOrDefault(t => t.Id == p.Source.BindingId);
        var chromatic = p.FrozenSuit is Suit.Spade or Suit.Club or Suit.Heart or Suit.Diamond;
        if (program?.GameplayHash != p.GameplayHash || trigger?.Effects is not [_, _, { Op: SkillProgramEffectOp.IssueShownEntityTurnPolicy, StateId: { } state }] || state != p.StateId || p.EffectIndex != 2 || p.ProgramFrameId <= 0 || p.ActualPlayStartingFrameId <= 0 || p.ActualPlayStartingFrameId >= p.ProgramFrameId || p.TurnNumber < 1 || p.ActualTurnOwnerSeat != p.Source.OwnerSeat || p.OriginalFrom != CardLocation.Hand(p.Source.OwnerSeat) || !Enum.IsDefined(p.FrozenSuit) || p.AffectedSeats is not System.Collections.IList { IsReadOnly: true } || p.RestrictionSequences is not System.Collections.IList { IsReadOnly: true } || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.OwnerSeat == p.Source.OwnerSeat && e.SkillId == p.Source.SkillId && e.BindingId == p.Source.BindingId && e.SkillInstanceId == p.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.PlayPhaseStarting) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCardsRevealedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.OwnerSeat == p.Source.OwnerSeat && e.SkillId == p.Source.SkillId && e.BindingId == p.Source.BindingId && e.Cards.Count == 1 && e.Cards[0].Id == p.CardId) != 1 || p.RestrictionSequences.Count != (chromatic ? p.AffectedSeats.Count : 0))
            throw new InvalidOperationException("Shown entity policy lost its true source/reveal and actual own Play-start issuance.");
        for (var i = 0; i < p.RestrictionSequences.Count; i++)
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HandCardColorRestrictionGrantedEvent>().Count(e => e.Restriction.GrantSequence == p.RestrictionSequences[i] && e.Restriction.ParentFrameId == p.ProgramFrameId && e.Restriction.TurnNumber == p.TurnNumber && e.Restriction.TurnSeat == p.ActualTurnOwnerSeat && e.Restriction.Source == p.Source && e.Restriction.AffectedSeat == p.AffectedSeats[i] && e.Restriction.IsRed == global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(p.FrozenSuit) && e.Restriction.RequiresChromaticSuit) != 1)
                throw new InvalidOperationException("Shown entity policy lost an exact public frozen recipient/color restriction.");
    }

    internal void AssertShownEntityTurnPrograms()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            AssertShownEntityUseBenefits(use);
    }

    void global::CardGame.Core.IShownEntityTurnPolicyHost.IssueShownEntityTurnPolicy(ProgramSkillFrame f, string bind, string stateId) => IssueShownEntityTurnPolicy(f, bind, stateId);
}
