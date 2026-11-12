using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static class HandLimitExemptionIntegrationChecks
{
    private const string Continuous = "fixture:hand-limit-continuous", Driver = "fixture:hand-limit-driver", Quiet = "fixture:hand-limit-quiet";
    private const string Hero = "fixture:hand-limit-owner", Peer = "fixture:hand-limit-peer", Mark = "fixture:counted-mark";

    public static void RealDeclarationsKeepRecipientAndEntryScopesThroughPartialDiscardCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "hand-limit-shared-integration", Hero, 2, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 1), new(CardKind.Peach, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3),
                new(CardKind.DrawTwo, Suit.Diamond, 4), new(CardKind.FireAttack, Suit.Spade, 5)], [],
            [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
            [new(Peer, Hp: 8) { StartingHand = [] }], new string('d', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        Submit(new StartGameCommand()); Play();
        var continuous = V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var permanent = V(0).Hand.Single(c => c.Kind == CardKind.Peach).Id;
        var entry = V(0).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        Need(V(0).HandCount == 5 && V(0).HandLimit == 2 && Counted(0).Length == 5,
            "The fixed real hand starts with five counted entities and the ordinary HP-based limit two.");
        Skill("mark", []);
        Reach(() => P()?.Choices.Any(c => c.Cards.SequenceEqual([continuous])) == true);
        Answer(c => c.Cards.SequenceEqual([continuous])); Play();
        Need(Counted(0).Length == 4 && !Counted(0).Contains(continuous),
            "An actual markCards instruction activates the current skill's declared mark selector.");
        Issue("permanent", permanent);
        Issue("entry", entry);
        var originalEntry = Capabilities().Get<ICurrentHandEntryQueries>().CurrentHandEntry(0, entry);
        Need(Counted(0).Length == 2 && !Counted(0).Contains(permanent) && !Counted(0).Contains(entry) && originalEntry is not null,
            "The two real grant instructions issue distinct exact-entity and current-Hand-entry rights alongside the continuous declaration.");
        Cold();
        TransferAndReturn(permanent);
        Need(Counted(0).Length == 2 && !Counted(0).Contains(permanent),
            "An issued permanent entity right resumes only for its original recipient after a real give and return.");
        TransferAndReturn(entry);
        Need(Counted(0).Length == 3 && Counted(0).Contains(entry) &&
            Capabilities().Get<ICurrentHandEntryQueries>().CurrentHandEntry(0, entry) != originalEntry &&
            Capabilities().Get<IHandLimitExemptions>().GetMatchingRelations("fixture:entry", 0, entry).Count == 0,
            "Returning the same physical card creates a new Hand entry and cannot revive the old entry-bound exemption.");
        Skill("lose-sources", []); Play();
        var counted = Counted(0);
        Need(V(0).Skills?.All(skill => skill.ContentId != Continuous && skill.ContentId != Driver) == true && counted.Length == 4 &&
            counted.Contains(continuous) && counted.Contains(entry) && !counted.Contains(permanent) &&
            game.GetRuntimeModule<CoreGameEngineTurnHandCategoryRestrictionsRuntime>().GetSelfDiscardableOwnedHand(Owner(0))
                .Any(card => card.Id == permanent),
            "Losing both real sources ends continuous qualification without revoking the issued permanent right; that exempt entity remains legal for an ordinary skill discard cost.");
        Cold();
        Submit(new EndPlayPhaseCommand(0, game.Revision, P()!.PromptId));
        Reach(() => P() is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });
        Need(P()!.RequiredCardCount == 2 && P()!.ValidCardIds.Order().SequenceEqual(counted.Order()),
            "The actual manual obligation and published payment set read the same four counted entities, excluding the issued permanent card.");
        var beforeRejected = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var rejected = game.Submit(new DiscardCardsCommand(0, [permanent], P()!.PromptId, game.Revision));
        Need(!rejected.Accepted && beforeRejected == GameCheckpointJson.Serialize(game.CreateCheckpoint()),
            "A hand-limit payment cannot consume the exempt entity or mutate the original obligation.");
        Submit(new DiscardCardsCommand(0, [continuous], P()!.PromptId, game.Revision));
        Reach(() => P() is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards });
        var partial = game.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        Need(partial is { PaidCount: 1, RemainingCount: 1 } && P()!.RequiredCardCount == 1 &&
            P()!.ValidCardIds.Order().SequenceEqual(Counted(0).Order()) && !Counted(0).Contains(permanent),
            "One actual partial payment preserves the original remaining quota and refreshes only the current counted hand.");
        Cold();
        Submit(new DiscardCardsCommand(0, [entry], P()!.PromptId, game.Revision));
        Reach(() => !game.ResolutionStack.OfType<ManualDiscardFrame>().Any() && P()?.Kind != DecisionKind.DiscardCards);
        Need(V(0).HandCount == 3 && Counted(0).Length == 2 && V(0).Hand.Any(c => c.Id == permanent) &&
            game.CardMovements.Count(m => m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard) == 2 &&
            new[] { continuous, entry }.All(id => game.CardMovements.Count(m => m.CardId == id && m.Reason == CardMoveReasons.HandLimitDiscard) == 1),
            "Both native partial payments settle once: two counted cards remain under limit while the original recipient keeps its independent exempt entity.");
        Cold();

        ProgramCapabilityRegistry Capabilities() => (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        CharacterState Owner(int seat) => Capabilities().Get<IMatchParticipantStateAccess>().At(seat);
        int[] Counted(int seat) => Capabilities().Get<IHandLimitCounting>().GetCountedHand(Owner(seat)).Select(card => card.Id).ToArray();
        PendingDecision? P() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot V(int seat) => game.CreateSnapshot(0, true).Players[seat];
        void Skill(string id, IReadOnlyList<int> cards, IReadOnlyList<int>? targets = null) =>
            Submit(new UseProgramSkillCommand(0, Driver, id, cards, targets ?? [], game.Revision, P()!.PromptId));
        void Answer(Func<PromptChoice, bool> choose)
        { var p = P()!; Submit(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(choose).Id, game.Revision)); }
        void Issue(string activation, int id)
        {
            Skill(activation, []);
            Reach(() => P()?.Choices.Any(c => c.Cards.SequenceEqual([id])) == true);
            Answer(c => c.Cards.SequenceEqual([id])); Play();
        }
        void TransferAndReturn(int id)
        {
            Skill("give", [id], [1]); Play();
            Need(V(1).Hand is [var foreign] && foreign.Id == id && Counted(1).SequenceEqual([id]),
                "A foreign holder receives the actual entity without inheriting another recipient's issued exemption.");
            Cold(); Skill("take", [], [1]);
            Reach(() => P()?.Kind == DecisionKind.PlayCard || P()?.Choices.Any(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand") == true);
            if (P()!.Kind != DecisionKind.PlayCard)
                Answer(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand" && c.Parameters.GetValueOrDefault("slot-index") == "0");
            Play();
            Need(V(0).Hand.Any(c => c.Id == id) && V(1).HandCount == 0 &&
                game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(1)) == 1 &&
                game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(1) && m.To == CardLocation.Hand(0)) == 1,
                "The original entity actually leaves and returns through two native movement instructions once.");
            Cold();
        }
        void Submit(GameCommand command)
        { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected hand-limit integration command."); }
        void Play() => Reach(() => P() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++)
            {
                if (ready()) return;
                if (P() is { PlayerSeat: 0 }) throw new InvalidOperationException("Unexpected hand-limit integration prompt " + Diagnostic());
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Missing bounded hand-limit integration boundary " + Diagnostic());
        }
        string State() => JsonSerializer.Serialize(new { game.State, Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), game.CardMovements, Counted = Counted(0) });
        void Cold()
        { var before = State(); game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry); Need(before == State(), "Cold replay preserves issued scopes, current entry, source loss and the native partial obligation."); }
        string Diagnostic() => JsonSerializer.Serialize(new { Prompt = P(), Frames = game.ResolutionStack, Counted = Counted(0), Facts = game.Events.TakeLast(8).Select(e => e.Payload.ToString()) });
        void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + Diagnostic()); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-hand-limit-integration", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Continuous}}}","revision":1,"cardPolicies":[{"id":"marked-hand","kind":"handLimitCardKindExemption","handLimitMatch":{"mark":"{{{Mark}}}"}}]},
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"mark","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"marked"},
                      {"op":"revealBoundCards","target":"owner","sourceBind":"marked"},{"op":"markCards","target":"owner","mark":"{{{Mark}}}","bind":"marked"}]},
                    {"id":"permanent","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"chosen"},
                      {"op":"revealBoundCards","target":"owner","sourceBind":"chosen"},{"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:permanent","cardsBind":"chosen"}]},
                    {"id":"entry","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"chosen"},
                      {"op":"revealBoundCards","target":"owner","sourceBind":"chosen"},{"op":"grantHandLimitExemption","target":"owner","relationId":"fixture:entry","cardsBind":"chosen","requireCurrentHandEntry":true}]},
                    {"id":"give","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"giveSelected","target":"selectedTarget","amount":1}]},
                    {"id":"take","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":null,"effects":[
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"ownerHand","awaitMovementTriggers":true}]},
                    {"id":"lose-sources","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Continuous}}}","{{{Driver}}}"]}}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Quiet, Continuous, Driver }.ToDictionary(id => id, id => new { name = id, description = "Shared hand-limit relation integration" }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Shared counted hand", "", Continuous, "wei", 2, [Driver, Quiet]));
            b.AddGeneral(new(Peer, "Shared foreign holder", "", Quiet, "shu", 8));
            b.AddDeck(new("fixture:hand-limit-integration-deck", "Fixed counted hand", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Shared hand-limit integration", 4, 4, new Dictionary<string, int>(), "fixture:hand-limit-integration-deck",
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
