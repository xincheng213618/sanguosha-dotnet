#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed partial class CoreProgramCardUsePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _resolve;
    private CoreProgramCardUsePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramCardUsePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> resolve) => new(resolve);
}
