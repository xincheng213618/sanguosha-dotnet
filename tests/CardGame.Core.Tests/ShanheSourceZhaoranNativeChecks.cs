using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceZhaoranNativeChecks
{
    private const string Skill = StandardShanheSourceYouyanWarriorPackage.ZhaoranSkillId;
    private const string Driver = "fixture:source-zhaoran-driver", Quiet = "fixture:source-zhaoran-quiet", Spare = "fixture:source-zhaoran-spare";
    private const string Owner = "fixture:source-zhaoran-owner", Peer = "fixture:source-zhaoran-peer", Deck = "fixture:source-zhaoran-deck";

    public static void ActualPlayPublicHandSuitQuotaDrawDiscardAndPhaseReset()
    {
        var s = Scene.Start();
        Need(s.V(0).HandCount == 5 && !s.OthersSeeHand(), "Hand stays private before the real optional Play-entry activation.");
        s.Arm();
        Need(s.OthersSeeHand() && s.E<ShanheSourceZhaoranArmedEvent>().Length == 1,
            "Accepting source7014 exposes the owner's complete current Hand to every other viewer for this actual Play.");
        s.Slash(Suit.Club, 8); s.Play();
        Need(s.Claims == 0, "Losing one of two Club cards does not exhaust that Hand suit.");
        s.Slash(Suit.Club, 10); s.Benefit();
        var first = s.E<ShanheSourceZhaoranSuitClaimedEvent>().Single();
        Need(first.Suit == Suit.Club && s.V(0).Hand.All(c => c.Suit != Suit.Club),
            "The actual last Club payment creates one frozen suit claim before its native Slash finishes.");
        var hand = s.V(0).HandCount;
        s.Option("draw"); s.Play();
        var gained = s.V(0).Hand.Single(c => c.Suit == Suit.Club);
        Need(s.V(0).HandCount == hand + 1 && s.OthersSeeHand() &&
            s.G.CardMovements.Any(m => m.CardId == gained.Id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)),
            "The draw benefit obtains a real new entity and that newly acquired Hand card is public immediately.");
        s.Slash(Suit.Club, gained.Rank); s.Play();
        Need(s.Claims == 1, "Regaining and losing Club again cannot reopen this actual Play's spent Club quota.");

        var peerHand = s.V(1).HandCount;
        s.Slash(Suit.Diamond, 9); s.Benefit(); s.Option("discard");
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("target-kind") == nameof(SkillProgramTargetKind.OtherLivingWithDiscardableHandOrEquipment)) == true);
        s.Answer(c => c.Targets.SequenceEqual([1]));
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        Need(s.P!.Choices.All(c => c.Cards.Count == 0), "Discarding the other owner's unrevealed Hand retains real opaque slot commands.");
        s.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)); s.Play();
        Need(s.Claims == 2 && s.V(1).HandCount == peerHand - 1 && s.G.CardMovements.Any(m =>
            m.From == CardLocation.Hand(1) && m.To == CardLocation.DiscardPile),
            "The other benefit actually discards one eligible foreign Hand entity and returns to the original paid Slash.");

        var pair = s.V(0).Hand.Where(c => c.Suit is Suit.Spade or Suit.Heart).Select(c => c.Id).ToArray();
        Need(pair.Length == 2, "The original Heart and Spade remain available for one real atomic discard.");
        s.Use("discard-pair", pair); s.Benefit(); s.Option("draw"); s.Benefit(); s.Option("draw"); s.Play();
        var pairLosses = s.E<ShanheSourceZhaoranSuitLostEvent>().Where(e => e.Suit is Suit.Spade or Suit.Heart).ToArray();
        Need(s.Claims == 4 && pairLosses.Length == 2 && pairLosses[0].BatchId == pairLosses[1].BatchId &&
            s.E<ShanheSourceZhaoranSuitClaimedEvent>().Select(e => e.Suit).Distinct().Count() == 4 && s.OthersSeeHand(),
            "One real batch freezes both exhausted suits before nested draw children; each suit receives one independent benefit.");

        var arm = s.E<ShanheSourceZhaoranArmedEvent>().Single();
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.G.State.CurrentSeat != 0 || s.G.State.Phase != TurnPhase.Play);
        Need(!s.OthersSeeHand(), "Leaving the actual Play immediately closes public Hand access.");
        s.Entry();
        Need(!s.OthersSeeHand(), "The next real Play starts private until its own optional activation.");
        s.Arm();
        Need(s.E<ShanheSourceZhaoranArmedEvent>().Last().PlayInstance != arm.PlayInstance, "A fresh real Play gets a new disclosure and suit quota identity.");
        var clubs = s.V(0).Hand.Where(c => c.Suit == Suit.Club).ToArray();
        Need(clubs.Length == 2, "Both prior real draw children supplied one Club each.");
        s.Slash(Suit.Club, clubs[0].Rank); s.Play();
        s.Slash(Suit.Club, clubs[1].Rank); s.Benefit(); s.Option("draw"); s.Play();
        Need(s.Claims == 5 && s.E<ShanheSourceZhaoranSuitClaimedEvent>().Count(e => e.Suit == Suit.Club) == 2,
            "The new actual Play can earn Club again while retaining the previous phase's already-consumed facts.");
        s.Use("lose-zhaoran"); s.Play();
        Need(!s.V(0).Skills!.Any(skill => skill.ContentId == Skill) && !s.OthersSeeHand() &&
            s.G.State is { CurrentSeat: 0, Phase: TurnPhase.Play },
            "A real shared-layer skill replacement closes disclosure immediately inside the same Play, without waiting for phase exit.");
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 3).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Claims => E<ShanheSourceZhaoranSuitClaimedEvent>().Length;
        public bool OthersSeeHand() => Enumerable.Range(1, 2).All(viewer => G.CreateSnapshot(viewer).Players[0].Hand.Select(c => c.Id).Order()
            .SequenceEqual(V(0).Hand.Select(c => c.Id).Order()));
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected native Zhaoran command."); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; var c = p.Choices.FirstOrDefault(choose); Need(c is not null, "Missing native Zhaoran choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Entry() => Until(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Arm() { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); Play(); }
        public void Benefit() => Until(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "draw"));
        public void Option(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void Use(string id, int[]? cards = null) => Accept(new UseProgramSkillCommand(0, Driver, id, cards ?? [], [], G.Revision, P!.PromptId));
        public void Slash(Suit suit, int rank)
        {
            var card = V(0).Hand.First(c => c.Kind == CardKind.Slash && c.Suit == suit && c.Rank == rank);
            var a = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, card.Id, [1], G.Revision, P!.PromptId, a.PlayedCardKind));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var i = 0; i < 160; i++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "The Zhaoran check stays in a live short battle. " + Diagnostic());
                if (P is { PlayerSeat: 0 } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(c is not null, "Unexpected native Zhaoran boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Zhaoran exceeded its bounded native flow. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
            Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null,
            Stack = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name }),
            Recent = G.Events.TakeLast(6).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(91, 1, "zhaoran-native", Owner, 20, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Club, 8), new(CardKind.Slash, Suit.Club, 10), new(CardKind.Slash, Suit.Diamond, 9), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Snatch, Suit.Spade, 3)],
                [], [new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Spade, 5), new(CardKind.Slash, Suit.Heart, 6)] },
                     new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Spade, 5)] }], [], new string('e', 64));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry));
            s.Accept(new StartGameCommand()); s.Entry(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-zhaoran", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            string Read(string part)
            { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-source-zhaoran-7014." + part + ".json")!; using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
            var source = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            b.AddSkill(new(Skill, source.Presentations[Skill].Name, source.Presentations[Skill].Description)
                { Program = source.Programs[Skill], ProgramPresentation = source.Presentations[Skill] });
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"native-slashes","query":"slashLimit","operation":"unlimited","value":0,"priority":0}],"activations":[
                    {"id":"discard-pair","minCards":2,"maxCards":2,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"sourceZones":["hand"],"effects":[{"op":"discardSelected","target":"owner","amount":2}]},
                    {"id":"lose-zhaoran","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{{Skill}}}"],"sourceBind":"{{{Spare}}}"}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"wait","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{{Spare}}}","revision":1,"modifiers":[{"id":"spare","query":"handLimit","operation":"add","value":1,"priority":0}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet, Spare }.ToDictionary(id => id, id => new { name = id, description = "昭然真实移牌与阶段夹具" }) });
            var fixture = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in fixture.Programs) b.AddSkill(new(id, fixture.Presentations[id].Name, fixture.Presentations[id].Description)
                { Program = program, ProgramPresentation = fixture.Presentations[id] });
            b.AddGeneral(new(Owner, "昭然原生持有者", "supporter", Driver, "shu", 20, [Skill]));
            b.AddGeneral(new(Peer, "静守目标", "supporter", Quiet, "wei", 30));
            b.AddDeck(new(Deck, "零自然起手摸牌的梅花实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 11)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "昭然原生三人阵", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
