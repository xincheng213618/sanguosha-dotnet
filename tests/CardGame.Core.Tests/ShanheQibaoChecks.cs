using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheQibaoChecks
{
    private const string Owner = "fixture:qibao-owner", Peer = "fixture:qibao-peer";
    private const string Driver = "fixture:qibao-driver", Quiet = "fixture:qibao-quiet", Deck = "fixture:qibao-deck";
    private const string HpLoss = "fixture:qibao-hp-loss";

    public static void ManualEquipmentAllSlashKindsMatchHealthArmorAndPublicPrediction()
    {
        foreach (var kind in new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash })
        {
            var s = Scene.Start(kind, 4, withLion: true);
            var weapon = s.V().Hand.Single(c => c.Kind == CardKind.QiBaoDao).Id;
            Need(s.V().Equipment.Count == 0 && !s.Actions(kind).Any(a => a.TargetSeats.SequenceEqual([2])),
                "A real Qi Bao Dao in hand grants neither its range-two reach nor its equipped damage ability.");
            s.Slash(kind, 1, 1);
            s.Equip();
            Need(s.V().Equipment.Single().Id == weapon && s.G.CardMovements.Count(m => m.CardId == weapon &&
                    m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Processing &&
                    m.To == CardLocation.Equipment(0)) == 1 && s.Actions(kind).Any(a => a.TargetSeats.SequenceEqual([2])),
                "Manual native equipment use pays the same weapon entity once and enables its real range-two target.");
            s.Use("equip-peer", [2], [s.V().Hand.Single(c => c.Kind == CardKind.SilverLion).Id]);
            Need(s.V(2).Equipment is [{ Kind: CardKind.SilverLion }] && s.V(2).Hp == s.V(2).MaxHp,
                "The range-two target wears an actual Silver Lion before the full-health hit.");
            s.Slash(kind, 2, 2);
            s.Slash(kind, 2, 1);
            s.Use("unequip");
            Need(s.V().Equipment.Count == 0 && s.G.CardMovements.Count(m => m.CardId == weapon &&
                    m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile) == 1,
                "Discarding the actual weapon removes its future damage ability through native equipment movement.");
            s.Slash(kind, 3, 1);
        }
    }

    public static void DirectSkillDamageAndChainRecipientsDoNotAcquireAnotherWeaponBonus()
    {
        var s = Scene.Start(CardKind.FireSlash, 1);
        s.Equip();
        s.Use("hit", [1]);
        s.Use("fire", [2]);
        s.Use("source-less");
        Need(s.E<DamageAppliedEvent>().Select(e => (e.TargetSeat, e.Amount, e.SourceLess))
                .SequenceEqual([(1, 1, false), (2, 1, false), (0, 1, true)]),
            "A real equipped Qi Bao Dao adds nothing to direct normal/fire skill damage or source-less damage.");
        s.Use("chain", [1]); s.Use("chain", [3]);
        var before = s.E<DamageAppliedEvent>().Length;
        s.Slash(CardKind.FireSlash, 1, 1);
        Need(s.E<DamageAppliedEvent>().Skip(before).Select(e => (e.TargetSeat, e.Amount, e.Nature))
                .SequenceEqual([(1, 1, DamageNature.Fire), (3, 1, DamageNature.Fire)]) &&
            s.E<ChainedDamagePropagatedEvent>().Length == 1,
            "An injured Slash origin carries one damage to the still-full-health chained recipient without reapplying Qi Bao Dao there.");

        var full = Scene.Start(CardKind.FireSlash, 1);
        full.Equip(); full.Use("chain", [1]); full.Use("chain", [2]);
        full.Slash(CardKind.FireSlash, 1, 2);
        Need(full.E<DamageAppliedEvent>().Select(e => (e.TargetSeat, e.Amount))
                .SequenceEqual([(1, 2), (2, 2)]) && full.E<ChainedDamagePropagatedEvent>().Length == 1,
            "A full-health origin receives exactly one weapon bonus, then propagates finalized two without adding a third point.");

        var loss = Scene.Start(CardKind.Slash, 1, extraSkillIds: [HpLoss]);
        loss.Equip(); loss.Slash(CardKind.Slash, 1, 1, hpLoss: true);
        Need(loss.E<DamageAppliedEvent>().Length == 0 &&
            loss.E<DamageReplacedWithHpLossEvent>() is [{ PolicyOwnerSeat: 0, TargetSeat: 1, Amount: 1 }],
            "Replacing the real full-health Slash with HP loss uses the card layer's one point, before Qi Bao Dao's damage-only bonus.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public LegalAction[] Actions(CardKind kind) => G.GetHumanLegalActions().Where(a => a.CardId is { } id &&
            V().Hand.Any(c => c.Id == id && c.Kind == kind) && a.ConversionSource is null).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Qi Bao Dao command."); }
        private void Play()
        {
            for (var step = 0; step < 240; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(G.State.Winner == Winner.None, "The small Qi Bao Dao fixture must reach the next native play boundary.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("action") == "cold-damage" ||
                        c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(choice is not null, "Unexpected Qi Bao Dao child prompt: " + prompt.Kind);
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native Qi Bao Dao flow did not return to Play.");
        }
        public void Equip()
        {
            var card = V().Hand.Single(c => c.Kind == CardKind.QiBaoDao);
            var action = Actions(CardKind.QiBaoDao).Single(a => a.Kind == LegalActionKind.Equip);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId)); Play();
        }
        public void Use(string activation, int[]? targets = null, int[]? cards = null)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
            Play();
        }
        public void Slash(CardKind kind, int target, int expected, bool hpLoss = false)
        {
            var action = Actions(kind).First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]));
            var beforeHp = V(target).Hp; var beforeRevision = G.Revision;
            var prediction = G.PredictPublicActionDamage(0, action, target);
            Need((hpLoss ? prediction.DamageAmount == 0 && prediction.DirectHpLossAmount == expected : prediction.DamageAmount == expected) &&
                prediction.HpLossAmount == expected && G.Revision == beforeRevision,
                "Public " + kind + " damage must match the real weapon/full-health predicate without issuing a use.");
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind));
            Play();
            var use = E<CardUseDeclaredEvent>().Last(e => e.CardId == action.CardId && e.SourceSeat == 0);
            var damage = E<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == use.ResolutionId && e.TargetSeat == target).ToArray();
            var nature = kind switch { CardKind.FireSlash => DamageNature.Fire, CardKind.ThunderSlash => DamageNature.Thunder,
                CardKind.ColdSlash => DamageNature.Cold, _ => DamageNature.Normal };
            var matched = hpLoss
                ? damage.Length == 0 && E<DamageReplacedWithHpLossEvent>().Count(e => e.ParentResolutionId == use.ResolutionId &&
                    e.TargetSeat == target && e.Amount == expected) == 1
                : damage is [{ SourceSeat: 0 } hit] && hit.Amount == expected && hit.Nature == nature;
            Need(matched && V(target).Hp == beforeHp - expected &&
                G.CardMovements.Count(m => m.CardId == action.CardId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
                "The paid " + kind + " must apply the predicted native damage once, including its armor bypass and actual hand payment.");
        }
        public static Scene Start(CardKind slash, int count, bool withLion = false, string[]? extraSkillIds = null)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] cards = [new(CardKind.QiBaoDao, Suit.Heart, 6),
                .. Enumerable.Range(0, count).Select(i => new ShanheStartingCard(slash, Suit.Spade, i + 5)),
                .. withLion ? new[] { new ShanheStartingCard(CardKind.SilverLion, Suit.Club, 1) } : []];
            var setup = new ShanheBattleSetup(83, 1, "fixture:qibao", Owner, 30, 0, 0, 0, 10, extraSkillIds ?? [], cards, [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(),
                [], new string('a', 64));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:qibao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var (id, kind) in new[] { ("fixture:qibao-card", CardKind.QiBaoDao), ("fixture:qibao-cold", CardKind.ColdSlash),
                ("fixture:qibao-lion", CardKind.SilverLion) })
            {
                var card = CardCatalog.Get(kind);
                builder.AddCard(new(id, card.DisplayName, card.CategoryName, card.Description, kind));
            }
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]},
                    {"id":"source-less","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"receiveOwnerDamage","target":"owner","amount":1}]},
                    {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                    {"id":"equip-peer","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"real-equipment"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"real-equipment"}]},
                    {"id":"unequip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{HpLoss}}","revision":1,"cardPolicies":[{"id":"replace-damage","kind":"damageBecomesHpLoss","cardKinds":[]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, HpLoss }.ToDictionary(id => id, _ => new { name = "真实七宝刀验证", description = "实际装备、技能伤害与连环" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "七宝刀使用者", "supporter", Driver, "qun", 30));
            builder.AddGeneral(new(Peer, "固定敌方", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "固定无闪牌库", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "七宝刀实体装备", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
