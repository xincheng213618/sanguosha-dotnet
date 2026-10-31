#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseRolesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _resolve;
    private CoreGameEngineProgramCardUseRolesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseRolesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> resolve) => new(resolve);
}
