#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed partial class CoreGameEngineDesignatedExtraTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _resolve;
    private CoreGameEngineDesignatedExtraTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDesignatedExtraTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> resolve) => new(resolve);
}
