#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime))]
internal sealed partial class CoreGameEngineProgramDiscardOriginsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> _resolve;
    private CoreGameEngineProgramDiscardOriginsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardOriginsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> resolve) => new(resolve);
}
