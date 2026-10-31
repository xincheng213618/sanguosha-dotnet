#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime))]
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> _resolve;
    private CoreGameEngineSourceExecutionerHuanfuEntries(Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerHuanfuEntries Create(Func<EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> resolve) => new(resolve);
}
