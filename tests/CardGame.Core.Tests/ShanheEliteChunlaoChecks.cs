using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheEliteChunlaoChecks
{
    private const string Skill = "shanhe:elite-chunlao", Owner = "fixture:elite-chunlao-owner";
    private const string Peer = "fixture:elite-chunlao-peer", Mode = ShanheBattleSetup.BattleModeId, Deck = "fixture:elite-chunlao-deck";

    public static void StoresMoreThanTwentyRealSlashesWithoutAnArtificialCap()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            new StandardShanheEliteDongwuPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "fixture:elite-chunlao", Owner, 30, 0, 0, 0, 0,
            [], [], [], [new(Peer)], [], new string('e', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = Mode, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, registry);
        void Accept(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Elite Chunlao command rejected.");
        }
        void Until(Func<bool> reached)
        {
            for (var step = 0; step < 80 && !reached(); step++) Accept(new AdvanceOneStepCommand(game.Revision));
            Need(reached(), "The native storage fixture did not reach its expected boundary.");
        }
        void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = game.PendingDecision!;
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));
        }

        Accept(new StartGameCommand());
        Until(() => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var material = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        Need(material.Length > 20, "The small fixed deck must supply more than twenty actual Slash entities.");
        Accept(new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId));
        Until(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill);
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        foreach (var card in material)
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards" && choice.Cards.SequenceEqual([card]));
        if (game.PendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") == true)
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
        Until(() => game.CreateSnapshot(0).Players[0].ChunlaoCount == material.Length);
        var stored = game.CardMovements.Where(move => move.From == CardLocation.Hand(0) && move.To == CardLocation.Chunlao(0)).ToArray();
        Need(game.CreateSnapshot(0).Players[0].HandCount == 0 && stored.Length == material.Length &&
            stored.Select(move => move.CardId).ToHashSet().SetEquals(material),
            "The screenshot variant stores the entire selected batch beyond twenty, moving each original entity once into its owner's Chunlao pile.");
    }

    private static void Need(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-elite-chunlao", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "存醇者", "supporter", Skill, "wu", 30));
            builder.AddGeneral(new(Peer, "普通对手", "supporter", null, "wei", 30));
            builder.AddDeck(new(Deck, "真实杀实体", 22, 0, [])
            { PhysicalCards = Enumerable.Range(0, 70).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "无固定存醇上限", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralCandidateCount: 2, GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
