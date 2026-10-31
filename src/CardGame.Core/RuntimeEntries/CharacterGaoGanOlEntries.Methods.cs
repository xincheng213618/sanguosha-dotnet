#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoGanOlEntries
{
    internal void AssertJuguanUses() => _resolve().AssertJuguanUses();
    internal global::CardGame.Core.JuguanDeadlineSettledEvent? CurrentJuguanDeadline(int owner) => _resolve().CurrentJuguanDeadline(owner);
    internal bool IsJuguanConversion(global::CardGame.Core.CardConversionSource source) => _resolve().IsJuguanConversion(source);
    internal int JuguanActualDrawAdjustment(global::CardGame.Core.CharacterState player) => _resolve().JuguanActualDrawAdjustment(player);
    internal void ObserveJuguanActualUseAndDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveJuguanActualUseAndDamage(payload);
    internal void ObserveJuguanDeadlineAndActualDraw(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveJuguanDeadlineAndActualDraw(payload);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.JuguanVictimDamagedEvent> PendingJuguanVictims(int owner) => _resolve().PendingJuguanVictims(owner);
    internal void RequireJuguanUse(global::CardGame.Core.CardUseFrame use) => _resolve().RequireJuguanUse(use);
    internal bool TracksJuguanDrawObligations { get => _resolve().TracksJuguanDrawObligations; }
}
