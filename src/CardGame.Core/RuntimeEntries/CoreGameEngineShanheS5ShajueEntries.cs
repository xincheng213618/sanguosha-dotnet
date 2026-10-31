#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ShajueEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5ShajueRuntime> _resolve;
    private CoreGameEngineShanheS5ShajueEntries(Func<EngineOwners.CoreGameEngineShanheS5ShajueRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShajueEntries Create(Func<EngineOwners.CoreGameEngineShanheS5ShajueRuntime> resolve) => new(resolve);
}
