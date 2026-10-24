using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryYuanShuMobileChecks
{
    private const string Hero = "boundary:yuan-shu-mobile", Yongsi = "boundary:yongsi-mobile", Jixi = "boundary:jixi-mobile", Wangzun = "boundary:wangzun-mobile",
        Owner = "fixture:boundary-yuan-shu-owner", Driver = "fixture:boundary-yuan-shu-driver", Defense = "fixture:boundary-yuan-shu-defense", Watch = "fixture:boundary-yuan-shu-paid", Mode = "identity:classic-boundary-yuan-shu";

    public static void FactionReplacementPaidDiscardThreeOwnTurnsAndPermanentLordGrantCold()
    {
        var s = Scene.Start(); s.Play(0); s.End(); s.Play(1);
        s.Need(s.V(1).HandCount == 8 && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Yongsi && e.Window == SkillProgramTriggerWindow.DrawPhaseStarting) == 1,
            "Four actually represented factions replace the native normal draw2 with exactly4, rather than add four to the normal draw.");
        s.Use(1, "lose-recover"); s.Play(1);
        s.Need(s.V(1).Hp == 4 && s.E<BoundaryYuanShuOwnHpDecreasedEvent>() is [{ OwnerSeat: 1, Before: 4, After: 3 }],
            "Real direct HP loss in the owner's actual turn is remembered even after native recovery restores the same current HP.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Yongsi && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "discard"));
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "discard");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        var cost = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Answer(c => c.Cards.SequenceEqual([cost])); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        s.Need(s.V(1).Hand.All(c => c.Id != cost) && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Yongsi),
            "The mandatory branch pays one actual hand entity while the original Yongsi frame still awaits its native cards-moved child.");
        s.Cold(); s.Answer(_ => true); s.Play(2); s.Use(2, "wound-recover-target", 1); s.Play(2);
        s.Need(s.V(1).Hp == 4 && s.E<BoundaryYuanShuOwnHpDecreasedEvent>().Length == 1,
            "An actual damage and recovery during another player's turn does not poison a consecutive own-turn window.");
        s.End(); s.Play(3); s.End(); s.Play(4); s.End(); s.Play(0); s.End(); s.Play(1);
        s.Use(1, "self-damage-recover", 0); s.Play(1);
        s.Need(s.E<BoundaryYuanShuOwnHpDecreasedEvent>().Length == 2 && s.V(1).Hp == 4,
            "Native damage during the next own turn also breaks the streak and remains recorded after recovery.");
        s.NextOwn(); s.NextOwn();
        s.Need(s.E<MaximumHpChangedEvent>().All(e => e.SkillId != Jixi), "Two clean own turns cannot awaken from the unchanged current HP or global table-turn count.");
        s.NextOwn(); s.End(); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "lord") == true);
        s.Need(s.V(1).MaxHp == 5 && s.V(1).Hp == 5 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jixi).BoundaryYuanShuJixi is not null,
            "Only the third consecutive clean actual own turn issues Jixi; its original frame has already paid native maximumHP+1 and recovery1 before the branch prompt.");
        s.Cold(); var hand = s.V(1).HandCount; s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "lord"); s.Play(2);
        s.Need(s.V(1).HandCount == hand + 2 && s.V(1).MaxHp == 5 && s.V(1).Role != Role.Lord && s.V(1).Skills!.Any(k => k.Id == "classic:jijiang") &&
            s.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Jixi && e.Delta == 1) == 1 && s.E<BoundaryYuanShuLordSkillGrantedEvent>() is [{ OwnerSeat: 1, LordSeat: 0, SkillId: "classic:jijiang" }] &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(1) && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program." + Yongsi + ".SelectAndMoveOwnedCard") == 1,
            "Cold resumes the paid awakening once, draws exactly2 after discard and obtains an actual source-bound nonlord copy of the current Lord's Jijiang.");
        s.End(); s.Play(3); s.End(); s.Play(4); s.End(); s.Play(0); s.Use(0, "remove-jijiang"); s.Play(0); s.End(); s.Play(1);
        var request = s.G.GetLegalActions(1).Single(a => a.Kind == LegalActionKind.UseProgramSkill && a.ProgramSkillId == "classic:jijiang");
        s.Accept(new UseProgramSkillCommand(1, "classic:jijiang", request.ProgramActivationId!, [], [2], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 }); var slash = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single();
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([slash])); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 });
        s.Need(s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Action is { ActorSeat: 1, ProviderSeat: 0, RequesterSeat: 1 } a && a.PhysicalCards.Any(c => c.CardId == slash)),
            "The permanently acquired Lord skill remains qualified after the source Lord really loses it, and actually issues the Shu provider's original physical Slash for the nonlord requester.");
        s.Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); s.Play(1);
        s.Use(1, "remove-jijiang"); s.Play(1); s.Use(1, "plain-jijiang"); s.Play(1); s.NextOwn();
        s.Need(s.V(1).Skills!.Any(k => k.Id == "classic:jijiang") && s.G.GetLegalActions(1).All(a => a.ProgramSkillId != "classic:jijiang") &&
            s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 1 && e.TargetSeat == 2),
            "Removing the exact issued copy and ordinarily reacquiring the same skill ID cannot inherit its old Lord-role qualification receipt.");
    }

    public static void WangzunOnlyLordPreparationAndCurrentTurnHandLimit()
    {
        var s = Scene.Start(); s.Play(0); s.End(); s.Play(1); s.NextOwn(); s.NextOwn(); s.End();
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "wangzun") == true); s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "wangzun"); s.Play(2);
        s.Need(s.V(1).Skills!.Any(k => k.Id == Wangzun) && s.V(1).Skills!.All(k => k.Id != "classic:jijiang") && s.E<SkillAwakenedEvent>().Count(e => e.SkillId == Jixi) == 1,
            "The alternative paid awakening branch acquires the official embedded Wangzun instead of either Weidi or a different wiki revision.");
        s.End(); s.Play(3); s.End(); s.Play(4); s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Wangzun && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        var hand = s.V(1).HandCount; var ordinary = s.Limit(0); var turn = s.E<TurnStartedEvent>().Last(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play(0);
        s.Need(turn.ActorSeat == 0 && s.V(1).HandCount == hand + 1 && s.Limit(0) == ordinary - 1 &&
            s.E<TurnRuleModifierGrantedEvent>().Last().Modifier is { Query: SkillRuleQuery.HandLimit, Amount: -1, AffectedSeat: 0 } modifier && modifier.TurnNumber == turn.TurnNumber && modifier.TurnSeat == 0,
            "Wangzun draws the owner exactly1 at the actual Lord's preparation and applies native handlimit-1 to that Lord during that same actual turn.");
        s.Cold(); s.End(); s.Play(1);
        s.Need(s.Limit(0) == ordinary && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Wangzun) == 1,
            "The native penalty expires at the Lord's real turn end and cannot trigger again at a nonlord's preparation.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Limit(int seat) { const BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic; var p = (IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players", f)!.GetValue(G)!; return (int)typeof(GameEngine).GetMethod("GetHandLimit", f)!.Invoke(G, [p[seat]])!; }
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } && P.PlayerSeat == seat);
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void NextOwn() { End(); foreach (var seat in new[] { 2, 3, 4, 0 }) { Play(seat); End(); } Play(1); }
        public void Use(int owner, string id, params int[] targets) => Accept(new UseProgramSkillCommand(owner, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected Yuan Shu fixture command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 260 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 1 ? Owner : "fixture:boundary-yuan-shu-peer-" + setup.PlayerSeat, G.Revision, setup.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Yongsi)
                {
                    if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "discard")) { Answer(c => c.Parameters.GetValueOrDefault("option-id") == "discard"); continue; }
                    if (P.Choices.Any(c => c.Cards.Count == 1)) { Answer(c => c.Cards.Count == 1); continue; }
                }
                if (P?.SkillPrompt?.SkillId == Watch) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger or DecisionKind.RespondDodge or DecisionKind.RespondSlash }) throw new InvalidOperationException("Unexpected Yuan Shu boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The bounded native Yuan Shu fixture missed its expected real boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the paid original Yongsi/awakening frame, issued Lord qualification and native current-turn modifier."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).MaxHp, V(i).HandCount }) })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 42 }, r);
            var s = new Scene(r, g); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-yuan-shu", new(1, 0, 0));
        internal static string Rules() => $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
         {"id":"{{Defense}}","revision":1,"viewAs":[{"id":"heart-dodge","inputKinds":[],"inputSuits":["heart"],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
         {"id":"{{Driver}}","revision":1,"activations":[
          {"id":"lose-recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"recover","target":"owner","amount":1}]},
          {"id":"self-damage-recover","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1},{"op":"recover","target":"owner","amount":1}]},
          {"id":"wound-recover-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1},{"op":"recover","target":"selectedTarget","amount":1}]},
          {"id":"remove-jijiang","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["classic:jijiang"],"sourceBind":"standard:none"}]},
          {"id":"plain-jijiang","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["classic:jijiang"]}]}
         ]},
         {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-yongsi","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:yongsi-mobile.SelectAndMoveOwnedCard"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation() => JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "原生体力与技能变化", description = "实际失血伤害回复及失去/普通重获主公技" }, [Defense] = new { name = "实体闪响应", description = "红心手牌作为真实闪响应" }, [Watch] = new { name = "庸肆实际付款子窗", description = "在真实卡牌移动后停留原始付款帧", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:liu-bei"); var c = SkillProgramCatalog.Load(Rules(), Presentation()); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "界袁术真实回合", "supporter", Yongsi, "qun", 4, AdditionalSkillIds: [Jixi, Driver, Watch]));
            var peers = new[] { 0, 2, 3, 4 }.Select(i => "fixture:boundary-yuan-shu-peer-" + i).ToArray();
            foreach (var seat in new[] { 0, 2, 3, 4 }) b.AddGeneral(new("fixture:boundary-yuan-shu-peer-" + seat, "固定实际角色", "supporter", Driver, seat switch { 0 => "shu", 2 => "wei", 3 => "wu", _ => "qun" }, 8, AdditionalSkillIds: seat == 0 ? ["classic:jijiang", Defense] : [Defense]));
            const string deck = "fixture:boundary-yuan-shu-deck"; b.AddDeck(new(deck, "固定真实杀实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 180).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界袁术实际身份边界", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
