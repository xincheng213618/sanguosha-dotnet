using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSanshouArmorChecks
{
    private const string Armor = StandardShanheSanshouArmorPackage.SkillId, Printed = "happy:sanshou";
    private const string Owner = "fixture:sanshou-armor-owner", Enemy = "fixture:sanshou-armor-enemy";
    private const string Driver = "fixture:sanshou-armor-driver", Quiet = "fixture:sanshou-armor-quiet", Deck = "fixture:sanshou-armor-deck";
    private const string EnemySeal = "fixture:sanshou-armor-enemy-seal";

    public static void PhysicalArmorGrantPreventsOnlyWhileEffectiveAndLeavesPrintedSanshouAlone()
    {
        var scene = Scene.Start();
        Need(scene.Registry.Cards[StandardShanheSanshouArmorPackage.CardId].LegacyKind == CardKind.ShanheSanshou &&
            scene.Registry.Cards[StandardShanheSanshouArmorPackage.SecondSourceCardId].LegacyKind == CardKind.ShanheSanshou &&
            EquipmentCatalog.Get(CardKind.ShanheSanshou).Slot == EquipmentSlot.Armor &&
            EquipmentCatalog.GetSkillBinding(CardKind.ShanheSanshou)?.SkillId == Armor,
            "Both source20776/20777 physical faces retain explicit content identities and share one real Armor kind and native skill grant.");
        var armorId = scene.View.Hand.Single(card => card.Kind == CardKind.ShanheSanshou).Id;
        Need(!scene.Has(Armor), "An extra fixed hand card grants no armor skill before a real equip use.");
        scene.Equip(armorId); scene.Play();
        Need(scene.View.Equipment.Single(card => card.Kind == CardKind.ShanheSanshou).Id == armorId && scene.Has(Armor) &&
            scene.Game.CardMovements.Count(move => move.CardId == armorId && move.To == CardLocation.Equipment(0)) == 1,
            "The original Heart Q hand entity really enters the armor slot once and receives its equipment grant.");
        var hp = scene.View.Hp;
        scene.Use("hit", [1]); scene.Offer(Armor); scene.Activate(); scene.Play();
        var prevented = scene.Events<AnniversarySanshouResolvedEvent>().Single();
        var shown = scene.Events<ProgramCardsRevealedEvent>().Where(fact => fact.SkillId == Armor).SelectMany(fact => fact.Cards).ToArray();
        Need(prevented.Source.SkillId == Armor && prevented.Source.SkillInstanceId == "equipment:shanhe-sanshou:" + armorId &&
            prevented.Prevented && scene.View.Hp == hp && shown.Length == 3 && shown.Select(card => card.Id).Distinct().Count() == 3 &&
            shown.All(card => scene.Game.CardMovements.Count(move => move.CardId == card.Id &&
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1),
            "The physical armor reuses Sanshou's whole-turn type snapshot, reveals three real cards, prevents damage and cleans each revealed entity once.");

        // The native restriction belongs to another skill owner. On the real
        // enemy turn, its once-only preparation selects player0, disables the
        // armor and then damages that same selected recipient before expiry.
        scene.End(); scene.Play();
        var restrictions = scene.Events<CardTargetRestrictionGrantedEvent>().Where(fact =>
            fact.Restriction.TargetSeat == 0 && fact.Restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn).ToArray();
        Need(restrictions is [var seal] && seal.Restriction.Source.SkillId == EnemySeal && seal.Restriction.Source.OwnerSeat == 1 &&
            scene.Has(Armor) && scene.View.Equipment.Any(card => card.Id == armorId),
            "The native armor-ineffective restriction keeps the physical card and its grant, while removing only the equipment effect's eligibility.");
        Need(scene.View.Hp == hp - 1 && scene.Events<AnniversarySanshouResolvedEvent>().Length == 1 &&
            scene.Events<ProgramCardsRevealedEvent>().Count(fact => fact.SkillId == Armor) == 1,
            "An ineffective armor offers no Sanshou reveal and takes the original actual damage.");
        scene.Use("unequip"); scene.Play();
        Need(!scene.Has(Armor) && scene.View.Equipment.Count == 0 && scene.Game.CardMovements.Count(move =>
                move.CardId == armorId && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile) == 1,
            "Leaving the equipment slot removes exactly that physical armor grant through the existing native equipment boundary.");
        scene.End(); scene.Play();
        var hpAfterExit = scene.View.Hp;
        scene.Use("hit", [1]); scene.Play();
        Need(scene.View.Hp == hpAfterExit - 1 && scene.Events<AnniversarySanshouResolvedEvent>().Length == 1 && !scene.Has(Armor),
            "After the real turn boundary expires the armor restriction, the departed armor still grants no residual prevention skill.");

        var printed = Scene.Start(printed: true);
        printed.Equip(printed.View.Hand.Single(card => card.Kind == CardKind.ShanheSanshou).Id); printed.Play();
        var printedHp = printed.View.Hp;
        printed.End(); printed.Offer(Printed); printed.Activate(); printed.Play();
        Need(printed.View.Hp == printedHp && printed.Has(Armor) && printed.Has(Printed) &&
            printed.Events<CardTargetRestrictionGrantedEvent>().Any(fact => fact.Restriction.Source.SkillId == EnemySeal &&
                fact.Restriction.Source.OwnerSeat == 1 && fact.Restriction.TargetSeat == 0 &&
                fact.Restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn) &&
            printed.Events<AnniversarySanshouResolvedEvent>() is [{ Source.SkillId: Printed, Prevented: true }] &&
            printed.Events<ProgramCardsRevealedEvent>().All(fact => fact.SkillId != Armor),
            "The same disabled physical armor cannot suppress the holder's separately printed original Sanshou; only its precise equipment grant is filtered.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine Game { get; } = game;
        public ContentRegistry Registry { get; } = registry;
        public PlayerSnapshot View => Game.CreateSnapshot(0).Players[0];
        private PendingDecision? P => Enumerable.Range(0, 2).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public bool Has(string skill) => View.Skills?.Any(item => item.ContentId == skill) == true;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Sanshou armor command."); }
        public void Use(string activation, int[]? targets = null) => Submit(new UseProgramSkillCommand(0, Driver,
            activation, [], targets ?? [], Game.Revision, P!.PromptId));
        public void Equip(int cardId)
        {
            var action = Game.GetHumanLegalActions().Single(item => item.CardId == cardId && item.Kind == LegalActionKind.Equip);
            Submit(new PlayCardCommand(0, cardId, action.TargetSeats, Game.Revision, P!.PromptId, action.PlayedCardKind));
        }
        public void End() => Submit(new EndPlayPhaseCommand(0, Game.Revision, P!.PromptId));
        public void Offer(string skill) => Until(() => P?.Kind == DecisionKind.ProgramTrigger && P.SkillPrompt?.SkillId == skill);
        public void Activate()
        {
            var prompt = P!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId,
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate").Id, Game.Revision));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 140; step++)
            {
                if (done()) return;
                Need(P is null || P.PlayerSeat != 0,
                    "Unexpected native Sanshou armor boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The native Sanshou armor flow did not return to its actual turn boundary.");
        }
        public static Scene Start(bool printed = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSanshouArmorPackage(), new Fixture(printed));
            var setup = new ShanheBattleSetup(71, 1, "sanshou-native", Owner, 8, 0, 0, 0, 0, [],
                [new(CardKind.ShanheSanshou, Suit.Heart, 12)], [], [new(Enemy) { StartingHand = [] }], [], new string('c', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var scene = new Scene(game, registry); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }
    private sealed class Fixture(bool printed) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sanshou-armor", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (printed)
            {
                string Read(string part)
                {
                    using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream(
                        "CardGame.Content.Standard.SkillPrograms.happy-shen-zhang-jiao." + part + ".json")
                        ?? throw new InvalidOperationException("Missing native happy Sanshou bundle.");
                    using var reader = new StreamReader(stream); return reader.ReadToEnd();
                }
                var native = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
                builder.AddSkill(new(Printed, native.Presentations[Printed].Name, native.Presentations[Printed].Description)
                    { Program = native.Programs[Printed], ProgramPresentation = native.Presentations[Printed] });
            }
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                      "effects":[{"op":"damage","target":"owner","amount":1,"sourceRef":{"kind":"selectedTarget"}}]},
                    {"id":"unequip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]}]}]},
                  {"id":"{{{EnemySeal}}}","revision":1,"triggers":[{"id":"native-foreign-armor-seal","window":"turnStartBeforeNormalFlow","subject":"owner",
                    "optional":false,"usageScope":"game","usageLimit":1,"effects":[
                      {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                      {"op":"grantTurnCardTargetRestriction","target":"selectedTarget","targetRestriction":"armorIneffectiveForTurn"},
                      {"op":"damage","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, EnemySeal }.ToDictionary(id => id, _ => new { name = "三首实体检查", description = "实际伤害、禁甲和装备移出" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "真实三首持有者", "supporter", Driver, "shu", 8)
                { AdditionalSkillIds = printed ? [Printed] : [] });
            builder.AddGeneral(new(Enemy, "真实伤害来源", "supporter", EnemySeal, "wei", 8) { AdditionalSkillIds = [Quiet] });
            builder.AddDeck(new(Deck, "未使用基本类型", 0, 0, [new("standard:dodge", 30)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "三首两人阵", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
