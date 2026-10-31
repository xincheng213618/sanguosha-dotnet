#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> _resolve;
    private CoreGameEngineGuanduGeneralAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralAiRuntime> resolve) => new(resolve);
}
