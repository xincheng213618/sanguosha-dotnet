#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHellDizangProgramsRuntime))]
internal sealed partial class CoreHellDizangProgramsEntries
{
    private readonly Func<EngineOwners.CoreHellDizangProgramsRuntime> _resolve;
    private CoreHellDizangProgramsEntries(Func<EngineOwners.CoreHellDizangProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellDizangProgramsEntries Create(Func<EngineOwners.CoreHellDizangProgramsRuntime> resolve) => new(resolve);
}
