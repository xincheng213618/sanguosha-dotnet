#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime))]
internal sealed partial class CoreGameEngineResponseCompletionZhangbaEntries
{
    private readonly Func<EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> _resolve;
    private CoreGameEngineResponseCompletionZhangbaEntries(Func<EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseCompletionZhangbaEntries Create(Func<EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> resolve) => new(resolve);
}
