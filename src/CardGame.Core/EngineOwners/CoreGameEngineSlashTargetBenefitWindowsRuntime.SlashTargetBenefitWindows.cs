#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashTargetBenefitWindowsRuntime
{
    internal bool HasSlashTargetBenefitCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferSlashTargetBenefit);

    internal static bool IsSlashTargetBenefitWindow(SkillProgramTriggerWindow w) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SlashTargetBenefitWindows.IsSlashTargetBenefitWindow(w);
    internal ActualUseTargetIdentity? FreezeSlashTargetBenefitUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int? designatedTarget = null)
    {
        var target = designatedTarget ?? attack.TargetSeat;
        if (!HasSlashTargetBenefitCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !use.TargetSeats.Contains(target) || attack.IsSourceLess || attack.CardUserSeat != use.SourceSeat || attack.SourceSeat != use.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            return null;
        if (use.Action is { Type: CardActionType.Use } action && action.ActorSeat == use.SourceSeat && action.EffectiveKind == use.CardKind)
            return new(use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, null);
        return use.Action is null && SlashBenefitLegacyProducer(use, target)is { } parent ? new(use.Id, null, use.SourceSeat, use.SourceSeat, use.CardKind, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, parent) : null;
    }

    internal bool MatchesSlashTargetBenefitUse(ActualUseTargetIdentity identity)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is not { } use || use.CardKind != identity.EffectiveKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || identity.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || identity.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return false;
        if (!use.TargetSeats.Contains(identity.TargetSeat) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetBenefitOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ActorSeat == identity.ActorSeat && e.TargetSeat == identity.TargetSeat && (use.SlashTargetBenefits ?? []).Any(r => r.Use == identity && r.OfferWindowId == e.WindowId && r.OfferCandidateIndex == e.CandidateIndex)))
            return false;
        if (identity.ActionId is null)
            return identity.LegacyProducerProgramId is { } producer && use.Action is null && use.SourceSeat == identity.ActorSeat && SlashBenefitLegacyProducer(use, identity.TargetSeat, issued: true) == producer;
        if (identity.LegacyProducerProgramId is not null || use.Action is not { Type: CardActionType.Use } action || action.ActionId != identity.ActionId || action.ActorSeat != use.SourceSeat || action.ProviderSeat != identity.ProviderSeat || action.EffectiveKind != identity.EffectiveKind || !(use.PhysicalCardIds ?? []).SequenceEqual(action.PhysicalCards.Select(c => c.CardId)))
            return false;
        if (use.SourceSeat == identity.ActorSeat)
            return true;
        // This is a read-only causal chain, never a new actor/source or fake Action.
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declaration = history.OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId).ToArray();
        if (declaration is not [var declared] || !history.OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.Type == CardActionType.Use && e.Action.ProviderSeat == identity.ProviderSeat && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards)))
            return false;
        var cursor = declared.SourceSeat;
        var passedIssuer = cursor == identity.ActorSeat;
        foreach (var change in history.OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
        {
            if (change.PreviousActorSeat != cursor || change.OwnerSeat != cursor || change.ProviderSeat != identity.ProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat) || change.ActorSeat == cursor || !history.OfType<ProgramBindingStartedEvent>().Any(e => e.FrameId == change.FrameId && e.SkillId == change.SkillId && e.OwnerSeat == change.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized))
                return false;
            cursor = change.ActorSeat;
            passedIssuer |= cursor == identity.ActorSeat;
        }

        return passedIssuer && cursor == use.SourceSeat;
    }

    internal long? SlashBenefitLegacyProducer(CardUseFrame use, int target, bool issued = false)
    {
        if (use.CardAttack is not { ProgramSkillCardUseFrameId: { } producer } attack || !_coreSameTypeActualUseAidOperations.MatchesSameTypeAidLegacyProducer(use, producer))
            return null;
        var declaration = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Single(e => e.ResolutionId == use.Id);
        // This new root needs all finalized targets, not only the currently active
        // damage target. Added targets retain the mature public same-use fact.
        if (declaration.TargetSeats.Contains(target) || attack.TargetSeat == target || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameTypeAidTargetAddedEvent>().Any(e => e.RecipientSeat == target && _coreSameTypeActualUseAidOperations.IsSameTypeAidTargetFact(use, e)) || issued && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetBenefitOfferedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ActorSeat == use.SourceSeat && e.TargetSeat == target))
            return producer;
        return null;
    }

    internal SlashTargetBenefitReceipt? SlashTargetBenefitCurrent(SlashTargetBenefitReceipt identity) => _coreCardUseLifecycleOperations.LifecycleCardUse(identity.Use.CardUseFrameId)?.SlashTargetBenefits?.SingleOrDefault(r => r.OfferWindowId == identity.OfferWindowId && r.OfferCandidateIndex == identity.OfferCandidateIndex);
    internal void ReplaceSlashTargetBenefit(SlashTargetBenefitReceipt receipt) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(receipt.Use.CardUseFrameId, u => u with { SlashTargetBenefits = Array.AsReadOnly((u.SlashTargetBenefits ?? []).Select(r => r.OfferWindowId == receipt.OfferWindowId && r.OfferCandidateIndex == receipt.OfferCandidateIndex ? receipt : r).ToArray()) });
    internal bool TryBeginSlashTargetBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false)
    {
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || FreezeSlashTargetBenefitUse(attack)is not { } identity || !_coreMatchParticipantsPlayersAccess._players[identity.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[identity.TargetSeat].IsAlive)
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)!;
        var facts = use.Action is { } a ? _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[identity.ActorSeat], a) : _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[identity.ActorSeat]);
        var eligible = _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[identity.ActorSeat], SkillProgramTriggerWindow.ActualSlashTargetBenefit, facts).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.OfferSlashTargetBenefit }]).ToArray();
        var entries = new List<(ProgramTriggerCandidate Candidate, ActualUseTargetIdentity Identity)>();
        // The finalized list is captured before the FIRST target effect. Each target's
        // optional benefit completes here, rather than waiting for prior damage/death.
        // Any later actual addition is admitted once by its original unvisited key.
        foreach (var target in use.TargetSeats.Distinct())
        {
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || FreezeSlashTargetBenefitUse(attack, target)is not { } targetIdentity)
                continue;
            foreach (var c in eligible)
                if (!(use.SlashTargetBenefits ?? []).Any(r => r.Use.ActorSeat == c.OwnerSeat && r.Use.TargetSeat == target && r.Source.SkillId == c.SkillId && r.Source.BindingId == c.BindingId))
                    entries.Add((c, targetIdentity));
        }

        if (entries.Count == 0)
            return false;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var contexts = new List<ProgramSkillWindowContext>();
        var receipts = new List<SlashTargetBenefitReceipt>();
        for (var i = 0; i < entries.Count; i++)
        {
            var(c, targetIdentity) = entries[i];
            var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(c);
            var receipt = new SlashTargetBenefitReceipt(id, i, targetIdentity, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, trigger.Effects[0].StateId!, SlashTargetBenefitStage.Offered);
            receipts.Add(receipt);
            contexts.Add(new(SkillProgramTriggerWindow.ActualSlashTargetBenefit, id, c.OwnerSeat, SourceSeat: targetIdentity.ActorSeat, TargetSeat: targetIdentity.TargetSeat, OccurrenceIndex: c.OccurrenceIndex) { SlashTargetBenefit = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetBenefitOfferedEvent(use.Id, id, i, targetIdentity.ActorSeat, targetIdentity.TargetSeat, receipt.Source, receipt.GameplayHash));
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { SlashTargetBenefits = Array.AsReadOnly((u.SlashTargetBenefits ?? []).Concat(receipts).ToArray()) });
        var returnKind = afterActualTargets ? legacy ? SlashTargetBenefitReturn.AfterActualTargetsLegacy : SlashTargetBenefitReturn.AfterActualTargetsSlash : legacy ? SlashTargetBenefitReturn.LegacyVirtualSlash : SlashTargetBenefitReturn.FinalizedSlash;
        _coreRuntimeOperations.PushRuntimeFrame(new SlashTargetBenefitWindowFrame(id, use.Id, returnKind, Array.AsReadOnly(entries.Select(e => e.Candidate).ToArray()), Array.AsReadOnly(contexts.ToArray())));
        _coreRuntimeOperations.AdvanceRuntimeTop<SlashTargetBenefitWindowFrame>();
        return true;
    }

    internal bool TryBeginDodgeCancelledSlashBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        if (use is null || !HasSlashTargetBenefitCapability || attack.SuccessfulDodgeResponses < attack.RequiredDodgeResponses || _coreSlashSuitDiscardOperations.IsSlashDodgeCancellationPrevented(attack))
            return false;
        var offered = (use.SlashTargetBenefits ?? []).Where(r => r.Stage == SlashTargetBenefitStage.Paid && r.Use.TargetSeat == attack.TargetSeat).ToArray();
        if (offered.Length == 0)
            return false;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var candidates = new List<ProgramTriggerCandidate>();
        var contexts = new List<ProgramSkillWindowContext>();
        foreach (var r in offered)
        {
            if (!_coreSlashTargetBenefitObserversOperations.ValidSlashTargetBenefitReceipt(r) || !MatchesSlashTargetBenefitUse(r.Use))
                throw new InvalidOperationException("A Dodge cancellation lost its issued same-use benefit.");
            var qualified = r with
            {
                Stage = SlashTargetBenefitStage.CancellationQualified
            };
            ReplaceSlashTargetBenefit(qualified);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetBenefitCancellationEvent(use.Id, r.OfferWindowId, r.OfferCandidateIndex, r.Use.ActorSeat, r.Use.TargetSeat));
            var candidate = new ProgramTriggerCandidate(r.Source.OwnerSeat, r.Source.SkillId, r.SettlementBinding, r.Source.SkillInstanceId!, r.GameplayHash, 0);
            candidates.Add(candidate);
            contexts.Add(new(SkillProgramTriggerWindow.SlashDodgeCancelledBenefit, id, candidate.OwnerSeat, SourceSeat: r.Use.ActorSeat, TargetSeat: r.Use.TargetSeat) { SlashTargetBenefit = qualified });
        }

        _coreRuntimeOperations.PushRuntimeFrame(new SlashTargetBenefitWindowFrame(id, use.Id, SlashTargetBenefitReturn.DodgeCancelled, Array.AsReadOnly(candidates.ToArray()), Array.AsReadOnly(contexts.ToArray())));
        _coreRuntimeOperations.AdvanceRuntimeTop<SlashTargetBenefitWindowFrame>();
        return true;
    }

    internal bool CanRunSlashTargetBenefitCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsSlashTargetBenefitWindow(context.Window) || context.SlashTargetBenefit is not { } identity || SlashTargetBenefitCurrent(identity)is not { } receipt || receipt.Source.SkillId != c.SkillId || receipt.Source.OwnerSeat != c.OwnerSeat || receipt.Source.SkillInstanceId != c.SkillInstanceId || receipt.GameplayHash != c.GameplayHash || !MatchesSlashTargetBenefitUse(receipt.Use) || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program is not { } definition || definition.GameplayHash != c.GameplayHash || ProgramInstructionResolver.Default.FindTrigger(definition, c.BindingId)is not { } trigger || trigger.Window != context.Window)
            return false;
        if (context.Window == SkillProgramTriggerWindow.ActualSlashTargetBenefit)
            return receipt.Stage == SlashTargetBenefitStage.Offered && c.BindingId == receipt.Source.BindingId && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[receipt.Use.TargetSeat].IsAlive && _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.Use.CardUseFrameId)!.TargetSeats.Contains(receipt.Use.TargetSeat) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId);
        return receipt.Stage == SlashTargetBenefitStage.CancellationQualified && receipt.SettlementBinding == c.BindingId && _coreSlashTargetBenefitObserversOperations.ValidSlashTargetBenefitReceipt(receipt);
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 110, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(SlashTargetBenefitWindowFrame), 240)]
    internal void ContinueSlashTargetBenefitWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is SlashTargetBenefitWindowFrame w)
        {
            if (w.CandidateIndex >= w.Candidates.Count)
            {
                _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.SlashTargetBenefitWindow);
                var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
                if (attack is null || attack.ResolutionId != w.ParentFrameId)
                    throw new InvalidOperationException("A Slash benefit return lost its exact attack.");
                if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
                {
                    _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                }
                else if (w.ReturnKind == SlashTargetBenefitReturn.DodgeCancelled)
                    _coreNativeDodgeOperations.ContinueSlashAfterDodgeCancellation(attack);
                else if (w.ReturnKind is SlashTargetBenefitReturn.AfterActualTargetsSlash or SlashTargetBenefitReturn.AfterActualTargetsLegacy)
                    ContinueSlashAfterTargetBenefitAdditions(attack, w.ReturnKind == SlashTargetBenefitReturn.AfterActualTargetsLegacy);
                else if (w.ReturnKind == SlashTargetBenefitReturn.LegacyVirtualSlash)
                {
                    if (!_coreSlashTargetPenaltyWindowsOperations.TryBeginSlashTargetPenalties(attack, legacy: true) && !_coreActualUseTargetProgramsOperations.TryBeginActualUseTargetPrograms(attack, ActualUseTargetReturnKind.LegacyVirtualSlash))
                        _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack);
                }
                else
                    _coreNativeSlashUseOperations.ContinueSlashAfterFinalizedTargets(attack);
                return;
            }

            var c = w.Candidates[w.CandidateIndex];
            var context = w.Contexts[w.CandidateIndex];
            if (!CanRunSlashTargetBenefitCandidate(c, context))
            {
                var receipt = SlashTargetBenefitCurrent(context.SlashTargetBenefit!);
                if (receipt is not null)
                    ReplaceSlashTargetBenefit(receipt with { Stage = receipt.Stage == SlashTargetBenefitStage.CancellationQualified ? SlashTargetBenefitStage.Cancelled : SlashTargetBenefitStage.Declined });
                _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(w with { Step = ResolutionFrameStep.AwaitingResponse });
            _coreProgramLifecycleOperations.BeginProgramBinding(c, context);
            return;
        }
    }

    internal void ContinueSlashAfterTargetBenefitAdditions(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy)
    {
        // New gains can themselves add another genuine target. Refill only this
        // capability's unvisited benefit keys before entering the original effect.
        if (_coreSlashTargetPenaltyWindowsOperations.TryBeginSlashTargetPenalties(attack, legacy, afterActualTargets: true))
            return;
        if (TryBeginSlashTargetBenefits(attack, legacy, afterActualTargets: true))
            return;
        if (_coreUniqueHpTargetAnnouncementsOperations.TryBeginUnannouncedUniqueHpTargets(attack, legacy))
            return;
        if (legacy)
        {
            if (_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(attack.ResolutionId, attack.TargetSeat))
            {
                _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }
            else
                _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack);
        }
        else if (!_coreCardActionsOperations.TryBeginProgramCardUseBeforeTargetEffects(attack))
            _coreNativeSlashUseOperations.ContinueSlashAfterProgramTargetEffects(attack);
    }

    internal void CompleteSlashTargetBenefitBinding(ProgramSkillFrame f)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not SlashTargetBenefitWindowFrame w || w.Id != f.WindowContext?.ParentFrameId || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
            throw new InvalidOperationException("A Slash benefit child lost its exact candidate return.");
        var receipt = SlashTargetBenefitCurrent(w.Contexts[w.CandidateIndex].SlashTargetBenefit!);
        if (receipt?.Stage == SlashTargetBenefitStage.Offered)
            ReplaceSlashTargetBenefit(receipt with { Stage = SlashTargetBenefitStage.Declined });
        if (receipt?.Stage == SlashTargetBenefitStage.CancellationQualified)
            ReplaceSlashTargetBenefit(receipt with { Stage = SlashTargetBenefitStage.Cancelled });
        _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
        _coreRuntimeOperations.AdvanceRuntimeTop<SlashTargetBenefitWindowFrame>();
    }

    [ProgramIssuedTrigger(720, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger720(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsSlashTargetBenefitWindow(context.Window) ? CanRunSlashTargetBenefitCandidate(candidate, context) : null;
}
