using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryJuShouMobileChecks
{
    private const string Hero = "boundary:ju-shou-mobile", Jianying = "boundary:jianying-mobile", Shibei = "boundary:shibei-mobile", Owner = "fixture:mobile-ju-shou-owner",
        Driver = "fixture:mobile-ju-shou-driver", Hp = "fixture:mobile-ju-shou-hp", Mode = "identity:classic-mobile-ju-shou";

    public static void ColoredInheritanceAndColorlessPreviousDoNotForgeSuitMatchesCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("stock"); s.Play();
        var material = s.V(0).Hand.Single(c => c.Suit == Suit.Spade && c.Rank == 11);
        var heart = s.V(0).Hand.Single(c => c.Suit == Suit.Heart && c.Rank == 6);
        var reserve = s.V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Reserve(reserve); s.Play(); s.PlayPhysical(heart.Id); s.Play();
        var choice = s.G.GetLegalActions(0).Single(a => a.CardId == material.Id && a.PlayedCardKind == CardKind.Peach && a.ConversionSource?.SkillId == Jianying);
        s.Need(s.G.GetLegalActions(0).Any(a => a.CardId == material.Id && a.PlayedCardKind == CardKind.ColdSlash && a.ConversionSource?.SkillId == Jianying),
            "The inherited one-card phase conversion includes the engine's native Cold Slash as a basic use, alongside ordinary basic use options.");
        s.PlayAction(choice); s.Reach(() => s.Offered());
        var actual = s.E<CardUseAppearanceCapturedEvent>().Last(e => e.Action.ActorSeat == 0).Action;
        s.Need(actual.EffectiveKind == CardKind.Peach && actual.EffectiveSuit == Suit.Heart && actual.EffectiveRank == 11 && actual.PhysicalCards is [var cost] &&
            cost.CardId == material.Id && cost.CardKind == CardKind.Crossbow && cost.From == CardLocation.Hand(0) && material.Suit == Suit.Spade && actual.ConversionChain.Any(c => c.SkillId == Jianying),
            "The real converted use inherits its previous Heart appearance while retaining the original Spade material and rank; that appearance qualifies the adjacent-use draw.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
        s.Need(s.V(0).Hand.Any(c => c.Id == reserve) && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Jianying && e.BindingId == "draw-on-matching-card") == 1 &&
            s.G.GetLegalActions(0).All(a => a.ConversionSource?.SkillId != Jianying),
            "The actual reserved top entity is drawn once after cold restore, and all basic outputs share the consumed Play-phase conversion allowance.");

        var none = Scene.Start(); none.Play(); none.Use("stock"); none.Play();
        var first = none.V(0).Hand.Single(c => c.Suit == Suit.None && c.Rank == 3).Id;
        var second = none.V(0).Hand.Single(c => c.Suit == Suit.None && c.Rank == 4).Id;
        var ownSuit = none.V(0).Hand.Single(c => c.Suit == Suit.Spade && c.Rank == 11);
        none.PlayPhysical(first); none.Play(); none.PlayPhysical(second); none.Play();
        none.Need(none.E<CardUseAppearanceCapturedEvent>().Where(e => e.Action.ActorSeat == 0).TakeLast(2).All(e => e.Action.EffectiveSuit == Suit.None) &&
            none.E<ProgramBindingStartedEvent>().All(e => e.SkillId != Jianying || e.BindingId != "draw-on-matching-card"),
            "Two actual colorless uses with different ranks do not share a real suit and must not create a Jianying draw opportunity.");
        var useOwn = none.G.GetLegalActions(0).Single(a => a.CardId == ownSuit.Id && a.PlayedCardKind == CardKind.Peach && a.ConversionSource?.SkillId == Jianying);
        none.Cold(); none.PlayAction(useOwn); none.Play();
        var fallback = none.E<CardUseAppearanceCapturedEvent>().Last(e => e.Action.ActorSeat == 0).Action;
        none.Need(fallback.EffectiveKind == CardKind.Peach && fallback.EffectiveSuit == Suit.Spade && fallback.EffectiveRank == 11 &&
            none.E<ProgramBindingStartedEvent>().All(e => e.SkillId != Jianying || e.BindingId != "draw-on-matching-card"),
            "A previous Suit.None is not inheritable; the converted native use keeps its material's real Spade suit and does not falsely match the colorless predecessor.");
    }

    public static void InitialArmorDamageInstancesFirstAndLaterHpChildrenResetByTurnCold()
    {
        var s = Scene.Start(); s.Play();
        s.Need(s.V(0).Hp == 3 && s.V(0).MaxHp == 4 && s.V(0).Armor == 3,
            "The mobile 2/3 starting body plus native Lord HP adjustment begins with exactly three armor, rather than the old full-HP OL body.");
        s.Use("hit-two", 0); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Shibei));
        var first = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Shibei);
        s.Need(first.TriggerId == "first-injury" && s.V(0).Hp == 4 && s.V(0).Armor == 1 &&
            s.E<DamageAppliedEvent>().Single(e => e.TargetSeat == 0) is { Amount: 2, ArmorPayment: { ArmorAbsorbed: 2, ActualHpLost: 0 } },
            "One two-point damage instance absorbed by armor still counts as the first damage and issues exactly one native Recover1 child.");
        s.Cold(); s.Answer(_ => true); s.Play();
        s.Use("hit-one", 0); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Shibei));
        var later = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Shibei);
        s.Need(later.TriggerId == "later-injury" && s.V(0).Hp == 3 && s.V(0).Armor == 0 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0) == 2 && s.E<ProgramSkillHpLostEvent>().Count(e => e.SkillId == Shibei && e.Amount == 1) == 1,
            "The next real damage consumes the final armor and then issues one LoseHp1; damage points do not masquerade as additional instances.");
        s.Answer(_ => true); s.Play(); s.EndPlay(); s.Play(1); var before = s.V(0).Hp;
        s.UseBy(1, "hit-one", 0); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Shibei));
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Shibei).TriggerId == "first-injury" && s.V(0).Hp == before,
            "The actual next turn resets the first-damage fact: real HP damage is followed by Recover1 instead of carrying over the prior turn's LoseHp branch.");
        s.Answer(_ => true); s.Play(1);
        s.Need(s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Shibei && e.Completed) == 3 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.SkillId == Shibei) == 1,
            "All three original damage windows and their typed HP children finish once without repeating the cold-restored first recovery.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Offered() => P?.SkillPrompt?.SkillId == Jianying && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play(int seat = 0) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void EndPlay() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Use(string id, params int[] targets) => UseBy(0, id, targets);
        public void UseBy(int seat, string id, params int[] targets) => Accept(new UseProgramSkillCommand(seat, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void PlayPhysical(int id) => PlayAction(G.GetLegalActions(0).First(a => a.CardId == id && a.ConversionSource is null));
        public void PlayAction(LegalAction action) => Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        public void Reserve(int id)
        { Use("reserve"); Reach(() => P?.Choices.Any(c => c.Cards.SequenceEqual(new[] { id })) == true); Answer(c => c.Cards.SequenceEqual([id])); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Ju Shou command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-ju-shou-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId == Hp) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Ju Shou boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "Bounded mobile Ju Shou scene missed its expected boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold retains actual Play history, native conversion appearance/quota, armor damage and original HP-child returns."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).Armor, V(i).HandCount }) });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + Short()); }
        public static Scene Start()
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r)); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-ju-shou-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Activation(string id, int targets, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] {
                    Activation("stock", 0, new { op = "draw", target = "owner", amount = 8 }),
                    Activation("hit-one", 1, new { op = "damage", target = "selectedTarget", amount = 1 }),
                    Activation("hit-two", 1, new { op = "damage", target = "selectedTarget", amount = 2 }),
                    Activation("reserve", 0, new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, zones = new[] { "hand" }, count = 1, destination = "drawPileTop", resultBind = "reserved", awaitMovementTriggers = true }) } },
                new { id = Hp, revision = 1, triggers = new[] { new { id = "hp-child", window = "afterHealthChanged", subject = "owner", optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "pause", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "原生库存和伤害", description = "固定八实体，真实用牌及伤害" }, [Hp] = new { name = "原生HP孩子", description = "实际HP变化后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动界沮授真实场景", "supporter", Jianying, "qun", 3, AdditionalSkillIds: [Shibei, Driver, Hp]) { InitialHp = 2 });
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:mobile-ju-shou-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "固定真实角色", "supporter", "standard:none", "wu", 4, AdditionalSkillIds: [Driver]));
            const string deck = "fixture:mobile-ju-shou-deck";
            b.AddDeck(new(deck, "完整八张固定实体", 0, 0, []) { PhysicalCards = [
                new("standard:crossbow", Suit.None, 3), new("standard:crossbow", Suit.None, 4), new("standard:crossbow", Suit.Heart, 6), new("standard:crossbow", Suit.Spade, 11),
                new("standard:crossbow", Suit.Club, 8), new("standard:crossbow", Suit.Diamond, 9), new("standard:slash", Suit.Spade, 12), new("standard:peach", Suit.Heart, 13)] });
            b.AddMode(new(Mode, "移动界沮授原生流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
