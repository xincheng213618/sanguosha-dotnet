using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using System.Windows.Media;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public bool IsShanheStoryEventOpen => _shanheSession is { Snapshot.Phase: ShanheCampaignPhase.NodeChoice } session &&
        session.Definition.Nodes.Single(node => node.Id == session.Snapshot.CurrentNodeId).StoryEvents is not null;
    private string ShanheRecruitedCompanionsText => _shanheSession?.Snapshot.RecruitedCompanions is { Count: > 0 } companions
        ? "\n奇遇队友：" + string.Join("、", ShanheRecruitedCompanionCards.Select(companion => companion.Name)) : "";

    public IReadOnlyList<ShanheStoryCompanionCard> ShanheRecruitedCompanionCards => _shanheSession is not { } session ? [] :
        session.Snapshot.RecruitedCompanions.Select(companion =>
        {
            var choice = session.Definition.Nodes.SelectMany(node => node.StoryEvents ?? []).SelectMany(story => story.Choices)
                .FirstOrDefault(choice => choice.Companion is { } member && member.GeneralId == companion.GeneralId &&
                    (member.ExtraSkillIds ?? []).SequenceEqual(companion.ExtraSkillIds ?? []));
            var probe = new ShanheBattleSetup(1, 1, "companion-display", session.Snapshot.PlayerGeneralId, 1, 0, 0, 0, 0,
                [], [], [], [new("classic:cao-cao")], [companion], session.ContentHash);
            var name = StandardShanheCampaignCatalog.CreateBattleRegistry(probe).Generals[companion.GeneralId].Name;
            return new ShanheStoryCompanionCard(name.Replace("界蔡文姬", "蔡文姬", StringComparison.Ordinal) + (choice?.GoldCost > 0 ? " · 精英" : " · 普通"),
                GeneralArt.GetPortrait(companion.GeneralId, decodeWidth: 120), choice?.Description ?? "奇遇中加入的同行队友。");
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

public sealed record ShanheStoryCompanionCard(string Name, Brush Portrait, string Description);
