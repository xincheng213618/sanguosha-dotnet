#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramEquipmentRecastRecoveriesEntries
{
    internal bool ResumeProgramEquipmentRecast(long frameId) => _resolve().ResumeProgramEquipmentRecast(frameId);
    internal bool TryPauseProgramEquipmentRecast(ProgramSkillFrame paidFrame, Card card) => _resolve().TryPauseProgramEquipmentRecast(paidFrame, card);
}
