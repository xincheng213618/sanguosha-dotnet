using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceLiuYanTrioChecks
{
    private const string Xiechan = StandardShanheSourceLiuYanTrioPackage.XiechanSkillId;
    private const string Jingqi = StandardShanheSourceLiuYanTrioPackage.JingqiSkillId;
    private const string Actor = "fixture:source-liuyan-trio-actor", Aura = "fixture:source-liuyan-trio-aura";
    private const string Peer = "fixture:source-liuyan-trio-peer", Driver = "fixture:source-liuyan-trio-driver";
    private const string Watch = "fixture:source-liuyan-trio-response-child", Deck = "fixture:source-liuyan-trio-deck";

    public static void Source1161TeamDistanceAnd4055LimitedActualPindianDirectionalDuel()
    {
        TeamDistanceIncludesHolderAndAlliesAndDropsDeadHolder();
        foreach (var (sourceRank, opponentRank) in new[] { (13, 12), (7, 13), (13, 13) })
            ActualPindianAndDuelResponseReturn(sourceRank, opponentRank);
    }

    private static void TeamDistanceIncludesHolderAndAlliesAndDropsDeadHolder()
    {
        var s = Scene.Start(aura: true);
        var holder = s.V().Players.Single(p => p.GeneralId == Aura).Seat;
        var plainAlly = s.V().Players.Single(p => p.Seat != 0 && p.TeamId == ShanheBattleSetup.PlayerTeamId && p.GeneralId != Aura).Seat;
        // Production campaign seats are player, all allies, then all enemies.
        // Removing seat1 leaves the shorter 0->7->6->5 path unchanged, so its
        // distance increase isolates the removal of exactly one aura term.
        var before = new { Holder = holder, Ally = plainAlly,
            OwnerToEnemy = s.G.GetCombatDistance(0, 5), AllyToEnemy = s.G.GetCombatDistance(plainAlly, 6),
            OwnerToHolder = s.G.GetCombatDistance(0, holder), AllyToOwner = s.G.GetCombatDistance(plainAlly, 0),
            EnemyToOwner = s.G.GetCombatDistance(5, 0) };
        Need(s.G.PlayerCount == 8 && holder == 1 && plainAlly == 2 &&
            before.OwnerToEnemy == 1 && before.AllyToEnemy == 2 && before.OwnerToHolder == 1 &&
            before.AllyToOwner == 2 && before.EnemyToOwner == 3,
            "Two living source1161 holders each subtract one for their own team's enemy direction, including a holder and an ordinary ally; friendly targets and enemy-to-player distance are untouched. " +
            JsonSerializer.Serialize(before) + s.Diagnostic());
        s.Accept(new UseProgramSkillCommand(0, Driver, "damage", [], [holder], s.G.Revision, s.P!.PromptId)); s.Play();
        var after = new { OwnerToEnemy = s.G.GetCombatDistance(0, 5), EnemyToOwner = s.G.GetCombatDistance(5, 0) };
        Need(!s.Player(holder).IsAlive && after.OwnerToEnemy == 2 && after.EnemyToOwner == 3,
            "After the ally holder really dies, only the surviving owner's aura remains; the unchanged shorter seat path isolates loss of that one directional contribution. " +
            JsonSerializer.Serialize(after) + s.Diagnostic());
    }

    private static void ActualPindianAndDuelResponseReturn(int sourceRank, int opponentRank)
    {
        var s = Scene.Start(aura: false, sourceRank, opponentRank);
        var opponent = s.V().Players.First(p => p.TeamId == ShanheBattleSetup.EnemyTeamId).Seat;
        var sourceCard = s.Player(0).Hand.Single(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Spade && c.Rank == sourceRank).Id;
        s.Accept(new UseProgramSkillCommand(0, Xiechan, "limited-contest-duel", [], [opponent], s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") is "pindian-source-card" or "pindian-start") == true);
        s.Answer(c => c.Cards.SequenceEqual([sourceCard]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.SelectedActorDuelOrigin?.ShanheSourceXiechan is not null));
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.SelectedActorDuelOrigin?.ShanheSourceXiechan is not null);
        var origin = use.SelectedActorDuelOrigin!;
        var issued = s.E<ShanheSourceXiechanDuelIssuedEvent>().Single();
        var contest = s.E<PindianResultDeterminedEvent>().Single(e => e.SkillId == Xiechan);
        var sourceWon = sourceRank > opponentRank;
        var expectedActor = sourceWon ? 0 : opponent; var expectedTarget = sourceWon ? opponent : 0;
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == origin.ParentProgramFrameId);
        Need(contest.Result.SourceCardId == sourceCard && contest.Result.SourceRank == sourceRank && contest.Result.OpponentRank == opponentRank &&
            issued.PindianFrameId == contest.FrameId && issued.SourceCardId == contest.Result.SourceCardId &&
            issued.OpponentCardId == contest.Result.OpponentCardId && issued.SourceWon == sourceWon &&
            issued.ActorSeat == expectedActor && issued.TargetSeat == expectedTarget &&
            use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0 } action &&
            action.ActorSeat == expectedActor && action.ProviderSeat == expectedActor && action.TargetSeats.SequenceEqual([expectedTarget]) &&
            parent.InstructionIndex == 2 && parent.PindianResultBindings.Single().NativePindianFrameId == contest.FrameId &&
            s.E<ProgramSkillResolvedEvent>().All(e => e.FrameId != parent.Id) &&
            s.E<CardUseFinishedEvent>().All(e => e.ResolutionId != use.Id) &&
            s.E<CardRespondedEvent>().Any(e => e.ResponderSeat == 0 && e.EffectiveCardKind == CardKind.Slash),
            "The original actual Pindian and both physical contest entities choose the real Duel actor (ties reverse); a real Slash response child pauses before the exact original program/use return. " + s.Diagnostic());
        s.Play();
        Need(s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == issued.UseId && e.CardKind == CardKind.Duel) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == parent.Id && e.Completed) == 1 &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == opponent && e.EffectiveCardKind == CardKind.Slash) == (sourceWon ? 2 : 1) &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 0 && e.EffectiveCardKind == CardKind.Slash) == 1 &&
            // Enemy HP1 makes the native AI's real Slash response critical.
            // It has two Slashes versus our one, so alternating responses end
            // with enemy->owner damage even when our initial Duel actor won.
            s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == opponent && e.TargetSeat == 0 && e.Amount == 1) &&
            s.Player(0).Hp == 2 && s.Player(opponent).Hp == 1 &&
            s.G.CardMovements.Count(m => m.CardId == contest.Result.SourceCardId && m.Reason == CardMoveReasons.PindianFinish) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == contest.Result.OpponentCardId && m.Reason == CardMoveReasons.PindianFinish) == 1,
            "Both sides respond with real Slash entities; their deterministic alternating sequence deals enemy-to-owner damage, then the genuine Duel completes and resumes the original limited program once. " + s.Diagnostic());
        Need(s.Player(0).Hand.Count > 0 && s.Player(opponent).HandCount > 0,
            "Both participants retain a real spare hand card, so another contest would remain payable without the spent limited quota.");
        var revision = s.G.Revision;
        var repeated = s.G.Submit(new UseProgramSkillCommand(0, Xiechan, "limited-contest-duel", [], [opponent], revision, s.P!.PromptId));
        Need(!repeated.Accepted && s.G.Revision == revision && s.E<PindianResultDeterminedEvent>().Count(e => e.SkillId == Xiechan) == 1 &&
            s.E<ShanheSourceXiechanDuelIssuedEvent>().Length == 1,
            "Source4055's once-game quota rejects a second genuine activation without another contest, use or mutation.");
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public GameSnapshot V() => G.CreateSnapshot(0);
        public PlayerSnapshot Player(int seat) => V().Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Fresh source2463 command rejected."); }
        public void Answer(Func<PromptChoice, bool> test)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(test) ?? throw new InvalidOperationException("Missing source2463 choice. " + Diagnostic());
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 220; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fresh bounded source2463 scene stays live. " + Diagnostic());
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.SkillPrompt?.SkillId == Watch) { Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); continue; }
                    if (p.Kind == DecisionKind.RespondSlash && p.Choices.Any(c => c.Cards.Count == 1 &&
                        Player(0).Hand.Any(card => card.Id == c.Cards[0] && card.Kind == CardKind.Slash)))
                    { Answer(c => c.Cards.Count == 1 && Player(0).Hand.Any(card => card.Id == c.Cards[0] && card.Kind == CardKind.Slash)); continue; }
                    if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage"))
                    { Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage"); continue; }
                    throw new InvalidOperationException("Unexpected source2463 human prompt. " + Diagnostic());
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source2463 missed its bounded actual boundary. " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new { G.State.Phase, G.State.CurrentSeat, P,
            Players = V().Players.Select(p => new { p.Seat, p.Hp, p.IsAlive, p.TeamId }),
            Frames = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool aura, int sourceRank = 13, int opponentRank = 12)
        {
            var setup = new ShanheBattleSetup(93, 1, "fixture:source-liuyan-trio-2463", Actor, 3, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Spade, sourceRank), new(CardKind.Slash, Suit.Club, 10), new(CardKind.Dodge, Suit.Heart, 1)], [],
                aura ? Enumerable.Range(0, 5).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray() :
                [new(Peer, Hp: 1) { StartingHand = [new(CardKind.Dodge, Suit.Diamond, opponentRank),
                    new(CardKind.Slash, Suit.Club, 2), new(CardKind.Slash, Suit.Spade, 3), new(CardKind.Dodge, Suit.Heart, 1)] },
                 new(Peer) { StartingHand = [] }],
                aura ? [new(Aura) { StartingHand = [] }, new(Peer) { StartingHand = [] }] : [new(Peer) { StartingHand = [] }],
                new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceLiuYanTrioPackage.LiuYanBaseId,
                EnemyRoster = setup.EnemyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray(),
                AllyRoster = setup.AllyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray()
            }).WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, Aura, Peer]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = aura ? 8 : 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
            }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-liuyan-trio", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
              {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":4}]}]},
                {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"real-duel-response","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["{{{CardMoveReasons.Respond.Value}}}"],"optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
              """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实伤害入口", description = "实际造成伤害，核实精骑持有者阵亡。" },
                    [Watch] = new { name = "真实决斗响应孩子", description = "保留原挟缠决斗及拼点来源后返回。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p });
            b.AddGeneral(new(Actor, "精骑挟缠主体", "supporter", Jingqi, "qun", 3, [Xiechan, Driver, Watch]));
            b.AddGeneral(new(Aura, "精骑队友", "supporter", Jingqi, "qun", 2));
            b.AddGeneral(new(Peer, "普通角色", "supporter", null, "qun", 6));
            // Explicit recipes alone supply all contest/response entities. The
            // ordinary initial/draw counts are zero, with no random hand filler.
            b.AddDeck(new(Deck, "固定小型牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 110).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 3)).ToArray() });
        }
    }
}
