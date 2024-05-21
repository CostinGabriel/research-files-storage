using FluentValidation;
using MediatR;
using ResearchFilesStorage.Application.Enums;
using ResearchFilesStorage.Domain.Repository;

namespace ResearchFilesStorage.Application.Commands;

public record AddSecuredResearchFileCommand(string Name, string Content, Extension Extension, string Password) : IRequest<Guid>;

public class AddSecuredResearchFileCommandHandler(
    IResearchFileRepository researchFileRepository,
    IFileBuilder fileBuilder) : IRequestHandler<AddSecuredResearchFileCommand, Guid>
{
    public async Task<Guid> Handle(AddSecuredResearchFileCommand request, CancellationToken cancellationToken)
    {
        var researchFile = fileBuilder.SetName(request.Name)
                                      .SetContent(request.Content)
                                      .SetExtension(request.Extension)
                                      .SetPassword(request.Password)
                                      .SetIsSecured(true)
                                      .Build();

        var id = await researchFileRepository.AddAsync(researchFile);

        return id;
    }
}

public sealed class AddSecuredResearchFileCommandValidator : AbstractValidator<AddSecuredResearchFileCommand>
{
    private const int CONTENT_MAX_LENGTH = 512;
    public AddSecuredResearchFileCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("File Name is required");

        RuleFor(command => command.Content)
            .NotEmpty()
            .WithMessage("Content is required")
            .MaximumLength(CONTENT_MAX_LENGTH)
            .WithMessage($"Maximum content length allowed is {CONTENT_MAX_LENGTH}");

        RuleFor(command => command.Extension)
            .NotEmpty()
            .WithMessage("File Extension is required");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}