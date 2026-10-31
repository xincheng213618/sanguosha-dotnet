#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawAdviceProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> _resolve;
    private CoreGameEngineDrawAdviceProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawAdviceProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime> resolve) => new(resolve);
}
