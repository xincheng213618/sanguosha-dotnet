#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramDamageInsertionRuntimeRuntime))]
internal sealed partial class CoreProgramDamageInsertionRuntimeEntries
{
    private readonly Func<EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> _resolve;
    private CoreProgramDamageInsertionRuntimeEntries(Func<EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramDamageInsertionRuntimeEntries Create(Func<EngineOwners.CoreProgramDamageInsertionRuntimeRuntime> resolve) => new(resolve);
}
