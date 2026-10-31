#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAvailableBoundSubsetProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> _resolve;
    private CoreAvailableBoundSubsetProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAvailableBoundSubsetProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> resolve) => new(resolve);
}
