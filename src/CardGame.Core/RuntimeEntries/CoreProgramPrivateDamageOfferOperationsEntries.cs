#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime))]
internal sealed partial class CoreProgramPrivateDamageOfferOperationsEntries
{
    private readonly Func<EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> _resolve;
    private CoreProgramPrivateDamageOfferOperationsEntries(Func<EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPrivateDamageOfferOperationsEntries Create(Func<EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> resolve) => new(resolve);
}
