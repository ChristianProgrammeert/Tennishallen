using Tennishallen.Data.Base;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Services;

public class CourtService(ApplicationDbContext context)
    : BaseRepository<Court, int>(context)
{
}