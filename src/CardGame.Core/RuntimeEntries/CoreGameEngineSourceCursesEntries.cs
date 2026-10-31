#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceCursesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _resolve;
    private CoreGameEngineSourceCursesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceCursesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> resolve) => new(resolve);
}
