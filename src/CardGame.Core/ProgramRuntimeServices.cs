namespace CardGame.Core;

// Trusted runtime primitives preserve the command session and the one frame
// store. Modules receive individual frames, never the mutable store itself.
internal interface IProgramRuntimeFrames : IProgramRuntimeOperations
{
    ResolutionFrame? Top { get; }
    ResolutionFrame? Find(long id);
    ResolutionFrame? ParentOf(long id);
    ResolutionFrame? ChildOf(long id);
    IReadOnlyList<T> OfType<T>() where T : ResolutionFrame;
    ProgramSkillFrame ActiveProgram(long id);
    long NextId();
    void ReplaceTop(ResolutionFrame frame);
    void Replace(long id, ResolutionFrame frame);
    void Push(ResolutionFrame frame);
    void Pop(long id, ResolutionFrameKind kind);
    void Advance(long id);
    void AdvanceTop<T>() where T : ResolutionFrame;
    void AdvanceProgram(long id);
}

internal interface IProgramRuntimeEvents : IProgramRuntimeOperations
{
    void AdvanceFact(IGameEvent fact);
}

internal interface IProgramRuntimePrompts : IProgramRuntimeOperations
{
    PendingDecision? Current { get; }
    PromptId NextId();
    void Clear();
    void Set(PendingDecision decision, bool isHuman);
}

public sealed partial class GameEngine
{
}
