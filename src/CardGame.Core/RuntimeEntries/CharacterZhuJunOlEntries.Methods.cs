#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuJunOlEntries
{
    internal bool AdvanceZhuJunSubtree() => _resolve().AdvanceZhuJunSubtree();
    internal SkillProgramStepOutcome BeginZhuJunCuipo(ProgramSkillFrame input) => _resolve().BeginZhuJunCuipo(input);
    internal bool CanOfferZhuJun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhuJun(candidate, trigger, context);
    internal bool ContinuePaidZhuJun(ProgramSkillFrame f) => _resolve().ContinuePaidZhuJun(f);
    internal void ContinueZhuJunCuipo(long id) => _resolve().ContinueZhuJunCuipo(id);
    internal bool IsZhuJunDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsZhuJunDrawMovement(f, m);
    internal void ObserveZhuJunActualUse(IGameEvent payload) => _resolve().ObserveZhuJunActualUse(payload);
    internal bool ResumeZhuJunCuipo(long id) => _resolve().ResumeZhuJunCuipo(id);
    internal bool ReturnZhuJunMovement(ProgramSkillFrame f) => _resolve().ReturnZhuJunMovement(f);
    internal bool TryBeginZhuJunLegacyAlcoholUse(long useId, long programParentId, long dyingId) => _resolve().TryBeginZhuJunLegacyAlcoholUse(useId, programParentId, dyingId);
    internal bool TryReturnZhuJunLegacyAlcohol(ProgramCardTriggerWindowFrame frame) => _resolve().TryReturnZhuJunLegacyAlcohol(frame);
    internal int ZhuJunCuipoDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ZhuJunCuipoDamageBonus(attack);
    internal bool ZhuJunFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhuJunFirstChild(f, child);
    internal ProgramSkillFrame? ZhuJunObserverRoot() => _resolve().ZhuJunObserverRoot();
    internal ZhuJunCuipoQualifiedEvent? ZhuJunQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ZhuJunQualification(candidate, context);
    internal bool HasZhuJunCapability { get => _resolve().HasZhuJunCapability; }
}
