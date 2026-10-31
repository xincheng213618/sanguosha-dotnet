#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed partial class CoreGameEngineProgramCardUseRolesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _resolve;
    private CoreGameEngineProgramCardUseRolesEntries(Func<EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseRolesEntries Create(Func<EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> resolve) => new(resolve);
}
