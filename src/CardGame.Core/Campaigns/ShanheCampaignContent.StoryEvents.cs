namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    private static void ValidateStoryEvents(ShanheNodeDefinition node)
    {
        if (node.StoryEvents is not { } stories) return;
        Require(node.Kind == ShanheNodeKind.Event && stories.Count is > 0 and <= 64 && (node.Choices?.Count ?? 0) == 0,
            "Only event nodes may use a bounded story pool instead of fixed choices.");
        UniqueIds(stories.Select(story => story?.Id ?? ""), "story event");
        foreach (var story in stories)
        {
            RequireText(story.Name, "story event name");
            Require(story.StoryText is not null && story.StoryText.Length <= 16_384 && story.Choices is { Count: > 0 and <= 64 },
                "A story event requires bounded narrative and choices.");
            UniqueIds(story.Choices!.Select(choice => choice?.Id ?? ""), "story choice");
            if (story.SkillRewardIds is { } skills)
            {
                Require(skills.Count is >= 2 and <= 512, "A skill story requires a bounded pool with at least two skills.");
                UniqueIds(skills, "story skill reward");
                Require(story.Choices.All(choice => !choice.Id.StartsWith("story-skill:", StringComparison.Ordinal)),
                    "Authored story choices cannot use generated skill choice ids.");
            }
        }
    }

    private static ShanheNodeChoiceDefinition FreezeStoryChoice(ShanheNodeChoiceDefinition choice) => choice with
    { Companion = choice.Companion is null ? null : FreezeCombatant(choice.Companion) };
}
