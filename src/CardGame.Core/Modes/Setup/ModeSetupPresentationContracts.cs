namespace CardGame.Core;

/// <summary>Scalar facts for the current selection; no mutable general or character is exposed.</summary>
public sealed record ModeGeneralSelectionFacts(Role Role, bool GeneralSelected,
    string PrimaryGeneralName, int PrimaryBaseHp);

public sealed record ModeGeneralChoiceFacts(string Id, string Name, string SkillSummary,
    string? FactionId, string FactionName, int BaseHp, int? InitialHp, string CandidateId);

public sealed class ModeSetupCompletionFacts
{
    public ModeSetupCompletionFacts(IEnumerable<string> publicGeneralNames) =>
        PublicGeneralNames = Array.AsReadOnly((publicGeneralNames ?? throw new ArgumentNullException(nameof(publicGeneralNames))).ToArray());
    public IReadOnlyList<string> PublicGeneralNames { get; }
}

public interface IMatchModeSetupPresentation : IGameModeFeature
{
    PromptChoice GeneralChoice(ModeGeneralSelectionFacts player, ModeGeneralChoiceFacts general, bool fullRoster = false);
    string SelectionPrompt(ModeGeneralSelectionFacts player);
    string SetupCompletedText(ModeSetupCompletionFacts facts);
}
