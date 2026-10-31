#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellActivityProgramsRuntime> _resolve;
    private CoreHellActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellActivityProgramsRuntime> resolve) => new(resolve);
}
