namespace CardGame.Core;

internal static class MobileLuSuDimengSelection
{
    internal static bool CanPay(ProgramSelectionContext context, IReadOnlyList<int> targets, IProgramSelectionQueries queries)
    {
        if (targets is not [var first, var second] || first == second || targets.Contains(context.OwnerSeat)) return false;
        var source = queries.Get<IProgramSelectionSourceQuery>(); var players = source.Participants();
        if (targets.Any(seat => !players.Any(p => p.Seat == seat && p.IsAlive))) return false;
        var cards = queries.Get<IProgramSelectionCardState>();
        var available = source.Hand(context.OwnerSeat).Concat(source.Equipment(context.OwnerSeat))
            .Count(card => !cards.IsSelfDiscardForbidden(context.OwnerSeat, card, source.Location(card.Id)));
        return available >= Math.Abs(players.Single(p => p.Seat == first).HandCount - players.Single(p => p.Seat == second).HandCount);
    }
}
[ProgramSelectionModule(ProgramSelectionStage.Projection, 71501, SkillProgramEffectOp.ExchangeSelectedTargetHands)]
internal sealed class MobileLuSuDimengSelectionFilter : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        if (context.SkillId != "mobile:dimeng") return true;
        var players = queries.Get<IProgramSelectionSourceQuery>().Participants();
        var pairs = candidates.Targets.SelectMany(first => candidates.Targets.Where(second => first < second)
            .Select(second => new[] { first, second })).Where(pair => MobileLuSuDimengSelection.CanPay(context, pair, queries))
            .Select(pair => (IReadOnlyList<int>)Array.AsReadOnly(pair.OrderBy(seat => players.Single(p => p.Seat == seat).HandCount).ThenBy(seat => seat).ToArray())).ToArray();
        candidates.TargetSequences = Array.AsReadOnly(pairs);
        candidates.Targets = candidates.Targets.Where(seat => pairs.Any(pair => pair.Contains(seat))).ToArray();
        return pairs.Length > 0;
    }
}
[ProgramSelectionValidator(ProgramSelectionValidationStage.BeforePublicPrerequisites, 71501, SkillProgramEffectOp.ExchangeSelectedTargetHands)]
internal sealed class MobileLuSuDimengSelectionValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) =>
        request.Context.SkillId != "mobile:dimeng" || MobileLuSuDimengSelection.CanPay(request.Context, request.Targets, queries)
            ? null : new(CommandErrorCode.IllegalAction, "缔盟须有足额可弃置的手牌和装备牌支付两名目标的手牌数差额。");
}

public sealed partial class GameEngine
{
    // Mobile Dimeng pays before exchanging. The already paid original cost is
    // retained by the native binding and movement ledger, including source loss
    // in the real discard child; neither the cost nor the hand swap is reissued.
    private bool MobileLuSuDimengPaid(ProgramSkillFrame f) => f.SkillId == "mobile:dimeng" && f.TriggerId is null &&
        f.ActivationId == "alliance-pact" && f.InstructionIndex is 2 or 3 && f.SelectedTargetSeats is [var first, var second] && first != second &&
        f.CardSetBindings.SingleOrDefault(b => b.Name == "dimeng-cost") is { CardIds.Count: > 0 } cost &&
        cost.CardIds.Select((id, i) => (Id: id, From: cost.SourceLocations[i])).All(c =>
            c.From.OwnerSeat == f.OwnerSeat && c.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
            CompleteProgramEventHistory().SkipWhile(e => e is not ProgramSkillStartedEvent started || started.FrameId != f.Id).OfType<CardMovedEvent>()
                .Count(m => m.CardId == c.Id && m.From == c.From && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.mobile:dimeng.MoveBoundCards") == 1) &&
        ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 71500)]
    private bool KeepMobileLuSuDimengPaid(ProgramSkillFrame f) => MobileLuSuDimengPaid(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 71500)]
    private bool ReturnMobileLuSuDimengPaid(ProgramSkillFrame f)
    {
        if (!MobileLuSuDimengPaid(f) || f.PendingMovementContinuation is not { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } || seat != f.OwnerSeat) return false;
        ReplaceRuntimeTop(f with { PendingMovementContinuation = null }); AdvanceRuntimeProgram(f.Id); return true;
    }
}
