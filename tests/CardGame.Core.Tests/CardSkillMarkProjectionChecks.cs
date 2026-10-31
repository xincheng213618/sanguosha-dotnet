using CardGame.Core;

internal static class CardSkillMarkProjectionChecks
{
    internal static void OrderedCompositionPreservesEmptyLabelsAndCapturedTail()
    {
        var sources = new Sources();
        var projector = new CardSkillMarkProjector(sources);
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
    }

    private delegate void SourceProjection(int cardId, ref CardSkillMarkProjection projection);

    private sealed class Sources : ICardSkillMarkSources
    {
        internal SourceProjection? ProjectLabels;
        public void Project(int cardId, ref CardSkillMarkProjection projection) => ProjectLabels?.Invoke(cardId, ref projection);
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
