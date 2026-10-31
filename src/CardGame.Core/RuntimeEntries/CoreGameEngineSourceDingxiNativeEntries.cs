#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceDingxiNativeRuntime))]
internal sealed partial class CoreGameEngineSourceDingxiNativeEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _resolve;
    private CoreGameEngineSourceDingxiNativeEntries(Func<EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiNativeEntries Create(Func<EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve) => new(resolve);
}
