using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceJielueChecks
{
    private const string Skill = StandardShanheSourceDemonRaiderPackage.JielueSkillId;
    private const string Qiangxi = StandardShanheSourceDemonRaiderPackage.QiangxiSkillId;
    private const string Watch = "fixture:source-jielue-gain-child", Deck = "fixture:source-jielue-deck";
    private const string Reason = "skill-program.shanhe:source-jielue-1149.SelectAndMoveOwnedCard";

    public static void ActualRegionsReturnThroughMovementChildrenBeforeHealthLossAndEmptyTargetsStillCharge()
    {
        var s = Scene.Start();
        var delayed = s.V(0).Hand.Single(c => c.Kind == CardKind.Indulgence).Id;
        var hand = s.V(1).Hand.Single().Id;
        var weapon = s.V(1).Equipment.Single().Id;
        var install = s.G.GetHumanLegalActions().Single(a => a.CardId == delayed && a.TargetSeats.SequenceEqual([1]));
        s.Accept(new PlayCardCommand(0, delayed, install.TargetSeats, s.G.Revision, s.P!.PromptId, install.PlayedCardKind)); s.Play();
        Need(s.Zone(delayed) == CardLocation.Judgment(1) &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == delayed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == delayed && m.From == CardLocation.Processing && m.To == CardLocation.Judgment(1)) == 1,
            "The future judgment-zone loot is the original Indulgence installed by its real native card use, with no test mutation of the field.");

        s.Accept(new UseProgramSkillCommand(0, Qiangxi, "receive-damage", [], [1], s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Skill);
        var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var paidAttack = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Qiangxi);
        Need(frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            s.V(0).Hp == 3 && s.V(1).Hp == 3 &&
            s.E<DamageAppliedEvent>().Count(e => e.TargetSeat == 0 && e.Amount == 1 && e.SourceLess) == 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == 1 &&
            s.P!.Choices.All(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("source-zone") == "Hand"),
            "Source3186 first pays actual sourceless damage, which cannot trigger Jielue; only its genuine damage to the other player issues one locked Jielue with opaque hand slots.");

        foreach (var (zone, card) in new[] { (CardZoneKind.Hand, hand), (CardZoneKind.Equipment, weapon), (CardZoneKind.Judgment, delayed) })
        {
            s.Until(() => s.P?.SkillPrompt?.SkillId == Skill);
            s.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == zone.ToString());
            s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
            var moved = s.G.CardMovements.Where(m => m.Reason.Value == Reason).ToArray();
            Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == frame.Id && f.SkillId == Skill && f.PendingMovementContinuation is not null) &&
                s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == frame.Id) &&
                s.Zone(card) == CardLocation.Hand(0) &&
                moved.Count(m => m.CardId == card && m.From == new CardLocation(zone, 1) && m.To == CardLocation.Hand(0)) == 1 &&
                s.V(0).Hp == 3 && s.E<ProgramSkillHpLostEvent>().All(e => e.SkillId != Skill) &&
                s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != frame.Id),
                "Each original region entity really moves once; its gain child holds the same Jielue parent before the next region and before the final HP loss.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        }
        s.Play();
        Need(s.V(0).Hp == 2 && s.V(1).HandCount == 0 && s.V(1).Equipment.Count == 0 && s.V(1).Judgment.Count == 0 &&
            new[] { hand, weapon, delayed }.All(id => s.Zone(id) == CardLocation.Hand(0)) &&
            s.G.CardMovements.Count(m => m.Reason.Value == Reason) == 3 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == Skill && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == frame.Id && e.Completed) == 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == paidAttack.Id && e.SkillId == Qiangxi &&
                e.ActivationId == "receive-damage" && e.Completed) == 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Watch) == 3,
            "After all three actual gain children return, Jielue loses exactly one HP and returns through the original paid Qiangxi to the same live Play, retaining all three original entities. " +
            JsonSerializer.Serialize(new { OwnerHp = s.V(0).Hp, Target = new { s.V(1).HandCount, Equipment = s.V(1).Equipment.Count, Judgment = s.V(1).Judgment.Count },
                Loot = new[] { hand, weapon, delayed }.Select(id => new { Card = id, Zone = s.Zone(id) }),
                HpLoss = s.E<ProgramSkillHpLostEvent>(), BindingReturns = s.E<ProgramBindingResolvedEvent>(), ActiveReturns = s.E<ProgramSkillResolvedEvent>(),
                WatchStarts = s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Watch) }));

        // Empty regions do not waive the locked final HP loss, and subsequent
        // damage in this same turn still issues its own independent occurrence.
        s.Accept(new UseProgramSkillCommand(0, Qiangxi, "discard-weapon", [weapon], [2], s.G.Revision, s.P!.PromptId)); s.Play();
        Need(s.V(0).Hp == 1 && s.V(2).Hp == 3 && s.Zone(weapon) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == weapon && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == 2 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 2 &&
            s.E<ProgramSkillHpLostEvent>().Count(e => e.SkillId == Skill && e.Amount == 1) == 2 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Watch) == 3 &&
            s.G.CardMovements.Count(m => m.Reason.Value == Reason) == 3 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 1) == 1 &&
            s.G.State is { CurrentSeat: 0, Phase: TurnPhase.Play, Winner: Winner.None },
            "The same source can attack a second real target by discarding the weapon it truly acquired; a second independent Jielue with three empty regions still loses one HP without fabricating gains or movement children. " +
            JsonSerializer.Serialize(new { OwnerHp = s.V(0).Hp, TargetHp = s.V(2).Hp, WeaponZone = s.Zone(weapon),
                WeaponMoves = s.G.CardMovements.Where(m => m.CardId == weapon),
                JielueStarts = s.E<ProgramBindingStartedEvent>().Where(e => e.SkillId == Skill),
                JielueReturns = s.E<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Skill),
                HpLoss = s.E<ProgramSkillHpLostEvent>(), WatchStarts = s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Watch),
                GainMoves = s.G.CardMovements.Where(m => m.Reason.Value == Reason), Damage = s.E<DamageAppliedEvent>(),
                ActiveReturns = s.E<ProgramSkillResolvedEvent>(), s.G.State.CurrentSeat, s.G.State.Phase, s.G.State.Winner }));
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected source-Jielue command."); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(choose).Id, G.Revision)); }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> ready)
        {
            for (var step = 0; step < 160; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "The Jielue fixture remains in short live native flows.");
                if (P is { } p && G.CreateSnapshot(p.PlayerSeat).Players[p.PlayerSeat].IsHuman &&
                    !(p.Kind == DecisionKind.PlayCard && G.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected Jielue decision: " + JsonSerializer.Serialize(p));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Jielue exceeded its bounded native progression.");
        }
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(487, 1, "fixture:source-jielue", StandardShanheSourceDemonRaiderPackage.HardId,
                4, 0, 0, 0, 0, [Watch], [new(CardKind.Indulgence, Suit.Heart, 6)], [],
                [new("classic:guan-yu") { StartingHand = [new(CardKind.Peach, Suit.Heart, 3)],
                    StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)] },
                 new("classic:guan-yu") { StartingHand = [] }], [], "fixture:source-jielue") { ExtraSkillSlots = 1 };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] }).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, ConfirmOpeningHand = false, AdvanceAfterHumanCommands = false, MaxTurns = 2 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-jielue", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new
            { id = Watch, revision = 1, triggers = new[] { new { id = "actual-jielue-gain", window = "cardsGained", subject = "owner",
                destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { Reason }, optional = false,
                effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                { [Watch] = new { name = "劫掠实际取牌孩子", description = "返回同一个分区取牌帧。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            b.AddSkill(new(Watch, "劫掠实际取牌孩子", "返回同一个分区取牌帧。")
            { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            b.AddDeck(new(Deck, "零自然起手摸牌的劫掠牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
        }
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
