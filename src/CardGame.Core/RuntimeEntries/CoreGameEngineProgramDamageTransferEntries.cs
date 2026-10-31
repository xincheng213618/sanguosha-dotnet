#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDamageTransferEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageTransferRuntime> _resolve;
    private CoreGameEngineProgramDamageTransferEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageTransferRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDamageTransferEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageTransferRuntime> resolve) => new(resolve);
}
