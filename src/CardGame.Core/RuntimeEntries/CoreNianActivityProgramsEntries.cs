#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNianActivityProgramsRuntime))]
internal sealed partial class CoreNianActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreNianActivityProgramsRuntime> _resolve;
    private CoreNianActivityProgramsEntries(Func<EngineOwners.CoreNianActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNianActivityProgramsEntries Create(Func<EngineOwners.CoreNianActivityProgramsRuntime> resolve) => new(resolve);
}
