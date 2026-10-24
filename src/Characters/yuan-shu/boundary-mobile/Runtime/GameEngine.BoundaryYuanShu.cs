namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryYuanShuHost(GameEngine engine) : IBoundaryYuanShuHost
    {
        public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
        {
            SkillProgramEffectOp.PrepareBoundaryYuanShuJixi => engine.PrepareBoundaryYuanShuJixi(f),
            SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills => engine.GrantBoundaryYuanShuLordSkills(f),
            SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty => engine.GrantBoundaryYuanShuWangzunPenalty(f),
            _ => engine.BindBoundaryYuanShuYongsiDiscard(f)
        };
    }
    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 55200)]
    private bool TracksBoundaryYuanShuHp(int seat) => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryYuanShuJixi);
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureHealthChange, 55200)]
    private void ObserveBoundaryYuanShuHp(HpChangeContext change)
    {
        if (!TracksBoundaryYuanShuHp(change.TargetSeat) || change.HpAfter >= change.HpBefore || _turnNumber <= 0 ||
            change.TargetSeat != _turnProgression.OwnerSeat || _turnProgression.TurnNumber != _turnNumber ||
            ProgramEventHistory<BoundaryYuanShuOwnHpDecreasedEvent>().Any(e => e.HpChangeId == change.Id)) return;
        AdvanceEventRulesAndQueueFact(new BoundaryYuanShuOwnHpDecreasedEvent(change.TargetSeat, _turnNumber, change.Id, change.HpBefore, change.HpAfter));
    }
    private int[] BoundaryYuanShuLastOwnTurns(int owner) => ProgramEventHistory<TurnStartedEvent>()
        .Where(e => e.ActorSeat == owner).TakeLast(3).Select(e => e.TurnNumber).ToArray();
    private bool BoundaryYuanShuThreeCleanTurns(int owner)
    {
        var turns = BoundaryYuanShuLastOwnTurns(owner);
        return owner == _turnProgression.OwnerSeat && turns.Length == 3 && turns[2] == _turnNumber &&
            turns.Take(2).All(t => ProgramEventHistory<TurnEndedEvent>().Any(e => e.ActorSeat == owner && e.TurnNumber == t)) &&
            !ProgramEventHistory<BoundaryYuanShuOwnHpDecreasedEvent>().Any(e => e.OwnerSeat == owner && turns.Contains(e.TurnNumber));
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 55200)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 55200)]
    private bool QualifyBoundaryYuanShu(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == BoundaryYuanShuComposition.Wangzun) return IsValidPlayerSeat(_currentSeat) && _players[_currentSeat].IsAlive && _players[_currentSeat].Role == Role.Lord;
        if (gate.SkillId != BoundaryYuanShuComposition.Jixi) return true;
        return gate.WindowContext is { } context && _resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OwnerSeat == gate.OwnerSeat && f.SkillId == gate.SkillId &&
            f.SkillInstanceId == gate.Candidate.SkillInstanceId && f.TriggerId == gate.Candidate.BindingId && f.WindowContext == context && ValidBoundaryYuanShuJixi(f)) || BoundaryYuanShuThreeCleanTurns(gate.OwnerSeat);
    }
    private bool ValidBoundaryYuanShuJixi(ProgramSkillFrame f) => f.SkillId == BoundaryYuanShuComposition.Jixi && f.BoundaryYuanShuJixi is { } r &&
        f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } w && r.WindowId == w.ParentFrameId && r.FirstTurn > 0 && r.FirstTurn < r.SecondTurn && r.SecondTurn < r.ThirdTurn &&
        ProgramEventHistory<BoundaryYuanShuJixiPreparedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash && e.Receipt == r) == 1;
    private SkillProgramStepOutcome PrepareBoundaryYuanShuJixi(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryYuanShuComposition.Jixi || f.InstructionIndex != 1 || f.BoundaryYuanShuJixi is not null ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w || !BoundaryYuanShuThreeCleanTurns(f.OwnerSeat))
            throw new InvalidOperationException("Jixi requires three completed clean own-turn windows on its original turn-ending frame.");
        var turns = BoundaryYuanShuLastOwnTurns(f.OwnerSeat); var receipt = new BoundaryYuanShuJixiReceipt(w.ParentFrameId, turns[0], turns[1], turns[2]);
        ReplaceRuntimeFrame(f.Id, f with { BoundaryYuanShuJixi = receipt });
        AdvanceEventRulesAndQueueFact(new BoundaryYuanShuJixiPreparedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, receipt));
        return SkillProgramStepOutcome.Continue;
    }
    private SkillProgramStepOutcome BindBoundaryYuanShuYongsiDiscard(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryYuanShuComposition.Yongsi || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPhaseStarting })
            throw new InvalidOperationException("Yongsi must capture its actual discard availability before its mandatory cost choice.");
        var owner = f.OwnerSeat;
        var witness = HasDiscardableHeBy(owner, owner) ? GetHand(_players[owner]).Where(c => !IsSelfHandCategoryDiscardForbidden(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard))
            .Concat(GetEquipment(owner).Where(c => !IsForeignEquipmentDiscardPrevented(owner, c, CardLocation.Equipment(owner), OwnedCardMoveIntent.Discard))).Take(1).Select(c => c.Id).ToArray() : [];
        SetProgramCardSet(f.Id, "yongsi-discardable", witness, SkillProgramCardSetVisibility.Private);
        return SkillProgramStepOutcome.Continue;
    }
    private SkillProgramStepOutcome GrantBoundaryYuanShuLordSkills(ProgramSkillFrame f)
    {
        if (!ValidBoundaryYuanShuJixi(f) || f.InstructionIndex != 7 || f.ChoiceBindings.SingleOrDefault(b => b.Name == "jixi-gain")?.OptionId != "lord")
            throw new InvalidOperationException("Jixi's Lord grant requires its original paid awakening and chosen draw branch.");
        var lord = _players.SingleOrDefault(p => p.IsAlive && p.Role == Role.Lord);
        if (lord is null) { GrantProgramSkills(f, []); return SkillProgramStepOutcome.Continue; }
        var sourceId = $"acquired:boundary:jixi-mobile:{f.Id}:lord";
        var sourceGrants = GetSkillBindingShard(lord).ActiveGrants.Where(g => _contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord)).DistinctBy(g => g.SkillId).ToArray();
        // The ordinary acquisition/awakening event remains the shared mutation's
        // responsibility. The additional issued source authorizes only this copy.
        GrantProgramSkills(f, Array.AsReadOnly(sourceGrants.Select(g => g.SkillId).ToArray()));
        foreach (var source in sourceGrants)
        {
            var grant = GrantOwnedProgramSkillSource(GetActiveProgramFrame(f.Id), sourceId, source.SkillId);
            AdvanceEventRulesAndQueueFact(new BoundaryYuanShuLordSkillGrantedEvent(f.Id, f.OwnerSeat, lord.Seat, grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.SourceId, source.GrantId, source.SkillInstanceId));
        }
        return SkillProgramStepOutcome.Continue;
    }
    private bool IsBoundaryYuanShuLordGrantQualified(CharacterState owner, string skillId, string? instance) =>
        instance is not null && owner.SkillGrants.Grants.Any(g => g.IsEnabled && g.SkillId == skillId && g.SkillInstanceId == instance &&
            ProgramEventHistory<BoundaryYuanShuLordSkillGrantedEvent>().Any(e => e.OwnerSeat == owner.Seat && e.GrantId == g.GrantId &&
                e.SkillId == g.SkillId && e.SkillInstanceId == g.SkillInstanceId && e.SourceId == g.SourceId));
    private SkillProgramStepOutcome GrantBoundaryYuanShuWangzunPenalty(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryYuanShuComposition.Wangzun || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } ||
            _players[_currentSeat].Role != Role.Lord || _currentSeat != _turnProgression.OwnerSeat) throw new InvalidOperationException("Wangzun must penalize its original preparing Lord's actual turn.");
        ValidateProgramTurnEffectGrant(f);
        var grant = _turnCardUseEffects.GrantRuleModifier(_turnNumber, _turnProgression.OwnerSeat, f.Id, f.InstructionIndex - 1,
            CreateProgramTurnEffectSource(f), SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, affectedSeat: _currentSeat);
        AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
        return SkillProgramStepOutcome.Continue;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 55200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 55200)]
    private bool KeepPreparedBoundaryYuanShu(ProgramSkillFrame f) => ValidBoundaryYuanShuJixi(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55200)]
    private void AssertBoundaryYuanShuJixi(ProgramSkillFrame f)
    { if (f.BoundaryYuanShuJixi is not null && !ValidBoundaryYuanShuJixi(f)) throw new InvalidOperationException("Jixi lost its original issued three-turn qualification."); }
}
