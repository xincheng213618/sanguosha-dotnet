using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static class CardResponseRuleChecks
{
    private const string Hero = "fixture:response-rule-owner", Peer = "fixture:response-rule-peer",
        Driver = "fixture:response-rule-driver", NoDraw = "fixture:response-rule-no-draw",
        Watch = "fixture:response-rule-watch", Rewrite = "fixture:response-rule-rewrite",
        Poxi = "mobile:shi-poxi", Ganjue = "mobile:shi-ganjue", Cuixin = "happy:cuixin";

    public static void PoxiDoesNotLeakIntoCuixinNestedUseAndGanjueUsesPrintedSuitCold()
    {
        var s = Scene.Start(poxi: true);
        var discarded = s.V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Accept(new UseProgramSkillCommand(0, Driver, "discard", [discarded], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Poxi);
        s.Answer(c => c.Targets.SequenceEqual([1]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var outer = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.ShiXiahouBaPoxi is not null);
        s.Need(outer.CardId == discarded && s.V(0).HandCount == 2 &&
            s.V(1).Hand is [{ Kind: CardKind.Dodge }] && s.V(3).Hand is [{ Kind: CardKind.Dodge }],
            "The original discarded physical Slash reaches its real pre-response window with a live qualifying hand comparison.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Cuixin);
        s.Need(s.V(1).Hp == 11 && s.V(1).Hand.Single().Kind == CardKind.Dodge &&
            s.E<CardRespondedEvent>().All(e => e.ResponderSeat != 1) &&
            s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.Id == outer.Id).Step == ResolutionFrameStep.Completed,
            "Poxi prohibits its own target's available Dodge, and its genuine completed use remains beneath the Cuixin choice.");
        s.Cold(); s.Answer(c => c.Targets.SequenceEqual([3])); s.Play();
        var inner = s.E<ShenDengAiNativeIssuedEvent>().Single().Origin;
        s.Need(inner.OriginalUseId == outer.Id && inner.UseId != outer.Id && inner.ActionId != outer.Action!.ActionId &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 3 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.V(3).Hp == 12 && s.V(3).HandCount == 0 && s.V(1).Hp == 11 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == outer.Id) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == inner.UseId) == 1,
            "A true Cuixin nested Slash gets its own response scope: the ancestor Poxi ban does not block the other target's Dodge, and both original uses return once.");

        foreach (var matchingSuit in new[] { true, false })
        {
            var g = Scene.Start(poxi: false, matchingSuit);
            g.Accept(new UseProgramSkillCommand(0, Ganjue, "ganjue-use", [], [1], g.G.Revision, g.P!.PromptId));
            g.Reach(() => g.P?.SkillPrompt?.SkillId == Ganjue);
            g.Answer(c => c.Cards.Count == 1);
            g.Reach(() => g.P?.SkillPrompt?.SkillId == Watch);
            var issued = g.E<ShiSsGanjueIssuedEvent>().Single();
            g.Need(issued.StrikeSuit == Suit.Spade && g.V(1).Hand.Single().Suit == (matchingSuit ? Suit.Spade : Suit.Heart),
                "Ganjue signs the equipped card's original printed suit before the actual response window.");
            g.Cold(); g.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); g.Play();
            g.Need(g.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == (matchingSuit ? 1 : 0) &&
                g.V(1).Hp == (matchingSuit ? 12 : 11) &&
                g.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == issued.UseId) == 1,
                "The responder's current printed hand suit controls Ganjue, even when its effective suit is rewritten to the opposite answer, and cold recovery returns once.");
        }
    }

    public static void YingboAllAudienceBlocksRealDuelAndNonTargetCounterspellCold()
    {
        var s = Scene.Start(poxi: false, yingbo: true);
        s.Need(s.G.GetRuntimeModule<CoreGameEngineMouLuMengRuntime>().UsesFormalMouLuMeng,
            "The real classic package capability must enable Yingbo in this small native fixture.");
        var card = s.V(0).Hand.Single(c => c.Kind == CardKind.Duel).Id;
        var action = s.G.GetHumanLegalActions().Single(a => a.CardId == card && a.TargetSeats.SequenceEqual([1]));
        s.Accept(new PlayCardCommand(0, card, [1], s.G.Revision, s.P!.PromptId, action.PlayedCardKind));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single();
        void CheckAudience()
        {
            var lifecycle = s.G.GetRuntimeModule<CoreGameEngineCardUseLifecycleRuntime>();
            foreach (var (responder, required) in new[] { (1, CardKind.Slash), (0, CardKind.Slash), (3, CardKind.Nullification) })
            {
                var query = lifecycle.CreateResponseQuery(use.Id, responder == 0 ? 1 : 0, responder,
                    CardKind.Duel, required, [1], providerSeat: 2)!.Value;
                s.Need(lifecycle.GetResponseDecision(query).Forbidden &&
                    lifecycle.GetResponseDecision(query with { ProviderSeat = responder }).Forbidden,
                    "The real first-use Yingbo row covers both Duel directions and a non-target counterspell responder, independently of who supplies its material.");
            }
        }
        s.Need(use.YingboUnrespondable && use.ResponseRules is [{ Audience: ResponseRuleAudience.All, Forbid: true }] &&
            s.V(0).Hand is [{ Kind: CardKind.Slash }] && s.V(1).Hand is [{ Kind: CardKind.Slash }] &&
            s.V(3).Hand is [{ Kind: CardKind.Nullification }],
            "An actual first-name Duel signs one all-audience response rule while both duelists and a genuine non-target have their respective response cards.");
        CheckAudience(); s.Cold(); CheckAudience();
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Kind == DecisionKind.Yingbo);
        s.Need(!s.E<NullificationRespondedEvent>().Any() && !s.E<CardRespondedEvent>().Any() &&
            s.V(1).Hp == 11 && s.V(0).HandCount == 1 && s.V(1).HandCount == 1 && s.V(3).HandCount == 1,
            "The native counterspell and Duel paths honor the same issued rule, without spending any available response or borrowing another provider's authority.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("action") == "yingbo-skip"); s.Play();
        s.Need(s.E<YingboGiftResolvedEvent>().Single().TargetSeat is null &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            !s.G.ResolutionStack.OfType<CardUseFrame>().Any(),
            "The independent original Yingbo gift qualification survives cold recovery and returns the genuine use once after its real choice.");
    }

    public static void QiaoliArmorSignsAllAudienceBeforeNativeCounterspellCandidatesCold()
    {
        var s = Scene.Start(poxi: false, qiaoli: true);
        var material = s.V(0).Hand.Single(c => c.Kind == CardKind.BaguaFormation).Id;
        s.Accept(new UseProgramSkillCommand(0, "ol:qiaoli", "qiaoli-armor", [material], [1], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single();
        s.Need(use.CardId == material && use.CardKind == CardKind.Duel &&
            use.ResponseRules is [{ Audience: ResponseRuleAudience.All, Forbid: true, Source.SkillId: "ol:qiaoli", Source.BindingId: "qiaoli-armor" }] &&
            s.V(1).Hp == 11 && s.V(0).Hand is [{ Kind: CardKind.Slash }] &&
            s.V(1).Hand is [{ Kind: CardKind.Slash }] && s.V(3).Hand is [{ Kind: CardKind.Nullification }] &&
            !s.E<NullificationRequestedEvent>().Any() && !s.E<NullificationRespondedEvent>().Any() && !s.E<CardRespondedEvent>().Any(),
            "The real Qiaoli armor Duel signs its source-owned all-audience rule before any counterspell candidate is published, despite an available non-target Nullification and both duelists' Slash cards.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.E<ProgramQiaoliArmorCommittedEvent>().Count() == 1 &&
            s.E<IssuedCardNoResponseEvent>().Count(e => e.Effect.ParentFrameId == use.Id) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            !s.G.ResolutionStack.OfType<CardUseFrame>().Any(),
            "Cold return completes the paid armor conversion once, preserving its one public issuance and one physical material payment.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0, true).Players[seat];
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        {
            var result = G.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The native response-rule command was rejected.");
        }
        public void Answer(Func<PromptChoice, bool> select)
        { var p = P!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(select).Id, G.Revision)); }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 110; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None && P is not { PlayerSeat: 0 }, "Unexpected native response-rule boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native response-rule scene did not reach its next window.");
        }
        public void Cold()
        {
            var state = State();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(state == State(), "Cold replay preserves the original response rule and its real native child windows.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), G.CardMovements
        });
        public void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message + JsonSerializer.Serialize(new
            { G.State, Prompt = P, Facts = G.Events.TakeLast(8).Select(e => e.Payload.ToString()) }));
        }
        public static Scene Start(bool poxi, bool matchingSuit = true, bool yingbo = false, bool qiaoli = false)
        {
            var fixture = new Fixture(poxi, matchingSuit, yingbo, qiaoli);
            var registry = yingbo ? ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(),
                new StandardRescueSkillExpansionPackage(), fixture) : ContentRegistry.Build(new StandardContentPackage(), fixture);
            var target = new ShanheCombatantDefinition(Peer, Hp: 12)
            { StartingHand = [new(yingbo || qiaoli ? CardKind.Slash : CardKind.Dodge, poxi ? Suit.Club : matchingSuit ? Suit.Spade : Suit.Heart, 2)] };
            var setup = new ShanheBattleSetup(31, 1, "native-response-rule", Hero, 12, 0, 0, 0, 0, [],
                qiaoli ? [new(CardKind.BaguaFormation, Suit.Club, 7), new(CardKind.Slash, Suit.Spade, 8)] :
                    yingbo ? [new(CardKind.Duel, Suit.Club, 7), new(CardKind.Slash, Suit.Spade, 8)] :
                    poxi ? [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Heart, 4)] : [],
                poxi || yingbo || qiaoli ? [] : [new(CardKind.Crossbow, Suit.Spade, 7)],
                [new(Peer, Hp: 12) { StartingHand = [] }, yingbo || qiaoli ? new(Peer, Hp: 12)
                    { StartingHand = [new(CardKind.Nullification, Suit.Club, 9)] } : target], [target], new string('b', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(bool poxi, bool matchingSuit, bool yingbo, bool qiaoli) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = yingbo ? new StandardClassicGeneralPackage().Manifest :
            new("fixture-native-response-rules", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:shi-xiahou-ba", "mobile:shi-sun-shao", "happy:shen-deng-ai");
            if (yingbo) CharacterFixtureContent.Register(b, "mou:lu-meng");
            if (qiaoli) CharacterFixtureContent.Register(b, "ol:rui-ji");
            var watchContext = qiaoli ? "\"window\":\"afterDamageApplied\",\"subject\":\"damageSource\",\"damageOccurrence\":\"perDamage\"" :
                "\"window\":\"" + (yingbo ? "cardUseCommitted" : "slashBeforeResponse") + "\",\"ownerRelation\":\"actor\"";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}]},
              {"id":"{{{NoDraw}}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"actual-response",{{{watchContext}}},"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]},
              {"id":"{{{Rewrite}}}","revision":1,"cardPolicies":[{"id":"effective-opposite","kind":"rewriteSuit","inputSuit":"{{{(matchingSuit ? "spade" : "heart")}}}","outputSuit":"{{{(matchingSuit ? "heart" : "spade")}}}"}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, NoDraw, Watch, Rewrite }.ToDictionary(id => id, id => new
                { name = id, description = "Real native card response boundaries", optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] })
            }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Native response source", "", Driver, "wei", 12,
                qiaoli ? [NoDraw, Watch, "ol:qiaoli"] : yingbo ? [NoDraw, Watch, "mou:yingbo"] : poxi ? [NoDraw, Watch, Poxi, Cuixin] : [NoDraw, Watch, Ganjue]));
            b.AddGeneral(new(Peer, "Native response target", "", NoDraw, "shu", 12, [Rewrite]));
            var crossbow = CardCatalog.Get(CardKind.Crossbow);
            b.AddCard(new("fixture:response-rule-crossbow", crossbow.DisplayName, crossbow.CategoryName, crossbow.Description, CardKind.Crossbow));
            foreach (var kind in new[] { CardKind.Duel, CardKind.Nullification, CardKind.BaguaFormation })
            { var card = CardCatalog.Get(kind); b.AddCard(new("fixture:response-rule-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            b.AddDeck(new("fixture:response-rule-deck", "Fixed native response cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Native response-rule fixture", 4, 4, new Dictionary<string, int>(), "fixture:response-rule-deck",
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
