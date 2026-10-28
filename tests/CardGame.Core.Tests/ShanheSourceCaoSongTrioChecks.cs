using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceCaoSongTrioChecks
{
    private const string Chuanyun = StandardShanheSourceCaoSongTrioPackage.ChuanyunSkillId;
    private const string Longwei = StandardShanheSourceCaoSongTrioPackage.LongweiSkillId;
    private const string Weizhong = StandardShanheSourceCaoSongTrioPackage.WeizhongSkillId;
    private const string Archer = "fixture:caosong-source-archer", Rescuer = "fixture:caosong-source-rescuer";
    private const string Target = "fixture:caosong-source-target", Driver = "fixture:caosong-source-driver";
    private const string Watch = "fixture:caosong-source-gain-child", Deck = "fixture:caosong-source-deck";

    public static void Source665EnemyHpAnd1602PaidMaximumChildBeforeNegativeHpRescue()
    {
        EndingOnlySelectsEnemyWithStrictlyHigherHp();
        PaidMaximumRunsRealWeizhongChildThenRescuesOriginalNegativeHpAlly();
        MaximumHpMinimumCannotProduceFreeRescue();
    }

    private static void EndingOnlySelectsEnemyWithStrictlyHigherHp()
    {
        var s = Scene.Start(rescue: false);
        var higher = s.V().Players.Single(p => p.TeamId == ShanheBattleSetup.EnemyTeamId && p.Hp == 3).Seat;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Chuanyun);
        s.Activate(Chuanyun);
        s.Until(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true ||
            s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
        Need(s.P!.Choices.All(c => c.Targets.SequenceEqual([higher])),
            "Source665 offers only the higher-HP enemy: equal-HP enemies and a higher-HP friendly character are excluded.");
        s.Answer(c => c.Targets.SequenceEqual([higher]));
        s.Until(() => s.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == higher));
        var hit = s.E<DamageAppliedEvent>().Single(e => e.SourceSeat == 0 && e.TargetSeat == higher);
        Need(hit.Amount == 1 && s.Player(higher).Hp == 2,
            "Source665 deals one genuine damage from its original ending program after selecting its strict public enemy.");
    }

    private static void PaidMaximumRunsRealWeizhongChildThenRescuesOriginalNegativeHpAlly()
    {
        var s = Scene.Start(rescue: true);
        var enemy = s.V().Players.First(p => p.TeamId == ShanheBattleSetup.EnemyTeamId).Seat;
        s.Skill("wound", [enemy]);
        s.Play();
        // Lifecycle candidates that fail qualification still publish a typed
        // false/false skip receipt. That receipt is not a skill invocation.
        Need(!s.Player(enemy).IsAlive && s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == enemy && e.Amount == 5 && e.RemainingHp < 0) &&
            s.E<DyingResolvedEvent>().Any(e => e.VictimSeat == enemy && !e.Survived) &&
            s.E<ProgramBindingStartedEvent>().All(e => e.SkillId != Longwei) &&
            s.E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Longwei).All(e => !e.Activated && !e.Completed) &&
            s.E<MaximumHpChangedEvent>().All(e => e.SkillId != Longwei) &&
            s.E<RecoveryAppliedEvent>().All(e => e.TargetSeat != enemy),
            "An enemy entering negative-HP dying must really die, with no invocation, payment or recovery from friendly-only source1602. " + s.Diagnostic());
        var ally = s.V().Players.Single(p => p.Seat != 0 && p.TeamId == ShanheBattleSetup.PlayerTeamId).Seat;
        var before = s.Player(0).Hand.Count;
        s.Skill("wound", [ally]);
        s.Until(() => s.P?.SkillPrompt?.SkillId == Longwei);
        s.Activate(Longwei);
        s.Until(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual([ally])) == true);
        Need(s.P!.Choices.All(c => c.Targets.SequenceEqual([ally])),
            "Source1602 binds the single actual friendly dying victim, with no unrelated recovery target.");
        s.Answer(c => c.Targets.SequenceEqual([ally]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Longwei);
        Need(s.Player(0).MaxHp == 5 && s.Player(ally).Hp == -3 && s.Player(0).Hand.Count == before + 2 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Weizhong) &&
            s.E<RecoveryAppliedEvent>().All(e => e.TargetSeat != ally) &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != parent.Id),
            "The real one-point maximum payment first triggers source239 and its two-card gain child; the same Longwei frame and negative-HP victim wait without early recovery.");
        var maximum = s.E<MaximumHpChangedEvent>().Single(e => e.PlayerSeat == 0 && e.SkillId == Longwei);
        s.Play();
        var healed = s.E<RecoveryAppliedEvent>().Single(e => e.TargetSeat == ally);
        Need(maximum.Delta == -1 && s.Player(0).MaxHp == 5 && s.Player(ally).IsAlive && s.Player(ally).Hp == 1 && healed.Amount == 4 &&
            s.E<MaximumHpChangedEvent>().Count(e => e.PlayerSeat == 0 && e.SkillId == Longwei) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == parent.Id) == 1,
            "The real gain child returns to the original Longwei frame once, pays no second cost, and restores -3 HP to 1 rather than healing one point.");
    }

    private static void MaximumHpMinimumCannotProduceFreeRescue()
    {
        var s = Scene.Start(rescue: true);
        s.Skill("shrink", []); s.Play();
        Need(s.Player(0).MaxHp == 2, "The fresh native maximum-HP mutation leaves one final payable point.");
        var ally = s.V().Players.Single(p => p.Seat != 0 && p.TeamId == ShanheBattleSetup.PlayerTeamId).Seat;
        s.Skill("wound", [ally]);
        s.Until(() => s.P?.SkillPrompt?.SkillId == Longwei); s.Activate(Longwei);
        s.Until(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual([ally])) == true);
        s.Answer(c => c.Targets.SequenceEqual([ally])); s.Play();
        Need(s.Player(0).MaxHp == 1 && s.Player(ally).Hp == 1 &&
            s.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Longwei && e.Delta == -1) == 1,
            "A real payment from two MaxHp to one retains the original victim through its source239 gain child and still rescues to one.");
        s.Skill("wound", [ally]); s.Play();
        Need(!s.Player(ally).IsAlive && s.E<MaximumHpChangedEvent>().Count(e => e.SkillId == Longwei) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Longwei && e.Completed) == 1,
            "At the engine's one-MaxHp minimum there is no actual one-point payment, so source1602 cannot give a free rescue.");
    }

    private static void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public GameSnapshot V() => G.CreateSnapshot(0);
        public PlayerSnapshot Player(int seat) => V().Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            Players = V().Players.Select(p => new { p.Seat, p.TeamId, p.Hp, p.MaxHp, p.IsAlive }),
            Damage = E<DamageAppliedEvent>(), Dying = E<PlayerDyingEvent>(), ResolvedDying = E<DyingResolvedEvent>(),
            LongweiStarted = E<ProgramBindingStartedEvent>().Where(e => e.SkillId == Longwei),
            LongweiResolved = E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Longwei),
            Maximum = E<MaximumHpChangedEvent>(), Recovery = E<RecoveryAppliedEvent>()
        });
        public void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Fresh source2485 command rejected."); }
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick) ??
                throw new InvalidOperationException("Missing source2485 choice: " + JsonSerializer.Serialize(p));
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
        }
        public void Activate(string skill)
        {
            if (P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        }
        public void Skill(string activation, int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId));
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 180; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fresh source2485 scene ended before its focused boundary.");
                if (P is { PlayerSeat: 0 } p && p.SkillPrompt?.SkillId == Watch)
                { Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); continue; }
                Need(P is null || !Player(P.PlayerSeat).IsHuman, "Unexpected source2485 prompt: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Bounded source2485 scene missed its native boundary: " + JsonSerializer.Serialize(new
                { G.State.Phase, G.State.CurrentSeat, P, Frames = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }) }));
        }
        public static Scene Start(bool rescue)
        {
            ShanheStartingCard[] hands = Enumerable.Range(0, 12).Select(_ => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2)).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture:source-caosong-2485", rescue ? Rescuer : Archer,
                rescue ? 6 : 2, 0, 0, 0, 5, [], [], [],
                [new(Target, Hp: 3) { StartingHand = hands }, new(Target, Hp: 2) { StartingHand = hands }],
                [new(Target, Hp: rescue ? 2 : 4) { StartingHand = hands }], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceCaoSongTrioPackage.CaoSongHellId,
                EnemyRoster = setup.EnemyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray(),
                AllyRoster = setup.AllyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray()
            }).WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Archer, Rescuer, Target]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-caosong-trio", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
              {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                {"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"wound","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":5}]},
                  {"id":"shrink","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"changeMaximumHp","target":"owner","amount":-4}]}]},
                {"id":"{{{Watch}}}","revision":1,"triggers":[
                  {"id":"real-gain-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
              """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "真实伤害与上限付款入口", description = "制造实际伤害和体力上限变化。" },
                    [Watch] = new { name = "实际摸牌孩子", description = "等待后返回原有父帧。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var skill in catalog.Programs)
                builder.AddSkill(new(skill.Key, catalog.Presentations[skill.Key].Name, catalog.Presentations[skill.Key].Description) { Program = skill.Value });
            builder.AddGeneral(new(Archer, "源穿云", "supporter", Chuanyun, "qun", 2, [Driver]));
            builder.AddGeneral(new(Rescuer, "源龙威", "supporter", Longwei, "wei", 6, [Weizhong, Driver, Watch]));
            builder.AddGeneral(new(Target, "普通目标", "supporter", null, "qun", 4));
            builder.AddDeck(new(Deck, "小型实体牌堆", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
        }
    }
}
