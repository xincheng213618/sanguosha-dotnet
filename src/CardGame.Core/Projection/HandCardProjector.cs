namespace CardGame.Core;

internal readonly record struct HandCardIdentityProjection(CardKind Kind, string SkillId);

internal interface IHandCardProjectionRules : IProgramSelectionQuery
{
    HandCardIdentityProjection? FirstIdentity(CharacterState owner, Card card);
    int? Rank(CharacterState owner, Card card);
    CardSnapshot Decorate(CharacterState owner, Card card, CardSnapshot snapshot);
}

internal interface IHandCardViewQueries : IProgramSelectionQuery
{
    CardSnapshot Project(CharacterState owner, Card card);
    string? AuthorityName(IReadOnlyList<GeneralSkillDefinition>? skills);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandCardProjector(IProgramCardSnapshotQueries cards, IProgramContentQueries content,
    IHandCardProjectionRules rules) : IHandCardViewQueries
{
    public CardSnapshot Project(CharacterState owner, Card card)
    {
        var snapshot = cards.Snapshot(card);
        var identity = rules.FirstIdentity(owner, card);
        if (identity is { } selected)
            snapshot = snapshot with
            {
                IdentityKind = selected.Kind,
                IdentitySkillName = content.SkillName(selected.SkillId)
            };
        if (rules.Rank(owner, card) is { } rank)
            snapshot = snapshot with { Rank = rank, RankText = rank.ToString(System.Globalization.CultureInfo.InvariantCulture) };
        return rules.Decorate(owner, card, snapshot);
    }

    public string? AuthorityName(IReadOnlyList<GeneralSkillDefinition>? skills)
    {
        if (skills is null) return null;
        foreach (var skill in skills)
            if (skill.ContentId is { } contentId && content.AuthorityName(contentId) is { } name)
                return name;
        return null;
    }
}
