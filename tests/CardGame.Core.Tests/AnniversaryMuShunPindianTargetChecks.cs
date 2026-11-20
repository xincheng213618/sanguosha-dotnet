using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryMuShunPindianTargetChecks
{
    private const string Hero = "anniversary:mu-shun", Jinjian = "anniversary:jinjian";
    private const string Driver = "fixture:jinjian-damage", Peer = "fixture:jinjian-peer", Deck = "fixture:jinjian-deck";

    internal static void ActualDamageSourceOpensOriginalPindianCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "anniversary-mu-shun", Hero, 4, 0, 0, 0, 0, [Driver],
            [new(CardKind.Slash, Suit.Heart, 13), new(CardKind.Dodge, Suit.Diamond, 3)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        var ownerCard = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Slash);
        var damageSourceCard = game.CreateSnapshot(1).Players[1].Hand.Single();
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Driver });
        Require(game.State is { CurrentSeat: 1, Phase: TurnPhase.Play } &&
            P(game)!.Choices.Single().Targets.SequenceEqual([1]),
            "The original foreign Play-phase window fixes the damage source to its actual turn owner.");
        Answer(game, choice => choice.Targets.SequenceEqual([1]));

        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jinjian });
        var issue = Facts<MuShunJinjianIssuedEvent>().Single();
        var promptId = P(game)!.PromptId;
        Require(Facts<DamageAppliedEvent>().Single() is { SourceSeat: 1, TargetSeat: 0, Amount: 2, SourceLess: false } &&
            issue is { DamageSourceSeat: 1, MarkerBefore: 0, MarkerAfter: 1 } &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == issue.FrameId).MuShunJinjian is
                { Stage: MuShunContestStage.Choice, DamageSourceSeat: 1 } &&
            P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("mode") == "contest"),
            "Actual damage grants one Jin and permits Pindian against the original source holding a real card.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(P(game)!.PromptId == promptId && Facts<MuShunJinjianIssuedEvent>().Single() == issue,
            "Cold restore retains the original Jinjian choice and its damage source.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("mode") == "contest");
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        Require(pair is { SourceSeat: 0, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard } &&
            pair.ParentFrameId == issue.FrameId && P(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([ownerCard.Id])),
            "The restored choice opens the original native Pindian with Mu Shun's exact source card available.");
        Answer(game, choice => choice.Cards.SequenceEqual([ownerCard.Id]));

        for (var step = 0; step < 60 && Facts<RecoveryAppliedEvent>().Length == 0; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        var result = Facts<PindianResultDeterminedEvent>().Single();
        var finished = game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).ToArray();
        Require(result.FrameId == pair.Id && result.Result is { SourceRank: 13, OpponentRank: 2, SourceWon: true } &&
            result.Result.SourceCardId == ownerCard.Id && result.Result.OpponentCardId == damageSourceCard.Id &&
            finished.Select(move => move.CardId).SequenceEqual([ownerCard.Id, damageSourceCard.Id]) &&
            finished.All(move => move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) &&
            Facts<MuShunContestClosedEvent>().Single() == new MuShunContestClosedEvent(issue.FrameId, pair.Id, true, true) &&
            Facts<RecoveryAppliedEvent>().Single() is { TargetSeat: 0, Amount: 1 } && game.CreateSnapshot(0).Players[0].Hp == 3,
            "The original pair resolves both physical cards once and returns a win to Jinjian's actual recovery.");

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-jinjian-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"triggers":[
                    {"id":"first-foreign-damage","window":"playPhaseStarting","subject":"owner","turnOwnerScope":"otherLiving",
                      "optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,"effects":[
                        {"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},
                        {"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "回合伤害", description = "当前回合角色造成两点伤害。" } }
                }));
            builder.AddSkill(new(Driver, "回合伤害", "当前回合角色造成两点伤害。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "固定牌堆", 0, 0, [new("standard:slash", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "劲坚原伤害来源", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
