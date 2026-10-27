using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheSourceWhiteHorseChecks
{
    private const string Skill = StandardShanheSourceWhiteHorsePackage.XiyingSkillId;
    private const string Mingce = "classic:mingce";
    private const string Watch = "fixture:white-horse-reward-child", Deck = "fixture:white-horse-deck";
    private const string RewardReason = "skill-program.source-xiying-1180.reward";

    public static void PaidXiyingDiscardsBansNativeUseAndResponseAndRewardsAtActualEnding()
    {
        // Without the actual payment, the exact holder can really use this original Slash.
        var baseline = Scene.Start(); baseline.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); baseline.OwnPlay();
        var baselineSlash = baseline.V(3).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var baselineWeapon = baseline.V(3).Equipment.Single().Id; baseline.UseBorrowedSword();
        baseline.Until(() => baseline.P is { PlayerSeat: 3 } p && p.Choices.Any(c => c.Cards.SequenceEqual([baselineSlash])));
        baseline.Answer(c => c.Cards.SequenceEqual([baselineSlash])); baseline.OwnPlay();
        Need(baseline.Events<CardUseDeclaredEvent>().Count(e => e.CardId == baselineSlash && e.SourceSeat == 3) == 1 &&
            baseline.Events<CardUseFinishedEvent>().Count(e => e.CardId == baselineSlash) == 1 && baseline.Zone(baselineWeapon) == CardLocation.Equipment(3),
            "The unbanned native Borrowed Sword holder genuinely pays, uses and finishes its original Slash; the weapon is not given away by the baseline fixture.");
        baseline.BeginMingceGift();
        baseline.Until(() => baseline.P is { PlayerSeat: 3 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("offer-option") == "slash"));
        baseline.Answer(c => c.Parameters.GetValueOrDefault("offer-option") == "slash");
        baseline.Until(() => baseline.P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Targets.SequenceEqual([2])));
        baseline.Answer(c => c.Targets.SequenceEqual([2])); baseline.OwnPlay();
        Need(baseline.Events<CardUseDeclaredEvent>().Count(e => e.CardId == 0 && e.CardKind == CardKind.Slash && e.SourceSeat == 3) == 1,
            "The same unbanned recipient can genuinely choose and complete the existing Mingce virtual Slash Use during another player's actual turn.");

        var s = Scene.Start(); var cost = s.V(0).Hand.Single(c => c.Kind == CardKind.Duel).Id;
        var bannedSlash = s.V(3).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var bannedDodge = s.V(1).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        var keptWeapon = s.V(3).Equipment.Single().Id;
        s.ActivateAndAnswerOthers(cost);
        var paid = s.Events<ShanheSourceXiyingPaidEvent>().Single();
        Need(s.Events<ShanheSourceXiyingTargetPaidEvent>() is [{ TargetSeat: 2 }] &&
            s.Events<CardTargetRestrictionGrantedEvent>().Where(e => e.Restriction.Source.SkillId == Skill &&
                e.Restriction.Restriction == SkillProgramCardTargetRestriction.CannotUseOrPlayForTurn).Select(e => e.Restriction.TargetSeat).Order().SequenceEqual(new int?[] { 1, 3 }) &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.V(0).Skills!.All(k => k.ContentId != "guandu:xiying") && Enumerable.Range(1, 3).Any(i => s.V(i).TeamId == s.V(0).TeamId),
            "The standalone Shanhe source pays its non-basic hand entity; all three other roles, including its ally, independently discard or receive the shared real turn ban without Guandu mode.");
        var borrowed = s.V(0).Hand.Single(c => c.Kind == CardKind.BorrowedSword).Id;
        var beforeRejected = s.G.Revision;
        Need(s.G.GetHumanLegalActions().All(a => a.CardId != borrowed || a.TargetSeats.FirstOrDefault(-1) != 3) &&
            !s.G.Submit(new PlayCardCommand(0, borrowed, [3, 2], s.G.Revision, s.P!.PromptId)).Accepted && s.G.Revision == beforeRejected &&
            s.G.CardMovements.All(m => m.CardId != borrowed || m.From != CardLocation.Hand(0)) && s.Zone(keptWeapon) == CardLocation.Equipment(3),
            "The native Use ban also removes Borrowed Sword's initial holder qualification, so an unpublished banned-holder pair is rejected before any card payment or weapon transfer.");
        var recipientHand = s.V(3).HandCount;
        var gift = s.BeginMingceGift(); s.OwnPlay(forbidSeat: 3, forbidCards: [bannedSlash]);
        Need(s.Zone(gift) == CardLocation.Hand(3) && s.V(3).HandCount == recipientHand + 2 &&
            s.G.CardMovements.Count(m => m.CardId == gift && m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(3)) == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(3)) == 1 &&
            s.Zone(bannedSlash) == CardLocation.Hand(3) && s.Events<CardUseDeclaredEvent>().All(e => e.SourceSeat != 3),
            "The real Mingce gift still moves its original entity to the banned recipient, but the native offer can only draw once instead of using the Slash that the same baseline recipient really used.");
        var slash = s.UseSlashOnOne(); s.OwnPlay(forbidSeat: 1, forbidCards: [bannedDodge]);
        var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        Need(s.Events<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == use) is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            s.Zone(bannedDodge) == CardLocation.Hand(1) && s.Events<CardRespondedEvent>().All(e => e.CardId != bannedDodge) &&
            s.Events<ShanheSourceXiyingRewardEvent>().Length == 0,
            "The banned target's original Dodge cannot answer a genuine paid Slash. Actual Play damage qualifies the receipt but awards nothing during Play.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var reward = s.Events<ShanheSourceXiyingRewardEvent>().Single();
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        Need(parent.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c &&
            parent.ShanheSourceXiying is { Stage: ShanheSourceXiyingStage.RewardChildren } r && r.PaidFrameId == paid.FrameId &&
            s.G.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Any(f => f.Id == c.ParentFrameId) &&
            reward.PaidFrameId == paid.FrameId && reward.CardId is not null &&
            s.G.CardMovements.Count(m => m.CardId == reward.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == RewardReason) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use && e.CardId == slash) == 1,
            "Only the actual Ending issues the one random original deck entity. Its movement child stays on the exact reward producer and original paid receipt after the original Slash has already finished once.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.NextHuman();
        Need(s.Events<ShanheSourceXiyingRewardEvent>().Length == 1 && s.Events<ShanheSourceXiyingPaidEvent>().Length == 1 &&
            !s.G.ResolutionStack.Any(f => f.Id == parent.Id) && s.V(0).HandCount == 3,
            "The genuine reward child returns once without paying again or duplicating the earned original card.");

        // Seat one accepted the turn-zero ban. Its next real turn can Use a Peach,
        // and the formerly banned seat three can answer an actual group response.
        var peach = s.V(1).Hand.Single(c => c.Kind == CardKind.Peach).Id;
        s.PlayPeerCard(peach, self: true); s.PeerPlay();
        Need(s.Events<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 1 && e.CardId == peach && e.CardKind == CardKind.Peach) && s.V(1).Hp == 20,
            "The existing turn-boundary cleanup restores actual card Use to the previously banned player without removing a source skill or starting a replacement game.");
        var aoe = s.V(1).Hand.Single(c => c.Kind == CardKind.BarbarianAssault).Id;
        s.PlayPeerCard(aoe); s.Until(() => s.P is { PlayerSeat: 3 } p && p.Choices.Any(c => c.Cards.SequenceEqual([bannedSlash])));
        s.Answer(c => c.Cards.SequenceEqual([bannedSlash])); s.PeerPlay();
        Need(s.Events<CardRespondedEvent>().Count(e => e.CardId == bannedSlash && e.ResponderSeat == 3) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == bannedSlash && m.From == CardLocation.Hand(3) && m.To == CardLocation.Processing) == 1 &&
            s.Zone(bannedSlash) == CardLocation.DiscardPile && s.Events<CardUseFinishedEvent>().Count(e => e.CardId == aoe) == 1,
            "The same formerly banned original Slash can genuinely pay the next-turn native group response, proving response cleanup as well as active Use cleanup.");

        var peaceful = Scene.Start(); peaceful.ActivateAndAnswerOthers(peaceful.V(0).Hand.Single(c => c.Kind == CardKind.Duel).Id);
        peaceful.Accept(new EndPlayPhaseCommand(0, peaceful.G.Revision, peaceful.P!.PromptId)); peaceful.NextHuman();
        Need(peaceful.Events<ShanheSourceXiyingPaidEvent>().Length == 1 && peaceful.Events<ShanheSourceXiyingRewardEvent>().Length == 0 &&
            peaceful.Events<ProgramBindingStartedEvent>().All(e => e.SkillId != Skill || e.BindingId != "paid-play-ending-reward"),
            "A paid activation with no actual own-Play damage still applies its decisions but never creates a free Ending reward.");

        var earlier = Scene.Start(extraPlay: true);
        earlier.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); earlier.OwnPlay();
        var actualTurn = earlier.G.State.TurnNumber;
        var earlierSlash = earlier.UseSlashOnOne(); earlier.OwnPlay();
        var earlierUse = earlier.Events<CardUseDeclaredEvent>().Single(e => e.CardId == earlierSlash).ResolutionId;
        Need(earlier.Events<DamageAppliedEvent>().Single(e => e.SourceCardUseResolutionId == earlierUse) is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            earlier.Events<ShanheSourceXiyingPaidEvent>().Length == 0,
            "The real Dangxian extra Play first skips Xiying and genuinely damages a target who actually passes its available Dodge, before any Xiying payment.");
        earlier.Accept(new EndPlayPhaseCommand(0, earlier.G.Revision, earlier.P!.PromptId));
        earlier.Until(() => earlier.P?.SkillPrompt?.SkillId == Skill && earlier.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        Need(earlier.G.State.TurnNumber == actualTurn && earlier.G.State.CurrentSeat == 0 &&
            earlier.Events<PhaseChangedEvent>().Count(e => e.ActorSeat == 0 && e.Phase == TurnPhase.Play) == 2,
            "The earlier real extra Play returns through normal preparation to a second actual Play inside the same original turn.");
        earlier.ActivateAndAnswerOthers(earlier.V(0).Hand.Single(c => c.Kind == CardKind.Duel).Id);
        earlier.Accept(new EndPlayPhaseCommand(0, earlier.G.Revision, earlier.P!.PromptId));
        earlier.Until(() => earlier.P?.SkillPrompt?.SkillId == Watch);
        var earlyPaid = earlier.Events<ShanheSourceXiyingPaidEvent>().Single();
        var earlyReward = earlier.Events<ShanheSourceXiyingRewardEvent>().Single();
        var facts = earlier.G.Events.Select(e => e.Payload).ToArray();
        Need(earlyReward.PaidFrameId == earlyPaid.FrameId && earlyReward.CardId is not null &&
            earlier.Events<DamageAppliedEvent>().Length == 1 &&
            Array.FindIndex(facts, e => e is DamageAppliedEvent) < Array.FindIndex(facts, e => e is ShanheSourceXiyingPaidEvent) &&
            earlier.G.CardMovements.Count(m => m.CardId == earlyReward.CardId && m.From == CardLocation.DrawPile &&
                m.To == CardLocation.Hand(0) && m.Reason.Value == RewardReason) == 1,
            "The actual Ending awards its original deck entity from earlier own-Play damage in this same turn, even though that damage preceded payment and no later Play damage occurred.");
        earlier.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); earlier.NextHuman();
        Need(earlier.Events<ShanheSourceXiyingPaidEvent>().Length == 1 && earlier.Events<ShanheSourceXiyingRewardEvent>().Length == 1,
            "The earlier-Play reward child returns to the same Ending without issuing another payment or reward.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, (r.Error?.Message ?? "White-horse native command rejected.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; var c = p.Choices.FirstOrDefault(choose); Need(c is not null, "Missing exact native source1180 choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void ActivateAndAnswerOthers(int cost)
        {
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Until(() => P is { PlayerSeat: 0 } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-xiying"));
            Need(P!.Choices.All(c => c.Cards.Count == 1 && V(0).Hand.Single(h => h.Id == c.Cards.Single()).Kind != CardKind.Slash && V(0).Hand.Single(h => h.Id == c.Cards.Single()).Kind != CardKind.Dodge),
                "Only real non-basic hand entities can pay the source1180 initial cost.");
            Answer(c => c.Cards.SequenceEqual([cost]));
            foreach (var seat in new[] { 1, 2, 3 })
            {
                Until(() => P is { } p && p.PlayerSeat == seat && p.SkillPrompt?.SkillId == Skill);
                if (seat == 2) { var id = V(2).Hand.Single(c => c.Kind == CardKind.Slash).Id; Answer(c => c.Cards.SequenceEqual([id])); }
                else Answer(c => c.Parameters.GetValueOrDefault("branch") == "ban");
            }
            OwnPlay();
        }
        public void UseBorrowedSword()
        {
            var card = V(0).Hand.Single(c => c.Kind == CardKind.BorrowedSword);
            var a = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.Count == 2 && a.TargetSeats[0] == 3 && a.TargetSeats[1] != 0);
            Accept(new PlayCardCommand(0, card.Id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind));
        }
        public int BeginMingceGift()
        {
            var gift = V(0).Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
            Accept(new UseProgramSkillCommand(0, Mingce, "offer-command", [], [3], G.Revision, P!.PromptId));
            Until(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
            Answer(c => c.Cards.SequenceEqual([gift])); return gift;
        }
        public int UseSlashOnOne()
        {
            var card = V(0).Hand.Single(c => c.Kind == CardKind.Slash);
            var a = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, card.Id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind)); return card.Id;
        }
        public void PlayPeerCard(int id, bool self = false)
        {
            var p = P!; Need(p.PlayerSeat == 1 && p.Kind == DecisionKind.PlayCard, "The next native human owns this Play command.");
            var c = p.Choices.First(c => c.Cards.SequenceEqual([id]) && (!self || c.Targets.Count == 0 || c.Targets.SequenceEqual([1])));
            Accept(new PlayCardCommand(1, id, c.Targets, G.Revision, p.PromptId));
        }
        public void OwnPlay(int? forbidSeat = null, int[]? forbidCards = null) => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0, forbidSeat, forbidCards);
        public void PeerPlay() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 } && G.ResolutionStack.Count == 0);
        public void NextHuman() => PeerPlay();
        public void Until(Func<bool> stop, int? forbidSeat = null, int[]? forbidCards = null)
        {
            for (var step = 0; step < 260; step++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "White-horse check must stay in short live native turns. " + Diagnostic());
                if (P is { } p)
                {
                    if (p.PlayerSeat == forbidSeat) Need(p.Choices.All(c => !c.Cards.Intersect(forbidCards ?? []).Any()), "The real banned original entity was offered by native Use/Response. " + Diagnostic());
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" ||
                        c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "borrowed-sword-give-weapon");
                    Need(c is not null, "Unexpected native source1180 boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("White-horse exceeded bounded native progression. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
            Pending = P, Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Recent = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool extraPlay = false)
        {
            ShanheCombatantDefinition Other() => new("classic:guan-yu", MaxHpBonus: 16, Hp: 20)
            { StartingHand = [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Slash, Suit.Spade, 5), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.BarbarianAssault, Suit.Spade, 11)],
                StartingEquipment = [new(CardKind.FangtianHalberd, Suit.Diamond, 12)] };
            var extraSkills = extraPlay ? new[] { Watch, Mingce, "classic:dangxian" } : [Watch, Mingce];
            var setup = new ShanheBattleSetup(227, 1, "fixture:source-white-horse", StandardShanheSourceWhiteHorsePackage.BaseId, 4, 0, 0, 0, 0, extraSkills,
                [new(CardKind.Duel, Suit.Diamond, 1), new(CardKind.BorrowedSword, Suit.Club, 12), new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Club, 8), new(CardKind.Crossbow, Suit.Club, 1)],
                [], [Other(), Other()], [Other()], "fixture:source-white-horse") { ExtraSkillSlots = extraSkills.Length };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = extraSkills.Where(id => id != Watch).ToArray() }).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry));
            s.Accept(new StartGameCommand()); s.Until(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:white-horse", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
            var program = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new
            { id = Watch, revision = 1, triggers = new[] { new { id = "original-reward-movement", window = "cardsGained", subject = "owner", optional = false,
                destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { RewardReason },
                effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Watch] = new { name = "原收益移牌孩子", description = "返回同一个袭营结束阶段收益。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            b.AddSkill(new(Watch, "原收益移牌孩子", "返回同一个袭营结束阶段收益。") { Program = program.Programs[Watch], ProgramPresentation = program.Presentations[Watch] });
            b.AddDeck(new(Deck, "零自然摸牌的实际收益牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(i => new ContentDeckPhysicalCard(i % 2 == 0 ? "standard:slash" : "standard:duel", Suit.Spade, i % 13 + 1)).ToArray() });
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:white-horse-human-setup"; public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions o, ModeConfigurationValidationPhase p) => new StandardModeSetupFactory().ValidateInitialOptions(o, p);
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(c));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:white-horse-human-profile"; public string ModeId => ShanheBattleSetup.BattleModeId;
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
