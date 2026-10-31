#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHandGuidanceRuntime))]
internal sealed partial class CoreGameEngineHandGuidanceEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHandGuidanceRuntime> _resolve;
    private CoreGameEngineHandGuidanceEntries(Func<EngineOwners.CoreGameEngineHandGuidanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHandGuidanceEntries Create(Func<EngineOwners.CoreGameEngineHandGuidanceRuntime> resolve) => new(resolve);
}
