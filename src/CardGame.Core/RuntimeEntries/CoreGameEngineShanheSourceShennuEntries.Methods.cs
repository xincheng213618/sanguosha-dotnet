#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceShennuEntries
{
    internal bool CanReceiveShanheSourceShennuDamage(ProgramTriggerLegalityContext context) => _resolve().CanReceiveShanheSourceShennuDamage(context);
    internal bool CaptureShanheShennuOutgoing(GameEngine.IDamageAttempt attack, long damageId) => _resolve().CaptureShanheShennuOutgoing(attack, damageId);
    internal void CompleteShanheShennuOutgoing(AfterDamageEvent completed) => _resolve().CompleteShanheShennuOutgoing(completed);
    internal void ReceiveShanheSourceShennuDamage(ProgramSkillFrame supplied) => _resolve().ReceiveShanheSourceShennuDamage(supplied);
    internal int ShanheShennuDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShanheShennuDamageBonus(attack);
    internal int ShanheShennuPublicDamageBonus(GameSnapshot view, int sourceSeat, bool sourceLess, bool chain, GameEngine.IDamageAttempt? attack) => _resolve().ShanheShennuPublicDamageBonus(view, sourceSeat, sourceLess, chain, attack);
    internal bool IsShanheSourceShennuMode { get => _resolve().IsShanheSourceShennuMode; }
}
