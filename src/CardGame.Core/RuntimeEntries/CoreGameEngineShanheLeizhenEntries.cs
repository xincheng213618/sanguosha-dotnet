#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheLeizhenRuntime))]
internal sealed partial class CoreGameEngineShanheLeizhenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheLeizhenRuntime> _resolve;
    private CoreGameEngineShanheLeizhenEntries(Func<EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheLeizhenEntries Create(Func<EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve) => new(resolve);
}
