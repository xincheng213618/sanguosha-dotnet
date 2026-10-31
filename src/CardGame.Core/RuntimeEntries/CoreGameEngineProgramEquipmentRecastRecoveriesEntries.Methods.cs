#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEquipmentRecastRecoveriesEntries
{
    internal bool ResumeProgramEquipmentRecast(long frameId) => _resolve().ResumeProgramEquipmentRecast(frameId);
    internal bool TryPauseProgramEquipmentRecast(global::CardGame.Core.ProgramSkillFrame paidFrame, global::CardGame.Core.Card card) => _resolve().TryPauseProgramEquipmentRecast(paidFrame, card);
}
