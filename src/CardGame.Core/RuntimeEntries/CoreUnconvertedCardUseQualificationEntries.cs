#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreUnconvertedCardUseQualificationRuntime))]
internal sealed partial class CoreUnconvertedCardUseQualificationEntries
{
    private readonly Func<EngineOwners.CoreUnconvertedCardUseQualificationRuntime> _resolve;
    private CoreUnconvertedCardUseQualificationEntries(Func<EngineOwners.CoreUnconvertedCardUseQualificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreUnconvertedCardUseQualificationEntries Create(Func<EngineOwners.CoreUnconvertedCardUseQualificationRuntime> resolve) => new(resolve);
}
