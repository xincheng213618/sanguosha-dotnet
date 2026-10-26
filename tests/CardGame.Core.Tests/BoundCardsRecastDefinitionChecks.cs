using System.Text.Json;
using CardGame.Core;

internal static class BoundCardsRecastDefinitionChecks
{
    public static void GroupsRequireOneOrTwoOwnedHeMaterialsAndKeepSingleCardWindow()
    {
        Load(Selection(1, 1));
        Load(Selection(1, 2));
        Reject(Selection(0, 2));
        Reject(Selection(1, 3));
        Reject(Selection(1, 2, "judgment"));
        Reject("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},""" +
            Selection(1, 2).Replace("\"target\":\"owner\"", "\"target\":\"selectedTarget\"", StringComparison.Ordinal));
        Load(Selection(1, 1), "recastBoundCard", drawEnded: true);
        Reject(Selection(1, 1), "recastBoundCard");
    }

    private static string Selection(int minimum, int maximum, string zone = "hand") => $$$"""
        {"op":"selectOwnedCards","target":"owner","minimumCards":{{{minimum}}},"maximumCards":{{{maximum}}},
         "zones":["{{{zone}}}"],"resultBind":"materials"}
        """;

    private static void Reject(string selection, string operation = "recastBoundCards")
    {
        try { Load(selection, operation); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Recast resource loading accepted an invalid owned group or changed the old single-card window contract.");
    }

    private static void Load(string selection, string operation = "recastBoundCards", bool drawEnded = false)
    {
        var effects = selection + "," + $$$"""{"op":"{{{operation}}}","target":"owner","sourceBind":"materials"}""";
        var entry = drawEnded
            ? $$$"""
              "triggers":[{"id":"recast","window":"drawPhaseEnded","subject":"owner","optional":false,"effects":[{{{effects}}}]}]
              """
            : $$$"""
              "activations":[{"id":"recast","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{{{effects}}}]}]
              """;
        _ = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"fixture:bound-recast-shape","revision":1,{{{entry}}}}]}
            """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    ["fixture:bound-recast-shape"] = new { name = "重铸材料", description = "既有本人选牌与原生组重铸" }
                }
            }));
    }
}
