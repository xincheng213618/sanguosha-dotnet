#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheWoxinEntries
{
    internal global::CardGame.Core.ShanheWoxinCreditEarnedEvent[] AvailableWoxinCredits(global::CardGame.Core.CardUseEffectSource source, string hash) => _resolve().AvailableWoxinCredits(source, hash);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ShanheWoxinReservation> LiveWoxinReservations() => _resolve().LiveWoxinReservations();
    internal void RecordWoxinAppliedDamage(global::CardGame.Core.IGameEvent payload) => _resolve().RecordWoxinAppliedDamage(payload);
    internal int ReserveWoxinDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ReserveWoxinDamage(attack);
    internal int WoxinPublicBonus(global::CardGame.Core.GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, global::CardGame.Core.GameEngine.IDamageAttempt? attack) => _resolve().WoxinPublicBonus(view, sourceSeat, sourceLess, chain, attack);
    internal global::CardGame.Core.ShanheWoxinReservation? WoxinReceipt(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WoxinReceipt(attack);
    internal global::CardGame.Core.CardUseEffectSource? WoxinSource(int owner, global::System.Collections.Generic.IReadOnlySet<string>? visible = default) => _resolve().WoxinSource(owner, visible);
    internal bool HasWoxinPolicy { get => _resolve().HasWoxinPolicy; }
}
