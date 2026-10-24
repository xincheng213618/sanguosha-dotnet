namespace CardGame.Core;

internal static class BoundaryWuGuoTaiMobile
{
    internal const string Ganlu = "boundary:ganlu-mobile", Buyi = "boundary:buyi-mobile";
}
internal interface IBoundaryWuGuoTaiQueries : IProgramSelectionQuery { bool CanExchange(int owner, int first, int second); }

[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 57100, SkillProgramEffectOp.ExchangeSelectedTargetEquipment)]
[ProgramSelectionModule(ProgramSelectionStage.Relationships, 57100, SkillProgramEffectOp.ExchangeSelectedTargetEquipment)]
internal sealed class BoundaryWuGuoTaiGanluPairs : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        if (context.SkillId != BoundaryWuGuoTaiMobile.Ganlu) return true;
        var players = queries.Get<ISelectionParticipantQueries>();
        var rules = queries.Get<IBoundaryWuGuoTaiQueries>();
        var pairs = players.Seats.SelectMany(first => players.Seats.Where(second => first < second && rules.CanExchange(context.OwnerSeat, first, second))
            .Select(second => (IReadOnlyList<int>)Array.AsReadOnly(new[] { first, second }))).ToArray();
        candidates.TargetSequences = Array.AsReadOnly(pairs);
        candidates.Targets = pairs.SelectMany(pair => pair).Distinct().Order().ToArray();
        return pairs.Length > 0;
    }
}

[ProgramSelectionValidator(ProgramSelectionValidationStage.BeforePublicPrerequisites, 57100, SkillProgramEffectOp.ExchangeSelectedTargetEquipment)]
internal sealed class BoundaryWuGuoTaiGanluSelectionValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries) =>
        request.Context.SkillId != BoundaryWuGuoTaiMobile.Ganlu || request.Targets is [var first, var second] &&
            queries.Get<IBoundaryWuGuoTaiQueries>().CanExchange(request.Context.OwnerSeat, first, second)
        ? null : new(CommandErrorCode.InvalidTarget, "甘露须选择两名不同的存活角色；不包含本人时装备数之差不得超过已损失体力。");
}
