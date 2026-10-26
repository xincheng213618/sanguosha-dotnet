using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileSunQuanChecks
{
    private const string Mobile = "mobile:jiuyuan", Classic = "classic:jiuyuan", Driver = "fixture:mobile-sun-quan-ordinary-driver", Watch = "fixture:mobile-sun-quan-ordinary-hp";
    private const string Fumian = "classic:fumian", Mode = "identity:classic-mobile-sun-quan-ordinary";

    public static void NonDyingAddedPeachTargetUsesActualWuSourceAndPolicyScopeCold()
    {
        foreach (var mobile in new[] { true, false })
        {
            var s = Scene.Start(mobile); var maximum = s.V(0).MaxHp;
            s.Use("lose-two"); s.Play(0); var before = s.V(0).Hp;
            s.Use("ordinary-recovery"); s.HpWatch();
            s.Need(s.V(0).Hp == before + 1 && s.E<NativeCardRecoveryAppliedEvent>().Length == 0,
                "An actual ordinary skill recovery remains one under Jiuyuan; the optional policy never adds to every recovery.");
            s.Continue(); s.Play(0); s.Use("lose-two"); s.Play(0); s.Use("wound-other", [1]); s.Play(0);
            var lordBefore = s.V(0).Hp; var providerBefore = s.V(1).Hp;
            s.Need(s.V(0).Role == Role.Lord && lordBefore == maximum - 3 && lordBefore > 0 && providerBefore == 2 && s.Faction(1) == "wu",
                "The original target is the actual living wounded non-dying Lord; the distinct real Peach user is Wu and is wounded for its implicit self target.");
            s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Fumian && s.P.PlayerSeat == 1 && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Fumian && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "targets"));
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "targets"); s.Play(1);
            var action = s.G.GetLegalActions(1).FirstOrDefault(a => a.Kind == LegalActionKind.Peach && a.ConversionSource is null &&
                a.ProgramActivationId == "red-additional-targets" && a.TargetSeats.SequenceEqual([1, 0]));
            s.Need(action is not null, "The existing real red-target mechanism must expose native Peach with its implicit user and the extra non-dying Lord.");
            var material = action!.CardId!.Value; var expected = mobile ? 2 : 1;
            s.Accept(new PlayCardCommand(1, material, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            s.HpWatch();
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
            var identity = new
            {
                use.Id, use.CardKind, use.SourceSeat, use.DyingResponse, use.TargetIndex, use.TargetSeats,
                Action = use.Action is { } actual ? new { actual.ActionId, actual.Type, actual.ActorSeat, actual.ProviderSeat, actual.EffectiveKind,
                    actual.TargetSeats, actual.EffectiveDesignatedTargetSeats, actual.PhysicalCards } : null,
                use.RecoveryApplication, MaterialLocation = s.Zone(material),
                Moves = s.G.CardMovements.Where(m => m.CardId == material).Select(m => new { m.Sequence, m.From, m.To, m.Reason }).ToArray()
            };
            s.Need(use is { CardKind: CardKind.Peach, SourceSeat: 1, DyingResponse: null } &&
                use.Action is { Type: CardActionType.Use, ActorSeat: 1, ProviderSeat: 1, EffectiveKind: CardKind.Peach } native &&
                native.TargetSeats.SequenceEqual([1, 0]) && native.PhysicalCards is [var physical] && physical.CardId == material && physical.From == CardLocation.Hand(1) &&
                use.TargetSeats.SequenceEqual([1, 0]) && native.EffectiveDesignatedTargetSeats.SequenceEqual([1, 0]),
                "The original physical Peach keeps its actual Wu actor/provider, complete native recipient list and original Hand entity: " + JsonSerializer.Serialize(identity));
            s.Need(
                use.RecoveryApplication is { SourceSeat: 1, TargetSeat: 0, CardKind: CardKind.Peach } receipt && receipt.RequestedAmount == expected &&
                receipt.HpBefore == lordBefore && receipt.HpAfter == lordBefore + expected &&
                s.V(0).Hp == lordBefore + expected && s.V(1).Hp == providerBefore + 1 &&
                s.E<ProgramRecoveryPolicyAppliedEvent>().Count(e => e.SkillId == Mobile && e.OwnerSeat == 0 && e.ProviderSeat == 1 && e.PeachCardId == material) == (mobile ? 1 : 0),
                "The same real Peach recovers its user by one and the extra non-dying Lord by two only with explicit mobile NativePeach scope; classic null scope remains one: " + JsonSerializer.Serialize(identity));
            s.Need(s.Zone(material) == CardLocation.DiscardPile &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) == 1,
                "The last native recipient cleans the paid Peach before FinishCardUse opens its HP child; the child retains the same owning use and finished physical invoice: " + JsonSerializer.Serialize(identity));
            if (mobile) s.Cold();
            s.Continue(); s.Play(1);
            s.Need(s.V(0).Hp == lordBefore + expected && s.V(1).Hp == providerBefore + 1 &&
                s.E<NativeCardRecoveryAppliedEvent>().Count(e => e.CardUseFrameId == use.Id && e.Receipt.TargetSeat == 0 && e.Receipt.RequestedAmount == expected) == 1 &&
                s.E<NativeCardRecoveryAppliedEvent>().Count(e => e.CardUseFrameId == use.Id && e.Receipt.TargetSeat == 1 && e.Receipt.RequestedAmount == 1) == 1 &&
                s.E<RedAdditionalTargetsConsumedEvent>() is [{ OwnerSeat: 1 } consumed] && consumed.Targets.SequenceEqual([1, 0]) &&
                s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material && e.CardKind == CardKind.Peach) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(1) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) == 1,
                "The paid recovery child cold-returns through one original multi-target Peach: each actual recipient recovers once, one original material finishes, and the red-target grant is consumed once without repeating Jiuyuan.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry content, bool mobile)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public string? Faction(int seat) => V(seat).FactionId ?? content.Generals[V(seat).GeneralId].FactionId;
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected native ordinary mobile Sun Quan command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "Missing genuine native Sun Quan choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision)); }
        public void Use(string id, int[]? targets = null) => Accept(new UseProgramSkillCommand(P!.PlayerSeat, Driver, id, [], targets ?? [], G.Revision, P.PromptId));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void HpWatch() => Reach(() => P?.SkillPrompt?.SkillId == Watch);
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } general) { Accept(new SelectGeneralCommand(general.PlayerSeat, General(general.PlayerSeat), G.Revision, general.PromptId)); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } d) { Accept(new DiscardCardsCommand(d.PlayerSeat, d.ValidCardIds.Take(d.RequiredCardCount).ToArray(), d.PromptId, G.Revision)); continue; }
                Need(P is null, "Unexpected real Sun Quan native boundary while seeking its original continuation.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded native Sun Quan boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), content);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) && G.CardMovements.SequenceEqual(copy.CardMovements) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold preserves original Peach action, both recipients, the already paid recovery receipt and the exact HP child."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Mobile = mobile, G.State.TurnNumber, G.State.CurrentSeat,
                Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Take(6).Select(c => new { c.Cards, c.Targets, c.Parameters }).ToArray() } : null,
                Players = Enumerable.Range(0, 2).Select(i => new { Seat = i, V(i).Hp, V(i).MaxHp, SnapshotFaction = V(i).FactionId, EffectiveFixtureFaction = Faction(i), V(i).Role }).ToArray(),
                Facts = G.Events.Select(e => e.Payload).Where(e => e is NativeCardRecoveryAppliedEvent or ProgramRecoveryPolicyAppliedEvent or RecoveryAppliedEvent or RedAdditionalTargetsConsumedEvent)
                    .TakeLast(7).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start(bool mobile)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, r), r, mobile);
            s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-sun-quan-ordinary-general-" + seat;
    private sealed class Fixture(bool mobile) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-sun-quan-ordinary", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "classic:sun-quan", "classic:wu-xian", "mobile:sun-quan");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[
                {"id":"lose-two","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":2}]},
                {"id":"ordinary-recovery","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]},
                {"id":"wound-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]}
              ]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-lord-recovery","window":"afterHpRecovered","subject":"owner","hpChangeOccurrence":"perEvent","optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"recovery-pause","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实孙权场景驱动", description = "原生失体力及普通非桃回复。" },
                    [Watch] = new { name = "真实主公回复子窗", description = "原生回复已生效后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var c = SkillProgramCatalog.Load(rules, labels);
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(General(0), "实际孙权主公", "supporter", mobile ? Mobile : Classic, "wu", 4, [Driver, Watch]));
            b.AddGeneral(new(General(1), "实际吴桃使用者", "supporter", Fumian, "wu", 4, [Driver]));
            for (var seat = 2; seat < 4; seat++) b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:mobile-sun-quan-ordinary-deck";
            b.AddDeck(new(deck, "真实红桃实体", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(Mode, "移动普通孙权原生桃", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 },
                deck, GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
