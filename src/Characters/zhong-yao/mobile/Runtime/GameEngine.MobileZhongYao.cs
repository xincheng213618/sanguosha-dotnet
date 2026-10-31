namespace CardGame.Core;

internal interface IMobileZhongYaoSelectionQueries : IProgramSelectionQuery
{
    IReadOnlyList<int> EligibleTargets(int ownerSeat);
}

[ProgramTargetResolver(SkillProgramTargetKind.MobileZhongYaoZuodingTarget)]
internal sealed class MobileZhongYaoZuodingTargetResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) =>
        queries.Get<IMobileZhongYaoSelectionQueries>().EligibleTargets(context.OwnerSeat);
}
