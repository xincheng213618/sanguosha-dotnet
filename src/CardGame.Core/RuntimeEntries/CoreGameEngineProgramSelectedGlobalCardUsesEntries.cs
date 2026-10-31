#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime))]
internal sealed partial class CoreGameEngineProgramSelectedGlobalCardUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> _resolve;
    private CoreGameEngineProgramSelectedGlobalCardUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSelectedGlobalCardUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> resolve) => new(resolve);
}
