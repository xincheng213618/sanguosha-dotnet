using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheAppliedDamageReflectionChecks
{
    private const string Reflect = "shanhe:current-pofu-chenzhou-2131";
    private const string Jiyi = StandardShanheCampaignCatalog.CurrentJiyiSkillId;
    private const string Owner = "fixture:reflection-owner", Enemy = "fixture:reflection-enemy";
    private const string Driver = "fixture:reflection-driver", EnemyDriver = "fixture:reflection-enemy-driver";
    private const string Deck = "fixture:reflection-deck";

    public static void OutOfTurnAppliedSegmentsReflectWithoutAggregatingHpLossOrSmallDamage()
    {
        var s = Scene.Start(fire: false);
        Need(s.View().Skills!.Any(skill => skill.ContentId == Reflect) && s.View().GeneralId == Owner,
            "The unrelated printed general carries the production tactic through the real acquired-tactic setup.");
        s.Use("self-three");
        Need(s.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 3 }] &&
            !s.Events<ProgramBindingResolvedEvent>().Any(item => item.SkillId == Reflect),
            "A genuine sourced three-point self-damage during the acquired owner's own turn does not qualify.");
        s.NextOwnPlay();
        Need(s.Events<DamageAppliedEvent>().Select(hit => (hit.SourceSeat, hit.TargetSeat, hit.Amount))
                .SequenceEqual([(0, 0, 3), (1, 0, 2), (1, 0, 2), (1, 0, 3), (0, 1, 3)]) &&
            s.Events<ProgramSkillHpLostEvent>() is [{ SkillId: EnemyDriver, TargetSeat: 0, Amount: 3 }] &&
            s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Reflect) is
                { OwnerSeat: 0, Window: SkillProgramTriggerWindow.AfterDamageApplied, Activated: true, Completed: true } &&
            s.View() is { Hp: 17, Armor: 0 } && s.View(1).Hp == 27 &&
            s.Game.ResolutionStack.OfType<ProgramSkillFrame>().All(frame => frame.SkillId != Reflect),
            "The actual next NPC turn keeps two separate damage segments of two and a separate lose-HP instruction distinct; only its one real three-point segment reflects once and returns to its original parent.",
            new { Hits = s.Events<DamageAppliedEvent>(), Lost = s.Events<ProgramSkillHpLostEvent>(), Bindings = s.Events<ProgramBindingResolvedEvent>(), Owner = s.View(), Enemy = s.View(1) });
        Need(s.Events<DamageRequestedEvent>().Last() is
                { SourceSeat: 0, TargetSeat: 1, Amount: 3, SourceCard: null, Nature: DamageNature.Normal, SourceLess: false } &&
            s.Events<DamageAppliedEvent>().Last().SourceCardUseResolutionId is null,
            "The reflected segment is real sourced native skill damage, with no inherited card use or manufactured HP subtraction.");
    }

    public static void ReflectionCopiesFrozenFinalNatureAndUsesFreshSkillDamageWithNativeArmor()
    {
        foreach (var silverLion in new[] { false, true })
        {
            var s = Scene.Start(fire: true, silverLion);
            s.Use("armor-two");
            if (silverLion)
            {
                var lion = s.View().Hand.Single(card => card.Kind == CardKind.SilverLion).Id;
                s.Use("equip-peer", [lion], [1]);
                Need(s.View(1).Equipment.Single().Id == lion &&
                    s.Game.CardMovements.Any(move => move.CardId == lion && move.From == CardLocation.Hand(0) && move.To == CardLocation.Equipment(1)),
                    "A real original Silver Lion is placed in the attacker's equipment area by native selected-equipment movement before damage.");
            }
            var fireCard = s.View(1).Hand.Single(card => card.Kind == CardKind.FireSlash).Id;
            s.NextOwnPlay();
            var hits = s.Events<DamageAppliedEvent>();
            var incoming = hits.Single(hit => hit.SourceSeat == 1 && hit.TargetSeat == 0);
            var reflected = hits.Single(hit => hit.SourceSeat == 0 && hit.TargetSeat == 1);
            var use = s.Events<CardUseDeclaredEvent>().Single(item => item.SourceSeat == 1 && item.CardId == fireCard);
            Need(hits.Length == 2 && incoming is { Amount: 3, Nature: DamageNature.Fire, SourceLess: false,
                    ArmorPayment: { DamageAmount: 3, ArmorBefore: 2, ArmorAbsorbed: 2, ArmorAfter: 0, HpLossAmount: 1 } } &&
                incoming.SourceCardUseResolutionId == use.ResolutionId && s.View() is { Hp: 29, Armor: 0 } &&
                use.CardKind == CardKind.FireSlash &&
                s.Events<CardUseFinishedEvent>().Count(item => item.ResolutionId == use.ResolutionId) == 1 &&
                s.Game.CardMovements.Count(move => move.CardId == fireCard && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1 &&
                s.Game.CardMovements.Count(move => move.CardId == fireCard && move.To == CardLocation.DiscardPile) == 1,
                "The NPC actually pays and finishes one physical Fire Slash; its final three-point Fire segment spends two Armor and loses only one HP.",
                new { silverLion, Hits = hits, Uses = s.Events<CardUseDeclaredEvent>(), Owner = s.View() });
            var bonus = s.Events<ProgramCardDamageModifiedEvent>().Single(item => item.Source.SkillId == Jiyi);
            Need(bonus is { TargetSeat: 1, CardKind: null, BaseAmount: 3, ModifiedAmount: 4 } &&
                reflected.Amount == (silverLion ? 1 : 4) && reflected.Nature == DamageNature.Fire &&
                reflected is { SourceLess: false, SourceCardUseResolutionId: null } &&
                !s.Events<ProgramCardDamageModifiedEvent>().Any(item => item.Source.OwnerSeat == 0 && item.Source.SkillId == Driver) &&
                s.Events<ProgramCardDamageModifiedEvent>().Single(item => item.Source.SkillId == EnemyDriver) is
                    { TargetSeat: 0, CardKind: CardKind.FireSlash, BaseAmount: 1, ModifiedAmount: 3 } &&
                s.Events<ProgramBindingResolvedEvent>().Single(item => item.SkillId == Reflect) is { Activated: true, Completed: true } &&
                s.View(1).Hp == (silverLion ? 29 : 26),
                "The reflected child copies the original finalized three and Fire, gets its own legitimate direct-skill +1 once, and never reapplies either participant's Slash-only bonus; real Silver Lion subsequently caps that fresh native damage to one.",
                new { silverLion, Hits = hits, Modifiers = s.Events<ProgramCardDamageModifiedEvent>(), Requests = s.Events<DamageRequestedEvent>() });
        }
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? Prompt => Game.PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native reflection command."); }
        private void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 240; step++)
            {
                if (condition()) return;
                Need(Game.State.Winner == Winner.None, "The bounded reflection scene ended before its expected native boundary.");
                if (Prompt is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    Need(prompt.Kind != DecisionKind.PlayCard, "Unexpected ordinary Play before the native reflection child completed.");
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(item => item.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded native reflection boundary: " + JsonSerializer.Serialize(Prompt));
        }
        private void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation, int[]? cards = null, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, cards ?? [], targets ?? [], Game.Revision, Prompt!.PromptId)); Play(); }
        public void NextOwnPlay()
        {
            var turn = Game.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Prompt!.PromptId));
            Reach(() => Game.State.TurnNumber > turn && Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(bool fire, bool silverLion = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheAppliedDamageReflectionPackage(),
                new StandardShanheDirectSkillDamagePackage(), new Fixture(fire));
            var setup = new ShanheBattleSetup(83, 1, "fixture:reflection", Owner, 30, 0, 0, 0, 0, [],
                silverLion ? [new(CardKind.SilverLion)] : [], [],
                [new(Enemy) { StartingHand = fire ? [new(CardKind.FireSlash, Suit.Heart, 4)] : [] }], [], new string('e', 64))
                { TacticSkillIds = fire ? [Reflect, Jiyi] : [Reflect] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool fire) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-applied-damage-reflection", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var lion = CardCatalog.Get(CardKind.SilverLion);
            builder.AddCard(new("fixture:reflection-silver-lion", lion.DisplayName, lion.CategoryName, lion.Description, CardKind.SilverLion));
            var enemyTriggers = fire ? "" : """
                ,"triggers":[
                  {"id":"separate-real-segments","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[
                    {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                    {"op":"damage","target":"selectedTarget","amount":2},
                    {"op":"damage","target":"selectedTarget","amount":2},
                    {"op":"loseHp","target":"selectedTarget","amount":3},
                    {"op":"damage","target":"selectedTarget","amount":3}]},
                  {"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]
                """;
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"damageModifiers":[
                    {"id":"owner-slash-only","cardKinds":["slash","fireSlash","thunderSlash"],"amount":4,"condition":"always","sourceScope":"ownerUsed"}],
                   "activations":[
                    {"id":"self-three","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":3}]},
                    {"id":"armor-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantArmor","target":"owner","amount":2}]},
                    {"id":"equip-peer","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"captureSelectedCards","target":"owner","resultBind":"lion"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"lion"}]}]},
                  {"id":"{{{EnemyDriver}}}","revision":1,"damageModifiers":[
                    {"id":"actual-fire-slash-three","cardKinds":["fireSlash"],"amount":2,"condition":"always","sourceScope":"ownerUsed"}]{{{enemyTriggers}}}}
                ]}
                """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, EnemyDriver }.ToDictionary(id => id, _ => new { name = "真实伤害段与装备驱动", description = "仅使用原生伤害、护甲和实际实体火杀。" })
            }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "独立反伤战法持有者", "supporter", Driver, "shu", 30));
            builder.AddGeneral(new(Enemy, "原生实体火杀来源", "supporter", EnemyDriver, "wei", 30));
            builder.AddDeck(new(Deck, "零自然摸牌的固定实体牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "真实单段伤害反射", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
