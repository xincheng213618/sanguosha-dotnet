#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime))]
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> _resolve;
    private CoreGameEngineProgramBooleanStateExpirationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramBooleanStateExpirationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> resolve) => new(resolve);
}
