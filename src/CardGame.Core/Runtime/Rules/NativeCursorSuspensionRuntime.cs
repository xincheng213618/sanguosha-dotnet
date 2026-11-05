namespace CardGame.Core;

[Flags]
public enum NativeCursorScope
{
    None = 0,
    AttackAndContinuations = 1,
    Dying = 2,
    Nullification = 4,
    All = AttackAndContinuations | Dying | Nullification
}

public sealed record NativeCursorSuspension(long IssuerFrameId, NativeCursorScope Scope);

public abstract partial record ResolutionFrame
{
    private readonly IReadOnlyList<NativeCursorSuspension> _nativeCursorSuspensions =
        Array.AsReadOnly(Array.Empty<NativeCursorSuspension>());
    public IReadOnlyList<NativeCursorSuspension> NativeCursorSuspensions
    {
        get => _nativeCursorSuspensions;
        init => _nativeCursorSuspensions = Array.AsReadOnly(value.ToArray());
    }
}

internal interface INativeCursorSuspensions : IProgramRuntimeOperations
{
    void Suspend(long ownerId, long issuerFrameId, NativeCursorScope scopes);
    void Release(long issuerFrameId, NativeCursorScope scopes = NativeCursorScope.All);
    bool IsSuspended(long ownerId, NativeCursorScope scope);
    bool HasIssuedSuspension(long ownerId, NativeCursorScope scope, long? issuerParentFrameId = null);
    bool OwnsSuspension(long ownerId, long issuerFrameId, NativeCursorScope scope);
}

// A paid producer declares its exact interval once. Visibility reads consume the
// original frame's declaration, independently of source skills and event history.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCursorSuspensionRuntime(IProgramRuntimeFrames frames,
    IPublicHandRevealWindows publicReveal) : INativeCursorSuspensions
{
    public void Suspend(long ownerId, long issuerFrameId, NativeCursorScope scopes)
    {
        ValidateScope(scopes);
        var owner = frames.Find(ownerId);
        if (owner is null || ownerId == issuerFrameId || frames.Find(issuerFrameId) is null ||
            !IsAncestor(ownerId, issuerFrameId) ||
            (scopes & NativeCursorScope.AttackAndContinuations) != 0 && owner is not (CardUseFrame or ProgramSkillFrame or JudgmentFrame) ||
            (scopes & NativeCursorScope.Dying) != 0 && owner is not DyingFrame ||
            (scopes & NativeCursorScope.Nullification) != 0 && owner is not NullificationWindowFrame)
            throw new InvalidOperationException("A native cursor suspension requires its actual ancestor owner, live issuer and supported cursor scope.");
        var existing = owner.NativeCursorSuspensions.SingleOrDefault(item => item.IssuerFrameId == issuerFrameId);
        if (existing is not null && (existing.Scope & scopes) == scopes) return;
        frames.Replace(ownerId, owner with
        {
            NativeCursorSuspensions = existing is null
                ? owner.NativeCursorSuspensions.Append(new(issuerFrameId, scopes)).ToArray()
                : owner.NativeCursorSuspensions.Select(item => item == existing
                    ? item with { Scope = item.Scope | scopes } : item).ToArray()
        });
    }

    public void Release(long issuerFrameId, NativeCursorScope scopes = NativeCursorScope.All)
    {
        ValidateScope(scopes);
        if (issuerFrameId <= 0) throw new InvalidOperationException("A native suspension release requires its original issuer identity.");
        foreach (var owner in frames.OfType<ResolutionFrame>().ToArray())
        {
            if (!owner.NativeCursorSuspensions.Any(item => item.IssuerFrameId == issuerFrameId && (item.Scope & scopes) != 0)) continue;
            frames.Replace(owner.Id, owner with
            {
                NativeCursorSuspensions = owner.NativeCursorSuspensions
                    .Select(item => item.IssuerFrameId == issuerFrameId ? item with { Scope = item.Scope & ~scopes } : item)
                    .Where(item => item.Scope != NativeCursorScope.None).ToArray()
            });
        }
    }

    public bool IsSuspended(long ownerId, NativeCursorScope scope)
    {
        ValidateScope(scope);
        return publicReveal.Suspends(ownerId) || Matches(ownerId, scope);
    }

    // Continuation qualification consumes the actual issued interval, not the
    // broader visibility effect of an unrelated public reveal. Response returns
    // can further require the issuer's exact original window.
    public bool HasIssuedSuspension(long ownerId, NativeCursorScope scope, long? issuerParentFrameId = null)
    {
        ValidateScope(scope);
        return Matches(ownerId, scope, issuerParentFrameId: issuerParentFrameId);
    }

    public bool OwnsSuspension(long ownerId, long issuerFrameId, NativeCursorScope scope)
    {
        ValidateScope(scope);
        return Matches(ownerId, scope, issuerFrameId);
    }

    private bool Matches(long ownerId, NativeCursorScope scope, long? issuerFrameId = null, long? issuerParentFrameId = null) =>
        frames.Find(ownerId)?.NativeCursorSuspensions.Any(item => (item.Scope & scope) != 0 &&
            (issuerFrameId is null || item.IssuerFrameId == issuerFrameId) && frames.Find(item.IssuerFrameId) is not null &&
            (issuerParentFrameId is null || frames.ParentOf(item.IssuerFrameId)?.Id == issuerParentFrameId)) == true;

    private bool IsAncestor(long ownerId, long issuerFrameId)
    {
        for (var parent = frames.ParentOf(issuerFrameId); parent is not null; parent = frames.ParentOf(parent.Id))
            if (parent.Id == ownerId) return true;
        return false;
    }

    private static void ValidateScope(NativeCursorScope scope)
    {
        if (scope == NativeCursorScope.None || (scope & ~NativeCursorScope.All) != 0)
            throw new InvalidOperationException("A native cursor suspension requires a supported nonempty scope.");
    }
}
