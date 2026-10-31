#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramChoicesRuntime))]
internal sealed partial class CoreGameEngineProgramChoicesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramChoicesRuntime> _resolve;
    private CoreGameEngineProgramChoicesEntries(Func<EngineOwners.CoreGameEngineProgramChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramChoicesEntries Create(Func<EngineOwners.CoreGameEngineProgramChoicesRuntime> resolve) => new(resolve);
}
