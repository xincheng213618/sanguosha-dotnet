#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed partial class CoreHellKingProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> _resolve;
    private CoreHellKingProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellKingProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> resolve) => new(resolve);
}
