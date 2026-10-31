#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellMengpoProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _resolve;
    private CoreHellMengpoProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellMengpoProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> resolve) => new(resolve);
}
