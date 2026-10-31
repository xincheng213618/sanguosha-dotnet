#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGameActivationAwakeningRuntime))]
internal sealed partial class CoreGameEngineGameActivationAwakeningEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> _resolve;
    private CoreGameEngineGameActivationAwakeningEntries(Func<EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameActivationAwakeningEntries Create(Func<EngineOwners.CoreGameEngineGameActivationAwakeningRuntime> resolve) => new(resolve);
}
