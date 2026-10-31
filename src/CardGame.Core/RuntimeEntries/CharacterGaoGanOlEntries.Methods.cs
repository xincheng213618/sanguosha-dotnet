#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoGanOlEntries
{
    internal void AssertJuguanUses() => _resolve().AssertJuguanUses();
    internal JuguanDeadlineSettledEvent? CurrentJuguanDeadline(int owner) => _resolve().CurrentJuguanDeadline(owner);
    internal bool IsJuguanConversion(CardConversionSource source) => _resolve().IsJuguanConversion(source);
    internal int JuguanActualDrawAdjustment(CharacterState player) => _resolve().JuguanActualDrawAdjustment(player);
    internal void ObserveJuguanActualUseAndDamage(IGameEvent payload) => _resolve().ObserveJuguanActualUseAndDamage(payload);
    internal void ObserveJuguanDeadlineAndActualDraw(IGameEvent payload) => _resolve().ObserveJuguanDeadlineAndActualDraw(payload);
    internal IEnumerable<JuguanVictimDamagedEvent> PendingJuguanVictims(int owner) => _resolve().PendingJuguanVictims(owner);
    internal void RequireJuguanUse(CardUseFrame use) => _resolve().RequireJuguanUse(use);
    internal bool TracksJuguanDrawObligations { get => _resolve().TracksJuguanDrawObligations; }
}
