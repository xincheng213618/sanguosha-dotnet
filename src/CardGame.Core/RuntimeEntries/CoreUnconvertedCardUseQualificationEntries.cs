#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime))]
internal sealed partial class CoreUnconvertedCardUseQualificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime> _resolve;
    private CoreUnconvertedCardUseQualificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreUnconvertedCardUseQualificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreUnconvertedCardUseQualificationRuntime> resolve) => new(resolve);
}
