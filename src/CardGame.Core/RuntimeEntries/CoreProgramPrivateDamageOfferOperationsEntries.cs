#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime))]
internal sealed partial class CoreProgramPrivateDamageOfferOperationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> _resolve;
    private CoreProgramPrivateDamageOfferOperationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPrivateDamageOfferOperationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramPrivateDamageOfferOperationsRuntime> resolve) => new(resolve);
}
