#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> _resolve;
    private CoreGameEngineDamagePreventionDrawEntries(Func<EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamagePreventionDrawEntries Create(Func<EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> resolve) => new(resolve);
}
