namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class BoundaryYuanShuRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramContentQueries content, IProgramTurnQueries turn, ISelectionParticipantQueries participants,
    IMatchRoleQueries roles, IProgramEventHistoryQueries history, IProgramSkillGrantQueries skills,
    ICardZoneQueries cards, IOwnedCardMoveQueries moves, IProgramCardSetBindings bindings,
    IProgramSkillAcquisitions acquisitions, IProgramTurnModifiers turnModifiers) : IBoundaryYuanShuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation switch
    {
        SkillProgramEffectOp.PrepareBoundaryYuanShuJixi => PrepareJixi(frame),
        SkillProgramEffectOp.GrantBoundaryYuanShuLordSkills => GrantLordSkills(frame),
        SkillProgramEffectOp.GrantBoundaryYuanShuWangzunPenalty => GrantWangzunPenalty(frame),
        _ => BindYongsiDiscard(frame)
    };

    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 55200)]
    private bool TracksHp(int seat) => content.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryYuanShuJixi);

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureHealthChange, 55200)]
    private void ObserveHp(HpChangeContext change)
    {
        if (!TracksHp(change.TargetSeat) || change.HpAfter >= change.HpBefore || turn.TurnNumber <= 0 ||
            change.TargetSeat != turn.TurnOwnerSeat || turn.TurnOwnerTurn != turn.TurnNumber ||
            history.Read<BoundaryYuanShuOwnHpDecreasedEvent>().Any(item => item.HpChangeId == change.Id)) return;
        events.AdvanceFact(new BoundaryYuanShuOwnHpDecreasedEvent(change.TargetSeat, turn.TurnNumber, change.Id, change.HpBefore, change.HpAfter));
    }

    private int[] LastOwnTurns(int owner) => history.Read<TurnStartedEvent>()
        .Where(item => item.ActorSeat == owner).TakeLast(3).Select(item => item.TurnNumber).ToArray();

    private bool ThreeCleanTurns(int owner)
    {
        var turns = LastOwnTurns(owner);
        return owner == turn.TurnOwnerSeat && turns.Length == 3 && turns[2] == turn.TurnNumber &&
            turns.Take(2).All(turnNumber => history.Read<TurnEndedEvent>().Any(item => item.ActorSeat == owner && item.TurnNumber == turnNumber)) &&
            !history.Read<BoundaryYuanShuOwnHpDecreasedEvent>().Any(item => item.OwnerSeat == owner && turns.Contains(item.TurnNumber));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 55200)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 55200)]
    private bool Qualify(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == BoundaryYuanShuComposition.Wangzun)
            return participants.IsValidSeat(turn.CurrentSeat) && participants.IsAlive(turn.CurrentSeat) && roles.Role(turn.CurrentSeat) == Role.Lord;
        if (gate.SkillId != BoundaryYuanShuComposition.Jixi) return true;
        return gate.WindowContext is { } context && frames.OfType<ProgramSkillFrame>().Any(frame => frame.OwnerSeat == gate.OwnerSeat &&
            frame.SkillId == gate.SkillId && frame.SkillInstanceId == gate.Candidate.SkillInstanceId && frame.TriggerId == gate.Candidate.BindingId &&
            frame.WindowContext == context && ValidJixi(frame)) || ThreeCleanTurns(gate.OwnerSeat);
    }

    private bool ValidJixi(ProgramSkillFrame frame) => frame.SkillId == BoundaryYuanShuComposition.Jixi && frame.BoundaryYuanShuJixi is { } receipt &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } window && receipt.WindowId == window.ParentFrameId &&
        receipt.FirstTurn > 0 && receipt.FirstTurn < receipt.SecondTurn && receipt.SecondTurn < receipt.ThirdTurn &&
        history.Read<BoundaryYuanShuJixiPreparedEvent>().Count(item => item.FrameId == frame.Id && item.OwnerSeat == frame.OwnerSeat &&
            item.SkillInstanceId == frame.SkillInstanceId && item.GameplayHash == frame.GameplayHash && item.Receipt == receipt) == 1;

    private SkillProgramStepOutcome PrepareJixi(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryYuanShuComposition.Jixi || frame.InstructionIndex != 1 || frame.BoundaryYuanShuJixi is not null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } window || !ThreeCleanTurns(frame.OwnerSeat))
            throw new InvalidOperationException("Jixi requires three completed clean own-turn windows on its original turn-ending frame.");
        var turns = LastOwnTurns(frame.OwnerSeat);
        var receipt = new BoundaryYuanShuJixiReceipt(window.ParentFrameId, turns[0], turns[1], turns[2]);
        frames.Replace(frame.Id, frame with { BoundaryYuanShuJixi = receipt });
        events.AdvanceFact(new BoundaryYuanShuJixiPreparedEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash, receipt));
        return SkillProgramStepOutcome.Continue;
    }

    private SkillProgramStepOutcome BindYongsiDiscard(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryYuanShuComposition.Yongsi || frame.InstructionIndex != 1 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPhaseStarting })
            throw new InvalidOperationException("Yongsi must capture its actual discard availability before its mandatory cost choice.");
        var owner = frame.OwnerSeat;
        var witness = cards.CardsAt(CardLocation.Hand(owner))
            .Where(card => !moves.IsSelfHandCategoryDiscardForbidden(owner, card, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard))
            .Concat(cards.CardsAt(CardLocation.Equipment(owner))
                .Where(card => !moves.IsForeignEquipmentDiscardPrevented(owner, card, CardLocation.Equipment(owner), OwnedCardMoveIntent.Discard)))
            .Take(1).Select(card => card.Id).ToArray();
        bindings.SetProgramCardSet(frame.Id, "yongsi-discardable", witness, SkillProgramCardSetVisibility.Private);
        return SkillProgramStepOutcome.Continue;
    }

    private SkillProgramStepOutcome GrantLordSkills(ProgramSkillFrame frame)
    {
        if (!ValidJixi(frame) || frame.InstructionIndex != 7 ||
            frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == "jixi-gain")?.OptionId != "lord")
            throw new InvalidOperationException("Jixi's Lord grant requires its original paid awakening and chosen draw branch.");
        var lord = participants.Seats.Where(seat => participants.IsAlive(seat) && roles.Role(seat) == Role.Lord)
            .Select(seat => (int?)seat).SingleOrDefault();
        if (lord is null) { acquisitions.Grant(frame, frame.OwnerSeat, []); return SkillProgramStepOutcome.Continue; }
        var sourceId = $"acquired:boundary:jixi-mobile:{frame.Id}:lord";
        var sourceGrants = skills.ActiveGrants(lord.Value).Where(grant => content.SkillTags(grant.SkillId).HasFlag(SkillTag.Lord))
            .DistinctBy(grant => grant.SkillId).ToArray();
        // Acquisition and awakening remain shared mutations; this issued source
        // authorizes only the original Lord-skill copy.
        acquisitions.Grant(frame, frame.OwnerSeat, Array.AsReadOnly(sourceGrants.Select(grant => grant.SkillId).ToArray()));
        foreach (var source in sourceGrants)
        {
            var grant = acquisitions.GrantOwnedSource(frames.ActiveProgram(frame.Id), sourceId, source.SkillId);
            events.AdvanceFact(new BoundaryYuanShuLordSkillGrantedEvent(frame.Id, frame.OwnerSeat, lord.Value,
                grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.SourceId, source.GrantId, source.SkillInstanceId));
        }
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsLordGrantQualified(int owner, string skillId, string? instance) => instance is not null &&
        skills.Grants(owner).Any(grant => grant.IsEnabled && grant.SkillId == skillId && grant.SkillInstanceId == instance &&
            history.Read<BoundaryYuanShuLordSkillGrantedEvent>().Any(item => item.OwnerSeat == owner && item.GrantId == grant.GrantId &&
                item.SkillId == grant.SkillId && item.SkillInstanceId == grant.SkillInstanceId && item.SourceId == grant.SourceId));

    private SkillProgramStepOutcome GrantWangzunPenalty(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryYuanShuComposition.Wangzun || frame.InstructionIndex != 2 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } ||
            roles.Role(turn.CurrentSeat) != Role.Lord || turn.CurrentSeat != turn.TurnOwnerSeat)
            throw new InvalidOperationException("Wangzun must penalize its original preparing Lord's actual turn.");
        turnModifiers.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, turn.CurrentSeat);
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 55200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 55200)]
    private bool KeepPrepared(ProgramSkillFrame frame) => ValidJixi(frame);

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55200)]
    private void AssertJixi(ProgramSkillFrame frame)
    {
        if (frame.BoundaryYuanShuJixi is not null && !ValidJixi(frame))
            throw new InvalidOperationException("Jixi lost its original issued three-turn qualification.");
    }
}
