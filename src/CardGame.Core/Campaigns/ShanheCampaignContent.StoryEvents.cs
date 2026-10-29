namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    private static void ValidateStoryEvents(ShanheNodeDefinition node)
    {
        if (node.StoryEvents is null && node.SupplementalStoryEvents is null) return;
        var stories = AllStoryEvents(node).ToArray();
        Require(node.Kind == ShanheNodeKind.Event && stories.Length is > 0 and <= 64 && (node.Choices?.Count ?? 0) == 0,
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
            if (story.TacticRewardIds is { } tactics)
            {
                Require(story.SkillRewardIds is null && tactics.Count is >= 2 and <= 512,
                    "A tactic story requires its own bounded pool with at least two tactics.");
                UniqueIds(tactics, "story tactic reward");
                Require(story.Choices.All(choice => !choice.Id.StartsWith("story-tactic:", StringComparison.Ordinal)),
                    "Authored story choices cannot use generated tactic choice ids.");
            }
        }
    }

    private static IEnumerable<ShanheStoryEventDefinition> AllStoryEvents(ShanheNodeDefinition node) =>
        (node.StoryEvents ?? []).Concat(node.SupplementalStoryEvents ?? []);

    private static ShanheStoryEventDefinition FreezeStoryEvent(ShanheStoryEventDefinition story) => story with
    {
        Choices = Freeze(story.Choices.Select(FreezeStoryChoice)),
        SkillRewardIds = story.SkillRewardIds is null ? null : Freeze(story.SkillRewardIds),
        TacticRewardIds = story.TacticRewardIds is null ? null : Freeze(story.TacticRewardIds)
    };

    private static ShanheNodeChoiceDefinition FreezeStoryChoice(ShanheNodeChoiceDefinition choice) => choice with
    { Companion = choice.Companion is null ? null : FreezeCombatant(choice.Companion) };
}
