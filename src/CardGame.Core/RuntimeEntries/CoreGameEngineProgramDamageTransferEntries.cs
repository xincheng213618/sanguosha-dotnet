#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDamageTransferEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDamageTransferRuntime> _resolve;
    private CoreGameEngineProgramDamageTransferEntries(Func<EngineOwners.CoreGameEngineProgramDamageTransferRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDamageTransferEntries Create(Func<EngineOwners.CoreGameEngineProgramDamageTransferRuntime> resolve) => new(resolve);
}
