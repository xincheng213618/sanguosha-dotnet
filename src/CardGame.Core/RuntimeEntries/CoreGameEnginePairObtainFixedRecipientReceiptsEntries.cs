#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime))]
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> _resolve;
    private CoreGameEnginePairObtainFixedRecipientReceiptsEntries(Func<EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairObtainFixedRecipientReceiptsEntries Create(Func<EngineOwners.CoreGameEnginePairObtainFixedRecipientReceiptsRuntime> resolve) => new(resolve);
}
