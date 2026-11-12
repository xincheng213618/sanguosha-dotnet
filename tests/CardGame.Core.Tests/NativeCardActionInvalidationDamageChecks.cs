using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeCardActionInvalidationDamageChecks
{
    private const string Hero = "fixture:chijie-native-owner", Peer = "fixture:chijie-native-peer", Attacker = "fixture:chijie-native-attacker";
    private const string Chijie = "ol:chijie", Quiet = "fixture:chijie-native-quiet", Fast = "fixture:chijie-native-fast";
    private const string Repeat = "fixture:chijie-native-repeat", Hold = "fixture:chijie-native-hold";

    public static void RealChijieKeepsTheFirstHitAndStopsNativeChainDamageAndIssuedEpisodesCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "chijie-native-invalidation", Hero, 12, 0, 0, 0, 0, [],
            [new(CardKind.IronChain, Suit.Club, 6)], [],
            [new(Peer, Hp: 12) { StartingHand = [] }, new(Attacker, Hp: 12) { StartingHand = [new(CardKind.FireSlash, Suit.Heart, 7)] }],
            [new(Peer, Hp: 12) { StartingHand = [] }], new string('f', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 5, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
        Accept(new StartGameCommand()); Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var chain = View(0).Hand.Single().Id;
        Accept(new PlayCardCommand(0, chain, [0, 1], game.Revision, Prompt()!.PromptId));
        Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(View(0).IsChained && View(1).IsChained && View(3).Hand is [{ Kind: CardKind.FireSlash }],
            "A real Iron Chain links the human and one other participant before the actual bare enemy Fire Slash turn.");
        Accept(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Drive(() => Prompt()?.SkillPrompt?.SkillId == Chijie &&
            Prompt()!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(u => u.CardKind == CardKind.FireSlash);
        var material = use.Action!.PhysicalCards.Single().CardId;
        var episode = use.EffectEpisodes!.Episodes.Single();
        Need(use.SourceSeat == 3 && use.TargetSeats.SequenceEqual([0]) && View(0).Hp == 11 && View(1).Hp == 12 &&
            Events<DamageAppliedEvent>().Count(e => e.SourceSeat == 3 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            !Events<DamageAppliedEvent>().Any(e => e.SourceSeat == 3 && e.TargetSeat == 1) &&
            episode.Issue.Source.SkillId == Repeat && episode.Started == 0 &&
            !Capabilities().Get<INativeCardActionInvalidations>().IsUseIneffective(use.Id),
            "The real first elemental hit is already applied, while its linked non-target damage and independently issued extra pass are still pending.");
        Cold(); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        Drive(() => Prompt()?.SkillPrompt?.SkillId == Hold &&
            Prompt()!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var shared = Capabilities().Get<INativeCardActionInvalidations>();
        Need(shared.IsUseIneffective(use.Id) && shared.SuppressesRemainingCardDamage(use.Id) &&
            Events<ChijieUseInvalidatedEvent>().Single().Proof is { UseId: var ownedUse, SourceSeat: 3, TargetSeat: 0 } && ownedUse == use.Id &&
            Events<ChijieUsedEvent>().Count(e => e.UseId == use.Id && e.Branch == "invalidate") == 1 &&
            View(0).Hp == 11 && View(1).Hp == 12 &&
            game.CardMovements.Last(m => m.CardId == material).To == CardLocation.Processing,
            "The actual Chijie writer publishes the original-use fact after its real once-per-turn acceptance and before damage children resume.");
        Cold(); Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Drive(() => Events<CardUseFinishedEvent>().Any(e => e.ResolutionId == use.Id) && game.ResolutionStack.All(f => f.Id != use.Id));
        Need(View(0).Hp == 11 && View(1).Hp == 12 && View(2).Hp == 12 &&
            Events<DamageAppliedEvent>().Count(e => e.SourceSeat == 3) == 1 &&
            Events<NativeCardEffectEpisodeCancelledEvent>().Count(e => e.UseFrameId == use.Id && e.Episode.Id == episode.Id &&
                e.Episode.Started == 0 && e.Episode.Completed == 0) == 1 &&
            !Events<NativeCardEffectEpisodeStartedEvent>().Any(e => e.UseFrameId == use.Id) &&
            Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            Events<ChijieUseInvalidatedEvent>().Count(e => e.Proof.UseId == use.Id) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(3) && m.To == CardLocation.Processing) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "Cancelling this actual use preserves its first damage, suppresses the linked participant outside its target list, settles the unstarted extra episode and finishes one physical payment once.");
        Cold();

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot View(int seat) => game.CreateSnapshot(0, true).Players[seat];
        T[] Events<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        ProgramCapabilityRegistry Capabilities() => (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        void Accept(GameCommand command)
        { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Chijie native command."); }
        void Answer(Func<PromptChoice, bool> predicate)
        { var p = Prompt()!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(predicate).Id, game.Revision)); }
        void Drive(Func<bool> ready)
        {
            for (var i = 0; i < 220; i++)
            {
                if (ready()) return;
                Need(game.State.Winner == Winner.None, "Unexpected terminal in the bounded first native Chijie chain.");
                if (Prompt() is not { PlayerSeat: 0 } p) Accept(new AdvanceOneStepCommand(game.Revision));
                else if (p.Kind == DecisionKind.RespondDodge) Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage");
                else if (p.Kind == DecisionKind.Nullification) Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected native Chijie boundary " + Diagnostic());
            }
            throw new InvalidOperationException("The fixed native Chijie chain did not reach its next boundary " + Diagnostic());
        }
        string State() => JsonSerializer.Serialize(new { game.State, Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), game.CardMovements });
        void Cold()
        {
            var before = State(); game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(before == State(), "Cold replay retains the real first damage, original Chijie acceptance and native pending chain and episode ownership.");
        }
        string Diagnostic() => JsonSerializer.Serialize(new { Prompt = Prompt(), Frames = game.ResolutionStack,
            Hp = Enumerable.Range(0, 4).Select(i => View(i).Hp), Facts = game.Events.TakeLast(8).Select(e => e.Payload.ToString()) });
        void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + Diagnostic()); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-chijie-invalidation", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:xin-pi");
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-opening","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Fast}}}","revision":1,"triggers":[{"id":"quiet-peer","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{{Repeat}}}","revision":1,"triggers":[{"id":"native-use-repeat","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["fireSlash"],"optional":false,
                    "effects":[{"op":"issueCurrentCardEffectEpisode","target":"owner"}]}]},
                  {"id":"{{{Hold}}}","revision":1,"triggers":[{"id":"after-real-chijie","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","priority":-1000,"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"cancelled","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Quiet, Fast, Repeat, Hold }.ToDictionary(id => id, id => new { name = id, description = "Actual Chijie native chain",
                        optionLabels = id == Hold ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Actual Chijie defender", "", Chijie, "wei", 12, [Quiet, Hold]));
            b.AddGeneral(new(Peer, "Quiet linked peer", "", Quiet, "shu", 12, [Fast]));
            b.AddGeneral(new(Attacker, "Actual elemental attacker", "", Repeat, "shu", 12, [Quiet]));
            foreach (var kind in new[] { CardKind.IronChain, CardKind.FireSlash })
            { var card = CardCatalog.Get(kind); b.AddCard(new("fixture:chijie-native-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            b.AddDeck(new("fixture:chijie-native-deck", "Fixed Chijie native cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Native Chijie fixture", 4, 4, new Dictionary<string, int>(), "fixture:chijie-native-deck",
                GeneralPoolIds: [Hero, Peer, Attacker], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
