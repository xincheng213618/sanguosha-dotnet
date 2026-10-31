#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed partial class CoreGameEngineDesignatedExtraTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _resolve;
    private CoreGameEngineDesignatedExtraTargetsEntries(Func<EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDesignatedExtraTargetsEntries Create(Func<EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> resolve) => new(resolve);
}
