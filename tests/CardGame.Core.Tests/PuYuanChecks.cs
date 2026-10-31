using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class PuYuanChecks
{
    private const string Mode = "identity:classic-puyuan-fixture";
    public static void ThreeResultsFreezeOffersAndResumePaidNativePlacement()
    {
        foreach (var (quality, count) in new[] { ("perfect", 3), ("success", 2), ("failure", 1) })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(includeJijiu: true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture());
            var g = Start(registry);
            var opening = g.CardMovements.Where(m => m.To == CardLocation.Equipment(0)).ToArray();
            Need(opening.Length == 2 && opening.Select(m => EquipmentCatalog.Get(m.CardKind).Slot is EquipmentSlot.DefensiveHorse
                ? EquipmentSlot.OffensiveHorse : EquipmentCatalog.Get(m.CardKind).Slot).Distinct().Count() == 2,
                "Native Qisi equips two different categories from the actual deck.");
            var action = g.GetHumanLegalActions().First(a => a.ProgramSkillId == "ol:shengong");
            var material = action.SelectableCardIds.First();
            var before = g.CardMovements.Count;
            Accept(g, new UseProgramSkillCommand(0, "ol:shengong", action.ProgramActivationId!, [material], [], g.Revision, P(g)!.PromptId));
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == quality));
            g = Cold(g, registry);
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == quality);
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "equipment-offer"));
            var pending = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "ol:shengong");
            Need(pending.EquipmentOffer!.Templates.Count == count && P(g)!.Choices.Count == count,
                "The chosen result freezes exactly three/two/one distinct outside-game candidates.");
            Need(pending.EquipmentOffer.Templates.Select(t => t.Kind).Distinct().Count() == count,
                "A random offer cannot contain duplicate equipment kinds.");
            Need(Enumerable.Range(1, 3).All(seat => g.CreateSnapshot(seat).PendingDecision is null),
                "Outside-game candidates remain private to their chooser.");
            var offered = JsonSerializer.Serialize(pending.EquipmentOffer);
            g = Cold(g, registry);
            Need(JsonSerializer.Serialize(g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "ol:shengong").EquipmentOffer) == offered,
                "Cold restore preserves the original sampled offer, without a second payment or random draw.");
            var chosen = pending.EquipmentOffer.Templates[0];
            Answer(g, _ => true);
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "equipment-recipient"));
            g = Cold(g, registry);
            Answer(g, c => c.Targets.SequenceEqual([0]));
            Reach(g, p => p.Kind == DecisionKind.PlayCard);
            var created = g.CreateCardZoneDiagnostics().Single(c => c.CardKind == chosen.Kind && c.Location == CardLocation.Equipment(0));
            Need(!g.CreateCardZoneDiagnostics().Any(c => c.CardId == created.CardId && c.Location == CardLocation.DrawPile) &&
                g.CardMovements.Skip(before).Count(m => m.CardId == material && m.To == CardLocation.DiscardPile) == 1 &&
                !g.GetHumanLegalActions().Any(a => a.ProgramSkillId == "ol:shengong" && a.ProgramActivationId == action.ProgramActivationId),
                "The native placement creates one outside-game physical entity, pays once and consumes only its phase category.");
            _ = Cold(g, registry);
        }
    }

    private static GameEngine Start(ContentRegistry registry)
    {
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0,
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true,
            AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
        Accept(g, new StartGameCommand()); Reach(g, p => p.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, "ol:pu-yuan", g.Revision, P(g)!.PromptId));
        Reach(g, p => p.Kind == DecisionKind.PlayCard);
        return g;
    }
    private static PendingDecision? P(GameEngine g) => g.CreateSnapshot(0).PendingDecision;
    private static void Reach(GameEngine g, Func<PendingDecision, bool> stop)
    {
        for (var i = 0; i < 120; i++)
        {
            var p = P(g); if (p is not null && stop(p)) return;
            if (p?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip") == true)
                Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (p?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "weapon") == true)
                Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "weapon");
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Pu Yuan missed its native boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Answer(GameEngine g, Func<PromptChoice, bool> choose)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(0, p.PromptId, p.Choices.First(choose).Id, g.Revision)); }
    private static void Accept(GameEngine g, GameCommand command)
    { var result = g.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Pu Yuan command."); }
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Need(State(restored) == State(g), "Pu Yuan cold replay changed a private view, physical payment or original frame.");
        return restored;
    }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new {
        Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))),
        Frames = JsonSerializer.Serialize(g.ResolutionStack), g.CardMovements, Zones = g.CreateCardZoneDiagnostics()
    });
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:puyuan", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            var ids = new List<string> { "ol:pu-yuan" };
            for (var i = 1; i < 4; i++) { var id = "fixture:puyuan-" + i; ids.Add(id); b.AddGeneral(new(id, id, id, null)); }
            var kinds = new[] { CardKind.Crossbow, CardKind.BaguaFormation, CardKind.OffensiveHorse, CardKind.DefensiveHorse, CardKind.WoodenOx };
            foreach (var kind in kinds) b.AddCard(new("fixture:puyuan-card-" + kind, kind.ToString(), "装备牌", "测试实体", kind));
            b.AddDeck(new("fixture:puyuan-deck", "蒲元定向牌堆", 4, 2,
                kinds.Select(k => new ContentDeckCardCount("fixture:puyuan-card-" + k, 10)).ToArray()));
            b.AddMode(new(Mode, "蒲元定向模式", 4, 4,
                new Dictionary<string, int> { ["Lord"] = 1, ["Loyalist"] = 1, ["Rebel"] = 1, ["Renegade"] = 1 },
                "fixture:puyuan-deck", GeneralPoolIds: ids));
        }
    }
}
