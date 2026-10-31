#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingMouEntries
{
    internal void IssueMouGuanPingUse(global::CardGame.Core.CardUseDeclaredEvent declared) => _resolve().IssueMouGuanPingUse(declared);
    internal bool MouGuanPingActualActivationQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MouGuanPingActualActivationQuota(q);
    internal int MouGuanPingDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MouGuanPingDamage(attack);
    internal bool MouGuanPingIssuedDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().MouGuanPingIssuedDistance(q);
    internal bool MouGuanPingPaidSuppression(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().MouGuanPingPaidSuppression(f, seat);
    internal bool MouGuanPingSingleCardAlias(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().MouGuanPingSingleCardAlias(q);
    internal bool MouGuanPingWholeColorBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().MouGuanPingWholeColorBan(q);
    internal global::CardGame.Core.MouGuanPingRuntime MouGuanPing { get => _resolve().MouGuanPing; }
}
