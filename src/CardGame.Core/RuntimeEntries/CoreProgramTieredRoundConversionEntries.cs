#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramTieredRoundConversionRuntime))]
internal sealed partial class CoreProgramTieredRoundConversionEntries
{
    private readonly Func<EngineOwners.CoreProgramTieredRoundConversionRuntime> _resolve;
    private CoreProgramTieredRoundConversionEntries(Func<EngineOwners.CoreProgramTieredRoundConversionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramTieredRoundConversionEntries Create(Func<EngineOwners.CoreProgramTieredRoundConversionRuntime> resolve) => new(resolve);
}
