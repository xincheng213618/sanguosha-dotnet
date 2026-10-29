using System.Windows.Media;
using CardGame.Core;

namespace CardGame.Wpf.ViewModels;

/// <summary>A passive table badge, separate from activatable general skills.</summary>
public sealed record WushuangTacticViewModel(int Id)
{
    private sealed record ContentPresentation(string Name, string Description, int Quality, string? SkillId);
    private ContentPresentation? Content { get; init; }
    private WushuangTactic Tactic => WushuangTactics.ById[Id];
    public static WushuangTacticViewModel FromContent(int artworkId, string name, string description, int quality, string? skillId) =>
        new(artworkId) { Content = new(name, description, quality, skillId) };
    public string Name => Content?.Name ?? Tactic.Name;
    public string DisplayName => ShanheDisplayText.RewardName(Name);
    public string? SkillId => Content is { } content ? content.SkillId : Tactic.SkillId;
    private int Quality => Content?.Quality ?? Tactic.Quality;
    private string TooltipHeading => Name.StartsWith("战法·", StringComparison.Ordinal) || Name.StartsWith("战法 ·", StringComparison.Ordinal)
        ? Name : $"战法 · {Name}";
    public string Tooltip => $"{TooltipHeading}\n{Content?.Description ?? Tactic.Description}";
    public ImageSource? Icon => ShanheArt.Get(Id);
    public ImageSource? Frame => WushuangArt.Get($"rogue_zhanfa_bg_{Quality}.png");
    public string Foreground => Quality switch
    {
        2 => "#DEC5F1", 3 => "#F5D38B", 4 => "#FFAF87", _ => "#E6D1AE"
    };
}
