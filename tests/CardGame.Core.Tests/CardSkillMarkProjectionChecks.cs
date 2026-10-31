using CardGame.Core;
using System.Text.Json;

internal static class CardSkillMarkProjectionChecks
{
    internal static void OrderedCompositionPreservesEmptyLabelsAndCapturedTail()
    {
        var sources = new Sources();
        var marks = new Marks();
        var projector = new CardSkillMarkProjector(sources, marks);
        Require(projector.Label(7) is null, "No applicable source must retain a null label.");
        sources.ProjectLabels = (int _, ref CardSkillMarkProjection projection) => projection.Append("");
        Require(projector.Label(7) == "", "An applicable empty label must remain distinct from no label.");
        sources.ProjectLabels = (int _, ref CardSkillMarkProjection projection) =>
        {
            projection.Append(null);
            projection.Append("");
            projection.Append("existing");
            projection.Append(null);
            projection.Append("");
        };
        Require(projector.Label(7) == " / existing / ", "Only null labels are omitted; empty labels retain their separators.");

        foreach (var (silv, zhongshen, expected) in new (bool, bool, string?)[]
                 { (false, false, null), (true, false, "闾"), (false, true, "重身"), (true, true, "闾 / 重身") })
        {
            sources.ProjectLabels = (int _, ref CardSkillMarkProjection projection) =>
            {
                projection.Append(silv ? "闾" : null);
                projection.Append(zhongshen ? "重身" : null);
            };
            Require(projector.Label(7) == expected, "Ordered prefix sources must preserve the original paired-mark combinations.");
        }

        string? capturedTail = "丰蔚";
        var reads = new List<string>();
        sources.ProjectLabels = (int cardId, ref CardSkillMarkProjection projection) =>
        {
            Require(cardId == 7, "Every source must receive the original physical identity.");
            reads.Add("prefix");
            projection.Append("闾");
            projection.Append("重身");
            reads.Add("existing");
            projection.Append("registered");
            reads.Add("dongxin");
            projection.Append("恫心");
            reads.Add("fengwei");
            projection.AppendLast(capturedTail);
            capturedTail = null;
            reads.Add("ordinary");
            projection.Append("激鼓");
            projection.Append("幻术");
        };
        Require(projector.Label(7) == "闾 / 重身 / registered / 恫心 / 激鼓 / 幻术 / 丰蔚" &&
                reads.SequenceEqual(["prefix", "existing", "dongxin", "fengwei", "ordinary"]),
            "A tail label must retain its original read point and captured value while being displayed last.");
        Require(projector.Label(7) == "闾 / 重身 / registered / 恫心 / 激鼓 / 幻术",
            "Each projection must rebuild from its current sources without retaining a previous call's tail.");
        marks.Names = [("qiqin", "琴"), ("qi", "炁")];
        sources.ProjectLabels = (int _, ref CardSkillMarkProjection projection) =>
        {
            projection.Append("existing");
            projection.OverrideMarkLabel("qiqin", "琴（乐槽）");
            projection.AppendLast("tail");
        };
        Require(projector.Label(7) == "existing / 炁 / 琴（乐槽） / tail",
            "Declared mark names have deterministic key order, a dynamic slot overrides its one key, and the existing captured tail remains last.");
        marks.Names = [];
        Require(projector.Label(7) == "existing / 琴（乐槽） / tail",
            "A dynamic mark modifier can display its known physical entity without requiring a redundant default label.");
        PresentationLabelsAreFrozenAndValidated();
    }

    private static void PresentationLabelsAreFrozenAndValidated()
    {
        var rules = $$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:mark-label","revision":1,"triggers":[{"id":"opening","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}""";
        string Presentation(string value)
        {
            using var labelDocument = JsonDocument.Parse(value);
            return JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:mark-label"] = new { name = "Mark", description = "Declared physical names", cardMarks = labelDocument.RootElement } } });
        }
        var labels = SkillProgramCatalog.Load(rules, Presentation("{\"mark\":\"Label\"}")).Presentations["fixture:mark-label"].CardMarks!;
        var frozen = false;
        try { ((IDictionary<string, string>)labels)["mark"] = "changed"; } catch (NotSupportedException) { frozen = true; }
        Require(frozen && labels["mark"] == "Label", "A loaded presentation freezes its mark-label map.");
        foreach (var value in new[] { "{\"mark\":\"\"}", "{\"mark\":\"A\",\"mark\":\"B\"}", "{\"mark\":17}" })
        {
            try { SkillProgramCatalog.Load(rules, Presentation(value)); }
            catch (InvalidOperationException error) when (error.Message.Contains("cardMarks", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Invalid declared card mark labels were accepted.");
        }
    }

    private delegate void SourceProjection(int cardId, ref CardSkillMarkProjection projection);

    private sealed class Sources : ICardSkillMarkSources
    {
        internal SourceProjection? ProjectLabels;
        public void Project(int cardId, ref CardSkillMarkProjection projection) => ProjectLabels?.Invoke(cardId, ref projection);
    }

    private sealed class Marks : ICardEntityMarkQueries
    {
        internal IReadOnlyList<(string Mark, string Label)> Names = [];
        public IReadOnlyList<(string Mark, string Label)> MarkedNames(int cardId) => Names;
        public bool Has(int cardId, string mark) => false;
        public int? InitialOwnerSeat(int cardId, string mark) => null;
        public IReadOnlyList<int> MarkedCardIds(string mark) => [];
        public int CountMarked(IEnumerable<int> cardIds, string mark) => 0;
        public int CountMarked(int seat, string mark, CardZoneKind zone) => 0;
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
