#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime))]
internal sealed partial class CoreGameEngineAssistedSlashPaymentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> _resolve;
    private CoreGameEngineAssistedSlashPaymentEntries(Func<EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAssistedSlashPaymentEntries Create(Func<EngineOwners.CoreGameEngineAssistedSlashPaymentRuntime> resolve) => new(resolve);
}
