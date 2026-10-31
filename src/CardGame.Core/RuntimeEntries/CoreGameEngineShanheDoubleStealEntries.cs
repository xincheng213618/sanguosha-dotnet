#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDoubleStealEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheDoubleStealRuntime> _resolve;
    private CoreGameEngineShanheDoubleStealEntries(Func<EngineOwners.CoreGameEngineShanheDoubleStealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDoubleStealEntries Create(Func<EngineOwners.CoreGameEngineShanheDoubleStealRuntime> resolve) => new(resolve);
}
