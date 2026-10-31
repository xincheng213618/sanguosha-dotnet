#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime))]
internal sealed partial class CoreGameEngineSourceExecutionerYishiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> _resolve;
    private CoreGameEngineSourceExecutionerYishiEntries(Func<EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerYishiEntries Create(Func<EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> resolve) => new(resolve);
}
