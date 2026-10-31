#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengJiChengCuiOlRuntime
{
    internal bool XumingDamageEligible(int owner, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.BeforeDamageApplied && c.Amount > 0 && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } a && _coreTableSeatQueriesOperations.IsValidPlayerSeat(a.SourceSeat) && a.SourceSeat != a.TargetSeat && (a.SourceSeat == owner || a.TargetSeat == owner) && c.SourceSeat == a.SourceSeat && c.TargetSeat == a.TargetSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId) is BeforeDamageProgramWindowFrame;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32201, SkillProgramEffectOp.ResolveXumingDamagePromise)]
    internal bool CanXumingDamage(int owner, ProgramSkillWindowContext c) => XumingDamageEligible(owner, c);
    internal SkillProgramStepOutcome ResolveXumingDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext!;
        if (!XumingDamageEligible(f.OwnerSeat, c) || f.InstructionIndex != 1 || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not BeforeDamageProgramWindowFrame w || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack)
            throw new InvalidOperationException("Xuming requires the original other-character native damage attempt.");
        var source = ChengJiSource(f);
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<XumingPromiseIssuedEvent>().Any(e => e.DamageWindowId == w.Id && e.Source.OwnerSeat == f.OwnerSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<XumingDamageIncreasedEvent>().Any(e => e.DamageWindowId == w.Id && e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Xuming cannot process the same owner's damage attempt twice.");
        // Alternative costs are payment entities, not the corresponding card of
        // this virtual Slash. In particular Tousui's bottom cards cause no damage.
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var materials = attack.PhysicalCards.Where(card => card.Id > 0 && !_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(use?.Action, card.Id)).DistinctBy(card => card.Id).ToArray();
        if (materials.Length == 0)
        {
            attack.IncreaseFinalizedDamageAmount(1);
            _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { Amount = checked(w.Amount + 1) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XumingDamageIncreasedEvent(f.Id, source, f.GameplayHash, attack.ResolutionId, w.Id, w.Amount, w.Amount + 1));
            return SkillProgramStepOutcome.Continue;
        }

        var actor = attack.SourceSeat == f.OwnerSeat ? attack.TargetSeat : attack.SourceSeat;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XumingPromiseIssuedEvent(w.Id, source, f.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, attack.ResolutionId, w.Id, actor, f.OwnerSeat, materials.Length));
        for (var index = 0; index < materials.Length; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XumingMaterialFrozenEvent(w.Id, source, index, materials[index].Id, materials[index].Kind));
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<XumingPromiseIssuedEvent> PendingXumingPromises(int owner) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<XumingPromiseIssuedEvent>().Where(p => p.Source.OwnerSeat == owner && p.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && p.TurnOwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<XumingPromiseSettledEvent>().Any(e => e.PromiseId == p.PromiseId && e.Source == p.Source && e.ActualTurn == p.ActualTurn)).ToArray());
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeXumingAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (!HasChengJiChengCui || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(ChengJiChengCuiComposition.Xuming)?.Program is not { } program)
            return ordinary;
        var trigger = program.Triggers.Single(t => t.Id == "issued-whole-turn-uses");
        var added = new List<TurnEndingBoundaryItem>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var first in PendingXumingPromises(owner.Seat).Where(p => p.ActualTurn == parent.TurnNumber && p.TurnOwnerSeat == parent.OwnerSeat).DistinctBy(p => (p.Source.SkillInstanceId, p.GameplayHash)))
            {
                bool SameSource(TurnEndingBoundaryItem i) => i.Candidate is { } c && c.OwnerSeat == owner.Seat && c.SkillId == ChengJiChengCuiComposition.Xuming && c.SkillInstanceId == first.Source.SkillInstanceId && c.GameplayHash == first.GameplayHash;
                var identity = $"xuming:{(owner.Seat)}:{(first.Source.SkillInstanceId)}:promise-{(first.PromiseId)}";
                if (ordinary.Any(i => SameSource(i) && i.StableIdentity == identity) || ordinary.Skip(parent.AfterTurnEnded?.ItemIndex ?? 0).Any(SameSource))
                    continue;
                var occurrence = ordinary.Count(SameSource);
                var c = new ProgramTriggerCandidate(owner.Seat, ChengJiChengCuiComposition.Xuming, trigger.Id, first.Source.SkillInstanceId, first.GameplayHash, trigger.Priority, occurrence);
                added.Add(new(TurnEndingBoundaryItemKind.Program, trigger.Priority, identity, c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner)));
            }

        if (added.Count == 0)
            return ordinary;
        var ordered = added.OrderBy(i => _coreTableOrderOperations.RelativeTableOrder(parent.OwnerSeat, i.Candidate!.OwnerSeat)).ThenByDescending(i => i.Priority).ThenBy(i => i.StableIdentity, StringComparer.Ordinal);
        return Array.AsReadOnly((parent.AfterTurnEnded is null ? ordinary.Concat(ordered).OrderBy(i => _coreTableOrderOperations.RelativeTableOrder(parent.OwnerSeat, i.Candidate!.OwnerSeat)).ThenByDescending(i => i.Priority).ThenBy(i => i.StableIdentity, StringComparer.Ordinal) : ordinary.Concat(ordered)).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32202, SkillProgramEffectOp.ResolveXumingIssuedTurnEndUses)]
    internal bool CanOfferXumingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedXumingCandidate(candidate, context);
    [ProgramIssuedTrigger(340)]
    internal bool IsIssuedXumingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == ChengJiChengCuiComposition.Xuming && c.BindingId == "issued-whole-turn-uses" && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.OwnerSeat == c.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is DeferredTurnEndFrame parent && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) == c && context.SourceSeat == parent.OwnerSeat && PendingXumingPromises(c.OwnerSeat).Any(p => p.Source.SkillInstanceId == c.SkillInstanceId && p.GameplayHash == c.GameplayHash && p.ActualTurn == parent.TurnNumber && p.TurnOwnerSeat == parent.OwnerSeat);
    internal SkillProgramStepOutcome BeginXumingEnding(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XumingEnding is not null)
        {
            ContinueXumingEnding(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.InstructionIndex != 1 || f.WindowContext is not { } c || !IsIssuedXumingCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c))
            throw new InvalidOperationException("Xuming lost its issued original actual-turn-end candidate.");
        var promises = PendingXumingPromises(f.OwnerSeat).Where(p => p.Source.SkillInstanceId == f.SkillInstanceId && p.GameplayHash == f.GameplayHash).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XumingEnding = new(ChengJiSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, c.ParentFrameId, XumingStage.Choosing) { Promises = promises } });
        ContinueXumingEnding(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<Card> XumingCurrentMaterials(XumingPromiseIssuedEvent p)
    {
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<XumingMaterialFrozenEvent>().Where(e => e.PromiseId == p.PromiseId && e.Source == p.Source).OrderBy(e => e.Index).ToArray();
        if (facts.Length != p.MaterialCount || !facts.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, p.MaterialCount)) || facts.Select(e => e.CardId).Distinct().Count() != facts.Length || facts.Any(e => e.CardId <= 0 || _cardZonesOperations._cardZones.GetLocation(e.CardId).Zone is not (CardZoneKind.Processing or CardZoneKind.DiscardPile)))
            return[];
        var cards = facts.Select(e => _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(e.CardId)).ToArray();
        return cards.Where((card, i) => card.Kind != facts[i].PrintedKind).Any() ? [] : Array.AsReadOnly(cards);
    }

    internal IReadOnlyList<XumingUsePlan> XumingPlans(ProgramSkillFrame f)
    {
        var r = f.XumingEnding!;
        if (r.PromiseIndex >= r.Promises.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var p = r.Promises[r.PromiseIndex];
        var cards = XumingCurrentMaterials(p);
        if (!_coreMatchParticipantsPlayersAccess._players[p.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[p.TargetSeat].IsAlive || cards.Count != p.MaterialCount)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[p.ActorSeat];
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards);
        var result = new List<XumingUsePlan>();
        foreach (var kind in new[]
        {
            CardKind.BorrowedSword,
            CardKind.Dismantlement
        }

        )
        {
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
                continue;
            foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: cards.Select(c => c.Id).ToArray(), conversionSource: r.Source))
            {
                if (option.TargetSeats.Count == 0 || option.TargetSeats[0] != p.TargetSeat || kind == CardKind.Dismantlement && option.TargetSeats.Count != 1)
                    continue;
                result.Add(new(kind, option.ActionKind, option.TargetCardId, option.RequiredCardKind, option.Description) { Targets = option.TargetSeats });
            }
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal IReadOnlyList<PromptChoice> XumingChoices(ProgramSkillFrame f) => Array.AsReadOnly(XumingPlans(f).Select((p, i) => new PromptChoice(new($"xuming.{(f.Id)}.{(f.XumingEnding!.PromiseIndex)}.{(i)}"), p.Description, XumingCurrentMaterials(f.XumingEnding.Promises[f.XumingEnding.PromiseIndex]).Select(c => c.Id).ToArray(), p.Targets, new Dictionary<string, string> { ["program-action"] = "xuming-issued-use", ["frame-id"] = f.Id.ToString(), ["plan"] = i.ToString() })).ToArray());
    internal void SettleXumingPromise(ProgramSkillFrame f, bool used, long? useId = null)
    {
        var r = f.XumingEnding!;
        var p = r.Promises[r.PromiseIndex];
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<XumingPromiseSettledEvent>().Any(e => e.PromiseId == p.PromiseId && e.Source == p.Source))
            throw new InvalidOperationException("Xuming cannot settle an original damage obligation twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XumingPromiseSettledEvent(f.Id, p.PromiseId, p.Source, r.ActualTurn, used, useId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { XumingEnding = r with { PromiseIndex = r.PromiseIndex + 1, Stage = XumingStage.Choosing, UseId = null, ActionId = null, Plan = null } });
    }

    internal void ContinueXumingEnding(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XumingEnding: { } r } f || f.Id != id)
            return;
        if (r.Stage == XumingStage.NativeUse)
            return;
        while (f.XumingEnding!.PromiseIndex < f.XumingEnding.Promises.Count)
        {
            var plans = XumingPlans(f);
            if (plans.Count == 0)
            {
                SettleXumingPromise(f, false);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                continue;
            }

            var p = f.XumingEnding.Promises[f.XumingEnding.PromiseIndex];
            var choices = XumingChoices(f);
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, p.ActorSeat, "畜鸣：将原伤害牌完整当借刀或过河拆桥使用。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices,
                TargetSeat = p.TargetSeat,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[p.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XumingEnding = f.XumingEnding!with { Stage = XumingStage.Complete } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XumingCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveXumingIssuedTurnEndUses)]
    internal PromptChoice AiXumingUse(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => c.Parameters["plan"], StringComparer.Ordinal).First();
    [ProgramContinuation(ProgramContinuationKind.Resume, 32202)]
    internal bool ResumeXumingEnding(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XumingEnding: not null } f || f.Id != id)
            return false;
        ContinueXumingEnding(id);
        return true;
    }
}
