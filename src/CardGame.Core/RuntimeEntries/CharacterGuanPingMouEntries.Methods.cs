#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingMouEntries
{
    internal void IssueMouGuanPingUse(CardUseDeclaredEvent declared) => _resolve().IssueMouGuanPingUse(declared);
    internal bool MouGuanPingActualActivationQuota(ProgramSlashQuotaQuery q) => _resolve().MouGuanPingActualActivationQuota(q);
    internal int MouGuanPingDamage(GameEngine.IDamageAttempt attack) => _resolve().MouGuanPingDamage(attack);
    internal bool MouGuanPingIssuedDistance(ProgramSlashDistanceQuery q) => _resolve().MouGuanPingIssuedDistance(q);
    internal bool MouGuanPingPaidSuppression(ProgramSkillFrame f, int seat) => _resolve().MouGuanPingPaidSuppression(f, seat);
    internal bool MouGuanPingSingleCardAlias(ProgramConfiguredViewAsQuery q) => _resolve().MouGuanPingSingleCardAlias(q);
    internal bool MouGuanPingWholeColorBan(ProgramViewAsLegalityQuery q) => _resolve().MouGuanPingWholeColorBan(q);
    internal MouGuanPingRuntime MouGuanPing { get => _resolve().MouGuanPing; }
}
