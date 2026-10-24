using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryGuanXingZhangBaoMobileChecks
{
    private const string Hero = "boundary:guan-xing-zhang-bao-mobile", Skill = "boundary:fuhun-mobile",
        Wusheng = "classic:wusheng", Paoxiao = "classic:paoxiao", Peer = "fixture:mobile-gxb-peer",
        Quiet = "fixture:mobile-gxb-quiet", Driver = "fixture:mobile-gxb-driver", Hold = "fixture:mobile-gxb-damage";
    private const string Conversion = "two-hand-cards-as-slash";

    public static void ConvertedDamagePaidColdGrantsWorkingSkillsUntilActualTurnEnd()
    {
        var s = Scene.Start(dodge: false); s.Play();
        var slash = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Native(s.Actions().Single(a => a.CardId == slash && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([3])));
        s.Pause(); s.Need(s.V(3).Hp == 7 && !s.HasAnyParent() && s.Grants().Length == 0,
            "A real ordinary Slash deals damage without acquiring the mobile parent's skills.");
        s.Continue(); s.Play();
        var pair = new[] { s.Card(CardKind.Dodge, Suit.Heart), s.Card(CardKind.Dodge, Suit.Spade) };
        s.Convert(pair, 3); s.Pause();
        var mother = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Action?.ConversionChain.Any(c => c.SkillId == Skill) == true);
        s.Need(mother.SelectedCardIds.SequenceEqual(pair) && use.Action is { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash } &&
            use.Action.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(pair.Order()) && pair.All(id => s.Zone(id) == CardLocation.Processing) &&
            s.V(3).Hp == 6 && !s.HasAnyParent() && s.Grants().Length == 0,
            "The original two-hand activation owns its native ordinary Slash and paid physical cards; its actual damage child pauses before the parent's skill grant.");
        s.Cold(); s.Continue(); s.Play();
        var grant = s.Grants().Single();
        s.Need(s.HasParents() && grant.GrantedSkillIds.SequenceEqual([Wusheng, Paoxiao]) && grant.TurnExpiry == new SkillGrantTurnExpiry(s.G.State.TurnNumber, 0) &&
            pair.All(id => s.Zone(id) == CardLocation.DiscardPile && s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1),
            "Cold returns through the original paid damage child once and issues the two native skills with the exact actual-turn expiry.");
        s.Drive("discard-weapon", []); s.Play();
        var red = s.Card(CardKind.Peach, Suit.Diamond);
        s.Need(s.V().Equipment.Count == 0, "The actual Crossbow was discarded before testing Paoxiao's further Slash allowance.");
        var wusheng = s.Actions().Single(a => a.CardId == red && a.Kind == LegalActionKind.Slash && a.ConversionSource?.SkillId == Wusheng && a.TargetSeats.SequenceEqual([3]));
        s.Native(wusheng); s.Pause(); s.Continue(); s.Play();
        s.Need(s.V(3).Hp == 5 && s.Grants().Length == 1 && s.Facts<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Use && e.Action.ConversionChain.Any(c => c.SkillId == Wusheng)),
            "The granted Wusheng converts a real red card into a third Slash with no Crossbow; Paoxiao permits it, and that other conversion does not re-trigger Fuhun.");
        s.End(); s.Reach(() => s.G.State.CurrentSeat != 0);
        s.Need(!s.HasAnyParent() && s.Grants().Length == 1,
            "Both issued parent skills expire at the actual end of this turn, rather than carrying into the next player's turn.");
    }

    public static void OppositeColorDodgeAndNativePlayedPairDoNotGrantSkills()
    {
        var s = Scene.Start(dodge: true); s.Play();
        var dodge = s.V(3).Hand.Single().Id;
        var redPair = new[] { s.Card(CardKind.Dodge, Suit.Heart), s.Card(CardKind.Peach, Suit.Diamond) };
        s.Convert(redPair, 3); s.Play();
        s.Need(s.V(3).Hp == 8 && s.Zone(dodge) == CardLocation.DiscardPile && s.Facts<CardRespondedEvent>().Any(e => e.ResponderSeat == 3 && e.CardId == dodge && e.EffectiveCardKind == CardKind.Dodge) &&
            redPair.All(id => s.Zone(id) == CardLocation.DiscardPile) && !s.HasAnyParent() && s.Grants().Length == 0,
            "A real black Dodge can answer the red mobile Fuhun Slash; this version has no OL same-color restriction and a fully dodged Slash grants nothing.");
        var played = new[] { s.Card(CardKind.Dodge, Suit.Spade), s.Card(CardKind.Dismantlement, Suit.Club) };
        s.Drive("duel", [3, 0]); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        s.Answer(c => c.Cards.Order().SequenceEqual(played.Order()) &&
            (c.Parameters.GetValueOrDefault("conversion-skill-id") ?? c.Parameters.GetValueOrDefault("conversion-skill")) == Skill);
        s.Pause(); s.Need(s.V(3).Hp == 7 && s.Facts<DuelResponseEvent>().Any(e => e.ResponderSeat == 0 && e.UsedSlash) &&
            s.Facts<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Response && e.Action.ActorSeat == 0 && e.Action.EffectiveKind == CardKind.Slash && e.Action.ConversionChain.Any(c => c.SkillId == Skill)) &&
            !s.HasAnyParent() && s.Grants().Length == 0,
            "Two genuine hand cards can be played as Slash in the original Duel; winning that Duel deals damage but is not a Fuhun Slash Use and grants nothing.");
        s.Continue(); s.Play(); s.Need(!s.HasAnyParent() && s.Grants().Length == 0 && played.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1),
            "The native response finishes with one payment of each physical card and no delayed false parent-skill acquisition.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramTurnSkillsGrantedEvent[] Grants() => Facts<ProgramTurnSkillsGrantedEvent>().Where(e => e.SkillId == Skill).ToArray();
        public bool HasParents() => new[] { Wusheng, Paoxiao }.All(id => V().Skills!.Any(k => k.ContentId == id));
        public bool HasAnyParent() => V().Skills!.Any(k => k.ContentId is Wusheng or Paoxiao);
        public IReadOnlyList<LegalAction> Actions() => G.GetHumanLegalActions();
        public int Card(CardKind kind, Suit suit) => V().Hand.Single(c => c.Kind == kind && c.Suit == suit).Id;
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Pause() => Reach(() => P?.SkillPrompt?.SkillId == Hold);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Convert(IReadOnlyList<int> cards, int target) => Accept(new UseProgramSkillCommand(0, Skill, Conversion, cards, [target], G.Revision, P!.PromptId));
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, BoundaryGuanXingZhangBaoMobileChecks.Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Native(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 150; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human prompt interrupted the native mobile Fuhun scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native mobile Fuhun scene exceeded its fixed boundary steps.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the original paid conversion activation, native Slash and after-damage grant parent return."); G = restored;
        }
        private void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Fuhun command."); }
        public void Need(bool value, string why)
        {
            if (!value) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool dodge)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-guan-xing-zhang-bao", Hero, 8, 4, 0, 0, 0, [Quiet, Driver, Hold],
                [new(CardKind.Slash, Suit.Club, 6), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Peach, Suit.Diamond, 8), new(CardKind.Dodge, Suit.Spade, 9), new(CardKind.Dismantlement, Suit.Club, 10)],
                [new(CardKind.Crossbow, Suit.Club, 5)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = dodge ? [new(CardKind.Dodge, Suit.Club, 7)] : [] }],
                [new(Peer, Hp: 8) { StartingHand = [] }], new string('c', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-guan-xing-zhang-bao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:guan-yu", "classic:zhang-fei");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"discard-weapon","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]}]},
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]}
              ]},
              {"id":"{{Hold}}","revision":1,"triggers":[{"id":"damage-child","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"isSkillInvocation":false,"priority":1000,"effects":[{"op":"chooseOption","target":"owner","resultBind":"damage-return","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>
            {
                [Quiet] = new { name = "安静摸牌", description = "省去本例无关的普通摸牌。" },
                [Driver] = new { name = "原生路径", description = "真实弃武器与原生决斗。" },
                [Hold] = new { name = "伤害子窗", description = "保留真实父魂杀伤害与授技之间的原生窗口。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "父魂安静同桌", "", Quiet, "wei", 8));
            const string deck = "fixture:mobile-gxb-deck";
            b.AddDeck(new(deck, "固定闪库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界关兴张苞真实转换", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
