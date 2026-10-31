#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellDizangProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> _resolve;
    private CoreHellDizangProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHellDizangProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> resolve) => new(resolve);
}
