#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMangYaChangAnniversaryRuntime
{
    internal bool HasAnniversaryJiedao => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseAnniversaryMangYaChangJiedao);

    internal int JiedaoChainCursor(GameEngine.IDamageAttempt attack) => attack.IsChainPropagation ? attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex : 0;
    internal bool FirstActualJiedaoSourceDamage(int seat) => !_corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => e.Amount > 0 && !e.SourceLess && e.SourceSeat == seat);
    internal bool TryBeginAnniversaryJiedaoSourceWindow(GameEngine.IDamageAttempt attack)
    {
        if (!HasAnniversaryJiedao || attack.IsSourceLess || attack.DamageAmountFinalized || attack.DamageWasApplied || attack.DamageAmount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], AnniversaryJiedaoComposition.Skill) || !FirstActualJiedaoSourceDamage(attack.SourceSeat))
            return false;
        var cursor = JiedaoChainCursor(attack);
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoWindowIssuedEvent>().Any(e => e.Receipt.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && e.Receipt.AttackId == attack.ResolutionId && e.Receipt.Target == attack.TargetSeat && e.Receipt.ChainCursor == cursor && e.Receipt.ChainPropagation == attack.IsChainPropagation))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        if (!owner.HasHpValue || owner.MaxHp - owner.Hp <= 0)
            return false;
        var candidate = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.BeforeDamageApplied).FirstOrDefault(c => c.SkillId == AnniversaryJiedaoComposition.Skill && c.BindingId == AnniversaryJiedaoComposition.Choose);
        if (candidate is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? throw new InvalidOperationException("Jiedao requires a real native damage owner.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var receipt = new AnniversaryJiedaoSourceReceipt(id, parent, attack.ResolutionId, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, attack.TargetSeat, cursor, attack.IsChainPropagation, attack.DamageAmount, owner.MaxHp - owner.Hp);
        var window = new BeforeDamageProgramWindowFrame(id, parent, attack.SourceSeat, attack.TargetSeat, attack.DamageAmount, _coreNativeCardClassificationsOperations.GetDamageNature(attack), BeforeDamageProgramContinuation.Attack, Array.AsReadOnly(new[] { new BeforeDamageProgramCandidate(candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner)) }))
        {
            AnniversaryJiedaoSource = receipt,
            ContinuationAttackResolutionId = attack.ResolutionId
        };
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJiedaoWindowIssuedEvent(receipt));
        _coreRuntimeOperations.AdvanceRuntimeTop<BeforeDamageProgramWindowFrame>();
        return true;
    }

    internal bool ResumeAnniversaryJiedaoSourceWindow(BeforeDamageProgramWindowFrame window, GameEngine.IDamageAttempt attack)
    {
        if (window.AnniversaryJiedaoSource is not { } r)
            return false;
        if (!ValidJiedaoOrigin(r) || window.Id != r.WindowId || window.ParentFrameId != r.ParentId || window.CandidateIndex != window.Candidates.Count || window.Prevented || window.RedirectedTargetSeat is not null || attack.ResolutionId != r.AttackId || attack.IsSourceLess || attack.SourceSeat != r.Source.OwnerSeat || attack.TargetSeat != r.Target || JiedaoChainCursor(attack) != r.ChainCursor || attack.IsChainPropagation != r.ChainPropagation || attack.DamageAmountFinalized || attack.DamageAmount != r.OriginalAmount || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != r.ParentId || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoWindowReturnedEvent>().Any(e => e.WindowId == r.WindowId))
            throw new InvalidOperationException("Jiedao source choice lost its exact unfinalized native damage return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJiedaoWindowReturnedEvent(r.WindowId, r.AttackId, r.Target, r.ChainCursor));
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
        return true;
    }

    internal bool ValidJiedaoOrigin(AnniversaryJiedaoSourceReceipt r) => r.Source.SkillId == AnniversaryJiedaoComposition.Skill && r.Source.BindingId == AnniversaryJiedaoComposition.Choose && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == r.Hash && r.MaximumBonus > 0 && r.OriginalAmount > 0 && r.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoWindowIssuedEvent>().Count(e => e.Receipt == r) == 1;
    internal bool ValidJiedaoGrant(AnniversaryJiedaoGrant g) => ValidJiedaoOrigin(g.Origin) && g.Bonus >= 0 && g.Bonus <= g.Origin.MaximumBonus && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoChosenEvent>().Count(e => e.Grant == g) == 1;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 44400)]
    internal int JiedaoSourceDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasAnniversaryJiedao || attack.IsSourceLess)
            return 0;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoChosenEvent>().Where(e => e.Grant.Origin.AttackId == attack.ResolutionId && e.Grant.Origin.Target == attack.TargetSeat && e.Grant.Origin.Source.OwnerSeat == attack.SourceSeat && e.Grant.Origin.ChainPropagation == attack.IsChainPropagation && e.Grant.Origin.ChainCursor == JiedaoChainCursor(attack) && e.Grant.Origin.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && ValidJiedaoGrant(e.Grant)).Sum(e => e.Grant.Bonus);
    }

    internal AnniversaryJiedaoSourceReceipt? JiedaoSourceWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => w.Window == SkillProgramTriggerWindow.BeforeDamageApplied && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { AnniversaryJiedaoSource: { } r } parent && ValidJiedaoOrigin(r) && c.SkillId == r.Source.SkillId && c.BindingId == r.Source.BindingId && c.OwnerSeat == r.Source.OwnerSeat && c.SkillInstanceId == r.Source.SkillInstanceId && c.GameplayHash == r.Hash && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == c.OwnerSeat && w.TargetSeat == r.Target && w.Amount == r.OriginalAmount && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex].Candidate == c && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmountFinalized: false } a && a.ResolutionId == r.AttackId && a.TargetSeat == r.Target && a.SourceSeat == c.OwnerSeat && JiedaoChainCursor(a) == r.ChainCursor ? r : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 44400, SkillProgramEffectOp.ChooseAnniversaryMangYaChangJiedao, SkillProgramEffectOp.PayAnniversaryMangYaChangJiedao)]
    internal bool CanOfferAnniversaryJiedao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id == AnniversaryJiedaoComposition.Choose ? JiedaoSourceWitness(c, w)is not null && FirstActualJiedaoSourceDamage(c.OwnerSeat) : _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && w.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && JiedaoCostWitness(c, w)is not null;
}
