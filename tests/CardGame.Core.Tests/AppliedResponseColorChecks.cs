using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static class AppliedResponseColorChecks
{
    private const string Hero = "fixture:response-color-owner", Donor = "fixture:response-color-donor",
        Peer = "fixture:response-color-peer", Driver = "fixture:response-color-driver",
        Watch = "fixture:response-color-watch", Quiet = "fixture:response-color-quiet",
        Rewrite = "fixture:response-color-rewrite", Duorui = "happy:duorui";

    public static void ActualPrivateTakeFreezesColorAndSharesNativeAndDamageMatchingCold()
    {
        var s = Scene.Start(unknownColor: false);
        var relation = s.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, true);
        var paid = s.E<HappyZlPaidEvent>().Single();
        s.Need(relation is { ActorSeat: 0, ResponderSeat: 1, Spec.Match.IsRed: true } &&
            relation.ProducerFrameId == paid.FrameId && relation.Source.SkillId == Duorui &&
            s.V(0).Hand.Single(card => card.Id == s.Taken).Suit == Suit.Club &&
            s.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, false) is null &&
            s.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, null) is null &&
            s.Rules.GetMatchingRelation(Duorui, 0, 3, CardKind.Slash, true) is null,
            "The real private take freezes its old holder's rewritten red color, independently of the card's printed Club and its new holder, with no unknown-color or other-recipient match.");
        s.Cold();

        // This DrawTwo has no explicit execution targets. The former holder's
        // counterspell opportunity is still covered by its all-audience rule.
        var draw = s.V(0).Hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
        var trick = s.Use(draw, []);
        var targetless = trick.TargetSeats.Count == 0;
        var originalHolderForbidden = s.Rules.IsResponseForbidden(trick.Id, 1, CardKind.Nullification);
        var otherResponderForbidden = s.Rules.IsResponseForbidden(trick.Id, 3, CardKind.Nullification);
        var wholeUseForbidden = s.Rules.HasUnconditionalResponseProhibition(trick.Id);
        s.Need(targetless && originalHolderForbidden && !otherResponderForbidden && !wholeUseForbidden,
            "The actual red trick keeps its non-target recipient restriction separate from other responders and whole-use prohibition." +
            JsonSerializer.Serialize(new { targetless, originalHolderForbidden, otherResponderForbidden, wholeUseForbidden,
                trick.TargetSeats, trick.ResponseRules, effectiveSuit = trick.Action?.EffectiveSuit,
                effectiveIsRed = trick.Action?.EffectiveIsRed }));
        s.ContinueUse(); s.Play();
        var rootRequests = s.E<NullificationRequestedEvent>().Where(e => e.ResolutionId == trick.Id).ToArray();
        var counterReplies = s.E<NullificationRespondedEvent>().Where(e => e.ResolutionId == trick.Id).ToArray();
        var trickFinishes = s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == trick.Id);
        s.Need(!rootRequests.Any(e => e.ResponderSeat == 1 && e.ChainDepth == 0) &&
            counterReplies.Any(e => e.ResponderSeat == 1 && e.ChainDepth == 2) && trickFinishes == 1,
            "The former holder cannot counter the original red trick, but can answer another actor's subsequent unrestricted counterspell node; the real trick returns once." +
            JsonSerializer.Serialize(new { rootRequests, counterReplies, trickFinishes }));

        var slash = s.Use(s.V(0).Hand.First(card => card.Kind == CardKind.Slash).Id, [1]);
        s.Need(s.Rules.IsResponseForbidden(slash.Id, 1, CardKind.Dodge) &&
            !s.Rules.IsResponseForbidden(slash.Id, 3, CardKind.Dodge),
            "The real Slash signs only its actual color relation before defense.");
        s.Cold();
        s.ContinueUse();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "damage"));
        var observer = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Last();
        s.Need(s.G.GetRuntimeModule<CharacterZhangLiaoHappyGodRuntime>().HappyZlDamageUse(0, observer.WindowContext!)?.Id == slash.Id &&
            s.Rules.GetMatchingRelation(Duorui, 0, 1, slash.CardKind, true)?.ProducerFrameId == relation!.ProducerFrameId,
            "Actual damage admission reads the same issued color relationship as native response qualification.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(1).Hp == 11 && s.E<CardRespondedEvent>().All(e => e.ResponderSeat != 1 || e.EffectiveCardKind != CardKind.Dodge) &&
            s.E<HappyZlLeaseEvent>().Length == 1,
            "The real recipient's available Dodge stays unspent and ordinary Duorui damage still performs its independent equipment-lease business.");

        s.Accept(new UseProgramSkillCommand(0, Driver, "lose-source", [], [], s.G.Revision, s.P!.PromptId));
        s.Play();
        s.Need(!s.Capabilities.Get<IMatchParticipantStateAccess>().At(0).SkillGrants.EffectiveSkillIds.Contains(Duorui) &&
            s.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, true) is not null,
            "A genuine JSON skill loss does not remove the applied relation issued by the earlier private take.");
        var afterLoss = s.Use(s.V(0).Hand.Single(card => card.Kind == CardKind.Slash).Id, [1]);
        s.Need(s.Rules.IsResponseForbidden(afterLoss.Id, 1, CardKind.Dodge),
            "The later native declaration consumes the still-issued relationship without requalifying the lost source skill.");
        s.ContinueUse();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "damage"));
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(1).Hp == 10 && s.E<HappyZlLeaseEvent>().Length == 1,
            "Issued response rights survive source loss without authorizing another unavailable Duorui equipment effect.");
        s.Cold();
        var turn = s.G.State.TurnNumber;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.G.State.TurnNumber > turn);
        s.Need(s.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, true) is null,
            "The applied color relation expires when the real turn number changes.");

        var unknown = Scene.Start(unknownColor: true);
        unknown.Need(unknown.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, true) is null &&
            unknown.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, false) is null &&
            unknown.Rules.GetMatchingRelation(Duorui, 0, 1, CardKind.Slash, null) is null,
            "A real suitless private take issues no red, black or unknown wildcard relationship.");
        var ordinary = unknown.Use(unknown.V(0).Hand.First(card => card.Kind == CardKind.Slash).Id, [1]);
        unknown.Need(!unknown.Rules.IsResponseForbidden(ordinary.Id, 1, CardKind.Dodge),
            "Unknown source color cannot prohibit the original recipient's real Dodge.");
        unknown.ContinueUse(); unknown.Play();
        unknown.Need(unknown.V(1).Hp == 12 && unknown.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            unknown.E<HappyZlLeaseEvent>().Length == 0,
            "The unknown-color native use retains its normal response and never borrows the colored-damage effect.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public int Taken;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0, true).Players[seat];
        public ProgramCapabilityRegistry Capabilities => (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(G)!;
        public IAppliedResponseRules Rules => Capabilities.Get<IAppliedResponseRules>();
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The color-relation command was rejected."); }
        public void Answer(Func<PromptChoice, bool> select)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(select).Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None && P is not { PlayerSeat: 0 }, "Unexpected color-relation prompt.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The fixed color-relation fixture did not reach its next native window.");
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public CardUseFrame Use(int cardId, IReadOnlyList<int> targets)
        {
            Accept(new PlayCardCommand(0, cardId, targets, G.Revision, P!.PromptId));
            Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "use"));
            return G.ResolutionStack.OfType<CardUseFrame>().Last();
        }
        public void ContinueUse() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Cold()
        {
            var before = JsonSerializer.Serialize(new { G.State, Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == JsonSerializer.Serialize(new { G.State, Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements }),
                "Cold replay retains the actual private-take delivery, issued native rules and original frame identities.");
        }
        public void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message + JsonSerializer.Serialize(new { G.State, Prompt = P,
                Facts = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) }));
        }
        public static Scene Start(bool unknownColor)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(unknownColor));
            var donor = new ShanheCombatantDefinition(Donor, Hp: 12)
            { StartingHand = [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Spade, 3), new(CardKind.Nullification, Suit.Club, 4)] };
            var setup = new ShanheBattleSetup(31, 1, "applied-response-color", Hero, 12, 0, 0, 0, 0, [],
                [new(CardKind.DrawTwo, Suit.Heart, 6), new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Heart, 8)],
                [new(CardKind.Crossbow, Suit.Spade, 7)],
                [new(Peer, Hp: 12) { StartingHand = [] }, new(Peer, Hp: 12) { StartingHand = [new(CardKind.Nullification, Suit.Club, 9)] }],
                [donor], new string('f', 64));
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand());
            scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Duorui);
            scene.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Duorui && scene.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option") == "target:1"));
            scene.Answer(c => c.Targets.SequenceEqual([1]));
            scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Duorui && scene.P.IsPrivate);
            scene.Need(scene.G.CreateSnapshot(2).PendingDecision is null && scene.P!.PlayerSeat == 0,
                "The real former-holder hand inspection remains private to the taking owner.");
            var choice = scene.P!.Choices.Single(c => c.Parameters.GetValueOrDefault("slot-index") == "0");
            scene.Taken = choice.Cards.Single(); scene.Answer(c => c.Id == choice.Id); scene.Play();
            scene.Need(scene.G.CardMovements.Count(m => m.CardId == scene.Taken && m.From == CardLocation.Hand(1) &&
                m.To == CardLocation.Hand(0) && m.Reason.Value == CharacterZhangLiaoHappyGodRuntime.HappyZlTake) == 1,
                "The relation originates from the actual once-delivered private take, rather than a hand-filled Grant.");
            return scene;
        }
    }

    private sealed class Fixture(bool unknownColor) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-applied-response-color", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "happy:shen-zhang-liao");
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"lose-source","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["happy:duorui"]}}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[
                    {"id":"use","window":"cardUseCommitted","ownerRelation":"actor","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"use","options":[{"id":"continue"}]}]},
                    {"id":"damage","window":"afterDamageApplied","subject":"damageSource","damageOccurrence":"perDamage","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"damage","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{Rewrite}}}","revision":1,"cardPolicies":[{"id":"former-holder-red","kind":"rewriteSuit","inputSuit":"club","outputSuit":"{{{(unknownColor ? "none" : "heart")}}}"}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, Watch, Rewrite }.ToDictionary(id => id, id => new { name = id,
                        description = "Real applied response color boundaries", optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Hero, "Applied color source", "", Duorui, "wei", 12, [Driver, Quiet, Watch]));
            builder.AddGeneral(new(Donor, "Original color holder", "", Quiet, "shu", 12, [Rewrite]));
            builder.AddGeneral(new(Peer, "Unrestricted peer", "", Quiet, "wei", 12));
            foreach (var kind in new[] { CardKind.Crossbow, CardKind.Nullification })
            { var card = CardCatalog.Get(kind); builder.AddCard(new("fixture:response-color-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            builder.AddDeck(new("fixture:response-color-deck", "Fixed color-relation cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Applied response color", 4, 4, new Dictionary<string, int>(), "fixture:response-color-deck",
                GeneralPoolIds: [Hero, Donor, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
