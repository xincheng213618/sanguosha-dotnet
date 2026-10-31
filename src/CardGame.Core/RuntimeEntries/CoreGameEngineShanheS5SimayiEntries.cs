#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5SimayiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5SimayiRuntime> _resolve;
    private CoreGameEngineShanheS5SimayiEntries(Func<EngineOwners.CoreGameEngineShanheS5SimayiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5SimayiEntries Create(Func<EngineOwners.CoreGameEngineShanheS5SimayiRuntime> resolve) => new(resolve);
}
