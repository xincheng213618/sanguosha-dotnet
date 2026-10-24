using CardGame.Core;

namespace CardGame.Modes.Standard;

/// <summary>Installed by the standard content package, including its custom-mode fixtures.</summary>
public sealed class StandardModeSetupFactory : IGameModeSetupFeatureFactory
{
    public string Id => "mode:standard-setup";
    public string ModeId => ModeFeatureTargets.AllModes;
    public IGameModeFeature Create(ModeFeatureCreationContext context) => new StandardModeSetupFeature(
        context.Options ?? throw new InvalidOperationException("Standard setup requires real match options."),
        context.ModeDefinition ?? throw new InvalidOperationException("Standard setup requires the selected mode."),
        context.ContentRegistry, context.GetHost<IModeSetupRandom>());
}

internal sealed partial class StandardModeSetupFeature(
    GameOptions options, ContentModeDefinition mode, ContentRegistry registry, IModeSetupRandom random) : IMatchModeSetupFeature
{
    private readonly GameOptions _options = options;
    private readonly ContentModeDefinition _mode = mode;
    private readonly ContentRegistry _registry = registry;
    private readonly IModeSetupRandom _random = random;
    private readonly IReadOnlyList<ContentGeneralDefinition> _generalPool = Array.AsReadOnly(
        (mode.GeneralPoolIds ?? registry.Generals.Keys.Where(registry.IsGeneralPlayable)
            .OrderBy(id => id, StringComparer.Ordinal).ToArray()).Select(id => registry.Generals[id]).ToArray());
    private int PlayerCount => _options.PlayerCount;
    private bool Interactive => _options.UseInteractiveSetup;
    private int RelativeSeat(int offset) => (Math.Max(0, _options.HumanSeat) + offset) % PlayerCount;
    private int SeatOffset(int seat) => (seat - Math.Max(0, _options.HumanSeat) + PlayerCount) % PlayerCount;
    private string[] TeamIds => _mode.TeamCounts?.Keys.OrderBy(id => id, StringComparer.Ordinal).ToArray() ?? [];
    private IReadOnlyList<ContentGeneralDefinition> GeneralPool => _generalPool;
    private bool IsHuman(int seat) => _options.HumanSeats?.Contains(seat) ?? seat == _options.HumanSeat;
    private string SeatName(int seat) => IsHuman(seat) ? _options.HumanSeats is null ? "你" : $"玩家 {seat + 1}" : $"AI {seat + 1}";
    private Role TeamRole(string teamId)
    {
        var index = Array.IndexOf(TeamIds, teamId);
        return index switch
        {
            0 => Role.TeamA, 1 => Role.TeamB,
            _ => throw new InvalidOperationException($"Mode '{_mode.Id}' does not define a supported team '{teamId}'.")
        };
    }
    private bool SelectionEligible(ContentGeneralDefinition general) =>
        _mode.GeneralSelectionPoolIds?.Contains(general.Id, StringComparer.Ordinal) != false;
    private bool UsesFamilyDraft => _mode.AiGeneralSwapExcludedIds is not null ||
        _mode.GeneralSelectionWeights is not null || _mode.GeneralSelectionPoolIds is not null;
    private bool UsesGeneralBaseHp => _mode.Id.StartsWith("identity:classic-", StringComparison.Ordinal) ||
        _mode.Id == WushuangMode.ModeId || _mode.ModeKind == ContentModeKind.Survival ||
        _mode.DragonBoatActivity is not null || _mode.GuanduActivity is not null;
    private string CharacterFamily(ContentGeneralDefinition general) => general.CharacterId ?? general.Id;
    private static ModeSeatSetupFacts Facts(ModeSeatSetup seat) => new(seat.Seat, seat.IsHuman, seat.Role,
        seat.TeamId, seat.NationalFactionId, seat.GeneralId, seat.GeneralSelected, seat.SecondaryGeneralId, seat.SecondaryGeneralSelected);

    public ModeSetupPlan BuildPlayers()
    {
        if (_mode.ModeKind == ContentModeKind.Survival) return BuildSurvivalPlayers();
        if (_mode.ModeKind == ContentModeKind.Team)
        {
            if (_options.ShanheBattle is not null) return BuildCampaignPlayers();
            if (_mode.GuanduActivity is not null) return BuildGuanduPlayers();
            if (_mode.DragonBoatActivity is not null) return BuildDragonBoatPlayers();
            if (_mode.ShenwuActivity is not null) return BuildShenwuPlayers();
            if (_mode.TeamActivity is not null) return BuildTeamActivityPlayers();
            if (_mode.BossActivity is not null) return BuildBossPlayers();
            return BuildPublicTeamPlayers();
        }
        return _mode.ModeKind == ContentModeKind.NationalWarLite ? BuildNationalPlayers() : BuildIdentityPlayers();
    }
}
