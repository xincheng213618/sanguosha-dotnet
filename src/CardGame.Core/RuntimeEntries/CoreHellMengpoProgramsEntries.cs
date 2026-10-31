#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed partial class CoreHellMengpoProgramsEntries
{
    private readonly Func<EngineOwners.CoreHellMengpoProgramsRuntime> _resolve;
    private CoreHellMengpoProgramsEntries(Func<EngineOwners.CoreHellMengpoProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellMengpoProgramsEntries Create(Func<EngineOwners.CoreHellMengpoProgramsRuntime> resolve) => new(resolve);
}
