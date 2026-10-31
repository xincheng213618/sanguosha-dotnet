#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime))]
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> _resolve;
    private CoreGameEnginePairObtainFixedRecipientReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairObtainFixedRecipientReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> resolve) => new(resolve);
}
