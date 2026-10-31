#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> _resolve;
    private CoreGameEngineJiqiaoProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJiqiaoProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> resolve) => new(resolve);
}
