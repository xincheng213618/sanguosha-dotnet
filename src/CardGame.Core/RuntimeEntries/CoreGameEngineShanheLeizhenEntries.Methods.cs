#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheLeizhenEntries
{
    internal void ApplyShanheLeizhenNativeBase(global::CardGame.Core.CardUsedEvent fact) => _resolve().ApplyShanheLeizhenNativeBase(fact);
    internal void AssertShanheLeizhenUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertShanheLeizhenUse(use);
    internal void IssueShanheLeizhenSlash(global::CardGame.Core.CardUseDeclaredEvent declared) => _resolve().IssueShanheLeizhenSlash(declared);
    internal global::CardGame.Core.ShanheLeizhenChargeEarnedEvent? PendingShanheLeizhenCharge(int owner) => _resolve().PendingShanheLeizhenCharge(owner);
    internal void RecordShanheLeizhenCommittedUse(global::CardGame.Core.CardActionContext action) => _resolve().RecordShanheLeizhenCommittedUse(action);
    internal void ResolveShanheLeizhenUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveShanheLeizhenUse(f);
    internal int ShanheLeizhenPublicActionBonus(int actorSeat, global::CardGame.Core.CardKind? effectiveKind) => _resolve().ShanheLeizhenPublicActionBonus(actorSeat, effectiveKind);
    internal bool HasShanheLeizhenCapability { get => _resolve().HasShanheLeizhenCapability; }
}
