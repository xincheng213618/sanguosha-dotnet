using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileTaDunChecks
{
    private const string Skill = "mobile:luanzhan", Driver = "fixture:mobile-ta-dun-driver", Owner = "fixture:mobile-ta-dun-owner",
        Mode = "identity:classic-mobile-ta-dun-fixture", Deck = "fixture:mobile-ta-dun-deck";

    public static void OnlyOutgoingDamageEventsGainOneIncludingSelfDamage()
    {
        var s = Scene.Start(); s.Play();
        var ownHp = s.V(0).Hp; var peerHp = s.V(1).Hp;
        s.Drive("incoming-two", [1]); s.Play();
        s.Need(s.V(0).Hp == ownHp - 2 && s.Count() == 0 && s.Facts<DamageTargetBudgetGainedEvent>().Length == 0,
            "A real incoming two-point damage occurrence does not grant mobile Luanzhan marks.");
        s.Drive("outgoing-two", [1]); s.Play();
        s.Need(s.V(1).Hp == peerHp - 2 && s.Count() == 1 && s.Facts<DamageTargetBudgetGainedEvent>() is [{ DamageSourceSeat: 0, DamageTargetSeat: 1, DamageAmount: 2 }],
            "One real outgoing two-point damage event grants exactly one mark rather than one mark per HP.");
        s.Drive("self-two"); s.Play();
        s.Need(s.V(0).Hp == ownHp - 4 && s.Count() == 2 && s.Facts<DamageTargetBudgetGainedEvent>().Last() is { DamageSourceSeat: 0, DamageTargetSeat: 0, DamageAmount: 2 },
            "Self-damage is still damage actually caused by the owner and grants once for that same event.");
    }

    public static void ShortfallClearsAllWhileThresholdAndOlDefaultKeepTheirRulesCold()
    {
        foreach (var (mobile, added, removed, remaining) in new[] {
            (true, new[] { 2 }, 3, 0), (false, new[] { 2 }, 2, 1), (true, new[] { 2, 3 }, 0, 3) })
        {
            var s = Scene.Start(mobile); s.Play();
            for (var i = 0; i < 3; i++) { s.Drive("outgoing-two", [1]); s.Play(); }
            s.Need(s.Count() == 3, "Three actual outgoing occurrences bank three real marks.");
            s.Native(CardKind.IronChain); s.Budget();
            var offered = s.Facts<DamageTargetBudgetOfferedEvent>().Single();
            s.Need(offered.Receipt.FrozenBudget == 3 && offered.Receipt.OriginalTargets.SequenceEqual([1]),
                "The existing receipt freezes the original paid black ordinary-trick use and its three-target extra allowance.");
            foreach (var target in added)
            {
                s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "add" && c.Targets.SequenceEqual([target]));
                s.Budget();
                if (mobile && removed == 3) s.Cold();
            }
            s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "done"); s.Play();
            var settled = s.Facts<DamageTargetBudgetSettledEvent>().Single();
            s.Need(settled.CardUseFrameId == offered.Receipt.CardUseFrameId && settled.ActionId == offered.Receipt.ActionId &&
                settled.FinalTargetCount == added.Length + 1 && settled.MarkersBeforeSettlement == 3 && settled.Removed == removed &&
                settled.NewCount == remaining && s.Count() == remaining && Enumerable.Range(0, 4)
                    .All(seat => s.V(seat).IsChained == (seat == 1 || added.Contains(seat))) &&
                s.Facts<DamageTargetBudgetResolvedEvent>().Count(e => e.ProgramFrameId == offered.ProgramFrameId) == 1,
                "The same native trick chains exactly its ordered real targets once: mobile shortfall clears all, equality retains all, and omitted OL policy still removes half rounded up.");
        }

        var slash = Scene.Start(slash: true); slash.Play(); slash.Drive("outgoing-two", [1]); slash.Play();
        slash.Drive("wide"); slash.Play(); var hp1 = slash.V(1).Hp; var hp2 = slash.V(2).Hp;
        slash.Native(CardKind.Slash); slash.Budget();
        slash.Answer(c => c.Parameters.GetValueOrDefault("branch") == "add" && c.Targets.SequenceEqual([2])); slash.Budget();
        slash.Answer(c => c.Parameters.GetValueOrDefault("branch") == "done"); slash.Play();
        slash.Need(slash.V(1).Hp == hp1 - 1 && slash.V(2).Hp == hp2 - 1 && slash.Count() == 3 &&
            slash.Facts<DamageTargetBudgetSettledEvent>() is [{ FinalTargetCount: 2, Removed: 0 }],
            "The existing typed return resumes one paid native Slash across both real targets, and only its subsequent two actual damage events add fresh marks.");
    }

    public static void LoaderRejectsUnknownShortfallAndPolicyOnGain()
    {
        foreach (var onGain in new[] { false, true })
        {
            var rules = JsonNode.Parse(Embedded("rules.json"))!;
            var effects = rules["skills"]![0]!["triggers"]![onGain ? 0 : 1]!["effects"]![0]!;
            effects["shortfallPolicy"] = onGain ? "removeAll" : "discardSome";
            try { SkillProgramCatalog.Load(rules.ToJsonString(), Embedded("presentation.json")); }
            catch (InvalidOperationException error) when (error.Message.Contains("shortfallPolicy", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Damage budget loader must reject an unknown policy and a shortfall policy on its gain node.");
        }
    }

    private static string Embedded(string suffix)
    {
        using var stream = typeof(StandardClassicGeneralPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.mobile-ta-dun." + suffix)!;
        using var reader = new StreamReader(stream); return reader.ReadToEnd();
    }
    private static string Peer(int seat) => "fixture:mobile-ta-dun-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Count() => (V(0).Markers ?? []).SingleOrDefault(m => m.Kind == PlayerMarkerKind.LuanZhan)?.Count ?? 0;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Budget() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "damage-target-budget") == true);
        public void Drive(string activation, IReadOnlyList<int>? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public void Native(CardKind kind)
        { var card = V(0).Hand.First(c => c.Kind == kind); Accept(new PlayCardCommand(0, card.Id, [1], G.Revision, P!.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification })
                { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage").Id, G.Revision)); continue; }
                Need(P is null, "Unexpected native mobile Luanzhan boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small seed-7 Luanzhan scene missed its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold keeps the original paid use, frozen allowance, already appended native target and unresolved final debit.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P)); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Luanzhan command."); }
        public static Scene Start(bool mobile = true, bool slash = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile, slash));
            var g = GameEngine.CreateStandard(new() { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture(bool mobile, bool slash) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-ta-dun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, mobile ? "mobile:ta-dun" : "ol:ta-dun");
            var c = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
              {"id":"outgoing-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
              {"id":"incoming-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]},
              {"id":"self-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":2}]},
              {"id":"wide","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"attackRange","ruleOperation":"unlimited"}]}
            ]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实伤害驱动", description = "实际伤害事件与原生范围准备。" } } }));
            b.AddSkill(new(Driver, "真实伤害驱动", "实际伤害事件与原生范围准备。") { Program = c.Programs[Driver], ProgramPresentation = c.Presentations[Driver] });
            b.AddGeneral(new(Owner, "乱战实际持有者", "supporter", mobile ? Skill : "ol:luanzhan", "qun", 20, [Driver]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实用牌目标", "supporter", "standard:none", "wei", 20));
            b.AddDeck(new(Deck, "固定黑色原生牌", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard(slash ? "standard:slash" : "standard:iron_chain", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "移动乱战真实目标与扣标", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
