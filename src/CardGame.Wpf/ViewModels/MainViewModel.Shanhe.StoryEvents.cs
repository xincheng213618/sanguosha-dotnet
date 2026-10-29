using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using System.Windows.Media;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public bool IsShanheStoryEventOpen => _shanheSession is { Snapshot.Phase: ShanheCampaignPhase.NodeChoice } session &&
        session.Snapshot.CurrentStoryEventName is not null;
    private string ShanheRecruitedCompanionsText => _shanheSession?.Snapshot.RecruitedCompanions is { Count: > 0 } companions
        ? "\n奇遇队友：" + string.Join("、", ShanheRecruitedCompanionCards.Select(companion => companion.Name)) : "";

    public IReadOnlyList<ShanheStoryCompanionCard> ShanheRecruitedCompanionCards => _shanheSession is not { } session ? [] :
        session.Snapshot.RecruitedCompanions.Select(companion =>
        {
            var choice = session.Definition.Nodes.SelectMany(node => (node.StoryEvents ?? []).Concat(node.SupplementalStoryEvents ?? [])).SelectMany(story => story.Choices)
                .FirstOrDefault(choice => choice.Companion is { } member && member.GeneralId == companion.GeneralId &&
                    (member.ExtraSkillIds ?? []).SequenceEqual(companion.ExtraSkillIds ?? []));
            var probe = new ShanheBattleSetup(1, 1, "companion-display", session.Snapshot.PlayerGeneralId, 1, 0, 0, 0, 0,
                [], [], [], [new("classic:cao-cao")], [companion], session.ContentHash);
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
            var name = registry.Generals[companion.GeneralId].Name;
            var rules = ShanheCompanionRules(registry, companion);
            var description = string.IsNullOrWhiteSpace(choice?.Description) ? rules : choice.Description + "\n\n" + rules;
            return new ShanheStoryCompanionCard(name.Replace("界蔡文姬", "蔡文姬", StringComparison.Ordinal) + (choice?.GoldCost > 0 ? " · 精英" : " · 普通"),
                GeneralArt.GetPortrait(companion.GeneralId, decodeWidth: 120), description);
        }).ToArray();

    private string ShanheStoryChoiceDescription(ShanheNodeChoiceDefinition choice, ShanheRewardDefinition? reward)
    {
        var description = string.IsNullOrWhiteSpace(choice.Description) ? reward?.Description ?? "继续当前路线。" : choice.Description;
        if (choice.Companion is { } companion && _shanheSession is { } session &&
            session.Snapshot.StartingCompanions.Concat(session.Snapshot.RecruitedCompanions).Any(member => member.GeneralId == companion.GeneralId))
            description += "\n此队友已加入。";
        return description;
    }
}

public sealed record ShanheStoryCompanionCard(string Name, Brush Portrait, string Description)
{
    public bool HasPortrait => Portrait is ImageBrush { ImageSource: not null };
    public string Glyph
    {
        get
        {
            var name = Name.Split('·')[0].Trim();
            return name[..Math.Min(2, name.Length)];
        }
    }
}
