using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class OlChengJiChengCuiChecks
{
    private const string Tousui = "ol:tousui", Xuming = "ol:xuming", Driver = "fixture:cjcc-driver",
        Watch = "fixture:cjcc-watch", Quiet = "fixture:cjcc-quiet", Owner = "fixture:cjcc-owner",
        Ally = "fixture:cjcc-ally", Enemy = "fixture:cjcc-enemy", Far = "fixture:cjcc-far", Deck = "fixture:cjcc-deck";

    public static void TousuiGeneralWeaponPaysItsNativeDestroyedMaterialOnceCold()
    {
        var s = Scene.Start(generalWeapon: true);
        s.Use(Driver, "weapon", [], []);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "fixture:cjcc-reserve") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "fixture:cjcc-reserve");
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"); s.Play();
        var gear = s.V().Equipment.Single(c => c.Kind == CardKind.GeneralWeapon).Id;
        s.Cold();
        s.Use(Tousui, "native-bottom-slash", [gear], [3]);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var issued = s.E<TousuiUseIssuedEvent>().Single();
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == issued.UseId);
        var cost = use.Action!.PhysicalCards.Single();
        var movement = s.G.CardMovements.Single(m => m.CardId == gear && m.From == CardLocation.Equipment(0));
        s.Need(use is { Tousui.BottomPaid: true, PaymentMovementReceipt: { } receipt } &&
            receipt.OwnerFrameId == use.Id && receipt.ActionId == issued.ActionId && receipt.Batch.ParentFrameId == use.Id &&
            receipt.Batch.Movements.Single() == movement && movement.Sequence > receipt.SequenceBefore && movement.Sequence <= receipt.SequenceAfter &&
            cost.CardId == gear && cost.CardKind == CardKind.GeneralWeapon && cost.From == CardLocation.Equipment(0) &&
            movement.To == CardLocation.OutsideGame && movement.Reason == CardMoveReasons.Use &&
            s.E<TousuiBottomPaidEvent>().Count(e => e.UseId == use.Id && e.Count == 1) == 1,
            "The genuinely sampled general weapon pays the exact original Tousui/native Use invoice by its real destruction, rather than entering the bottom or Processing.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == gear && m.From == cost.From) == 1 &&
            !s.G.CardMovements.Any(m => m.CardId == gear && m.Sequence > movement.Sequence) &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == gear).Location == CardLocation.OutsideGame,
            "The paid Slash and native parent finish once after cold return without discarding or resurrecting the destroyed weapon.");
        s.Cold();
    }

    private sealed class Fixture(bool incomingBorrowed, bool generalWeapon = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-cheng-ji-cheng-cui", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:cheng-ji-cheng-cui");
            var borrowed = CardCatalog.Get(CardKind.BorrowedSword);
            b.AddCard(new("fixture:cjcc-borrowed-sword", borrowed.DisplayName, borrowed.CategoryName, borrowed.Description, CardKind.BorrowedSword));
            var weapon = generalWeapon ? """,{"id":"weapon","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"equipSampledGenerals","target":"owner","amount":5}]}""" : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                {"id":"{{Driver}}","revision":1,"activations":[{"id":"lose-xuming","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Xuming}}"],"sourceBind":"standard:none"}]}{{weapon}}]},
                {"id":"{{Watch}}","revision":1,"triggers":[{"id":"original-damage","window":"beforeDamageApplied","subject":"damageSource","priority":-1000,"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"damage","options":[{"id":"continue"}]}]}]},
                {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """;
            var presentations = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实失源", description = "原生失技" },
                [Watch] = new { name = "真实伤害边界", description = "原生待结算伤害", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原生伤害" } },
                [Quiet] = new { name = "静默", description = "跳过无关出牌" }
            };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, "原生固定小局") { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "成济成倅本人", "", "standard:none", "wei", 6, [Tousui, Xuming, Driver, Watch]));
            foreach (var id in new[] { Ally, Enemy, Far }) b.AddGeneral(new(id, id, "", "standard:none", "qun", 8, id == Enemy && incomingBorrowed ? [] : [Quiet]));
            if (generalWeapon) b.AddGeneral(new("fixture:cjcc-reserve", "真实备用武将武器", "", "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定四十八张", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:indulgence", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "成济成倅原生团队小局", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: generalWeapon ? [Owner, Ally, Enemy, Far, "fixture:cjcc-reserve"] : [Owner, Ally, Enemy, Far], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private sealed class Scene(GameEngine g, ContentRegistry r)
    {
        public GameEngine G { get; private set; } = g;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string skill, string id, int[] cards, int[] targets) => Accept(new UseProgramSkillCommand(0, skill, id, cards, targets, G.Revision, P!.PromptId));
        public void NativeSlash(int target)
        {
            var a = G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.ConversionSource is null && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId));
        }
        public void Answer(Func<PromptChoice, bool> pred) { var p = P!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.First(pred).Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 120; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "Unexpected native cutoff.");
                if (P is null) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (P.Kind is DecisionKind.Nullification or DecisionKind.RespondDodge or DecisionKind.RespondSlash) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                else throw new InvalidOperationException("Unexpected Cheng Ji Cheng Cui native prompt: " + JsonSerializer.Serialize(P));
            }
            throw new InvalidOperationException("Missing bounded native Cheng Ji Cheng Cui boundary: " + JsonSerializer.Serialize(G.ResolutionStack));
        }
        public void Cold()
        {
            var view = SnapshotJson.Serialize(G.CreateSnapshot(0)); var moves = JsonSerializer.Serialize(G.CardMovements);
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), r);
            Need(view == SnapshotJson.Serialize(G.CreateSnapshot(0)) && moves == JsonSerializer.Serialize(G.CardMovements), "The actual pending receipt restores without duplicate native cost or return.");
        }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + result.Error?.Message); }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.Phase, G.State.Winner, P, FrameTypes = G.ResolutionStack.Select(f => f.GetType().Name), Players = G.CreateSnapshot(0).Players.Select(p => new { p.Seat, p.Hp, p.HandCount }) })); }
        public static Scene Start(bool incomingBorrowed = false, bool defenderDodges = false, bool generalWeapon = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(incomingBorrowed, generalWeapon));
            static ShanheStartingCard C(CardKind kind, Suit suit = Suit.Spade) => new(kind, suit, 7);
            var setup = new ShanheBattleSetup(83, 1, "cjcc-main", Owner, 6, 0, 0, 0, 0, [],
                [C(CardKind.Indulgence, Suit.Heart), C(CardKind.Indulgence, Suit.Diamond), C(CardKind.Slash), C(CardKind.Indulgence, Suit.Club)], incomingBorrowed ? [C(CardKind.Crossbow)] : [],
                [new(Enemy) { StartingHand = incomingBorrowed ? [C(CardKind.BorrowedSword)] : [C(CardKind.Indulgence)] },
                    new(Far) { StartingHand = defenderDodges ? [C(CardKind.Dodge), C(CardKind.Dodge)] : [C(CardKind.Indulgence)] }],
                [new(Ally) { StartingHand = [C(CardKind.Indulgence)] }], new string('c', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
}
