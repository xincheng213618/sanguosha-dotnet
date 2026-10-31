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
