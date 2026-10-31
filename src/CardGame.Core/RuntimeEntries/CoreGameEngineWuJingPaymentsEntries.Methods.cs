#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingPaymentsEntries
{
    internal bool HasWuJingHejiTrickPaymentProcessing(IReadOnlyList<Card> processing) => _resolve().HasWuJingHejiTrickPaymentProcessing(processing);
}
