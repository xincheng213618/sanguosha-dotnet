using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryGaoShunMobileChecks
{
    private const string Hero = "boundary:gao-shun-mobile", Xianzhen = "boundary:xianzhen-mobile", Jinjiu = "boundary:jinjiu-mobile",
        Peer = "fixture:mobile-gs-peer", Quiet = "fixture:mobile-gs-quiet", Extra = "fixture:mobile-gs-extra", Driver = "fixture:mobile-gs-driver", Watch = "fixture:mobile-gs-watch";

    public static void NativePindianOnlyThisPlayAndPrintedSlashTurnExemptionCold()
    {
        var s = Scene.Start(); s.Play(); s.Drive("equip", [2]); s.Play(); s.Drive("boost", []); s.Play();
        s.Need(s.V(2).Equipment.Single().Kind == CardKind.SilverLion && s.G.GetSeatDistance(0, 2) > s.G.GetAttackRange(0),
            "The actual native target is distant and really equipped with Silver Lion before the contest.");
        var high = s.V().Hand.Single(c => c.Kind == CardKind.Slash && c.Rank == 13).Id;
        s.Contest(high, 2); s.Need(s.G.ResolutionStack.Last() is PindianFrame, "The real native Pindian remains on the original activation frame.");
        s.Cold(); s.Play();
        var won = s.E<BoundaryGaoShunXianzhenIssuedEvent>().Single();
        s.Need(won.Won && won.SlashHandLimitExempt && s.V().TurnHandLimitCardKindExemptions is { Count: 1 } &&
            s.E<PindianResultDeterminedEvent>().Single().Result.SourceRank == 13 && !s.Actions().Any(a => a.ProgramSkillId == Xianzhen),
            "The completed real comparison issues one exact Play lease and a native turn hand-limit exemption for the owner's own printed Slash; this Play cannot contest twice.");
        for (var i = 0; i < 2; i++)
        {
            s.Slash(2); s.HeldUse();
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Last();
            s.Need(use.SourceSeat == 0 && use.Action!.TargetSeats.SequenceEqual([2]) &&
                use.Action.PhysicalCards is [{ CardId: var material }] &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                s.E<CardUseDebitRecordedEvent>().Count(e => e.Debit.ActorSeat == 0) == i + 1,
                "A real accepted distant Slash uses the won-opponent quota without OL extra targets.");
            if (i == 0) s.Cold(); s.Continue(); s.Play();
        }
        s.Need(s.V(2).Hp == 12 && s.E<DamageAppliedEvent>().Count(d => d.TargetSeat == 2 && d.Amount == 2) == 2,
            "Two actual physical Slashes ignore the real Silver Lion and retain their unrelated native +1 damage modifier.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == won.Phase.TurnNumber && s.Actions().Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([3])) &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash || !a.TargetSeats.Contains(2)) && s.V().TurnHandLimitCardKindExemptions is { Count: 1 },
            "The next true Play in this same actual turn loses the old distance/quota policy but retains the Slash hand-limit exemption.");
        var wine = s.V().Hand.First(c => c.Kind == CardKind.Alcohol).Id;
        s.Contest(wine, 2); s.Play();
        var lost = s.E<BoundaryGaoShunXianzhenIssuedEvent>().Last();
        s.Need(!lost.Won && !lost.SlashHandLimitExempt && s.E<PindianResultDeterminedEvent>().Last().Result.SourceRank == 1 &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash) && s.V().TurnHandLimitCardKindExemptions is { Count: 1 },
            "A real rank-one Wine Pindian is neither OL K nor a used/played Slash; loss bans Slash uses only for this new Play, regardless of the previous win.");
        s.Drive("duel", [1, 0]); s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 });
        var response = s.V().Hand.Single(c => c.Kind == CardKind.Alcohol).Id; s.Cold();
        s.Answer(c => c.Cards.SequenceEqual([response])); s.Play();
        s.Need(s.E<DuelResponseEvent>().Any(e => e.ResponderSeat == 0 && e.UsedSlash) &&
            s.E<CardActionAcceptedEvent>().Any(e => e.Action.Type == CardActionType.Response && e.Action.ActorSeat == 0 && e.Action.EffectiveKind == CardKind.Slash && e.Action.PhysicalCards.Any(c => c.CardId == response)) &&
            s.Actions().All(a => a.Kind != LegalActionKind.Slash),
            "The exact native cold Duel response may still play Wine as ordinary Slash while Xianzhen forbids only Slash uses.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber > won.Phase.TurnNumber && s.V().TurnHandLimitCardKindExemptions is null && s.Actions().Any(a => a.Kind == LegalActionKind.Slash),
            "Actual turn end removes the native hand-limit exemption and the next normal Play has no old loss prohibition.");
    }

    public static void NativeAlcoholConsumptionReductionKeepsOtherBonusesAndForeignBanCold()
    {
        var s = Scene.Start(wineAttack: true); s.Play(); s.Drive("boost", []); s.Play();
        var wine = s.V().Hand.Single(c => c.Kind == CardKind.Alcohol).Id;
        s.PlayAction(s.Actions().Single(a => a.CardId == wine && a.Kind == LegalActionKind.Alcohol)); s.Play();
        s.Slash(3); s.HeldUse();
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Last();
        s.Need(use is { ConsumedAlcoholBoost: true, ConsumedAlcoholBonus: 1 } &&
            s.E<CardUseAlcoholConsumptionCapturedEvent>().Single(e => e.CardUseFrameId == use.Id).AlcoholBonus == 1,
            "The original native physical Slash freezes its actual Wine consumption before reaching the target, independently of the other +1 bonus.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V(3).Hp == 6 && s.E<ProgramDamageReducedEvent>().Single(e => e.SkillId == Jinjiu) is { AmountBefore: 3, AmountAfter: 2 } &&
            s.E<DamageAppliedEvent>().Single(e => e.TargetSeat == 3).Amount == 2,
            "Jinjiu subtracts only the real Wine's one point from three, preserving unrelated damage rather than changing damage to one.");
        s.Slash(3); s.HeldUse(); s.Continue(); s.Play();
        s.Need(s.V(3).Hp == 4 && s.E<ProgramDamageReducedEvent>().Count(e => e.SkillId == Jinjiu) == 1 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 3 && e.Amount == 2) == 2,
            "A later real Slash with the same unrelated bonus and no consumed Wine is not reduced or given a stale Wine receipt.");
        var banned = Scene.Start(); banned.Play(); banned.Drive("kill", [1]); banned.Play();
        banned.Need(!banned.V(1).IsAlive && !banned.E<DyingResponseEvent>().Any(e => e.ResponderSeat == 1 && e.UsedAlcohol),
            "During the actual Gao Shun turn another role's real held Wine cannot be used to rescue itself.");
        var allowed = Scene.Start(wineAttack: true); allowed.Play();
        var rescueWine = allowed.V(1).Hand.Single(c => c.Kind == CardKind.Alcohol).Id;
        allowed.Drive("kill", [1]); allowed.Play();
        allowed.Need(allowed.V(1).IsAlive && allowed.V(1).Hp == 1 && allowed.E<DyingResponseEvent>().Any(e => e.ResponderSeat == 1 && e.UsedAlcohol),
            "With Gao Shun alive elsewhere but outside his actual turn, the same native foreign self-rescue really uses its physical Wine. " +
            JsonSerializer.Serialize(new { Victim = new { allowed.V(1).Hp, allowed.V(1).IsAlive, allowed.V(1).HandCount },
                WineMoves = allowed.G.CardMovements.Where(m => m.CardId == rescueWine).Select(m => new { m.From, m.To, m.Reason }),
                Responses = allowed.E<DyingResponseEvent>(), Wine = allowed.E<CardUseDeclaredEvent>().Where(e => e.CardKind == CardKind.Alcohol),
                Recoveries = allowed.E<RecoveryAppliedEvent>().Where(e => e.TargetSeat == 1) }));
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public IReadOnlyList<LegalAction> Actions() => G.GetHumanLegalActions();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void HeldUse() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Contest(int card, int target) => Accept(new UseProgramSkillCommand(0, Xianzhen, "contest", [card], [target], G.Revision, P!.PromptId));
        public void Slash(int target) => PlayAction(Actions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([target]) && V().Hand.Single(c => c.Id == a.CardId).Kind == CardKind.Slash));
        public void PlayAction(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 240; step++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human boundary interrupted the native mobile Gao Shun scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The fixed native Gao Shun scene exceeded its step bound.");
        }
        private void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected native Gao Shun command."); }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint()); var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold retains the actual native Pindian child, exact issued phase, physical card payments and original consumed-Wine use."); G = restored;
        }
        public void Need(bool yes, string why)
        {
            if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase,
                Pending = P?.Kind, Seat = P?.PlayerSeat, Skill = P?.SkillPrompt?.SkillId, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(5).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool wineAttack = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            string[] skills = wineAttack ? [Driver, Watch] : [Quiet, Driver, Watch, Extra];
            var hand = new List<ShanheStartingCard> { new(CardKind.Slash, Suit.Spade, 13), new(CardKind.Slash, Suit.Club, 2), new(CardKind.Slash, Suit.Diamond, 3), new(CardKind.Slash, Suit.Heart, 4), new(CardKind.Alcohol, Suit.Club, 1) };
            if (!wineAttack) hand.Add(new(CardKind.Alcohol, Suit.Spade, 1));
            var ally1 = new ShanheCombatantDefinition(Peer, Hp: 16) { StartingHand = [new(CardKind.Alcohol, Suit.Heart, 1)] };
            var enemy3 = wineAttack ? new ShanheCombatantDefinition(Hero, MaxHpBonus: 4, Hp: 8, ExtraSkillIds: [Quiet]) { StartingHand = [] }
                : new ShanheCombatantDefinition(Peer, Hp: 16) { StartingHand = [] };
            var enemy2 = new ShanheCombatantDefinition(Peer, Hp: 16) { StartingHand = wineAttack ? [] : [new(CardKind.DrawTwo, Suit.Club, 8), new(CardKind.DrawTwo, Suit.Spade, 8)] };
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-gao-shun", wineAttack ? Peer : Hero, wineAttack ? 16 : 8, wineAttack ? 0 : 4, 0, 0, 0, skills,
                hand, wineAttack ? [new(CardKind.Crossbow, Suit.Club, 5)] : [], [enemy2, enemy3], [ally1], new string('c', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-gao-shun-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:mobile-gs-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            object Activation(string id, int count, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = count, maxTargets = count, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[]
            {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Extra, revision = 1, triggers = new[] { new { id = "extra", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, usageScope = "game", usageLimit = 1,
                    effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Driver, revision = 1, activations = new[]
                {
                    Activation("equip", 1, new { op = "useRandomDeckEquipment", target = "owner", resultBind = "armor" }),
                    Activation("boost", 0, new { op = "grantTurnCardDamageModifier", target = "owner", cardKinds = new[] { "slash" }, amount = 1 }),
                    Activation("duel", 2, new { op = "startVirtualDuel", target = "owner" }),
                    Activation("kill", 1, new { op = "damage", target = "selectedTarget", amount = 16 })
                } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "held-use", window = "cardUseBeforeTargetEffects", ownerRelation = "actor", cardKinds = new[] { "slash", "fireSlash", "thunderSlash" },
                    optional = false, isSkillInvocation = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "next", options = new[] { new { id = "continue" } } } } } } }
            } });
            var names = new Dictionary<string, object> { [Quiet] = new { name = "静默摸牌", description = "跳过无关普通摸牌。" },
                [Extra] = new { name = "额外出牌", description = "原生插入出牌阶段。" }, [Driver] = new { name = "真实原生驱动", description = "装备、伤害及决斗。" },
                [Watch] = new { name = "真实杀子窗", description = "保留原生杀及其酒凭据。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "安静同桌", "", Quiet, "wei", 16));
            const string deck = "fixture:mobile-gs-deck";
            b.AddDeck(new(deck, "真实装备库存", 0, 2, []) { PhysicalCards = new[] { new ContentDeckPhysicalCard("fixture:mobile-gs-silver-lion", Suit.Club, 5) }.Concat(
                Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, i % 13 + 1))).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动版界高顺原生路径", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
