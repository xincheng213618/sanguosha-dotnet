using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceJiangdongTrioChecks
{
    private const string Lihuo = StandardShanheSourceJiangdongTrioPackage.LihuoSkillId;
    private const string Chunlao = StandardShanheSourceJiangdongTrioPackage.ChunlaoSkillId;
    private const string Actor = "fixture:jiangdong-source-actor", Peer = "fixture:jiangdong-source-peer";
    private const string WineOwner = "fixture:jiangdong-source-wine-owner", Target = "fixture:jiangdong-source-target";
    private const string Driver = "fixture:jiangdong-source-driver", Reach = "fixture:jiangdong-source-reach";
    private const string Watch = "fixture:jiangdong-source-child", Deck = "fixture:jiangdong-source-deck";

    public static void Source3160FirstDamagePaymentAnd3161DiscarderWineJudgmentDraw()
    {
        FirstDamageCostAndPermanentSingleTargetGrant();
        DiscarderChoosesAndPaidWineReplacesOneJudgmentWithRealDraw();
    }

    private static void FirstDamageCostAndPermanentSingleTargetGrant()
    {
        var s = Scene.Start(wine: false);
        Need(s.G.GetHumanLegalActions().All(a => a.ConversionSource?.SkillId != Lihuo ||
                s.V().Hand.Single(c => c.Id == a.CardId).Kind != CardKind.ThunderSlash),
            "Source3160 never converts a Thunder Slash through the non-fire conversion of source3470.");
        var slash = s.Begin(s.Action(a => a.Kind == LegalActionKind.Slash &&
            s.EffectiveKind(a) == CardKind.Slash && a.ConversionSource is null && Targets(a).SequenceEqual([1]), "ordinary Slash to seat one"));
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "extra") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "extra" && c.Targets.SequenceEqual([2]));
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "shanhe-source-lihuo-cost") == true);
        var use = s.E<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.ShanheSourceLihuoCost is not null);
        Need(s.E<ShanheSourceLihuoCostStartedEvent>() is [{ UseId: var id }] && id == use &&
            s.E<CardUseFinishedEvent>().All(e => e.ResolutionId != use) &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0) == 1,
            "The converted native multi-target Slash pays at its first actual damage while its original use is unfinished.");
        var hp = s.V().Hp;
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "discard" && c.Cards.Count == 1 &&
            s.V().Hand.Any(card => card.Id == c.Cards[0] && card.Kind == CardKind.Dodge));
        Need(s.E<ShanheSourceLihuoPaidEvent>().Length == 0 && s.V().Hp == hp,
            "Selecting the first of two materials does not prematurely discard one card or pay HP.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "discard" && c.Cards.Count == 1 &&
            s.V().Hand.Any(card => card.Id == c.Cards[0] && card.Kind == CardKind.Dodge));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var paid = s.E<ShanheSourceLihuoPaidEvent>().Single();
        Need(!paid.LostHp && paid.FirstCardId is not null && paid.SecondCardId is not null && paid.FirstCardId != paid.SecondCardId &&
            s.G.CardMovements.Count(m => m.Reason.Value == "shanhe.source-lihuo-3160.cost" &&
                m.To == CardLocation.DiscardPile && (m.CardId == paid.FirstCardId || m.CardId == paid.SecondCardId)) == 2 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == parent.Id && f.ShanheSourceLihuoCost is { Paid: true }) &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != parent.Id) && s.E<CardUseFinishedEvent>().All(e => e.ResolutionId != use),
            "Exactly two real discarded entities suspend the same first-damage cost under their actual movement child.");
        s.Play();
        Need(s.V().Hp == hp && s.E<ShanheSourceLihuoCostStartedEvent>().Length == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use) == 1,
            "The response and movement children return once; a later target cannot charge that converted Slash again.");

        var converted = s.Action(a => a.Kind == LegalActionKind.Slash && s.EffectiveKind(a) == CardKind.FireSlash &&
            a.ConversionSource?.SkillId == Lihuo && Targets(a).SequenceEqual([1]), "source3160 converted Slash");
        var secondSlash = s.Begin(converted);
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "extra") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "extra" && c.Targets.SequenceEqual([2]));
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "shanhe-source-lihuo-cost") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "hp"); s.Play();
        Need(s.V().Hp == hp - 1 && s.E<ShanheSourceLihuoPaidEvent>().Count(e => e.LostHp) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == secondSlash) == 1,
            "A second converted actual Slash independently pays one HP at its first damage and completes its original use.");
        // The frozen client delegates the maximum to the server. Fixed +1 is
        // this local interpretation, rather than a proved server stacking rule.
        var fireActions = s.G.GetHumanLegalActions().Where(a => a.Kind == LegalActionKind.Slash && s.EffectiveKind(a) == CardKind.FireSlash &&
            a.ConversionSource is null).ToArray();
        Need(fireActions.Any(a => Targets(a).Count == 2) && fireActions.All(a => Targets(a).Count <= 2),
            "The game-long Fire Slash target permission survives and remains exactly +1 after repeated conversions.");
        var natural = s.Begin(s.Action(a => a.Kind == LegalActionKind.Slash && s.EffectiveKind(a) == CardKind.FireSlash &&
            a.ConversionSource is null && Targets(a).SequenceEqual([1, 2]), "two-target natural Fire Slash")); s.Play();
        Need(s.E<ShanheSourceLihuoPaidEvent>().Length == 2 && s.E<CardUseFinishedEvent>().Count(e => e.CardId == natural) == 1 &&
            s.E<CardRespondedEvent>().Any(e => e.EffectiveCardKind == CardKind.Dodge),
            "An unconverted physical Fire Slash uses the unlocked target allowance without paying conversion cost, after a genuine Dodge response returned.");
    }

    private static void DiscarderChoosesAndPaidWineReplacesOneJudgmentWithRealDraw()
    {
        var s = Scene.Start(wine: true);
        var owner = s.G.CreateSnapshot(0).Players.Single(p => p.GeneralId == WineOwner).Seat;
        var material = s.V().Hand.Where(c => c.Kind == CardKind.Slash).Take(2).Select(c => c.Id).ToArray();
        s.Skill("discard", [material[0]]);
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "shanhe-source-chunlao-store") == true);
        Need(s.P!.PlayerSeat == 0 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Chunlao).OwnerSeat == owner,
            "The source3161 offer belongs to the actual discarded-card owner, distinct from the living Chunlao owner.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "decline"); s.Play();
        Need(s.Zone(material[0]) == CardLocation.DiscardPile && s.V(owner).ChunlaoCount == 0,
            "Declining preserves the original discarded Slash instead of forcibly collecting it as source3616 does.");
        s.Skill("discard", [material[1]]);
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "shanhe-source-chunlao-store") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "store"); s.Play();
        Need(s.Zone(material[1]) == CardLocation.Chunlao(owner) && s.V(owner).ChunlaoCount == 1 &&
            s.E<ShanheSourceChunlaoStoredEvent>() is [{ SourceSeat: 0, CardId: var stored }] && stored == material[1],
            "Acceptance moves the exact selected discarded entity once into the other source owner's public pile.");
        s.Skill("wound", []);
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch && s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.RoundPileAlcoholReturn is not null));
        var wine = s.E<ProgramRoundPileAlcoholIssuedEvent>().Single();
        Need(wine.Source.OwnerSeat == owner && wine.VictimSeat == 0 && wine.Price == 1 && s.V().Hp == 0 &&
            s.Zone(material[1]) == CardLocation.Processing &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == wine.CardUseFrameId && !f.RoundPileAlcoholCostDrained),
            "The actual dying responder pays one Chun into its original Alcohol use and waits for that use's real material-movement child before healing.");
        s.Play();
        Need(s.V().Hp == 1 && s.Zone(material[1]) == CardLocation.DiscardPile &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == wine.CardUseFrameId && e.CardKind == CardKind.Alcohol) == 1,
            "The single genuine wine rescue returns to the interrupted original Play with one recovery and no new discarded-Slash provenance.");
        var before = s.G.CardMovements.Max(m => m.Sequence);
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch &&
            s.G.ResolutionStack.OfType<DrawPhaseObligationFrame>().Any(f => f.ShanheSourceChunlaoJudgmentDraw is not null));
        var replaced = s.G.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single(f => f.ShanheSourceChunlaoJudgmentDraw is not null);
        Need(s.E<ShanheSourceChunlaoJudgmentConsumedEvent>() is [{ WineUseId: var wineId, DrawFrameId: var drawId }] &&
            wineId == wine.CardUseFrameId && drawId == replaced.Id && s.E<ShanheSourceChunlaoJudgmentCompletedEvent>().Length == 0,
            "The next actual Judgment becomes an owned real Draw whose gain child holds its typed normal-turn return.");
        s.Until(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.State.TurnNumber > 1);
        var complete = s.E<ShanheSourceChunlaoJudgmentCompletedEvent>().Single();
        Need(complete.DrawFrameId == replaced.Id && complete.DrawCount == 2 &&
            s.E<ActualDrawPhaseCompletedEvent>().Count(e => e.OwnerSeat == 0 && e.ActualTurnNumber == complete.ActualTurn) == 2 &&
            s.G.CardMovements.Count(m => m.Sequence > before && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 4 &&
            s.E<ProgramRoundPileAlcoholIssuedEvent>().Length == 1 && s.G.State.Winner == Winner.None,
            "After replacement Draw and its exact children, the same natural turn performs its original normal Draw too, then enters Play without finishing the battle.");
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private static IReadOnlyList<int> Targets(LegalAction a) => a.TargetSeats.Count > 0 ? a.TargetSeats : a.TargetSeat is { } target ? [target] : [];
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public CardKind EffectiveKind(LegalAction a) => a.PlayedCardKind ?? V().Hand.Single(c => c.Id == a.CardId).Kind;
        public LegalAction Action(Func<LegalAction, bool> pick, string purpose) => G.GetHumanLegalActions().FirstOrDefault(pick) ??
            throw new InvalidOperationException("Missing legal action for " + purpose + ": " + JsonSerializer.Serialize(G.GetHumanLegalActions().Select(a => new
                { a.Kind, a.CardId, EffectiveKind = a.CardId is null ? (CardKind?)null : EffectiveKind(a), Targets = Targets(a), a.ConversionSource })));
        public void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Rejected fresh source2492 command."); }
        public void Answer(Func<PromptChoice, bool> pick)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public int Begin(LegalAction a)
        { Accept(new PlayCardCommand(0, a.CardId!.Value, Targets(a), G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources }); return a.CardId.Value; }
        public void Skill(string activation, int[] cards) => Accept(new UseProgramSkillCommand(0, Driver, activation, cards, [], G.Revision, P!.PromptId));
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The small source2492 scenario ended unexpectedly.");
                if (P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Watch)
                { Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); continue; }
                if (P is { PlayerSeat: 0 } q && q.SkillPrompt?.SkillId == Lihuo &&
                    q.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); continue; }
                if (P is { PlayerSeat: 0 } r && r.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "shanhe-source-chunlao-store"))
                { Answer(c => c.Parameters.GetValueOrDefault("branch") == "decline"); continue; }
                Need(P is null || !V(P.PlayerSeat).IsHuman, "Unexpected source2492 prompt: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source2492 missed a bounded fresh native boundary: " + JsonSerializer.Serialize(new
                { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P, Frames = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }) }));
        }
        public static Scene Start(bool wine)
        {
            var setup = new ShanheBattleSetup(83, 1, "fixture:source-jiangdong-2492", wine ? Peer : Actor, 4, 0, 0, 0, 5, [],
                wine ? [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8)] :
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8), new(CardKind.ThunderSlash, Suit.Club, 9), new(CardKind.FireSlash, Suit.Diamond, 10)], [],
                wine ? [new(Target, MaxHpBonus: 8, Hp: 12) { StartingHand = [] }] :
                [new(Target, MaxHpBonus: 8, Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] },
                 new(Target, MaxHpBonus: 8, Hp: 12) { StartingHand = [] }, new(Target, MaxHpBonus: 8, Hp: 12) { StartingHand = [] }],
                wine ? [new(WineOwner) { StartingHand = [] }] : [], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceJiangdongTrioPackage.ChengPuHardId, StartingCards = [],
                EnemyRoster = setup.EnemyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray(),
                AllyRoster = setup.AllyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray()
            }).WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, Peer, WineOwner, Target]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = wine ? 3 : 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 10
            }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-jiangdong-trio", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
              {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                {"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                  {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"loseHp","target":"owner","amount":4}]}]},
                {"id":"{{{Reach}}}","revision":1,"modifiers":[{"id":"range","query":"attackRange","operation":"add","value":4,"priority":0}]},
                {"id":"{{{Watch}}}","revision":1,"triggers":[
                  {"id":"native-payment-child","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment","chunlao"],
                    "movementOccurrence":"perOwnerBatch","optional":false,
                    "movementReasons":["shanhe.source-lihuo-3160.cost","skill-program.{{{Chunlao}}}.round-pile-alcohol.pay"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]},
                  {"id":"native-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                    "movementOccurrence":"perBatch","optional":false,
                    "movementReasons":["{{{CardMoveReasons.Draw.Value}}}","program.extra-draw-phase.draw"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
              """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "原生弃牌与失去体力入口", description = "制造真实弃牌和濒死。" },
                    [Reach] = new { name = "测试射程", description = "使三个目标都在攻击范围内。" },
                    [Watch] = new { name = "实际付款与摸牌孩子", description = "完成后返回同一原始父帧。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var skill in catalog.Programs)
                builder.AddSkill(new(skill.Key, catalog.Presentations[skill.Key].Name, catalog.Presentations[skill.Key].Description) { Program = skill.Value });
            builder.AddGeneral(new(Actor, "源疠火", "supporter", Lihuo, "wu", 4, [Driver, Reach, Watch]));
            builder.AddGeneral(new(Peer, "原弃牌者", "supporter", Driver, "wu", 4, [Watch]));
            builder.AddGeneral(new(WineOwner, "源醇醪", "supporter", Chunlao, "wu", 4, [Watch]));
            builder.AddGeneral(new(Target, "无反击目标", "supporter", null, "qun", 4));
            builder.AddDeck(new(Deck, "小型实体牌堆", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
        }
    }
}
