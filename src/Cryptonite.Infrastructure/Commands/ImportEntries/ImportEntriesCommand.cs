using Cryptonite.Core.Enums;
using Cryptonite.Infrastructure.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cryptonite.Infrastructure.Commands.ImportEntries
{
    public class ImportEntriesCommand : UserBasedCommand<Unit>
    {
        public ImportEntriesCommand(IFormFile file, ImportType type)
        {
            File = file;
            Type = type;
        }

        public IFormFile File { get; set; }
        public ImportType Type { get; set; }
    }
}