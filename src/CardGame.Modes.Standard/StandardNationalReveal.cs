using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    // Keep forbidden-mode rejection and prompt-validation order compatible while
    // the entire command decision belongs to the installed setup implementation.
    public IReadOnlyList<Type> CommandTypes { get; } = Array.AsReadOnly<Type>([typeof(RevealGeneralCommand)]);
    private IModeNationalRevealHost? _nationalRevealHost;
    private IModeNationalRevealHost NationalRevealHost => _nationalRevealHost ??= _revealHostFactory();

    public CommandResult Submit(GameCommand command) => command is RevealGeneralCommand reveal
        ? SubmitReveal(reveal) : throw new InvalidOperationException("An unregistered command reached the standard mode feature.");

    private CommandResult SubmitReveal(RevealGeneralCommand command)
    {
        var host = NationalRevealHost;
        var pendingKind = host.PendingKind ?? DecisionKind.PlayCard;
        var validation = host.ValidateHumanPrompt(command.ActorSeat, pendingKind, command.PromptId, CommandErrorCode.IllegalAction);
        if (validation is not null) return host.Reject(validation.Code, validation.Message);
        if (pendingKind is not (DecisionKind.PlayCard or DecisionKind.RespondDodge or DecisionKind.RespondSlash))
            return host.Reject(CommandErrorCode.IllegalAction, "The engine is not waiting for a revealable national-war action.");
        if (!Rules.Visibility.AllowsGeneralReveal)
            return host.Reject(CommandErrorCode.IllegalAction, "General reveal is only available in the national-war lite mode.");
        if (!Enum.IsDefined(command.Slot))
            return host.Reject(CommandErrorCode.IllegalAction, "The general reveal slot is invalid.");
        if (!host.LegalRevealSlots(command.ActorSeat).Contains(command.Slot))
            return host.Reject(CommandErrorCode.IllegalAction, "The selected general is already public or cannot be revealed now.");
        return host.Accept(() => host.RevealHumanGeneral(command.Slot, _options.AdvanceAfterHumanCommands));
    }
}
