#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed partial class CoreNationalGeneralRevealEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> _resolve;
    private CoreNationalGeneralRevealEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNationalGeneralRevealEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> resolve) => new(resolve);
}
