#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduGeneralsRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralsEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralsEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralsRuntime> resolve) => new(resolve);
}
