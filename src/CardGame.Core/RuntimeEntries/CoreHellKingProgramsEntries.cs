#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed partial class CoreHellKingProgramsEntries
{
    private readonly Func<EngineOwners.CoreHellKingProgramsRuntime> _resolve;
    private CoreHellKingProgramsEntries(Func<EngineOwners.CoreHellKingProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellKingProgramsEntries Create(Func<EngineOwners.CoreHellKingProgramsRuntime> resolve) => new(resolve);
}
