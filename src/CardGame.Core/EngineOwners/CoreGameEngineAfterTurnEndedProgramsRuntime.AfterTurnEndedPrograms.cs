#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsRuntime
{
    internal bool HasAfterTurnEndedPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded);

    internal bool IsActualAfterTurnEndedParent(DeferredTurnEndFrame parent) => HasAfterTurnEndedPrograms && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && parent.Prelude is { Completed: true } && parent.Current is null && parent.ItemIndex == parent.DueIds.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == parent.TurnNumber && e.ActorSeat == parent.OwnerSeat);
    internal ProgramTriggerCandidate AfterTurnEndedCandidate(DeferredTurnEndFrame parent)
    {
        if (!IsActualAfterTurnEndedParent(parent) || parent.AfterTurnEnded is not { } window || window.ItemIndex < 0 || window.ItemIndex >= window.Items.Count || window.Items[window.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate, Facts: not null })
            throw new InvalidOperationException("An after-turn-ended window lost its actual end or frozen candidate.");
        return candidate;
    }

    internal ProgramSkillWindowContext CreateAfterTurnEndedContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate) => _characterCaiFuRenOlOperations.CreateOrdinaryCaiFuRenQietingContext(parent, candidate, _characterCaoYuOlOperations.CreateXiangxuAfterTurnEndedContext(parent, candidate, new ProgramSkillWindowContext(SkillProgramTriggerWindow.AfterTurnEnded, parent.Id, candidate.OwnerSeat, SourceSeat: parent.OwnerSeat, TargetSeat: parent.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts) { ActualEndedEquipment = _coreActualEndedTurnEquipmentOperations.IsActualEndedEquipmentTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _coreActualEndedTurnEquipmentOperations.CaptureActualEndedEquipmentContext(parent) : null }));
    internal bool CanRunAfterTurnEndedCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger)
    {
        var parent = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault();
        if (_characterXunYuMouOperations.IsIssuedMouXyAllocationCandidate(candidate, context) || _characterChengJiChengCuiOlOperations.IsIssuedXumingCandidate(candidate, context) || _characterHeQiMobileShiOperations.IsIssuedShiHqCleanupCandidate(candidate, context) || _characterCaoZhenMobileShiOperations.IsIssuedShiCaoZEndCandidate(candidate, context))
            return true;
        if (_characterCaiFuRenOlOperations.IsOrdinaryCaiFuRenQietingTrigger(trigger))
            return parent is not null && _characterCaiFuRenOlOperations.IsEligibleOrdinaryCaiFuRenQietingCandidate(parent, candidate, context);
        if (_coreActualEndedTurnEquipmentOperations.IsActualEndedEquipmentTrigger(trigger))
            return parent is not null && _coreActualEndedTurnEquipmentOperations.MatchesActualEndedEquipmentContext(parent, candidate, context, trigger);
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && trigger.Window == context.Window && parent is not null && IsActualAfterTurnEndedParent(parent) && parent.Id == context.ParentFrameId && AfterTurnEndedCandidate(parent) == candidate && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == candidate.OccurrenceIndex && context.Facts == parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts && (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.AnyLiving || (trigger.TurnOwnerScope == SkillProgramTurnOwnerScope.Own ? candidate.OwnerSeat == parent.OwnerSeat : candidate.OwnerSeat != parent.OwnerSeat && (_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive || _characterWenYangOlOperations.IsWenYangChoujueCandidate(candidate, context) || _characterAHuiNanOlOperations.IsAHuiNanJuemanEndingCandidate(candidate, context) || _characterQinLangOlOperations.IsIssuedQinLangXianyingCandidate(candidate, context) || _characterCaoYuOlOperations.IsIssuedXiangxuEndingCandidate(candidate, context) || trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.LoseHpForLostSourceCurses or SkillProgramEffectOp.DiscardForeignTurnHandGains or SkillProgramEffectOp.ResolveDailiTurnEnd or SkillProgramEffectOp.ResolveGoudeActualTurnEnd or SkillProgramEffectOp.ResolveZhijieActualTurnEnd or SkillProgramEffectOp.ResolveWuLuKangKegou or SkillProgramEffectOp.ResolveMouKongRongLiwen or SkillProgramEffectOp.PrepareMouLiuXieChensheng or SkillProgramEffectOp.ResolveMouXunYuAnniversaryBizuo or SkillProgramEffectOp.ResolveMouGuoJiaXianmou || e.NumberExpression == SkillProgramNumberExpression.ActualTurnDeathBenefits))));
    }

    internal bool ContinueAfterTurnEndedPrograms(DeferredTurnEndFrame parent)
    {
        if (!HasAfterTurnEndedPrograms)
            return false;
        if (!IsActualAfterTurnEndedParent(parent))
            throw new InvalidOperationException("After-turn-ended programs require the completed actual turn and prior due chain.");
        if (parent.AfterTurnEnded is null)
        {
            var items = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.AfterTurnEnded).Count != 0).SelectMany(owner =>
            {
                var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
                if (_coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.AfterTurnEnded).Any(t => t.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawFudaoRecordedPair)))
                    facts = facts with
                    {
                        EventTargetHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat]).Count
                    };
                return _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.AfterTurnEnded, facts).Where(c => !_coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment) && (_coreProgramLifecycleOperations.GetProgramTrigger2(c).TurnOwnerScope == SkillProgramTurnOwnerScope.AnyLiving || _coreProgramLifecycleOperations.GetProgramTrigger2(c).TurnOwnerScope == (owner.Seat == parent.OwnerSeat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving))).Select(c => new TurnEndingBoundaryItem(TurnEndingBoundaryItemKind.Program, c.Priority, $"program:{(c.SkillId)}:{(c.BindingId)}:{(c.SkillInstanceId)}", c, facts));
            }).OrderBy(i => _coreTableOrderOperations.RelativeTableOrder(parent.OwnerSeat, i.Candidate!.OwnerSeat)).ThenByDescending(i => i.Priority).ThenBy(i => i.StableIdentity, StringComparer.Ordinal).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { AfterTurnEnded = new(_characterCaoZhenMobileShiOperations.IncludeShiCaoZAfterTurnEndedItems(parent, _characterHeQiMobileShiOperations.IncludeShiHqAfterTurnEndedItems(parent, _characterZhangFeiAnniversaryGodOperations.IncludeShenZfAfterTurnEndedItems(parent, _characterXunYuMouOperations.IncludeMouXyAfterTurnEndedItems(parent, _characterChengJiChengCuiOlOperations.IncludeXumingAfterTurnEndedItems(parent, _characterHuangfuSongWumiaoOperations.IncludeWuHfEndingItems(parent, _characterCaoYuOlOperations.IncludeXiangxuAfterTurnEndedItems(parent, _characterQinLangOlOperations.IncludeQinLangXianyingAfterTurnEndedItems(parent, Array.AsReadOnly(items)))))))))) });
        }

        if (parent.AfterTurnEnded.CurrentChild is null)
        {
            var withLateXiangxu = _characterCaoZhenMobileShiOperations.IncludeShiCaoZAfterTurnEndedItems(parent, _characterHeQiMobileShiOperations.IncludeShiHqAfterTurnEndedItems(parent, _characterXunYuMouOperations.IncludeMouXyAfterTurnEndedItems(parent, _characterChengJiChengCuiOlOperations.IncludeXumingAfterTurnEndedItems(parent, _characterHuangfuSongWumiaoOperations.IncludeWuHfEndingItems(parent, _characterCaoYuOlOperations.IncludeXiangxuAfterTurnEndedItems(parent, parent.AfterTurnEnded.Items))))));
            if (!ReferenceEquals(withLateXiangxu, parent.AfterTurnEnded.Items))
                _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { AfterTurnEnded = parent.AfterTurnEnded with { Items = withLateXiangxu } });
        }

        while (parent.AfterTurnEnded!.ItemIndex < parent.AfterTurnEnded.Items.Count)
        {
            if (parent.AfterTurnEnded.CurrentChild is not null)
                throw new InvalidOperationException("An after-turn-ended cursor retained its completed child.");
            var candidate = AfterTurnEndedCandidate(parent);
            var context = CreateAfterTurnEndedContext(parent, candidate);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(candidate, context))
            {
                AdvanceAfterTurnEndedCandidate(parent, candidate, activated: false, completed: false);
                parent = (DeferredTurnEndFrame)_resolutionStackOperations._resolutionStack[^1];
                continue;
            }

            if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Optional)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Step = ResolutionFrameStep.AwaitingResponse });
                _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(candidate, context);
                return true;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
            return true;
        }

        return false;
    }

    internal void AttachAfterTurnEndedChild(ProgramSkillFrame child, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterTurnEnded)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeferredTurnEndFrame parent || parent.AfterTurnEnded?.CurrentChild is not null || !CanRunAfterTurnEndedCandidate(candidate, context, _coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            throw new InvalidOperationException("An after-turn-ended child requires its exact active candidate and immediate parent.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Step = ResolutionFrameStep.ResolvingEffect, AfterTurnEnded = parent.AfterTurnEnded!with { CurrentChild = new(child.Id, candidate) } });
    }

    internal void AdvanceAfterTurnEndedCandidate(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || AfterTurnEndedCandidate(parent) != candidate || parent.AfterTurnEnded!.CurrentChild is not null)
            throw new InvalidOperationException("An after-turn-ended candidate cannot advance another item or a live child.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(parent.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.AfterTurnEnded, activated, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Step = ResolutionFrameStep.ResolvingEffect, AfterTurnEnded = parent.AfterTurnEnded with { ItemIndex = parent.AfterTurnEnded.ItemIndex + 1 } });
    }

    internal bool MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context)
    {
        if (!IsActualAfterTurnEndedParent(parent) || parent.AfterTurnEnded?.CurrentChild is not { } current)
            return false;
        var candidate = AfterTurnEndedCandidate(parent);
        return current.FrameId == child.Id && current.Candidate == candidate && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.ParentFrameId == parent.Id && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == candidate.OccurrenceIndex && (_coreActualEndedTurnEquipmentOperations.IsActualEndedEquipmentTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) ? _coreActualEndedTurnEquipmentOperations.MatchesActualEndedEquipmentContext(parent, candidate, context, _coreProgramLifecycleOperations.GetProgramTrigger2(candidate), requireLivingTurnOwner: false) : context.Facts == parent.AfterTurnEnded.Items[parent.AfterTurnEnded.ItemIndex].Facts) && child.OwnerSeat == candidate.OwnerSeat && child.SkillId == candidate.SkillId && child.SkillInstanceId == candidate.SkillInstanceId && child.TriggerId == candidate.BindingId && child.ActivationId == candidate.BindingId && child.GameplayHash == candidate.GameplayHash;
    }

    internal bool CompleteAfterTurnEndedBinding(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context)
    {
        if (!MatchesAfterTurnEndedChild(parent, child, context))
            throw new InvalidOperationException("A finished after-turn-ended binding lost its exact child, candidate, parent or turn.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Step = ResolutionFrameStep.ResolvingEffect, AfterTurnEnded = parent.AfterTurnEnded!with { CurrentChild = null, ItemIndex = parent.AfterTurnEnded.ItemIndex + 1 } });
        _coreDeferredHandAlignmentsOperations.ContinueDeferredTurnEnd();
        return true;
    }

    internal bool HasAfterTurnEndedBoundaryFrame(DeferredTurnEndFrame parent)
    {
        var candidate = AfterTurnEndedCandidate(parent);
        if (_resolutionStackOperations._resolutionStack.Count == 1 && parent.Step == ResolutionFrameStep.AwaitingResponse && parent.AfterTurnEnded!.CurrentChild is null && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Optional && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } decision && decision.PlayerSeat == candidate.OwnerSeat && decision.SourceSeat == parent.OwnerSeat && decision.TargetSeat == parent.OwnerSeat && decision.Choices.Count == 2 && decision.Choices.All(choice => choice.Parameters.GetValueOrDefault("skill-id") == candidate.SkillId && choice.Parameters.GetValueOrDefault("binding-id") == candidate.BindingId && choice.Parameters.GetValueOrDefault("skill-instance-id") == candidate.SkillInstanceId) && decision.Choices.Select(choice => choice.Parameters.GetValueOrDefault("program-action")).Order(StringComparer.Ordinal).SequenceEqual(["activate", "skip"]))
            return true;
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1] is not ProgramSkillFrame child || child.WindowContext is not { } context || !MatchesAfterTurnEndedChild(parent, child, context))
            throw new InvalidOperationException("An after-turn-ended boundary lost its exact prompt or owning child.");
        return true;
    }
}
