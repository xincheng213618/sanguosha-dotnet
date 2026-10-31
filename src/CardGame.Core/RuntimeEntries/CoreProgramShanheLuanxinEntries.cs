#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheLuanxinEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheLuanxinRuntime> _resolve;
    private CoreProgramShanheLuanxinEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheLuanxinRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheLuanxinEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheLuanxinRuntime> resolve) => new(resolve);
}
