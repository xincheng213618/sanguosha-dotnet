namespace CardGame.Core;

internal interface INativeFrameRoutingFacts : IProgramSelectionQuery
{
    int FrameCount { get; }
    int LastFrameIndex(long id);
    ResolutionFrame FrameAt(int index);
}

internal interface INativeFrameRoutes : IProgramRuntimeOperations
{
    bool TryAdvance(ResolutionFrame frame);
}

internal interface INativeProgramExecution : IProgramRuntimeOperations
{
    bool TryResume(long frameId);
    void Execute(long frameId);
}

[ProgramRuntimeModule]
internal sealed class NativeFrameRoutingRuntime(IProgramRuntimeFrames frames,
    INativeFrameRoutingFacts facts, INativeFrameRoutes routes, INativeProgramExecution programs)
{
    internal void AdvanceTop<TFrame>() where TFrame : ResolutionFrame
    {
        if (frames.Top is TFrame frame) AdvanceFrame(frame.Id);
    }

    internal void AdvanceProgram(long frameId)
    {
        if (frames.Top is ProgramSkillFrame frame && frame.Id == frameId) AdvanceFrame(frameId);
    }

    internal void AdvanceFrame(long frameId)
    {
        var index = facts.LastFrameIndex(frameId);
        if (index < 0) throw new InvalidOperationException($"Runtime continuation {frameId} has no owner.");
        if (index != facts.FrameCount - 1) return;
        if (!routes.TryAdvance(facts.FrameAt(index)))
            throw new InvalidOperationException("The requested frame has no runtime continuation handler.");
    }

    [NativeFrameRoute(typeof(ProgramSkillFrame), 210)]
    internal void ContinueProgram(ProgramSkillFrame frame)
    {
        if (programs.TryResume(frame.Id)) return;
        programs.Execute(frame.Id);
    }
}

public sealed partial class GameEngine
{
}
