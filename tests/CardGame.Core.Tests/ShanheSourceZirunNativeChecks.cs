using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceZirunNativeChecks
{
    private const string Skill = StandardShanheSourceScholarsPackage.ZirunSkillId;
    private const string Actor = "fixture:source-zirun-owner", First = "fixture:source-zirun-first", Second = "fixture:source-zirun-second", Last = "fixture:source-zirun-last";
    private const string Watch = "fixture:source-zirun-watch", Deck = "fixture:source-zirun-deck", Reason = "shanhe.source-zirun-1172.draw";

    public static void ActualPreparationDrawsWaitForChildrenAndReadEachRecipientsCurrentEquipment()
    {
        var s = Scene.Start();
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Targets.Count == 1));
        var first = s.Seat(First); var second = s.Seat(Second); var last = s.Seat(Last);
        var order = new[] { 0, first, second, last }.OrderBy(seat => seat).ToArray();
        var owner = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var window = s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(f => f.Id == owner.WindowContext!.ParentFrameId);
        var gear = s.V(first).Equipment.Single().Id;
        Need(owner.ShanheSourceZirun is { Index: 0, RequestedCount: 1, ReturningChildren: true } receipt &&
            receipt.PreparationWindowId == window.Id && receipt.ActualTurn == 1 && receipt.Recipients.SequenceEqual(order) &&
            receipt.Recipients is System.Collections.IList { IsReadOnly: true } &&
            s.E<ShanheSourceZirunRequestedEvent>() is [{ RecipientSeat: 0, RequestedCount: 1, EquipmentCount: 0 }] &&
            s.E<ShanheSourceZirunReturnedEvent>() is [{ RecipientSeat: 0, RequestedCount: 1, ActualCount: 1 }] &&
            owner.NativeDraw is null && owner.NativeOperationDrawReturns is [{ Operation: SkillProgramEffectOp.ResolveShanheSourceZirun } paid] &&
            paid.Outcome.ActualDrawIds.Count == 1 && s.V(second).Equipment.Count == 1 &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != owner.Id),
            "The original own Preparation and first real one-card draw remain paid under their actual gain child, before any later recipient is visited. " + s.Diagnostic());
        var material = owner.NativeOperationDrawReturns.Single().Outcome.ActualDrawIds.Single();
        Need(s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.DrawPile &&
            m.To == CardLocation.Hand(0) && m.Reason.Value == Reason) == 1,
            "The first native return proves the actual DrawPile entity moved exactly once to the owner.");

        s.Answer(c => c.Targets.SequenceEqual([first]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Cards.SequenceEqual([gear])));
        s.Answer(c => c.Cards.SequenceEqual([gear]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var paused = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == owner.Id);
        Need(s.V(first).Equipment.Count == 0 && s.V(second).Equipment.Count == 1 &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == gear).Location == CardLocation.DiscardPile &&
            paused.ShanheSourceZirun is { Index: 0, RequestedCount: 1, ReturningChildren: true } &&
            s.E<ShanheSourceZirunRequestedEvent>().Length == 1 &&
            paused.NativeOperationDrawReturns.Single().Outcome.ActualDrawIds.SequenceEqual([material]),
            "A real earlier-recipient child removes the later actor's actual equipment while the same paid draw remains suspended, without reissuing it.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var requested = s.E<ShanheSourceZirunRequestedEvent>();
        var returns = s.E<ShanheSourceZirunReturnedEvent>();
        Need(requested.Select(e => e.RecipientSeat).SequenceEqual(order) && requested.All(e => e.FrameId == owner.Id) &&
            requested.Single(e => e.RecipientSeat == first) is { EquipmentCount: 0, RequestedCount: 1 } &&
            requested.Single(e => e.RecipientSeat == second) is { EquipmentCount: 1, RequestedCount: 2 } &&
            requested.Single(e => e.RecipientSeat == last) is { EquipmentCount: 0, RequestedCount: 1 } &&
            returns.Length == 4 && returns.All(e => e.ActualCount == e.RequestedCount && !e.Replaced) &&
            returns.Select(e => e.Index).SequenceEqual([0, 1, 2, 3]) &&
            s.G.CardMovements.Count(m => m.Reason.Value == Reason && m.From == CardLocation.DrawPile) == 5 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == owner.Id && e.Completed) == 1 &&
            s.G.State is { CurrentSeat: 0, TurnNumber: 1, Phase: TurnPhase.Play } && s.G.State.Winner == Winner.None,
            "After each real draw child returns, the original Preparation visits the next seat once using its equipment at that visit, then resumes the ordinary Draw and Play flow. " + s.Diagnostic());
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public int Seat(string id) => G.CreateSnapshot(0).Players.Single(p => p.GeneralId == id).Seat;
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, (r.Error?.Message ?? "Rejected source1172 native command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = P!; var choice = p.Choices.FirstOrDefault(match); Need(choice is not null, "Missing source1172 native choice. " + Diagnostic()); Accept(new AnswerPromptCommand(0, p.PromptId, choice!.Id, G.Revision)); }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 180; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fresh short draw encounter stays live. " + Diagnostic());
                if (P is { PlayerSeat: 0 }) throw new InvalidOperationException("Unexpected source1172 native boundary. " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source1172 exceeded bounded native progression. " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
            Stack = G.ResolutionStack.Select(f => new { f.Id, Kind = f.GetType().Name }),
            Requests = E<ShanheSourceZirunRequestedEvent>(), Returns = E<ShanheSourceZirunReturnedEvent>() });
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(183, 1, "source-zirun-native", Actor, 8, 0, 0, 0, 0, [], [], [],
                [new(First) { StartingHand = [], StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)] },
                 new(Second) { StartingHand = [], StartingEquipment = [new(CardKind.QinggangSword, Suit.Spade, 6)] },
                 new(Last) { StartingHand = [] }], [], new string('b', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { PlayerGeneralId = StandardShanheSourceScholarsPackage.CaiYongHellId,
                EnemyRoster = [new("classic:guan-yu"), new("classic:guan-yu"), new("classic:guan-yu")] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, First, Second, Last]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-zirun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"first-real-owner-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Reason}}"],"usageScope":"game","usageLimit":1,"optional":false,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true},
                      {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实首位摸牌孩子", description = "在后续角色被访问前实际移走其装备。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
            { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Actor, "滋润实际持有者", "supporter", Skill, "qun", 8, [Watch]));
            foreach (var id in new[] { First, Second, Last }) builder.AddGeneral(new(id, id, "supporter", null, "wei", 8));
            builder.AddDeck(new(Deck, "真实小摸牌堆", 0, 2, [new("standard:dodge", 48)]));
        }
    }
}
