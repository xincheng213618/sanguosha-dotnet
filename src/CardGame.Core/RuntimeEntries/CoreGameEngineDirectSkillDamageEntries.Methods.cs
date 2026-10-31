#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDirectSkillDamageEntries
{
    internal bool IsDirectSkillDamage(GameEngine.IDamageAttempt attack) => _resolve().IsDirectSkillDamage(attack);
}
