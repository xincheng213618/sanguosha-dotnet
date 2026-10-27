using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceNamanChecks
{
    private const string Skill = StandardShanheSourceRangerPackage.NamanSkillId;
    private const string Peer = "fixture:source-naman-peer", Watch = "fixture:source-naman-gain-child", Deck = "fixture:source-naman-deck";
    private const string ClaimReason = "skill-program." + Skill + ".naman-response-claim";

    public static void ActualCompletedSlashClaimsPhysicalAndConvertedMaterialsAndReturnsToParent()
    {
        foreach (var converted in new[] { false, true })
        {
            var s = Scene.Start(converted);
            var materials = s.G.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToArray();
            var arrows = s.V.Hand.Single(card => card.Kind == CardKind.BarbarianAssault);
            var action = s.G.GetHumanLegalActions().Single(candidate => candidate.CardId == arrows.Id && candidate.ConversionSource is null);
            s.Accept(new PlayCardCommand(0, arrows.Id, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            s.Until(() => s.P?.SkillPrompt?.SkillId == Skill);
            var parent = s.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == arrows.Id);
            var response = s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
            Need(response.ParentFrameId == parent.ResolutionId && response.ResponseCompletion is { CostsDrained: true, CompletionActorSeat: 1 } &&
                response.Action.Type == CardActionType.Response && response.Action.ProviderSeat == 1 &&
                response.Action.PhysicalCards.Select(cost => cost.CardId).Order().SequenceEqual(materials.Order()) &&
                response.Action.PhysicalCards.Count == (converted ? 2 : 1) &&
                (converted ? response.Action.PhysicalCards.All(cost => cost.CardKind == CardKind.Dodge) :
                    response.Action.PhysicalCards.Single().CardKind == CardKind.ColdSlash) &&
                materials.All(id => NativePaymentCount(s, id, CardLocation.Hand(1), CardLocation.Processing) == 1 &&
                    NativePaymentCount(s, id, CardLocation.Processing, CardLocation.DiscardPile) == 1) &&
                s.Events<CardUseFinishedEvent>().All(fact => fact.ResolutionId != parent.ResolutionId),
                "Naman waits for the real other player's fully paid Slash response and its original group parent: attribute Slash and the two non-Slash Zhangba materials retain exact native identities.");
            s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
            var claim = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
            Need(materials.All(id => s.V.Hand.Any(card => card.Id == id)) &&
                s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(frame => frame.Id == response.Id && frame.Action.ActionId == response.Action.ActionId) &&
                s.G.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == parent.ResolutionId) &&
                s.Events<ProgramBindingResolvedEvent>().All(fact => fact.FrameId != claim.Id) &&
                s.Events<CardUseFinishedEvent>().All(fact => fact.ResolutionId != parent.ResolutionId),
                "The exact discarded response materials enter Naman's hand once while its real gain child suspends the same response, claim and original group use.");

            var children = new HashSet<long>();
            while (s.P?.SkillPrompt?.SkillId == Watch)
            {
                var child = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
                Need(children.Count < materials.Length && children.Add(child.Id) &&
                    s.G.CardMovements.Count(move => move.Reason.Value == ClaimReason) == materials.Length,
                    "Only the actual paid hand-gain children return; no child can issue another Naman claim.");
                s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
                s.Until(() => s.P?.SkillPrompt?.SkillId == Watch ||
                    s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.ResolutionStack.Count == 0);
            }
            Need(s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == claim.Id && fact.Completed) == 1 &&
                children.All(id => s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == id && fact.Completed) == 1) &&
                s.Events<CardResponseCompletedEvent>().Count(fact => fact.ActionId == response.Action.ActionId) == 1 &&
                s.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == parent.ResolutionId) == 1 &&
                s.Events<CardActionAcceptedEvent>().Count(fact => fact.Action.ActionId == response.Action.ActionId) == 1 &&
                s.G.CardMovements.Count(move => move.Reason.Value == ClaimReason) == materials.Length &&
                materials.All(id => s.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.DiscardPile &&
                        move.To == CardLocation.Hand(0) && move.Reason.Value == ClaimReason) == 1 &&
                    NativePaymentCount(s, id, CardLocation.Hand(1), CardLocation.Processing) == 1 &&
                    NativePaymentCount(s, id, CardLocation.Processing, CardLocation.DiscardPile) == 1) &&
                s.Events<DamageAppliedEvent>().Length == 0 && s.G.State is { TurnNumber: 1, CurrentSeat: 0, Phase: TurnPhase.Play },
                "All original claim children and the same native response/group parent finish once without replaying a response, paying materials twice, inventing damage or starting another turn.");
        }
    }

    private static int NativePaymentCount(Scene scene, int id, CardLocation from, CardLocation to) =>
        scene.G.CardMovements.Count(move => move.CardId == id && move.From == from && move.To == to);
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual source231 command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, G.Revision)); }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None && P is null, "Unexpected source231 native prompt: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing source231 original response return: " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P,
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name }),
            GroupResponses = Events<GroupResponseEvent>(), CompletedResponses = Events<CardResponseCompletedEvent>(),
            MaterialMoves = G.CardMovements.Where(move => move.From == CardLocation.Hand(1) ||
                move.From == CardLocation.Processing || move.Reason.Value == ClaimReason).Select(move =>
                    new { move.CardId, move.CardKind, move.From, move.To, move.Reason }),
            Players = G.CreateSnapshot(0).Players.Select(player => new { player.Seat, player.GeneralId, player.Hp, player.MaxHp, player.HandCount }) });
        public static Scene Start(bool converted)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-naman", StandardShanheSourceRangerPackage.HellId,
                4, 0, 0, 0, 0, [Watch], [new(CardKind.BarbarianAssault, Suit.Spade, 7)], [],
                // At full 8/8 HP the public AI intentionally takes one damage
                // instead of spending a Slash; lethal pressure requests the real response.
                [new(Peer, Hp: 1) { StartingHand = converted ? [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Heart, 3)] :
                        [new(CardKind.ColdSlash, Suit.Spade, 4)],
                    StartingEquipment = converted ? [new(CardKind.ZhangbaSerpentSpear, Suit.Spade, 12)] : [] }], [], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [], EnemyRoster = [new("classic:guan-yu")] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Append(Peer).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && scene.G.ResolutionStack.Count == 0);
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-naman", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"actual-naman-hand-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{ClaimReason}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实纳蛮获得牌孩子", description = "完成后返回原响应与南蛮入侵。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "真实原生打出杀的目标", "supporter", null, "wei", 8));
            var coldSlash = CardCatalog.Get(CardKind.ColdSlash);
            builder.AddCard(new("fixture:source-naman-cold-slash", coldSlash.DisplayName, coldSlash.CategoryName,
                coldSlash.Description, LegacyKind: CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "零随机起手和摸牌的真实牌堆", 0, 0, [new("standard:dodge", 24)]));
        }
    }
}
