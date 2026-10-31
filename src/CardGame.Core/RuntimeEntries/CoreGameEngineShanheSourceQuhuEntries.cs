#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQuhuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> _resolve;
    private CoreGameEngineShanheSourceQuhuEntries(Func<EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceQuhuEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> resolve) => new(resolve);
}
