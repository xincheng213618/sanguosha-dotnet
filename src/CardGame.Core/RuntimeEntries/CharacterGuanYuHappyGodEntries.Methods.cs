#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuHappyGodEntries
{
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> HappyGuanYuHeartSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HappyGuanYuHeartSlashDamage(attack);
    internal bool IsHappyGuanYuDyingExitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, int victim) => _resolve().IsHappyGuanYuDyingExitCandidate(c, victim);
    internal bool IsHappyGuanYuDyingExitContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsHappyGuanYuDyingExitContext(c, context);
    internal bool IsHappyGuanYuDyingExitReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsHappyGuanYuDyingExitReturn(f);
    internal bool HasHappyGuanYuHeartSlashCapability { get => _resolve().HasHappyGuanYuHeartSlashCapability; }
}
