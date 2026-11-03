using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class SkillProgramTargetOrderChecks
{
    private const string RedirectSkill = "fixture:redirect-pair", RedirectDriver = "fixture:redirect-driver",
        RedirectQuiet = "fixture:redirect-quiet", RedirectMovement = "fixture:redirect-movement",
        RedirectOwner = "fixture:redirect-owner";

    private static void RedirectTargetsAndPaymentsSharePostCostRange()
    {
        foreach (var gear in new[] { CardKind.FangtianHalberd, CardKind.OffensiveHorse })
        {
            var onlyGear = RedirectScene.Start(gear, handCost: false);
            onlyGear.Request();
            Require(!onlyGear.P!.Choices.Any(c => c.Targets.SequenceEqual([2])) &&
                    onlyGear.P.Choices.Any(c => c.Targets.SequenceEqual([1])),
                "The only equipped payment cannot justify a far target with its own pre-discard weapon range or outgoing-horse reduction.");
            onlyGear.Answer(c => c.Targets.SequenceEqual([1]));
            onlyGear.Reach(() => onlyGear.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var id = onlyGear.V(0).Equipment.Single().Id;
            onlyGear.Answer(c => c.Cards.SequenceEqual([id])); onlyGear.Play();
            Require(onlyGear.V(0).Hp == 8 && onlyGear.V(1).Hp == 7 &&
                    onlyGear.G.CardMovements.Count(m => m.CardId == id && m.To == CardLocation.DiscardPile) == 1,
                "An adjacent target remains legal after the real one-card equipment payment and receives the original Slash once.");

            var hand = RedirectScene.Start(gear, handCost: true);
            hand.Request();
            Require(hand.P!.Choices.Any(c => c.Targets.SequenceEqual([2])),
                "A legal hand payment keeps the equipped range source, so the same far target is offered.");
            hand.Answer(c => c.Targets.SequenceEqual([2]));
            hand.Reach(() => hand.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var equipment = hand.V(0).Equipment.Single().Id;
            var cost = hand.V(0).Hand.Single().Id;
            Require(hand.P!.Choices.Any(c => c.Cards.SequenceEqual([cost])) &&
                    !hand.P.Choices.Any(c => c.Cards.Contains(equipment)),
                "The payment prompt uses the same target/material relation: hand is viable, the range-producing equipment is not.");
            hand.Cold(); hand.Answer(c => c.Cards.SequenceEqual([cost])); hand.Play();
            Require(hand.V(0).Hp == 8 && hand.V(2).Hp == 7 && hand.V(0).Equipment.Single().Id == equipment &&
                    hand.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == 1,
                "Cold recovery keeps the selected far target and original hand payment without consuming its retained range equipment.");
        }

        var exempt = RedirectScene.Start(null, handCost: true);
        exempt.Use("incoming-exemption", [3, 2]); exempt.Play(); exempt.Request();
        Require(!exempt.P!.Choices.Any(c => c.Targets.SequenceEqual([2])) &&
                exempt.P.Choices.Any(c => c.Targets.SequenceEqual([1])),
            "The original attacker's issued IgnoreDistance right does not substitute for the redirector's own attack range.");
        exempt.Answer(c => c.Targets.SequenceEqual([1]));
        exempt.Reach(() => exempt.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        exempt.Answer(c => c.Cards.Count == 1); exempt.Play();

        var changed = RedirectScene.Start(CardKind.FangtianHalberd, handCost: true, removeRange: true);
        changed.Request(); changed.Answer(c => c.Targets.SequenceEqual([2]));
        changed.Reach(() => changed.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        var paid = changed.V(0).Hand.Single().Id;
        changed.Answer(c => c.Cards.SequenceEqual([paid]));
        changed.Reach(() => changed.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue") == true);
        Require(changed.V(0).Equipment.Count == 0 && changed.V(0).HandCount == 0,
            "The original payment's real movement child removes the remaining range equipment before the redirect instruction resumes.");
        changed.Cold(); changed.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); changed.Play();
        Require(changed.V(0).Hp == 7 && changed.V(2).Hp == 8 &&
                changed.G.CardMovements.Count(m => m.CardId == paid && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1,
            "An invalidated paid redirect cancels without refund or reselection; the original target takes the original Slash and the cost is paid once.");
        changed.Cold();

        var noPair = RedirectScene.Start(CardKind.FangtianHalberd, handCost: false);
        noPair.Use("incoming-prohibition", [3, 1]); noPair.Play();
        noPair.Use("request", [3]); noPair.Play();
        Require(noPair.V(0).Hp == 7 && noPair.V(0).Equipment.Single().Kind == CardKind.FangtianHalberd &&
                !noPair.G.Events.Any(e => e.Payload is ProgramCardTriggerResolvedEvent { SkillId: RedirectSkill, Activated: true }),
            "Qualification requires a complete target/payment pair: the source's real target ban excludes the only adjacent recipient, and the sole weapon cannot pay for the remaining far target.");

        var gainedRange = RedirectScene.Start(CardKind.Crossbow, handCost: false, virtualAxe: true);
        Require(gainedRange.G.GetAttackRange(0) == 1 && gainedRange.G.GetCombatDistance(0, 2) == 2,
            "The real equipped Crossbow suppresses Zhanjin's virtual Stone Axe before its payment.");
        gainedRange.Request();
        Require(gainedRange.P!.Choices.Any(c => c.Targets.SequenceEqual([2])),
            "Qualification projects the actual post-payment range: discarding the sole weapon enables the declared virtual axe and makes the far target legal.");
        gainedRange.Answer(c => c.Targets.SequenceEqual([2]));
        gainedRange.Reach(() => gainedRange.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        var crossbow = gainedRange.V(0).Equipment.Single().Id;
        gainedRange.Answer(c => c.Cards.SequenceEqual([crossbow])); gainedRange.Play();
        Require(gainedRange.G.GetAttackRange(0) == 3 && gainedRange.V(0).Hp == 8 && gainedRange.V(2).Hp == 7 &&
                gainedRange.G.CardMovements.Count(m => m.CardId == crossbow && m.To == CardLocation.DiscardPile) == 1,
            "The same equipment pair remains legal at payment and redirect, and the actual removal activates the shared virtual-weapon range once.");

        var empty = RedirectScene.Start(null, handCost: false);
        empty.Use("request", [3]); empty.Play();
        Require(empty.V(0).Hp == 7 && !empty.G.Events.Any(e => e.Payload is ProgramCardTriggerResolvedEvent { SkillId: RedirectSkill, Activated: true }),
            "No viable owned payment means no redirect opportunity, rather than a target choice that cannot be paid.");
    }

    private sealed class RedirectScene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public void Use(string id, int[] targets) => Accept(new UseProgramSkillCommand(0, RedirectDriver, id, [], targets, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) => Accept(new AnswerPromptCommand(0, P!.PromptId, P.Choices.First(predicate).Id, G.Revision));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Request()
        {
            Use("request", [3]);
            Reach(() => P?.SkillPrompt?.SkillId == RedirectSkill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 140; step++)
            {
                if (ready()) return;
                Require(G.State.Winner == Winner.None, "Redirect fixture ended before its expected boundary.");
                if (P is not { PlayerSeat: 0 }) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (P.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)
                    Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                else throw new InvalidOperationException("Unexpected redirect fixture boundary: " + Diagnostic());
            }
            throw new InvalidOperationException("Missing bounded redirect fixture boundary: " + Diagnostic());
        }
        public void Cold()
        {
            var before = Diagnostic();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Require(before == Diagnostic(), "Original target/payment cursor and real movement children survive command-prefix cold recovery.");
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), G.CardMovements });
        private void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Require(result.Accepted, "Rejected redirect command " + command.GetType().Name + ": " + JsonSerializer.Serialize(result.Error));
        }
        public static RedirectScene Start(CardKind? gear, bool handCost, bool removeRange = false, bool virtualAxe = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(),
                new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
                new StandardClassicGeneralPackage(), new RedirectFixture(removeRange, virtualAxe));
            static ShanheStartingCard C(CardKind kind) => new(kind, Suit.Club, 7);
            var setup = new ShanheBattleSetup(83, 1, "redirect-pair", RedirectOwner, 8, 0, 0, 0, 0, [],
                handCost ? [C(CardKind.Peach)] : [], gear is { } kind ? [C(kind)] : [],
                [new("fixture:redirect-enemy") { StartingHand = [] }, new("fixture:redirect-source") { StartingHand = [C(CardKind.Slash)] }],
                [new("fixture:redirect-ally") { StartingHand = [] }], new string('b', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry);
            var scene = new RedirectScene(game, registry); scene.Accept(new StartGameCommand()); scene.Play();
            Require(scene.V(3).GeneralId == "fixture:redirect-source" && scene.V(2).GeneralId == "fixture:redirect-enemy" &&
                    scene.G.GetCombatDistance(0, 2) == (gear == CardKind.OffensiveHorse ? 1 : 2),
                "The fixed four-seat public fixture places the real incoming source next to the owner and the redirected enemy opposite it.");
            return scene;
        }
    }

    private sealed class RedirectFixture(bool removeRange, bool virtualAxe) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-redirect-pair", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var movement = removeRange ? $$"""
                ,{"id":"{{RedirectMovement}}","revision":1,"triggers":[{"id":"payment-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{RedirectSkill}}.SelectAndMoveOwnedCard"],"optional":false,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]},{"op":"chooseOption","target":"owner","resultBind":"child","options":[{"id":"continue"}]}]}]}
                """ : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{RedirectSkill}}","revision":1,"triggers":[{"id":"redirect","window":"slashTargetRedirecting","ownerRelation":"target","cardKinds":["slash","fireSlash","thunderSlash"],"optional":true,"effects":[{"op":"selectTarget","target":"owner","targetKind":"slashRedirectable"},{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand","equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true,"slashRedirectPostCostRange":true},{"op":"redirectCurrentAttack","target":"selectedTarget"}]}]},
                  {"id":"{{RedirectDriver}}","revision":1,"activations":[
                    {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"requested"}]},
                    {"id":"incoming-exemption","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"selectedFirst"},"targetRef":{"kind":"selectedSecond"},"effects":["ignoreDistance"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[]}]},
                    {"id":"incoming-prohibition","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"selectedFirst"},"targetRef":{"kind":"selectedSecond"},"effects":["forbidTarget"],"requiresEffectiveSource":false,"captureOnUse":true,"until":[]}]}]},
                  {"id":"{{RedirectQuiet}}","revision":1,"triggers":[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]}
                  {{movement}}]}
                """;
            var labels = new Dictionary<string, object>
            {
                [RedirectSkill] = new { name = "Shared redirect", description = "Target/material post-cost range" },
                [RedirectDriver] = new { name = "Native attack", description = "Actual physical Slash and directed right" },
                [RedirectQuiet] = new { name = "Quiet", description = "Fixed public opening" }
            };
            if (removeRange) labels[RedirectMovement] = new { name = "Payment movement child", description = "Actual range loss", optionLabels = new { @continue = "Continue" } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var ownerSkills = new List<string> { RedirectDriver, RedirectQuiet };
            if (removeRange) ownerSkills.Add(RedirectMovement);
            if (virtualAxe) ownerSkills.Add("ol:zhanjin");
            builder.AddGeneral(new(RedirectOwner, "Redirect owner", "", RedirectSkill, "wu", 8, ownerSkills));
            foreach (var id in new[] { "fixture:redirect-ally", "fixture:redirect-enemy", "fixture:redirect-source" })
                builder.AddGeneral(new(id, "Public peer", "", RedirectQuiet, "qun", 8));
            const string deck = "fixture:redirect-deck";
            builder.AddDeck(new(deck, "Fixed empty opening", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Shared redirect payment", 4, 4,
                new Dictionary<string, int>(), deck, ModeKind: ContentModeKind.Team,
                GeneralPoolIds: [RedirectOwner, "fixture:redirect-ally", "fixture:redirect-enemy", "fixture:redirect-source"],
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
