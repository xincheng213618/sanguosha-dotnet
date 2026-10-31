#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> _resolve;
    private CoreGameEngineGuanduGeneralChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralChoicesRuntime> resolve) => new(resolve);
}
