#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed partial class CoreProgramCardLegalityEntries
{
    private readonly Func<EngineOwners.CoreProgramCardLegalityRuntime> _resolve;
    private CoreProgramCardLegalityEntries(Func<EngineOwners.CoreProgramCardLegalityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramCardLegalityEntries Create(Func<EngineOwners.CoreProgramCardLegalityRuntime> resolve) => new(resolve);
}
