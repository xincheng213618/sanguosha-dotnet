using CardGame.Core;

internal static class ProgramInsertionChecks
{
    public static void SchedulerOwnershipComposesAndOnlySuspendsOriginalDying()
    {
        var frames = new FrameStore();
        var root = Program(1, "original") with { AttackAttempt = new(0, 1, 1, DamageNature.Normal, false) };
        frames.PushOwned(root);
        frames.PushOwned(new DamageFrame(2, root.Id, 0, 1, 1));
        var dying = new DyingFrame(3, 2, 1, 0, [1], 0, DyingContinuationKind.Damage);
        frames.PushOwned(dying);
        // The intermediate frame has no entry in a damage permission registry.
        frames.PushOwned(new DeferredTurnEndFrame(4, 1, 1, false, []));
        var child = Program(5, "new-effect");
        frames.PushOwned(child);
        var facts = new Facts(frames) { AttackOwnerFrameId = root.Id, DyingFrameId = dying.Id };
        var runtime = new ProgramDamageInsertionRuntime(facts);
        var origin = runtime.Describe(child);
        Need(origin == new ProgramInsertionReturn(root.Id, dying.Id),
            "An arbitrary scheduler-issued child composes inside the current damage and dying scope without a skill registration.");
        Need(runtime.Describe(child with { SkillInstanceId = "forged" }) is null && runtime.Describe(root) is null,
            "A caller cannot borrow another source instance or resume a suspended parent as the active child.");
        facts.InconsistentIndex = 3;
        Need(runtime.Describe(child) is null, "Scheduler provenance cannot bypass an inconsistent frozen child contract.");
        facts.InconsistentIndex = -1;
        frames.Replace(child with { SkillInstanceId = "forged" });
        Need(runtime.Describe((ProgramSkillFrame)frames[^1]) is null,
            "Replacing program data does not reissue its original source identity.");
        frames.Replace(child);
        var stamp = child.RuntimeOrigin;
        Reject(() => frames.PushOwned(child));
        Need(ReferenceEquals(stamp, child.RuntimeOrigin), "A rejected duplicate push must leave the live ownership receipt untouched.");

        child = child with { AttackAttempt = new(1, 2, 1, DamageNature.Fire, false),
            AttackReturn = new(root.Id, null, null) { Insertion = origin } };
        frames.Replace(child);
        runtime.AssertScope(child);
        frames.PushOwned(new DamageFrame(6, child.Id, 1, 2, 1, Nature: DamageNature.Fire));
        var innerDying = new DyingFrame(7, 6, 2, 1, [0, 1, 2], 0, DyingContinuationKind.Damage);
        frames.PushOwned(innerDying);
        Need(runtime.IsSuspended(dying) && !runtime.IsSuspended(innerDying),
            "Inserting damage hides only its original dying token; the inner victim still receives ordinary rescue and death handling.");
        frames.CompleteTop(innerDying.Id, innerDying.Kind);
        frames.CompleteTop(6, ResolutionFrameKind.Damage);
        Reject(() => runtime.AssertScope(child with
            { AttackReturn = child.AttackReturn! with { Insertion = new(999, dying.Id) } }));
        frames.Replace(child with { AttackAttempt = null, AttackReturn = null });
        Need(!runtime.IsSuspended(dying), "Completing the child restores the original dying scope without an external pending object.");
        frames.CompleteTop(child.Id, child.Kind);
        var unissued = Program(8, "unissued");
        frames.Push(unissued);
        Need(runtime.Describe(unissued) is null, "A diagnostic frame without scheduler issuance cannot authorize inserted damage.");
    }

    private static ProgramSkillFrame Program(long id, string skill) =>
        new(id, 1, skill, "effect", "content", 1, [], []) { SkillInstanceId = "instance:" + skill };
    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Expected a rejected child ownership change.");
    }
    private sealed class Facts(FrameStore frames) : IProgramDamageInsertionFacts
    {
        public int FrameCount => frames.Count;
        public ResolutionFrame FrameAt(int index) => frames[index];
        public int FrameIndex(long id) => frames.FindIndex(frame => frame.Id == id);
        public bool HasIssuedLink(int index) => frames.HasIssuedLink(index);
        public bool HasConsistentChild(int index) => index != InconsistentIndex;
        public long? AttackOwnerFrameId { get; set; }
        public long? DyingFrameId { get; set; }
        internal int InconsistentIndex { get; set; } = -1;
    }
}
