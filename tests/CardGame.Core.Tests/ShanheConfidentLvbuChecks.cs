using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheConfidentLvbuChecks
{
    private const string Xiaorui = StandardShanheConfidentLvbuPackage.XiaoruiId;
    private const string Driver = "fixture:xiaorui-driver", Foreign = "fixture:xiaorui-foreign";
    private const string Owner = "fixture:xiaorui-owner", Peer = "fixture:xiaorui-peer";

    public static void ActualAlliedSlashHitsGrantOneQuotaToActorAndExcludeOtherTurnsCold()
    {
        foreach (var holderSeat in new[] { 0, 1, 2 })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
                new StandardShanheConfidentLvbuPackage(), new Fixture());
            var setup = new ShanheBattleSetup(91, 1, "fixture:xiaorui", Owner, 20, 0, 0, 0, 0,
                holderSeat == 0 ? [Xiaorui, Foreign] : [Foreign],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8), new(CardKind.Slash, Suit.Heart, 9)], [],
                [new(Peer, ExtraSkillIds: holderSeat == 2 ? [Xiaorui] : []), new(Peer)],
                [new(Peer, ExtraSkillIds: holderSeat == 1 ? [Xiaorui] : [])], new string('a', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            Submit(game, new StartGameCommand()); Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Submit(game, new UseProgramSkillCommand(0, Driver, "hurt", [], [2], game.Revision, Prompt(game)!.PromptId));
            Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            Need(Grants(game).Length == 0, "A skill-damage event does not grant Slash quota merely because its source is an allied turn player.");

            var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([2]));
            Submit(game, new PlayCardCommand(0, slash.CardId!.Value, [2], game.Revision, Prompt(game)!.PromptId, slash.PlayedCardKind));
            if (holderSeat != 2)
            {
                Reach(game, () => Grants(game).Length == 1);
                game = Cold(game, registry);
            }
            Reach(game, () => Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            var grants = Grants(game);
            var slashDamage = Events<DamageAppliedEvent>(game).Last();
            Need(slashDamage is { SourceSeat: 0, TargetSeat: 2, Amount: 2 } &&
                grants.Length == (holderSeat == 2 ? 0 : 1) &&
                grants.All(grant => grant.Modifier is { AffectedSeat: 0, Amount: 1, Query: SkillRuleQuery.SlashLimit } &&
                    grant.Modifier.Source.OwnerSeat == holderSeat) &&
                game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash) == (holderSeat != 2),
                "One real two-point Slash hit grants one additional use to its actual actor from self/allied Xiaorui; an enemy holder grants none.");
            game = Cold(game, registry);

            var quotaCount = Grants(game).Length;
            Submit(game, new EndPlayPhaseCommand(0, game.Revision, Prompt(game)!.PromptId));
            Reach(game, () => Prompt(game) is { PlayerSeat: 0 } prompt && prompt.SkillPrompt?.SkillId == Foreign);
            var foreignPrompt = Prompt(game)!;
            game = Cold(game, registry);
            var selected = Prompt(game)!.Choices.First(choice => choice.Targets.SequenceEqual([2]));
            Submit(game, new AnswerPromptCommand(0, foreignPrompt.PromptId, selected.Id, game.Revision));
            Reach(game, () => Events<CardUseFinishedEvent>(game).Count() >= 2);
            Need(game.CreateSnapshot(0).CurrentSeat == 1 && Grants(game).Length == quotaCount &&
                Events<DamageAppliedEvent>(game).Last() is { SourceSeat: 0, TargetSeat: 2 },
                "A genuine allied Slash used during another actor's actual turn cannot gain Xiaorui quota.");
            _ = Cold(game, registry);
        }
    }

    private static TurnRuleModifierGrantedEvent[] Grants(GameEngine game) => Events<TurnRuleModifierGrantedEvent>(game)
        .Where(grant => grant.Modifier.Source.SkillId == Xiaorui).ToArray();
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static PendingDecision? Prompt(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Xiaorui native command rejected.");
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 180; step++)
        {
            if (reached()) return;
            if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge or DecisionKind.RescueDying } response)
                Submit(game, new AnswerPromptCommand(0, response.PromptId, response.Choices.First(choice => choice.Cards.Count == 0).Id, game.Revision));
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Xiaorui native boundary missing: " + JsonSerializer.Serialize(Prompt(game)));
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(restored.Events) == JsonSerializer.Serialize(game.Events),
            "Cold command replay retains the actual source, original frame grant and pending child without duplicate Slash quota.");
        return restored;
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-xiaorui", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,
                   "modifiers":[{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0}],
                   "damageModifiers":[{"id":"two-point-slash","cardKinds":["slash"],"amount":1,"condition":"always","damageLayer":"cardDamage"}],
                   "activations":[{"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{Foreign}}","revision":1,"triggers":[
                    {"id":"once-in-foreign-turn","window":"turnStartBeforeNormalFlow","subject":"owner","turnOwnerScope":"otherLiving",
                     "usageScope":"game","usageLimit":1,"optional":false,
                     "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                       {"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":false}]}]}
                ]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "真实伤害与范围驱动", description = "普通技能伤害和实体杀伤害。" },
                        [Foreign] = new { name = "真实回合外杀", description = "在其他角色的准备阶段，使用一张真实虚拟杀。" }
                    }
                }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, "fixture") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "玩家", "supporter", Driver, "shu", 20));
            builder.AddGeneral(new(Peer, "固定同伴或敌人", "supporter", null, "wei", 20));
            builder.AddDeck(new("fixture:xiaorui-deck", "无闪的短局", 0, 0, [new("standard:peach", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "骁锐原生短局", 4, 4, new Dictionary<string, int>(), "fixture:xiaorui-deck",
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
