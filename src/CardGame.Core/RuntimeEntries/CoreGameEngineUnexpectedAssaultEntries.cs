#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnexpectedAssaultRuntime))]
internal sealed partial class CoreGameEngineUnexpectedAssaultEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _resolve;
    private CoreGameEngineUnexpectedAssaultEntries(Func<EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnexpectedAssaultEntries Create(Func<EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> resolve) => new(resolve);
}
