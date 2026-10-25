namespace CardGame.Core;

public sealed record MobileJiyuInvokedEvent(long FrameId, CardConversionSource Source, string Hash,
    SkillGrantPhaseExpiry Phase, int TurnOwner, int Target, bool ExtraUse) : IGameEvent;
public sealed record MobileJiyuCompletedEvent(long FrameId, bool EarnedExtraUse) : IGameEvent;

internal interface IMobileJiyuSelection : IProgramSelectionQuery
{
    int PhaseLimit(int owner);
    bool MaySelect(int owner, int target);
}
[ProgramSelectionModule(ProgramSelectionStage.PhaseLimit, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuPhaseLimit : IProgramSelectionPhaseLimitProvider
{
    public int? GetLimit(ProgramSelectionContext context, IProgramSelectionQueries queries) => context.SkillId == "mobile:jiyu"
        ? queries.Get<IMobileJiyuSelection>().PhaseLimit(context.OwnerSeat) : context.Activation.UsesPerPhase;
}
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuTargetFilter : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int targetSeat, IProgramSelectionQueries queries) => context.SkillId != "mobile:jiyu" ||
        queries.Get<IMobileJiyuSelection>().MaySelect(context.OwnerSeat, targetSeat);
}
[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 78300, SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition)]
internal sealed class MobileJiyuTargetValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) => request.Context.SkillId != "mobile:jiyu" ||
        request.Targets is [var target] && queries.Get<IMobileJiyuSelection>().MaySelect(request.Context.OwnerSeat, target)
        ? null : new(CommandErrorCode.IllegalAction, "讥谀的额外次数不能选择本回合已经选择过的角色。");
}
internal static class MobileJiyuComposition
{
    [ProgramCatalogValidation(78300)]
    private static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "mobile:jiyu") return;
        if (program.Triggers.Count != 0 || program.Activations is not [{ Id: "hand-discard-restriction", MinCards: 0, MaxCards: 0,
            MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.AnyLivingWithHand, UsesPerTurn: null, UsesPerPhase: 1,
            Condition.Kind: SkillProgramConditionKind.Always, TargetPhaseLedgerId: null,
            Effects: [{ Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1,
                Zones: [CardZoneKind.Hand], ResultBind: "discard" },
                { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "discard", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                { Op: SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition, Target: SkillProgramEffectTarget.Owner, SourceBind: "discard" },
                { Op: SkillProgramEffectOp.TurnOver, Target: SkillProgramEffectTarget.Owner },
                { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] }])
            throw new InvalidOperationException(path + ": mobile Jiyu requires its original native discard, turn suit prohibition, turnover and HP loss in that order.");
    }
}

