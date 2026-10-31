#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> _resolve;
    private CoreGameEngineGuanduGeneralChoicesEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralChoicesEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> resolve) => new(resolve);
}
