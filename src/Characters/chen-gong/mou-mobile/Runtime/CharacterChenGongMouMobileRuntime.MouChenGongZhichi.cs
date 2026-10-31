#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    internal int MouChenGongActualTurnOrdinal => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count();

    internal MouChenGongZhichiProtection? CurrentMouChenGongProtection(int owner)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().LastOrDefault(e => e is TurnStartedEvent or TurnEndedEvent)is not TurnStartedEvent turn || turn.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || turn.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return null;
        var ordinal = MouChenGongActualTurnOrdinal;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongZhichiIssuedEvent>().Select(e => e.Protection).SingleOrDefault(p => p.Source.OwnerSeat == owner && p.ActualTurnNumber == turn.TurnNumber && p.ActualTurnOwnerSeat == turn.ActorSeat && p.ActualTurnOrdinal == ordinal);
    }

    internal bool ValidMouChenGongProtection(MouChenGongZhichiProtection p) => p.Source.SkillId == MouChenGongComposition.Zhichi && p.Source.BindingId == "arm-protection" && p.DamageTargetSeat == p.Source.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(p.Source.OwnerSeat) && p.WindowId > 0 && p.DamageFrameId > 0 && p.AttackResolutionId > 0 && p.ActualTurnOrdinal > 0 && OriginalMouChenGongProtectionTurn(p) && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program is { } program && program.GameplayHash == p.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongZhichiIssuedEvent>().Count(e => e.Protection == p) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageTriggerWindowOpenedEvent>().Count(e => e.ResolutionId == p.WindowId && e.DamageFrameId == p.DamageFrameId && e.TargetSeat == p.Source.OwnerSeat && e.Candidates.Any(c => c.ToProgramCandidate()is { } candidate && candidate.OwnerSeat == p.Source.OwnerSeat && candidate.SkillId == p.Source.SkillId && candidate.SkillInstanceId == p.Source.SkillInstanceId && candidate.BindingId == p.Source.BindingId && candidate.GameplayHash == p.GameplayHash)) == 1;
    internal bool OriginalMouChenGongProtectionTurn(MouChenGongZhichiProtection p)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var index = Array.FindIndex(history, e => e is DamageTriggerWindowOpenedEvent window && window.ResolutionId == p.WindowId);
        if (index < 0)
            return false;
        var prefix = history.Take(index).ToArray();
        return prefix.LastOrDefault(e => e is TurnStartedEvent or TurnEndedEvent)is TurnStartedEvent turn && turn.TurnNumber == p.ActualTurnNumber && turn.ActorSeat == p.ActualTurnOwnerSeat && prefix.OfType<TurnStartedEvent>().Count() == p.ActualTurnOrdinal && prefix.OfType<DamageRequestedEvent>().Count(e => e.ResolutionId == p.DamageFrameId && e.TargetSeat == p.DamageTargetSeat && e.Amount == p.DamageAmount && e.Nature == p.Nature && e.SourceLess == p.SourceLess) == 1;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 29601)]
    internal void ObserveMouChenGongZhichi(IGameEvent fact)
    {
        if (fact is DamageTriggerWindowOpenedEvent opened && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { TriggerWindow: SkillProgramTriggerWindow.AfterDamageApplied } window && window.Id == opened.ResolutionId && window.ParentFrameId == opened.DamageFrameId && _coreMatchParticipantsPlayersAccess._players[window.TargetSeat].IsAlive && CurrentMouChenGongProtection(window.TargetSeat)is null)
        {
            var candidates = window.Candidates.Select(c => c.ToProgramCandidate()).Where(c => c is not null && c.OwnerSeat == window.TargetSeat && c.SkillId == MouChenGongComposition.Zhichi && c.BindingId == "arm-protection").Cast<ProgramTriggerCandidate>().ToArray();
            if (candidates.Length == 0)
                return;
            var candidate = candidates.OrderBy(c => c.SkillInstanceId, StringComparer.Ordinal).First();
            var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
            if (_contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } program || program.GameplayHash != candidate.GameplayHash || attack.TargetSeat != window.TargetSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is not DamageFrame damage || damage.TargetSeat != window.TargetSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageRequestedEvent>().Count(e => e.ResolutionId == damage.Id && e.SourceSeat == damage.SourceSeat && e.TargetSeat == damage.TargetSeat && e.Amount == damage.Amount && e.Nature == damage.Nature) != 1)
                throw new InvalidOperationException("Zhichi can arm only from its actual native after-damage window and frozen original skill candidate.");
            var protection = new MouChenGongZhichiProtection(new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, window.Id, window.ParentFrameId, attack.ResolutionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, MouChenGongActualTurnOrdinal, window.TargetSeat, damage.Amount, damage.Nature, attack.IsSourceLess);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongZhichiIssuedEvent(protection));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[window.TargetSeat].Name)} 的【智迟】生效，本回合后续受到的伤害均被防止。", window.TargetSeat);
            return;
        }

        if (fact is not ProgramDamagePreventedEvent prevented || prevented.SkillId != MouChenGongComposition.Zhichi || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WindowContext.MouChenGongProtection: { } protectionReceipt } f)
            return;
        int? nativeSource = prevented.SourceSeat;
        if (!ExactMouChenGongBeforeFrame(f, allowPrevented: true) || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == f.WindowContext.ParentFrameId)is not BeforeDamageProgramWindowFrame parent || prevented.FrameId != parent.Id || prevented.BindingId != "prevent-later-damage" || prevented.OwnerSeat != f.OwnerSeat || prevented.TargetSeat != f.OwnerSeat || _coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(parent) != (f.WindowContext.SourceSeat is null) || (_coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(parent) ? nativeSource is not null && nativeSource != parent.SourceSeat : nativeSource != parent.SourceSeat) || prevented.Amount != f.WindowContext.Amount)
            throw new InvalidOperationException("Zhichi prevention must return through its exact native before-damage parent and already issued protection.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongZhichiPreventedEvent(prevented.FrameId, protectionReceipt, f.WindowContext.SourceSeat, prevented.TargetSeat, prevented.Amount));
    }

    internal bool IsMouChenGongZhichiBeforeCandidate(ProgramTriggerCandidate candidate) => candidate.SkillId == MouChenGongComposition.Zhichi && candidate.BindingId == "prevent-later-damage";
    internal IEnumerable<BeforeDamageProgramCandidate> CollectIssuedMouChenGongZhichiCandidates(int source, int target, bool rangeChainOnly)
    {
        if (CurrentMouChenGongProtection(target)is not { } protection || !ValidMouChenGongProtection(protection))
            yield break;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(protection.Source.SkillId).Program!.Triggers.Single(t => t.Id == "prevent-later-damage");
        yield return new(new(target, protection.Source.SkillId, trigger.Id, protection.Source.SkillInstanceId, protection.GameplayHash, trigger.Priority), _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[target])with { EventTargetHp = _coreMatchParticipantsPlayersAccess._players[target].Hp });
    }

    internal ProgramSkillWindowContext AttachMouChenGongProtection(BeforeDamageProgramCandidate item, ProgramSkillWindowContext context) => IsMouChenGongZhichiBeforeCandidate(item.Candidate) && CurrentMouChenGongProtection(item.Candidate.OwnerSeat)is { } p && p.Source.SkillInstanceId == item.Candidate.SkillInstanceId && p.GameplayHash == item.Candidate.GameplayHash ? context with
    {
        MouChenGongProtection = p
    }

    : context;
    [ProgramIssuedTrigger(100)]
    internal bool IsIssuedMouChenGongZhichiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MouChenGongComposition.Zhichi || context.OwnerSeat != candidate.OwnerSeat || CurrentMouChenGongProtection(candidate.OwnerSeat)is not { } p || !ValidMouChenGongProtection(p) || p.Source.SkillInstanceId != candidate.SkillInstanceId || p.GameplayHash != candidate.GameplayHash)
            return false;
        if (candidate.BindingId == "arm-protection")
            return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.ParentFrameId == p.WindowId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.WindowId)is DamageTriggerWindowFrame w && w.TriggerWindow == context.Window && w.ParentFrameId == p.DamageFrameId && w.TargetSeat == candidate.OwnerSeat && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex].ToProgramCandidate() == candidate && (_coreProgramLifecycleOperations.CreateAfterDamageProgramContext(w, candidate)with
            {
                Facts = null
            }

            ) == (context with
            {
                Facts = null
            }

            );
        if (candidate.BindingId != "prevent-later-damage" || context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || context.MouChenGongProtection != p || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not BeforeDamageProgramWindowFrame before || before.Prevented || before.RedirectedTargetSeat is not null || before.TargetSeat != candidate.OwnerSeat || before.Amount <= 0 || before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count)
            return false;
        var current = before.Candidates[before.CandidateIndex];
        return current.Candidate == candidate && _coreDamagePreventionProgramsOperations.CreateBeforeDamageProgramContext(before, current) == context && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && attack.TargetSeat == before.TargetSeat && attack.SourceSeat == before.SourceSeat;
    }

    internal bool ExactMouChenGongBeforeFrame(ProgramSkillFrame f, bool allowPrevented)
    {
        if (f.TriggerId != "prevent-later-damage" || f.ActivationId != f.TriggerId || f.SkillId != MouChenGongComposition.Zhichi || f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, MouChenGongProtection: { } p } context || CurrentMouChenGongProtection(f.OwnerSeat) != p || !ValidMouChenGongProtection(p) || p.Source.OwnerSeat != f.OwnerSeat || p.Source.SkillInstanceId != f.SkillInstanceId || p.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not BeforeDamageProgramWindowFrame w || w.TargetSeat != f.OwnerSeat || w.Amount <= 0 || w.RedirectedTargetSeat is not null || w.Prevented && !allowPrevented || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].Candidate) || _coreDamagePreventionProgramsOperations.CreateBeforeDamageProgramContext(w, w.Candidates[w.CandidateIndex]) != context)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == w.Id;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29602)]
    internal bool GateMouChenGongZhichi(ProgramTriggerLegalityContext gate) => gate.SkillId != MouChenGongComposition.Zhichi || gate.Candidate.BindingId != "prevent-later-damage" || CurrentMouChenGongProtection(gate.OwnerSeat)is not null;
    internal bool PermitMouChenGongZhichiAfterLoss(ProgramSkillFrame f)
    {
        if (f.SkillId != MouChenGongComposition.Zhichi || f.WindowContext is not { } context)
            return false;
        if (f.TriggerId == "prevent-later-damage")
            return ExactMouChenGongBeforeFrame(f, allowPrevented: true);
        return f.TriggerId == "arm-protection" && f.InstructionIndex is 0 or 1 && IsIssuedMouChenGongZhichiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context);
    }
}
