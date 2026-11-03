using System.Text.Json;
using CardGame.Core;

internal static partial class ProgramAmountExpressionChecks
{
    private static void GivenCardExpressionsKeepScopeAndKnownIdentity()
    {
        var pending = new List<IGameEvent>();
        var history = new ProgramEventHistoryIndex([], pending);
        var advancing = new GivenExpressionAdvancing();
        var scopes = new NativeGiveScopeRuntime(new MatchNativeFactDemand(ProgramTrackedFact.NativeGive), history, history, advancing);
        var given = new NativeGivenCardQueries(history, advancing, scopes);
        var turn = new GivenExpressionTurn();
        void Record(int entity, int recipient, long id, int sequence)
        {
            var move = new CardMovementRecord(sequence, 1, entity, CardKind.Slash,
                CardLocation.Hand(0), CardLocation.Hand(recipient), new("fixture.given-expression"));
            history.Append(new NativeCardsGivenEvent(id, 90, 0, recipient, 1, 1, 0, sequence)
            {
                Receipt = new(id, 90, 0, recipient, 1, 0, sequence)
                { Movements = [move], ScopeCutoffAtIssue = scopes.CurrentCutoff(1, 0) }
            });
        }
        Record(10, 1, 1, 1); Record(20, 2, 2, 2); Record(10, 1, 3, 3);
        using var cardsJson = JsonDocument.Parse("""["givenCards","owner","eventTarget"]""");
        var cards = ProgramExpressions.ParseCardSet(cardsJson.RootElement, "fixture.given-cards");
        var world = new GivenExpressionWorld(0, given, turn, 2);
        var read = ProgramExpressions.EvaluateCardSet(cards, world)!;
        Need(read is [{ CardId: 10, SourceEntrySequence: 3 }] && read[0].CapturedLocation == CardLocation.Hand(1) &&
            read[0].PrintedKind is null && read[0].EffectiveKind is null && read[0].EffectiveSuit is null,
            "Given-card reads select the two actual parties, retain the latest delivery witness and never invent private faces.");
        Need(ProgramGivenCards.Read(given, turn, 1, 0, 1)!.Count == 1 &&
            ProgramGivenCards.Read(given, turn, 2, 0, 1) is null &&
            ProgramGivenCards.Read(null, turn, 0, 0, 1) is null,
            "Either actual party can read known delivery identities; an unrelated viewer or unavailable history stays unknown.");
        history.Append(new TurnEndedEvent(1, 0));
        Need(ProgramExpressions.EvaluateCardSet(cards, world)!.Count == 0,
            "The read uses the current actual-turn segment, so old given entities expire at its real End boundary.");
        Record(30, 1, 4, 4);
        Need(ProgramExpressions.EvaluateCardSet(cards, world) is [{ CardId: 30 }],
            "A later Give after the same-number End cannot revive entities from the closed segment.");
        using var amountJson = JsonDocument.Parse("""["windowAmount"]""");
        var amount = ProgramExpressions.ParseInt(amountJson.RootElement, "fixture.window-amount");
        Need(ProgramExpressions.EvaluateInt(amount, world) == 2 &&
            ProgramExpressions.EvaluateInt(amount, new GivenExpressionWorld(0, given, turn, null)) is null,
            "Window amount is an explicit numeric fact; a missing window amount remains unknown.");
        var program = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,
              "activations":[{"id":"read","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                "effects":[{"op":"captureCards","target":"owner","resultBind":"given","cardsExpr":["givenCards","owner","selectedTarget"]}]}]}]}
            """, Presentation(Skill)).Programs[Skill];
        Need(new SkillProgramDependencies([new ContentSkillDefinition(Skill, Skill, Skill) { Program = program }])
                .RequiredFacts.HasFlag(ProgramTrackedFact.NativeGive),
            "A shared history read declares native Give demand independently of a Give trigger or character operation.");
    }

    private sealed class GivenExpressionAdvancing : IAdvancingRuleFacts
    { public IEnumerable<IGameEvent> Facts => []; }
    private sealed class GivenExpressionTurn : IProgramTurnQueries
    { public int TurnNumber => 1; public int CurrentSeat => 0; public int TurnOwnerSeat => 0; public int TurnOwnerTurn => 1; }
    private sealed class GivenExpressionWorld(int owner, INativeGivenCardQueries given, IProgramTurnQueries turn, int? amount) : IProgramExpressionWorld
    {
        public int? Participant(string name) => name switch { "owner" => owner, "eventTarget" => 1, _ => null };
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public int? WindowAmount() => amount;
        public IReadOnlyList<ProgramExpressionCardFact>? GivenCards(int giver, int recipient) =>
            ProgramGivenCards.Read(given, turn, owner, giver, recipient);
    }
}
