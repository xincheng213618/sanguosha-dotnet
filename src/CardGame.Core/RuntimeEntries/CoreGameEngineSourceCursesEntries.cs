#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed partial class CoreGameEngineSourceCursesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceCursesRuntime> _resolve;
    private CoreGameEngineSourceCursesEntries(Func<EngineOwners.CoreGameEngineSourceCursesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceCursesEntries Create(Func<EngineOwners.CoreGameEngineSourceCursesRuntime> resolve) => new(resolve);
}
