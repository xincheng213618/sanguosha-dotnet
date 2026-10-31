#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveDamageCapsEntries
{
    internal (CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(CharacterState target, int? sourceSeat, IReadOnlySet<string>? visibleSkillIds = default) => _resolve().FindPassiveProgramDamageCap(target, sourceSeat, visibleSkillIds);
    internal int? GetIncomingFixedDamageCap(CharacterState target, IReadOnlySet<string>? visibleSkillIds = default) => _resolve().GetIncomingFixedDamageCap(target, visibleSkillIds);
    internal (CardUseEffectSource Source, int Cap)? GetPassiveProgramDamageCap(GameEngine.IDamageAttempt attack) => _resolve().GetPassiveProgramDamageCap(attack);
    internal bool HasNextRecipientPassiveDamageCap(GameEngine.IDamageAttempt attack) => _resolve().HasNextRecipientPassiveDamageCap(attack);
}
