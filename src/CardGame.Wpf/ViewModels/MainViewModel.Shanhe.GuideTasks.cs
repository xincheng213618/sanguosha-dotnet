using CardGame.Core.Campaigns;
using CardGame.Content.Standard;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private ShanheCodexProfile? _guideTasksProfile;
    private IReadOnlyList<ShanheGuideTaskChapter>? _guideTaskChapters;
    private string? _guideTasksEdition;
    private int _guideTasksDifficulty;
    public string ShanheGuideTaskHeader => "章节任务 · 4 项进度 / 2 项待核";
    public string ShanheGuideTaskNotice => SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId
        ? "选择符合条件的难度完成任务，进度自动累计，每项奖励10册且只领取一次。两个评分任务暂不结算。"
        : "四项任务可在2026来源25档版本完成，当前本地三档不计入任务进度。两个评分任务保留原条件，待计分规则核实。";

    private void RefreshShanheGuideTasks()
    {
        foreach (var property in new[] { nameof(ShanheGuideTaskHeader), nameof(ShanheGuideTaskNotice), nameof(ShanheGuideTaskChapters) })
            RaisePropertyChanged(property);
    }
    public IReadOnlyList<ShanheGuideTaskChapter> ShanheGuideTaskChapters
    {
        get
        {
            var edition = SelectedShanheScenario.Id;
            var difficulty = SelectedShanheDifficulty?.Value ?? 1;
            if (!ReferenceEquals(_guideTasksProfile, _shanheCodexProfile) || _guideTaskChapters is null ||
                _guideTasksEdition != edition || _guideTasksDifficulty != difficulty)
            {
                _guideTasksProfile = _shanheCodexProfile;
                _guideTasksEdition = edition; _guideTasksDifficulty = difficulty;
                _guideTaskChapters = Array.AsReadOnly(ShanheGuideTaskCatalog.Tasks.GroupBy(task => task.Stage).OrderBy(group => group.Key)
                    .Select(group => new ShanheGuideTaskChapter(group.Key,
                        Array.AsReadOnly(group.Select(task => new ShanheGuideTaskCard(task, _shanheCodexProfile,
                            edition == StandardShanheCampaignCatalog.Source2026EditionId, difficulty)).ToArray()))).ToArray());
            }
            return _guideTaskChapters;
        }
    }
}

public sealed record ShanheGuideTaskChapter(int Stage, IReadOnlyList<ShanheGuideTaskCard> Tasks)
{
    public string Title => $"第{(Stage == 1 ? "一" : "二")}章";
}

public sealed class ShanheGuideTaskCard
{
    internal ShanheGuideTaskCard(ShanheGuideTaskDefinition source, ShanheCodexProfile profile, bool sourceEdition, int difficulty)
    {
        SourceId = source.SourceId; Name = source.Name; Condition = source.Description;
        RequiresScore = source.RequiresScore; RewardLabel = $"奖励 {source.BookReward} 册";
        VerificationNote = source.SourceId switch
        {
            200347 => "当前可计入版本：经典袁术。达成后自动结算10册，每个账号仅领取一次。",
            200357 => "当前可计入版本：经典曹操、OL魔曹操。达成后自动结算10册，每个账号仅领取一次。",
            _ => RequiresScore ? "战绩计分规则待核实，暂不累计分数或结算本任务。" : "达成后自动结算10册，每个账号仅领取一次。"
        };
        var progress = profile.GuideTaskProgress ?? ShanheGuideTaskProgressState.Empty;
        CompletedCount = progress.Count(source.SourceId); RequiredCount = source.RequiredCount;
        IsClaimed = ShanheGuideTaskSettlement.HasClaimed(profile, source.SourceId);
        CanProgress = !IsClaimed && !RequiresScore && sourceEdition && difficulty >= source.MinimumSourceDifficultyId;
        Availability = IsClaimed ? $"已完成 · {CompletedCount}/{RequiredCount} · 图册已入账"
            : RequiresScore ? "待开放 · 计分依据待核"
            : !sourceEdition ? $"切换至2026来源25档 · 进度 {CompletedCount}/{RequiredCount}"
            : !CanProgress ? $"需{ShanheGuideTaskCatalog.SourceDifficultyName(source.MinimumSourceDifficultyId)}及以上 · 进度 {CompletedCount}/{RequiredCount}"
            : $"可挑战 · 进度 {CompletedCount}/{RequiredCount}";
    }

    public int SourceId { get; }
    public string Name { get; }
    public string Condition { get; }
    public bool RequiresScore { get; }
    public string RewardLabel { get; }
    public string VerificationNote { get; }
    public string Availability { get; }
    public int CompletedCount { get; }
    public int RequiredCount { get; }
    public bool IsClaimed { get; }
    public bool CanProgress { get; }
}
