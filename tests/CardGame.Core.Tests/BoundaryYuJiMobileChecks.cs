using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryYuJiMobileChecks
{
    private const string Hero = "boundary:yu-ji-mobile", Guhuo = "boundary:guhuo-mobile", Chanyuan = "boundary:chanyuan-mobile",
        Friend = "fixture:boundary-yu-ji-friend", Peer = "fixture:boundary-yu-ji-peer", Quiet = "fixture:boundary-yu-ji-quiet",
        Skip = "fixture:boundary-yu-ji-skip", Driver = "fixture:boundary-yu-ji-driver", Probe = "fixture:boundary-yu-ji-probe";

    public static void FailedUseAndPlayedResponseShareActualTurnQuota()
    {
        var s = Scene.Start(probe: true); s.Play(); var firstTurn = s.G.State.TurnNumber;
        var falseCost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
        s.Declare(falseCost, CardKind.DrawTwo); s.Challenge(); var original = s.Declaration;
        s.Need(original.Payment.Cost.CardId == falseCost && original.Payment.Cost.From == CardLocation.Hand(0) &&
            original.Return.Purpose == CardDeclarationPurpose.Use && s.Usage().Length == 1 && s.Usage()[0].Scope == SkillUsageScope.Turn,
            "The real Use declaration consumes its actual-turn opportunity when its one Hand entity is paid, before anyone answers.");
        s.Cold(); s.Answer(c => Answer(c) == "challenge"); s.Play();
        s.Need(s.Facts<CardDeclarationRevealedEvent>() is [{ Succeeded: false }] && s.Zone(falseCost) == CardLocation.DiscardPile &&
            !s.Facts<CardUseDeclaredEvent>().Any(e => e.CardId == falseCost) && s.G.GetHumanLegalActions().All(a => a.ConversionSource?.SkillId != Guhuo),
            "A challenged false ordinary-trick declaration is discarded without native Use and does not refund the shared opportunity.");
        s.Drive("duel", [1, 0]); s.Response(DecisionKind.RespondSlash);
        s.Need(s.G.State.TurnNumber == firstTurn && s.P!.Choices.All(c => Conversion(c) != Guhuo),
            "The same actual turn's real virtual-Duel Played-Slash prompt cannot spend Guhuo again after a failed Use.");
        s.Answer(c => c.Cards.Count == 0); s.Play(); s.End(); s.Response(DecisionKind.RespondDodge);
        s.Need(s.G.State.TurnNumber > firstTurn && s.G.State.CurrentSeat == 1 && s.V().Hp == 7,
            "The next real ally turn attacks the uniquely wounded public teammate through its native Slash trigger.");
        var choice = s.P!.Choices.First(c => Conversion(c) == Guhuo && c.Parameters.GetValueOrDefault("response-card-kind") == nameof(CardKind.Dodge));
        var secondCost = choice.Cards.Single(); s.Answer(c => c.Id == choice.Id); s.Challenge();
        s.Need(s.Declaration.Return.Purpose == CardDeclarationPurpose.Dodge && s.Declaration.Payment.Cost.CardId == secondCost && s.Usage().Length == 2,
            "A different role's actual turn refreshes the same Guhuo skill for a genuine Dodge response and original typed return.");
        s.Cold(); s.Answer(c => Answer(c) == "challenge"); s.Play();
        s.Need(s.V().Hp == 7 && s.Facts<CardRespondedEvent>().Any(e => e.ResponderSeat == 0 && e.CardId == secondCost && e.EffectiveCardKind == CardKind.Dodge) &&
            s.G.CardMovements.Count(m => m.CardId == secondCost && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay") == 1 &&
            s.Facts<CardDeclarationRevealedEvent>().Count(e => e.Succeeded) == 1,
            "The cold true Dodge returns once to the actual incoming Slash, prevents damage and claims the paid entity without paying again.");
    }

    public static void FirstTrueChallengeGrantsDynamicChanyuanAndSkipsLaterChallenges()
    {
        var s = Scene.Start(probe: false); s.Play(); var trueCost = s.V().Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
        s.Declare(trueCost, CardKind.DrawTwo); s.Challenge(); s.Need(s.P!.PlayerSeat == 1, "The first other seat receives the original native question.");
        s.Answer(c => Answer(c) == "challenge"); s.Play();
        s.Need(s.V(1).Skills!.Any(skill => skill.ContentId == Chanyuan) && s.Facts<CardDeclarationRevealedEvent>() is [{ ChallengerSeat: 1, Succeeded: true }] &&
            s.G.GetAttackRange(1) == 3, "The first true challenger gains this edition's exact Chanyuan, while its other native capability still works above one HP.");
        s.Drive("lose-seven", [1]); s.Play();
        s.Need(s.V(1).Hp == 1 && s.G.GetAttackRange(1) == 1 && s.V(1).Skills!.Any(skill => skill.ContentId == Chanyuan),
            "At exactly one HP Chanyuan remains and dynamically suppresses the other skill's real attack-range modifier.");
        s.Cold(); s.Drive("recover", [1]); s.Play();
        s.Need(s.V(1).Hp == 2 && s.G.GetAttackRange(1) == 3, "Native recovery away from one HP restores the existing skill without granting a replacement.");
        s.End(); s.Play(); var falseCost = s.V().Hand.First(c => c.Kind == CardKind.Dodge).Id;
        s.Declare(falseCost, CardKind.DrawTwo); s.Challenge();
        s.Need(s.P!.PlayerSeat == 2 && !s.G.ResolutionStack.OfType<CardDeclarationChallengeFrame>().Single().EligibleSeats.Contains(1),
            "The acquired Chanyuan forbids the former challenger even after HP recovery; the native sequence proceeds to seat2.");
        s.Cold(); s.PassAll(); s.Play();
        s.Need(s.Facts<CardDeclarationRevealedEvent>().Length == 1 && s.Facts<CardUseDeclaredEvent>().Any(e => e.CardId == falseCost && e.CardKind == CardKind.DrawTwo) &&
            s.Usage().Length == 2 && s.Zone(falseCost) == CardLocation.DiscardPile,
            "When all eligible seats pass, even a printed Dodge proceeds as the declared ordinary trick without another reveal or grant, then finishes normally.");
    }

    private static string? Answer(PromptChoice c) => c.Parameters.GetValueOrDefault("declaration-answer");
    private static string? Conversion(PromptChoice c) => c.Parameters.GetValueOrDefault("conversion-skill-id") ?? c.Parameters.GetValueOrDefault("conversion-skill");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public CardDeclarationFrame Declaration => G.ResolutionStack.OfType<CardDeclarationFrame>().Single();
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public SkillUsageConsumedEvent[] Usage() => Facts<SkillUsageConsumedEvent>().Where(e => e.SkillOwnerSeat == 0 && e.SkillId == Guhuo && e.UsageId == "declaration").ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Challenge() => Reach(() => P?.Choices.Any(c => BoundaryYuJiMobileChecks.Answer(c) is not null) == true);
        public void Response(DecisionKind kind) => Reach(() => P is { PlayerSeat: 0 } p && p.Kind == kind && p.Choices.All(c => BoundaryYuJiMobileChecks.Answer(c) is null));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Declare(int id, CardKind kind)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId == id && a.PlayedCardKind == kind && a.ConversionSource?.SkillId == Guhuo);
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, BoundaryYuJiMobileChecks.Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void PassAll()
        {
            for (var i = 0; i < 4 && P?.Choices.Any(c => BoundaryYuJiMobileChecks.Answer(c) is not null) == true; i++)
                Answer(c => BoundaryYuJiMobileChecks.Answer(c) == "pass");
        }
        private void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 160; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "An unexpected human prompt interrupted the native Guhuo boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The native Guhuo boundary did not settle.");
        }
        public void Cold()
        {
            var checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold replay preserves the actual original declaration, paid entity, shared turn usage and challenge cursor."); G = restored;
        }
        private void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Guhuo command."); }
        public void Need(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool probe)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(probe));
            var setup = new ShanheBattleSetup(83, 1, "boundary-yu-ji-mobile", Hero, 8, 5, 0, 0, 0, [Quiet, Driver],
                [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Diamond, 4), new(CardKind.DrawTwo, Suit.Club, 7), new(CardKind.Slash, Suit.Spade, 8)], [],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Friend, Hp: 8) { StartingHand = [] }], new string('e', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var scene = new Scene(g, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture(bool probe) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-yu-ji-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"add","value":2,"priority":0}],"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Skip}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Probe}}","revision":1,"triggers":[{"id":"native-incoming-slash","window":"turnStartBeforeNormalFlow","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"publicTeamWoundedAllies"},{"op":"useVirtualSlash","target":"selectedTarget"}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]},
                {"id":"lose-seven","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":7}]},
                {"id":"recover","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"selectedTarget","amount":1}]}
              ]}
            ]}
            """;
            var ids = new[] { Quiet, Skip, Probe, Driver };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = ids.ToDictionary(id => id, id => new { name = id, description = "合法实际回合与原生声明检查。" }) }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Friend, "界于吉同伴", "", Quiet, "qun", 8, probe ? [Skip, Probe] : [Skip]));
            b.AddGeneral(new(Peer, "界于吉同桌", "", Quiet, "wei", 8, [Skip]));
            const string deck = "fixture:boundary-yu-ji-deck";
            b.AddDeck(new(deck, "于吉固定闪库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "界于吉原生声明", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Friend, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
