#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime))]
internal sealed partial class CoreGameEngineFixedRecipientBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> _resolve;
    private CoreGameEngineFixedRecipientBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFixedRecipientBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFixedRecipientBenefitsRuntime> resolve) => new(resolve);
}
