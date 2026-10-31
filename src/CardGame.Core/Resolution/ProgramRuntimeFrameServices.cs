namespace CardGame.Core;

// Trusted frame operations reach the actual scheduler and identity owner.
// Continuing a frame resolves the same deferred router as registered handlers.
internal sealed class ProgramRuntimeFrameServices(ResolutionScheduler scheduler,
    NativeFrameRoutingProvider routing) : IProgramRuntimeFrames
{
    private readonly ResolutionIdentitySequence _identities = scheduler.Identities;
    public ResolutionFrame? Top => scheduler.Top;
    public ResolutionFrame? Find(long id) => scheduler.Find(id);
    public ResolutionFrame? ParentOf(long id) => scheduler.ParentOf(id);
    public ResolutionFrame? ChildOf(long id) => scheduler.ChildOf(id);
    public IReadOnlyList<T> OfType<T>() where T : ResolutionFrame => scheduler.OfType<T>();
    public ProgramSkillFrame ActiveProgram(long id) => scheduler.ActiveProgram(id);
    public long NextId() => _identities.Next();
    public void ReplaceTop(ResolutionFrame frame) => scheduler.ReplaceTop(frame);
    public void Replace(long id, ResolutionFrame frame) => scheduler.Replace(id, frame);
    public void Push(ResolutionFrame frame) => scheduler.Push(frame);
    public void Pop(long id, ResolutionFrameKind kind) => scheduler.Complete(id, kind);
    public void Advance(long id) => routing.Runtime.AdvanceFrame(id);
    public void AdvanceTop<T>() where T : ResolutionFrame => routing.Runtime.AdvanceTop<T>();
    public void AdvanceProgram(long id) => routing.Runtime.AdvanceProgram(id);
}
