#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheLeizhenEntries
{
    internal void ApplyShanheLeizhenNativeBase(CardUsedEvent fact) => _resolve().ApplyShanheLeizhenNativeBase(fact);
    internal void IssueShanheLeizhenSlash(CardUseDeclaredEvent declared) => _resolve().IssueShanheLeizhenSlash(declared);
    internal ShanheLeizhenChargeEarnedEvent? PendingShanheLeizhenCharge(int owner) => _resolve().PendingShanheLeizhenCharge(owner);
    internal void RecordShanheLeizhenCommittedUse(CardActionContext action) => _resolve().RecordShanheLeizhenCommittedUse(action);
    internal void ResolveShanheLeizhenUse(ProgramSkillFrame f) => _resolve().ResolveShanheLeizhenUse(f);
    internal int ShanheLeizhenPublicActionBonus(int actorSeat, CardKind? effectiveKind) => _resolve().ShanheLeizhenPublicActionBonus(actorSeat, effectiveKind);
    internal bool HasShanheLeizhenCapability { get => _resolve().HasShanheLeizhenCapability; }
}
