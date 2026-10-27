using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheSourceExecutionerChecks
{
    private const string Yishi = StandardShanheSourceExecutionerPackage.YishiSkillId;
    private const string Huanfu = StandardShanheSourceExecutionerPackage.HuanfuSkillId;
    private const string Driver = "fixture:executioner-driver", Multi = "fixture:executioner-multi", Watch = "fixture:executioner-paid-child", Deck = "fixture:executioner-deck";

    public static void YishiPreventsActualSlashAndAcquiresTheOriginalTargetEntity()
    {
        var s = Scene.Start(Profile.Yishi); var equipment = s.V(1).Equipment.Single().Id;
        var slash = s.Slash([1]);
        s.Until(() => s.P?.SkillPrompt?.SkillId == Yishi);
        Need(s.V(1).Hp == 20 && s.Events<SourceExecutionerYishiPreventedEvent>().Length == 0,
            "Source611 offers on the original positive Slash damage before preventing or acquiring anything.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Yishi && s.P.Choices.Any(c => c.Cards.Contains(equipment)));
        s.Answer(c => c.Cards.Contains(equipment)); s.Play();
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        Need(s.Events<SourceExecutionerYishiPreventedEvent>() is [{ OwnerSeat: 0, TargetSeat: 1, Amount: 1 } fact] && fact.UseId == use &&
            s.Events<DamageAppliedEvent>().All(e => e.SourceCardUseResolutionId != use) && s.V(1).Hp == 20 &&
            s.Zone(equipment) == CardLocation.Hand(0) &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Equipment(1) && m.To == CardLocation.Hand(0)) == 1,
            "Accepted Yishi prevents precisely the original Slash segment and obtains the actual chosen equipment once.");
        s.AssertSlashPaid(slash, use);
        var decline = Scene.Start(Profile.Yishi); var declined = decline.Slash([1]); decline.Play();
        Need(decline.V(1).Hp == 19 && decline.Events<SourceExecutionerYishiPreventedEvent>().Length == 0 && decline.V(1).Equipment.Count == 1 &&
            decline.Events<DamageAppliedEvent>().Single().Amount == 1, "Declining leaves the real Slash damage and original target equipment unchanged.");
        var empty = Scene.Start(Profile.Empty); empty.Slash([1]); empty.Play();
        Need(empty.V(1).Hp == 19 && empty.Events<ProgramBindingStartedEvent>().All(e => e.SkillId != Yishi),
            "A target with no HEJ has no Yishi candidate; the original Slash applies normally.");
        var direct = Scene.Start(Profile.Yishi); direct.UseDriver(); direct.Play();
        Need(direct.V(1).Hp == 19 && direct.Events<ProgramBindingStartedEvent>().All(e => e.SkillId != Yishi) &&
            direct.Events<DamageAppliedEvent>().Single().SourceCardUseResolutionId is null,
            "A genuine independent program damage attempt is not a Slash-use qualification.");
    }

    public static void HuanfuPaysBothNativeWagersAndSettlesTheSameMultiTargetSlash()
    {
        var s = Scene.Start(Profile.Both); var actorCosts = s.V(0).Hand.Where(c => c.Kind != CardKind.Slash).Select(c => c.Id).ToArray();
        var targetCost = s.V(1).Equipment.Single().Id; var slash = s.Slash([1, 2]);
        s.Wager(0, actorCosts);
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var arm = s.Events<SourceExecutionerHuanfuPaidEvent>().Single();
        Need(arm.Amount == 2 && arm.Source.OwnerSeat == 0 && s.G.ResolutionStack.OfType<CardUseFrame>().Single().SourceExecutionerHuanfuBets is [{ Amount: 2, Settled: false }] &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Huanfu && f.SourceExecutionerHuanfu?.Stage == SourceExecutionerHuanfuStage.CostChildren),
            "A real HE discard pauses on its movement child while both the original Slash and its original wager frame retain the paid receipt.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Wager(1, [targetCost]); s.Play();
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        var paid = s.Events<SourceExecutionerHuanfuPaidEvent>(); var settled = s.Events<SourceExecutionerHuanfuSettledEvent>();
        Need(paid.Length == 2 && paid.All(e => e.UseId == use) && paid.Select(e => e.Source.OwnerSeat).Order().SequenceEqual([0, 1]) &&
            s.Events<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == use).Select(e => (e.SourceSeat, e.TargetSeat, e.Amount)).SequenceEqual([(0, 1, 1), (0, 2, 1)]) &&
            settled.Length == 2 && settled.Any(e => e.OwnerSeat == 0 && e.Wager == 2 && e.ActualDamage == 2 && e.RequestedDraw == 4) &&
            settled.Any(e => e.OwnerSeat == 1 && e.Wager == 1 && e.ActualDamage == 1 && e.RequestedDraw == 2) &&
            s.Events<SourceExecutionerHuanfuDrawReturnedEvent>().Select(e => (e.Requested, e.ActualCount)).Order().SequenceEqual([(2, 2), (4, 4)]) &&
            s.V(0).HandCount == 4 && s.V(1).HandCount == 2 && s.V(1).Hp == 19 && s.V(2).Hp == 19,
            "The original user totals the same Slash's original-target damage; the defending wager counts only that defender. Both native draws pay twice their own actual discard.");
        foreach (var id in actorCosts.Append(targetCost))
            Need(s.G.CardMovements.Count(m => m.CardId == id && m.Reason.Value == "skill-program.source-huanfu-3156.discard" && m.To == CardLocation.DiscardPile) == 1,
                "Each independently selected HE entity is actually discarded once despite native movement children.");
        s.AssertSlashPaid(slash, use);
    }

    public static void HuanfuFailedWagerRetainsRealDodgePaymentAndDrawsNothing()
    {
        var s = Scene.Start(Profile.Dodged); var dodge = s.V(1).Hand.Single().Id;
        var costs = s.V(0).Hand.Where(c => c.Kind != CardKind.Slash).Select(c => c.Id).ToArray(); var slash = s.Slash([1]);
        s.Wager(0, costs);
        s.Until(() => s.P?.Choices.Any(c => c.Cards.Contains(dodge)) == true);
        s.Answer(c => c.Cards.Contains(dodge)); s.Play();
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        Need(s.Events<SourceExecutionerHuanfuPaidEvent>() is [{ Amount: 2, Source.OwnerSeat: 0 }] &&
            s.Events<SourceExecutionerHuanfuSettledEvent>() is [{ Wager: 2, ActualDamage: 0, RequestedDraw: 0 }] &&
            s.Events<SourceExecutionerHuanfuDrawReturnedEvent>().Length == 0 && s.V(0).HandCount == 0 && s.V(1).Hp == 20 &&
            s.Events<CardRespondedEvent>().Any(e => e.CardId == dodge && e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) &&
            s.G.CardMovements.Count(m => m.CardId == dodge && m.To == CardLocation.Processing) == 1 && s.Zone(dodge) == CardLocation.DiscardPile &&
            s.Events<DamageAppliedEvent>().All(e => e.SourceCardUseResolutionId != use),
            "The failed two-card wager is still genuinely paid; the target's original Dodge is consumed by the native response and the zero-damage result draws nothing.");
        s.AssertSlashPaid(slash, use);

        var redirect = Scene.Start(Profile.Redirect); var wagerCard = redirect.V(0).Hand.First(c => c.Kind == CardKind.Peach).Id;
        var liuliCard = redirect.V(1).Hand.Single().Id; var redirectedSlash = redirect.Slash([1]);
        // Native PrepareProgramSlashTargets completes Liuli before issuing finalized-target wagers.
        redirect.Until(() => redirect.P?.SkillPrompt?.SkillId == "classic:liuli");
        redirect.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        redirect.Until(() => redirect.P?.Choices.Any(c => c.Targets.SequenceEqual([2])) == true);
        redirect.Answer(c => c.Targets.SequenceEqual([2]));
        redirect.Until(() => redirect.P?.Choices.Any(c => c.Cards.Contains(liuliCard)) == true);
        redirect.Answer(c => c.Cards.Contains(liuliCard));
        redirect.Wager(0, [wagerCard]); redirect.Play();
        var redirectedUse = redirect.Events<CardUseDeclaredEvent>().Single(e => e.CardId == redirectedSlash).ResolutionId;
        Need(redirect.Events<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == redirectedUse).Select(e => (e.SourceSeat, e.TargetSeat, e.Amount)).SequenceEqual([(0, 2, 1)]) &&
            redirect.V(1).Hp == 20 && redirect.V(2).Hp == 19 &&
            redirect.Events<SourceExecutionerHuanfuSettledEvent>() is [{ OwnerSeat: 0, Wager: 1, ActualDamage: 1, RequestedDraw: 2 }] &&
            redirect.Events<SourceExecutionerHuanfuDrawReturnedEvent>() is [{ Requested: 2, ActualCount: 2 }] &&
            redirect.G.CardMovements.Count(m => m.CardId == liuliCard && m.From == CardLocation.Hand(1) && m.To == CardLocation.DiscardPile) == 1,
            "Native Liuli changes the original Slash's actual target: its user's wager counts that same Slash damage while the real redirect cost is paid once.");
        redirect.AssertSlashPaid(redirectedSlash, redirectedUse);
    }

    private enum Profile { Yishi, Empty, Both, Dodged, Redirect }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, (r.Error?.Message ?? "Executioner native command rejected.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> select)
        { var p = P!; var c = p.Choices.FirstOrDefault(select); Need(c is not null, "Missing exact native choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public int Slash(int[] targets)
        {
            var card = V(0).Hand.Single(c => c.Kind == CardKind.Slash);
            var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Need(a is not null, "The native physical Slash must independently permit these targets. " + Diagnostic());
            Accept(new PlayCardCommand(0, card.Id, a!.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind)); return card.Id;
        }
        public void UseDriver() => Accept(new UseProgramSkillCommand(0, Driver, "hurt", [], [1], G.Revision, P!.PromptId));
        public void Wager(int seat, int[] cards)
        {
            Until(() => P?.SkillPrompt?.SkillId == Huanfu && P.PlayerSeat == seat && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Until(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-huanfu-wager") == true);
            foreach (var id in cards) Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([id]));
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "pay");
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 240; step++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "Executioner fixture stays within a short live native action. " + Diagnostic());
                if (P is { } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" || c.Parameters.GetValueOrDefault("option-id") == "continue");
                    Need(c is not null, "Unexpected native executioner boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Executioner exceeded bounded native progression. " + Diagnostic());
        }
        public void AssertSlashPaid(int id, long use) => Need(
            G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use && e.CardId == id) == 1 &&
            Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use && e.CardId == id) == 1 &&
            !G.ResolutionStack.Any(f => f.Id == use), "One original Slash is paid, declared, finished and cleaned up exactly once.");
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
            Pending = P, Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Recent = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });

        public static Scene Start(Profile profile)
        {
            var wager = profile is Profile.Both or Profile.Dodged or Profile.Redirect;
            string[] ownerSkills = wager ? [Huanfu, Multi, Watch, Driver] : [Yishi, Driver];
            string[] targetSkills = profile == Profile.Both ? [Huanfu, Watch] : profile == Profile.Redirect ? ["classic:liuli"] : [];
            var enemies = Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition("classic:guan-yu", MaxHpBonus: 16, Hp: 20,
                ExtraSkillIds: seat == 1 ? targetSkills : [])
            { StartingHand = seat == 1 && profile == Profile.Dodged ? [new(CardKind.Dodge, Suit.Club, 2)] : seat == 1 && profile == Profile.Redirect ? [new(CardKind.Peach, Suit.Heart, 8)] : [],
                StartingEquipment = seat == 1 && (profile == Profile.Yishi || profile == Profile.Both)
                    ? new[] { new ShanheStartingCard(CardKind.Crossbow, Suit.Club, 1) } : Array.Empty<ShanheStartingCard>() }).ToArray();
            var setup = new ShanheBattleSetup(191, 1, "fixture:source-executioner", "classic:guan-yu", 4, 0, 0, 0, 0, ownerSkills,
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Alcohol, Suit.Diamond, 9)],
                wager ? [new(CardKind.FangtianHalberd, Suit.Diamond, 12)] : [], enemies, [], "fixture:source-executioner");
            var dependencySetup = setup with { ExtraSkillIds = ownerSkills.Where(s => !s.StartsWith("fixture:", StringComparison.Ordinal)).ToArray(),
                EnemyRoster = enemies.Select(e => e with { ExtraSkillIds = e.ExtraSkillIds!.Where(s => !s.StartsWith("fixture:", StringComparison.Ordinal)).ToArray() }).ToArray() };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(dependencySetup).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:executioner", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
            var skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new[] { new { id = "hurt", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                    targetKind = "otherLiving", usesPerTurn = 1, effects = new[] { new { op = "damage", target = "selectedTarget", amount = 1 } } } } },
                new { id = Multi, revision = 1, modifiers = new[] { new { id = "one-extra-slash-target", query = "cardTargetCount", operation = "add", value = 1, priority = 0,
                    cardKinds = new[] { "slash", "fireSlash", "thunderSlash", "coldSlash" }, condition = new { kind = "ownPlayPhase" } } } },
                new { id = Watch, revision = 1, triggers = new[] { "hand", "equipment" }.Select(zone => new
                { id = "paid-" + zone, window = "cardsMoved", subject = "owner", optional = false,
                    sourceZones = new[] { zone }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.source-huanfu-3156.discard" },
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } }
                }).ToArray() }
            };
            var labels = new Dictionary<string, object>
            { [Driver] = new { name = "独立原生伤害", description = "独立原生技能伤害。" }, [Multi] = new { name = "原生多目标", description = "本阶段的杀额外一个合法目标。" },
                [Watch] = new { name = "实际弃牌孩子", description = "实际宦浮弃牌后返回原付款帧。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
            { Program = p, ProgramPresentation = programs.Presentations[id] });
            b.AddDeck(new(Deck, "零自然起手摸牌的真实实体牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:executioner-human-setup";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions o, ModeConfigurationValidationPhase p) => new StandardModeSetupFactory().ValidateInitialOptions(o, p);
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(c));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:executioner-human-profile";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(c.Options! with { HumanSeats = null },
            c.ModeDefinition!, c.ContentRegistry, new ModeHostRegistry().Register<IModeSetupRandom>(() => c.GetHost<IModeSetupRandom>())
                .Register<IModeNationalRevealHost>(() => c.GetHost<IModeNationalRevealHost>()).Register<IModeSetupEquipmentHost>(() => c.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers() { var p = inner.BuildPlayers(); return new(p.Seats.Select(s => s with { IsHuman = true }), p.AvailableGeneralIds,
            p.SelectionOrder, p.ShenwuSharedGodIds, p.ShenwuLeaderGeneralId, p.GuanduConfiguration, p.InitialActorSeat); }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest r) => inner.GetGeneralOffers(r);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> d) => inner.PrepareDeck(d);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> d) => inner.ProjectDeckCardNames(d);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> d, IReadOnlyList<ModeSeatSetupFacts> s) => inner.PrepareSelectedEquipment(d, s);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts s, string id) => inner.PrepareSelectedGeneral(s, id);
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
