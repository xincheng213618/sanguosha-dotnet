#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreHellActivityProgramsRuntime> _resolve;
    private CoreHellActivityProgramsEntries(Func<EngineOwners.CoreHellActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellActivityProgramsEntries Create(Func<EngineOwners.CoreHellActivityProgramsRuntime> resolve) => new(resolve);
}
