using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class ProgramAmountExpressionChecks
{
    private static void ExactOwnedTransferIsTypedAndKeepsItsLegacySingleCardContract()
    {
        const string prefix = """
            {"op":"draw","target":"owner","amount":1,"resultBind":"known"},
            {"op":"captureCards","target":"owner","resultBind":"forbidden","cardsExpr":["boundCards","known"]},
            {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
            """;
        const string selector = """
            {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},"cardOwnerRef":{"kind":"selectedTarget"},
              "zones":["hand","equipment"],"countExpr":["+",1,1],"exceptBind":"forbidden","onInsufficient":"bindEmpty",
              "destination":"ownerHand","giveBy":"materialOwner","resultBind":"accepted","awaitMovementTriggers":true}
            """;
        const string read = """,{"op":"captureNumber","target":"owner","resultBind":"accepted-count","valueExpr":["boundCount","accepted"]}""";
        var program = Load(prefix + selector + read);
        var spaced = Load(prefix + selector.Replace("[\"+\",1,1]", "[ \"+\", 1, 1 ]", StringComparison.Ordinal) + read);
        var otherCount = Load(prefix + selector.Replace("[\"+\",1,1]", "3", StringComparison.Ordinal) + read);
        Need(program.Activations.Single().Effects[3] is
                { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ResultBind: "accepted", ExceptBind: "forbidden", AwaitMovementTriggers: true,
                    CountExpression.Type: ProgramExpressionType.Int, OnInsufficient: ProgramInsufficientCardBehavior.BindEmpty } &&
            program.GameplayHash == spaced.GameplayHash && program.GameplayHash != otherCount.GameplayHash,
            "An exact transfer consumes a typed count and known exclusion binding, publishes its accepted result, and fingerprints its count expression independently of whitespace.");
        var constant = JsonNode.Parse(selector)!; constant.AsObject().Remove("countExpr"); constant["count"] = 2;
        _ = Load(prefix + constant.ToJsonString() + read);
        _ = Load("""
            {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
            {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"ownerHand"}
            """);
        void Refuse(string field, JsonNode value)
        {
            var invalid = JsonNode.Parse(selector)!; invalid[field] = value;
            Reject(prefix + invalid.ToJsonString() + read);
        }
        Refuse("countExpr", JsonValue.Create(true)!);
        Refuse("count", JsonValue.Create(1)!);
        Refuse("exceptBind", JsonValue.Create("missing")!);
        Refuse("chooserRef", JsonNode.Parse("""{"kind":"owner"}""")!);
        Refuse("zones", JsonNode.Parse("""["judgment"]""")!);
        Refuse("destination", JsonValue.Create("discardPile")!);
        Refuse("awaitMovementTriggers", JsonValue.Create(false)!);
        Refuse("allowDecline", JsonValue.Create(true)!);
        Refuse("onInsufficient", JsonValue.Create("draw")!);
        var noResult = JsonNode.Parse(selector)!; noResult.AsObject().Remove("resultBind");
        Reject(prefix + noResult.ToJsonString());
    }
}
