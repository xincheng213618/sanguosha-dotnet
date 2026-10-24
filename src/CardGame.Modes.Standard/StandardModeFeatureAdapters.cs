using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed class StandardModeSetupAdapter(StandardModeSetupFeature inner) : IMatchModeSetupFeature
{
    public ModeSetupPlan BuildPlayers() => inner.BuildPlayers();
    public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request) => inner.GetGeneralOffers(request);
    public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string generalId) => inner.RemoveGeneralIdsAfterSelection(generalId);
    public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => inner.PrepareDeck(original);
    public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => inner.ProjectDeckCardNames(original);
    public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
    public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats) =>
        inner.PrepareSelectedEquipment(drawPile, seats);
    public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) =>
        inner.PrepareSelectedGeneral(seat, generalId);
}

internal sealed class StandardModeProfileAdapter(StandardModeSetupFeature inner) : IMatchModeRuleProvider,
    IMatchModeConfigurationValidator, IMatchModeEquipmentProfile, IMatchModePresentation,
    IMatchModeSetupPresentation, IMatchModeFactionPolicy, IMatchModeAllegiancePolicy, IMatchModeCommandHandler
{
    public ModeRuleSet Rules => inner.Rules;
    public void ValidateOptions() => inner.ValidateOptions();
    public ModeEquipmentRecipe EquipmentRecipe => inner.EquipmentRecipe;
    public string StartLog(ModePresentationContext context) => inner.StartLog(context);
    public string RulesText(ModePresentationContext context) => inner.RulesText(context);
    public string WinnerLog(ModeWinnerPresentationContext context) => inner.WinnerLog(context);
    public PromptChoice GeneralChoice(ModeGeneralSelectionFacts player, ModeGeneralChoiceFacts general, bool fullRoster = false) =>
        inner.GeneralChoice(player, general, fullRoster);
    public string SelectionPrompt(ModeGeneralSelectionFacts player) => inner.SelectionPrompt(player);
    public string SetupCompletedText(ModeSetupCompletionFacts facts) => inner.SetupCompletedText(facts);
    public string? EffectiveFaction(ModeFactionFacts facts) => inner.EffectiveFaction(facts);
    public string? ScenarioGodFaction(string? printedFaction) => inner.ScenarioGodFaction(printedFaction);
    public IReadOnlyList<string> NativeFactionChoices => inner.NativeFactionChoices;
    public bool AllowsAvatarFactionChanges => inner.AllowsAvatarFactionChanges;
    public bool AllowsLordSkillProjection => inner.AllowsLordSkillProjection;
    public bool AllowsIdentityConcealment => inner.AllowsIdentityConcealment;
    public bool AllowsPublicTeamSkillSynergy => inner.AllowsPublicTeamSkillSynergy;
    public bool IsPublicFactionVisible(ModeFactionVisibilityFacts facts) => inner.IsPublicFactionVisible(facts);
    public Role? PublicIdentityDeathRole(ModeIdentityDeathFacts facts) => inner.PublicIdentityDeathRole(facts);
    public bool SamePublicTeam(ModeAllegianceSeatFacts first, ModeAllegianceSeatFacts second) => inner.SamePublicTeam(first, second);
    public bool PublicTeamEnemy(ModeAllegianceSeatFacts owner, ModeAllegianceSeatFacts target) => inner.PublicTeamEnemy(owner, target);
    public IReadOnlyList<Type> CommandTypes => inner.CommandTypes;
    public CommandResult Submit(GameCommand command) => inner.Submit(command);
}
