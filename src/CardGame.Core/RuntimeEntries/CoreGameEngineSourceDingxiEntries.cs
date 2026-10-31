#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceDingxiRuntime))]
internal sealed partial class CoreGameEngineSourceDingxiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceDingxiRuntime> _resolve;
    private CoreGameEngineSourceDingxiEntries(Func<EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiEntries Create(Func<EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve) => new(resolve);
}
