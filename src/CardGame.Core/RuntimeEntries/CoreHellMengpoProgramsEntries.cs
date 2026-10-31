#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed partial class CoreHellMengpoProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _resolve;
    private CoreHellMengpoProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellMengpoProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> resolve) => new(resolve);
}
