namespace CardGame.Core.Campaigns;

public sealed record ShanheBossRewardGroupDefinition(string Id, string Name, IReadOnlyList<string> RewardIds, int ChoiceCount = 3);
public sealed record ShanheBossRewardDefinition(int FixedGold, int SkillSlots, IReadOnlyList<ShanheBossRewardGroupDefinition> Groups);

public sealed record ShanheBossRewardGroupSnapshot(string Id, string Name,
    IReadOnlyList<ShanheRewardDefinition> Offers, ShanheRewardDefinition? SelectedReward)
{
    public bool IsSkipped { get; init; }
    public int SkipGold { get; init; }
    public bool CanSkip => !IsResolved && SkipGold > 0;
    public bool IsExhausted => Offers.Count == 0;
    public bool IsResolved => SelectedReward is not null || IsExhausted || IsSkipped;
}

public sealed record ShanheBossRewardSummary(int ChapterNumber, string Title, int FixedGold, int SkillSlots,
    IReadOnlyList<ShanheBossRewardGroupSnapshot> Groups, long Revision)
{
    public bool IsBonusOnly { get; init; }
    public bool CanConfirm => Groups.All(group => group.IsResolved);
}
