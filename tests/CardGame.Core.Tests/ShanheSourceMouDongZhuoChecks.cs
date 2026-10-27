using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceMouDongZhuoChecks
{
    private const string General = StandardShanheSourceMouDongZhuoPackage.GeneralId;
    private const string Xiongni = StandardShanheSourceMouDongZhuoPackage.XiongniSkillId;
    private const string Fengshang = StandardShanheSourceMouDongZhuoPackage.FengshangSkillId;
    private const string Zhibing = StandardShanheSourceMouDongZhuoPackage.ZhibingSkillId;
    private const string Zhixi = StandardShanheSourceMouDongZhuoPackage.ZhixiSkillId;
    private const string Neutral = "fixture:source-mou-neutral", Peer = "fixture:source-mou-peer", Driver = "fixture:source-mou-driver";
    private const string CostWatch = "fixture:source-mou-cost-watch", WineWatch = "fixture:source-mou-wine-watch";
    private const string HpWatch = "fixture:source-mou-hp-watch", DrawWatch = "fixture:source-mou-draw-watch", Deck = "fixture:source-mou-deck";
    private const string AlcoholQuota = "fixture:source-mou-alcohol-quota";

    public static void FiveSuitPaymentMaximumHpAndUnrestrictedDyingAllocationUseOriginalChildrenAndWine()
    {
        var s = Scene.Start(false);
        s.Until(() => s.At(Xiongni));
        Need(s.V(0).Markers?.Single(marker => marker.Kind == PlayerMarkerKind.MouDongZhuoGuanbian).Count == 3,
            "Source2684 reuses the real three-participant Guanbian initialization.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        var cost = s.V(0).Hand.Single(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Club).Id;
        var five = s.V(1).Hand.Select(card => card.Id).Concat(s.V(1).Equipment.Select(card => card.Id)).ToArray();
        Need(five.Length == 5 && s.V(2).HandCount == 4, "One real peer can pay five same-suit HE entities, the other cannot.");
        s.Answer(c => c.Cards.SequenceEqual([cost])); s.Until(() => s.At(CostWatch));
        var source = s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Xiongni);
        Need(source.ShanheSourceMou is { Paid: true, Stage: ShanheSourceMouStage.OwnChildren } &&
            s.Facts<ShanheSourceMouPaidEvent>() is [{ Payer: 0, CardId: var paid }] && paid == cost &&
            s.Game.CardMovements.Count(move => move.CardId == cost && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) == 1,
            "The real first HE discard pays once and the original source frame waits for its actual card-loss child.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.Facts<ShanheSourceMouPaidEvent>().Count(e => e.Payer == 1) == 5 &&
            five.All(id => s.Facts<ShanheSourceMouPaidEvent>().Count(e => e.Payer == 1 && e.CardId == id && e.Suit == Suit.Club) == 1) &&
            s.V(1).HandCount == 0 && s.V(1).Equipment.Count == 0 && s.V(1).MaxHp == 8 && s.V(2).MaxHp == 7 &&
            s.Facts<MaximumHpChangedEvent>().Count(e => e.PlayerSeat == 2 && e.Delta == -1 && e.SkillId == Xiongni) == 1 &&
            s.Facts<DamageAppliedEvent>().Length == 0 && s.V(0).Skills!.All(skill => skill.Id != "mou-ol:guanbian") &&
            s.V(0).Markers?.Any(marker => marker.Kind == PlayerMarkerKind.MouDongZhuoGuanbian && marker.Count != 0) != true,
            "Each frozen peer resolves once: exact five-card real discard avoids loss, insufficient cards lose maximum HP rather than damage, and paid Xiongni removes Guanbian and its marker.");

        var collection = new[] { cost }.Concat(five).Take(3).ToArray();
        s.Use("same-suit-allocation", Fengshang);
        foreach (var id in collection) s.Answer(c => c.Cards.SequenceEqual([id]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "gift-finish"); s.Answer(c => c.Targets.SequenceEqual([2]));
        s.Until(() => s.At(WineWatch));
        var allocation = s.Facts<ShanheSourceMouAllocationEvent>().Single();
        var wine = s.Facts<ShanheSourceMouWineIssuedEvent>().Single();
        Need(allocation is { Owner: 0, Recipient: 2, Count: 3, Suit: Suit.Club } &&
            collection.All(id => s.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(2)) == 1) &&
            s.Game.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == wine.UseId && frame.CardKind == CardKind.Alcohol && frame.PhysicalCardIds.Count == 0 &&
                frame.ShanheSourceMouWine?.ParentId == allocation.FrameId) &&
            !s.Facts<CardUseDebitRecordedEvent>().Any(e => e.Debit.CardActionId == wine.ActionId),
            "A nonempty three-card same-suit subset is really given to one recipient before the original material-free wine pauses at a true Use child without spending a quota.");
        Need(s.Facts<JianjiOrdinaryAlcoholPaidEvent>() is [{ NativeUseId: var ledgerUse, ActorSeat: 0, CountsQuota: false }] && ledgerUse == wine.UseId,
            "With a real AdditionalAlcoholUses policy loaded, the original native alcohol ledger records this exact source wine as quota-exempt.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.V(0).HasAlcoholEffect && s.Facts<ShanheSourceMouWineReturnedEvent>() is [{ UseId: var returned }] && returned == wine.UseId &&
            s.Facts<CardUseFinishedEvent>().Count(e => e.ResolutionId == wine.UseId && e.CardKind == CardKind.Alcohol) == 1 &&
            !s.Game.GetHumanLegalActions().Any(action => action.ProgramSkillId == Fengshang),
            "The original wine returns once and the active allocation remains once per actual play phase.");

        // This is a second real Fengshang in the same turn and same suit, after
        // its phase activation. The dying binding has no ordinary round/turn cap.
        s.Use("hurt-peer", Driver, [2]); s.Until(() => s.At(Fengshang));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        var remaining = new[] { cost }.Concat(five).Except(collection).ToArray();
        foreach (var id in remaining) s.Answer(c => c.Cards.SequenceEqual([id]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "gift-finish"); s.Answer(c => c.Targets.SequenceEqual([0]));
        s.Until(() => !s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Fengshang));
        Need(s.Facts<ShanheSourceMouAllocationEvent>() is [{ Suit: Suit.Club }, { Recipient: 0, Count: 3, Suit: Suit.Club }] &&
            s.Facts<ShanheSourceMouWineIssuedEvent>().Length == 1 && remaining.All(id => s.V(0).Hand.Any(card => card.Id == id)),
            "The real dying entry permits another same-turn same-suit allocation after phase use; receiving those actual cards oneself issues no second wine.");
        s.Until(() => s.IsOwnPlay || s.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
        if (!s.IsOwnPlay)
            s.Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die" && c.Parameters.GetValueOrDefault("target-seat") == "2");
        s.Play();
        Need(!s.V(2).IsAlive && s.V(1).IsAlive && s.Game.State.Winner == Winner.None,
            "The original peer's dying resolves while another enemy remains, keeping this a bounded ongoing native game.");
        var discarded = s.Game.CardMovements.Last(move => move.TurnNumber == s.Facts<TurnStartedEvent>().Last().TurnNumber &&
            move.To == CardLocation.DiscardPile && !remaining.Contains(move.CardId)).CardId;
        var recoveriesBefore = s.Facts<RecoveryAppliedEvent>().Length;
        s.Use("hurt-self", Driver); s.Until(() => s.At(Fengshang));
        Need(s.V(0).Hp == 0 && s.V(0).IsAlive, "The source owner enters a fresh real dying before its own wine is issued.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Answer(c => c.Cards.SequenceEqual([discarded]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "gift-finish"); s.Answer(c => c.Targets.SequenceEqual([1]));
        s.Until(() => s.At(WineWatch));
        var selfWine = s.Facts<ShanheSourceMouWineIssuedEvent>().Single(e => e.SelfDying);
        Need(s.Facts<ShanheSourceMouAllocationEvent>().Last() is { Recipient: 1, Count: 1 } &&
            s.Game.ResolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == selfWine.UseId && frame.ShanheSourceMouWine is { SelfDying: true }) &&
            !s.Facts<JianjiOrdinaryAlcoholPaidEvent>().Any(e => e.NativeUseId == selfWine.UseId && e.CountsQuota) &&
            !s.Facts<CardUseDebitRecordedEvent>().Any(e => e.Debit.CardActionId == selfWine.ActionId),
            "A real one-card dying allocation to another actor issues the exact self-rescue Alcohol Use without spending ordinary or additional quota.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.V(0).IsAlive && s.V(0).Hp == 1 && s.Facts<RecoveryAppliedEvent>().Skip(recoveriesBefore).Count(e => e.SourceSeat == 0 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.Facts<DyingResolvedEvent>().Count(e => e.VictimSeat == 0 && e.Survived) == 1 &&
            s.Facts<ShanheSourceMouWineReturnedEvent>().Count(e => e.UseId == selfWine.UseId && e.FrameId == selfWine.FrameId) == 1 &&
            s.Facts<CardUseFinishedEvent>().Count(e => e.ResolutionId == selfWine.UseId && e.CardKind == CardKind.Alcohol) == 1,
            "The actual self-dying wine recovers one HP, returns once to its original Fengshang and allows the original dying frame to resolve survived.");
    }

    public static void TrueBlackUsesThresholdThreeSixNineGrantUseOnlyZhixiUntilActualTurnEnd()
    {
        foreach (var initialBossHand in new[] { 0, 11 }) RunThresholdCase(initialBossHand);
    }

    private static void RunThresholdCase(int initialBossHand)
    {
        var s = Scene.Start(true, initialBossHand); s.Play();
        var originalBossHand = s.V(1).Hand.Select(card => card.Id).ToArray();
        var materials = s.V(0).Hand.Where(card => card.Kind == CardKind.Crossbow).Select(card => card.Id).ToArray();
        Need(materials.Length == 10 && s.V(0).Hp == 8, "Ten distinct native black equipment entities form the bounded actual-Use sequence.");
        for (var count = 1; count <= 9; count++)
        {
            s.Equip(materials[count - 1]);
            if (count == 3)
            {
                s.Until(() => s.At(HpWatch));
                Need(s.V(0).Hp == 7 && s.Facts<ShanheSourceMouThresholdEvent>() is [{ Owner: 1, Actor: 0, Ordinal: 3 }] &&
                    s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Zhibing && f.ShanheSourceMou is { Actor: 0, Ordinal: 3 }),
                    "The third actual black Use loses one HP and retains its original observer frame while the real loss child waits.");
                s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            }
            if (count == 6)
            {
                s.Until(() => s.At(DrawWatch));
                Need(s.P!.PlayerSeat == 1 && s.V(1).HandCount == 9 && s.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f =>
                    f.SkillId == Zhibing && f.ShanheSourceMou is { Actor: 0, Ordinal: 6 } && f.PendingMovementContinuation is not null),
                    "The sixth true Use adjusts the source owner's hand to nine and owns the actual gain/discard child before returning to the original equipment use.");
                if (initialBossHand == 11)
                {
                    var paid = s.Facts<ShanheSourceMouPaidEvent>().Where(e => e.Payer == 1).ToArray();
                    Need(paid.Length == 2 && paid.Select(e => e.CardId).Distinct().Count() == 2 && paid.All(e => originalBossHand.Contains(e.CardId) && e.From == CardLocation.Hand(1)) &&
                        paid.All(e => s.Game.CardMovements.Count(move => move.CardId == e.CardId && move.From == CardLocation.Hand(1) &&
                            move.To == CardLocation.DiscardPile && move.Reason.Value == "skill.shanhe-source-mou.discard") == 1) &&
                        s.Game.CardMovements.All(move => move.Reason.Value != "skill.shanhe-source-zhibing.draw"),
                        "An actual eleven-card owner selects and pays precisely two original hand entities, rather than drawing, and pauses at the real discard child.");
                }
                else Need(s.Game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                    move.Reason.Value == "skill.shanhe-source-zhibing.draw") == 9,
                    "The zero-card owner really draws nine distinct native entities before its gain children return.");
            }
            s.Play();
        }
        Need(s.Facts<ShanheSourceMouThresholdEvent>().Select(e => e.Ordinal).SequenceEqual([3, 6, 9]) &&
            s.Facts<ShanheSourceMouBlackUseEvent>().Count(e => e.Actor == 0 && e.Black) == 9 &&
            s.Facts<ShanheSourceMouBlackUseEvent>().Where(e => e.Actor == 0 && e.Black).All(e => e.Kind == CardKind.Crossbow && e.Suit == Suit.Spade) &&
            s.Facts<CardUseFinishedEvent>().Count(e => materials.Take(9).Contains(e.CardId) && e.CardKind == CardKind.Crossbow) == 9 &&
            s.V(0).Skills!.Any(skill => skill.Id == Zhixi) && s.V(1).HandCount == 9 &&
            !s.Game.GetHumanLegalActions().Any(action => action.CardId == materials[9]),
            "One actor's actual-turn counts trigger exactly3/6/9; the ninth original Use still completes, then the source-specific locked Zhixi prohibits a tenth Use.");
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
        var query = typeof(GameEngine).GetMethod("IsCardUseForbidden", flags)!;
        bool Banned(CardKind kind, CardActionType action) => (bool)query.Invoke(s.Game, [0, kind, action, false, null])!;
        Need(Banned(CardKind.Slash, CardActionType.Use) && !Banned(CardKind.Slash, CardActionType.Response),
            "The same native legality rule prohibits actual Use while preserving a pure played Slash response; it is not the use-and-play ban or an unrelated Zhixi.");
        var turn = s.Facts<TurnStartedEvent>().Last().TurnNumber;
        s.Submit(new EndPlayPhaseCommand(0, s.Game.Revision, s.P!.PromptId));
        s.Until(() => s.V(0).Skills!.All(skill => skill.Id != Zhixi));
        Need(s.Facts<TurnEndedEvent>().Any(e => e.TurnNumber == turn && e.ActorSeat == 0),
            "The issued native grant expires on the actual granting turn end rather than leaking into later turns.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, Game.CreateSnapshot(0).Players.Count).Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Facts<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool At(string skill) => P?.Kind == DecisionKind.ProgramTrigger && P.SkillPrompt?.SkillId == skill;
        public bool IsOwnPlay => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Game.ResolutionStack.Count == 0;
        public void Submit(GameCommand command) { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source2684 native command."); }
        public void Answer(Func<PromptChoice, bool> select) { var p = P!; Need(p.PlayerSeat == 0, "Only the actual human answers externally."); Submit(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(select).Id, Game.Revision)); }
        public void Use(string activation, string skill, int[]? targets = null) => Submit(new UseProgramSkillCommand(0, skill, activation, [], targets ?? [], Game.Revision, P!.PromptId));
        public void Equip(int id) { var action = Game.GetHumanLegalActions().Single(a => a.CardId == id && a.Kind == LegalActionKind.Equip); Submit(new PlayCardCommand(0, id, action.TargetSeats, Game.Revision, P!.PromptId, action.PlayedCardKind)); }
        public void Play() => Until(() => IsOwnPlay);
        public void Until(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                Need(P is null || !V(P.PlayerSeat).IsHuman, "Unexpected source2684 human boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source2684 did not reach its bounded native boundary.");
        }
        public static Scene Start(bool zhibing, int initialBossHand = 0)
        {
            var roster = zhibing ? new ShanheCombatantDefinition[] { new(General, ExtraSkillIds: [DrawWatch])
                { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 6), initialBossHand).ToArray() } } :
                [new(Peer) { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), 4).ToArray(), StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)] },
                 new(Peer) { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Club, 2), 4).ToArray() }];
            var starting = zhibing ? Enumerable.Repeat(new ShanheStartingCard(CardKind.Crossbow, Suit.Spade, 1), 10).ToArray() :
                new[] { new ShanheStartingCard(CardKind.Dodge, Suit.Club, 3) };
            var setup = new ShanheBattleSetup(31, 1, "source2684-native", zhibing ? Neutral : General, zhibing ? 8 : 20,
                0, 0, 0, 0, zhibing ? [Driver, HpWatch] : [Driver, CostWatch, WineWatch, AlcoholQuota], starting, [], roster, [], new string('a', 64));
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
                { PlayerGeneralId = General, Hp = 20, ExtraSkillIds = [], EnemyRoster = roster.Select(_ => new ShanheCombatantDefinition("classic:guan-yu")).ToArray() });
            var registry = production.WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Neutral, Peer]).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 1 + roster.Length, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Submit(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source2684", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                {"id":"{{{AlcoholQuota}}}","revision":1,"cardPolicies":[{"id":"one-additional-alcohol-use","kind":"additionalAlcoholUses","cardKinds":["alcohol"],"value":1}]},
                {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"hurt-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"loseHp","target":"selectedTarget","amount":7}]},
                  {"id":"hurt-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"loseHp","target":"owner","amount":20}]}]},
                {"id":"{{{CostWatch}}}","revision":1,"triggers":[{"id":"actual-cost-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill.shanhe-source-mou.discard"],"optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"cost","options":[{"id":"continue"}]}]}]},
                {"id":"{{{WineWatch}}}","revision":1,"triggers":[{"id":"actual-wine-child","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["alcohol"],"optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"wine","options":[{"id":"continue"}]}]}]},
                {"id":"{{{HpWatch}}}","revision":1,"triggers":[{"id":"actual-loss-child","window":"afterHpLost","subject":"owner","optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"health","options":[{"id":"continue"}]}]}]},
                {"id":"{{{DrawWatch}}}","revision":1,"triggers":[{"id":"actual-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill.shanhe-source-zhibing.draw"],"optional":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"draw","options":[{"id":"continue"}]}]},
                  {"id":"actual-discard-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill.shanhe-source-mou.discard"],"optional":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"discard","options":[{"id":"continue"}]}]}]}]}
                """;
            var names = new[] { Driver, CostWatch, WineWatch, HpWatch, DrawWatch, AlcoholQuota }.ToDictionary(id => id, id => new
                { name = id, description = "真实专版前向孩子", optionLabels = id is Driver or AlcoholQuota ? new Dictionary<string, string>() : new Dictionary<string, string> { ["continue"] = "继续" } });
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Neutral, "真实用牌者", "supporter", null, "wei", 8));
            builder.AddGeneral(new(Peer, "原生付款目标", "supporter", null, "wei", 8));
            builder.AddDeck(new(Deck, "零基础摸牌", 0, 0, [new("standard:dodge", 100)]));
        }
    }
}
