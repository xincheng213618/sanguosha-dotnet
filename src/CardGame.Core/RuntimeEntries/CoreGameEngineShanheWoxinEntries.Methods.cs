#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheWoxinEntries
{
    internal ShanheWoxinCreditEarnedEvent[] AvailableWoxinCredits(CardUseEffectSource source, string hash) => _resolve().AvailableWoxinCredits(source, hash);
    internal IEnumerable<ShanheWoxinReservation> LiveWoxinReservations() => _resolve().LiveWoxinReservations();
    internal void RecordWoxinAppliedDamage(IGameEvent payload) => _resolve().RecordWoxinAppliedDamage(payload);
    internal int ReserveWoxinDamage(GameEngine.IDamageAttempt attack) => _resolve().ReserveWoxinDamage(attack);
    internal int WoxinPublicBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, GameEngine.IDamageAttempt? attack) => _resolve().WoxinPublicBonus(view, sourceSeat, sourceLess, chain, attack);
    internal ShanheWoxinReservation? WoxinReceipt(GameEngine.IDamageAttempt attack) => _resolve().WoxinReceipt(attack);
    internal CardUseEffectSource? WoxinSource(int owner, IReadOnlySet<string>? visible = default) => _resolve().WoxinSource(owner, visible);
    internal bool HasWoxinPolicy { get => _resolve().HasWoxinPolicy; }
}
