using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryGuoHuaiMobileChecks
{
    private const string Hero = "boundary:guo-huai-mobile", Skill = "boundary:jingce-mobile", Peer = "fixture:mobile-gh-peer",
        Quiet = "fixture:mobile-gh-quiet", Driver = "fixture:mobile-gh-driver", Gain = "fixture:mobile-gh-gain", Signal = "fixture:mobile-gh-ending", Arm = "fixture:mobile-gh-arm";
    private const string Fuhun = "classic:fuhun", Conversion = "two-hand-cards-as-slash", Reward = "skill-program.boundary:jingce-mobile.Draw";

    public static void TrueUseAndPlayedMaterialsCountOnceAndEndingDrawCold()
    {
        var s = Scene.Start(); s.Play();
        var discarded = s.Card(CardKind.Dismantlement, Suit.Spade); s.Drive("discard", [discarded]); s.Play();
        var recast = s.Card(CardKind.IronChain, Suit.Club); s.Recast(recast); s.Play();
        s.Need(s.Count() == 0 && s.Zone(discarded) == CardLocation.DiscardPile && s.Zone(recast) == CardLocation.DiscardPile,
            "Real skill discard and real Iron Chain recast enter the discard pile without becoming used-or-played entities.");
        var played = new[] { s.Card(CardKind.Dodge, Suit.Heart), s.Card(CardKind.Dodge, Suit.Spade) };
        s.Drive("duel", [], [3, 0]); s.Response(); s.Answer(c => c.Cards.Order().SequenceEqual(played.Order()) && ConversionSkill(c) == Fuhun); s.Play();
        s.Need(s.Count() == 2 && s.Entities().All(e => played.Contains(e.CardId)) && s.Entities().All(e => e.ActionId > 0) &&
            s.Facts<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Response && e.Action.PhysicalCards.Count == 2),
            "A genuine two-hand Played Slash counts two native discarded materials; the zero-material virtual Duel contributes no imaginary entity.");
        var slash = s.Card(CardKind.Slash, Suit.Club); var dodge = s.V(3).Hand.Single().Id;
        s.Native(slash, 3); s.Play();
        s.Need(s.Count() == 4 && s.Entities().Count(e => e.CardId == dodge) == 1 && s.Entities().Count(e => e.CardId == slash) == 1 &&
            s.Facts<CardRespondedEvent>().Any(e => e.ResponderSeat == 3 && e.CardId == dodge),
            "The source's real Slash and another player's real Dodge each count once, despite native-use and response-completion observers sharing their evidence.");
        var used = new[] { s.Card(CardKind.Dodge, Suit.Club), s.Card(CardKind.Dodge, Suit.Diamond) };
        s.Accept(new UseProgramSkillCommand(0, Fuhun, Conversion, used, [3], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.Count() == 6 && used.All(id => s.Entities().Count(e => e.CardId == id) == 1) &&
            s.Entities().Select(e => e.MovementSequence).Distinct().Count() == 6,
            "A converted Slash Use also contributes both actual materials, rather than one card kind or one whole action.");
        var before = s.V().Hand.Count; s.End(); s.Offer();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Gain);
        var mother = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var qualified = s.Facts<BoundaryGuoHuaiEndingQualifiedEvent>().Single();
        s.Need(qualified.FrameId == mother.Id && qualified is { Count: 6, Hp: 4, ActualTurnOwnerSeat: 0 } &&
            mother.InstructionIndex == 2 && s.V().Hand.Count == before + 2 && s.Draws() == 2 &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == mother.Id),
            "The original own-Ending frame freezes the real entity threshold and waits on its actual two-card draw child.");
        s.Cold(); s.Continue(); s.Ending(0, finishGains: true);
        s.Need(s.Draws() == 2 && s.Facts<BoundaryGuoHuaiEndingQualifiedEvent>().Length == 1 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.BindingId == "actual-turn-discard-count" && e.Completed && e.Activated) == 1,
            "Cold finishes the paid native draw once, without recomputing a second reward or replaying any response material payment.");
    }

    public static void DiscardRecastEquipmentExcludedForeignEndingAndActualTurnReset()
    {
        var s = Scene.Start(negative: true); s.Play(); var old = s.V().Equipment.Single().Id;
        var weapon = s.Card(CardKind.Crossbow, Suit.Club); s.Native(weapon); s.Play();
        var discarded = s.Card(CardKind.Dismantlement, Suit.Spade); s.Drive("discard", [discarded]); s.Play();
        var recast = s.Card(CardKind.IronChain, Suit.Club); s.Recast(recast); s.Play();
        s.Need(s.Zone(old) == CardLocation.DiscardPile && s.Zone(weapon) == CardLocation.Equipment(0) &&
            s.Zone(discarded) == CardLocation.DiscardPile && s.Zone(recast) == CardLocation.DiscardPile && s.Count() == 0,
            "Installing a new weapon, replacing the old equipment, an ordinary discard and recast create no qualifying used-or-played discard entity.");
        s.End(); s.Ending(0);
        s.Need(s.Facts<BoundaryGuoHuaiEndingQualifiedEvent>().Length == 0 && s.Draws() == 0,
            "An own Ending below the actual-entity threshold offers no Jingce reward despite several unrelated discard-pile entries.");
        s.Continue(); s.Response(skipSignals: true);
        s.Need(s.G.State.CurrentSeat == 2 && s.P!.SourceSeat == 2, "The real enemy's ordinary physical Duel reaches the human response in its own actual turn.");
        var pair = new[] { s.Card(CardKind.Dodge, Suit.Heart), s.Card(CardKind.Dodge, Suit.Spade) };
        s.Answer(c => c.Cards.Order().SequenceEqual(pair.Order()) && ConversionSkill(c) == Fuhun); s.Ending(2);
        s.Need(s.Count() == 3 && s.V().Hp == 3 && s.Facts<BoundaryGuoHuaiEndingQualifiedEvent>().Length == 0 && s.Draws() == 0,
            "The foreign actual turn contains a real Duel and two Played materials, meeting Guo Huai's HP, but his skill still cannot trigger at someone else's Ending.");
        s.Continue(); s.Play(skipSignals: true); s.End(); s.Ending(0);
        s.Need(s.Count() == 0 && s.Entities().Length == 0 && s.Facts<BoundaryGuoHuaiDiscardedEntityEvent>().Length == 3 &&
            s.Facts<BoundaryGuoHuaiEndingQualifiedEvent>().Length == 0 && s.Draws() == 0,
            "A new own actual turn uses a fresh count, while immutable previous-turn facts remain in history and cannot satisfy its Ending threshold.");
    }

    private static string? ConversionSkill(PromptChoice c) => c.Parameters.GetValueOrDefault("conversion-skill-id") ?? c.Parameters.GetValueOrDefault("conversion-skill");
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, G.State.Players.Count).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public BoundaryGuoHuaiDiscardedEntityEvent[] Entities() => Facts<BoundaryGuoHuaiDiscardedEntityEvent>().Where(e => e.TurnNumber == G.State.TurnNumber && e.ActualTurnOwnerSeat == G.State.CurrentSeat).ToArray();
        public int Count() => Entities().Length;
        public int Draws() => G.CardMovements.Count(m => m.Reason.Value == Reward && m.To == CardLocation.Hand(0));
        public int Card(CardKind kind, Suit suit) => V().Hand.First(c => c.Kind == kind && c.Suit == suit).Id;
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play(bool skipSignals = false) => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, skipSignals);
        public void Response(bool skipSignals = false) => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash }, skipSignals);
        public void Ending(int owner, bool finishGains = false) => Reach(() => P?.SkillPrompt?.SkillId == Signal && G.State.CurrentSeat == owner, finishGains: finishGains);
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Drive(string id, IReadOnlyList<int> cards, IReadOnlyList<int>? targets = null) => Accept(new UseProgramSkillCommand(0, BoundaryGuoHuaiMobileChecks.Driver, id, cards, targets ?? [], G.Revision, P!.PromptId));
        public void Recast(int id) => Accept(new RecastCardCommand(0, id, G.Revision, P!.PromptId));
        public void Native(int id, int? target = null)
        {
            var a = G.GetHumanLegalActions().First(a => a.CardId == id && (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal, bool skipSignals = false, bool finishGains = false)
        {
            for (var i = 0; i < 220; i++)
            {
                if (goal()) return;
                if (skipSignals && P?.SkillPrompt?.SkillId == Signal) { Continue(); continue; }
                if (finishGains && P?.SkillPrompt?.SkillId == Gain) { Continue(); continue; }
                Need(P?.PlayerSeat != 0, "Unexpected human prompt interrupted the native mobile Jingce scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native mobile Jingce scene exceeded its fixed boundary steps.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the scalar original-turn entity ledger, original Ending frame and actual paid draw child."); G = restored;
        }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Jingce command."); }
        public void Need(bool value, string why)
        {
            if (!value) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, Count = Count(), P, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name), Actions = Facts<CardActionAcceptedEvent>().TakeLast(4).Select(e => new { e.Action.Type, e.Action.ActorSeat, e.Action.EffectiveKind, e.Action.TargetSeats, Cards = e.Action.PhysicalCards.Count }), Used = Facts<CardUsedEvent>().TakeLast(4), OwnerHp = V().Hp }));
        }
        public static Scene Start(bool negative = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(negative));
            var cards = new List<ShanheStartingCard>
            { new(negative ? CardKind.Crossbow : CardKind.Slash, Suit.Club, 6), new(CardKind.Dodge, Suit.Heart, 7), new(CardKind.Dodge, Suit.Spade, 8),
                new(CardKind.Dodge, Suit.Club, 9), new(CardKind.Dodge, Suit.Diamond, 10), new(CardKind.IronChain, Suit.Club, 11), new(CardKind.Dismantlement, Suit.Spade, 12) };
            string[] skills = negative ? [Quiet, Driver, Arm, Gain, Signal] : [Quiet, Driver, Fuhun, Gain, Signal];
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-guo-huai", Hero, negative ? 3 : 4, 0, 0, 0, 0, skills, cards,
                [new(CardKind.Crossbow, Suit.Spade, 5)],
                [new(Peer, Hp: 8) { StartingHand = [] },
                    new(Peer, Hp: 8) { StartingHand = negative ? [new(CardKind.Duel, Suit.Spade, 1)] : [new(CardKind.Dodge, Suit.Diamond, 2)] }],
                negative ? [] : [new(Peer, Hp: 8) { StartingHand = [] }], new string('e', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = negative ? 3 : 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 7, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool negative) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-guo-huai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:guan-xing-zhang-bao", "classic:guan-yu", "classic:zhang-fei");
            var rules = $$$$"""
            {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
              {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"keep-materials","query":"handLimit","operation":"add","value":10,"priority":0,"condition":{"kind":"always"}}],"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{{{Driver}}}}","revision":1,"activations":[
                {"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand","equipment"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]}
              ]},
              {"id":"{{{{Arm}}}}","revision":1,"triggers":[{"id":"actual-duel-conversion","window":"cardUseBeforeTargetEffects","ownerRelation":"target","cardKinds":["duel"],"optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{{Fuhun}}}}"]}]}]},
              {"id":"{{{{Gain}}}}","revision":1,"triggers":[{"id":"gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{{Reward}}}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-return","options":[{"id":"continue"}]}]}]},
              {"id":"{{{{Signal}}}}","revision":1,"triggers":[{"id":"ending","window":"turnEnding","subject":"owner","turnOwnerScope":"anyLiving","priority":-1000,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"ending-return","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Driver, Arm }) names[id] = new { name = id, description = "固定真实牌与原生使用、重铸、弃置或目标确认后的原生授技。" };
            foreach (var id in new[] { Gain, Signal }) names[id] = new { name = id, description = "真实摸牌与结束阶段观察。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "精策安静同桌", "", Quiet, "wei", 8));
            const string deck = "fixture:mobile-gh-deck";
            b.AddDeck(new(deck, "固定闪库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 13)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界郭淮真实弃牌实体", negative ? 3 : 4, negative ? 3 : 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = negative ? 1 : 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
