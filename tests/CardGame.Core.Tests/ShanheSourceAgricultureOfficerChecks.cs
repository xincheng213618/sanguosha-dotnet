using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheSourceAgricultureOfficerChecks
{
    private const string Tuntian = StandardShanheSourceAgricultureOfficerPackage.TuntianSkillId;
    private const string Tiandu = StandardShanheSourceAgricultureOfficerPackage.TianduSkillId;
    private const string Reason = "skill.shanhe.source-tuntian-959", Deck = "fixture:source-agriculture-deck";

    public static void ActualOffTurnDodgeLossJudgesOnceAndSharesTheOriginalJudgmentWithTiandu()
    {
        foreach (var (suit, claim) in new[] { (Suit.Spade, false), (Suit.Heart, true), (Suit.Spade, true) })
        {
            var s = Scene.Start(suit); var dodge = s.V(1).Hand.Single().Id;
            Need(s.V(1).Hp == 3 && s.V(1).MaxHp == 3 && s.G.GetCombatDistance(1, 3) == 2,
                "The fresh source4871 body is 3/3 and starts without fields or an implicit distance bonus.");
            var slash = s.UseSlash();
            s.Until(() => s.P is { PlayerSeat: 1 } p && p.Choices.Any(c => c.Cards.Contains(dodge)));
            s.Answer(c => c.Cards.Contains(dodge));
            s.Until(() => s.P?.SkillPrompt?.SkillId == Tuntian);
            Need(s.G.CardMovements.Count(m => m.CardId == dodge && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing) == 1 &&
                s.G.State.CurrentSeat == 0 && s.Events<JudgmentResolvedEvent>().All(e => e.Reason != Reason),
                "A genuine off-turn native Dodge payment creates one optional owned-loss trigger before any field judgment is paid.");
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Until(() => s.IsTianduOpportunity);
            var result = s.Events<JudgmentResolvedEvent>().Single(e => e.Reason == Reason);
            Need(result.TargetSeat == 1 && result.Suit == suit && result.CardId is not null &&
                s.Events<ProgramBindingStartedEvent>().Count(e => e.SkillId == Tuntian && e.BindingId == "off-turn-owned-loss") == 1,
                "Source959 uses the single real judgment belonging to the original Dodge-loss candidate; Tiandu sees that same finalized judgment.");
            s.Answer(c => c.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-" + (claim ? "activate" : "skip"));
            s.Play(); var judged = result.CardId!.Value; var field = !claim && suit != Suit.Heart;
            Need(s.Events<JudgmentResolvedEvent>().Count(e => e.Reason == Reason) == 1 &&
                s.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Tuntian && e.BindingId == "off-turn-owned-loss" && e.Completed) == 1 &&
                s.Events<ProgramJudgmentCardClaimedEvent>().Count(e => e.JudgmentFrameId == result.ResolutionId && e.SkillId == Tiandu && e.OwnerSeat == 1 && e.CardId == judged) == (claim ? 1 : 0) &&
                s.V(1).AuthorityCount == (field ? 1 : 0) && s.G.GetCombatDistance(1, 3) == (field ? 1 : 2) &&
                s.Zone(judged) == (claim ? CardLocation.Hand(1) : new CardLocation(CardZoneKind.Authority, 1)),
                "An unclaimed non-heart original becomes one field and reduces outgoing distance. Tiandu instead retains the same original in hand, including non-heart, with no second acquisition or field.");
            var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
            Need(s.Events<CardRespondedEvent>().Count(e => e.CardId == dodge && e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == dodge && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
                s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use && e.CardId == slash) == 1 &&
                s.Events<DamageAppliedEvent>().All(e => e.SourceCardUseResolutionId != use) && s.V(1).Hp == 3 &&
                !s.G.ResolutionStack.Any(f => f.Id == use),
                "The real Dodge still answers the original paid Slash exactly once; later response cleanup neither triggers another field judgment nor causes damage.");
        }
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public bool IsTianduOpportunity => P is { Kind: DecisionKind.ProgramJudgmentTrigger, PlayerSeat: 1 } &&
            G.ResolutionStack.LastOrDefault() is ProgramJudgmentTriggerWindowFrame w && w.CandidateIndex >= 0 &&
            w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex].SkillId == Tiandu;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, (r.Error?.Message ?? "Agriculture native command rejected.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; var c = p.Choices.FirstOrDefault(choose); Need(c is not null, "Missing original source959/Tiandu choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public int UseSlash()
        {
            var card = V(0).Hand.Single(c => c.Kind == CardKind.Slash);
            var a = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]));
            Accept(new PlayCardCommand(0, card.Id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind)); return card.Id;
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 220; step++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "Agriculture check stays in one live original action. " + Diagnostic());
                if (P is { } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" || c.Parameters.GetValueOrDefault("action") == "program-judgment-trigger-skip" || c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(c is not null, "Unexpected native agriculture boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Agriculture exceeded bounded native progression. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
            Pending = P, Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Recent = G.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(Suit suit)
        {
            var enemies = new ShanheCombatantDefinition[]
            {
                new(StandardShanheSourceAgricultureOfficerPackage.HardId, Hp: 3) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] },
                new("classic:guan-yu") { StartingHand = [] }, new("classic:guan-yu") { StartingHand = [] }
            };
            var setup = new ShanheBattleSetup(211, 1, "fixture:source-agriculture", "classic:guan-yu", 4, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Heart, 7)], [], enemies, [], "fixture:source-agriculture");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(suit),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-agriculture", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
            b.AddDeck(new(Deck, "零自然起手摸牌的真实判定牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", suit, 2)).ToArray() });
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:agriculture-human-setup";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions o, ModeConfigurationValidationPhase p) => new StandardModeSetupFactory().ValidateInitialOptions(o, p);
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(c));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:agriculture-human-profile";
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
