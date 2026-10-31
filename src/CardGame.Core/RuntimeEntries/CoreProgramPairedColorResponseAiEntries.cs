#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramPairedColorResponseAiRuntime))]
internal sealed partial class CoreProgramPairedColorResponseAiEntries
{
    private readonly Func<EngineOwners.CoreProgramPairedColorResponseAiRuntime> _resolve;
    private CoreProgramPairedColorResponseAiEntries(Func<EngineOwners.CoreProgramPairedColorResponseAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramPairedColorResponseAiEntries Create(Func<EngineOwners.CoreProgramPairedColorResponseAiRuntime> resolve) => new(resolve);
}
