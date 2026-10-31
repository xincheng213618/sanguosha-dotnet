#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiYangAnniversaryRuntime
{
    internal bool HasAnniversaryCaiYang => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ArmAnniversaryCaiYangXunji);

    internal static CardConversionSource CaiYangSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static ProgramTriggerCandidate CaiYangCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 45600)]
    internal void StartCaiYangNextTurn(TurnStartedEvent t)
    {
        if (!HasAnniversaryCaiYang || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Count == 0)
            return;
        foreach (var g in _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Select(e => e.Grant).Where(g => g.Target == t.ActorSeat && g.IssuedTurn < t.TurnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Any(e => e.GrantFrameId == g.FrameId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiDueEvent(g.FrameId, t.TurnNumber, t.ActorSeat));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 45600)]
    internal void TrackCaiYangBlackUse(CardUseAppearanceCapturedEvent e)
    {
        if (!HasAnniversaryCaiYang || e.Action is not { Type: CardActionType.Use, EffectiveSuit: Suit.Spade or Suit.Club } a || !_coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Any(d => d.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && d.Target == a.ActorSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangBlackUseEvent>().Any(f => f.ActionId == a.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangBlackUseEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, a.ActorSeat, a.ActionId));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 45601)]
    internal void ExpireCaiYangXunji(TurnEndedEvent t)
    {
        if (!HasAnniversaryCaiYang || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Count == 0)
            return;
        foreach (var d in _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Where(d => d.ActualTurn == t.TurnNumber && d.Target == t.ActorSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiConsumedEvent>().Any(e => e.GrantFrameId == d.GrantFrameId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiConsumedEvent(d.GrantFrameId, t.TurnNumber, false));
    }

    internal bool ValidCaiYangGrant(CaiYangXunjiGrant g) => g.Source.SkillId == AnniversaryCaiYangComposition.Xunji && g.Source.BindingId == AnniversaryCaiYangComposition.Arm && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Target) && g.Target != g.Source.OwnerSeat && g.FrameId > 0 && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Count(e => e.Grant == g) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 45600)]
    internal void AppendCaiYangEnding(List<TurnEndingBoundaryItem> items)
    {
        if (!HasAnniversaryCaiYang || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Count == 0)
            return;
        // Remove the ordinary skill enumeration; only actual issued next-turn obligations can enter.
        items.RemoveAll(i => i.Candidate is { SkillId: AnniversaryCaiYangComposition.Xunji, BindingId: AnniversaryCaiYangComposition.Ending });
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(AnniversaryCaiYangComposition.Xunji).Program!.Triggers.Single(t => t.Id == AnniversaryCaiYangComposition.Ending);
        foreach (var due in _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Where(d => d.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && d.Target == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiConsumedEvent>().Any(e => e.GrantFrameId == d.GrantFrameId)).ToArray())
        {
            var g = _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Single(e => e.Grant.FrameId == due.GrantFrameId).Grant;
            if (!ValidCaiYangGrant(g) || !_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(g.Target, g.Source.OwnerSeat) || !_coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangBlackUseEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == g.Target))
                continue;
            var c = new ProgramTriggerCandidate(g.Source.OwnerSeat, g.Source.SkillId, trigger.Id, g.Source.SkillInstanceId, g.Hash, trigger.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, c.Priority, "cai-yang-xunji:" + g.FrameId, c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }
    }

    internal CaiYangXunjiGrant? CaiYangEndingWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool begun = false)
    {
        if (c.SkillId != AnniversaryCaiYangComposition.Xunji || c.BindingId != AnniversaryCaiYangComposition.Ending || w.Window != SkillProgramTriggerWindow.TurnEnding || w.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || w.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(x => x.Id == w.ParentFrameId)is not { } p || p.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || p.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate != c)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Select(e => e.Grant).SingleOrDefault(g => p.Items[p.ItemIndex].StableIdentity == "cai-yang-xunji:" + g.FrameId && g.Source.OwnerSeat == c.OwnerSeat && g.Source.SkillInstanceId == c.SkillInstanceId && g.Hash == c.GameplayHash && g.Target == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && ValidCaiYangGrant(g) && _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDueEvent>().Count(d => d.GrantFrameId == g.FrameId && d.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && d.Target == g.Target) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangBlackUseEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == g.Target) && _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiConsumedEvent>().Count(e => e.GrantFrameId == g.FrameId) == (begun ? 1 : 0));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 45600)]
    internal bool AdmitCaiYangXunji(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => HasAnniversaryCaiYang && c.SkillId == AnniversaryCaiYangComposition.Xunji && (c.BindingId is AnniversaryCaiYangComposition.Ending or AnniversaryCaiYangComposition.Retaliate) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && (CaiYangEndingWitness(c, w)is { } g && _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(g.Target, c.OwnerSeat) || CaiYangDuelDamage(c.OwnerSeat, w)is not null);
    internal int CaiYangChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => a.IsChainPropagation ? a is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex : 0;
    internal bool CaiYangFirstDamage(int owner) => !_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => e.SourceSeat == owner && !e.SourceLess && e.Amount > 0);
    internal bool TryBeginAnniversaryCaiYangJiaofengSourceWindow(global::CardGame.Core.GameEngine.IDamageAttempt a)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryCaiYangJiaofeng) || a.IsSourceLess || a.DamageAmountFinalized || a.DamageWasApplied || a.DamageAmount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(a.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[a.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[a.TargetSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[a.SourceSeat], AnniversaryCaiYangComposition.Jiaofeng) || !CaiYangFirstDamage(a.SourceSeat))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[a.SourceSeat];
        var lost = _coreHealthQueriesOperations.UnambiguousHp(owner)is { } hp ? Math.Max(0, owner.MaxHp - hp) : 0;
        if (lost == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangJiaofengWindowIssuedEvent>().Any(e => e.Origin.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Origin.AttackId == a.ResolutionId && e.Origin.Target == a.TargetSeat && e.Origin.ChainCursor == CaiYangChainCursor(a) && e.Origin.Chain == a.IsChainPropagation))
            return false;
        var c = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.BeforeDamageApplied).FirstOrDefault(c => c.SkillId == AnniversaryCaiYangComposition.Jiaofeng && c.BindingId == AnniversaryCaiYangComposition.First);
        if (c is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.Last().Id;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var r = new CaiYangJiaofengOrigin(id, parent, a.ResolutionId, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, a.TargetSeat, CaiYangChainCursor(a), a.IsChainPropagation, a.DamageAmount, lost);
        var window = new BeforeDamageProgramWindowFrame(id, parent, a.SourceSeat, a.TargetSeat, a.DamageAmount, _coreNativeCardClassificationsOperations.GetDamageNature(a), BeforeDamageProgramContinuation.Attack, [new(c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner))])
        {
            CaiYangJiaofeng = r,
            ContinuationAttackResolutionId = a.ResolutionId
        };
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangJiaofengWindowIssuedEvent(r));
        _coreRuntimeOperations.AdvanceRuntimeTop<BeforeDamageProgramWindowFrame>();
        return true;
    }

    internal bool ValidCaiYangFirstOrigin(CaiYangJiaofengOrigin r) => r.Source.SkillId == AnniversaryCaiYangComposition.Jiaofeng && r.Source.BindingId == AnniversaryCaiYangComposition.First && r.LostHp > 0 && r.OriginalAmount > 0 && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == r.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangJiaofengWindowIssuedEvent>().Count(e => e.Origin == r) == 1;
    internal CaiYangJiaofengOrigin? CaiYangFirstWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.BeforeDamageApplied && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(x => x.Id == w.ParentFrameId)is { CaiYangJiaofeng: { } r } p && ValidCaiYangFirstOrigin(r) && c.OwnerSeat == r.Source.OwnerSeat && c.SkillId == r.Source.SkillId && c.BindingId == r.Source.BindingId && c.SkillInstanceId == r.Source.SkillInstanceId && c.GameplayHash == r.Hash && w.SourceSeat == c.OwnerSeat && w.TargetSeat == r.Target && w.Amount == r.OriginalAmount && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && p.Candidates[p.CandidateIndex].Candidate == c && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmountFinalized: false } a && a.ResolutionId == r.AttackId && a.SourceSeat == c.OwnerSeat && a.TargetSeat == r.Target && CaiYangChainCursor(a) == r.ChainCursor ? r : null;
    internal bool ResumeAnniversaryCaiYangJiaofengSourceWindow(BeforeDamageProgramWindowFrame p, global::CardGame.Core.GameEngine.IDamageAttempt a)
    {
        if (p.CaiYangJiaofeng is not { } r)
            return false;
        if (!ValidCaiYangFirstOrigin(r) || p.Id != r.WindowId || p.ParentFrameId != r.ParentId || p.CandidateIndex != p.Candidates.Count || p.Prevented || p.RedirectedTargetSeat is not null || a.ResolutionId != r.AttackId || a.IsSourceLess || a.SourceSeat != r.Source.OwnerSeat || a.TargetSeat != r.Target || CaiYangChainCursor(a) != r.ChainCursor || a.IsChainPropagation != r.Chain || a.DamageAmountFinalized || a.DamageAmount != r.OriginalAmount || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != r.ParentId || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangJiaofengWindowReturnedEvent>().Any(e => e.WindowId == p.Id))
            throw new InvalidOperationException("Jiaofeng lost its original unfinalized damage return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangJiaofengWindowReturnedEvent(p.Id, r.AttackId, r.Target, r.ChainCursor));
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(a))
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(a);
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 45600)]
    internal int CaiYangJiaofengDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt a) => a.IsSourceLess ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangJiaofengWindowIssuedEvent>().Count(e => e.Origin.LostHp > 1 && e.Origin.AttackId == a.ResolutionId && e.Origin.Source.OwnerSeat == a.SourceSeat && e.Origin.Target == a.TargetSeat && e.Origin.ChainCursor == CaiYangChainCursor(a) && e.Origin.Chain == a.IsChainPropagation && ValidCaiYangFirstOrigin(e.Origin));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45600, SkillProgramEffectOp.ResolveAnniversaryCaiYangXunji, SkillProgramEffectOp.ResolveAnniversaryCaiYangJiaofeng)]
    internal bool CanOfferCaiYang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id switch
    {
        AnniversaryCaiYangComposition.First => CaiYangFirstWitness(c, w)is not null,
        AnniversaryCaiYangComposition.Ending => CaiYangEndingWitness(c, w)is { } g && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(g.Target, c.OwnerSeat),
        AnniversaryCaiYangComposition.Retaliate => CaiYangDuelDamage(c.OwnerSeat, w)is not null,
        _ => false
    };
}
