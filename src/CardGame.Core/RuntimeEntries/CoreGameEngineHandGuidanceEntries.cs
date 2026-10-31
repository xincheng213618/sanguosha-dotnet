#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime))]
internal sealed partial class CoreGameEngineHandGuidanceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime> _resolve;
    private CoreGameEngineHandGuidanceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHandGuidanceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime> resolve) => new(resolve);
}
