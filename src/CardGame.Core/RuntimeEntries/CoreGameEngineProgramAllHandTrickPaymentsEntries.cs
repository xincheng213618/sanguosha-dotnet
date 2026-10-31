#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAllHandTrickPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> _resolve;
    private CoreGameEngineProgramAllHandTrickPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAllHandTrickPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> resolve) => new(resolve);
}
