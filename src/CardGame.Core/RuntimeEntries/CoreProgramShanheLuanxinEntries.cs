#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheLuanxinEntries
{
    private readonly Func<EngineOwners.CoreProgramShanheLuanxinRuntime> _resolve;
    private CoreProgramShanheLuanxinEntries(Func<EngineOwners.CoreProgramShanheLuanxinRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheLuanxinEntries Create(Func<EngineOwners.CoreProgramShanheLuanxinRuntime> resolve) => new(resolve);
}
