#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveDamageCapsEntries
{
    internal (global::CardGame.Core.CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(global::CardGame.Core.CharacterState target, int? sourceSeat, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => _resolve().FindPassiveProgramDamageCap(target, sourceSeat, visibleSkillIds);
    internal int? GetIncomingFixedDamageCap(global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => _resolve().GetIncomingFixedDamageCap(target, visibleSkillIds);
    internal (global::CardGame.Core.CardUseEffectSource Source, int Cap)? GetPassiveProgramDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GetPassiveProgramDamageCap(attack);
    internal bool HasNextRecipientPassiveDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasNextRecipientPassiveDamageCap(attack);
}
