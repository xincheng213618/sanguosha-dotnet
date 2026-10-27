using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheZhaoguMirrorChecks
{
    private const string Skill = "shanhe:zhaogu-mirror-6018", Deck = "fixture:zhaogu-mirror-deck";
    private const string TemplateAction = "shanhe-zhaogu-template", UseAction = "shanhe-zhaogu-use";

    public static void RevealedOrdinaryTrickUsesRealDrawAndPaidCounterspellBranches()
    {
        foreach (var counter in new[] { false, true })
        {
            var s = Scene.Start(counter ? Profile.CounteredTrick : Profile.Trick);
            var template = s.V(0).Hand.Single(c => c.Kind == CardKind.DrawTwo).Id;
            var before = s.V(0).HandCount;
            var counterId = counter ? s.V(1).Hand.Single(c => c.Kind == CardKind.Nullification).Id : 0;
            var producer = s.Show(template);
            s.Until(() => s.IsUseChoice);
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == UseAction &&
                (c.Targets.Count == 0 || c.Targets.SequenceEqual([0])));
            if (counter)
            {
                s.Until(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 1 });
                Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == producer) &&
                    s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.CardId == 0 && f.CardKind == CardKind.DrawTwo),
                    "The actual ordinary trick and its original ending owner remain suspended on the native counterspell window.");
                s.Answer(c => c.Cards.SequenceEqual([counterId]));
            }
            s.NextPlay();
            var use = s.Events<CardUseDeclaredEvent>().Single(e => e.CardId == 0 && e.CardKind == CardKind.DrawTwo && e.SourceSeat == 0);
            var accepted = s.Events<CardActionAcceptedEvent>().Select(e => e.Action).Single(a =>
                a.ActorSeat == 0 && a.EffectiveKind == CardKind.DrawTwo && a.PhysicalCards.Count == 0);
            AssertVirtualAction(accepted, CardKind.DrawTwo);
            Need(s.V(0).HandCount == before + (counter ? 0 : 2) &&
                s.Events<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) == (counter ? 0 : 2),
                "The virtual Draw Two really draws two deck entities, while an actually paid counterspell prevents the original draw effect.");
            if (counter)
                Need(s.Events<NullificationResolvedEvent>().Single(e => e.ResolutionId == use.ResolutionId).EffectNullified &&
                    s.G.CardMovements.Count(m => m.CardId == counterId && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing) == 1 &&
                    s.G.CardMovements.Count(m => m.CardId == counterId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
                    "The native counterspell uses its real original material exactly once before the mirror's ending return.");
            s.AssertReturned(producer, template, use.ResolutionId, CardKind.DrawTwo);
        }

        var duel = Scene.Start(Profile.Duel);
        var duelTemplate = duel.V(0).Hand.Single(c => c.Kind == CardKind.Duel).Id;
        Need(duel.V(1).HandCount == 0, "The actual Duel target has no Slash entity before the mirror displays its template.");
        var duelProducer = duel.Show(duelTemplate);
        duel.Until(() => duel.IsUseChoice);
        duel.Answer(c => c.Parameters.GetValueOrDefault("program-action") == UseAction && c.Targets.SequenceEqual([1]));
        duel.NextPlay();
        var duelUse = duel.Events<CardUseDeclaredEvent>().Single(e => e.CardId == 0 && e.CardKind == CardKind.Duel && e.SourceSeat == 0);
        var duelAction = duel.Events<CardActionAcceptedEvent>().Select(e => e.Action).Single(a =>
            a.ActorSeat == 0 && a.EffectiveKind == CardKind.Duel && a.PhysicalCards.Count == 0);
        AssertVirtualAction(duelAction, CardKind.Duel);
        var facts = duel.G.Events.Select(e => e.Payload).ToArray();
        var damageIndex = Array.FindIndex(facts, e => e is DamageAppliedEvent d && d.SourceCardUseResolutionId == duelUse.ResolutionId);
        var finishIndex = Array.FindIndex(facts, e => e is CardUseFinishedEvent f && f.ResolutionId == duelUse.ResolutionId);
        var returnIndex = Array.FindIndex(facts, e => e is ProgramBindingResolvedEvent p && p.FrameId == duelProducer && p.Completed);
        Need(duel.V(1).Hp == 19 && duel.V(0).Hp == 20 &&
            duel.Events<DuelResponseEvent>().Where(e => e.ResolutionId == duelUse.ResolutionId).ToArray() is
                [{ ResponderSeat: 1, UsedSlash: false, SlashCardId: null }] &&
            duel.Events<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == duelUse.ResolutionId).ToArray() is
                [{ SourceSeat: 0, TargetSeat: 1, Amount: 1, Nature: DamageNature.Normal }] &&
            damageIndex >= 0 && finishIndex > damageIndex && returnIndex > finishIndex,
            "The original zero-material Duel takes the target's actual failed Slash response, applies one native damage, then finishes before returning once to its own PlayEnding program.");
        duel.AssertReturned(duelProducer, duelTemplate, duelUse.ResolutionId, CardKind.Duel);
    }

    private static void AssertVirtualAction(CardActionContext action, CardKind kind) => Need(
        action is { Type: CardActionType.Use, ActorSeat: 0, ProviderSeat: 0, PhysicalCards.Count: 0,
            EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: null } && action.EffectiveKind == kind,
        "The native mirror use has zero material entities and no color, suit or rank inherited from its shown original template.");

    private enum Profile { Slash, Trick, CounteredTrick, Duel, Dodge, Counterspell, SlashQuota, Replace }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool IsUseChoice => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == UseAction) == true;
        public void Accept(GameCommand command)
        { var r = G.Submit(command); Need(r.Accepted, (r.Error?.Message ?? "Native mirror command rejected.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> test)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(test);
            Need(c is not null, "Missing exact mirror/native choice. " + Diagnostic());
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public long Show(int card)
        {
            End();
            Until(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == TemplateAction) == true);
            Need(P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == TemplateAction && c.Cards.SequenceEqual([card])),
                "An otherwise unusable basic/ordinary-trick entity must remain eligible for the actual initial display step.");
            var producer = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            Need(producer.OwnerSeat == 0 && producer.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding &&
                V(0).Equipment.Any(c => c.Kind == CardKind.ZhaoguMirror),
                "The template is selected by the real equipped source on its original own PlayEnding frame.");
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == TemplateAction && c.Cards.SequenceEqual([card]));
            return producer.Id;
        }
        public void AssertReturned(long producer, int template, long use, CardKind kind)
        {
            Need(Events<ProgramCardsRevealedEvent>().Count(e => e.FrameId == producer && e.SkillId == Skill &&
                    e.Cards.Select(c => c.Id).SequenceEqual([template])) == 1 &&
                Zone(template) == CardLocation.Hand(0) && !G.CardMovements.Any(m => m.CardId == template && m.From == CardLocation.Hand(0)) &&
                Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use && e.CardId == 0 && e.CardKind == kind) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use && e.CardId == 0 && e.CardKind == kind) == 1 &&
                Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == producer && e.SkillId == Skill && e.Completed) == 1 &&
                !G.CardMovements.Any(m => m.CardId == 0) && !G.ResolutionStack.Any(f => f.Id == producer || f.Id == use),
                "One real display pays no entity: one zero-material native use and finish return exactly once to the original ending owner, retaining the original hand card.");
        }
        public void Physical(CardKind kind, int rank, int[] targets)
        {
            var card = V(0).Hand.Single(c => c.Kind == kind && c.Rank == rank);
            var a = G.GetHumanLegalActions().First(x => x.CardId == card.Id && x.ConversionSource is null &&
                (x.TargetSeats.SequenceEqual(targets) || kind == CardKind.WoodenOx && targets.Length == 0 && x.TargetSeats.SequenceEqual([0])) &&
                (x.PlayedCardKind ?? card.Kind) == kind);
            Accept(new PlayCardCommand(0, card.Id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId));
            Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
            Need(G.CardMovements.Count(m => m.CardId == card.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                Events<CardUseFinishedEvent>().Count(e => e.CardId == card.Id) == 1,
                "The baseline Slash or actual replacement Treasure pays its own physical entity and finishes once.");
        }
        public void NextPlay() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 180; step++)
            {
                if (stop()) return;
                if (P is { } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" ||
                        c.Parameters.GetValueOrDefault("program-action") == "activate" &&
                            (p.SkillPrompt?.SkillId == Skill || c.Parameters.GetValueOrDefault("skill-id") == Skill));
                    Need(c is not null, "Unexpected native mirror boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Mirror exceeded bounded native continuation. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner, Pending = P,
            Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Events = G.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(Profile profile)
        {
            CardKind template = profile switch
            { Profile.Trick or Profile.CounteredTrick => CardKind.DrawTwo, Profile.Duel => CardKind.Duel, Profile.Dodge => CardKind.Dodge,
                Profile.Counterspell => CardKind.Nullification, _ => CardKind.Slash };
            List<ShanheStartingCard> cards = [new(template, Suit.Heart, 7)];
            if (profile == Profile.SlashQuota) cards.Add(new(CardKind.Slash, Suit.Spade, 3));
            if (profile == Profile.Replace) cards.Add(new(CardKind.WoodenOx, Suit.Diamond, 4));
            var enemies = Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition("classic:guan-yu", MaxHpBonus: 16, Hp: 20)
            { StartingHand = seat == 1 && profile == Profile.Slash ? [new(CardKind.Dodge, Suit.Club, 2)] :
                seat == 1 && profile == Profile.CounteredTrick ? [new(CardKind.Nullification, Suit.Spade, 11)] : [] }).ToArray();
            var setup = new ShanheBattleSetup(59, 1, "fixture:zhaogu-mirror", "classic:guan-yu", 20,
                16, 0, 0, 0, [], cards, [new(CardKind.ZhaoguMirror, Suit.Heart, 1)], enemies, [], "fixture:zhaogu-mirror");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            s.Accept(new StartGameCommand());
            s.Until(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.ResolutionStack.Count == 0);
            Need(s.V(0).Equipment.Any(c => c.Kind == CardKind.ZhaoguMirror) && s.V(0).Skills!.Any(k => k.ContentId == Skill),
                "The actual Treasure entity installs its physical equipment skill before the first human Play.");
            return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:zhaogu-mirror", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
            b.AddDeck(new(Deck, "零起手摸牌的真实牌堆", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 2)).ToArray() });
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:zhaogu-human-setup";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions o, ModeConfigurationValidationPhase p) => new StandardModeSetupFactory().ValidateInitialOptions(o, p);
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(c));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:zhaogu-human-profile";
        public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new StandardModeProfileFactory().Create(
            new ModeFeatureCreationContext(c.Options! with { HumanSeats = null }, c.ModeDefinition!, c.ContentRegistry,
                new ModeHostRegistry().Register<IModeSetupRandom>(() => c.GetHost<IModeSetupRandom>())
                    .Register<IModeNationalRevealHost>(() => c.GetHost<IModeNationalRevealHost>())
                    .Register<IModeSetupEquipmentHost>(() => c.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers()
        {
            var p = inner.BuildPlayers();
            return new(p.Seats.Select(s => s with { IsHuman = true }), p.AvailableGeneralIds, p.SelectionOrder,
                p.ShenwuSharedGodIds, p.ShenwuLeaderGeneralId, p.GuanduConfiguration, p.InitialActorSeat);
        }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest r) => inner.GetGeneralOffers(r);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> d) => inner.PrepareDeck(d);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> d) => inner.ProjectDeckCardNames(d);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> d, IReadOnlyList<ModeSeatSetupFacts> s) => inner.PrepareSelectedEquipment(d, s);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts s, string id) => inner.PrepareSelectedGeneral(s, id);
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
