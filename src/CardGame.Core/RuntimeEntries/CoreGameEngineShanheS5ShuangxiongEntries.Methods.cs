#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ShuangxiongEntries
{
    internal bool IsRoundScaledPaidColorSource(global::CardGame.Core.CardUseEffectSource source) => _resolve().IsRoundScaledPaidColorSource(source);
    internal void RecordShanheS5ShuangxiongDuel(global::CardGame.Core.CardUseDeclaredEvent declared) => _resolve().RecordShanheS5ShuangxiongDuel(declared);
    internal void RecordShanheS5ShuangxiongPayment(global::CardGame.Core.ProgramPaidColorConversionPaidEvent paid) => _resolve().RecordShanheS5ShuangxiongPayment(paid);
    internal bool TryGetShanheS5ShuangxiongDuelBaseDamage(long useId, scoped out int damage) => _resolve().TryGetShanheS5ShuangxiongDuelBaseDamage(useId, out damage);
    internal bool TryGetShanheS5ShuangxiongPublicDuelBaseDamage(int viewerSeat, global::CardGame.Core.LegalAction action, scoped out int damage) => _resolve().TryGetShanheS5ShuangxiongPublicDuelBaseDamage(viewerSeat, action, out damage);
}
