using System.Text.Json;
using System.IO;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class WushuangOfficialSkillsAChecks
{
    private const string Driver = "fixture:wushuang-a-driver", Quiet = "fixture:wushuang-a-quiet", Hero = "fixture:wushuang-a-hero";

    public static void JinzhiVirtualResponseCannotBeTakenAsFunanDiscardMaterialCold()
    {
        var s = Scene.Start([3252], activeEnemy: true, enemyFunan: true);
        s.End(); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        var sourceSeat = s.P!.SourceSeat ?? throw new InvalidOperationException("The real incoming Slash must publish its source seat.");
        s.Need(s.V(sourceSeat).GeneralId == "fixture:wushuang-a-enemy" &&
            s.V(sourceSeat).Skills!.Any(skill => skill.ContentId == "classic:funan"), "The actual Slash source owns production Funan before its opponent's virtual response.");
        s.Cold();
        s.Answer(choice => choice.Parameters.GetValueOrDefault("response") == "extended-view-as" &&
            choice.Parameters.GetValueOrDefault("conversion-skill-id") == "wushuang:skill-3252" && choice.Cards.Count == 1);
        s.Reach(() => s.E<WushuangJinzhiPaidEvent>().Any()); s.Cold(); s.Play();
        var paid = s.E<WushuangJinzhiPaidEvent>().Single();
        s.Need(paid is { Ordinal: 1, Action.Type: CardActionType.Response } && paid.Action.OpponentSeat == sourceSeat &&
            s.E<WushuangOriginalSkillDrawEvent>().Count(fact => fact.FrameId == paid.FrameId && fact.Requested == 1 && fact.Actual == 1) == 1 &&
            !s.E<ProgramResponseEntityClaimedEvent>().Any(fact => fact.ClaimedActionId == paid.Action.ActionId) &&
            paid.Action.PhysicalCards.All(cost => s.G.CardMovements.Count(move => move.CardId == cost.CardId &&
                move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill-program.wushuang.jinzhi.discard") == 1),
            "Funan cannot take the unrelated physical discard cost of a zero-entity Jinzhi Dodge before its actual once-paid same-color invoice; the original response still pays, draws and returns once.");
        s.Cold();
    }

    internal sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public ContentRegistry Registry => registry;
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public string? Stage => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.WushuangOriginalSkill?.Stage;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Need(bool okay, string why) { if (!okay) throw new InvalidOperationException(why + Diagnostic()); }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.Status, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
            Prompt = P, Stage, Hand = V().Hand, Paid = E<WushuangJinzhiPaidEvent>().Select(e => new { e.Ordinal, e.RoundNumber, e.Action.Type }), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())) });
        public void Accept(GameCommand c) { try { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected command"); } catch (Exception e) { throw new InvalidOperationException(e.Message + Diagnostic(), e); } }
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var selected = p.Choices.FirstOrDefault(match); Need(selected is not null, "The expected published choice is absent."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, selected!.Id, G.Revision)); }
        public void ActivateTrigger() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Activate(int id, string activation) => Accept(new UseProgramSkillCommand(0, "wushuang:skill-" + id, activation, [], [], G.Revision, P!.PromptId));
        public void Targeted(int id, string activation, int target) => Accept(new UseProgramSkillCommand(0, "wushuang:skill-" + id, activation, [], [target], G.Revision, P!.PromptId));
        public void Qingnang() => Accept(new UseProgramSkillCommand(0, "wushuang:skill-3049", "discard-and-heal", [V().Hand.First().Id], [0], G.Revision, P!.PromptId));
        public void Use(string id, int target) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [target], G.Revision, P!.PromptId));
        public void UseCard(CardKind kind, int target)
        { var action = G.GetHumanLegalActions().First(a => a.CardId is { } card && V().Hand.Single(c => c.Id == card).Kind == kind && a.TargetSeats.SequenceEqual([target])); Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind) { ConversionSource = action.ConversionSource }); }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Jinzhi(CardKind kind)
        {
            var action = G.GetHumanLegalActions().First(a => a.ProgramSkillId == "wushuang:skill-3252" && a.PlayedCardKind == kind);
            Accept(new UseProgramSkillCommand(0, action.ProgramSkillId!, action.ProgramActivationId!, action.SelectableCardIds,
                kind == CardKind.Peach ? [0] : [], G.Revision, P!.PromptId));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 240; i++)
            {
                if (ready()) return;
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Kind == DecisionKind.PlayCard) { End(); continue; }
                    var skip = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" || c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    if (skip is not null) { Answer(c => c.Id == skip.Id); continue; }
                    throw new InvalidOperationException("Unexpected actual human prompt." + Diagnostic());
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded official skill flow did not arrive." + Diagnostic());
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold replay retains the exact owning skill, physical payment and current private prompt.");
        }
        public static Scene Start(int[] skills, bool activeEnemy = false, bool recoveryLoss = false, bool responseBan = false,
            bool tauntSlash = false, bool equipmentOnly = false, bool extraPlay = false, bool enemyFunan = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skills, activeEnemy, recoveryLoss, responseBan, extraPlay, enemyFunan));
            var hand = new[] { new ShanheStartingCard(CardKind.Slash, Suit.Heart, 7), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Duel, Suit.Club, 6), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Heart, 7) };
            var enemy = activeEnemy ? new[] { new ShanheStartingCard(CardKind.DrawTwo, Suit.Heart, 3), new(CardKind.DrawTwo, Suit.Club, 4), new(CardKind.Slash, Suit.Diamond, 7) }
                : new[] { new ShanheStartingCard(CardKind.Peach, Suit.Heart, 3), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Duel, Suit.Club, 6), new(CardKind.BaguaFormation, Suit.Heart, 2) };
            var setup = new ShanheBattleSetup(7, 1, "wushuang-a", Hero, 8, 0, 0, 0, 0, [], equipmentOnly ? [] : hand,
                equipmentOnly ? [new ShanheStartingCard(CardKind.QinggangSword, Suit.Club, 2)] : [],
                [new("fixture:wushuang-a-enemy", Hp: 12) { StartingHand = enemy }, new("fixture:wushuang-a-other", Hp: 12) { StartingHand = tauntSlash ? [new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7)] : [] }],
                [new("fixture:wushuang-a-ally", Hp: 12) { StartingHand = [] }], new string('a', 64));
            var g = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(int[] skills, bool activeEnemy, bool recoveryLoss, bool responseBan, bool extraPlay, bool enemyFunan) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-a", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var assembly = typeof(StandardContentPackage).Assembly;
            SkillProgramCatalog Bundle(string name)
            {
                using var rules = new StreamReader(assembly.GetManifestResourceStream($"CardGame.Content.Standard.SkillPrograms.{name}.rules.json")!);
                using var presentation = new StreamReader(assembly.GetManifestResourceStream($"CardGame.Content.Standard.SkillPrograms.{name}.presentation.json")!);
                return SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd());
            }
            var official = Bundle("wushuang-skills-a"); var audit = Bundle("wushuang-audit-a");
            if (enemyFunan)
            {
                var exchanged = Bundle("classic-xue-zong");
                const string id = "classic:funan";
                b.AddSkill(new(id, exchanged.Presentations[id].Name, exchanged.Presentations[id].Description)
                    { Program = exchanged.Programs[id], ProgramPresentation = exchanged.Presentations[id] });
            }
            foreach (var number in skills)
            { var id = "wushuang:skill-" + number; var cat = official.Programs.ContainsKey(id) ? official : audit; b.AddSkill(new(id, cat.Presentations[id].Name, cat.Presentations[id].Description) { Program = cat.Programs[id], ProgramPresentation = cat.Presentations[id], Tags = number is 3246 or 3265 or 3106 ? SkillTag.Locked : SkillTag.None }); }
            if (skills.Contains(3140))
            {
                // An ungranted consumer opts into the shared native invoice so
                // this regression verifies a real logical equipment discard.
                const string invoiceConsumer = "wushuang:skill-3252";
                b.AddSkill(new(invoiceConsumer, official.Presentations[invoiceConsumer].Name, official.Presentations[invoiceConsumer].Description)
                { Program = official.Programs[invoiceConsumer], ProgramPresentation = official.Presentations[invoiceConsumer] });
            }
            foreach (var (bundle, id) in new[] { ("boundary-cao-cao", "boundary:jianxiong"), ("classic-cao-pi", "classic:xingshang"), ("classic-lifecycle-skills", "classic:dangxian") })
            { var cat = Bundle(bundle); b.AddSkill(new(id, cat.Presentations[id].Name, cat.Presentations[id].Description) { Program = cat.Programs[id], ProgramPresentation = cat.Presentations[id] }); }
            var json = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
             {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"fixture-range","query":"slashDistanceLimit","operation":"unlimited","value":0,"priority":0}],"activations":[{"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]},
             {"id":"fixture:wushuang-a-range","revision":1,"modifiers":[{"id":"range","query":"slashDistanceLimit","operation":"unlimited","value":0,"priority":0}]},
             {"id":"fixture:wushuang-a-shield","revision":1,"cardPolicies":[{"id":"shield","kind":"prohibitTarget","cardKinds":["slash","fireSlash","thunderSlash"]}]},
             {"id":"fixture:wushuang-a-recovery-loss","revision":1,"triggers":[{"id":"lose-after-recovery","window":"afterHpRecovered","subject":"owner","hpChangeOccurrence":"perEvent","optional":false,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
             {"id":"fixture:wushuang-a-response-ban","revision":1,"triggers":[{"id":"ban-target-hand","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"eventTarget"},{"op":"grantTurnHandCardProhibition","target":"selectedTarget"}]}]},
             {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
            ]}
            """;
            var own = SkillProgramCatalog.Load(json, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, "fixture:wushuang-a-range", "fixture:wushuang-a-shield", "fixture:wushuang-a-recovery-loss", "fixture:wushuang-a-response-ban" }.ToDictionary(id => id, id => new { name = id, description = "固定原生单骑无双规则夹具" }) }));
            foreach (var (id, p) in own.Programs) b.AddSkill(new(id, id, id) { Program = p, ProgramPresentation = own.Presentations[id] });
            b.AddGeneral(new(Hero, Hero, "", Driver, "shu", 12, skills.Select(id => "wushuang:skill-" + id)
                .Concat(extraPlay ? ["classic:dangxian"] : Array.Empty<string>())
                .Concat(recoveryLoss ? ["fixture:wushuang-a-recovery-loss"] : Array.Empty<string>()).ToArray()));
            foreach (var id in new[] { "fixture:wushuang-a-enemy", "fixture:wushuang-a-other", "fixture:wushuang-a-ally" })
                b.AddGeneral(new(id, id, "", activeEnemy && id.EndsWith("enemy", StringComparison.Ordinal) ? "fixture:wushuang-a-range" : Quiet, "wei", 12,
                    id.EndsWith("enemy", StringComparison.Ordinal) ? responseBan ? ["fixture:wushuang-a-response-ban"] : enemyFunan ? ["classic:funan"] : [] : ["fixture:wushuang-a-shield"]));
            b.AddDeck(new("fixture:wushuang-a-deck", "固定红桃闪补牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "单骑无双官方技能规则小局", 4, 4, new Dictionary<string, int>(), "fixture:wushuang-a-deck",
                GeneralPoolIds: [Hero, "fixture:wushuang-a-enemy", "fixture:wushuang-a-other", "fixture:wushuang-a-ally"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}

