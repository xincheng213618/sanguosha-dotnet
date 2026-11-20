using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

using static BoundaryLiDianChecks;

internal static class ZhangJiChecks
{
    private const string Hero = "ol:zhang-ji", Lueming = "ol:lueming", Falu = "ol:falu", Zhenyi = "ol:zhenyi";
    private const string Peer = "fixture:lueming-peer", Watch = "fixture:lueming-damage-watch", Deck = "fixture:lueming-judgment-deck";

    internal static void ZhenyiEffectiveRankReturnsToOriginalLuemingCold()
    {
        const int owner = 0, target = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(7, 1, "ol-lueming-zhenyi-final-rank", Hero, 4, 0, 0, 0, 0, [Falu, Zhenyi],
            [], [new(CardKind.Crossbow, Suit.Club, 1)], [peer, peer], [peer], new string('b', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanSeats = [owner, target], HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Accept(game, new StartGameCommand());
        Settle(game);
        var hp = Player(target).Hp;
        Require(Player(owner).GeneralId == Hero && Player(owner).Equipment.Single().Kind == CardKind.Crossbow && Player(target).Equipment.Count == 0,
            "The printed OL Zhang Ji has one actual equipped weapon and can challenge the other player's empty equipment area.");
        Accept(game, new UseProgramSkillCommand(owner, Lueming, "choose-rank", [], [target], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt.PlayerSeat == target && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("rank") == "5"));
        var root = Root();
        Require(P(game) is { IsPrivate: true, PlayerSeat: target } && P(game)!.Choices.Count == 13 && root.SelectedTargetSeats.SequenceEqual([target]),
            "The original other participant receives the private selection of all thirteen ranks.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("rank") == "5");
        Reach(game, prompt => prompt is { PlayerSeat: owner, Kind: DecisionKind.ProgramJudgmentTrigger });

        var chosen = Facts<ZhangJiRankChosenEvent>().Single();
        var judgment = game.ResolutionStack.OfType<JudgmentFrame>().Single();
        var beforeResult = game.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single();
        var printed = Player(owner).Judgment.Single(card => card.Id == judgment.CardId);
        Require(chosen == new ZhangJiRankChosenEvent(root.Id, target, 5, judgment.Id) &&
            judgment.ParentFrameId == root.Id && judgment.TargetSeat == owner && judgment.Reason == "lueming" &&
            beforeResult.ZhenyiBeforeResult == true && beforeResult.ParentFrameId == judgment.Id &&
            beforeResult.Judgment is { Rank: 7, Suit: Suit.Club } && beforeResult.Judgment.CardId == printed.Id &&
            printed is { Rank: 7, Suit: Suit.Club } && !Facts<JudgmentResolvedEvent>().Any(fact => fact.ResolutionId == judgment.Id),
            "The original Lueming judgment revealed a real printed Club 7 and has reached the paid override opportunity before any final result.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-activate");
        Reach(game, prompt => prompt.PlayerSeat == owner && prompt.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "zhenyi-judgment"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("suit") == nameof(Suit.Heart));
        Reach(game, prompt => prompt is { PlayerSeat: target, SkillPrompt.SkillId: Watch });

        var overrideReceipt = Facts<ZhenyiJudgmentOverriddenEvent>().Single().Receipt;
        var final = Facts<JudgmentResolvedEvent>().Single(fact => fact.ResolutionId == judgment.Id);
        var returned = Facts<ZhangJiJudgmentReturnedEvent>().Single();
        var harm = game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single();
        var pending = P(game)!.PromptId;
        Require(overrideReceipt is { Rank: 5, Suit: Suit.Heart } && overrideReceipt.Source.SkillId == Zhenyi &&
            overrideReceipt.JudgmentId == judgment.Id && overrideReceipt.CardId == printed.Id &&
            final is { TargetSeat: owner, Reason: "lueming", Rank: 5, Suit: Suit.Heart, Succeeded: true } &&
            final.ParentResolutionId == root.Id && final.CardId == printed.Id &&
            returned == new ZhangJiJudgmentReturnedEvent(root.Id, judgment.Id, printed.Id, 5) &&
            Root().Id == root.Id && Root().ZhangJi is { Stage: "damage" } &&
            harm is { SourceSeat: owner, TargetSeat: target, Amount: 2 } && harm.ParentFrameId == root.Id &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == printed.Id).Location == CardLocation.Processing && Player(target).Hp == hp,
            "The same printed entity returns as effective Heart 5 and issues two damage on the original parent, while damage and physical cleanup are still pending.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(P(game)?.PromptId == pending && Root().Id == root.Id && game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single().Id == harm.Id &&
            Facts<JudgmentResolvedEvent>().Where(fact => fact.ResolutionId == judgment.Id).SequenceEqual([final]) &&
            Facts<ZhangJiJudgmentReturnedEvent>().SequenceEqual([returned]) && Player(target).Hp == hp,
            "Cold replay retains the same original returned judgment and live damage observer.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Settle(game);

        var movement = game.CardMovements.Where(move => move.CardId == printed.Id && move.Reason != CardMoveReasons.InitialDeal).ToArray();
        Require(Player(target).Hp == hp - 2 && Facts<DamageAppliedEvent>() is [{ SourceSeat: owner, TargetSeat: target, Amount: 2 }] &&
            !Facts<ZhangJiCardGainedEvent>().Any() && Facts<ZhangJiJudgmentReturnedEvent>().SequenceEqual([returned]) &&
            Facts<ProgramSkillResolvedEvent>().Count(fact => fact.FrameId == root.Id && fact.Completed) == 1 &&
            movement is [var reveal, var handoff, var cleanup] && reveal.Reason == CardMoveReasons.JudgmentReveal &&
            reveal.From == CardLocation.DrawPile && reveal.To == CardLocation.Judgment(owner) && handoff.From == reveal.To &&
            handoff.To == CardLocation.Processing && handoff.Reason.Value == "skill-program.judgment.result" &&
            cleanup.From == handoff.To && cleanup.To == CardLocation.DiscardPile && cleanup.Reason.Value == "skill-program.ol:lueming.choose-rank.complete-cleanup",
            "The original target takes exactly two damage and the same physical judgment card is cleaned from Processing once.");

        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        ProgramSkillFrame Root() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Lueming);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-lueming-final-rank", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "ol:zhang-qi-ying");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"lueming-issued-damage","window":"beforeDamageApplied","subject":"damageTarget","optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"掠命伤害观察","description":"在实际伤害结算前暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "", Watch, "wei", 6));
            builder.AddDeck(new(Deck, "掠命固定印刷梅花七", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7), 32).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "掠命真实改判", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
