namespace CardGame.Core;

// The contest, payment and child cursor remain on the original program frame.
// Native card issuance is a producer below this protocol, never its owner.
[ProgramRuntimeModule]
internal sealed class ForeignTurnContestRuntime(IProgramRuntimeFrames frames, INativeFrameRoutingFacts nativeFrames, IProgramRuntimeEvents events,
    IProgramEventHistoryQueries history, ICardZoneQueries zones, ICardMovementHistoryQueries movements,
    IProgramBoundCardMovementOperations movement, IProgramContentQueries content, IProgramTurnQueries turn,
    ISelectionParticipantQueries participants, IProgramPhaseFrameQueries phaseFrames,
    IProgramPindianPolicyQueries pindianPolicies, IActivePlayerTurnEffectQueries turnEffects,
    IProgramMovementWindows movementWindows, IForeignTurnContestFacts facts,
    IForeignTurnContestContinuations continuations, IForeignTurnContestSlashProducer slashes)
{
    internal const string ClaimReason = "program.foreign-turn-contest.claim";

    private static CardConversionSource Source(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame),
            frame.OwnerSeat, frame.SkillInstanceId);

    private static bool HasIndependentForeignTurnRule(string skillId) =>
        skillId is "mobile-god:powei" or "happy:chongwei" or "anniversary:cuixi" or "anniversary:shiwu";

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 140, SkillProgramTriggerWindow.OtherActualTurnStarted)]
    internal bool CanRunForeignActualTurnContest(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var owner = candidate.OwnerSeat;
        return context.Window == SkillProgramTriggerWindow.OtherActualTurnStarted &&
            participants.IsAlive(owner) && (HasIndependentForeignTurnRule(candidate.SkillId) ||
                (candidate.SkillId == "boundary:zhuikong-mobile"
                    ? facts.HasHpValue(owner) && facts.HasHpValue(turn.CurrentSeat) && participants.Hp(turn.CurrentSeat) >= participants.Hp(owner)
                    : participants.Hp(owner) > 0 && facts.IsRuleWounded(owner))) &&
            context.SourceSeat == turn.CurrentSeat && context.TargetSeat == turn.CurrentSeat && owner != turn.CurrentSeat &&
            participants.IsAlive(turn.CurrentSeat) && (HasIndependentForeignTurnRule(candidate.SkillId) ||
                HasPindianInput(owner) && HasPindianInput(turn.CurrentSeat) &&
                ProgramOperationLegalityPolicy.HandContest.CanSelectTarget(PindianParticipant(owner), PindianParticipant(turn.CurrentSeat))) &&
            LastForeignActualTurnStartWindow() is { } parent &&
            parent.Id == context.ParentFrameId && parent.OwnerSeat == turn.CurrentSeat && parent.ActualTurnNumber == turn.TurnNumber &&
            parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count &&
            parent.Candidates[parent.CandidateIndex] == candidate &&
            history.Read<TurnStartedEvent>().LastOrDefault() is { } started &&
            started.TurnNumber == turn.TurnNumber && started.ActorSeat == turn.CurrentSeat;
    }

    private ForeignActualTurnStartWindowFrame? LastForeignActualTurnStartWindow()
    {
        for (var index = nativeFrames.FrameCount - 1; index >= 0; index--)
            if (nativeFrames.FrameAt(index) is ForeignActualTurnStartWindowFrame parent) return parent;
        return null;
    }

    private bool ContainsCardUse(long frameId)
    {
        for (var index = 0; index < nativeFrames.FrameCount; index++)
            if (nativeFrames.FrameAt(index) is CardUseFrame use && use.Id == frameId) return true;
        return false;
    }

    private bool HasPindianInput(int seat) => zones.Count(CardLocation.Hand(seat)) > 0 || pindianPolicies.CanDiscardForPindian(seat);
    private ProgramLegalityParticipant PindianParticipant(int seat) =>
        new(seat, zones.Count(CardLocation.Hand(seat)), pindianPolicies.ProhibitsPindianTarget(seat), pindianPolicies.CanDiscardForPindian(seat));

    // Called while the revealed native Pindian is still the direct child. Old
    // bindings retain their null origin and exact historical JSON shape.
    internal ForeignTurnPindianOrigin? FreezeOrigin(ProgramSkillFrame program, PindianFrame pindian)
    {
        if (!phaseFrames.IsForeignActualTurnPindian(program)) return null;
        var plan = ProgramInstructionResolver.Default.Resolve(program, content.Skill(program.SkillId).Program!);
        if (!plan.Instructions.Any(effect => effect.Op == SkillProgramEffectOp.ResolveForeignTurnPindian)) return null;
        if (plan.Instructions.Count != 3 || program.InstructionIndex != 2 ||
            plan.Instructions[1].Op != SkillProgramEffectOp.StartPindian ||
            plan.Instructions[1].ResultBind != pindian.ProgramResultBind ||
            plan.Instructions[2].Op != SkillProgramEffectOp.ResolveForeignTurnPindian ||
            plan.Instructions[2].SourceBind != pindian.ProgramResultBind ||
            pindian.ParentFrameId != program.Id || pindian.Result is not { } result ||
            result.SourceSeat != program.OwnerSeat || result.OpponentSeat != turn.CurrentSeat ||
            program.SelectedTargetSeats is not [var selected] || selected != result.OpponentSeat ||
            !history.Read<PindianResultDeterminedEvent>().Any(e =>
                e.FrameId == pindian.Id && e.SkillId == program.SkillId && e.Result == result))
            throw new InvalidOperationException("A foreign turn contest lost its original revealed result producer.");
        var origin = new ForeignTurnPindianOrigin(pindian.Id, program.Id, turn.TurnNumber,
            turn.CurrentSeat, program.GameplayHash, result);
        events.AdvanceFact(new ForeignTurnContestOriginFrozenEvent(origin, Source(program), pindian.ProgramResultBind!));
        return origin;
    }

    internal bool MatchesOrigin(ProgramSkillFrame frame, string bind, ForeignTurnPindianOrigin origin)
    {
        var result = frame.PindianResultBindings.SingleOrDefault(binding => binding.Name == bind);
        return phaseFrames.IsForeignActualTurnPindian(frame) && origin.ProgramFrameId == frame.Id && origin.PindianFrameId > frame.Id &&
            origin.ActualTurnNumber == turn.TurnNumber && origin.ActualTurnOwnerSeat == turn.CurrentSeat &&
            origin.GameplayHash == frame.GameplayHash && result?.ForeignTurnOrigin == origin &&
            result.SourceSeat == origin.Result.SourceSeat && result.OpponentSeat == origin.Result.OpponentSeat &&
            result.SourceRank == origin.Result.SourceRank && result.OpponentRank == origin.Result.OpponentRank &&
            result.SourceWon == origin.Result.SourceWon && origin.Result.SourceSeat == frame.OwnerSeat &&
            origin.Result.OpponentSeat == turn.CurrentSeat && origin.Result.SourceCardId > 0 &&
            origin.Result.OpponentCardId > 0 && origin.Result.SourceCardId != origin.Result.OpponentCardId &&
            history.Read<ForeignTurnContestOriginFrozenEvent>().Count(e =>
                e.Origin == origin && e.Source == Source(frame) && e.ResultBind == bind) == 1 &&
            history.Read<PindianResultDeterminedEvent>().Count(e =>
                e.FrameId == origin.PindianFrameId && e.SkillId == frame.SkillId && e.Result == origin.Result) == 1;
    }

    internal SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, string bind)
    {
        var current = frames.ActiveProgram(frame.Id);
        var binding = current.PindianResultBindings.SingleOrDefault(item => item.Name == bind);
        if (current.ForeignTurnContest is not null || binding?.ForeignTurnOrigin is not { } origin ||
            !MatchesOrigin(current, bind, origin))
            throw new InvalidOperationException("A foreign turn contest cannot replay or replace its original result.");
        if (facts.Winner != Winner.None || !participants.IsAlive(current.OwnerSeat) || !participants.IsAlive(turn.CurrentSeat) ||
            !facts.HasSkillInstance(current.OwnerSeat, current.SkillId, current.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (origin.Result.SourceWon)
        {
            frames.ReplaceTop(current = current with { ForeignTurnContest = new(current.InstructionIndex, bind, origin,
                ForeignTurnContestStage.Complete, RestrictionIssued: true) });
            continuations.GrantSelfOnly(current, origin.Result.OpponentSeat);
            var grant = turnEffects.TargetRestrictions.Single(item => item.ParentFrameId == current.Id && item.EffectIndex == current.InstructionIndex - 1);
            events.AdvanceFact(new ForeignTurnContestRestrictedEvent(current.Id, Source(current), origin, grant.GrantSequence));
            return SkillProgramStepOutcome.Continue;
        }
        frames.ReplaceTop(current = current with { ForeignTurnContest = new(current.InstructionIndex, bind, origin, ForeignTurnContestStage.ClaimChildren),
            PendingMovementContinuation = new(current.OwnerSeat, 0, null) });
        var id = origin.Result.OpponentCardId;
        if (zones.GetLocation(id) == CardLocation.DiscardPile)
        {
            var card = zones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == id);
            var before = movements.Read().Count;
            movement.Move(card, CardLocation.DiscardPile, CardLocation.Hand(current.OwnerSeat), new(ClaimReason));
            current = frames.ActiveProgram(current.Id);
            var moved = movements.Read().Skip(before).Single(item => item.CardId == id &&
                item.From == CardLocation.DiscardPile && item.To == CardLocation.Hand(current.OwnerSeat) && item.Reason.Value == ClaimReason);
            frames.ReplaceTop(current = current with { ForeignTurnContest = current.ForeignTurnContest! with { ClaimedCardId = id, MovementSequence = moved.Sequence } });
            events.AdvanceFact(new ForeignTurnContestCardClaimedEvent(current.Id, Source(current), origin, id, moved.Sequence));
        }
        frames.AdvanceProgram(current.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 550)]
    internal bool ResumeForeignTurnContest(long frameId)
    {
        if (frames.Top is not ProgramSkillFrame frame || frame.Id != frameId || frame.ForeignTurnContest is not { } receipt) return false;
        Assert(frame);
        if (receipt.Stage == ForeignTurnContestStage.SlashIssued)
            throw new InvalidOperationException("A foreign virtual Slash returned without its exact typed receipt.");
        if (receipt.Stage == ForeignTurnContestStage.Complete) { continuations.Finish(frame, true); return true; }
        if (continuations.BeginRecoveryReplacement(frame.Id) || continuations.BeginHpChanged(frame.Id) ||
            movementWindows.Begin(frame.Id, subjectSeat: frame.OwnerSeat)) return true;
        frames.ReplaceTop(frame = frame with { PendingMovementContinuation = null });
        if (facts.Winner != Winner.None || !participants.IsAlive(frame.OwnerSeat) || !participants.IsAlive(receipt.Origin.Result.OpponentSeat) ||
            !facts.HasSkillInstance(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId) ||
            !slashes.CanIssue(receipt.Origin.Result.OpponentSeat, frame.OwnerSeat))
        {
            frames.ReplaceTop(frame = frame with { ForeignTurnContest = receipt with { Stage = ForeignTurnContestStage.Complete } });
            continuations.Finish(frame, false);
            return true;
        }
        var issue = slashes.Capture(frame.Id, frame.InstructionIndex, Source(frame), receipt.Origin);
        frames.ReplaceTop(frame with { ForeignTurnContest = receipt with { Stage = ForeignTurnContestStage.SlashIssued, SlashReturn = issue.Return } });
        slashes.Commit(issue);
        return true;
    }

    internal void CompleteSlash(ForeignTurnContestSlashCompletion completion)
    {
        var returned = completion.Return;
        var frame = frames.ActiveProgram(returned.ProgramFrameId);
        var receipt = frame.ForeignTurnContest;
        if (completion.ProgramFrameId != frame.Id || completion.ResolutionId != returned.CardUseFrameId ||
            receipt is not { Stage: ForeignTurnContestStage.SlashIssued } || receipt.SlashReturn != returned ||
            returned.Source != Source(frame) || returned.Origin != receipt.Origin || returned.InstructionIndex != frame.InstructionIndex ||
            ContainsCardUse(returned.CardUseFrameId) ||
            !history.Read<CardUseFinishedEvent>().Any(e => e.ResolutionId == returned.CardUseFrameId))
            throw new InvalidOperationException("The foreign Slash completed outside its original program and paid result.");
        frames.ReplaceTop(frame = frame with { ForeignTurnContest = receipt with { Stage = ForeignTurnContestStage.Complete } });
        events.AdvanceFact(new ForeignTurnContestSlashReturnedEvent(returned));
        continuations.Finish(frame, true);
    }

    internal void Assert(ProgramSkillFrame frame)
    {
        if (frame.ForeignTurnContest is not { } receipt) return;
        if (receipt.InstructionIndex != frame.InstructionIndex || frame.InstructionIndex != 3 ||
            !MatchesOrigin(frame, receipt.SourceBind, receipt.Origin) || !Enum.IsDefined(receipt.Stage) ||
            receipt.RestrictionIssued != receipt.Origin.Result.SourceWon ||
            receipt.RestrictionIssued && receipt.Stage != ForeignTurnContestStage.Complete ||
            receipt.ClaimedCardId is { } id && (id != receipt.Origin.Result.OpponentCardId || receipt.MovementSequence is not { } sequence ||
                !movements.Read().Any(item => item.Sequence == sequence && item.CardId == id && item.From == CardLocation.DiscardPile &&
                    item.To == CardLocation.Hand(frame.OwnerSeat) && item.Reason.Value == ClaimReason) ||
                history.Read<ForeignTurnContestCardClaimedEvent>().Count(e =>
                    e.ProgramFrameId == frame.Id && e.Source == Source(frame) && e.Origin == receipt.Origin && e.CardId == id && e.MovementSequence == sequence) != 1) ||
            receipt.ClaimedCardId is null && receipt.MovementSequence is not null)
            throw new InvalidOperationException("A foreign contest changed its exact original result, turn or paid claim.");
        if (receipt.RestrictionIssued && !history.Read<ForeignTurnContestRestrictedEvent>().Any(e =>
            e.ProgramFrameId == frame.Id && e.Source == Source(frame) && e.Origin == receipt.Origin &&
            turnEffects.TargetRestrictions.Any(grant => grant.GrantSequence == e.GrantSequence && grant.ParentFrameId == frame.Id &&
                grant.EffectIndex == frame.InstructionIndex - 1 && grant.SubjectSeat == receipt.Origin.Result.OpponentSeat &&
                grant.Restriction == SkillProgramCardTargetRestriction.SelfOnly && grant.TurnNumber == turn.TurnNumber && grant.TurnSeat == turn.CurrentSeat)))
            throw new InvalidOperationException("The foreign contest lost its actual-turn self-only issuance.");
        if (receipt.SlashReturn is { } returned && (returned.ProgramFrameId != frame.Id || returned.InstructionIndex != frame.InstructionIndex ||
            returned.Source != Source(frame) || returned.Origin != receipt.Origin ||
            !history.Read<ForeignTurnContestSlashIssuedEvent>().Any(e => e.Return == returned)))
            throw new InvalidOperationException("A foreign contest lost its exact issued Slash child.");
    }

    internal void AssertSlashChildren()
    {
        for (var index = 0; index < nativeFrames.FrameCount; index++)
        {
            if (nativeFrames.FrameAt(index) is not CardUseFrame { ForeignTurnContestSlashReturn: not null } use) continue;
            var returned = use.ForeignTurnContestSlashReturn!;
            if (index == 0 || nativeFrames.FrameAt(index - 1) is not ProgramSkillFrame parent ||
                parent.Id != returned.ProgramFrameId || parent.ForeignTurnContest is not { Stage: ForeignTurnContestStage.SlashIssued } contest ||
                contest.SlashReturn != returned || returned.Origin != contest.Origin ||
                use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } ||
                (use.CurrentSlashFirePolicy?.OriginalAction ?? use.Action) is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action ||
                action.ActionId != returned.CardActionId || action.ActorSeat != returned.Origin.Result.OpponentSeat ||
                action.ProviderSeat != action.ActorSeat || action.PhysicalCards.Count != 0 ||
                !history.Read<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id &&
                    e.TargetSeats.SequenceEqual([returned.Origin.Result.SourceSeat])) ||
                use.TargetSeats.Count > 1 && !facts.HasAllowedSlashTargetTail(use))
                throw new InvalidOperationException("A foreign virtual Slash lost its exact issued zero-entity use and typed parent.");
        }
    }
}