public sealed partial class GameEngine
{
    private const string MobileJiyu = "mobile:jiyu", MobileJiyuActivation = "hand-discard-restriction";
    private SkillGrantPhaseExpiry? MobileJiyuCurrentPlay(int owner) => _phase == TurnPhase.Play && _currentSeat == owner &&
        _turnNumber > 0 && _cardUseDebitPhaseInstanceId > 0 ? new(_turnNumber, owner, _cardUseDebitPhaseInstanceId) : null;
    private bool ValidMobileJiyuInvocation(MobileJiyuInvokedEvent invoked) => invoked.Source.SkillId == MobileJiyu &&
        invoked.Source.BindingId == MobileJiyuActivation && IsValidPlayerSeat(invoked.Source.OwnerSeat) && IsValidPlayerSeat(invoked.Target) &&
        IsValidPlayerSeat(invoked.TurnOwner) && invoked.Phase is { TurnNumber: > 0, PhaseInstanceId: > 0 } &&
        invoked.Phase.PhaseActorSeat == invoked.Source.OwnerSeat && invoked.Hash == _contentRegistry.GetSkill(MobileJiyu).Program?.GameplayHash &&
        ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == invoked.FrameId && e.OwnerSeat == invoked.Source.OwnerSeat &&
            e.SkillId == MobileJiyu && e.ActivationId == MobileJiyuActivation) == 1;
    private IEnumerable<MobileJiyuInvokedEvent> MobileJiyuInvocations(int owner) => ProgramEventHistory<MobileJiyuInvokedEvent>()
        .Where(e => e.Source.OwnerSeat == owner && ValidMobileJiyuInvocation(e));
    private int MobileJiyuLimit(int owner) => MobileJiyuCurrentPlay(owner) is { } phase ? 1 + MobileJiyuInvocations(owner)
        .Count(start => start.Phase == phase && ProgramEventHistory<MobileJiyuCompletedEvent>().Any(done => done.FrameId == start.FrameId && done.EarnedExtraUse)) : 0;
    private bool MobileJiyuMaySelect(int owner, int target, int usesBefore) => IsValidPlayerSeat(target) &&
        _players[target].IsAlive && GetHand(_players[target]).Count > 0 && (usesBefore == 0 ||
            !MobileJiyuInvocations(owner).Any(e => e.Phase.TurnNumber == _turnNumber && e.TurnOwner == _turnProgression.OwnerSeat && e.Target == target));
    [ProgramCapability]
    private sealed class MobileJiyuSelection(GameEngine engine) : IMobileJiyuSelection
    {
        public int PhaseLimit(int owner) => engine.MobileJiyuLimit(owner);
        public bool MaySelect(int owner, int target) => engine.MobileJiyuMaySelect(owner, target,
            engine._programPhaseUses.GetValueOrDefault((owner, MobileJiyu, MobileJiyuActivation)));
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 78300)]
    private void CaptureMobileJiyuInvocation(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != MobileJiyu) return;
        var frame = GetActiveProgramFrame(started.FrameId);
        var uses = _programPhaseUses.GetValueOrDefault((frame.OwnerSeat, MobileJiyu, MobileJiyuActivation));
        if (frame.TriggerId is not null || frame.ActivationId != MobileJiyuActivation || frame.InstructionIndex != 0 ||
            frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var target] || MobileJiyuCurrentPlay(frame.OwnerSeat) is not { } phase ||
            uses < 1 || uses > MobileJiyuLimit(frame.OwnerSeat) || !MobileJiyuMaySelect(frame.OwnerSeat, target, uses - 1) ||
            ProgramEventHistory<MobileJiyuInvokedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Mobile Jiyu must freeze the original actual-Play allowance and target before its native discard.");
        AdvanceEventRulesAndQueueFact(new MobileJiyuInvokedEvent(frame.Id,
            new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            phase, _turnProgression.OwnerSeat, target, uses > 1));
    }
    private bool HasMobileJiyuOriginalSelection(ProgramSkillFrame frame) => frame.SkillId == MobileJiyu && frame.TriggerId is null &&
        frame.ActivationId == MobileJiyuActivation && frame.SelectedCardIds.Count == 0 &&
        ProgramEventHistory<MobileJiyuInvokedEvent>().Where(e => e.FrameId == frame.Id).ToArray() is [var invoked] &&
        ValidMobileJiyuInvocation(invoked) && invoked.Source == new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) &&
        invoked.Hash == frame.GameplayHash && frame.SelectedTargetSeats.SequenceEqual([invoked.Target]);
    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 78300)]
    private bool KeepMobileJiyuInitialSelection(ProgramSkillFrame frame) => HasMobileJiyuOriginalSelection(frame);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 78300)]
    private void AssertMobileJiyuOriginalSelection(ProgramSkillFrame frame)
    {
        if (frame.SkillId == MobileJiyu && !HasMobileJiyuOriginalSelection(frame))
            throw new InvalidOperationException("Mobile Jiyu lost its committed original target and actual Play allowance.");
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 78301)]
    private void CompleteMobileJiyuInvocation(ProgramSkillResolvedEvent resolved)
    {
        if (resolved.SkillId != MobileJiyu || !resolved.Completed) return;
        var frame = GetActiveProgramFrame(resolved.FrameId);
        if (!HasMobileJiyuOriginalSelection(frame) || ProgramEventHistory<MobileJiyuCompletedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Mobile Jiyu cannot award the original actual-Play allowance more than once.");
        AdvanceEventRulesAndQueueFact(new MobileJiyuCompletedEvent(frame.Id, HasActuallyUsableHandCard(_players[frame.OwnerSeat])));
    }
}
