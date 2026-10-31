#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed partial class CoreNationalGeneralRevealEntries
{
    private readonly Func<EngineOwners.CoreNationalGeneralRevealRuntime> _resolve;
    private CoreNationalGeneralRevealEntries(Func<EngineOwners.CoreNationalGeneralRevealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNationalGeneralRevealEntries Create(Func<EngineOwners.CoreNationalGeneralRevealRuntime> resolve) => new(resolve);
}
