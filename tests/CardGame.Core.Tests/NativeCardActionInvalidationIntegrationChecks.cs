using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeCardActionInvalidationIntegrationChecks
{
    private const string Ambush = "fixture:native-invalidation-ambush";
    private const string Driver = "fixture:native-invalidation-driver", Quiet = "fixture:native-invalidation-quiet";
    private const string Hero = "fixture:native-invalidation-owner", Ally = "fixture:native-invalidation-ally", Peer = "fixture:native-invalidation-peer";

    public static void RealAmbushPaymentsCancelDodgeAndOnlyTheirOwnCounterspellNodeCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "native-invalidation-responses", Hero, 12, 0, 0, 0, 0, [],
            [new(CardKind.Peach, Suit.Heart, 2), new(CardKind.Peach, Suit.Heart, 3)], [],
            [new(Peer, Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Club, 4), new(CardKind.Nullification, Suit.Spade, 5), new(CardKind.Nullification, Suit.Spade, 6)] },
                new(Peer, Hp: 12) { StartingHand = [] }],
            [new(Ally, Hp: 12) { StartingHand = [] }], new string('b', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        Accept(new StartGameCommand()); Reach(() => P() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(V(1).TeamId == V(0).TeamId && V(2).Hand.Count == 3 && V(3).Hand.Count == 0,
            "The fixed next actor is an ally and only the enemy defender has physical response cards.");
        var responseMaterials = V(2).Hand.Select(c => c.Id).ToArray();
        Accept(new EndPlayPhaseCommand(0, game.Revision, P()!.PromptId));
        Record(CardKind.Dodge); Record(CardKind.Nullification);
        Need(E<NamedAmbushDepositedEvent>().Select(e => e.Deposit.RecordedKind).SequenceEqual([CardKind.Dodge, CardKind.Nullification]) && V(0).Hand.Count == 0,
            "Both names are issued by actual private-card recording instructions and their real Hand departures.");
        Cold();

        var dodgeIssued = RemoveAmbush();
        var dodge = dodgeIssued.Window.Action;
        var slash = (CardUseFrame)dodgeIssued.Parent;
        Need(dodge is { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, ActorSeat: 2, ProviderSeat: 2, ResponderSeat: 2 } &&
            dodge.ParentActionId == slash.Action!.ActionId && slash.CardAttack?.TargetSeat == 2 &&
            dodge.PhysicalCards is [var dodgeCost] && responseMaterials.Contains(dodgeCost.CardId) && dodgeCost.From == CardLocation.Hand(2) &&
            E<NamedAmbushUseInvalidatedEvent>().Single().Use.ActionId == dodge.ActionId,
            "Actual removal of the Dodge-named deposit invalidates only the accepted defender response on its original Slash.");

        var firstIssued = RemoveAmbush();
        var firstAction = firstIssued.Window.Action;
        var counter = (NullificationWindowFrame)firstIssued.Parent;
        var counterId = counter.Id;
        Need(V(2).Hp == 11 && E<DamageAppliedEvent>().Count(e => e.SourceSeat == 1 && e.TargetSeat == 2 && e.Amount == 1) == 1 &&
            firstAction is { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification, ActorSeat: 2, ProviderSeat: 2, ResponderSeat: 2 } &&
            counter.ChainDepth == 1 && counter.ResponseRuleNode?.Action.ActionId == firstAction.ActionId &&
            counter.NamedAmbushResponse?.ActionId == firstAction.ActionId && !counter.EffectNullified &&
            E<NamedAmbushUseInvalidatedEvent>().Count(e => e.Use.ParentFrameId == counterId && e.Use.ActionId == firstAction.ActionId) == 1,
            "The ineffective physical Dodge causes one real damage; the second actual ambush payment invalidates precisely the first accepted counterspell node.");
        Reach(() => E<CardUseFinishedEvent>().Any(e => e.ResolutionId == counter.ParentFrameId && e.CardKind == CardKind.DrawTwo));
        var offsets = E<NullificationRespondedEvent>().Where(e => e.WindowFrameId == counterId).ToArray();
        var counterActions = E<CardActionAcceptedEvent>().Select(e => e.Action).Where(a => a.Type == CardActionType.Response &&
            a.EffectiveKind == CardKind.Nullification && a.ParentActionId == firstAction.ParentActionId).ToArray();
        Need(offsets is [{ ResponderSeat: 2, ChainDepth: 1, EffectNullified: false }, { ResponderSeat: 2, ChainDepth: 2, EffectNullified: true }] &&
            counterActions is [var first, var next] && first.ActionId == firstAction.ActionId && next.ActionId != first.ActionId &&
            next is { ActorSeat: 2, ProviderSeat: 2, ResponderSeat: 2 } &&
            E<NamedAmbushRemovedEvent>().Length == 2 && E<NamedAmbushUseInvalidatedEvent>().Length == 2 &&
            E<NamedAmbushDepositedEvent>().All(e => game.CardMovements.Count(m => m.CardId == e.Deposit.CardId && m.From == e.Deposit.Location &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.named-ambush.remove") == 1) &&
            responseMaterials.All(id => game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(2) && m.To == CardLocation.Processing) == 1 &&
                game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1) &&
            E<CardUseFinishedEvent>().Count(e => e.ResolutionId == slash.Id) == 1 &&
            E<CardActionAcceptedEvent>().Count(e => e.Action.ActionId == dodge.ActionId || e.Action.ActionId == firstAction.ActionId || e.Action.ActionId == next.ActionId) == 3,
            "Only the first counterspell is ineffective: the distinct later action in the same window offsets DrawTwo, all three physical responses and both ambush cards settle once, and the Slash returns once.");
        Cold();

        PendingDecision? P() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot V(int seat) => game.CreateSnapshot(0, true).Players[seat];
        T[] E<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        bool OptionalAmbush() => P()?.SkillPrompt?.SkillId == Ambush && P()!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        void Answer(Func<PromptChoice, bool> choose)
        { var p = P()!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(choose).Id, game.Revision)); }
        void Record(CardKind kind)
        {
            Reach(OptionalAmbush); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P()?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "card") == true);
            var id = P()!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "card").Cards.Single();
            Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([id]));
            Reach(() => P()?.Choices.Any(c => c.Parameters.GetValueOrDefault("recorded-kind") == kind.ToString()) == true);
            Answer(c => c.Parameters.GetValueOrDefault("recorded-kind") == kind.ToString());
        }
        (ProgramCardTriggerWindowFrame Window, ResolutionFrame Parent) RemoveAmbush()
        {
            Reach(OptionalAmbush); Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P()?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "remove") == true);
            var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
            var parent = game.ResolutionStack.Single(f => f.Id == window.ParentFrameId);
            Cold(); Answer(c => c.Parameters.GetValueOrDefault("branch") == "remove");
            return (window, parent);
        }
        void Accept(GameCommand command)
        { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected actual response cancellation command."); }
        void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 240; i++)
            {
                if (ready()) return;
                if (P() is { PlayerSeat: 0 } p)
                {
                    if (p.Kind == DecisionKind.Nullification) Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    else throw new InvalidOperationException("Unexpected real-response cancellation prompt " + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Missing bounded response cancellation boundary " + Diagnostic());
        }
        string State() => JsonSerializer.Serialize(new { game.State, Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), game.CardMovements });
        void Cold()
        { var before = State(); game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry); Need(before == State(), "Cold replay retains the actual private payment, response action and original native cursor."); }
        string Diagnostic() => JsonSerializer.Serialize(new { Prompt = P(), Frames = game.ResolutionStack, Facts = game.Events.TakeLast(10).Select(e => e.Payload.ToString()) });
        void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + Diagnostic()); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-invalidation-responses", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-opening","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Ambush}}}","revision":1,"triggers":[
                    {"id":"record-first","window":"turnEnding","subject":"owner","optional":true,"effects":[{"op":"recordNamedPrivateAmbush","target":"owner"}]},
                    {"id":"record-second","window":"turnEnding","subject":"owner","optional":true,"effects":[{"op":"recordNamedPrivateAmbush","target":"owner"}]},
                    {"id":"cancel-response","window":"cardUseCommitted","ownerRelation":"observer","includeResponseUses":true,"optional":true,"effects":[{"op":"nullifyMatchingHandUse","target":"owner"}]}]},
                  {"id":"{{{Driver}}}","revision":1,"triggers":[{"id":"forced-response-chain","window":"playEnding","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLivingHighestHand"},{"op":"useVirtualSlash","target":"selectedTarget"},
                      {"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"owner"},"outputKind":"drawTwo"}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Quiet, Ambush, Driver }.ToDictionary(id => id, id => new { name = id, description = "Actual response cancellation" }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Actual ambush owner", "", Ambush, "wei", 12, [Quiet]));
            b.AddGeneral(new(Ally, "Actual response driver", "", Driver, "wei", 12, [Quiet]));
            b.AddGeneral(new(Peer, "Actual defender", "", Quiet, "shu", 12));
            b.AddDeck(new("fixture:native-invalidation-deck", "Fixed response cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Actual response cancellation", 4, 4, new Dictionary<string, int>(), "fixture:native-invalidation-deck",
                GeneralPoolIds: [Hero, Ally, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
