using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ShanheChanrangEdictChecks
{
    private const string Driver = "fixture:chanrang-driver", Mode = "fixture:chanrang-mode";
    public static void FirstGainIndependentOwnersForeignPayerAndEquipmentPaymentCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheChanrangEdictPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new() { Seed = 71, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
        Submit(new StartGameCommand()); Play(0); Equip(0);
        AdvanceTo(1); Equip(1);
        Draw(1, 2);
        var offeredOwners = new HashSet<int>();
        for (var offer = 0; offer < 2; offer++)
        {
            Reach(() => P() is { Kind: DecisionKind.ProgramTrigger } &&
                game.ResolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheChanrang.Stage: "branch" });
            var owner = P()!.PlayerSeat;
            Need(owner is 0 or 1 && offeredOwners.Add(owner), "Each real equipment owner receives exactly one first-gain branch in native trigger order.");
            if (owner == 0) { Answer(choice => choice.Parameters["branch"] == "skip"); continue; }
            Answer(choice => choice.Parameters["branch"] == "take"); Edict(2, "card");
            Need(P() is { PlayerSeat: 2, Choices.Count: > 0 } && game.CreateSnapshot(1).PendingDecision?.Choices.Any(choice => choice.Cards.Count > 0) != true,
                "The requested participant chooses its own private HE payment, not the equipment owner.");
            Cold(); Answer(choice => choice.Cards.Count == 1);
        }
        Play(1);
        var turn = game.State.TurnNumber;
        Need(Facts<ShanheChanrangFirstGainEvent>().Count(fact => fact.ActualTurn == turn && fact.RecipientSeat == 2) == 1 &&
            Facts<ShanheChanrangPaidEvent>() is [{ OwnerSeat: 1, GiverSeat: 2, RecipientSeat: 1 }],
            "Two equipment owners independently see the same true first gain, while only the accepted request pays.");
        Draw(1, 2); Play(1);
        Need(Facts<ShanheChanrangPaidEvent>().Length == 1 && Facts<ShanheChanrangFirstGainEvent>().Count(fact => fact.ActualTurn == turn && fact.RecipientSeat == 2) == 1,
            "Declining the recipient's first gain cannot create another entry on its second gain in that actual turn.");

        AdvanceTo(0); Draw(0, 2); Edict(0, "branch"); Answer(choice => choice.Parameters["branch"] == "give"); Edict(0, "card");
        var equipment = game.CreateSnapshot(0).Players[0].Equipment.Single(card => card.Kind == CardKind.ShanRangZhaoShu).Id;
        Answer(choice => choice.Cards.SequenceEqual([equipment])); Edict(1, "branch");
        Need(game.CreateSnapshot(0).Players[0].Equipment.All(card => card.Id != equipment) &&
            game.CreateSnapshot(2).Players[2].Hand.Any(card => card.Id == equipment) &&
            Facts<ShanheChanrangPaidEvent>().Count(fact => fact.OwnerSeat == 0 && fact.CardId == equipment) == 1,
            "Giving the real equipped treasure retracts its grant but finishes the already-paid transfer once on the original frame.");
        Cold(); Answer(choice => choice.Parameters["branch"] == "skip"); Play(0);
        Draw(0, 3); Edict(1, "branch"); Answer(choice => choice.Parameters["branch"] == "skip"); Play(0);
        Need(Facts<ShanheChanrangPaidEvent>().Length == 2 &&
            game.CardMovements.Count(move => move.CardId == equipment && move.Reason.Value == "shanhe.chanrang-edict.give") == 1,
            "The former holder cannot trigger from the moved treasure, and cold replay never repeats a paid physical movement.");

        // Keep the private human-payer recovery above, then exercise the same issued
        // payment through the real native AI before the AI participant's own turn.
        game = GameEngine.CreateStandard(new() { Seed = 71, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2],
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry);
        Submit(new StartGameCommand()); Play(0); Equip(0); Draw(0, 3); Edict(0, "branch"); Cold();
        Answer(choice => choice.Parameters["branch"] == "take"); Play(0);
        Need(Facts<ShanheChanrangPaidEvent>() is [{ OwnerSeat: 0, GiverSeat: 3, RecipientSeat: 0 } aiPayment] &&
            game.CardMovements.Any(move => move.CardId == aiPayment.CardId && move.From == CardLocation.Hand(3) &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == "shanhe.chanrang-edict.give"),
            "A real AI recipient answers its own issued HE payment and transfers the selected entity once without a human answer.");

        PendingDecision? P() => Enumerable.Range(0, 4).Select(seat => (Seat: seat, Prompt: game.CreateSnapshot(seat).PendingDecision))
            .FirstOrDefault(item => item.Prompt?.PlayerSeat == item.Seat).Prompt;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command) { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Edict command rejected."); }
        void Answer(Func<PromptChoice, bool> select)
        { var prompt = P()!; Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(select).Id, game.Revision)); }
        void Reach(Func<bool> stop, bool advancePlayers = false)
        {
            for (var step = 0; step < 160; step++)
            {
                if (stop()) return;
                if (P() is { Kind: DecisionKind.PlayCard } prompt && game.ResolutionStack.Count == 0)
                {
                    if (!advancePlayers) throw new InvalidOperationException("Edict reached an unexpected Play boundary.");
                    Submit(new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
                }
                else if (P() is { Kind: DecisionKind.ProgramTrigger } skill && game.CreateSnapshot(0).Players[skill.PlayerSeat].IsHuman)
                    throw new InvalidOperationException("Edict reached an unexpected skill chooser: " + JsonSerializer.Serialize(skill));
                else Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Edict did not reach its bounded native boundary: " + JsonSerializer.Serialize(P()));
        }
        void Play(int seat) => Reach(() => P() is { Kind: DecisionKind.PlayCard } prompt && prompt.PlayerSeat == seat && game.ResolutionStack.Count == 0);
        void AdvanceTo(int seat) => Reach(() => P() is { Kind: DecisionKind.PlayCard } prompt && prompt.PlayerSeat == seat && game.ResolutionStack.Count == 0, advancePlayers: true);
        void Edict(int chooser, string stage) => Reach(() => P()?.PlayerSeat == chooser &&
            game.ResolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheChanrang.Stage: var actual } && actual == stage);
        void Equip(int seat)
        {
            Play(seat); var card = game.CreateSnapshot(seat).Players[seat].Hand.First(card => card.Kind == CardKind.ShanRangZhaoShu);
            Submit(new PlayCardCommand(seat, card.Id, [], game.Revision, P()!.PromptId)); Play(seat);
        }
        void Draw(int actor, int recipient)
        { Play(actor); Submit(new UseProgramSkillCommand(actor, Driver, "draw", [], [recipient], game.Revision, P()!.PromptId)); }
        void Cold()
        {
            var before = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray();
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(before.SequenceEqual(Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)))),
                "Cold replay preserves the foreign private payer and the paid equipment child on its original frame.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:chanrang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                "activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                "effects":[{"op":"draw","target":"selectedTarget","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实获牌", description = "实际回合外获牌" } } }));
            builder.AddSkill(new(Driver, "真实获牌", "实际回合外获牌") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            var generals = Enumerable.Range(0, 4).Select(seat => "fixture:chanrang-general-" + seat).ToArray();
            foreach (var id in generals) builder.AddGeneral(new(id, "诏书参与者", "", Driver, "qun", 4));
            builder.AddDeck(new("fixture:chanrang-deck", "固定诏书实体", 4, 2, [new("shanhe:chanrang-edict", 60)]));
            builder.AddMode(new(Mode, "诏书原生短局", 4, 4, new Dictionary<string, int>
            { ["Lord"] = 1, ["Loyalist"] = 1, ["Rebel"] = 1, ["Renegade"] = 1 }, "fixture:chanrang-deck", GeneralPoolIds: generals));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
