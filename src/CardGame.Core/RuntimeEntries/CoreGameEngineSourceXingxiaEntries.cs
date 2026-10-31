#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceXingxiaRuntime))]
internal sealed partial class CoreGameEngineSourceXingxiaEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceXingxiaRuntime> _resolve;
    private CoreGameEngineSourceXingxiaEntries(Func<EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceXingxiaEntries Create(Func<EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve) => new(resolve);
}
