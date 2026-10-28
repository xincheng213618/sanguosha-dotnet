using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheSourceQingjianNativeChecks
{
    private const string Skill = "shanhe:source-qingjian-721", Driver = "fixture:source-qingjian-driver";
    private const string Watch = "fixture:source-qingjian-watch", Mutation = "fixture:source-qingjian-mutation", Quiet = "fixture:source-qingjian-quiet";
    private const string DrawPeer = "fixture:source-qingjian-draw-peer";
    private const string Owner = "fixture:source-qingjian-owner", Peer = "fixture:source-qingjian-peer", Deck = "fixture:source-qingjian-deck";
    private const string Acquire = "skill-program.obtain-deck-cards", Give = "skill-program.shanhe-source-qingjian-721.give";

    public static void RepeatedBatchesOriginalEntitiesRealGiveChildrenAndDrawOwnerQualification()
    {
        var s = Scene.Start();
        Need(s.E<ProgramBindingStartedEvent>().All(e => e.SkillId != Skill) && s.V(0).HandCount == 4 &&
            s.G.CardMovements.Any(m => m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.InitialDeal),
            "Real initial deal follows the existing no-gain-window pipeline; the owner's natural Draw gains also do not offer Qingjian.");
        var old = s.V(0).Hand.Select(card => card.Id).ToHashSet();
        s.Use("gain-two"); s.Offer(); s.Activate();
        var frame = s.Active;
        var ids = frame.ShanheSourceQingjian!.OriginalCardIds.ToArray();
        Need(ids.Length == 2 && ids.All(id => !old.Contains(id)) && s.P!.ValidCardIds.Order().SequenceEqual(ids.Order()) &&
            frame.ShanheSourceQingjian.OriginalCardIds is IList<int> { IsReadOnly: true },
            "One actual two-card gain exposes only its original current Hand entities, excluding older hand cards.");
        s.GiveTo(ids[0], 1); s.WatchChild();
        var child = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == Watch);
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == frame.Id);
        Need(parent.ShanheSourceQingjian is { ReturningChild: true, PaidCardId: var card, RecipientSeat: 1 } receipt && card == ids[0] &&
            parent.PendingMovementContinuation is not null && child.WindowContext?.MovementBatch is { } batch &&
            batch.OriginSkillId == Skill && batch.OriginOwnerSeat == 0 && batch.Movements.Any(m => m.CardId == ids[0] && m.To == CardLocation.Hand(1)) &&
            s.V(0).Armor == 0 && receipt.GivenCardIds.SequenceEqual([ids[0]]),
            "The real recipient gain child pauses under the original paid Qingjian frame, before its parent acquisition continuation.");
        s.ContinueWatch(); s.Distribution(); s.GiveTo(ids[1], 2); s.WatchChild(); s.ContinueWatch(); s.Play();
        Need(s.E<ShanheSourceQingjianGivenEvent>().Length == 2 && s.V(0).Armor == 1 &&
            ids.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.Reason.Value == Give) == 1),
            "The original two cards are really distributed to different recipients once, then the original acquisition resumes once.");

        s.Use("gain-one"); s.Offer(); s.Activate();
        var second = s.Active.ShanheSourceQingjian!;
        Need(second.OriginalCardIds.Count == 1 && second.BatchId != frame.ShanheSourceQingjian.BatchId && s.G.State.TurnNumber == 1,
            "A later one-card batch in the same actual turn independently offers Qingjian without a quota or a two-card minimum.");
        var kept = second.OriginalCardIds.Single(); s.Finish(); s.Play();
        Need(s.Zone(kept) == CardLocation.Hand(0) && s.E<ShanheSourceQingjianGivenEvent>().Length == 2 && s.V(0).Armor == 2,
            "Cancelling before any transfer retains the original gained entity and pays no give movement.");

        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == DrawPeer && s.P.Choices.Any(c => c.Targets.SequenceEqual([0])));
        Need(s.G.State is { CurrentSeat: 1, Phase: TurnPhase.Draw } && s.P!.PlayerSeat == 1,
            "The other actor's actual Draw-phase entry owns the legal recipient selection child while its Draw phase is active. " + s.Diagnostic());
        s.Answer(c => c.Targets.SequenceEqual([0])); s.Offer();
        Need(s.G.State is { CurrentSeat: 1, Phase: TurnPhase.Draw } && s.P!.PlayerSeat == 0,
            "A real gain during another actor's actual Draw phase still offers this owner's Qingjian. " + s.Diagnostic());
        s.Activate(); var foreignDraw = s.Active.ShanheSourceQingjian!.OriginalCardIds.Single();
        s.GiveTo(foreignDraw, 2); s.WatchChild(); s.ContinueWatch();
        s.Until(() => s.E<ShanheSourceQingjianGivenEvent>().Length == 3 && !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShanheSourceQingjian is not null));
        Need(s.Zone(foreignDraw) == CardLocation.Hand(2), "The other-Draw gain is immediately given as its real original entity.");
    }

    public static void EarlierGainChildRemovalLeavesOnlyTheRemainingOriginalBatchCards()
    {
        var s = Scene.Start(removeFirst: true);
        var old = s.V(0).Hand.Select(card => card.Id).ToHashSet();
        s.Use("gain-two");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Mutation && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        var mutation = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == Mutation);
        var originals = mutation.WindowContext!.MovementBatch!.Movements.Where(m => m.To == CardLocation.Hand(0)).Select(m => m.CardId).Distinct().ToArray();
        Need(originals.Length == 2 && originals.All(id => !old.Contains(id)), "The earlier real gain child belongs to the original two-card batch.");
        s.Answer(c => c.Cards.SequenceEqual([originals[0]])); s.Offer(); s.Activate();
        Need(s.Zone(originals[0]) == CardLocation.DiscardPile && s.Active.ShanheSourceQingjian!.OriginalCardIds.SequenceEqual([originals[1]]) &&
            s.P!.ValidCardIds.SequenceEqual([originals[1]]),
            "A card moved away by the earlier real child is excluded; no old Hand card substitutes for the missing original.");
        s.GiveTo(originals[1], 1); s.WatchChild(); s.ContinueWatch(); s.Play();
        Need(s.Zone(originals[0]) == CardLocation.DiscardPile && s.Zone(originals[1]) == CardLocation.Hand(1) &&
            s.E<ShanheSourceQingjianGivenEvent>() is [{ CardId: var given }] && given == originals[1],
            "Only the still-present original card transfers once and the first paid discard remains intact.");
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 3).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame Active => G.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.ShanheSourceQingjian is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;
        public void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Rejected native Qingjian command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "Missing Qingjian native choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision)); }
        public void Use(string id) => Accept(new UseProgramSkillCommand(0, Driver, id, [], [], G.Revision, P!.PromptId));
        public void Offer() => Until(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); Distribution(); }
        public void Distribution() => Until(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-qingjian-distribute") == true);
        public void GiveTo(int card, int recipient) => Answer(c => c.Cards.SequenceEqual([card]) && c.Targets.SequenceEqual([recipient]));
        public void Finish() => Answer(c => c.Parameters.GetValueOrDefault("branch") == "finish");
        public void WatchChild() => Until(() => P?.SkillPrompt?.SkillId == Watch);
        public void ContinueWatch() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var i = 0; i < 180; i++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "Native Qingjian stays in a live short encounter.");
                if (P is { } p)
                {
                    if (p is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 }) Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId));
                    else if (p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage") is { } response)
                        Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, response.Id, G.Revision));
                    else throw new InvalidOperationException("Unexpected Qingjian native boundary. " + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Qingjian exceeded bounded native progression. " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
            Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null,
            Players = Enumerable.Range(0, 3).Select(seat => new { Seat = seat, V(seat).IsHuman }),
            DrawPeerFrames = G.ResolutionStack.OfType<ProgramSkillFrame>().Where(f => f.SkillId == DrawPeer)
                .Select(f => new { f.Id, f.OwnerSeat, f.SkillId, f.InstructionIndex, f.WindowContext, f.SelectedTargetSeats }),
            Stack = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool removeFirst = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(removeFirst));
            var setup = new ShanheBattleSetup(93, 1, "qingjian-native", Owner, 20, 0, 0, 0, 0, [], [new(CardKind.Dodge, Suit.Heart, 7)], [],
                [new(Peer), new(Peer)], [], new string('e', 64));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanSeats = [0, 1, 2], HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 7 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(bool removeFirst) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-qingjian", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            string Read(string part)
            { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-source-qingjian-721." + part + ".json")!; using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
            var source = SkillProgramCatalog.Load(Read("rules"), Read("presentation"));
            b.AddSkill(new(Skill, source.Presentations[Skill].Name, source.Presentations[Skill].Description) { Program = source.Programs[Skill], ProgramPresentation = source.Presentations[Skill] });
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"gain-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"obtainDeckCards","target":"owner","amount":2,"cardKinds":["slash"]},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"gain-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"cardKinds":["slash"]},{"op":"grantArmor","target":"owner","amount":1}]}]},
                  {"id":"{{{DrawPeer}}}","revision":1,"triggers":[{"id":"real-own-draw-recipient","window":"drawPhaseStarting","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"draw","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"actual-recipient-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{Give}}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{{Mutation}}}","revision":1,"triggers":[{"id":"first-original-gain-leaves","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{Acquire}}}"],"priority":200,"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"wait","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Watch, Mutation, Quiet, DrawPeer }.ToDictionary(id => id, id => new { name = id, description = "清俭真实获牌和分配孩子", optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) });
            var fixture = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in fixture.Programs) b.AddSkill(new(id, fixture.Presentations[id].Name, fixture.Presentations[id].Description) { Program = program, ProgramPresentation = fixture.Presentations[id] });
            b.AddGeneral(new(Owner, "原清俭持有者", "supporter", Driver, "shu", 20, removeFirst ? [Skill, Mutation] : [Skill]));
            b.AddGeneral(new(Peer, "真实分配对象", "supporter", Quiet, "wei", 20, [Watch, DrawPeer]));
            b.AddDeck(new(Deck, "固定原牌", 1, 2, [new("standard:slash", 96)]));
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "清俭原生三人阵", 3, 3, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:qingjian-human-setup"; public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) => new StandardModeSetupFactory().ValidateInitialOptions(options, phase);
        public IGameModeFeature Create(ModeFeatureCreationContext context) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(context));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:qingjian-human-profile"; public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(c.Options! with { HumanSeats = null }, c.ModeDefinition!, c.ContentRegistry,
            new ModeHostRegistry().Register<IModeSetupRandom>(() => c.GetHost<IModeSetupRandom>()).Register<IModeNationalRevealHost>(() => c.GetHost<IModeNationalRevealHost>()).Register<IModeSetupEquipmentHost>(() => c.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers() { var p = inner.BuildPlayers(); return new(p.Seats.Select(seat => seat with { IsHuman = true }), p.AvailableGeneralIds, p.SelectionOrder, p.ShenwuSharedGodIds, p.ShenwuLeaderGeneralId, p.GuanduConfiguration, p.InitialActorSeat); }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest r) => inner.GetGeneralOffers(r);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) => inner.PrepareSelectedGeneral(seat, generalId);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> cards) => inner.PrepareDeck(cards);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> cards) => inner.ProjectDeckCardNames(cards);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> cards, IReadOnlyList<ModeSeatSetupFacts> seats) => inner.PrepareSelectedEquipment(cards, seats);
    }
}
