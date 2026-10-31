#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineZhuijiProgramsRuntime))]
internal sealed partial class CoreGameEngineZhuijiProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineZhuijiProgramsRuntime> _resolve;
    private CoreGameEngineZhuijiProgramsEntries(Func<EngineOwners.CoreGameEngineZhuijiProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhuijiProgramsEntries Create(Func<EngineOwners.CoreGameEngineZhuijiProgramsRuntime> resolve) => new(resolve);
}
