#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCapturedEquipmentDrawsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCapturedEquipmentAndDraw(global::CardGame.Core.ProgramSkillFrame f, int recipient, string bind, int requested) => _resolve().BeginCapturedEquipmentAndDraw(f, recipient, bind, requested);
    internal bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId) => _resolve().CanPlaceCapturedEquipmentActivation(owner, recipient, id, skillId);
    internal bool CanPlaceCapturedEquipmentCost(int owner, int recipient, int id) => _resolve().CanPlaceCapturedEquipmentCost(owner, recipient, id);
    internal bool ResumeCapturedEquipmentAndDraw(long id) => _resolve().ResumeCapturedEquipmentAndDraw(id);
}
