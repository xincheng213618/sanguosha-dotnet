#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceOwnerGainsRuntime))]
internal sealed partial class CoreGameEngineSourceOwnerGainsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> _resolve;
    private CoreGameEngineSourceOwnerGainsEntries(Func<EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceOwnerGainsEntries Create(Func<EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve) => new(resolve);
}
