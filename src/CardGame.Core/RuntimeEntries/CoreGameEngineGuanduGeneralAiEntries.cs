#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> _resolve;
    private CoreGameEngineGuanduGeneralAiEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralAiEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> resolve) => new(resolve);
}
