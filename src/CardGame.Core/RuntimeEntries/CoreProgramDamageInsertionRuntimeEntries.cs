#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime))]
internal sealed partial class CoreProgramDamageInsertionRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> _resolve;
    private CoreProgramDamageInsertionRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramDamageInsertionRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> resolve) => new(resolve);
}
