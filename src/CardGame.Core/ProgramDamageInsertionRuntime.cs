namespace CardGame.Core;

internal interface IProgramDamageInsertionFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int FrameIndex(long frameId);
    bool HasIssuedLink(int index);
    bool HasConsistentChild(int index);
    long? AttackOwnerFrameId { get; }
    long? DyingFrameId { get; }
}

// The scheduler owns ancestry. Rules own their trigger, cost and damage amount;
// inserting an admitted effect never needs another skill-specific permission.
[ProgramRuntimeModule]
internal sealed class ProgramDamageInsertionRuntime(IProgramDamageInsertionFacts facts)
{
    internal ProgramInsertionReturn? Describe(ProgramSkillFrame frame)
    {
        if (facts.FrameCount == 0 || facts.FrameAt(facts.FrameCount - 1) is not ProgramSkillFrame current ||
            current.Id != frame.Id || current.RuntimeOrigin?.Program is not { } identity || !identity.Matches(frame) ||
            frame.AttackAttempt is not null) return null;
        var attack = facts.AttackOwnerFrameId;
        var dying = facts.DyingFrameId;
        var roots = new[] { attack, dying }.Where(id => id is not null)
            .Select(id => facts.FrameIndex(id!.Value)).ToArray();
        if (roots.Length == 0 || roots.Any(index => index < 0)) return null;
        var root = roots.Min();
        if (root >= facts.FrameCount - 1) return null;
        for (var index = root; index < facts.FrameCount; index++)
            if (!facts.HasIssuedLink(index) || index > root && !facts.HasConsistentChild(index)) return null;
        return new(facts.FrameAt(root).Id, dying);
    }

    internal bool IsSuspended(DyingFrame dying)
    {
        var dyingIndex = facts.FrameIndex(dying.Id);
        for (var index = dyingIndex + 1; dyingIndex >= 0 && index < facts.FrameCount; index++)
            if (facts.FrameAt(index) is ProgramSkillFrame { AttackAttempt: not null, AttackReturn.Insertion: { } origin } &&
                origin.SuspendedDyingFrameId == dying.Id && HasIssuedPath(origin.ScopeRootFrameId, index) &&
                HasIssuedPath(dying.Id, index)) return true;
        return false;
    }

    internal void AssertScope(ProgramSkillFrame frame)
    {
        if (frame.AttackReturn?.Insertion is not { } origin) return;
        var index = facts.FrameIndex(frame.Id);
        var dyingIndex = origin.SuspendedDyingFrameId is { } suspended ? facts.FrameIndex(suspended) : -1;
        if (frame.AttackAttempt is null || !HasIssuedPath(origin.ScopeRootFrameId, index) ||
            frame.AttackReturn.ParentAttackOwnerFrameId is { } attack && !HasIssuedPath(attack, index) ||
            origin.SuspendedDyingFrameId is { } dying &&
                (dyingIndex < 0 || facts.FrameAt(dyingIndex) is not DyingFrame || !HasIssuedPath(dying, index)))
            throw new InvalidOperationException("An inserted damage lost its scheduler-owned parent scope or suspended dying return.");
    }

    private bool HasIssuedPath(long rootFrameId, int childIndex)
    {
        var root = facts.FrameIndex(rootFrameId);
        if (root < 0 || childIndex <= root) return false;
        for (var index = root; index <= childIndex; index++)
            if (!facts.HasIssuedLink(index)) return false;
        return true;
    }
}
