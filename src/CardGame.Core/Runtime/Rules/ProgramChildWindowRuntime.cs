namespace CardGame.Core;

internal interface IProgramChildWindows : IProgramRuntimeOperations
{
    bool Begin(long frameId, PostEventContinuation continuation);
}

internal interface IProgramChildWindowEntries : IProgramRuntimeOperations
{
    bool Recovery(long frameId, PostEventContinuation continuation);
    bool CharacterState(long frameId);
    bool HpChanged(long frameId, PostEventContinuation continuation);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramChildWindowRuntime(IProgramChildWindowEntries entries,
    IProgramMovementWindows movement, IProgramSkillsChangedWindows skills) : IProgramChildWindows
{
    public bool Begin(long frameId, PostEventContinuation continuation) =>
        entries.Recovery(frameId, continuation) || entries.CharacterState(frameId) ||
        entries.HpChanged(frameId, continuation) || movement.Begin(frameId) || skills.Begin(frameId);
}

public sealed partial class GameEngine
{
}
