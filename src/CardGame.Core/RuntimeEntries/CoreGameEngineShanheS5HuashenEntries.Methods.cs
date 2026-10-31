#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HuashenEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseOrExchangeShanheS5Avatar(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseOrExchangeShanheS5Avatar(effect, frame);
    internal void FinishShanheS5AvatarChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishShanheS5AvatarChoice(frame);
    internal void PublishShanheS5AvatarChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShanheS5AvatarChoice(frame);
    internal void ResolveShanheS5AvatarChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveShanheS5AvatarChoice(choice);
    internal bool ResumeShanheS5AvatarChoice(long id) => _resolve().ResumeShanheS5AvatarChoice(id);
    internal global::CardGame.Core.PrivateGeneralLibrary? ShanheS5AvatarChoiceLibrary(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheS5AvatarChoiceLibrary(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheS5AvatarChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheS5AvatarChoices(frame);
}
