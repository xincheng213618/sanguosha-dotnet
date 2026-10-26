using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileXuChuChecks
{
    private const string Hero = "mobile:xu-chu", Skill = "mobile:luoyi", Owner = "fixture:ordinary-mobile-xu-chu",
        Clock = "fixture:ordinary-xu-chu-extra-draw", Duel = "fixture:ordinary-xu-chu-duel", Quiet = "fixture:ordinary-xu-chu-quiet",
        Deck = "fixture:ordinary-xu-chu-deck", Mode = "identity:classic-ordinary-mobile-xu-chu";

    public static void ForeignDuelDamageSourceBonusExpiresAtActualTurnEnd()
    {
        var s = Scene.Start(); s.Offer();
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.DuelChoice();
        var grant = s.Facts<CardDamageModifierGrantedEvent>().Single(e => e.Modifier.Source.SkillId == Skill).Modifier;
        s.Need(s.NormalDraws == 1 && s.V(0).HandCount == 1 &&
            s.Facts<ProgramNormalDrawAdjustedEvent>().Single(e => e.SkillId == Skill).Adjustment == -1 &&
            grant.Expiration == SkillProgramDamageModifierExpiration.CurrentTurnEnd && grant.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource &&
            ProgramCardKindMatch.Contains(grant.CardKinds, CardKind.ColdSlash),
            "The genuine inserted Draw phase pays one normal card instead of two and issues one current-turn damage-source benefit, including the complete Slash family.");
        var turn = s.G.State.TurnNumber; var hp = s.V(1).Hp;
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel);
        s.Need(use.Action is { ActorSeat: 1, Type: CardActionType.Use } action && action.TargetSeats.SequenceEqual([0]) && s.V(1).HandCount == 0,
            "An actual native Duel is used by another character against Xu Chu during Xu Chu's granting turn; the opponent has no physical Slash to answer the counter-response.");
        var firstUse = use.Id; var slash = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single();
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([slash])); s.Offer();
        s.Need(s.V(1).Hp == hp - 2 && s.Facts<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == firstUse) is
            { SourceSeat: 0, TargetSeat: 1, Amount: 2 } &&
            s.Facts<CardActionAcceptedEvent>().Any(e => e.Action is { ActorSeat: 0, Type: CardActionType.Response, EffectiveKind: CardKind.Slash } a &&
                a.PhysicalCards.Any(c => c.CardId == slash)) &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0)) == 1,
            "The one real Slash response wins the foreign Duel and deals base1 plus Luoyi1, with no invented damage or repeated physical payment after cold.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        s.Need(s.NormalDraws == 3 && s.Facts<CardDamageModifierGrantedEvent>().Count(e => e.Modifier.Source.SkillId == Skill) == 1,
            "The following ordinary Draw is genuinely two cards when its separate Luoyi offer is declined; it neither replaces nor pays the earlier one-card draw again.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.G.State.TurnNumber > turn && s.IsDuelChoice);
        s.Need(s.Facts<TurnCardUseEffectsExpiredEvent>().Any(e => e.GrantSequences.Contains(grant.GrantSequence)) &&
            s.Facts<TurnStartedEvent>().Last().ActorSeat == 0,
            "The original issued benefit expires at its real turn end before Xu Chu's next preparation, rather than lasting until another draw or next skill activation.");
        hp = s.V(1).Hp; s.Answer(c => c.Targets.SequenceEqual([1]));
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        var nextUse = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Duel).Id;
        s.Answer(c => c.Cards.Count == 1); s.Offer();
        s.Need(s.V(1).Hp == hp - 1 && s.Facts<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == nextUse) is
            { SourceSeat: 0, TargetSeat: 1, Amount: 1 } && nextUse != firstUse &&
            s.Facts<CardDamageModifierGrantedEvent>().Count(e => e.Modifier.Source.SkillId == Skill) == 1,
            "The same genuine foreign-Duel counter-response after expiry deals only base1 before any new Luoyi grant.");
    }

    private static string Peer(int seat) => "fixture:ordinary-xu-chu-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int NormalDraws => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw);
        public bool IsDuelChoice => P?.SkillPrompt?.SkillId == Duel && P.Choices.Any(c => c.Targets.SequenceEqual([1]));
        public void DuelChoice() => Reach(() => IsDuelChoice);
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Xu Chu command."); }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 260; step++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose) { Accept(new SelectGeneralCommand(choose.PlayerSeat, choose.PlayerSeat == 0 ? Owner : Peer(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } play) { Accept(new EndPlayPhaseCommand(play.PlayerSeat, G.Revision, play.PromptId)); continue; }
                if (P is { Kind: DecisionKind.Nullification } || P is { Kind: DecisionKind.RespondSlash, PlayerSeat: not 0 })
                { Answer(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage"); continue; }
                Need(P is null, "Unexpected fixed native Xu Chu boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The fixed Xu Chu scene did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the one-card Draw, issued source-scoped receipt and actual foreign Duel response parent.");
        }
        public void Need(bool yes, string why)
        { if (!yes) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase, Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-xu-chu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var definitions = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "no-peer-draw", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Clock, revision = 1, triggers = new[] { new { id = "one-native-extra-draw", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, isSkillInvocation = false,
                    priority = 1000, usageScope = "game", usageLimit = 1, effects = new[] { new { op = "insertPhase", target = "owner", phase = "draw", phaseContinuation = "beforeNormalPreparation" } } } } },
                new { id = Duel, revision = 1, triggers = new[] { new { id = "real-foreign-duel", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, isSkillInvocation = false,
                    priority = -10, effects = new object[] { new { op = "selectTarget", target = "owner", targetKind = "otherLiving" },
                        new { op = "useVirtualOrdinaryTrick", target = "owner", actorRef = new { kind = "selectedTarget" }, targetRef = new { kind = "owner" }, outputKind = "duel" } } } } }
            };
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "真实空手对手", description = "对手正常摸牌减2。" },
                [Clock] = new { name = "一次真实额外摸牌阶段", description = "在原准备窗口前完成真实Draw后返回原父帧。" },
                [Duel] = new { name = "真实他人决斗", description = "其他角色在原准备窗口对本人使用原生决斗。" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实普通移动许褚", "supporter", Skill, "wei", 4, [Clock, Duel]));
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "真实无杀对手", "supporter", Quiet, "wei", 10));
            b.AddDeck(new(Deck, "固定真实杀实体", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "普通移动许褚伤害来源期限", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
