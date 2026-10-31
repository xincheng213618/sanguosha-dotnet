#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAllHandTrickPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> _resolve;
    private CoreGameEngineProgramAllHandTrickPaymentsEntries(Func<EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAllHandTrickPaymentsEntries Create(Func<EngineOwners.CoreGameEngineProgramAllHandTrickPaymentsRuntime> resolve) => new(resolve);
}
