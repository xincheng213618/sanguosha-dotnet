using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSanshouFieldEquipmentChecks
{
    private const string Armor = StandardShanheSanshouArmorPackage.SkillId;
    private const string Owner = "fixture:sanshou-armor-owner", Enemy = "fixture:sanshou-armor-enemy";
    private const string Driver = "fixture:sanshou-armor-driver", Quiet = "fixture:sanshou-armor-quiet";
    private const string EnemySeal = "fixture:sanshou-armor-enemy-seal", Deck = "fixture:sanshou-armor-deck";

    internal static void OriginalEquippedGrantPreventsOnlyWhileArmorIsEffectiveCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSanshouArmorPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "sanshou-native", Owner, 8, 0, 0, 0, 0, [],
            [new(CardKind.ShanheSanshou, Suit.Heart, 12)], [], [new(Enemy) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, registry);

        PlayerSnapshot Holder() => game.CreateSnapshot(0).Players[0];
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        IEnumerable<T> Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>();
        bool HasGrant() => Holder().Skills?.Any(skill => skill.ContentId == Armor) == true;
        bool Play() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
        void Need(bool success, string message)
        {
            if (!success)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.CurrentSeat, game.State.TurnNumber, game.State.Phase, Prompt = Prompt(),
                    Recent = game.Events.TakeLast(8).Select(item => item.Payload.GetType().Name)
                }));
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Native Sanshou command rejected.");
        }
        void Reach(Func<bool> done)
        {
            for (var step = 0; step < 140; step++)
            {
                if (done()) return;
                Need(Prompt() is null, "An unexpected human prompt interrupted the native Sanshou sequence.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(false, "The fixed native Sanshou scene did not reach its expected boundary.");
        }

        Submit(new StartGameCommand());
        Reach(Play);
        var armor = Holder().Hand.Single(card => card.Kind == CardKind.ShanheSanshou);
        Need(armor.Suit == Suit.Heart && armor.Rank == 12 && !HasGrant(),
            "The original Heart Q Sanshou in hand does not yet grant the equipment skill.");
        var equip = game.GetHumanLegalActions().Single(action => action.CardId == armor.Id && action.Kind == LegalActionKind.Equip);
        Submit(new PlayCardCommand(0, armor.Id, equip.TargetSeats, game.Revision, Prompt()!.PromptId, equip.PlayedCardKind));
        Reach(Play);
        Need(Holder().Equipment.Single().Id == armor.Id && HasGrant() &&
            game.CardMovements.Count(move => move.CardId == armor.Id && move.From == CardLocation.Processing &&
                move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1,
            "The original physical card enters its armor slot through a native equipment use and grants Sanshou.");

        var hp = Holder().Hp;
        Submit(new UseProgramSkillCommand(0, Driver, "hit", [], [1], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.ProgramTrigger, SkillPrompt.SkillId: Armor });
        var damage = game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single();
        var source = damage.Candidates[damage.CandidateIndex].Candidate;
        var expectedInstance = "equipment:shanhe-sanshou:" + armor.Id;
        Need(damage is { SourceSeat: 1, TargetSeat: 0, Amount: 1, Prevented: false } &&
            source.SkillId == Armor && source.SkillInstanceId == expectedInstance && Holder().Equipment.Single().Id == armor.Id,
            "The original live damage window offers the grant tied to this exact equipped entity.");

        var snapshot = SnapshotJson.Serialize(game.CreateSnapshot(0));
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var resumed = game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single();
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == snapshot && resumed.Id == damage.Id &&
            resumed.Candidates[resumed.CandidateIndex].Candidate.SkillInstanceId == expectedInstance,
            "Cold restore retains the original damage prompt and exact armor grant.");
        var prompt = Prompt()!;
        var activate = prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Submit(new AnswerPromptCommand(0, prompt.PromptId, activate.Id, game.Revision));
        Reach(Play);
        var reveal = Facts<ProgramCardsRevealedEvent>().Single(fact => fact.SkillId == Armor).Cards;
        Need(Facts<AnniversarySanshouResolvedEvent>().Single() is { Prevented: true } resolved &&
            resolved.Source is { SkillId: Armor, OwnerSeat: 0 } && resolved.Source.SkillInstanceId == expectedInstance &&
            Facts<ProgramDamagePreventedEvent>().Single(fact => fact.SkillId == Armor).FrameId == damage.Id &&
            Holder().Hp == hp && !Facts<DamageAppliedEvent>().Any() && reveal.Count == 3 &&
            reveal.All(card => game.CardMovements.Count(move => move.CardId == card.Id &&
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1),
            "The active original grant prevents its real pending damage and cleans up each revealed card once.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(Play);
        var seal = Facts<CardTargetRestrictionGrantedEvent>().Single(fact => fact.Restriction.Source.SkillId == EnemySeal).Restriction;
        Need(seal is { TargetSeat: 0, Source.OwnerSeat: 1, Restriction: SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn } &&
            Holder().Equipment.Single().Id == armor.Id && HasGrant() && Holder().Hp == hp - 1 &&
            Facts<DamageAppliedEvent>().Single() is { SourceSeat: 1, TargetSeat: 0, Amount: 1 } &&
            Facts<AnniversarySanshouResolvedEvent>().Count() == 1 &&
            Facts<ProgramCardsRevealedEvent>().Count(fact => fact.SkillId == Armor) == 1,
            "The enemy's actual armor seal leaves the same card and grant equipped, and the new real damage applies without a Sanshou offer.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sanshou-field-equipment", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":1,"sourceRef":{"kind":"selectedTarget"}}]}]},
                  {"id":"{{{EnemySeal}}}","revision":1,"triggers":[{"id":"native-foreign-armor-seal","window":"turnStartBeforeNormalFlow","subject":"owner",
                    "optional":false,"usageScope":"game","usageLimit":1,"effects":[
                      {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                      {"op":"grantTurnCardTargetRestriction","target":"selectedTarget","targetRestriction":"armorIneffectiveForTurn"},
                      {"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, EnemySeal }.ToDictionary(id => id, _ => new { name = "三首实体检查", description = "实际伤害与禁甲" })
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "真实三首持有者", "supporter", Driver, "shu", 8));
            builder.AddGeneral(new(Enemy, "真实伤害来源", "supporter", EnemySeal, "wei", 8) { AdditionalSkillIds = [Quiet] });
            builder.AddDeck(new(Deck, "未使用基本类型", 0, 0, [new("standard:dodge", 30)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "三首两人阵", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
